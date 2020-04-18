Imports System.Data.SqlClient
Imports sys_ui


Public Class SecSwap
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private cnSQLx As SqlConnection
    Private strSQLx As String
    Private cmSQLx As SqlCommand
    Private drSQLx As SqlDataReader
    Private object_userlog As New usrlog.usrlog
    Private usrdet As New usrlog.usrlog
    Private maturity As String
    Private LoanDeal As Boolean 'is it a loan deal or a deposit deal
    'Private colDesc As String
    Private SavDls As New csvptt.csvptt
    Private selectedsec As String
    Private checkref As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblError.Text = ""
        If Not IsPostBack Then
            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then


                lblRef.Text = Request.QueryString("dealref")
                Call LoadDealInfo(lblRef.Text)
                Call LoadSecurity()
                lblTotalCollateral.Text = Format(GetLoanCollateral(Trim(lblRef.Text), "DEAL"), "###,###.00")
                Call GetCollateralItems()
                'Call LoadDeals(lblRef.Text)
                'Call LoadDealSecurities()
                Call LoadSecuredDeposit(lblRef.Text)
                txtSecured.Text = lblRef.Text
                Call load_TBs()
            Else
                lblError.Text = alert("Select a deal that you want to change tax.", "Incomplete informaton")


            End If
        End If
    End Sub
    Private Sub LoadSecurity()
        Try
            If LoanDeal = True Then
                strSQL = "select COLL_COLLATERAL_TYPES.CollateralDescription as Ctype, * from counterpartySecurity join COLLATERAL_ITEMS on counterpartySecurity.tb_id=COLLATERAL_ITEMS.collateralreference" & _
                " join COLL_COLLATERAL_TYPES on COLLATERAL_ITEMS.collateraltype=COLL_COLLATERAL_TYPES.collateralID " & _
                "where Dealreference = '" & Trim(lblRef.Text) & "' and status in ('Y','E')"
            Else
                strSQL = "select COLL_COLLATERAL_TYPES.CollateralDescription as Ctype, * from attachedsecurities join COLLATERAL_ITEMS on attachedsecurities.tb_id=COLLATERAL_ITEMS.collateralreference" & _
                             " join COLL_COLLATERAL_TYPES on COLLATERAL_ITEMS.collateraltype=COLL_COLLATERAL_TYPES.collateralID " & _
                             "where Dealreferencedeal = '" & Trim(lblRef.Text) & "' and matured in ('N','E')"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Grdsec2.DataSource = drSQL
            Grdsec2.DataBind()


            Do While drSQL.Read
                '    Dim ltm As New ListViewItem(Trim(drSQL.Item(0).ToString))

                If LoanDeal = True Then

                    'ltm.SubItems.Add(Trim(drSQL.Item(27).ToString))
                    'ltm.SubItems.Add(Format(CDbl((drSQL.Item("amount").ToString)), "###,###.00"))

                    If drSQL.Item("status").ToString = "E" Then
                        Session("expiredSec") = True
                    Else
                        Session("expiredSec") = False
                    End If
                Else
                    'ltm.SubItems.Add(Trim(drSQL.Item(27).ToString))
                    'ltm.SubItems.Add(Format(CDbl((drSQL.Item("securityamount").ToString)), "###,###.00"))

                    If drSQL.Item("matured").ToString = "E" Then
                        Session("expiredSec") = True
                    Else
                        Session("expiredSec") = False
                    End If

                End If



            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub
    Private Sub GetCollateralItems()
        Dim cnSQL2 As SqlConnection
        Dim cmSQL2 As SqlCommand
        Dim drSQL2 As SqlDataReader
        Dim strSQL2 As String
        Try
            If LoanDeal = True Then
                strSQL2 = "select COLL_COLLATERAL_TYPES.CollateralDescription as Ctype, * from COLLATERAL_ITEMS  join COLL_COLLATERAL_TYPES on COLLATERAL_ITEMS.collateraltype=COLL_COLLATERAL_TYPES.collateralID" & _
                   " where customernumber='" & Trim(lblcustNumber.Text) & "' and expired='N' and collateralCancelled='N' and collateralApproved='Y'"
            Else
                strSQL2 = "select COLL_COLLATERAL_TYPES.CollateralDescription as Ctype,* from COLLATERAL_ITEMS  join COLL_COLLATERAL_TYPES on COLLATERAL_ITEMS.collateraltype=COLL_COLLATERAL_TYPES.collateralID" & _
               " where  expired='N' and collateralCancelled='N' and collateralApproved='Y' and securedeposit='Y'"
            End If

            cnSQL2 = New SqlConnection(Session("ConnectionString"))
            cnSQL2.Open()
            cmSQL2 = New SqlCommand(strSQL2, cnSQL2)
            drSQL2 = cmSQL2.ExecuteReader()

            GrdSec1.DataSource = drSQL2
            GrdSec1.DataBind()

            Do While drSQL2.Read
                'Dim Itmx As New ListViewItem(drSQL2.Item("collateralreference").ToString)
                'Itmx.SubItems.Add(Trim(drSQL2.Item(22).ToString))
                'Itmx.SubItems.Add(Trim(drSQL2.Item(4).ToString))
                'Itmx.ImageIndex = 0
                'ListCollateralList.Items.Add(Itmx)
            Loop
            ' Close and Clean up objects
            drSQL2.Close()
            cnSQL2.Close()
            cmSQL2.Dispose()
            cnSQL2.Dispose()

        Catch ex As SqlException

            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "GetCustomerCollateral" & ex.Message, Session("serverName"), Session("client"))
        End Try

    End Sub
    Private Function GetLoanCollateral(ByVal ref As String, ByVal Deal_Collateral As String) As Decimal
        Dim cnSQL2 As SqlConnection
        Dim cmSQL2 As SqlCommand
        Dim drSQL2 As SqlDataReader
        Dim strSQL2 As String

        Dim res As Decimal = 0

        Try

            If LoanDeal = True Then 'Loan deals
                Select Case Deal_Collateral
                    Case "DEAL"
                        strSQL2 = "select sum(amount) from COUNTERPARTYSECURITY where DEALREFERENCE='" & ref & "' and status in('Y')"
                    Case "COLLATERAL"
                        strSQL2 = "select sum(amount) from COUNTERPARTYSECURITY where tb_id='" & ref & "' and status in('Y')"
                End Select
            Else
                Select Case Deal_Collateral
                    Case "DEAL"
                        strSQL2 = "select sum(securityamount) from attachedsecurities where DEALREFERENCEdeal='" & Trim(ref) & "' and matured in('N')"
                    Case "COLLATERAL"
                        'strSQL2 = "select sum(securityamount) from attachedsecurities where DEALREFERENCEdeal='" & Trim(ref) & "' and matured in('N')"
                        strSQL2 = "select sum(securityamount) from attachedsecurities where tb_id='" & Trim(ref) & "' and matured in('N')"
                End Select
            End If



            cnSQL2 = New SqlConnection(Session("ConnectionString"))
            cnSQL2.Open()
            cmSQL2 = New SqlCommand(strSQL2, cnSQL2)
            drSQL2 = cmSQL2.ExecuteReader()

            Do While drSQL2.Read
                If drSQL2.Item(0) Is DBNull.Value Then
                    res = 0
                Else
                    res = CDbl(drSQL2.Item(0))
                End If
            Loop
            ' Close and Clean up objects
            drSQL2.Close()
            cnSQL2.Close()
            cmSQL2.Dispose()
            cnSQL2.Dispose()


        Catch ex As SqlException
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "GetCustomerCollateral" & ex.Message, Session("serverName"), Session("client"))
        End Try

        Return res

    End Function
    Public Function LoadDealInfo(ByVal ref As String) As String

        Try
            'strSQLx = "select * from deals_live where dealreference = '" & ref & "'"
            strSQLx = "select * from DEALS_LIVE  join CUSTOMER on DEALS_LIVE.CustomerNumber  =CUSTOMER.Customer_Number where DealReference='" & ref & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader

            If drSQLx.HasRows = True Then
                Do While drSQLx.Read

                    'PortfolioID = Trim(drSQLx.Item("PortfolioID").ToString)
                    lblcustNumber.Text = Trim(drSQLx.Item("customernumber").ToString)
                    lblCustName.Text = Trim(drSQLx.Item("FullName").ToString)
                    lblDealValue.Text = Trim(drSQLx.Item("dealamount").ToString)
                Loop

            Else
                MsgBox("Cant find deal details", MsgBoxStyle.Critical, "No Details in Database")

            End If

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()




        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function
    Protected Sub GrdSec1_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call

        If e.CommandName = "select" Then
            Dim s As String = String.Empty

            lblsecref.Text = e.Item.Cells(1).Value.ToString()
            txtcolDesc.Text = e.Item.Cells(3).Value.ToString()
            txtsectypeD.Text = e.Item.Cells(2).Value.ToString()
            Call getSec()
            'Call DeleteSecurityTemp(lblsecref.Text)
        End If
    End Sub

    Private Sub getSec()

        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String

        lblCollCurrency.Text = ""
        lblValue.Text = ""
        lblExpiry.Text = ""
        lblCollAvailable.Text = ""


        Try
            'validate username first
            strSQL1 = "select * from COLLATERAL_ITEMS where collateralReference='" & Trim(lblsecref.Text) & "' and collateralapproved='Y'" & _
            " and expired='N' and collateralCancelled='N'"

            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()



            Do While drSQL1.Read
                lblCollCurrency.Text = (drSQL1.Item("collateralCurrency").ToString)
                lblValue.Text = Format(CDbl(drSQL1.Item("collateralBankValue").ToString), "###,###.00")
                lblExpiry.Text = (CDate(drSQL1.Item("collateralExpiry").ToString))
                lblCollAvailable.Text = Format((CDbl(drSQL1.Item("collateralBankValue").ToString) - GetLoanCollateral(Trim(lblsecref.Text), "COLLATERAL")), _
                "###,###.00")

                If Trim(drSQL1.Item("assignment").ToString) = "Full" Then
                    txtCollateralLoan.Text = lblCollAvailable.Text
                    txtCollateralLoan.Enabled = False
                Else
                    txtCollateralLoan.Text = ""
                    txtCollateralLoan.Enabled = True
                End If

            Loop

            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()


        Catch ex As SqlException
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "GetCustomerCollateral" & ex.Message, Session("serverName"), Session("client"))

        End Try
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If CDbl(txtCollateralLoan.Text) > CDbl(lblCollAvailable.Text) Then
            'lblModalError.Text = "hdh"
            lblError.Text = alert("Security amount cannot be greater that amount available.", "Error")

            ''MsgBox("Security amount cannot be greater that amount available.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        'Check if the amount is zero or less that zero
        If CDec(txtCollateralLoan.Text) <= 0 Then
            lblError.Text = alert("Security amount entered is not valid.", "Security")

            'MsgBox("Security amount entered is not valid.", MsgBoxStyle.Critical, "Security")
            Exit Sub
        End If
        'Check if the security has not been added to the list already
        Dim selectedCollateral As String
        For Each item As Global.EO.Web.GridItem In Grdsec2.Items
            selectedCollateral = item.Cells(1).Value
            If Trim(selectedCollateral) = Trim(lblsecref.Text) Then
                'messagebox required  Security has already been added to the list. Do you want to amend the security amount with this amount?
                ' If MessageBox.Show("Security has already been added to the list. Do you want to amend the security amount with this amount?" & _
                '" .", "Security", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                '     Exit Sub
                ' Else
                '     checkref = True
                ' End If

                checkref = True
            End If
        Next



        'If LstAssignList.Items.Count > 10 Then
        '    MsgBox("You have reached the maximum security items you can attach to this transaction", MsgBoxStyle.Information)
        '    Exit Sub
        'End If

        InsertSecurity(Trim(lblsecref.Text), Trim(txtcolDesc.Text), txtCollateralLoan.Text, lblExpiry.Text, Trim(Session("username")), Trim(lblcustNumber.Text))
        Call AttachSecurity(lblRef.Text)
        Dim message As String = "Security Has Been Added"""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "MsgBox('" & message & "');", True)
        Session("security") = "True"

        Call LoadDealInfo(lblRef.Text)
        Call LoadSecurity()
        lblTotalCollateral.Text = Format(GetLoanCollateral(Trim(lblRef.Text), "DEAL"), "###,###.00")
        Call GetCollateralItems()
    End Sub
    Protected Function InsertSecurity(ByVal security As String, ByVal dealref As String, ByVal amount As Double, ByVal expdate As String, ByVal dealer As String, ByVal customer As String)
        Dim cnSQL5 As SqlConnection
        Dim cmSQL5 As SqlCommand
        Dim drSQL5 As SqlDataReader
        Dim strSQL5 As String

        Try
            strSQL5 = "INSERT INTO SECURITYTEMP ([Security], [DealRef], [Amount], [ExpiryDate], [Dealer], [Customer]) VALUES ('" & security & "', '" & dealref & "', '" & amount & "', '" & expdate & "', '" & dealer & "', '" & customer & "')"
            cnSQL5 = New SqlConnection(Session("ConnectionString"))
            cnSQL5.Open()
            cmSQL5 = New SqlCommand(strSQL5, cnSQL5)
            drSQL5 = cmSQL5.ExecuteReader
            drSQL5.Close()
            cnSQL5.Close()
            cmSQL5.Dispose()
            cnSQL5.Dispose()
        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))            '************************END****************************************
        End Try

    End Function
    Protected Sub GrdSec2_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        If e.CommandName = "select" Then
            Dim s As String = String.Empty

            txtselectedsec.Text = e.Item.Cells(1).Value.ToString()
            txtamount.Text = e.Item.Cells(3).Value.ToString()
            'lblInfo.Text = txtdealref.Text

        End If


    End Sub


    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String

        'MsgBox("" + txtselectedsec.Text)
        Try
            'If MessageBox.Show("This will delete the security from the deal. Proceed?", "Security", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = _
            'Windows.Forms.DialogResult.No Then Exit Sub

            If LoanDeal = True Then
                If Session("expiredSec") = True Then
                    strSQL1 = "update COUNTERPARTYSECURITY set status='R' where tb_id='" & Trim(txtselectedsec.Text) & "' and dealreference='" & Trim(lblRef.Text) & "'"
                Else
                    strSQL1 = "delete COUNTERPARTYSECURITY where tb_id='" & Trim(txtselectedsec.Text) & "' and dealreference='" & Trim(lblRef.Text) & "'"

                End If
            Else
                If Session("expiredSec") = True Then
                    strSQL1 = "update ATTACHEDSECURITIES set Matured='R' where tb_id='" & Trim(txtselectedsec.Text) & "' and dealreferencedeal='" & Trim(lblRef.Text) & "'"
                Else
                    strSQL1 = "delete ATTACHEDSECURITIES where tb_id='" & Trim(txtselectedsec.Text) & "' and dealreferencedeal='" & Trim(lblRef.Text) & "'"

                End If
            End If



            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()

            'Log the event *****************************************************loggedUserLog formated for log
            object_userlog.SendDataToLog(Session("username") & "LON003" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Deal Ref : " & Trim(lblRef.Text) & " Security Ref : " & _
            Trim(txtselectedsec.Text) & " Amount : " & Trim(txtamount.Text), Session("serverName"), Session("client"))

            MsgBox("Security Item Removed", MsgBoxStyle.Information)

            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()



            Call LoadDealInfo(lblRef.Text)
            Call LoadSecurity()
            lblTotalCollateral.Text = Format(GetLoanCollateral(Trim(lblRef.Text), "DEAL"), "###,###.00")
            Call GetCollateralItems()

        Catch ex As SqlException
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "GetCustomerCollateral" & ex.Message, Session("serverName"), Session("client"))

        End Try


    End Sub
    Private Sub AttachSecurity(ByVal dealNum As String)

        Dim x As String = ""
        Dim cnSQLdd As SqlConnection
        Dim cmSQLdd As SqlCommand
        Dim drSQLdd As SqlDataReader
        Dim strSQLdd As String

        Try
            strSQLdd = "SELECT Security,DealRef,Amount,id FROM SECURITYTEMP  where Customer='" & Trim(lblcustNumber.Text) & "' and Dealer='" & Trim(Session("username")) & "'"
            cnSQLdd = New SqlConnection(Session("ConnectionString"))
            cnSQLdd.Open()
            cmSQLdd = New SqlCommand(strSQLdd, cnSQLdd)
            drSQLdd = cmSQLdd.ExecuteReader

            Do While drSQLdd.Read
                Call SavePlacementCollateral(dealNum, Trim(drSQLdd.Item(0)), CDec(Trim(drSQLdd.Item(2))), checkref)
                'Call SavDls.SaveDealSecurity(dealNum, Trim(drSQLdd.Item(0)), CDec(Trim(drSQLdd.Item(2))), Trim(drSQLdd.Item(1)), Session("serverName"), Session("dataBaseName"), Session("loggedUserLog"))
                DeleteSecurityTemp(Trim(drSQLdd.Item(1)))
            Loop

            ' Close and Clean up objects
            drSQLdd.Close()
            cnSQLdd.Close()
            cmSQLdd.Dispose()
            cnSQLdd.Dispose()


        Catch ec As Exception
            lblError.Text = alert(ec.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggeduserlog") & "ERR001 " & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ec.Message & " Save Deal Security", Session("serverName"), Session("client"))
            '************************END****************************************
        End Try


    End Sub
    Private Function GetTotalSecurity() As Decimal
        Dim cnSQL2 As SqlConnection
        Dim cmSQL2 As SqlCommand
        Dim drSQL2 As SqlDataReader
        Dim strSQL2 As String
        Dim res As Decimal = 0

        Try

            strSQL2 = "select sum(amount) FROM SECURITYTEMP  where Customer='" & Trim(lblcustNumber.Text) & "' and Dealer='" & Trim(Session("username")) & "'"

            cnSQL2 = New SqlConnection(Session("ConnectionString"))
            cnSQL2.Open()
            cmSQL2 = New SqlCommand(strSQL2, cnSQL2)
            drSQL2 = cmSQL2.ExecuteReader()

            Do While drSQL2.Read
                If drSQL2.Item(0) Is DBNull.Value Then
                    res = 0
                Else
                    res = CDbl(drSQL2.Item(0))
                End If
            Loop
            ' Close and Clean up objects
            drSQL2.Close()
            cnSQL2.Close()
            cmSQL2.Dispose()
            cnSQL2.Dispose()

        Catch ex As SqlException
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "GetCustomerTotalSecurity" & ex.Message, Session("serverName"), Session("client"))
        End Try

        Return res

    End Function
    Public Sub DeleteSecurityTemp(ByVal id As String)
        Dim cnSQL5 As SqlConnection
        Dim cmSQL5 As SqlCommand
        Dim drSQL5 As SqlDataReader
        Dim strSQL5 As String

        Try
            strSQL5 = "delete SECURITYTEMP where dealref='" & Trim(id) & "'"


            cnSQL5 = New SqlConnection(Session("ConnectionString"))
            cnSQL5.Open()
            cmSQL5 = New SqlCommand(strSQL5, cnSQL5)
            drSQL5 = cmSQL5.ExecuteReader


            drSQL5.Close()
            cnSQL5.Close()
            cmSQL5.Dispose()
            cnSQL5.Dispose()

        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "Error")

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))            '************************END****************************************
        End Try
    End Sub



    'Protected Sub cmbSecured_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSecured.SelectedIndexChanged
    '    Call LoadSecuredDeposit()
    'End Sub
    Private Sub LoadSecuredDeposit(ByVal ref As String)

        If Trim(ref) = "" Then     'There are no secured deals
            'Do nothing
        Else
            Try

                strSQL = "select TB_ID,securityamount,dealreferencesecurity,matured from attachedsecurities where dealreferencedeal ='" & _
                Trim(ref) & "' and  matured = 'N'"

                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()
                GrdSecured.DataSource = drSQL
                GrdSecured.DataBind()


                Do While drSQL.Read
                    'Dim itmx As New ListViewItem(Trim(drSQL.Item("TB_ID").ToString))
                    'itmx.SubItems.Add(Trim(drSQL.Item("dealreferencesecurity").ToString))
                    'itmx.SubItems.Add(Format(CDbl(drSQL.Item("securityamount").ToString), "###,###.00"))
                    If drSQL.Item("matured").ToString = "E" Then
                        Session("expiredSec2") = True
                    Else
                        Session("expiredSec2") = False
                    End If


                Loop
                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()

                strSQL = "select dealamount,maturitydate,currency from alldealsall where dealreference ='" & Trim(ref) & "'"
                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()
                Do While drSQL.Read
                    lblcurr.Text = Trim(drSQL.Item(2).ToString)
                Loop
                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()

                'load available securities

                Call load_TBs()


            Catch ex As NullReferenceException

            Catch ex As SqlException
                MsgBox(ex.Message, MsgBoxStyle.Critical)

                'Log the event *****************************************************
                object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************

            End Try

        End If
    End Sub


    Protected Sub cmbUnsecured_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUnsecured.SelectedIndexChanged
        'Get details about the TB
        If cmbUnsecured.SelectedValue.ToString <> "" Then
            Try
                'validate username first
                'strSQL = "select * from securitypurchase join deals_live on securitypurchase.tb_id=deals_live.TB_ID  where securitypurchase.tb_id ='" & _
                'lstTbs.FocusedItem.Text & "' and deals_live.TB_ID ='" & lstTbs.FocusedItem.Text & "' and deals_live.othercharacteristics='Discount Purchase' "


                'validate username first
                strSQL = "select securitypurchase.maturitydate , * from securitypurchase join deals_live on securitypurchase.tb_id=deals_live.TB_ID  " & _
                                    " where securitypurchase.tb_id ='" & cmbUnsecured.SelectedValue.ToString & "' and deals_live.TB_ID ='" & cmbUnsecured.SelectedValue.ToString & "'" & _
                                    " and securitypurchase.dealreference='" & getRef(cmbUnsecured.SelectedValue.ToString) & "' and" & _
                                    " deals_live.dealreference='" & getRef(cmbUnsecured.SelectedValue.ToString) & "' and deals_live.othercharacteristics='Discount Purchase' "

                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()
                Do While drSQL.Read
                    PurValue.Text = drSQL.Item(3).ToString
                    IntRate.Text = drSQL.Item(4).ToString
                    AvalableForSale.Text = CDec(drSQL.Item(13).ToString) - SecurityAttached(Trim(cmbUnsecured.SelectedValue.ToString()), Trim(getRef(cmbUnsecured.SelectedValue.ToString)))
                    'DaysMaturity.Text = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(drSQL.Item("mat")))
                    'MsgBox(Trim(drSQL.Item("mat").ToString))
                    'maturitySecurity.Text = Format(Trim(drSQL.Item("maturitydate").ToString), "Short Date")
                Loop

                'Assign selectedTB to the highlighted TB
                'selectedTB = lstTbs.FocusedItem.Text
                'GroupBox5.Text = "Security Details  ->  " & Trim(selectedTB)

                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()

            Catch ex As SqlException
                'MsgBox(ex.Message, MsgBoxStyle.Critical)
                lblError.Text = alert(ex.Message, "Error")
                'Log the event *****************************************************
                object_userlog.SendDataToLog(Session("loggedUserLog") & "ListTbs_Click : AttachSecurity" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************

            End Try
        End If
    End Sub

    Private Sub load_TBs()
        Try
            'validate username first
            strSQL = "select securitypurchase.tb_id,securitypurchase.dealreference from securitypurchase join deals_live on securitypurchase.dealreference=deals_live.dealreference" & _
                     "  where matured = 'N' and authorisationstatus='A' "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                'cmbUnsecured.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(1).ToString)))
                cmbUnsecured.Items.Add(Trim(drSQL.Item(0).ToString))
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'ListTbs.Columns(0).ListView.Sorting = SortOrder.Descending
            'ListTbs.Sort()

        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "Error")
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Load_TBs : AttachSecurity " & ex.Message, Session("serverName"), Session("client"))

        End Try
    End Sub
    ''this function updates colleteral if it already exits
    'Private Function updateSecurity(ByVal tbID As String) As Boolean
    '    Try
    '        If LoanDeal = True Then 'Loan deals 
    '            If checkref = False Then
    '                strSQL = " insert into CounterpartySecurity values ('" & Trim(securityRef) & "'," & CDbl(amt) & ",'" & Trim(dealref) & "','Y','N') " & _
    '                         " update deals_live set securityrequired='Y' where dealreference= '" & Trim(dealref) & "'"
    '            ElseIf checkref = True Then
    '                strSQL = "update CounterpartySecurity set amount=" & CDbl(amt) & " where tb_id='" & Trim(securityRef) & "' and dealreference='" & Trim(dealref) & "'"
    '            End If
    '        Else 'deposit deals
    '            If checkref = False Then
    '                strSQL = " insert into attachedsecurities values ('" & Trim(securityRef) & "','" & Trim(dealref) & "'," & CDbl(amt) & ",'N','" & Trim(txtcolDesc.Text) & "') " & _
    '                     " update deals_live set securityrequired='Y' where dealreference= '" & Trim(dealref) & "'"
    '            ElseIf checkref = True Then
    '                strSQL = "update attachedsecurities set securityamount=" & CDbl(amt) & " where tb_id='" & Trim(securityRef) & "' and dealreferencedeal='" & Trim(dealref) & "'"
    '            End If
    '        End If
    '        'validate username first
    '        strSQL = "update attachedsecurities set securityamount=" & CDbl(Trim(txtCollateralLoan.Text)) & " where tb_id='" & Trim(tbID) & "' and dealreferencedeal='" & Trim(lblRef.Text) & "'"
    '        'strSQL = "update CounterpartySecurity set amount=" & CDbl(Trim(txtCollateralLoan.Text)) & " where tb_id='" & Trim(tbID) & "' and dealreference='" & Trim(dealref) & "'"
    '        cnSQL = New SqlConnection(Session("ConnectionString"))
    '        cnSQL.Open()
    '        cmSQL = New SqlCommand(strSQL, cnSQL)
    '        drSQL = cmSQL.ExecuteReader()


    '        drSQL.Close()
    '        cnSQL.Close()
    '        cmSQL.Dispose()
    '        cnSQL.Dispose()



    '    Catch ex As SqlException
    '        lblError.Text = alert(ex.Message, "Error")
    '        object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Load_TBs : AttachSecurity " & ex.Message, Session("serverName"), Session("client"))

    '    End Try
    'End Function
    Private Function SecurityAttached(tbid As String, dealref As String) As Decimal
        Dim x As String
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Try

            'validate username first
            strSQL1 = "select sum(securityamount) from attachedsecurities where tb_id = '" & tbid & "' and dealreferencesecurity='" & dealref & "' "

            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()
            Do While drSQL1.Read
                x = drSQL1.Item(0).ToString
            Loop
            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()
            If x = "" Then x = "0"
            Return CDec(x)
        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "SecurityAttached : AttachSecurity" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function
    'Private Sub LoadDeals(ByVal ref As String)

    '    Try

    '        strSQL = "select Dealreference from Deals_live where securityrequired = 'Y' "
    '        cnSQL = New SqlConnection(Session("ConnectionString"))
    '        cnSQL.Open()
    '        cmSQL = New SqlCommand(strSQL, cnSQL)
    '        drSQL = cmSQL.ExecuteReader()


    '        Do While drSQL.Read

    '            cmbSecured.Items.Add(Trim(drSQL.Item(0).ToString))
    '        Loop
    '        ' Close and Clean up objects
    '        drSQL.Close()
    '        cnSQL.Close()
    '        cmSQL.Dispose()
    '        cnSQL.Dispose()

    '    Catch ex As SqlException
    '        MsgBox(ex.Message, MsgBoxStyle.Critical)

    '        'Log the event *****************************************************
    '        object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
    '        '************************END****************************************

    '    End Try
    'End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        On Error Resume Next
        'check if amount is not greater than available security
        Dim SaleAmnt As Decimal = CDec(txtSale.Text)

        If CDec(AvalableForSale.Text) - SaleAmnt < 0 Or AvalableForSale.Text = "" Then
            MsgBox("Amount entered is greater than what is available.", MsgBoxStyle.Critical, "Sale")
            Exit Sub

        Else

            If Trim(txtSecured.Text) = "" Then
                MsgBox("Select a valid deal reference", MsgBoxStyle.Critical, "Deal reference")
                Exit Sub
            End If

            If txtSale.Text = "" Or CDec(txtSale.Text) = 0 Then
                MsgBox("please enter a security amount", MsgBoxStyle.Critical, "Security Amount")
                Exit Sub
            End If
            'If MessageBox.Show("Are you sure you want to swap the security for this deal?", "Swap", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            '    'save new security details
            Call SaveSecurity()
            'End If
        End If
    End Sub

    Private Sub SaveSecurity()
        'MsgBox(cmbUnsecured.SelectedValue.ToString)
        Try

            strSQL = "begin tran InsertSecurity " & _
            "update attachedsecurities set matured='Y' where dealreferenceDeal='" & Trim(txtSecured.Text) & "'" & _
            " Update deals_live set TB_ID= '" & (cmbUnsecured.SelectedValue.ToString) & "' where dealreference = '" & getRef(Trim(txtSecured.Text)) & "'" & _
            " Insert into Attachedsecurities values('" & cmbUnsecured.SelectedValue.ToString & "','" & Trim(txtSecured.Text) & "'," & CDec(txtSale.Text) & ",'N','" & getRef(cmbUnsecured.SelectedValue.ToString) & "')" & _
            " Commit tran Insertsecurity"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            cmSQL.ExecuteNonQuery()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "SEC001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Deal Ref: " & Trim(txtSecured.Text) & " TB ID: " & txtSecured.Text, _
             Session("serverName"), Session("client"))
            '************************END****************************************

            MsgBox("Security Swaped", MsgBoxStyle.Information, "Swap")

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub
    Private Function getRef(ByVal tb As String)

        Try


            strSQL = " select securitypurchase.dealreference, matured from securitypurchase join deals_live on securitypurchase.dealreference=deals_live.dealreference" & _
                     "  where matured = 'N' and authorisationstatus='A' and deals_live.tb_id='" & tb & "' "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()


            Do While drSQL.Read

                Return Trim(drSQL.Item(0).ToString)

            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function
    Protected Sub GrdSecured_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        If e.CommandName = "Remove" Then
            Dim s As String = String.Empty

            'txtselectedsec.Text = e.Item.Cells(1).Value.ToString()
            'txtamount.Text = e.Item.Cells(3).Value.ToString()
            lblTB.Text = e.Item.Cells(1).Value.ToString()
            lblREmeveAmt.Text = e.Item.Cells(2).Value.ToString()
            Call Remove(lblTB.Text, lblREmeveAmt.Text)
        End If


    End Sub
    Private Sub Remove(ByVal tb As String, ByVal Amt As String)
        'MsgBox("tapinda")
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Try


            If Session("expiredSec2") = True Then
                strSQL1 = "update ATTACHEDSECURITIES set Matured='R' where tb_id='" & Trim(tb) & "' and dealreference='" & Trim(txtSecured.Text) & "'"
            Else

                strSQL1 = "delete ATTACHEDSECURITIES where tb_id='" & Trim(tb) & "' and dealreferencedeal='" & _
                Trim(txtSecured.Text) & "'"
            End If

            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()

            'Log the event *****************************************************loggedUserLog formated for log
            object_userlog.SendDataToLog(Session("username") & "LON003" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Deal Ref : " & Trim(txtSecured.Text) & " Security Ref : " & _
            Trim(tb) & " Amount : " & Trim(Amt), Session("serverName"), Session("client"))

            MsgBox("Security Item Removed", MsgBoxStyle.Information)

            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()


            Call LoadSecuredDeposit(lblRef.Text)

        Catch ex As SqlException
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "GetCustomerCollateral" & ex.Message, Session("serverName"), Session("client"))

        Catch er As Exception

        End Try
    End Sub

    'Protected Sub Exit_Click(sender As Object, e As EventArgs) Handles [Exit].Click
    '    Response.Redirect("mmdealblotter.aspx")
    'End Sub
    Private Sub SavePlacementCollateral(ByVal dealref As String, ByVal securityRef As String, ByVal amt As Decimal, ByVal checkref As Boolean)
        Try
            If LoanDeal = True Then 'Loan deals 
                If checkref = False Then
                    strSQL = " insert into CounterpartySecurity values ('" & Trim(securityRef) & "'," & CDbl(amt) & ",'" & Trim(dealref) & "','Y','N') " & _
                             " update deals_live set securityrequired='Y' where dealreference= '" & Trim(dealref) & "'"
                ElseIf checkref = True Then
                    strSQL = "update CounterpartySecurity set amount=" & CDbl(amt) & " where tb_id='" & Trim(securityRef) & "' and dealreference='" & Trim(dealref) & "'"
                End If
            Else 'deposit deals
                If checkref = False Then
                    strSQL = " insert into attachedsecurities values ('" & Trim(securityRef) & "','" & Trim(dealref) & "'," & CDbl(amt) & ",'N','" & Trim(txtcolDesc.Text) & "') " & _
                         " update deals_live set securityrequired='Y' where dealreference= '" & Trim(dealref) & "'"
                ElseIf checkref = True Then
                    strSQL = "update attachedsecurities set securityamount=" & CDbl(amt) & " where tb_id='" & Trim(securityRef) & "' and dealreferencedeal='" & Trim(dealref) & "'"
                End If
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader


            If checkref = False Then
                'Log the event *****************************************************loggedUserLog formated for log
                object_userlog.SendDataToLog(Session("username") & "LON001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Deal Ref : " & dealref & " Security Ref : " & _
                Trim(securityRef) & " Amount : " & amt, Session("serverName"), Session("client"))
                MsgBox("Security added.", MsgBoxStyle.Information, "Security")
                '************************END****************************************
            ElseIf checkref = True Then
                'Log the event *****************************************************loggedUserLog formated for log
                object_userlog.SendDataToLog(Session("username") & "LON002" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Deal Ref : " & dealref & " Security Ref : " & _
                Trim(securityRef) & " Amount : " & amt, Session("serverName"), Session("client"))
                '************************END****************************************
                MsgBox("Security amount changed", MsgBoxStyle.Information, "Security")
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            Call LoadSecurity()
            lblTotalCollateral.Text = Format(GetLoanCollateral(Trim(lblRef.Text), "DEAL"), "###,###.00")

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
        End Try
    End Sub
End Class
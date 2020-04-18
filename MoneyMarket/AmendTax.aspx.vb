Imports System.Data.SqlClient
Imports sys_ui
Public Class AmendTax
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
    Private dealOPs As New mmDeal.MMdealOperations
    Private accruedToDate As Decimal
    Private Rate As Integer
    'Revaluations variables
    Private RevalAccrued As Decimal
    Private DealType As String
    Private taxRate As Decimal 'stores the tax rate
    Private usrdet As New usrlog.usrlog
    Private PrintPages As Integer
    Private TaxAction As String
    Private TranxLimitVal As String() 'Stores status of Transaction limits
    Private ccy As String
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then


                txtDealref.Text = Request.QueryString("dealref")
                'Check deal status
                If dealOPs.RetrieveDealStatus(Trim(txtDealref.Text)) = True Then

                    lblError.Text = alert("Deal Cancelled.", "Deal Status")
                    btnValidateTax.Enabled = False
                    btnSaveTax.Enabled = False
                    Exit Sub
                End If

                If dealOPs.GetDealStatusAuth(Trim(txtDealref.Text)) = "A" Then
                    If dealOPs.CheckSystemChangeParameter() = "N" Then
                        'MsgBox("Maintence of deals after an authorisation cycle is disabled." & _
                        '       " Cancel this deal to effect a new change.", MsgBoxStyle.Information, "Tax Amendment")
                        lblError.Text = alert("Maintence of deals after an authorisation cycle is disabled.Cancel this deal to effect a new change.", "Tax Amendment")
                        btnValidateTax.Enabled = False
                        btnSaveTax.Enabled = False
                        Exit Sub
                    End If
                End If

                Dim DealType As String = ""

                Try

                    strSQLx = "select othercharacteristics from deals_live where dealreference= '" & Trim(txtDealref.Text) & "'"
                    cnSQLx = New SqlConnection(Session("ConnectionString"))
                    cnSQLx.Open()
                    cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                    drSQLx = cmSQLx.ExecuteReader()

                    Do While drSQLx.Read
                        DealType = Trim(drSQLx.Item("othercharacteristics").ToString)
                    Loop

                    drSQLx.Close()
                    cnSQLx.Close()
                    cmSQLx.Dispose()
                    cnSQLx.Dispose()

                    If Trim(DealType) = "Discount Purchase" Then
                        If checkSales(Trim(txtDealref.Text)) = True Then
                            'MsgBox(" This deal cannot be modified because this Security has sales made from it.", MsgBoxStyle.OkOnly, "Warning")
                            lblwarning.Text = warning("Select a deal that you want to change tax.", "Incomplete informaton")
                            btnValidateTax.Enabled = False
                            btnSaveTax.Enabled = False
                            Exit Sub
                        End If
                    End If


                    Call LoadDealInfo(txtDealref.Text)
                Catch ex As NullReferenceException
                    lblError.Text = alert("Select a deal that you want to change tax.", "Incomplete informaton")

                Catch eb As Exception
                    MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
                End Try
                btnSaveTax.Enabled = False
                Call GetPrintPages()
                Call gettaxRatecodes()
            Else
                lblError.Text = alert("Select a deal that you want to change tax.", "Incomplete informaton")
                btnValidateTax.Enabled = False
                btnSaveTax.Enabled = False
            End If
        End If
    End Sub
   
    Private Sub GetPrintPages()
        ''Check for the file that stores the print pages
        'If File.Exists(strCurrentDirectory & "\PrintControl.ini") = False Then
        '    Dim sr As StreamWriter = New StreamWriter(strCurrentDirectory & "\PrintControl.ini", False)
        '    sr.WriteLine("5")
        '    sr.Close()
        'End If

        'Try
        '    ' Create an instance of StreamReader to read from a file.
        '    Dim sr As StreamReader = New StreamReader(strCurrentDirectory & "\PrintControl.ini")
        '    Dim line As String

        '    'read the first line only
        '    Do
        '        line = sr.ReadLine()
        '        PrintPages = Int(line)
        '        Exit Do
        '    Loop
        '    sr.Close()

        'Catch ex As Exception
        '    'Log the event *****************************************************
        '    usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
        '    '************************END****************************************

        '    MsgBox(ex.Message)

        'End Try
    End Sub
    Private Sub gettaxRatecodes()

        Try
            strSQLx = "select taxcode from rates where applicable = 'Applicable'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            Do While drSQLx.Read
                cmbTaxCode.Items.Add(drSQLx.Item(0))
            Loop
            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            'SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message)
            '************************END****************************************
        End Try
    End Sub
 

    Protected Sub cmbCurrency_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTaxCode.SelectedIndexChanged
        Call getTaxRate()
    End Sub
    Private Sub getTaxRate()
        Try
            strSQLx = "select taxrate from rates where taxcode = '" & Trim(cmbTaxCode.Text) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            Do While drSQLx.Read
                txtTaxRate.Text = drSQLx.Item(0)
                'txtTaxRate = CDec(drSQLx.Item(0))
            Loop
            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            'SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message)
            '************************END****************************************
        End Try
    End Sub

   
    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("mmdealblotter.aspx")
    End Sub

  
    Protected Sub RDAddTax_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RDAddTax.SelectedIndexChanged
        If RDAddTax.SelectedValue = "rAddTax" Then
            cmbTaxCode.Visible = True
            txtTaxRate.Visible = True
        Else
            cmbTaxCode.Visible = False
            txtTaxRate.Visible = False
        End If


        'If RDAddTax.SelectedValue = "rRemoveTax" Then
        '    cmbTaxCode.Visible = False
        '    txtTaxRate.Visible = False
        'Else
        '    cmbTaxCode.Visible = False
        '    txtTaxRate.Visible = False
        'End If
    End Sub

    Protected Sub btnValidateTax_Click(sender As Object, e As EventArgs) Handles btnValidateTax.Click
        Dim NetInt As Decimal
        Dim GrossInt As Decimal
        Dim maturityAmnt As Decimal
        Dim maturityAmnt1 As Decimal
        Dim dealAmnt As Decimal
        Dim accrued As Decimal
        Dim taxAmnt As Decimal
        Dim taxAmnt1 As Decimal
        Dim roll As String = ""
        Dim tax As Decimal
        Dim dealcode As String = ""
        Dim tenor As Integer

        If Trim(txtReason.Text) = "" Then
            MsgBox("Please enter a valid reason for this operation", MsgBoxStyle.Exclamation, "Deal Tax Change")
            Exit Sub
        End If



        Try

            strSQLx = "select dealAmount,MaturityAmount,GrossInterest,TaxAmount,intaccruedTodate," & _
                     " othercharacteristics,rolloverdeal,taxrate,dealtype,tenor,currency from deals_live where dealreference= '" & Trim(txtDealref.Text) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            If drSQLx.HasRows = False Then 'Deal reference does not exit
                MsgBox("The deal reference entered does not exist. Input a correct reference and try again ", MsgBoxStyle.Critical, "Deal does not exist")
                Exit Sub

            End If

            Do While drSQLx.Read
                GrossInt = CDec(drSQLx.Item(2).ToString)
                dealAmnt = CDec(drSQLx.Item(0).ToString)
                accrued = CDec(drSQLx.Item(4).ToString)
                taxAmnt1 = CDec(drSQLx.Item(3).ToString)
                maturityAmnt1 = CDec(drSQLx.Item(1).ToString)
                DealType = drSQLx.Item(5).ToString
                roll = drSQLx.Item(6).ToString
                tax = CDec(drSQLx.Item("taxrate"))
                dealcode = Trim(drSQLx.Item("dealtype").ToString)
                tenor = Int(drSQLx.Item("tenor"))
                ccy = Trim(drSQLx.Item("currency").ToString)
            Loop

            If Trim(roll) = "P" Then
                MsgBox(" This deal cannot be modified because it has pending rollover instructions.", MsgBoxStyle.OkOnly, "Warning")

                drSQLx.Close()
                cnSQLx.Close()
                cmSQLx.Dispose()
                cnSQLx.Dispose()
                Exit Sub
            End If

            'check if its a taxable product
            If IsTaxable(dealcode) = False Then
                MsgBox("Product not taxable", MsgBoxStyle.Exclamation)
                Exit Sub
            End If


            If Trim(DealType) = "Discount Purchase" Then
                If checkSales(Trim(txtDealref.Text)) = True Then
                    MsgBox(" This deal cannot be modified because this Security has sales made from it.", MsgBoxStyle.OkOnly, "Warning")

                    drSQLx.Close()
                    cnSQLx.Close()
                    cmSQLx.Dispose()
                    cnSQLx.Dispose()
                    Exit Sub
                End If
            End If
            'Add tax to a deal without tax
            If RDAddTax.SelectedValue = "rAddTax" Then
                '    If txtTaxRate.Text <> "" Then
                '        If Trim(tax) > 0 Then
                '            MsgBox(" This deal has tax. Rate: " & tax & "%. Remove tax first if you want to change the tax rate.", MsgBoxStyle.Exclamation, "Warning")

                '            drSQLx.Close()
                '            cnSQLx.Close()
                '            cmSQLx.Dispose()
                '            cnSQLx.Dispose()
                '            Exit Sub
                '        End If
                'End If
                If txtTaxRate.Text = "" Then
                    MsgBox("Select tax code.", MsgBoxStyle.Critical, "Tax rate")
                    drSQLx.Close()
                    cnSQLx.Close()
                    cmSQLx.Dispose()
                    cnSQLx.Dispose()
                    Exit Sub
                End If

                TaxAction = "Add Tax"
                taxAmnt = GrossInt * taxRate / 100
                NetInt = GrossInt - taxAmnt
                maturityAmnt = dealAmnt + NetInt
                accruedToDate = accrued * (1 - taxRate / 100)

                txtDealAmnt.Text = Format(CDec(dealAmnt), "###,###,###.00").ToString()
                txtGrossInt.Text = Format(CDec(GrossInt), "###,###,###.00").ToString()
                txtNetInt.Text = Format(CDec(NetInt), "###,###,###.00").ToString()
                txtTaxAmnt.Text = Format(CDec(taxAmnt), "###,###,###.00").ToString()
                txtMaturityAmnt.Text = Format(CDec(maturityAmnt), "###,###,###.00").ToString()
                Rate = taxRate
                btnSaveTax.Enabled = True
                'Remove tax from deal
            ElseIf RDAddTax.SelectedValue = "rRemoveTax" Then
                If Trim(tax) = 0 Then
                    MsgBox(" This deal has no tax. Operation cannot be completed.", MsgBoxStyle.Exclamation, "Warning")

                    drSQLx.Close()
                    cnSQLx.Close()
                    cmSQLx.Dispose()
                    cnSQLx.Dispose()
                    Exit Sub
                End If

                TaxAction = "Remove tax"
                NetInt = GrossInt
                maturityAmnt = maturityAmnt1 + taxAmnt1
                accruedToDate = accrued * (1 + taxRate / 100)

                txtDealAmnt.Text = Format(CDec(dealAmnt), "###,###,###.00").ToString()
                txtGrossInt.Text = Format(CDec(GrossInt), "###,###,###.00").ToString()
                txtNetInt.Text = Format(CDec(NetInt), "###,###,###.00").ToString()
                txtMaturityAmnt.Text = Format(CDec(maturityAmnt), "###,###,###.00").ToString()
                txtTaxAmnt.Text = "0"
                Rate = 0
                btnSaveTax.Enabled = True

            Else
                'No option selected
                MsgBox("Please select an appropriate action to perform", MsgBoxStyle.OkOnly, "No action selected")
                Exit Sub
            End If

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()



            'Check Trnasaction Limits if implemented this function will return 1 to indicate that 
            'limits have been exceeded and 2 to indicate that transaction is within the limit
            '0 means Limit checking not implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount
            Dim limitsch As New usrlmt.usrlmt
            limitsch.clients = Session("client")
            limitsch.ConnectionString = Session("ConnectionString")
            TranxLimitVal = limitsch.PeriodLimitValidation(Int(tenor), getDealType(GetDealCode(Trim(txtDealref.Text))), Trim(Session("username")), _
                            CDbl(txtDealAmnt.Text), Trim(ccy), GetDealCode(Trim(txtDealref.Text)))

            If TranxLimitVal(0) = "1" Then
                MsgBox("Transaction Limit exceeded" & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       TranxLimitVal(1) & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       "Limit Amount             :   " & Format(CDbl(TranxLimitVal(2)), "###,###.00") & vbCrLf & _
                       "Transaction Amount :   " & txtDealAmnt.Text, MsgBoxStyle.Information, "Limits")
            End If


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Function checkSales(ByVal dealreference As String) As Boolean
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Dim response As Boolean
        Try
            'Save details
            strSQL1 = "Select count(dealreference) from deals_live where purchaseref='" & dealreference & "'"
            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()

            Do While drSQL1.Read
                If Int(drSQL1.Item(0).ToString) = 0 Then
                    response = False
                Else
                    response = True
                End If
            Loop


            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()

            Return response
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function
    Public Function getDealType(ByVal dealcode As String) As String
        Dim dealType As String = ""
        Dim cnSQL4 As SqlConnection
        Dim cmSQL4 As SqlCommand
        Dim drSQL4 As SqlDataReader
        Dim strSQL4 As String

        Try
            strSQL4 = "select dealbasictype from dealtypes where deal_code = '" & dealcode & "' "
            cnSQL4 = New SqlConnection(Session("ConnectionString"))
            cnSQL4.Open()
            cmSQL4 = New SqlCommand(strSQL4, cnSQL4)
            drSQL4 = cmSQL4.ExecuteReader()

            Do While drSQL4.Read
                If drSQL4.Item(0).ToString = "L" Then
                    dealType = "LoanLimit"
                Else
                    dealType = "depositLimit"
                End If
            Loop
            ' Close and Clean up objects
            drSQL4.Close()
            cnSQL4.Close()
            cmSQL4.Dispose()
            cnSQL4.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            'LogClass.SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, Session("serverName"), clients)
            '************************END****************************************
        End Try

        Return dealType

    End Function

    Public Function GetDealCode(ByVal ref As String) As String
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Dim code As String = ""
        Try
            'Save details
            strSQL1 = "Select dealtype from AllDealsAll where dealreference='" & Trim(ref) & "'"
            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()

            Do While drSQL1.Read
                code = drSQL1.Item(0).ToString
            Loop


            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()



        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

        Return code

    End Function
    Private Function IsTaxable(ByVal dealtype As String) As Boolean
        Dim x As Boolean
        Try
            strSQLx = "select taxable from DEALTYPES where deal_code = '" & dealtype & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                If Trim(drSQLx.Item(0)).Equals("Y") Then
                    x = True
                Else
                    x = False
                End If
            Loop
            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            'SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message)
            '************************END****************************************
        End Try

        Return x

    End Function

    Protected Sub btnSaveTax_Click(sender As Object, e As EventArgs) Handles btnSaveTax.Click
        Dim OrgTenor As String
        Dim OrgInterestrate As String
        Dim OrgNetInterest As String
        Dim OrgTaxAmount As String
        Dim OrgMaturityDate As String
        Dim OrgMaturityAmount As String
        Dim OrgDealAmount As String
        Dim OrgGross As String
        Dim maturitydate As Date
        Dim tenor As Integer

        Dim recn As Integer


        If Trim(txtReason.Text) = "" Then
            MsgBox("Please enter a valid reason for this operation", MsgBoxStyle.Exclamation, "Deal Tax Change")
            Exit Sub
        End If

        'Get Record number from historical modifications
        Try
            'retrieve the current info before updating
            strSQLx = "select count(dealreference) from earlymaturity where dealreference='" & Trim(txtDealref.Text) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            If drSQLx.HasRows = True Then
                Do While drSQLx.Read
                    recn = Int(drSQLx.Item(0)) + 1
                Loop
            Else
                recn = 1
            End If

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Critical)
            Exit Sub
        End Try


        Try

            strSQLx = "select maturitydate,tenor,maturityamount,interestrate,netinterest,taxamount," & _
                      " grossinterest,dealamount,maturitydate,tenor from deals_live where dealreference='" & Trim(txtDealref.Text) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                OrgTenor = drSQLx.Item(1).ToString
                OrgInterestrate = drSQLx.Item(3).ToString
                OrgNetInterest = drSQLx.Item(4).ToString
                OrgTaxAmount = drSQLx.Item(5).ToString
                OrgMaturityDate = drSQLx.Item(0).ToString
                OrgMaturityAmount = drSQLx.Item(2).ToString
                OrgGross = drSQLx.Item(6).ToString
                OrgDealAmount = drSQLx.Item(7).ToString
                maturitydate = drSQLx.Item(8).ToString
                tenor = drSQLx.Item(9)
            Loop
            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


            Dim singleQuote, Character As String
            singleQuote = "'"
            Character = """"

            txtReason.Text = Replace(Trim(txtReason.Text), singleQuote, Character)


            Dim changed As String = ""
            Dim taxCode As String = ""
            Dim taxrate As Integer

            If RDAddTax.SelectedValue = "rAddTax" Then
                changed = "Add Tax"
                taxCode = Trim(cmbTaxCode.Text)
                taxrate = Int(txtTaxRate.Text)
            Else
                changed = "Remove Tax"
                taxCode = ""
                taxrate = 0
            End If

            strSQLx = "Begin Tran Tr " & _
                    " update deals_live set authorisationstatus = 'P',useridlastupdate = '" & Trim(Session("username")) & "', " & _
                    " dateLastMaintained='" & Session("SysDate") & "',netinterest ='" & CDec(txtNetInt.Text) & "'," & _
                    " taxamount = '" & CDec(txtTaxAmnt.Text) & "',grossinterest = '" & CDbl(txtGrossInt.Text) & "'," & _
                    " maturityamount='" & CDbl(txtMaturityAmnt.Text) & "',intaccruedtodate='" & accruedToDate & "'," & _
                    " taxrate=" & taxrate & ",taxcode='" & taxCode & "' where dealreference = '" & Trim(txtDealref.Text) & "'" & _
                    "  update securitiesconfirmations set authorisationstatus = '" & RequireFrontAuthoriser() & "',useridlastupdate = '" & Trim(Session("username")) & "', " & _
                    " dateLastMaintained='" & Session("SysDate") & "',netinterest ='" & CDec(txtNetInt.Text) & "'," & _
                    " taxamount = '" & CDec(txtTaxAmnt.Text) & "',grossinterest = '" & CDbl(txtGrossInt.Text) & "'," & _
                    " maturityamount='" & CDbl(txtMaturityAmnt.Text) & "',intaccruedtodate='" & accruedToDate & "'," & _
                    " taxrate=" & taxrate & ",taxcode='" & taxCode & "' where dealreference = '" & Trim(txtDealref.Text) & "'" & _
                    "  insert into EarlyMaturity (oldMaturityDate,oldtenor,oldMaturityAmount,oldinterestrate,oldNetInterest," & _
                    " dealReference,userID,oldtaxAmount,oldgrossinterest,reason,daterevised,changed,recnumber," & _
                    " olddealamount,newdealamount,newMaturityDate,newTenor,newMaturityamount,NewNetInterest,NewGrossInterest," & _
                    " NewTaxAmount,newInterestrate,interestvaluedate)" & _
                    " values ('" & CDate(OrgMaturityDate) & "','" & Int(OrgTenor) & " ', '" & _
                      CDec(OrgMaturityAmount) & "', '" & OrgInterestrate & "','" & CDec(OrgNetInterest) & "','" & _
                       Trim(txtDealref.Text) & "','" & Trim(Session("username")) & "','" & CDec(OrgTaxAmount) & "','" & CDec(OrgGross) & "','" & Trim(txtReason.Text) & "','" & _
                       Session("SysDate") & "','" & changed & " ','" & recn & "','" & _
                       CDec(OrgDealAmount) & "','" & CDec(OrgDealAmount) & "','" & maturitydate & "','" & tenor & "','" & CDec(txtMaturityAmnt.Text) & "', '" & _
                       CDbl(txtNetInt.Text) & "','" & CDec(txtGrossInt.Text) & "', '" & CDec(txtTaxAmnt.Text) & "','" & CDec(OrgInterestrate) & "','" & CDate(Session("SysDate")) & "')" & _
                    " Commit tran Tr "

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()



            ' 1 to indicate that limits have been exceeded and 
            ' 2 to indicate that transaction is within the limit
            ' 0 means Limit checking not implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount
            'msg(3) = Limit period
            If TranxLimitVal(0) <> "0" Then
                Dim limitsch As New usrlmt.usrlmt
                limitsch.clients = Session("client")
                limitsch.ConnectionString = Session("ConnectionString")
                limitsch.SaveTranxLimitsDetails(Trim(Session("username")), Int(TranxLimitVal(3)), Trim(TranxLimitVal(1)), Trim(txtDealref.Text) _
                , CDbl(txtDealAmnt.Text), CDbl(TranxLimitVal(2)), CDate(Session("SysDate")), Int(TranxLimitVal(0)), Trim(ccy))
            End If




            'dertermine the number of amendments
            'Retrieve the last amendment so that we can set the actual days the amendment has been in effect
            Call recDaysEffectUpdate(Trim(txtDealref.Text))



            'Log this action here_______________________________________________________
            usrdet.SendDataToLog(Session("username") & "TAX001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & TaxAction & ". Changed tax status for deal " & Trim(txtDealref.Text), Session("serverName"), Session("client"))


            MsgBox("Changes applied sucessfully", MsgBoxStyle.Information, "Tax changed")

            'Select Case Trim(DealType)

            '    Case "Basic Deposit"
            '        REPORTNAME = "Deal Slip Deposit.rpt"
            '    Case "Basic Loan"
            '        REPORTNAME = "Deal Slip Loan.rpt"
            '    Case "Discount Purchase"
            '        REPORTNAME = "Deal Slip Purchase.rpt"
            '    Case "Discount Sale"
            '        REPORTNAME = "Deal Slip Sale.rpt"
            'End Select

            ''pass the deal ref parameter and the report name
            'Dim reportview As New csvptt.Reportviewer
            'reportview.db = Session("dataBaseName")
            'reportview.strCurrentD = clientlogin_vars.strCurrentDirectory
            'reportview.ReportN = "Deal Slip Deposit.rpt"
            'reportview.dealRefParam = Trim(txtDealref.Text)
            'reportview.PrintPages = PrintPages
            'reportview.Show()


            btnSaveTax.Enabled = False
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub
    Public Function RequireFrontAuthoriser() As String
        Dim res As String = ""

        Dim cnSQLX1 As SqlConnection
        Dim cmSQLX1 As SqlCommand
        Dim drSQLX1 As SqlDataReader
        Dim strSQLX1 As String

        Try

            strSQLX1 = "Select [value] from SYSTEMPARAMETERS where [parameter]='frontauth'"
            cnSQLX1 = New SqlConnection(Session("ConnectionString"))
            cnSQLX1.Open()
            cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
            drSQLX1 = cmSQLX1.ExecuteReader()

            Do While drSQLX1.Read
                If Trim(drSQLX1.Item(0).ToString) = "Y" Then
                    res = "N"
                Else
                    res = "P"
                End If
            Loop

            ' Close and Clean up objects
            drSQLX1.Close()
            cnSQLX1.Close()
            cmSQLX1.Dispose()
            cnSQLX1.Dispose()

        Catch ex As Exception

        End Try

        Return res

    End Function
    Private Sub recDaysEffectUpdate(ByVal dealNumber As String)
        Try

            Dim y As Integer

            strSQL = "select * from earlymaturity where dealreference = '" & dealNumber & "' order by recnumber desc"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                y = Int(drSQL.Item("recnumber"))
                Exit Do
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            strSQL = "update earlymaturity set DaysOnChangeActual=DaysOnChangeTemp where dealreference = '" & dealNumber & "' and recnumber=" & y - 1 & ""
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                y = Int(drSQL.Item("recnumber"))
                Exit Do
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Function LoadDealInfo(ByVal ref As String) As String

        Try
            strSQLx = "select * from deals_live where dealreference = '" & ref & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader

            If drSQLx.HasRows = True Then
                Do While drSQLx.Read
                   
                    txtDealAmnt.Text = Format(drSQLx.Item("dealamount"), "###,###,###.00")

                    txtMaturityAmnt.Text = Format(drSQLx.Item("Maturityamount"), "###,###,###.00")
                    txtNetInt.Text = Format(drSQLx.Item("netinterest"), "###,###,###.00")
                    txtGrossInt.Text = Format(drSQLx.Item("grossinterest"), "###,###,###.00")
                    txtIntRateTax.Text = drSQLx.Item("interestrate").ToString
                   
                    txtTaxRate.Text = drSQLx.Item("taxrate").ToString
                    txtTaxAmnt.Text = Format(drSQLx.Item("taxamount"), "###,###,###.00")
                   
                    txtacruedTax.Text = Format(drSQLx.Item("intaccruedtodate"), "###,###,###.00")
                   

                   

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
End Class
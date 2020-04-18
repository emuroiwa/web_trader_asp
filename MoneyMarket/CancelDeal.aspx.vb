Imports System.Data.SqlClient
Imports sys_ui
Public Class CancelDeal
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
    'Private SavDls As New csvptt.csvptt
    Private LimitsCh As New usrlmt.usrlmt
    Private DealMatured As String
    Private canCancelSecurity As Boolean
    Private TranxLimitVal As String() 'Stores status of Transaction limits
    Private ccy As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then


                lblRef.Text = Request.QueryString("dealref")
                Dim DealType As String = ""
                Dim StatusCancelled As String = "N"
                'Dim Session("dealtable") As String

                'If Trim(cmbStatus.Text) = "Matured" Then
                '    Session("dealtable") = "deals_matured"
                'Else
                '    Session("dealtable") = "deals_live"
                'End If

                Try

                    strSQLx = "select othercharacteristics,dealcancelled from " & Session("dealtable") & " where dealreference= '" & Trim(lblRef.Text) & "'"
                    cnSQLx = New SqlConnection(Session("ConnectionString"))
                    cnSQLx.Open()
                    cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                    drSQLx = cmSQLx.ExecuteReader()

                    Do While drSQLx.Read
                        DealType = Trim(drSQLx.Item("othercharacteristics").ToString)
                        StatusCancelled = Trim(drSQLx.Item("DealCancelled").ToString)
                    Loop

                    drSQLx.Close()
                    cnSQLx.Close()
                    cmSQLx.Dispose()
                    cnSQLx.Dispose()

                    If Trim(DealType) = "Discount Purchase" Then
                        If checkSales(Trim(lblRef.Text)) = True Then
                            MsgBox(" This deal cannot be modified because this Security has sales made from it.", MsgBoxStyle.OkOnly, "Warning")
                            btnSave.Enabled = False
                            Exit Sub
                        End If
                    End If

                    If Trim(StatusCancelled) = "Y" Then
                        MsgBox(" This deal has already been cancelled.", MsgBoxStyle.OkOnly, "Warning")
                        btnSave.Enabled = False
                        Exit Sub
                    End If

                    If Trim(DealType) = "Discount Sale" Then
                        MsgBox(" Cancel this deal from the securities master.", MsgBoxStyle.OkOnly, "Information")
                        btnSave.Enabled = False
                        Exit Sub
                    End If



                    LoadDealInfo(Trim(lblRef.Text), Session("dealtable"))




                Catch ex As NullReferenceException
                    MsgBox("Select a deal that you want to cancel.", MsgBoxStyle.Information, "Cancel Deal")
                    btnSave.Enabled = False
                Catch eb As Exception
                    MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
                End Try


            Else
                lblError.Text = alert("Select a deal that you want to change tax.", "Incomplete informaton")
                btnSave.Enabled = False

            End If
        End If

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
    'Load deal details
    Public Function LoadDealInfo(ByVal ref As String, ByVal dealTb As String) As String

        Try
            strSQLx = "select * from " & dealTb & " where dealreference = '" & ref & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader

            If drSQLx.HasRows = True Then
                Do While drSQLx.Read
                    lblRef.Text = drSQLx.Item("dealreference").ToString
                    lblStart.Text = Format(drSQLx.Item("startdate"), "Short Date")
                    lblMaturityDate.Text = Format(drSQLx.Item("Maturitydate"), "Short Date")
                    LblAmount.Text = Format(drSQLx.Item("dealamount"), "###,###,###.00")
                    lblMaturityAmount.Text = Format(drSQLx.Item("Maturityamount"), "###,###,###.00")
                    lblNet.Text = Format(drSQLx.Item("netinterest"), "###,###,###.00")
                    lblGross.Text = Format(drSQLx.Item("grossinterest"), "###,###,###.00")
                    lblRate.Text = drSQLx.Item("interestrate").ToString
                    lblTenor.Text = drSQLx.Item("tenor").ToString
                    lblTaxRate.Text = drSQLx.Item("taxrate").ToString
                    lblTaxAmount.Text = Format(drSQLx.Item("taxamount"), "###,###,###.00")
                    lblCaptureType.Text = drSQLx.Item("othercharacteristics").ToString
                    'txtid.Text = drSQLx.Item("tb_id").ToString
                    'txtTBID.Text = Trim(drSQLx.Item("othercharacteristics").ToString)
                    lblAccrued.Text = Format(drSQLx.Item("intaccruedtodate"), "###,###,###.00")
                    lblCurrency.Text = drSQLx.Item("currency").ToString
                Loop

            Else
                MsgBox("Cant find deal details", MsgBoxStyle.Critical, "No Details in Database")

            End If

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            If dealTb = "deals_matured" Then
                DealMatured = "Y"
            Else
                DealMatured = "N"
            End If

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim DealCapturer, DealType, portfolioid, customernum As String
        'Check for the dealing function
        'Important for distinguishing scenario pullers and actual dealers
        If Session("IsDealer") = False Then
            MsgBox("You have not been granted the dealing option", MsgBoxStyle.Critical, "Deal")
            Exit Sub
        End If

        'When a deal is cancelled set the authorised status to P and the deal cancelled status to Y

        If txtReasonCancel.Text = "" Then
            MsgBox("Please enter the reason for cancelling this deal.", MsgBoxStyle.Exclamation, "Cancel Deal")
            Exit Sub
        End If

        'if deal is a security purchase check if no sales have already been made
        If Trim(lblCaptureType.Text) = "Discount Purchase" Then
            If CanCancelSecurity = False Then
                MsgBox("Not authorised to cancel security", MsgBoxStyle.Critical, "Cancel Security")
                Exit Sub
            End If
            If CheckForSales(Trim(txtid.Text)) = True Then
                MsgBox("This deal cannot be cancelled because sales exist", MsgBoxStyle.Critical, "Cancel Purchase")
                Exit Sub
            End If
        End If
        Dim mmmmm As String = ""
        'If MessageBox.Show("Are you sure you want to cancel this deal?", "Cancel Deal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        If mmmmm = "" Then

            'Check Trnasaction Limits if implemented this function will return 1 to indicate that 
            'limits have been exceeded and 2 to indicate that transaction is within the limit
            '0 means Limit checking not implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount
            Dim limitsch As New usrlmt.usrlmt
            limitsch.clients = Session("client")
            limitsch.ConnectionString = Session("ConnectionString")
            TranxLimitVal = limitsch.PeriodLimitValidation(Int(lblTenor.Text), getDealType(GetDealCode(Trim(lblRef.Text))), Trim(Session("username")), _
                            CDbl(LblAmount.Text), Trim(lblCurrency.Text), GetDealCode(Trim(lblRef.Text)))

            If TranxLimitVal(0) = "1" Then
                MsgBox("Transaction Limit exceeded" & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       TranxLimitVal(1) & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       "Limit Amount             :   " & Format(CDbl(TranxLimitVal(2)), "###,###.00") & vbCrLf & _
                       "Transaction Amount :   " & LblAmount.Text, MsgBoxStyle.Information, "Limits")
            End If




            ' 1 to indicate that limits have been exceeded and 
            ' 2 to indicate that transaction is within the limit
            ' 0 means Limit checking not implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount
            'msg(3) = Limit period
            If TranxLimitVal(0) <> "0" Then
                limitsch.clients = Session("client")
                limitsch.ConnectionString = Session("ConnectionString")
                limitsch.SaveTranxLimitsDetails(Trim(Session("username")), Int(TranxLimitVal(3)), Trim(TranxLimitVal(1)), Trim(lblRef.Text) _
                , CDbl(LblAmount.Text), CDbl(TranxLimitVal(2)), CDate(Session("SysDate")), Int(TranxLimitVal(0)), Trim(lblCurrency.Text))
            End If


            txtReasonCancel.Text = Replace(Trim(txtReasonCancel.Text), "'", "")
            txtReasonCancel.Text = Replace(Trim(txtReasonCancel.Text), "&", "and")

            Try

                'If the deal has already matured it must be moved to the deals_live table.

                If DealMatured = "Y" Then

                    strSQLx = "begin tran Deal " & _
                          " insert into deals_live " & _
                          " select * from  deals_matured where dealreference = '" & Trim(lblRef.Text) & "'" & _
                          " delete deals_matured where dealreference = '" & Trim(lblRef.Text) & "'" & _
                          " Commit tran deals "
                    cnSQLx = New SqlConnection(Session("ConnectionString"))
                    cnSQLx.Open()
                    cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                    drSQLx = cmSQLx.ExecuteReader

                    ' Close and Clean up objects
                    drSQLx.Close()
                    cnSQLx.Close()
                    cmSQLx.Dispose()
                    cnSQLx.Dispose()
                End If



                strSQLx = " Begin tran T" & _
                        " insert  cancelDeals values('" & Trim(lblRef.Text) & "','" & txtReasonCancel.Text & "','" & Trim(Session("username")) & "')" & _
                        " update securitiesconfirmations set DealCancelled = 'Y', AuthorisationStatus='" & RequireFrontAuthoriser() & "'," & _
                        " datecancelled='" & CDate(Session("SysDate")) & "' where dealreference = '" & Trim(lblRef.Text) & "'" & _
                        " update deals_live set DealCancelled = 'Y', AuthorisationStatus='" & RequireFrontAuthoriser() & "'," & _
                        " datecancelled='" & CDate(Session("SysDate")) & "' where dealreference = '" & Trim(lblRef.Text) & "'" & _
                        " delete securitypurchase where dealreference='" & Trim(lblRef.Text) & "'" & _
                        " Commit tran T"

                cnSQLx = New SqlConnection(Session("ConnectionString"))
                cnSQLx.Open()
                cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                drSQLx = cmSQLx.ExecuteReader

                ' Close and Clean up objects
                drSQLx.Close()
                cnSQLx.Close()
                cmSQLx.Dispose()
                cnSQLx.Dispose()

                'Check If deal is secured and delete the security record
                If IsSecured(Trim(lblRef.Text)) = True Then
                    ReleaseSecurity(Trim(lblRef.Text))
                End If

                'dertermine the number of amendments
                'Retrieve the last amendment so that we can set the actual days the amendment has been in effect
                Call recDaysEffectUpdate(Trim(lblRef.Text))


                MsgBox("Deal cancelled.", MsgBoxStyle.Information)

                'Log the event *****************************************************
                usrdet.SendDataToLog(Session("username") & "DTC001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Deal Ref : " & Trim(lblRef.Text) & " Reason : " & _
                Trim(txtReasonCancel.Text), Session("serverName"), Session("client"))
                '************************END****************************************


            Catch ex As SqlException
                MsgBox(ex.Message, MsgBoxStyle.Critical)

                'Log the event *****************************************************
                usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************

            End Try

            Dim p As String
            'Retrieve the deal capturers name and the deal code
            Try
                strSQLx = "select dealcapturer,dealtype,portfolioid,customernumber,dealbasictype from deals_live" & _
                " join dealtypes on deals_live.dealtype=dealtypes.deal_code where dealreference = '" & Trim(lblRef.Text) & "'"
                cnSQLx = New SqlConnection(Session("ConnectionString"))
                cnSQLx.Open()
                cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                drSQLx = cmSQLx.ExecuteReader

                Do While drSQLx.Read
                    DealCapturer = Trim(drSQLx.Item(0).ToString)
                    DealType = Trim(drSQLx.Item(1).ToString)
                    portfolioid = Trim(drSQLx.Item(2).ToString)
                    customernum = Trim(drSQLx.Item(3).ToString)

                    'if its a deposit deal "D" then we should give it a negative sign otherwise positive sign
                    If Trim(drSQLx.Item(4).ToString) = "D" Then
                        p = "-"
                    Else
                        p = "+"
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
                usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************

            End Try

            limitsch.clients = Session("client")
            limitsch.ConnectionString = Session("ConnectionString")

            'Update all limit positions*******************************************
            limitsch.loggedUserLog = Session("username")

            If DealMatured = "N" Then

                'update dealer limit
                Call limitsch.UpdateDealerLimitCancel(DealCapturer, Trim(lblRef.Text), DealType, CDec(LblAmount.Text))
                'Update product Limit
                Call limitsch.UpdateProductPosCancel(DealType, CDec(LblAmount.Text))
                'update portfolio limit
                Call limitsch.UpdatePortPosCancel(portfolioid, CDec(LblAmount.Text), p, Trim(lblCurrency.Text))
                'update counterparty limit
                Call limitsch.UpdateCounterpartyLimitCancel(customernum, DealType, CDec(LblAmount.Text), Trim(lblCurrency.Text))

                If p = "-" Then
                    Call limitsch.UpdateAssetHoldingPos("-", CDec(LblAmount.Text), Trim(lblCurrency.Text)) 'update asset position and limits - portfolio / grouped products
                End If

            Else 'Cancelling a matured deal
                'update portfolio limit
                Dim r As String
                If p = "-" Then
                    r = "+"
                Else
                    r = "-"
                End If

                Call limitsch.UpdatePortPosCancel(portfolioid, CDec(lblNet.Text), r, Trim(lblCurrency.Text))

                If p = "+" Then 'loan deal
                    Call limitsch.UpdateAssetHoldingPos("-", CDec(lblNet.Text), Trim(lblCurrency.Text)) 'update asset position and limits - portfolio / grouped products
                End If

            End If


            'Update deal status

            recDaysEffectUpdate2(Trim(lblRef.Text))


            'print deal slip

            'Call PrintSlips(Trim(Session("username")), Trim(lblRef.Text), 1, GetDealStructure(Trim(lblRef.Text)) _
            ', Session("client"))

            '***********End********************************************************

        End If

    End Sub
    'deal is a security purchase check if no sales have already been made
    Private Function CheckForSales(ByVal TBID As String) As Boolean
        Dim x As Boolean
        Try
            strSQLx = "select * from revaluations where tb_id = '" & TBID & "' and matured = 'N'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader
            If drSQLx.HasRows = True Then x = True
            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            Return x

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function
    'Retrieve the last amendment so that we can set the actual days the amendment has been in effect
    Private Sub recDaysEffectUpdate2(ByVal dealNumber)
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

            strSQL = "update earlymaturity set DaysOnChangeActual=DaysOnChangeTemp,changed='Deal Cancelled' where dealreference = '" & dealNumber & "' and recnumber=" & y & ""
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
    'Get deal structure type loan or deposit
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
            'LogClass.SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

        Return dealType

    End Function
    Public Function IsSecured(ByVal ref As String) As Boolean
        Dim x As Boolean = False

        Try
            strSQLx = "select securityrequired from alldealsall where dealreference='" & Trim(ref) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                If Trim(drSQLx.Item(0).ToString) = "Y" Then
                    x = True
                Else
                    x = False
                End If
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            Return x

        Catch ex As Exception

        End Try
    End Function
    Private Sub ReleaseSecurity(ByVal ref As String)
        Dim x As Boolean = False

        Try
            strSQLx = "begin Tran X" & _
                    " Delete COUNTERPARTYSECURITY where dealreference='" & ref & "'" & _
                    " delete ATTACHEDSECURITIES where dealreferencedeal='" & Trim(ref) & "'" & _
                    " commit tran X"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()


            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "LON004" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Deal Cancelled Deleted Security item from deal  Deal Ref : " & _
            Trim(lblRef.Text) & " Reason : " & Trim(txtReasonCancel.Text), Session("serverName"), Session("client"))

            '************************END****************************************
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


        Catch ex As Exception

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
    'Retrieve the last amendment so that we can set the actual days the amendment has been in effect
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

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("mmdealblotter.aspx")
    End Sub
End Class
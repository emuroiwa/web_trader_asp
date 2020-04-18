Imports System.Data.SqlClient
Imports sys_ui

Public Class Authorisations
    Inherits System.Web.UI.Page
    Private object_mmFunctions As New authmm.mmFunctions
    Private object_userlog As New usrlog.usrlog
    Private IntOnline As New sysinter.sysinter
    Private sysAccounting As New accupd.accupdx
    Private ConfirmationsPrinter As New conprt.conprt
    Private SavDls As New csvptt.csvptt
    Private cnSQL As SqlConnection
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private strSQL As String
    Private strSQLX As String
    Public cnSQLX As SqlConnection
    Public cmSQLX As SqlCommand
    Public drSQLX As SqlDataReader
    'ernest
    Private SigGroup As String = "A"
    Private NumRows As Integer
    Private DepositLimit, LoanLimit As String
    Private AuthoriseAll As Boolean = False
    '  Private ConfirmationsPrinter As New conprt.conprt

    Private logDet As New usrlog.usrlog
    ' Private IntOnline As New sysinter.sysinter
    ' Private Mailer As New SocketMail
    ' Private SigGroup As String
    ' Private sysAccounting As New accupd.accupdx
    Private filez As String

    Public level As String 'Authorisation level
    Private CurrentRecSeq As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Call GetAuthorisationLevel() 'Also get the signature group
        Call LoadDeals()

        ' Call lstDeals()
        Call getConfirmationPrintingStatus()
    End Sub
    Protected Sub GrdDealsMM_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        If e.CommandName = "select" Then
        
            txtdealref.Text = e.Item.Cells(0).Value.ToString()
            'lblInfo.Text = txtdealref.Text
            ' MsgBox(txtdealref.Text)

            Call lstDeals(txtdealref.Text)
        End If
    End Sub
    Private Sub lstDeals(ByVal dealref As String)
        Try

         
            lblReference.Text = dealref
            lblDealer.Text = ""
            lblAmount.Text = ""
            lblDateEntered.Text = ""
            txtReason.Text = ""
            lblDiscountRate.Text = ""
            lblCustName.Text = ""
            lblMaturityDate.Text = ""
            lblMaturityAmount.Text = ""
            Verifier.Text = ""
            lblTenor.Text = ""
            lblRate.Text = ""
            lblCurrency.Text = ""
            lblDealStatus.Text = ""
            lblTaxRate.Text = ""
            lblNetInt.Text = ""
            lblTaxamt.Text = ""
            linkEmail.Text = ""
            lblCom.Text = ""
            lblCurrency1.Text = ""
            txtSecurityDetails.Text = ""

            cmdAuthorize.Enabled = True
            '  cmdDecline.Enabled = True


            strSQLX = "select * from " & _
                     " deals_live join dealtypes on deals_live.dealtype=dealtypes.deal_code " & _
                     " join customer on deals_live.customernumber=customer.customer_number" & _
                     " where dealreference = '" & dealref & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            Do While drSQLX.Read
                lblDealer.Text = drSQLX.Item("dealcapturer").ToString
                lblDealerName.Text = drSQLX.Item("dealcapturer").ToString
                lblRef.Text = Trim(dealref)
                ' lblDealerName.Text = Trim(lblDealer.Text)
                lblAmount.Text = Format(drSQLX.Item("dealAmount"), "###,###,###.00")
                lblMaturityAmount.Text = Format(drSQLX.Item("maturityAmount"), "###,###,###.00")
                lblDateEntered.Text = Format(drSQLX.Item("startdate"), "short date")
                lblMaturityDate.Text = Format(drSQLX.Item("maturitydate"), "short date")
                lblTenor.Text = drSQLX.Item("tenor").ToString & " " & "Days"
                txtReason.Text = ""
                dealtype.Text = drSQLX.Item("dealbasictype").ToString
                Verifier.Text = drSQLX.Item("verifiedby").ToString
                lblCustName.Text = drSQLX.Item("FullName").ToString
                lblCustNumber.Text = drSQLX.Item("CustomerNumber").ToString
                lblRate.Text = Trim(drSQLX.Item("interestrate")) & "%"
                lblDiscountRate.Text = Trim(drSQLX.Item("discountrate")) & "%"
                lblCurrency.Text = drSQLX.Item("currency")
                lblCurrency1.Text = Trim(lblCurrency.Text)
                lblCom.Text = Format(drSQLX.Item("acceptanceamount"), "###,###,###.00")
                If drSQLX.Item("taxrate") Is DBNull.Value Then
                    lblTaxRate.Text = ""
                Else
                    lblTaxRate.Text = drSQLX.Item("taxrate") & "%"
                End If

                lblNetInt.Text = Format(CDbl(drSQLX.Item("netinterest")), "###,###,###.00")
                lblTaxamt.Text = Format(drSQLX.Item("taxamount"), "###,###,###.00")

                If Trim(drSQLX.Item("entrytype").ToString) = "D" Then
                    DealAmount.Text = "Net Proceeds"
                    MaturityAmount.Text = "Face Value"
                    Netinterest.Text = "Discount Amount"
                Else
                    DealAmount.Text = "Deal Amount"
                    MaturityAmount.Text = "Maturity Amount"
                    Netinterest.Text = "Net Interest"
                End If

                If drSQLX.Item("custemail") Is DBNull.Value Then
                    linkEmail.Text = ""
                Else
                    linkEmail.Text = drSQLX.Item("custemail").ToString
                End If

            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

            'Get the reason if the deal has been cancelled

            strSQLX = "select reason from canceldeals where dealreference = '" & lblReference.Text & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            Do While drSQLX.Read
                txtReason.Text = drSQLX.Item(0).ToString
            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

            If txtReason.Text = "" Then
                txtReason.Visible = False
                lblReason.Visible = False
            Else
                txtReason.Visible = True
                lblReason.Visible = True
            End If

            'set the authorisation level value
            If level = "LEVEL1" Then
                AuthoriseAll = True
            Else
                AuthoriseAll = False
                Call GetLimits(level)
            End If


            Call GetStatus(lblReference.Text)
            Call getDealAccountDetails()
            Call getDealInstructions()
            Call getSecurityDetails()
            Call CheckDecline()
        Catch ex As NullReferenceException
            lblReference.Text = ""
            lblDealer.Text = ""
            lblAmount.Text = ""
            lblDateEntered.Text = ""
            txtReason.Text = ""
            lblDiscountRate.Text = ""
            lblCustName.Text = ""
            lblMaturityDate.Text = ""
            lblMaturityAmount.Text = ""
            Verifier.Text = ""
            lblTenor.Text = ""
            lblRate.Text = ""
            lblCustNumber.Text = ""
            cmdAuthorize.Enabled = False
            cmdAuthorize.Enabled = False

        Catch ep As Exception
            object_userlog.Msg(ep.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ep.Message, "error")

        End Try

    End Sub
    Private Sub getConfirmationPrintingStatus()
        Try
            'Get status of confirmation printing
            strSQLx = "select confimm from authconfig"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            Do While drSQLX.Read
                If drSQLX.Item(0).ToString.Equals("1") Then
                    PrintConfirmNow.Checked = True
                Else
                    PrintConfirmNow.Checked = False
                End If
            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
        End Try

    End Sub
   
    Private Sub GetStatus(ByVal ref As String)

        Dim x, y As Integer
        'dertermine the number of amendments
        Try
            strSQLX = "select changed from earlymaturity where dealreference = '" & ref & "' order by recnumber desc"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader

            Do While drSQLX.Read
                lblDealStatus.Text = Trim(drSQLX.Item("changed").ToString)
                Exit Do
            Loop
            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
        End Try

    End Sub
    Private Function GetLimits(ByVal lvl As String)
        Try
            strSQLX = "select debit, credit from authmm where authlevel = '" & lvl & "' and currency='" & Trim(lblCurrency.Text) & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()
            Do While drSQLX.Read
                DepositLimit = drSQLX.Item(1).ToString
                LoanLimit = drSQLX.Item(0).ToString
            Loop
            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()


        Catch ex As SqlException
            'lblError.Text = alert(ex.Message, "Load Deals")

            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Function
    Private Sub LoadDeals()

        Dim cnSQLO As SqlConnection
        Dim cmSQLO As SqlCommand
        Dim drSQLO As SqlDataReader
        Dim strSQLO As String

        Try
            If SigGroup = "B" And object_mmFunctions.Required2Authorisers() = True Then
                strSQL = "select * from deals_live  join customer on " & _
                          " deals_live.CustomerNumber=customer.Customer_Number where authorisationstatus = 'V' "
            ElseIf SigGroup = "A" And object_mmFunctions.Required2Authorisers() = True Then

                strSQL = "select * from deals_live  join customer on " & _
                                       " deals_live.CustomerNumber=customer.Customer_Number where authorisationstatus = 'B' "

            ElseIf object_mmFunctions.Required2Authorisers() = False Then
                strSQL = "select * from deals_live  join customer on " & _
                         " deals_live.CustomerNumber=customer.Customer_Number where authorisationstatus = 'V' "
            End If

     
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GrdDealsMM.DataSource = drSQL

            GrdDealsMM.DataBind()


            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            'lblError.Text = alert(ex.Message, "Load Deals")
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************


        End Try
    End Sub

    Private Sub GetSecurityDetailsSecurity()
        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String
        Try
            strSQLx = "select * from alldealsall where dealreference='" & txtdealref.Text & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()


            Do While drSQLx.Read
                If Trim(drSQLx.Item("othercharacteristics")) = "Discount Purchase" Then
                    txtSecurityDetails.Text = vbCrLf & "**********Deal is a Security Purchase**********" & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & _
                    "Security Reference : " & Trim(drSQLx.Item("tb_id").ToString) & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & vbCrLf & _
                   "********End of Data*******" & vbCrLf

                ElseIf Trim(drSQLx.Item("othercharacteristics")) = "Discount Sale" Then
                    txtSecurityDetails.Text = vbCrLf & "***********Deal is a Security Sell*********" & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & _
                    "Security Deal Reference : " & Trim(drSQLx.Item("purchaseref").ToString) & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & vbCrLf & _
                    "Security Reference : " & GetSecurityRef(Trim(drSQLx.Item("purchaseref").ToString)) & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & vbCrLf & _
                   "********End of Data*******" & vbCrLf
                End If
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try
    End Sub
   

    Private Sub getDealAccountDetails()
        Try
            strSQLx = "select * from  SETTLEDETAILS where dealreference='" & lblReference.Text & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            'lblIAccrualCredit.Text = ""
            'lblIAccrualDebit.Text = ""
            'lblInceptionCreditAcc.Text = ""
            'lblInceptionDebitAcc.Text = ""
            'lbllMaturityInterest.Text = ""


            'Do While drSQLx.Read
            '    lblIAccrualCredit.Text = Trim(drSQLx.Item("ethixcreditaccrual").ToString)
            '    lblIAccrualDebit.Text = Trim(drSQLx.Item("ethixdebitaccrual").ToString)
            '    lblInceptionCreditAcc.Text = Trim(drSQLx.Item("ethixcreditaccount").ToString)
            '    lblInceptionDebitAcc.Text = Trim(drSQLx.Item("ethixdebitaccount").ToString)
            '    lbllMaturityInterest.Text = Trim(drSQLx.Item("ethixinterestaccount").ToString)
            'Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


        Catch ex As Exception

        End Try
    End Sub

    Private Sub getDealInstructions()
        Try
            strSQLx = "select instruction,instructionmat from alldealsall where dealreference='" & Trim(lblReference.Text) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                lblStartInstr.Text = Trim(drSQLx.Item("instruction").ToString)
                lblEnd.Text = Trim(drSQLx.Item("instructionmat").ToString)
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()



        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try
    End Sub

    Private Sub getSecurityDetails()
        'Is it a secured deposit
        If IsSecured(Trim(lblReference.Text)) = True Then
            GetSecurityDetailsDeal()
            Exit Sub
        ElseIf NotSecuredOrSecurity() = True Then
            'Deal is a security Purchase or sell
            GetSecurityDetailsSecurity()
        Else
            txtSecurityDetails.Text = vbCrLf & "**********Deal has no security information***********" & vbCrLf
        End If
    End Sub
    Private Sub GetSecurityDetailsDeal()
        Try
            strSQLx = "select * from attachedSecurities join COLLATERAL_ITEMS on COLLATERAL_ITEMS.collateralReference=" & _
            "attachedSecurities.TB_ID where dealreferencedeal='" & Trim(lblReference.Text) & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()


            Do While drSQLx.Read
                txtSecurityDetails.Text = txtSecurityDetails.Text & _
                "Security Reference : - " & drSQLx.Item("collateralExtReference").ToString & vbCrLf & _
                "Security Description : - " & drSQLx.Item("collateralDescription").ToString & vbCrLf
                txtSecurityDetails.Text = txtSecurityDetails.Text & _
                "Secured Value: - " & Format(CDbl(drSQLx.Item("securityamount")), "###,###,###.00") & vbCrLf

                txtSecurityDetails.Text = txtSecurityDetails.Text & vbCrLf & _
               "**********End of Data**********" & vbCrLf
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


            strSQLx = "select * from COUNTERPARTYSECURITY join COLLATERAL_ITEMS on COLLATERAL_ITEMS.collateralReference=" & _
            "COUNTERPARTYSECURITY.TB_ID where dealreference='" & Trim(lblReference.Text) & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()


            Do While drSQLx.Read
                txtSecurityDetails.Text = txtSecurityDetails.Text & _
                "Security Reference : - " & drSQLx.Item("collateralExtReference").ToString & vbCrLf & _
                "Security Description : - " & drSQLx.Item("collateralDescription").ToString & vbCrLf
                txtSecurityDetails.Text = txtSecurityDetails.Text & _
                "Secured Value: - " & Format(CDbl(drSQLx.Item("amount")), "###,###,###.00") & vbCrLf

                txtSecurityDetails.Text = txtSecurityDetails.Text & vbCrLf & _
               "**********End of Data************" & vbCrLf
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try
    End Sub
    Private Function NotSecuredOrSecurity() As Boolean
        Dim x As Boolean = False

        Try
            strSQLx = "select securityrequired,othercharacteristics from alldealsall where dealreference='" & Trim(lblReference.Text) & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                If Trim(drSQLx.Item(0).ToString) = "N" And Trim(drSQLx.Item(1).ToString) = "Basic Loan" Or Trim(drSQLx.Item(1).ToString) = "Basic Deposit" Then
                    x = False
                Else
                    x = True
                End If
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            Return x

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try
    End Function
    Public Function IsSecured(ByVal ref As String) As Boolean
        Dim x As Boolean = False

        Try
            strSQLX = "select securityrequired from alldealsall where dealreference='" & Trim(ref) & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            Do While drSQLX.Read
                If Trim(drSQLX.Item(0).ToString) = "Y" Then
                    x = True
                Else
                    x = False
                End If
            Loop

            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

            Return x

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try
    End Function
    Private Function GetSecurityRef(ByVal ref) As String
        Dim x As String = ""

        Try
            strSQL = "select tb_id from alldealsall where dealreference='" & Trim(ref) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = drSQL.Item(0).ToString
            Loop

            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            Return x

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try
    End Function
    Private Function CheckSignature() As Boolean
        Dim x As Boolean = False

        Try

            strSQL = "select sig from AUTH_SIG where user_id='" & Trim(Session("username")) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    If drSQL.IsDBNull(0) = True Then
                        x = False
                    Else
                        x = True
                    End If
                Loop
            Else
                x = False
            End If
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException
            '  MsgBox(ex.Message, MsgBoxStyle.Critical, "CheckSignature")
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "CheckParam", "error")

            'Send(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "CheckParam")
            '************************END****************************************
        End Try

        Return x
    End Function
    Public Function CheckAuthorisationStatus(ByVal dealref As String) As Boolean
        Dim x As Boolean

        Try
            strSQL = "select AuthorisationStatus from deals_live where dealreference='" & Trim(dealref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                If Trim(drSQL.Item(0).ToString) = "A" Or Trim(drSQL.Item(0).ToString) = "DA" Then
                    x = True
                Else
                    x = False
                End If
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As NullReferenceException

        Catch eb As Exception
            object_userlog.Msg(eb.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & eb.Message, "error")
        End Try

        Return x

    End Function
    Private Function IsLimitTypeImplemented(ByVal LimitType As String) As Boolean
        Dim x As Boolean = False
        '*****************************************************************************
        'This function checks if limit checking has been implemented for the various *
        'types of limits in TDS and returns true or false to the calling function    *
        'LMTLEVEL1 <> Implement Dealer Deal size Limit Checking                      *
        'LMTLEVEL2 <> Implement Dealer Daily Limit Checking                          *
        'LMTLEVEL3 <> Implement Product Limit Checking                               *
        'LMTLEVEL4 <> Implement Portfolio Limit Checking                             *
        'LMTLEVEL5 <> Implement Authorisation Limits checking                        *
        '*****************************************************************************
        Dim drSQLy2 As SqlDataReader
        Dim cnSQLy2 As SqlConnection
        Dim cmSQLy2 As SqlCommand
        Dim strSQLy2 As String

        Try
            strSQLy2 = "select [value] from systemparameters where [parameter] ='" & Trim(LimitType) & "'"
            cnSQLy2 = New SqlConnection(Session("ConnectionString"))
            cnSQLy2.Open()
            cmSQLy2 = New SqlCommand(strSQLy2, cnSQLy2)
            drSQLy2 = cmSQLy2.ExecuteReader()

            Dim res As String = ""
            If drSQLy2.HasRows = True Then
                Do While drSQLy2.Read
                    If Trim(drSQLy2.Item(0)).Equals("P") Then
                        x = True
                    Else
                        x = False
                    End If

                Loop
            Else
                x = False
            End If

            ' Close and Clean up objects
            drSQLy2.Close()
            cnSQLy2.Close()
            cmSQLy2.Dispose()
            cnSQLy2.Dispose()



            Return x

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try

        Return x
    End Function
    Public Function Required2Authorisers() As Boolean
        Dim x As Boolean = False
        Try
            'validate username first
            strSQL = "select [value] from systemparameters where [parameter]='multiauthorise'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            If drSQL.HasRows = False Then
                MsgBox("Parameter value not set -- [multiauthorise]", MsgBoxStyle.Critical)
            End If

            Do While drSQL.Read
                If Trim(drSQL.Item(0).ToString) = "Y" Then
                    x = True
                Else
                    x = False
                End If
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            '  MsgBox(ex.Message, MsgBoxStyle.Critical, "Required2Authorisers"
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try

        Return x

    End Function
    Public Function GetField(ByVal dealref As String, ByVal returnVal As String) As String
        Dim x As String = ""

        Try
            strSQL = "select " & returnVal & " from alldealsall where dealreference='" & Trim(dealref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                If drSQL.IsDBNull(0) = False Then
                    x = UCase(Trim(drSQL.Item(0).ToString))
                End If
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As NullReferenceException

        Catch eb As Exception
            '     MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
            object_userlog.Msg(eb.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & eb.Message, "error")

        End Try

        Return x

    End Function
    Private Function PrepareForOnline() As String
        Dim res As String = ""
        Try
            'validate username first
            strSQLx = "select [value] from systemparameters where parameter='mmkinp'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            Do While drSQLX.Read
                res = Trim(drSQLX.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As SqlException

            'Log the event *****************************************************

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " PrepareForOnline ", "error")

            '************************END****************************************
        End Try

        Return res

    End Function
    Private Function getRecSequence(ByVal dealref As String) As Integer
        Dim res As Integer
        Dim tempDate As String = DateAdd(DateInterval.Day, 1, CDate(Session("Sysdate")))
        Dim day1 As String
        Dim month1 As String
        Dim year1 As String

        day1 = DatePart(DateInterval.Day, CDate(Session("Sysdate")))
        month1 = DatePart(DateInterval.Month, CDate(Session("Sysdate")))
        year1 = DatePart(DateInterval.Year, CDate(Session("Sysdate")))


        Try
            'Set Deal to authorised
            strSQLx = "select count(dealreference) from forwardopts where dealreference='" & dealref & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()
            If drSQLX.HasRows = True Then
                Do While drSQLX.Read
                    res = drSQLX.Item(0) + 1
                Loop
            Else
                res = 1
            End If
            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

            Return res

        Catch ex As Exception
            '  MsgBox(ex.Message, MsgBoxStyle.Information, "getRecSequence")
            'Log the event *****************************************************

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " getRecSequence ", "error")

            '************************END****************************************
        End Try

    End Function
    Private Function HostIMM() As Boolean
        Dim x As Boolean = False
        Try
            'validate username first
            strSQLx = "select [value] from systemparameters where parameter='hhst'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            Do While drSQLX.Read
                If Trim(drSQLX.Item(0).ToString) = "Y" Then
                    x = True
                Else
                    x = False
                End If
            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

            Return x
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "HostIMM")

            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " HostIMM ", "error")
            '************************END****************************************
        End Try

    End Function
    'Sub for forwarding Money Market Deals to interface
    Private Sub Online_MMK_Deal()
        'Deal send to online system only if the MMK parameter is set to yes Systemparameters [mmkinp]++++++++++
        If PrepareForOnline() = "Y" Then
            Dim FwdSts As String = "P"
            Dim RecSq As Integer = 0

            'Get Sequence to write for input record
            RecSq = getRecSequence(Trim(lblReference.Text))
            CurrentRecSeq = RecSq

            If HostIMM() = True Then
                '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                'Interface here
                'IntOnline.StartApplication(servername, dbname, Session("username"), clients, Session("ConnectionString"), cpforms)
                'Set record for forwarding
                FwdSts = "Y"
                Dim DealStatus As String = GetDealStatus(Trim(lblReference.Text))

                Call IntOnline.FowardRePrep(Trim(lblReference.Text), "Money Market", DealStatus, Session("Sysdate"), FwdSts, Trim(lblDealer.Text), "deals_live", RecSq)

                'if sendig of deal to the interface is not successful repeat the process
                If IntOnline.MMInterface(Trim(lblReference.Text), Trim(lblDealer.Text), RecSq, Session("Sysdate"), DealStatus) = False Then
                    MsgBox("Sending deal to online interface was not successfull. Deal not authorised. Please authorise the deal again." & vbCrLf & _
                            " If this message persists please inform your system adiministrator.", MsgBoxStyle.Information, "Interface")
                    Exit Sub
                End If


                '+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            Else

                'IntOnline.StartApplication(servername, dbname, Session("username"), clients, Session("ConnectionString"), cpforms)
                'Set record for forwarding
                Call IntOnline.FowardRePrep(Trim(lblReference.Text), "Money Market", GetDealStatus(Trim(lblReference.Text)), Session("Sysdate"), FwdSts, Trim(lblDealer.Text), "deals_live", RecSq)

            End If
        End If
    End Sub
    Private Function GetDealStatus(ByVal dealref As String) As String

        Try

            Dim DealCancelled As String
            Dim DateDealEntered As String
            Dim HasBeenProcessed As Boolean = False
            Dim EnteredToday As Boolean = False
            Dim TypeOfDeal As String


            'Check the input journal for the same ref. If there is a ref it means
            'Its a new deal but also now has an ammendment - treat it as an ammendment
            'otherwise its a new deal
            strSQLx = " Select * from  forwardopts where  dealreference = '" & Trim(dealref) & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            If drSQLX.HasRows = True Then
                Do While drSQLX.Read
                    HasBeenProcessed = True
                Loop
            Else
                HasBeenProcessed = False
            End If
            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()


            strSQLx = " Select * from  deals_live where  dealreference = '" & Trim(dealref) & "'"

            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()
            Do While drSQLX.Read

                DateDealEntered = Format(drSQLX.Item("dateentered"), "short date")
                If CDate(DateDealEntered) = CDate(Session("Sysdate")) Then
                    EnteredToday = True
                End If

                If Trim(drSQLX.Item("Dealcancelled").ToString) = "N" Then
                    DealCancelled = False
                Else
                    DealCancelled = True
                End If

                'if the date entered is not equal to the current system date
                'then the deal is being modified however only if the deal exists
                'in the inputupload table because a deal can be entered today and
                'be modified before it is forwarded to host.

                If DealCancelled = True Then
                    'Then the deal is being cancelled
                    Select Case UCase(Trim(drSQLX.Item("othercharacteristics").ToString)) & " CANCEL"

                        Case "BASIC DEPOSIT CANCEL"
                            TypeOfDeal = "DCCM"

                        Case "BASIC LOAN CANCEL"
                            TypeOfDeal = "LCCM"

                        Case "DISCOUNT SALE CANCEL"
                            TypeOfDeal = "SSCCM"

                        Case "DISCOUNT PURCHASE CANCEL"
                            TypeOfDeal = "SCPCM"

                    End Select

                End If

                If HasBeenProcessed = True And DealCancelled = False Then
                    'Then the deal is being modified
                    Select Case UCase(Trim(drSQLX.Item("othercharacteristics").ToString)) & " AMMEND"

                        Case "BASIC DEPOSIT AMMEND"
                            TypeOfDeal = "DPAMMD"

                        Case "BASIC LOAN AMMEND"
                            TypeOfDeal = "LAMMD"

                        Case "DISCOUNT SALE AMMEND"
                            TypeOfDeal = "SSLAMMD"

                        Case "DISCOUNT PURCHASE AMMEND"
                            TypeOfDeal = "SSAMMD"

                    End Select

                End If

                If DealCancelled = False And EnteredToday = False Then
                    'Then the deal is being modified
                    Select Case UCase(Trim(drSQLX.Item("othercharacteristics").ToString)) & " AMMEND"

                        Case "BASIC DEPOSIT AMMEND"
                            TypeOfDeal = "DPAMMD"

                        Case "BASIC LOAN AMMEND"
                            TypeOfDeal = "LAMMD"

                        Case "DISCOUNT SALE AMMEND"
                            TypeOfDeal = "SSLAMMD"

                        Case "DISCOUNT PURCHASE AMMEND"
                            TypeOfDeal = "SSAMMD"

                    End Select

                End If

                If HasBeenProcessed = False And DealCancelled = False And EnteredToday = True Then
                    'Then this is a new deal
                    Select Case UCase(Trim(drSQLX.Item("othercharacteristics").ToString))

                        Case "BASIC DEPOSIT"
                            TypeOfDeal = "NDPPDO"

                        Case "BASIC LOAN"
                            TypeOfDeal = "NLANDO"

                        Case "DISCOUNT PURCHASE"
                            TypeOfDeal = "NSSPDO"

                        Case "DISCOUNT SALE"
                            TypeOfDeal = "NSSSDO"

                    End Select

                End If
            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

            Return TypeOfDeal

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

            Return False
        End Try

    End Function
    Private Function RecordStatus(ref As String, rec As Integer) As Boolean
        Dim res As Boolean = True
        Dim forwardhdr As Boolean = True
        Dim forwardopts As Boolean = True
        Dim opts As Boolean = True

        Dim cnSQLX As SqlConnection
        Dim cmSQLX As SqlCommand
        Dim drSQLX As SqlDataReader
        Dim strSQLx As String

        Try

            strSQLx = "select * from forwardhdr where dealreference='" & Trim(ref) & "' and recseq=" & rec & ""

            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            If drSQLX.HasRows = False Then
                forwardhdr = False
            End If

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()


            strSQLx = "select * from forwardopts where dealreference='" & Trim(ref) & "' and recseq=" & rec & ""

            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            If drSQLX.HasRows = False Then
                forwardopts = False
            End If

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()


            strSQLx = "select opt from forwardopts where dealreference='" & Trim(ref) & "' and recseq=" & rec & ""
            ''and opt in('NSSPDO','NLANDO','NDPPDO','NSSSDO','LCCM'," & _
            '"'DCCM','SCPCM','SSCCM')"

            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            'If drSQLX.HasRows = False Then
            '    opts = False
            'End If

            Do While drSQLX.Read
                If drSQLX.IsDBNull(0) Then
                    opts = False

                End If
            Loop


            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()


            If forwardhdr = False Or forwardopts = False Or opts = False Then
                strSQLx = "delete forwardhdr where dealreference='" & Trim(ref) & "' and recseq=" & rec & "" & _
                          " delete forwardopts where dealreference='" & Trim(ref) & "' and recseq=" & rec & ""
                cnSQLX = New SqlConnection(Session("ConnectionString"))
                cnSQLX.Open()
                cmSQLX = New SqlCommand(strSQLx, cnSQLX)
                drSQLX = cmSQLX.ExecuteReader()

                ' Close and Clean up objects
                drSQLX.Close()
                cnSQLX.Close()
                cmSQLX.Dispose()
                cnSQLX.Dispose()


                res = False

            Else

                res = True

            End If

        Catch ex As SqlException

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " RecordStatus ", "error")

        End Try

        Return res
    End Function
    Protected Sub cmdAuthorize_Click(sender As Object, e As EventArgs) Handles cmdAuthorize.Click

        Dim drSQLy As SqlDataReader
        Dim cnSQLy As SqlConnection
        Dim cmSQLy As SqlCommand
        Dim strSQLy As String

        If Trim(Session("username")) = Trim(Verifier.Text) Then
            lblError.Text = alert("You cannot authorise deals that you have verified.", "Authorise")
            Exit Sub
        End If

        If Trim(Session("username")) = Trim(lblDealer.Text) Then
            lblError.Text = alert("You cannot authorise deals that you have captured.", "Authorise")
            Exit Sub
        End If

        If CheckSignature() = False Then 'Check if user has a signature
            lblError.Text = alert("Signature for authoriser not set, cannot authorise", "Authorise")
            Exit Sub
        End If
        'ernest

        If CheckAuthorisationStatus(Trim(lblReference.Text)) = True Then

            lblError.Text = alert("Someone else has already Authorised the deal", "Authorise")

            Exit Sub
        End If


        If SigGroup = "A" Then
            'Check if the deal has not yet been authorised by A

        ElseIf SigGroup = "B" Then
            'Check if the deal has not yet been authorised by B
        Else
            lblError.Text = alert("You are not authorised to authorise deals", "Authorise")
            Exit Sub
        End If

        Dim conf As String
        If PrintConfirmNow.Checked = True Then
            conf = "Y"
        Else
            conf = "N"
        End If

        'Compare against relevant amount if limit checking is enabled
        'If limit check is implemented proceed otherwise abort checking
        If IsLimitTypeImplemented("LMTLEVEL5") = True Then

            'debit
            If AuthoriseAll = False Then 'Bypass if true
                If dealtype.Text = "L" Then
                    If CDec(lblAmount.Text) > CDec(LoanLimit) Then
                        lblError.Text = alert("Authorisation limit exceeded.", "Authorise")
                        Exit Sub
                    End If
                End If
                'Credit
                If dealtype.Text = "D" Then
                    If CDec(lblAmount.Text) > CDec(DepositLimit) Then
                        lblError.Text = alert("Authorisation limit exceeded.", "Authorise")
                        Exit Sub
                    End If
                End If
            End If


        End If


        Dim dealCancelled As Boolean

        If txtdealref.Text = "" Then
            lblError.Text = alert("Select deal to authorise first.", "Authorise")
            Exit Sub
        End If




        'Perform Interface functions here
        If SigGroup = "A" Or Required2Authorisers = False Then
            'Do accounting postings here
            Dim DealCode As String = GetField(Trim(lblReference.Text), "dealtype")
            Dim entryType As String = GetField(Trim(lblReference.Text), "entrytype")
            Dim Cancelled As String = GetField(Trim(lblReference.Text), "DealCancelled")
            Dim AuthorisedBefore As String = GetField(Trim(lblReference.Text), "idofauthorizer")


            '===================================================================
            If Trim(Cancelled) = "Y" And AuthorisedBefore = "" Then
                'Do not call interface function when Deal cancelled is No and is a new deal because the deal has not been posted to the host before
            Else
                Call Online_MMK_Deal()

                'Check for interface record completeness and prompt user to re-authorise if there are any problems
                'we are only checking for field opt and if both records exist in forwardhdr and forwardopts

                If RecordStatus(Trim(txtdealref.Text), CurrentRecSeq) = False Then

                    'Log the event *****************************************************
                    object_userlog.Msg("An error occured while authorising this deal. Please re-authorise the deal again", True, "#", Session("loggedUserLog") & "DTAU01" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "An error occured while authorising this deal. Please re-authorise the deal again.   Deal Ref : " & Trim(lblReference.Text), "error")
                    '************************END****************************************



                    Exit Sub
                End If
            End If

            '===================================================================


            'Save accounting info
            If Trim(Cancelled) = "N" And AuthorisedBefore = "" Then 'Deal cancelled is No and is a new deal
                Select Case UCase(GetField(Trim(lblReference.Text), "othercharacteristics"))


                    Case "BASIC LOAN"  'PLACEMENT DEAL*****************************************************
                        sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_START_LOAN", "", getDealAccountTDS(DealCode, "FUN"), DealCode & "-" & Trim(lblCustNumber.Text), "LOAN", _
                        Trim(lblCurrency.Text), CDec(Trim(lblAmount.Text)), Trim(lblCustName.Text), False, "DL", Trim(DealCode), 0, "", 0, "", "", "SYSTR01", "1", "new")

                    Case "DISCOUNT PURCHASE" 'DISCOUNT PURCHASE DEAL*****************************************************
                        If Trim(entryType) = "D" Then
                            sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_START_PURCHASE", "INT_DISC_PURCHASE", getDealAccountTDS(Trim(DealCode), "FUN"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "PURCHASE", _
                           Trim(lblCurrency.Text), CDec(Trim(lblMaturityAmount.Text)), Trim(lblCustName.Text), False, "DP", Trim(DealCode), CDec(lblNetInt.Text), "IS", CDbl(lblCom.Text), "CM", "DEAL_COMM_RECIEVE", "SYSTR01", "1", "new")

                        Else
                            sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_START_PURCHASE", "", getDealAccountTDS(Trim(DealCode), "FUN"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "PURCHASE", _
                            Trim(lblCurrency.Text), CDec(Trim(lblAmount.Text)), Trim(lblCustName.Text), False, "DP", Trim(DealCode), 0, "", CDbl(lblCom.Text), "CM", "DEAL_COMM_RECIEVE", "SYSTR01", "1", "new")

                        End If
                    Case "BASIC DEPOSIT" 'BASIC DEPOSIT DEAL*****************************************************
                        sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_START_DEPOSIT", "", getDealAccountTDS(Trim(DealCode), "DAL"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "DEPOSIT", _
                        Trim(lblCurrency.Text), CDec(Trim(lblAmount.Text)), Trim(lblCustName.Text), False, "DA", Trim(DealCode), 0, "", 0, "", "", "SYSTR01", "1", "new")

                    Case "DISCOUNT SELL" 'DISCOUNT SELL DEAL*****************************************************
                        If Trim(entryType) = "D" Then
                            sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_START_SELL", "INT_DISC_SELL", getDealAccountTDS(Trim(DealCode), "DAL"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "SELL", _
                            Trim(lblCurrency.Text), CDec(Trim(lblMaturityAmount.Text)), Trim(lblCustName.Text), False, "DS", Trim(DealCode), CDec(lblNetInt.Text), "IX", 0, "", "", "SYSTR01", "1", "new")

                        Else
                            sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_START_SELL", "", getDealAccountTDS(Trim(DealCode), "DAL"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "SELL", _
                            Trim(lblCurrency.Text), CDec(Trim(lblAmount.Text)), Trim(lblCustName.Text), False, "DS", Trim(DealCode), 0, "", 0, "", "", "SYSTR01", "1", "new")

                        End If

                End Select

            ElseIf Trim(Cancelled) = "Y" And AuthorisedBefore <> "" Then 'Deal cancelled is Yes and is not a new deal
                Select Case UCase(GetField(Trim(lblReference.Text), "othercharacteristics"))


                    Case "BASIC LOAN"  'PLACEMENT DEAL*******************************************************************
                        sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_CANCEL_LOAN", "", getDealAccountTDS(DealCode, "FUN"), DealCode & "-" & Trim(lblCustNumber.Text), "LOAN", _
                        Trim(lblCurrency.Text), CDec(Trim(lblAmount.Text)), Trim(lblCustName.Text), False, "DL", Trim(DealCode), 0, "", 0, "", "", "SYSTR02", "-1", "cancel")

                        'Reverse accruals
                        'if interest accrual is greater than 0 reverse
                        Dim TDSIntRecievableAccout = getDealAccountTDS(DealCode, "INT")
                        Dim CustLoanAccount As String = DealCode & "-" & Trim(lblCustNumber.Text)
                        Dim intAccrued As Decimal = GetIntAccruedToDate(Trim(lblReference.Text))

                        If intAccrued > 0 Then
                            ReverseAccrual(TDSIntRecievableAccout, CustLoanAccount, intAccrued, Trim(lblCurrency.Text))
                        End If

                    Case "DISCOUNT PURCHASE" 'DISCOUNT PURCHASE DEAL*****************************************************
                        If Trim(entryType) = "D" Then
                            sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_CANCEL_PURCHASE", "INT_DISC_PURCHASE", getDealAccountTDS(Trim(DealCode), "FUN"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "PURCHASE", _
                           Trim(lblCurrency.Text), CDec(Trim(lblMaturityAmount.Text)), Trim(lblCustName.Text), False, "DP", Trim(DealCode), CDec(lblNetInt.Text), "IS", CDbl(lblCom.Text), "CM", "DEAL_COMM_RECIEVE", "SYSTR02", "-1", "cancel")

                            'Reverse accruals
                            'if interest accrual is greater than 0 reverse
                            Dim TDSIntRecievableAccout = getDealAccountTDS(DealCode, "INT")
                            Dim CustLoanAccount As String = DealCode & "-" & Trim(lblCustNumber.Text)
                            Dim intAccrued As Decimal = GetFieldVal(Trim(lblReference.Text), "grossinterest") - GetIntAccruedToDate(Trim(lblReference.Text))

                            If intAccrued > 0 Then
                                ReverseAccrual(TDSIntRecievableAccout, CustLoanAccount, intAccrued, Trim(lblCurrency.Text))
                            End If

                        Else
                            sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_CANCEL_PURCHASE", "", getDealAccountTDS(Trim(DealCode), "FUN"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "PURCHASE", _
                            Trim(lblCurrency.Text), CDec(Trim(lblAmount.Text)), Trim(lblCustName.Text), False, "DP", Trim(DealCode), 0, "", CDbl(lblCom.Text), "CM", "DEAL_COMM_RECIEVE", "SYSTR02", "-1", "cancel")

                            'Reverse accruals
                            'if interest accrual is greater than 0 reverse
                            Dim TDSIntRecievableAccout = getDealAccountTDS(DealCode, "INT")
                            Dim CustLoanAccount As String = DealCode & "-" & Trim(lblCustNumber.Text)
                            Dim intAccrued As Decimal = GetIntAccruedToDate(Trim(lblReference.Text))

                            If intAccrued > 0 Then
                                ReverseAccrual(TDSIntRecievableAccout, CustLoanAccount, intAccrued, Trim(lblCurrency.Text))
                            End If

                        End If
                    Case "BASIC DEPOSIT" 'BASIC DEPOSIT DEAL*****************************************************
                        sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_CANCEL_DEPOSIT", "", getDealAccountTDS(Trim(DealCode), "DAL"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "DEPOSIT", _
                        Trim(lblCurrency.Text), CDec(Trim(lblAmount.Text)), Trim(lblCustName.Text), False, "DA", Trim(DealCode), 0, "", 0, "", "", "SYSTR02", "-1", "cancel")

                        'Reverse accruals
                        'if interest accrual is greater than 0 reverse
                        Dim TDSIntPayableAccout = getDealAccountTDS(DealCode, "INT")
                        Dim CustDepositAccount As String = DealCode & "-" & Trim(lblCustNumber.Text)
                        Dim intAccrued As Decimal = GetIntAccruedToDate(Trim(lblReference.Text))

                        If intAccrued > 0 Then
                            ReverseAccrual(CustDepositAccount, TDSIntPayableAccout, intAccrued, Trim(lblCurrency.Text))
                        End If

                    Case "DISCOUNT SELL" 'DISCOUNT SELL DEAL*****************************************************
                        If Trim(entryType) = "D" Then
                            sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_CANCEL_SELL", "INT_DISC_SELL", getDealAccountTDS(Trim(DealCode), "DAL"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "SELL", _
                            Trim(lblCurrency.Text), CDec(Trim(lblMaturityAmount.Text)), Trim(lblCustName.Text), False, "DS", Trim(DealCode), CDec(lblNetInt.Text), "IX", 0, "", "", "SYSTR02", "-1", "cancel")

                            'Reverse accruals
                            'if interest accrual is greater than 0 reverse
                            Dim TDSIntPayableAccout = getDealAccountTDS(DealCode, "INT")
                            Dim CustDepositAccount As String = DealCode & "-" & Trim(lblCustNumber.Text)
                            Dim intAccrued As Decimal = GetFieldVal(Trim(lblReference.Text), "grossinterest") - GetIntAccruedToDate(Trim(lblReference.Text))

                            If intAccrued > 0 Then
                                ReverseAccrual(CustDepositAccount, TDSIntPayableAccout, intAccrued, Trim(lblCurrency.Text))
                            End If
                        Else
                            sysAccounting.PostTransactions(Trim(lblReference.Text), "DEAL_CANCEL_SELL", "", getDealAccountTDS(Trim(DealCode), "DAL"), Trim(DealCode) & "-" & Trim(lblCustNumber.Text), "SELL", _
                            Trim(lblCurrency.Text), CDec(Trim(lblAmount.Text)), Trim(lblCustName.Text), False, "DS", Trim(DealCode), 0, "", 0, "", "", "SYSTR02", "-1", "cancel")

                            'Reverse accruals
                            'if interest accrual is greater than 0 reverse
                            Dim TDSIntPayableAccout = getDealAccountTDS(DealCode, "INT")
                            Dim CustDepositAccount As String = DealCode & "-" & Trim(lblCustNumber.Text)
                            Dim intAccrued As Decimal = GetIntAccruedToDate(Trim(lblReference.Text))

                            If intAccrued > 0 Then
                                ReverseAccrual(CustDepositAccount, TDSIntPayableAccout, intAccrued, Trim(lblCurrency.Text))
                            End If
                        End If

                End Select
            End If
        End If

        Try

            If Required2Authorisers = True Then

                If SigGroup = "A" Then
                    'Set Deal to authorised
                    strSQLX = "begin Tran X" & _
                            " update deals_live set authorisationstatus = 'A',idofauthorizer='" & Trim(Session("username")) & "' where dealreference = '" & Trim(lblReference.Text) & "'" & _
                            " update securitiesconfirmations set authorisationstatus = 'A',idofauthorizer='" & Trim(Session("username")) & "',confirmPrinted='" & conf & "' where dealreference = '" & Trim(lblReference.Text) & "'" & _
                            " update DEALAUTHORISATIONS set authorise2='" & Trim(Session("username")) & "' where dealreference='" & Trim(lblReference.Text) & "' and seq= " & GetLastSeq(Trim(lblReference.Text)) & "" & _
                            " Commit Tran X"
                ElseIf SigGroup = "B" Then
                    'Set Deal to authorised
                    strSQLX = "begin Tran X" & _
                            " update deals_live set authorisationstatus = 'B' where dealreference = '" & Trim(lblReference.Text) & "'" & _
                            " update securitiesconfirmations set authorisationstatus = 'B',idofauthorizer='" & Trim(Session("username")) & "',confirmPrinted='" & conf & "' where dealreference = '" & Trim(lblReference.Text) & "'" & _
                            " update DEALAUTHORISATIONS set authorise1='" & Trim(Session("username")) & "' where dealreference='" & Trim(lblReference.Text) & "' and seq= " & GetLastSeq(Trim(lblReference.Text)) & "" & _
                            " Commit Tran X"


                    MsgBox("Deal authorised!", MsgBoxStyle.Information)
                End If

            Else

                'Set Deal to authorised
                strSQLX = "begin Tran X" & _
                        " update deals_live set authorisationstatus = 'A',idofauthorizer='" & Trim(Session("username")) & "' where dealreference = '" & Trim(lblReference.Text) & "'" & _
                        " update securitiesconfirmations set authorisationstatus = 'A',idofauthorizer='" & Trim(Session("username")) & "',confirmPrinted='" & conf & "' where dealreference = '" & Trim(lblReference.Text) & "'" & _
                        " update DEALAUTHORISATIONS set authorise2='" & Trim(Session("username")) & "',authorise1='" & GetDefaultAuthoriser & "' where dealreference='" & Trim(lblReference.Text) & "' and seq= " & GetLastSeq(Trim(lblReference.Text)) & "" & _
                        " Commit Tran X"

                lblError.Text = success("Deal authorised!", "Success")

            End If

            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()


            If SigGroup = "A" Or Required2Authorisers() = False Then
                'Check if deal is cancelled
                strSQLX = "Select DealCancelled from  deals_live where DealCancelled ='Y' and dealreference = '" & Trim(lblReference.Text) & "'"
                cnSQLX = New SqlConnection(Session("ConnectionString"))
                cnSQLX.Open()
                cmSQLX = New SqlCommand(strSQLX, cnSQLX)
                drSQLX = cmSQLX.ExecuteReader()
                If drSQLX.HasRows = True Then
                    dealCancelled = True
                End If
                ' Close and Clean up objects
                drSQLX.Close()
                cnSQLX.Close()
                cmSQLX.Dispose()
                cnSQLX.Dispose()

                'Move the deal to the cancelled deals table
                If dealCancelled = True Then
                    '  SavDls.clients = clients
                    Call SavDls.CancelDeal(Trim(lblReference.Text), Session("username"), Session("servername"), Session("dataBaseName"))
                End If


                'Confirmations will print here
                If PrintConfirmNow.Checked = True Then
                    If dealCancelled = False Then
                        lblError.Text = success("Deal authorised! Click Ok to print confirmation.", "Authorise")
                        'Print Customer Deal confirmation
                        Response.Redirect("../ReportPages/DealConfirm.aspx?report=Deal Confirmation&DealRef=" & Trim(lblReference.Text))

                        'ConfirmationsPrinter.ConfirmationPrint(Trim(lblReference.Text), "Deal Confirmation.rpt", False) 'set to false if printing is required

                    End If

                Else

                    If dealCancelled = False Then
                        lblError.Text = success("Deal authorised! Click Ok to print confirmation.", "Authorise")
                        'ernest
                        'Print Customer Deal confirmation
                        Response.Redirect("../ReportPages/DealConfirm.aspx?report=Deal Confirmation&DealRef=" & Trim(lblReference.Text))

                    End If

                End If
            End If



            'Log the event *****************************************************

            object_userlog.Msg("An error occured while authorising this deal. Please re-authorise the deal again", True, "#", Session("username") & "DTAU01" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Signature Group : " & SigGroup & "  Deal Ref : " & Trim(lblReference.Text), "error")
            '************************END****************************************

            ' ********************************Check if deal has already matured
            If SigGroup = "A" Or Required2Authorisers() = False Then
                strSQLX = "Select daystomaturity from  deals_live where dealreference = '" & Trim(lblReference.Text) & "'"
                cnSQLX = New SqlConnection(Session("ConnectionString"))
                cnSQLX.Open()
                cmSQLX = New SqlCommand(strSQLX, cnSQLX)
                drSQLX = cmSQLX.ExecuteReader()
                Do While drSQLX.Read
                    If drSQLX.Item(0) <= 0 Then

                        strSQLy = "begin tran Deal " & _
                         " insert into deals_matured " & _
                         " select * from  deals_live where dealreference = '" & Trim(lblReference.Text) & "'" & _
                         " delete deals_live where dealreference = '" & Trim(lblReference.Text) & "'" & _
                         " Commit tran deals "

                        cnSQLy = New SqlConnection(Session("ConnectionString"))
                        cnSQLy.Open()
                        cmSQLy = New SqlCommand(strSQLy, cnSQLy)
                        drSQLy = cmSQLy.ExecuteReader

                        ' Close and Clean up objects
                        drSQLy.Close()
                        cnSQLy.Close()
                        cmSQLy.Dispose()
                        cnSQLy.Dispose()
                    End If

                Loop
                ' Close and Clean up objects
                drSQLX.Close()
                cnSQLX.Close()
                cmSQLX.Dispose()
                cnSQLX.Dispose()


                '***********************************end **********************************
                'Save Confirmation Status info
                'ernest
                'If MessageBox.Show("Do you want to email the confirmation for this deal?", "Email", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                '    Call SaveConfirmationEmail(Trim(lblReference.Text), sysDatt, "N", getRecipients(Trim(lblCustNumber.Text), Trim(lblReference.Text)))
                'Else
                '    Call SaveConfirmationEmail(Trim(lblReference.Text), sysDatt, "S", getRecipients(Trim(lblCustNumber.Text), Trim(lblReference.Text)))
                'End If

            End If


            'Perfom Accounting Functions here*****************************************
            If SigGroup = "A" Then
                'determine the type of authorisation taking place because the deal could be cancelled before it had generated any accounting entries
                'some authorisations dont require any postings to be generated

            End If
            '***********************************end **********************************

            'clear fileds
            lblReference.Text = ""
            lblDealer.Text = ""
            lblAmount.Text = ""
            lblDateEntered.Text = ""
            txtReason.Text = ""
            lblDiscountRate.Text = ""
            lblCustName.Text = ""
            lblMaturityDate.Text = ""
            lblMaturityAmount.Text = ""
            Verifier.Text = ""
            lblTenor.Text = ""
            lblRate.Text = ""
            lblCustNumber.Text = ""
            lblNetInt.Text = ""

            'refresh list
            Call LoadDeals()



        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************

        End Try

    End Sub
    Public Function GetDefaultAuthoriser() As String
        Dim x As String = ""
        Try
            'validate username first
            strSQL = "select user_id from AUTH_SIG where defaultsig='Y'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            If drSQL.HasRows = False Then
                lblError.Text = alert("Default Signatory not set -- GetDefaultAuthoriser", "Authorise")
            End If

            Do While drSQL.Read
                x = Trim(drSQL.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            '   lblError.Text = alert(ex.Message, "GetDefaultAuthoriser")
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetDefaultAuthoriser", "error")


        End Try

        Return x

    End Function
    Public Function GetFieldVal(ByVal dealref As String, ByVal returnVal As String) As Decimal
        Dim x As Decimal = 0

        Try
            strSQL = "select " & returnVal & " from alldealsall where dealreference='" & Trim(dealref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                If drSQL.IsDBNull(0) = False Then
                    x = (Trim(drSQL.Item(0).ToString))
                End If
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As NullReferenceException

        Catch eb As Exception

            lblError.Text = alert(eb.Message, "Error")
        End Try

        Return x

    End Function
    Private Function GetLastSeq(ByVal ref As String) As Integer
        Dim x As Integer
        Try

            strSQL = "select max(seq) from DEALAUTHORISATIONS where dealreference='" & ref & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = drSQL.Item(0)
                Exit Do
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "UpdateConfirmationEmail")

            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "CheckParam", "error")

            'Send(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "CheckParam")
            '************************END****************************************
        End Try

        Return x

    End Function
    Private Sub ReverseAccrual(ByVal DebitAccount As String, ByVal creditAccount As String, ByVal amt As Decimal, ByVal ccy As String)

        Try

            strSQL = "begin tran x" & _
                    " update ACCBAL set accshaddowbal =accshaddowbal-" & amt & " where account='" & DebitAccount & "' and ccy='" & ccy & "'" & _
                    " update ACCBAL set accshaddowbal =accshaddowbal+" & amt & " where account='" & creditAccount & "' and ccy='" & ccy & "'" & _
                    " commit tran x"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "ReverseAccrual")

        End Try
    End Sub
    Public Function getDealAccountTDS(ByVal dealcode As String, ByVal typ As String) As String
        Dim x As String = ""
        Try
            'validate username first
            strSQL = "select TDSAccount from BRDEFAULTS where dealcode='" & Trim(dealcode) & "' and AccountType='" & typ & "' and isdefault='Y'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = Trim(drSQL.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException



            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "CheckParam", "error")

            '************************END****************************************
        End Try

        Return x
    End Function
    Private Function GetIntAccruedToDate(ByVal ref As String) As Decimal
        Dim x As Decimal = 0

        Try

            strSQL = "select intAccruedtodate from alldealsall where dealreference='" & Trim(ref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    If drSQL.IsDBNull(0) = True Then
                        x = 0
                    Else
                        x = drSQL.Item(0)
                    End If
                Loop
            End If
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException


            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "CheckParam", "error")

            '************************END****************************************
        End Try

        Return x
    End Function

    Protected Sub CheckDecline()

       


        Dim dealCancelled As Boolean

        'Check if deal is cancelled and revove cancelled status
        strSQLX = "Select DealCancelled from  deals_live where DealCancelled ='Y' and dealreference = '" & Trim(lblReference.Text) & "'"
        cnSQLX = New SqlConnection(Session("ConnectionString"))
        cnSQLX.Open()
        cmSQLX = New SqlCommand(strSQLX, cnSQLX)
        drSQLX = cmSQLX.ExecuteReader()
        If drSQLX.HasRows = True Then
            dealCancelled = True
        End If
        ' Close and Clean up objects
        drSQLX.Close()
        cnSQLX.Close()
        cmSQLX.Dispose()
        cnSQLX.Dispose()
        'ernest

        'Dim veriDecline As New frmVerificationDecline
        If dealCancelled = True Then
            lblDealCancelled.Text = "Yes"
        Else
            lblDealCancelled.Text = "No"
        End If

        'lblRef.Text = Trim(txtdealref.Text)
        'lblDealerName.Text = Trim(lblDealer.Text)

    End Sub

    Protected Sub CmdRefresh_Click(sender As Object, e As EventArgs) Handles CmdRefresh.Click
        Call LoadDeals()
    End Sub

    Protected Sub btnDecline_Click(sender As Object, e As EventArgs) Handles btnDecline.Click
        If CheckAuthorisationStatus(Trim(txtdealref.Text)) = "True" Then
            lblError.Text = alert("Someone else has already Authorised the deal", "Authorise")

            Exit Sub
        End If


        If Trim(lblReference.Text) = "" Then
            Exit Sub
        End If

        If Trim(Session("username")) = Trim(Verifier.Text) Then
            lblError.Text = alert("You cannot authorise deals that you have verified.", "Authorise")

            Exit Sub
        End If

        If Trim(Session("username")) = Trim(lblDealer.Text) Then
            lblError.Text = alert("You cannot authorise deals that you have captured.", "Authorise")

            Exit Sub
        End If

        If CheckAuthorisationStatus(Trim(lblRef.Text)) = True Then

            object_userlog.Msg("Someone else has already Authorised the deal", False, "#", Session("loggedUserLog") & "DECLIN" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " Revoked deal cancelled status ", "error")

            Exit Sub
        End If

        Dim singleQuote, Character As String
        singleQuote = "'"
        Character = """"

        txtComment.Text = Replace(Trim(txtComment.Text), singleQuote, Character)


        If lblDealCancelled.Text = "Yes" Then
            'If MessageBox.Show("This will revoke the cancelled status of the deal. Do you want to proceed?", "Revoke Cancelled Status", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Yes Then
            'Check if deal is cancelled and revove cancelled status
            strSQLX = "begin tran x" & _
                              " update deals_live set DealCancelled ='N', authorisationstatus='" & GetStataus(Trim(lblRef.Text)) & "' where dealreference = '" & Trim(lblRef.Text) & "'" & _
                               " insert DealStatusDecline values('" & Trim(lblRef.Text) & "','" & Session("username") & "','DA','" & Trim(txtComment.Text) & "','" & Now & "')" & _
                               " commit tran x"

            'Log the event *****************************************************
            object_userlog.Msg("Revoke Cancelled Status", True, "#", Session("loggedUserLog") & "DECLIN" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " Revoked deal cancelled status ", "error")

            '************************END****************************************


        Else

            strSQLX = "begin tran x" & _
                      " update deals_live set authorisationstatus='DA' where dealreference='" & Trim(lblRef.Text) & "' " & _
                      " insert DealStatusDecline values('" & Trim(lblRef.Text) & "','" & Session("username") & "','DA','" & Trim(txtComment.Text) & "','" & Now & "')" & _
                      " commit tran x"
        End If


        Try


            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

            'MsgBox("Deal authorisation declined", MsgBoxStyle.Information, "Verification")
            object_userlog.Msg("Deal authorisation declined", False, "#", Session("loggedUserLog") & "DECLIN" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " Revoked deal cancelled status ", "success")



            'refresh list
            'clear fileds
            lblReference.Text = ""
            lblDealer.Text = ""
            lblAmount.Text = ""
            lblDateEntered.Text = ""
            txtReason.Text = ""
            lblDiscountRate.Text = ""
            lblCustName.Text = ""
            lblMaturityDate.Text = ""
            lblMaturityAmount.Text = ""
            Verifier.Text = ""
            lblTenor.Text = ""
            lblRate.Text = ""


            'refresh list
            Call LoadDeals()

        Catch ex As NullReferenceException

        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try
    End Sub
    Private Function GetStataus(ByVal ref As String) As String
        Dim res As String = ""

        Try

            strSQLx = "select authorise1,authorise2 from DEALAUTHORISATIONS where dealreference='" & ref & "'"


            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQL.Read
                If Trim(drSQLx.Item(0).ToString) = "" Or Trim(drSQLx.Item(1).ToString) = "" Then
                    res = "P"
                Else
                    res = "A"
                End If
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ex As NullReferenceException

        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try

        Return res

    End Function
End Class
Imports System.Data
Imports System.Data.SqlClient
Imports mmDeal
Imports sys_ui
Public Class MaturedDeposit
    Inherits System.Web.UI.Page
    Dim dt As New DataTable()
    Private Dealstructure As String = "Deposit"
    Private CustName As String
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class
    Private globalvars_mmdeal As New GlobalVars.mmDeal   'instance of the userlogins class
    Private object_savedash As New mmDeal.SaveDashBoardDeal   'instance of the userlogins class
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    'ernest
    'Private dbinfo As New dbparm.dbinfo
    Private object_userlog As New usrlog.usrlog
    Private object_mmdeal As New mmDeal.DeaNumbers
    Private object_calcfxn As New mmDeal.CalculationFunctions
    Private object_dealinstr As New mmDeal.DealInstructions
    Private mmdeal_object As New mmDeal.DealMaturityCheck
    '*************************REFERALS***********************************************
    'Private ref As New Referal 'instance of the referals from
    Private RefMessageDealsize As String 'Referal Message Dealsize
    Private RefMessageDaily As String 'Referal Message Daily Limit
    Private referalRequired As Boolean 'Is referal required
    Private refAmount As Decimal 'amount being referred
    Private RefDealType As String 'Deal basic type
    Private dealsizeBroken As Boolean 'Infor on limit status
    Private dailyBroken As Boolean 'Infor on limit status
    Private PortfolioBroken As Boolean 'Infor on limit status
    Private ProductBroken As Boolean 'Infor on limit status
    Private CounterPartyBroken As Boolean 'Infor on limit status
    Private Limitauthoriser As String 'the authoriser
    Private RefMessagePortfolio As String ' Referal Message Portfolio
    Private RefMessageProduct As String 'Referal Message Product
    Private TranxLimitVal As String() 'Stores status of Transaction limits
    Private RefMessageCounterparty As String
    Private refIDRecieve As Integer
    Dim deal_object As New mmDeal.Instructions
    Private intAccruedToDate As Decimal
    ''days to maturity
    Private SavDls As New csvptt.csvptt
    Private daystomature As Decimal
    Public InterestBasisValue As Integer 'Stores the current value for use as the interest dayas basis
    Public IsDeposit As Boolean = False
    Public RollingOver As Boolean = False
    Private Yield As String
    Private btnstatus As String
    Public customerN As String 'custmer name from search
    Public customerA As String 'customer account number from search
    Public CurrencyX As String
    Public CustomerRimNumber As String ' Ethix rim number
    Public RollRef As String
    Public RollBool As Boolean
    Private contactTooTip As String
    Private FundingBasic As String
    Private FundingSuffix As String
    Private intOption As Integer
    Private RatesCheck As New usrfunc.usrfunc
    Private WarningMessage As String = ""
    'Private SecuritySellForm As SaleTBs 'SaleTBs form
    Private ChangedDaysBasis As Boolean = False 'Value indicating if the days basis default has been manually changed
    Private SecureUsingOther As Boolean
    Private crStatus As String 'Stores counterparty limit status 1=exceeded 2= ok
    'settlement variable parameters---------------------
    Dim tdsdebitAccount As String
    Dim tdsCreditAccount As String
    Dim tdscommissionAccount As String
    Dim tdsinterestaccount As String
    Dim ethixdebitAccount As String
    Dim ethixCreditAccount As String
    Dim ethixcommissionAccount As String
    Dim ethixinterestaccount As String
    Dim EthixAccrualDebit As String
    Dim EthixAccrualCredit As String
    '--------------------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call GetIntBasisValue()
            'put wen pledge is enabled
            TabPanel3.Visible = False
            TabPanel4.Visible = False
            Session("Security") = False
            StartDateDeposit.Text = Session("SysDate")
            MaturityDateDeposit.Text = Session("SysDate")
            tabMaturedDeposit.ActiveTabIndex = 0
            'btnSaveNew.Enabled = False
            'URL variables
            lblDays.Text = InterestBasisValue
            lblCurrency.Text = Request.QueryString("currency")
            lblPortfolioDeposit.Text = Request.QueryString("portifolio")
            lblDealtypeDeposit.Text = Request.QueryString("dealcode")
            lblProduct.Text = Request.QueryString("product")

            Call Deal_Instructions()
            Call LoadCustomers()
            Call load_TBs()

        End If
    End Sub
    Public Sub GetHist(cName As String, apptype As String, ccy As String)
        Try


            strSQL = "Select * from deals_live where CustomerNumber='" & Trim(cName) & "' and othercharacteristics='" & Trim(apptype) & "' and currency='" & Trim(ccy) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GridClientHistory.DataSource = drSQL
            GridClientHistory.DataBind()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


            strSQL = "select sum(CAST(interestrate AS decimal))/count(dealreference)," & _
                     "sum(dealamount),sum(tenor)/count(dealreference)" & _
                     " from deals_live where customernumber='" & Trim(cName) & "' and othercharacteristics='" & apptype & "' and currency='" & Trim(ccy) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()


            Do While drSQL.Read

                If drSQL.IsDBNull(0) = True Then
                    avgRate.Text = "Avg Rate=0"
                Else
                    avgRate.Text = "Avg Rate=" & drSQL.Item(0).ToString
                End If
                If drSQL.IsDBNull(1) = True Then
                    avgSize.Text = "Total Deals=0"
                Else
                    avgSize.Text = "Total Deals=" & Format(CDbl(drSQL.Item(1).ToString), "###,###.00")
                End If
                If drSQL.IsDBNull(2) = True Then
                    avgTenor.Text = "Avg Tenor=0"
                Else
                    avgTenor.Text = "Avg Tenor=" & drSQL.Item(2).ToString
                End If



            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
        End Try

    End Sub
    Public Sub GetCustomerAddressInfo(ByVal custNum As String)


        'Load Instructions
        Try
            'validate username first
            strSQL = "Select * from CUSTOMER where Customer_Number='" & Trim(custNum) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            lblAddress.Text = ""
            Do While drSQL.Read
                lblAddress.Text = Trim(drSQL.Item("Title").ToString) & "<br>" & Trim(drSQL.Item("FullName").ToString) & "<br>" & Trim(drSQL.Item("ID_Number").ToString) & "<br>" & Trim(drSQL.Item("Address_L1").ToString) & "<br>" & Trim(drSQL.Item("Address_L2").ToString) & "<br>" & Trim(drSQL.Item("Address_L3").ToString) & "<br>" & Trim(drSQL.Item("Address_L4").ToString) & "<br>" & Trim(drSQL.Item("Address_L5").ToString) & "<br>" & Trim(drSQL.Item("custemail").ToString)


                'If IsDBNull(drSQL.Item("custage")) = False Then
                '    custAge.Text = drSQL.Item("custage")
                'Else
                '    custAge.Text = "Not Specified"
                'End If

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
        End Try


    End Sub
    Private Sub LoadCustomers()

        Try
            strSQL = "select customer_Number,fullName from customer where frontoffice='Y' and len(customer_number)<=9 order by fullname"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    cmbCustomer.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
                Loop
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            'ernest
            '' CusDownloadStart.Suspend()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")
        End Try
    End Sub
    Public Sub Deal_Instructions()
        Try
            'validate username first
            strSQL = "Select * from instrucparam join dealinstr on instrucparam.instid= dealinstr.instid where appid='MNKDEP'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    If drSQL.Item("purpose").ToString.Equals("M") Then
                        cmbInstructionsDepositM.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                    ElseIf drSQL.Item("purpose").ToString.Equals("I") Then
                        cmbInstructionsDeposit.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                    End If
                Loop


            End If

            cmbInstructionsDeposit.Items.Add("Other")
            cmbInstructionsDepositM.Items.Add("Other")

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException


            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try

    End Sub
    Private Function GetInterestAccount(ByVal dealCode As String) As String
        Dim intAccount As String
        Try
            'save info for dealslip re-print
            strSQL = "select  EthixAccount from BRDEFAULTS where dealcode='" & dealCode & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                intAccount = drSQL.Item(0).ToString
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try

        Return intAccount
    End Function
    Protected Sub btnValidateDeposit_Click(sender As Object, e As EventArgs) Handles btnValidateDeposit.Click

        If mmdeal_object.CheckBackValueThreshhold("mmbkval") < Math.Abs(DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateDeposit.Text))) Then
            lblError.Text = alert("You have exceeded maximum back value threshold", "Error")
            Exit Sub
        End If


        Dim limitsch As New usrlmt.usrlmt
        limitsch.clients = Session("client")
        limitsch.ConnectionString = Session("ConnectionString")
        limitsch.ClearVariables()
        dealsizeBroken = False
        dailyBroken = False
        CounterPartyBroken = False
        ProductBroken = False
        PortfolioBroken = False
        Limitauthoriser = ""
        RefMessageDaily = ""
        RefMessageDealsize = ""
        RefMessageCounterparty = ""
        RefMessageProduct = ""
        RefMessagePortfolio = ""
        WarningMessage = ""


        'Request for users online
        object_userlog.SendData("REQUESTUSERS|" & Session("username"), Session("serverName"), Session("client"))
        Dim ID As String
        ID = Request.QueryString("portifolioid")
        Dim x As Decimal
        If getSysParam("fundrequired") = "Y" Then 'Check if the accounts are required

            If Trim(Request.QueryString("dealcode")) = "" Or Trim(Request.QueryString("dealcode")) = "undefined" Then
                lblError.Text = alert("Interest account not set for product, dealing will be disabled", "Deal Code Not Set")
                tabMaturedDeposit.ActiveTabIndex = 0
                Exit Sub
            End If

            If Trim(txtDealAccountDeposit.Text) = "" Then
                lblError.Text = alert("Please select Funding Account", "Funding Account")
                tabMaturedDeposit.ActiveTabIndex = 0

                Exit Sub
            End If

            If Trim(txtRecievingDeposit.Text) = "" Then
                lblError.Text = alert("Please select product Deal Account", "Deal Account")
                tabMaturedDeposit.ActiveTabIndex = 0

                Exit Sub
            End If
        End If


        If mmdeal_object.NonBusinessDay(MaturityDateDeposit.Text.ToString()) = True Then
            lblError.Text = alert("The selected maturity date is a not a business day.", "Non-Business Day")
            tabMaturedDeposit.ActiveTabIndex = 1
            Exit Sub
        End If

        If mmdeal_object.Holidays(MaturityDateDeposit.Text.ToString(), getBaseCurrency) = True Then
            lblError.Text = alert("The selected maturity date is a not a business day.", "Non-Business Day")
            tabMaturedDeposit.ActiveTabIndex = 1
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDateDeposit.Text)) < 0 Then
            lblError.Text = alert("Maturity date cannot be less than business date", "Maturity date")
            tabMaturedDeposit.ActiveTabIndex = 1
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDateDeposit.Text)) = 0 Then
            lblError.Text = alert("Maturity date cannot be the same as deal start date", "Maturity date")
            tabMaturedDeposit.ActiveTabIndex = 1
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateDeposit.Text)) > 0 Then
            lblError.Text = alert("Maturity date cannot be the same as deal start date", "Maturity date")
            tabMaturedDeposit.ActiveTabIndex = 1
            Exit Sub
        End If

        If Trim(Request.QueryString("currency")) = "" Or Trim(Request.QueryString("currency")) = "undefined" Then
            alert("Select Currency for deal", "Deal Currency Not Set")
            tabMaturedDeposit.ActiveTabIndex = 0
            Exit Sub
        End If
        If Trim(Request.QueryString("portifolio")) = "" Or Trim(Request.QueryString("portifolio")) = "undefined" Then
            alert("Select portifolio for deal", "Deal portifolio Not Set")
            tabMaturedDeposit.ActiveTabIndex = 0
            Exit Sub
        End If

        If cmbCustomer.Text = "0" Then
            lblError.Text = alert("Select customer for deal", "Deal customer Not Set")
            tabMaturedDeposit.ActiveTabIndex = 0
            Exit Sub
        End If
        If txtDealAmountDeposit.Text = "" Then
            lblError.Text = alert("Enter deal amount for deal", "Deal  deal amount Not Set")
            Exit Sub
        End If
        If txtIntRateDeposit.Text = "" Then
            lblError.Text = alert("Enter  interest rate", " interest rate")
            tabMaturedDeposit.ActiveTabIndex = 1
            Exit Sub
        End If
        If txtTenorDeposit.Text = "" Then
            lblError.Text = alert("Enter  the period", "Tenor")
            tabMaturedDeposit.ActiveTabIndex = 1
            'txtTenorDeposit.Focus()
            Exit Sub
        End If
        'If txtTaxRate.Text = "" Then
        '    lblError.Text = alert("Enter tax rate", "Tax rate")
        '    tabMaturedDeposit.ActiveTabIndex = 1
        '    txtTaxRate.Focus()
        '    Exit Sub
        'End If
        If cmbInstructionsDeposit.Text = "0" Then
            lblError.Text = alert("Enter instruction for deal Inception.", "Incomplete Instruction information")
            tabMaturedDeposit.ActiveTabIndex = 4
            Exit Sub
        End If
        If cmbInstructionsDeposit.Text = "Other" And txtOtherInsDeposit.Text = "" Then
            lblError.Text = alert("Please enter other instructions for deal inception.", "Incomplete deal inception information")
            tabMaturedDeposit.ActiveTabIndex = 4
            Exit Sub
        End If

        If cmbInstructionsDepositM.Text = "0" Then
            lblError.Text = alert("Enter instruction for deal maturity.", "Incomplete deal maturity information")
            tabMaturedDeposit.ActiveTabIndex = 4
            Exit Sub
        End If
        If cmbInstructionsDepositM.Text = "Other" And txtOtherInsDepositM.Text = "0" Then
            lblError.Text = alert("Please enter other instructions for deal maturity.", "Incomplete deal maturity information")
            tabMaturedDeposit.ActiveTabIndex = 4
            Exit Sub
        End If

        If object_dealinstr.checkDealAccount() = "Y" Then
            If txtDealAccountDeposit.Text = "" Then
                lblError.Text = alert("Deal funding account not selected", "Stop")
                tabMaturedDeposit.ActiveTabIndex = 1
                Exit Sub
            End If
        End If
        Dim daysz As Integer = 0
        If mmdeal_object.NonBusinessDay(MaturityDateDeposit.Text) = True Then
            lblError.Text = alert("The selected maturity date is a not a business day.", "Non-Business Day")
            tabMaturedDeposit.ActiveTabIndex = 1
            Exit Sub
        End If
        If mmdeal_object.Holidays(MaturityDateDeposit.Text, getBaseCurrency) = True Then
            lblError.Text = alert("The selected maturity date is a not a business day.", "Non-Business Day")
            tabMaturedDeposit.ActiveTabIndex = 1
            Exit Sub
        End If


        '*****************End




        If ChangedDaysBasis = True Then
            If chkRollover.Checked = True Then
                Call object_calcfxn.CalcEngine1(CDec(daysz), CDec(txtIntRateDeposit.Text), CDec(txtDealAmountDeposit.Text), CDec(cmbtax1.SelectedValue.ToString), InterestBasisValue)
            Else
                Call object_calcfxn.CalcEngine1(CDec(txtTenorDeposit.Text), CDec(txtIntRateDeposit.Text), CDec(txtDealAmountDeposit.Text), CDec(cmbtax1.SelectedValue.ToString), InterestBasisValue)
            End If

        Else
            If chkRollover.Checked = True Then
                Call object_calcfxn.CalcEngine(CDec(daysz), CDec(txtIntRateDeposit.Text), CDec(txtDealAmountDeposit.Text), CDec(cmbtax1.SelectedValue.ToString), Trim(Request.QueryString("currency")))
            Else
                Call object_calcfxn.CalcEngine(CDec(txtTenorDeposit.Text), CDec(txtIntRateDeposit.Text), CDec(txtDealAmountDeposit.Text), CDec(cmbtax1.SelectedValue.ToString), Trim(Request.QueryString("currency")))
            End If

            object_calcfxn.IntBasis(Trim(Request.QueryString("currency")))
        End If

        txtMaturityAmountDeposit.Text = object_calcfxn.maturityAmount.ToString
        txtNetIntDeposit.Text = object_calcfxn.netInt.ToString
        txtGrossInt.Text = object_calcfxn.grossInt.ToString
        txtNetIntDeposit.Text = object_calcfxn.netInt.ToString
        txtTaxAmountDeposit.Text = object_calcfxn.taxAmount.ToString

        x = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateDeposit.Text), )
        If x < 0 Then
            x = Math.Abs(x)
            daystomature = CDec(txtTenorDeposit.Text) - x
            object_calcfxn.CalcEngine(x, CDec(txtIntRateDeposit.Text), CDec(txtDealAmountDeposit.Text), CDec(cmbtax1.SelectedValue.ToString), Trim(Request.QueryString("currency")))
            intAccruedToDate = Session("intAccruedToDateCal")
        Else
            intAccruedToDate = 0
            daystomature = CDec(txtTenorDeposit.Text)
        End If




        '-------------------------------------
        're-initialize variablse
        object_calcfxn.netInt = 0
        object_calcfxn.maturityAmount = 0
        object_calcfxn.taxAmount = 0
        object_calcfxn.grossInt = 0
        object_calcfxn.DiscountRateDerived = 0
        object_calcfxn.YieldRateDerived = 0
        '-------------------------------------
       
        '************************************************************************

        If UCase(Trim(Request.QueryString("dash"))) = "FALSE" Then 'Check limits only if the deal is not on the dash board


            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ETHX02" & Format(Now, "dd/MM/yyyy hh:mm:ss"), Session("serverName"), Session("client"))
            '************************END****************************************

            'Check Trnasaction Limits if implemented this function will return 1 to indicate that 
            'limits have been exceeded and 2 to indicate that transaction is within the limit
            '0 means Limit checking nf ot implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount

          


                RefMessageDealsize = limitsch.RefMessageDealsize
                RefMessageDaily = limitsch.RefMessageDaily
                referalRequired = limitsch.referalRequired
                refAmount = limitsch.refAmount
                RefDealType = limitsch.RefDealType
                dealsizeBroken = limitsch.dealsizeBroken
                dailyBroken = limitsch.dailyBroken
                PortfolioBroken = limitsch.PortfolioBroken
                ProductBroken = limitsch.ProductBroken
                CounterPartyBroken = limitsch.CounterPartyBroken
                Limitauthoriser = limitsch.Limitauthoriser
                RefMessagePortfolio = limitsch.RefMessagePortfolio
                RefMessageProduct = limitsch.RefMessageProduct
                RefMessageCounterparty = limitsch.RefMessageCounterparty

                '    If MessageBox.Show("Limits have been exceeded. Do you want to send a referal?", "Limits", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                '        Dim ref As New Referal
                '        'ref.RefMsg(RefMessageDaily, RefMessageDealsize, RefMessageCounterparty, RefMessageProduct, RefMessagePortfolio, CustName, refAmount, RefDealType)
                '        ref.RefMessageDealsize = limitsch.RefMessageDealsize
                '        ref.RefMessageDaily = limitsch.RefMessageDaily
                '        ref.refAmount = limitsch.refAmount
                '        ref.RefDealType = limitsch.RefDealType
                '        ref.Limitauthoriser = limitsch.Limitauthoriser
                '        ref.RefMessagePortfolio = limitsch.RefMessagePortfolio
                '        ref.RefMessageProduct = limitsch.RefMessageProduct
                '        ref.RefMessageCounterparty = limitsch.RefMessageCounterparty


                '        ref.DealCustomer = Trim(txtCustomerNameDeposit.Text)
                '        ref.DealDealAmount = CDec(txtDealAmountDeposit.Text)
                '        ref.DealDealInterestRate = Trim(txtIntRateDeposit.Text)
                '        ref.DealTenor = Int(txtTenorDeposit.Text)
                '        ref.DealMaturityAmount = CDec(txtMaturityAmountDeposit.Text)
                '        ref.dealGrossInterest = CDec(txtGrossInt.Text)
                '        ref.dealPortfolio = Trim(cmbPortfolioDeposit.Text)
                '        ref.dealDealType = Trim(cmbDealcodeDeposit.Text)
                '        ref.DealCurrency = Trim(Currencies4.Text)
                '        ref.DealTransType = "Basic Deposit"
                '        ref.CustName = customerN

                '        ref.ShowDialog()

                '        Limitauthoriser = ref.Limitauthoriser

                '        ref.Close()

                '        If btnstatus = "N" Then Exit Sub 'if Dont send referal

                '        If referalResponse <> 1 Then
                '            referalResponse = 3
                '            Exit Sub 'response was no
                '        End If
                '    Else
                '        Exit Sub
                '    End If
                '    referalResponse = 3 're-initialise variable
                'End If
            End If

        cmdSaveDealDeposit.Enabled = True
        tabMaturedDeposit.ActiveTabIndex = 1
        lblSuccess.Text = success("Deal Testing Complete <br>Dealing Has Been Enabled <br> Dealer Can Procced To Deal", "Deal Testing")

    End Sub

    Private Function GetFieldVal(ByVal fldkey As String, ByVal returnVal As String, dealtype As String) As Decimal
        Dim x As Decimal = 0

        Try
            strSQL = "select " & returnVal & " from COUNTERLIMITS where customernumber='" & Trim(fldkey) & "' and dealtype='" & dealtype & "'"

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
            object_userlog.Msg(eb.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & eb.Message, "error")

        End Try

        Return x

    End Function
    Private Function CounterpartyLimitViolation(dealamt As Double, dealtype As String, curr As String, matDate As String) As String
        Dim x As String = ""
        Dim myLimit As Decimal = CDbl(GetFieldVal(customerA, "limit", dealtype))
        Dim mycumulativeTotal As Decimal = CDbl(GetFieldVal(customerA, "cumulativetotal", dealtype))

        x = limitExpires(customerA, dealtype, curr, CDate(matDate))
        If x <> "" Then
            lblError.Text = alert(x, "Error")
            x = "stop"
            ' Exit Function
        Else

            object_userlog.Msg("Counterparty Limit exceeded" & vbCrLf & _
                                 "_____________________________________________________" & vbCrLf & _
                                 "Excess : " & Format(myLimit - (CDbl(dealamt) + mycumulativeTotal), "###,###.00") & vbCrLf & _
                                 "Limit Amount :   " & Format(myLimit, "###,###.00") & vbCrLf & _
                                 "_____________________________________________________" & vbCrLf & _
                                 "Cumulative Total :   " & Format(mycumulativeTotal, "###,###.00") & vbCrLf & _
                                 "Transaction Amount :   " & Format(dealamt, "###,###.00"), False, "#", " ", "warning")
        End If

        If getSysParam("tranlmt") = "warn" Then

            object_userlog.Msg("Counterparty Limit violation will be recorded", False, "#", "", "error")


            x = "warn"
        ElseIf getSysParam("tranlmt") = "stop" Then
            'ernest
            lblWarning.Text = alert("Counterparty Limit exceeded, you cannot proceed with the transaction", "Counterparty Limit exceeded")
            x = "stop"
        End If

        Return x
    End Function
    Private Function limitExpires(cusnum As String, dealtype As String, curr As String, maturitydt As String)
        Dim x As String = ""

        strSQL = "select * from counterlimits join customer on customer.customer_number=" & _
                  " counterlimits.customernumber where" & _
                  " customernumber = '" & cusnum & "' and dealtype = '" & dealtype & "' and currency='" & Trim(curr) & "'"
        cnSQL = New SqlConnection(Session("ConnectionString"))
        cnSQL.Open()
        cmSQL = New SqlCommand(strSQL, cnSQL)
        drSQL = cmSQL.ExecuteReader()

        Do While drSQL.Read

            If CDate(maturitydt) > CDate(drSQL.Item("lmtexpiry")) Then
                x = "Limit expires before deal matures. Limit expiry date is : " & CDate(drSQL.Item("lmtexpiry")) & " and deal matures : " & CDate(maturitydt)
            End If
        Loop
        ' Close and Clean up objects
        drSQL.Close()
        cnSQL.Close()
        cmSQL.Dispose()
        cnSQL.Dispose()


        Return x
    End Function
    Private Function getBaseCurrency() As String
        Dim x As String
        Try
            'validate username first
            strSQL = "select currencycode from currencies where isbasecurrency='yes'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = Trim(drSQL.Item(0))
            Loop


            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            Return x

        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "Error")

            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " CheckBaseCurrency", "error")

            '************************END****************************************

        End Try
    End Function
    Private Function getSysParam(ByVal parm As String) As String

        Dim x As String = ""
        Dim cnSQLdd As SqlConnection
        Dim cmSQLdd As SqlCommand
        Dim drSQLdd As SqlDataReader
        Dim strSQLdd As String

        Try
            strSQLdd = "select * from SYSTEMPARAMETERS where [parameter]='" & parm & "'"
            cnSQLdd = New SqlConnection(Session("ConnectionString"))
            cnSQLdd.Open()
            cmSQLdd = New SqlCommand(strSQLdd, cnSQLdd)
            drSQLdd = cmSQLdd.ExecuteReader

            Do While drSQLdd.Read
                x = Trim(drSQLdd.Item(1))
            Loop

            ' Close and Clean up objects
            drSQLdd.Close()
            cnSQLdd.Close()
            cmSQLdd.Dispose()
            cnSQLdd.Dispose()


        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

        End Try

        Return Trim(x)

    End Function

    Private Function getDealAccountEthix(ByVal tdsaccount As String) As String
        Dim x As String = ""
        Try
            'validate username first
            strSQL = "select ETHIXAccount from BRDEFAULTS where tdsaccount='" & Trim(tdsaccount) & "' and isdefault='Y'"
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
            lblError.Text = alert(ex.Message, "Error")

            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try

        Return x
    End Function
    Private Sub GetIntBasisValue()

        Dim cnSQLf As SqlConnection
        Dim cmSQLf As SqlCommand
        Dim drSQLf As SqlDataReader
        Dim strSQLf As String

        Try
            strSQLf = "select [value] from systemparameters where parameter='intb'"

            cnSQLf = New SqlConnection(Session("ConnectionString"))
            cnSQLf.Open()
            cmSQLf = New SqlCommand(strSQLf, cnSQLf)
            drSQLf = cmSQLf.ExecuteReader()

            Do While drSQLf.Read
                InterestBasisValue = Int(drSQLf.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQLf.Close()
            cnSQLf.Close()
            cmSQLf.Dispose()
            cnSQLf.Dispose()

        Catch ex As SqlException
            'Log the event *****************************************************

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetIntBasisValue", "error")

            '************************END****************************************
        End Try
    End Sub
    Private Function getDealAccountTDS(ByVal dealcode As String, ByVal typ As String) As String
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
            lblError.Text = alert(ex.Message, "Error")

            'Log the event *****************************************************

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

            '************************END****************************************
        End Try

        Return x
    End Function

    Private Sub SetDefaultSettlementAccounts(ByVal dealref As String, ByVal tdsdebitAccount As String, ByVal tdsCreditAccount As String, _
   ByVal tdsinterestaccount As String, ByVal tdscommissionAccount As String, ByVal ethixdebitAccount As String, ByVal ethixCreditAccount As String, _
   ByVal ethixinterestaccount As String, ByVal ethixcommissionAccount As String, ByVal debitAccrual As String, ByVal creditAccrual As String)

        'update the record / or insert
        Try
            strSQL = "insert SETTLEDETAILS values('" & Trim(dealref) & "','" & ethixdebitAccount & "','" & ethixCreditAccount & "','" & ethixcommissionAccount & "','" & ethixinterestaccount & "'" & _
           " ,'" & tdsdebitAccount & "','" & tdsCreditAccount & "','" & tdscommissionAccount & "','" & tdsinterestaccount & "','" & debitAccrual & "','" & creditAccrual & "')"

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
            'Log the event *****************************************************

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "SetDefaultSettlementAccounts -- Update / Insert", "error")

            '************************END****************************************

        End Try

    End Sub
    Private Function Get_TB(ByVal dealref As String)
        Try
            'validate username first
            strSQL = "select securitypurchase.tb_id from securitypurchase join deals_live on securitypurchase.dealreference=deals_live.dealreference" & _
                     "  where matured = 'N' and authorisationstatus='A' and securitypurchase.dealreference='" & Trim(dealref) & "' and  currency='" & Trim(lblCurrency.Text) & "'"
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

            'ListTbs.Columns(0).ListView.Sorting = SortOrder.Descending
            'ListTbs.Sort()

        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "Error")
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Load_TBs : AttachSecurity " & ex.Message, Session("serverName"), Session("client"))

        End Try
    End Function
    Private Sub load_TBs()
        Try
            'validate username first
            strSQL = "select securitypurchase.tb_id,securitypurchase.dealreference from securitypurchase join deals_live on securitypurchase.dealreference=deals_live.dealreference" & _
                     "  where matured = 'N' and authorisationstatus='A' and currency='" & Trim(lblCurrency.Text) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                cmbSecurity.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(1).ToString)))
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
    Private Function getEthixAccrualAccount(ByVal dealCode As String, ByVal typ As String) As String
        Dim x As String = ""
        Try
            'validate username first
            strSQL = "select " & typ & " from ETHIX_PRODUCT_ACCRUAL where dealcode='" & Trim(dealCode) & "'"
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
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try

        Return x
    End Function

    Protected Sub cmdSaveDealDeposit_Click(sender As Object, e As EventArgs) Handles cmdSaveDealDeposit.Click

        customerA = cmbCustomer.SelectedValue.ToString()
        daystomature = Math.Abs(DateDiff(DateInterval.Day, CDate(StartDateDeposit.Text), CDate(MaturityDateDeposit.Text)))
        Dim Pledged As String

        Dim Instruction As String
        If Trim(cmbInstructionsDeposit.Text) = "Other" Then
            Instruction = Replace(Trim(txtOtherInsDeposit.Text), "’", "")
            Instruction = Replace(Trim(txtOtherInsDeposit.Text), "'", "")
            Instruction = Replace(Trim(txtOtherInsDeposit.Text), "&", "and")
        Else
            Instruction = Replace(Trim(cmbInstructionsDeposit.Text) & " -" & Trim(txtOtherInsDeposit.Text), "’", "")
            Instruction = Replace(Trim(cmbInstructionsDeposit.Text) & " -" & Trim(txtOtherInsDeposit.Text), "'", "")
            Instruction = Replace(Trim(cmbInstructionsDeposit.Text) & " -" & Trim(txtOtherInsDeposit.Text), "&", "and")
        End If

        Dim InstructionM As String
        If Trim(cmbInstructionsDepositM.Text) = "Other" Then
            InstructionM = Replace(Trim(txtOtherInsDepositM.Text), "’", "")
            InstructionM = Replace(Trim(txtOtherInsDepositM.Text), "'", "")
            InstructionM = Replace(Trim(txtOtherInsDepositM.Text), "&", "and")
        Else
            InstructionM = Replace(Trim(cmbInstructionsDepositM.Text) & " -" & Trim(txtOtherInsDepositM.Text), "’", "")
            InstructionM = Replace(Trim(cmbInstructionsDepositM.Text) & " -" & Trim(txtOtherInsDepositM.Text), "'", "")
            InstructionM = Replace(Trim(cmbInstructionsDepositM.Text) & " -" & Trim(txtOtherInsDepositM.Text), "&", "and")

        End If


        Dim dealNum As String = ""
        Dim secured As String = "N"

        'get deal number
        'if deal is not the dashboard
        ' False = False 
        If Request.QueryString("dash") = "False" Then
            dealNum = object_mmdeal.GetDealNumber(Request.QueryString("dealcode").ToString())
        Else
            dealNum = object_mmdeal.GetDealNumber(Request.QueryString("dealcode").ToString())
        End If

        'Check if the dealreference is ok
        If Trim(dealNum) = "" Then
            lblError.Text = alert(Session("DealRefError"), "Deal reference not generated")
            Exit Sub
        End If
        
        If Request.QueryString("dash") = "False" Then
            Call SavDls.saveMaturedDeposit(daystomature, Session("SysDate"), deal_object.RequireFrontAuthoriser(), Trim(Request.QueryString("portifolioid").ToString()), Trim(Request.QueryString("dealcode")), customerA, StartDateDeposit.Text _
                    , MaturityDateDeposit.Text, CDec(Trim(txtDealAmountDeposit.Text)), CDec(Trim(txtMaturityAmountDeposit.Text)), CDec(Trim(txtIntRateDeposit.Text)) _
                    , CDec(Trim(txtNetIntDeposit.Text)), Int(Trim(txtTenorDeposit.Text)), Instruction, dealNum, "Basic Deposit" _
                    , intAccruedToDate, CInt(cmbtax1.SelectedValue.ToString), CDec(Trim(txtTaxAmountDeposit.Text)), CDec(Trim(txtGrossInt.Text)), secured, Pledged, Trim(Request.QueryString("currency")) _
                    , Session("IsDealer"), Session("serverName"), Session("dataBaseName"), Session("loggedUserLog"), globalvars_mmdeal.PrintPages, InterestBasisValue, InstructionM, Trim(gettaxcode(cmbtax1.SelectedValue.ToString)))

            tdsdebitAccount = Trim(Request.QueryString("dealcode")) & "-" & Trim(customerA) 'getDealAccountTDS(Trim(cmbDealcodeDeposit.Text), "FUN")
            tdsCreditAccount = getDealAccountTDS(Trim(Request.QueryString("dealcode")), "DAL")
            tdscommissionAccount = getDealAccountTDS(Trim(Request.QueryString("dealcode")), "COM")
            tdsinterestaccount = getDealAccountTDS(Trim(Request.QueryString("dealcode")), "INT")
            ethixdebitAccount = Trim(txtDealAccountDeposit.Text) 'getDealAccountEthix(tdsdebitAccount)
            ''  ethixCreditAccount = Trim(txtRecievingDeposit.Text) 'getDealAccountEthix(tdsCreditAccount)
            ethixcommissionAccount = getDealAccountEthix(tdscommissionAccount)
            ethixinterestaccount = getDealAccountEthix(tdsinterestaccount)
            EthixAccrualDebit = getEthixAccrualAccount(Trim(Request.QueryString("dealcode")), "debitAccount")
            EthixAccrualCredit = getEthixAccrualAccount(Trim(Request.QueryString("dealcode")), "creditAccount")


            'Apply Default Settlement Details

            SetDefaultSettlementAccounts(dealNum, tdsdebitAccount, tdsCreditAccount, tdsinterestaccount, _
            tdscommissionAccount, ethixdebitAccount, ethixCreditAccount, ethixinterestaccount, _
            ethixcommissionAccount, EthixAccrualDebit, EthixAccrualCredit)

            '    'Set settlement instructions / accounts if true
            '    If GetSettlementDetailsPar() = True Then
            '        Dim settlem As New frmSettlement
            '        settlem.seqq = "1"
            '        settlem.typOFDeal.Text = "MM"
            '        settlem.lblReference.Text = dealNum
            '        settlem.ShowDialog()
            '    End If


        Else
            'save the dashboard deal
            object_savedash.saveDashboardDeposit(Trim(ID), Trim(Request.QueryString("dealcode")), customerA, StartDateDeposit.Text, MaturityDateDeposit.Text, CDec(Trim(txtDealAmountDeposit.Text)) _
            , CDec(Trim(txtIntRateDeposit.Text)), Int(Trim(txtTenorDeposit.Text)), Instruction, dealNum, "Basic Deposit", Session("SysDate"), Trim(txtDealAccountDeposit.Text), Trim(txtRecievingDeposit.Text), Trim(Request.QueryString("currency")))

            Call GetIntBasisValue() 'Reinitialise interest days basis

        End If





        'Reset controls
        cmdCancelDeposit.Enabled = False
        cmdSaveDealDeposit.Enabled = False

        'enable controls after save
        txtDealAmountDeposit.Enabled = True
        txtIntRateDeposit.Enabled = True
        txtTenorDeposit.Enabled = True
        ' txtTaxRate.Enabled = True
        chkTaxStatus.Enabled = True
        chkTaxStatus.Checked = False
        cmbInstructionsDeposit.Enabled = True
        txtOtherInsDeposit.Enabled = True
        'Clear control contents
        txtDealAmountDeposit.Text = ""
        txtIntRateDeposit.Text = ""
        txtTenorDeposit.Text = ""
        'txtTaxRate.Text = "0"
        '' cmbInstructionsDeposit.Text = ""
        txtOtherInsDeposit.Text = ""
        txtMaturityAmountDeposit.Text = ""
        txtGrossInt.Text = ""
        txtNetIntDeposit.Text = ""
        txtTaxAmountDeposit.Text = ""
        StartDateDeposit.Text = CDate(Session("SysDate"))
        MaturityDateDeposit.Text = CDate(Session("SysDate"))
        'cmbCustomer.Text = ""

        'Clear control contents
        txtDealAmountDeposit.Text = ""
        txtIntRateDeposit.Text = ""
        txtTenorDeposit.Text = ""
        'txtTaxRate.Text = "0"
        'cmbInstructionsDeposit.Text = "0"
        txtOtherInsDeposit.Text = ""
        txtOtherInsDepositM.Text = ""
        txtMaturityAmountDeposit.Text = ""
        txtGrossInt.Text = ""
        txtNetIntDeposit.Text = ""
        txtTaxAmountDeposit.Text = ""
        StartDateDeposit.Text = CDate(Session("SysDate"))
        MaturityDateDeposit.Text = CDate(Session("SysDate"))
        'cmbCustomer.SelectedValue = "0"
        'txtCustomerNameDeposit.Text = ""
        'lstSecurityList.Items.Clear()

        'btnRollOver.Enabled = True
        'btnPledge.Enabled = True
        'RadioDate.Checked = False
        'RadioDays.Checked = False
        txtDealAccountDeposit.Text = ""
        txtRecievingDeposit.Text = ""
        'lstSecurityList.Items.Clear()
        'cmbSecureDeposit.Enabled = True
        'CheckPledge.Enabled = True
        chkRollover.Enabled = True
        'lstHistDeposit.Items.Clear()
        'btnAvgRateDeposit.Text = "avg rate"
        'btnAvgSizeDeposit.Text = "avg size"
        'btnAvgTenorDeposit.Text = "avg tenor"

        'lblTitleDeposit.Text = ""
        'lblFullNameDeposit.Text = ""
        'lblIDNumberDeposit.Text = ""
        'addL1Deposit.Text = ""
        'addL2Deposit.Text = ""
        'addL3Deposit.Text = ""
        'addL4Deposit.Text = ""
        'addL5Deposit.Text = ""
        'lblCustEmailDeposit.Text = ""
        'txtTotalSecurityDeposit.Text = ""
        'txtDealAmountDeposit.BackColor = Color.White
        'txtIntRateDeposit.BackColor = Color.White
        'txtTenorDeposit.BackColor = Color.White
        'txtTaxRate.BackColor = Color.White
    End Sub
    'Retrieves value for setting the settlement instructions on deal capture
    Private Function GetSettlementDetailsPar() As Boolean

        Dim x As String = ""

        Dim cnSQLf As SqlConnection
        Dim cmSQLf As SqlCommand
        Dim drSQLf As SqlDataReader
        Dim strSQLf As String

        Try
            strSQLf = "select [value] from systemparameters where parameter='settle'"

            cnSQLf = New SqlConnection(Session("ConnectionString"))
            cnSQLf.Open()
            cmSQLf = New SqlCommand(strSQLf, cnSQLf)
            drSQLf = cmSQLf.ExecuteReader()

            Do While drSQLf.Read
                x = Trim(drSQLf.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQLf.Close()
            cnSQLf.Close()
            cmSQLf.Dispose()
            cnSQLf.Dispose()

            If x = "Y" Then
                Return True
            Else
                Return False
            End If

        Catch ex As SqlException

            'Log the event *****************************************************

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetIntBasisValue", "error")

            '************************END****************************************
        End Try
    End Function
    Protected Sub btnIntDaysBasisDeposit_Click(sender As Object, e As EventArgs) Handles btnIntDaysBasisDeposit.Click

    End Sub
    Protected Sub txtRecievingDeposit_TextChanged(sender As Object, e As EventArgs) Handles txtRecievingDeposit.TextChanged

    End Sub
    Protected Sub txtDealAmountDeposit_TextChanged(sender As Object, e As EventArgs) Handles txtDealAmountDeposit.TextChanged
        txtMaturityAmountDeposit.Text = Format(CDec(txtMaturityAmountDeposit.Text), "###,###,###.00").ToString
    End Sub
    Protected Sub txtMaturityAmountDeposit_TextChanged(sender As Object, e As EventArgs) Handles txtMaturityAmountDeposit.TextChanged
        txtMaturityAmountDeposit.Text = Format(CDec(txtMaturityAmountDeposit.Text), "###,###,###.00").ToString
    End Sub
    Protected Sub cmbCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCustomer.SelectedIndexChanged
        Call GetHist(cmbCustomer.SelectedValue.ToString(), "Basic Deposit", "USD")
        Call GetCustomerAddressInfo(cmbCustomer.SelectedValue.ToString())
    End Sub

    Protected Sub cmbSecurity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSecurity.SelectedIndexChanged
        ''MsgBox(cmbSecurity.SelectedValue.ToString())
        Try
            'validate username first
            strSQL = "select * from COLLATERAL_ITEMS where collateralReference='" & Trim(Get_TB(cmbSecurity.SelectedValue.ToString())) & " ' and collateralapproved='Y'" & _
            " and expired='N' and collateralCancelled='N' "



            'strSQL = "select * from securitypurchase join deals_live on securitypurchase.tb_id=deals_live.TB_ID  where securitypurchase.tb_id ='" & _
            'ListTbs.FocusedItem.Text & "' and deals_live.TB_ID ='" & ListTbs.FocusedItem.Text & "' and deals_live.othercharacteristics='Discount Purchase' "
            MsgBox("dcfhdf")

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                txtCollCurrency.Text = (drSQL.Item("collateralCurrency").ToString)
                txtValue.Text = Format(CDbl(drSQL.Item("collateralBankValue").ToString), "###,###.00")
                txtExpiry.Text = (Trim(drSQL.Item("collateralExpiry").ToString))
                txtCollateralLoan.Text = Format((CDbl(drSQL.Item("collateralBankValue").ToString) - GetLoanCollateral()), "###,###.00")

                If Trim(drSQL.Item("assignment").ToString) = "Full" Then
                    txtCollateralLoan.Text = txtCollateralLoan.Text
                    txtCollateralLoan.Enabled = False
                Else
                    txtCollateralLoan.Text = ""
                    txtCollateralLoan.Enabled = True
                End If

            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "Error")
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Attach Security Drop Downlist" & ex.Message, Session("serverName"), Session("client"))

        End Try

    End Sub
    Private Function GetLoanCollateral() As Decimal
        Dim cnSQL2 As SqlConnection
        Dim cmSQL2 As SqlCommand
        Dim drSQL2 As SqlDataReader
        Dim strSQL2 As String
        Dim res As Decimal = 0

        Try

            strSQL2 = "select sum(securityamount) from attachedsecurities where tb_id = '" & Trim(Get_TB(cmbSecurity.SelectedValue.ToString())) & "' and matured='N'"

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
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "GetCustomerCollateral" & ex.Message, Session("serverName"), Session("client"))
        End Try

        Return res

    End Function

    Protected Sub chkPledge_CheckedChanged(sender As Object, e As EventArgs) Handles chkPledge.CheckedChanged
        TabPanel4.Visible = True
        RefreshContracts(cmbCustomer.SelectedValue.ToString, True)
        tabMaturedDeposit.ActiveTabIndex = 3
        'MsgBox("gggg")

    End Sub

    Protected Sub chkRollover_CheckedChanged(sender As Object, e As EventArgs) Handles chkRollover.CheckedChanged
        TabPanel3.Visible = True
        tabMaturedDeposit.ActiveTabIndex = 2
    End Sub

    Protected Sub btnAddCollateralLoan_Click(sender As Object, e As EventArgs) Handles btnAddCollateralLoan.Click
        '' MsgBox("Security amount entered is not valid.", MsgBoxStyle.Critical, "Security")
        lblModalError.Text = alert("Security amount cannot be greater that amount available.", "Security")

        If CDbl(txtCollateralLoan.Text) > CDbl(txtAvailble.Text) Then
            lblModalError.Text = "hdh"
            lblError.Text = alert("Security amount cannot be greater that amount available.", "Error")

            ''MsgBox("Security amount cannot be greater that amount available.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        'Check if the amount is zero or less that zero
        If CDec(txtCollateralLoan.Text) <= 0 Then
            lblModalError.Text = alert("Security amount entered is not valid.", "Security")

            'MsgBox("Security amount entered is not valid.", MsgBoxStyle.Critical, "Security")
            Exit Sub
        End If
        MsgBox("Security amount entered is not valid.", MsgBoxStyle.Critical, "Security")
        InsertSecurity(Trim(cmbSecurity.SelectedValue.ToString), Trim(Get_TB(cmbSecurity.SelectedValue.ToString)), txtCollateralLoan.Text, txtExpiry.Text, Trim(Session("username")), Trim(cmbCustomer.SelectedValue.ToString))
        Session("ernest") = "True"
        Try
            cnSQL = New SqlConnection(Session("ConnectionString"))
            Dim htmlTable As New StringBuilder()
            Using scmd As New SqlCommand()
                scmd.Connection = cnSQL
                scmd.CommandType = CommandType.Text
                scmd.CommandText = " SELECT * FROM SECURITYTEMP  where Customer='" & Trim(cmbCustomer.SelectedValue.ToString) & "' and Dealer='" & Trim(Session("username")) & "'"

                cnSQL.Open()
                Dim articleReader As SqlDataReader = scmd.ExecuteReader()
                htmlTable.Append("<table Class='table table-striped' width='100%' id='TBSecurity'>")
                htmlTable.Append("<tr><th>Security</th><th>Deal Reference</th><th>Deal Amount</th><th>Expiry Date</th><th>Delete</th></tr>")

                While articleReader.Read()
                    htmlTable.Append("<tr>")
                    htmlTable.Append("<td>" & Convert.ToString(articleReader("security")) & "</td>")
                    htmlTable.Append("<td>" & Convert.ToString(articleReader("dealref")) & "</td>")
                    htmlTable.Append("<td>" & Convert.ToString(Format(articleReader("amount"), "###,###.00")) & "</td>")
                    htmlTable.Append("<td>" & Convert.ToString(Format(articleReader("expirydate"), "Short Date")) & "</td>")
                    htmlTable.Append("<td><a id='btnDleteSingle' onclick='DeleteUsers(" & Convert.ToString(articleReader("id")) & ")'>Delete</a></td>")
                    htmlTable.Append("</tr>")

                End While
                htmlTable.Append("</table>")
                '   lblSecurity.Text = htmlTable.ToString()
                '' lblSecurity2.Text = htmlTable.ToString()
                txtTotalSecurityDeposit.Text = GetTotalSecurity()
                articleReader.Close()
                articleReader.Dispose()
            End Using
            MsgBox(GetTotalSecurity())
            txtTotalSecurityDeposit.Text = GetTotalSecurity()

        Catch ex As Exception
            lblError.Text = alert(ex.Message, "Error")
        End Try
    End Sub
    Private Sub AttachSecurity(ByVal dealNum As String)

        Dim x As String = ""
        Dim cnSQLdd As SqlConnection
        Dim cmSQLdd As SqlCommand
        Dim drSQLdd As SqlDataReader
        Dim strSQLdd As String

        Try
            strSQLdd = "SELECT Security,DealRef,Amount FROM SECURITYTEMP  where Customer='" & Trim(cmbCustomer.SelectedValue.ToString) & "' and Dealer='" & Trim(Session("username")) & "'"
            cnSQLdd = New SqlConnection(Session("ConnectionString"))
            cnSQLdd.Open()
            cmSQLdd = New SqlCommand(strSQLdd, cnSQLdd)
            drSQLdd = cmSQLdd.ExecuteReader

            Do While drSQLdd.Read
                Call SavDls.SaveDealSecurity(dealNum, Trim(drSQLdd.Item(0)), CDec(Trim(drSQLdd.Item(2))), Trim(drSQLdd.Item(1)), Session("serverName"), Session("dataBaseName"), Session("loggedUserLog"))
            Loop

            ' Close and Clean up objects
            drSQLdd.Close()
            cnSQLdd.Close()
            cmSQLdd.Dispose()
            cnSQLdd.Dispose()


        Catch ec As Exception
            lblError.Text = alert(ec.Message, "Error")

        End Try


    End Sub
    Private Function GetTotalSecurity() As Decimal
        Dim cnSQL2 As SqlConnection
        Dim cmSQL2 As SqlCommand
        Dim drSQL2 As SqlDataReader
        Dim strSQL2 As String
        Dim res As Decimal = 0

        Try

            strSQL2 = "select sum(amount) FROM SECURITYTEMP  where Customer='" & Trim(cmbCustomer.SelectedValue.ToString) & "' and Dealer='" & Trim(Session("username")) & "'"

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

    Protected Sub btnSaveNew_Click(sender As Object, e As EventArgs) Handles btnSaveNew.Click
        Dim cnSQL5 As SqlConnection
        Dim cmSQL5 As SqlCommand
        Dim drSQL5 As SqlDataReader
        Dim strSQL5 As String
        MsgBox("sdjk")
        If Trim(cmbCustomer.Text) = "0" Then
            lblError.Text = alert("Customer not selected", "Error")
            Exit Sub
        End If

        If Trim(txtContractRefNew.Text) = "" Then
            lblError.Text = alert("Enter contract reference", "Error")
            Exit Sub
        End If

        If Trim(txtAmt.Text) = "" Then
            lblError.Text = alert("Enter contract value", "Error")
            Exit Sub
        End If

        If Trim(txtPurposeNew.Text) = "" Then
            lblError.Text = alert("Enter loan purpose", "Error")

            Exit Sub
        End If



        Try
            strSQL5 = "insert loan_pledges values('" & Trim(txtContractRefNew.Text) & "','" & Trim(cmbCustomer.SelectedValue.ToString()) & "'," & txtAmt.Text _
                      & ",'" & CDate(DateStart.Text) & "','" & CDate(MaturityDate.Text) & "','" & Trim(txtPurposeNew.Text) & "','','N') "
            cnSQL5 = New SqlConnection(Session("ConnectionString"))
            cnSQL5.Open()
            cmSQL5 = New SqlCommand(strSQL5, cnSQL5)
            drSQL5 = cmSQL5.ExecuteReader


            drSQL5.Close()
            cnSQL5.Close()
            cmSQL5.Dispose()
            cnSQL5.Dispose()

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "PLG001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " Contract Reference" & Trim(txtContractRefNew.Text) & " Customer :" & Trim(cmbCustomer.SelectedValue.ToString()) & " Contract Value " & txtAmt.Text _
                      & " Start Date " & CDate(DateStart.Text) & " Maturity Date  " & CDate(MaturityDate.Text) & "  Purpose " & Trim(txtPurposeNew.Text), Session("serverName"), Session("client"))
            '************************END****************************************


            lblWarning.Text = success("Pledge loan contract added successfully.", "Pledge loan")

            Call RefreshContracts(cmbCustomer.SelectedValue.ToString, True)

            'txtCustomerNumberNew.Text = ""
            txtContractRefNew.Text = ""
            txtAmt.Text = ""
            txtPurposeNew.Text = ""
            'txtCustomerNameNew.Text = ""

        Catch ex As SqlException
      'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Sub
    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim cnSQL5 As SqlConnection
        Dim cmSQL5 As SqlCommand
        Dim drSQL5 As SqlDataReader
        Dim strSQL5 As String

        Try
            strSQL5 = "delete loan_pledges where contractreference='" & Trim(ContractReference.Text) & "'"


            cnSQL5 = New SqlConnection(Session("ConnectionString"))
            cnSQL5.Open()
            cmSQL5 = New SqlCommand(strSQL5, cnSQL5)
            drSQL5 = cmSQL5.ExecuteReader


            drSQL5.Close()
            cnSQL5.Close()
            cmSQL5.Dispose()
            cnSQL5.Dispose()


            Call RefreshContracts(cmbCustomer.SelectedValue.ToString, True)

        Catch ex As SqlException
 'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Sub
    Public Sub RefreshContracts(custnum As String, pledged As Boolean)
        Dim cnSQL5 As SqlConnection
        Dim cmSQL5 As SqlCommand
        Dim drSQL5 As SqlDataReader
        Dim strSQL5 As String


        Try
            If custnum <> "" And pledged = True Then
                strSQL5 = "select contractreference,purpose from loan_pledges where customernumber='" & custnum & "'"
            ElseIf custnum <> "" And pledged = False Then
                strSQL5 = "select contractreference,purpose from loan_pledges where customernumber='" & custnum & "' and pledgedealreference=''"
            Else
                strSQL5 = "select contractreference,purpose from loan_pledges"
            End If

            'lstContracts.Items.Clear()
            cnSQL5 = New SqlConnection(Session("ConnectionString"))
            cnSQL5.Open()
            cmSQL5 = New SqlCommand(strSQL5, cnSQL5)
            drSQL5 = cmSQL5.ExecuteReader

            Do While drSQL5.Read
                cmbContract.Items.Add(New ListItem(Trim(drSQL5.Item(0).ToString) + " " + Trim(drSQL5.Item(1).ToString), Trim(drSQL5.Item(0).ToString)))
            Loop


            drSQL5.Close()
            cnSQL5.Close()
            cmSQL5.Dispose()
            cnSQL5.Dispose()

        Catch ex As SqlException
             'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Sub
    'Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

    'End Sub

    Protected Sub cmbContract_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbContract.SelectedIndexChanged
        Dim cnSQL5 As SqlConnection
        Dim cmSQL5 As SqlCommand
        Dim drSQL5 As SqlDataReader
        Dim strSQL5 As String

        'txtCustomerNumber.Text = ""
        'txtReference.Text = ""
        'txtContractValue.Text = ""
        'txtDealRef.Text = ""
        'lblStatus.Text = ""
        MsgBox(cmbContract.SelectedValue.ToString())
        Try
            strSQL5 = "select * from loan_pledges where contractreference='" & Trim(cmbContract.SelectedValue.ToString()) & "'"


            cnSQL5 = New SqlConnection(Session("ConnectionString"))
            cnSQL5.Open()
            cmSQL5 = New SqlCommand(strSQL5, cnSQL5)
            drSQL5 = cmSQL5.ExecuteReader

            Do While drSQL5.Read
                '' txtCustomerNumber.Text = drSQL5.Item("customernumber").ToString
                ContractReference.Text = drSQL5.Item("contractreference").ToString
                startDatePledge.Text = CDate(drSQL5.Item("startdate"))
                EndDatePledge.Text = CDate(drSQL5.Item("maturitydate"))
                txtContractValue.Text = CDec(drSQL5.Item("loanamount").ToString)
                ' txtDealRef.Text = Trim(drSQL5.Item("pledgedealreference").ToString)
                txtPurpose.Text = Trim(drSQL5.Item("purpose").ToString)

                'If drSQL5.Item("matured").ToString = "Y" Then
                '    lblStatus.Text = "Pledged Deal Matured"
                'Else
                '    lblStatus.Text = "Pledged Deal Live"
                'End If
            Loop




            drSQL5.Close()
            cnSQL5.Close()
            cmSQL5.Dispose()
            cnSQL5.Dispose()

        Catch ex As SqlException
            lblError.Text = alert(ex.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub

    Protected Sub btnContPledge_Click(sender As Object, e As EventArgs) Handles btnContPledge.Click
        Dim cnSQL5 As SqlConnection
        Dim cmSQL5 As SqlCommand
        Dim drSQL5 As SqlDataReader
        Dim strSQL5 As String

        'If MessageBox.Show("Are you sure you want to update this loan", "update loan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Forms.DialogResult.Yes Then

        'Pledged Deal

        'Check if the expirey date is not before deal start date
        If StartDateDeposit.Text > EndDatePledge.Text Then
            lblError.Text = alert("Pledge expiry date cannot be before the deal start date.", "Pledge expiry date")
            tabMaturedDeposit.ActiveTabIndex = 3
            Exit Sub
        End If

        'check if a pledge amount has been entered
        If PledgeAmt.Text = "" Then
            lblError.Text = alert("Enter the pledge amount.", "Pledge amount")
            tabMaturedDeposit.ActiveTabIndex = 3
            Exit Sub
        End If

        'check if the pledged amount is not greater than the dealamount
        If CDec(PledgeAmt.Text) > CDec(txtDealAmountDeposit.Text) Then
            lblError.Text = alert("Pledged amount cannot be greater than the deal amount..", "Pledge amount")
            tabMaturedDeposit.ActiveTabIndex = 3
            Exit Sub
        End If


        Try
            'ernest removed  pledgedealreference='" & Trim(txtDealRef.Text) & "'
            strSQL5 = "update loan_pledges set loanamount=" & txtContractValue.Text & ",startdate='" & CDate(startDatePledge.Text) & "', maturitydate='" & _
                       EndDatePledge.Text & "', purpose='" & Trim(txtPurpose.Text) & "'  where contractreference='" & Trim(ContractReference.Text) & "'"

            cnSQL5 = New SqlConnection(Session("ConnectionString"))
            cnSQL5.Open()
            cmSQL5 = New SqlCommand(strSQL5, cnSQL5)
            drSQL5 = cmSQL5.ExecuteReader


            drSQL5.Close()
            cnSQL5.Close()
            cmSQL5.Dispose()
            cnSQL5.Dispose()

            'move to next tab
            tabMaturedDeposit.ActiveTabIndex = 4
            Call RefreshContracts(cmbCustomer.SelectedValue.ToString, True)

        Catch ex As SqlException
 'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try

    End Sub

    'Protected Sub MaturityDateDeposit_TextChanged(sender As Object, e As EventArgs) Handles MaturityDateDeposit.TextChanged
    '    txtTenorDeposit.Text = Math.Abs(DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDateDeposit.Text)))
    'End Sub

    'Protected Sub txtTenorDeposit_TextChanged(sender As Object, e As EventArgs) Handles txtTenorDeposit.TextChanged
    '    MaturityDateDeposit.Text = DateAdd(DateInterval.Day, CDec(txtTenorDeposit.Text), CDate(Session("SysDate")))
    'End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click

    End Sub

    Protected Sub cmdResetDeposit_Click(sender As Object, e As EventArgs) Handles cmdResetDeposit.Click

        'Reset controls
        cmdCancelDeposit.Enabled = False
        ' btnSaveMatDeposit.Enabled = False
        cmdSaveDealDeposit.Enabled = False
        txtDealAccountDeposit.Enabled = True

        'btnValMatDeposit.Enabled = True
        chkPledge.Checked = False
        chkRollover.Checked = False

        'disable controls
        txtDealAmountDeposit.Enabled = True
        txtIntRateDeposit.Enabled = True
        txtTenorDeposit.Enabled = True
        'txtTaxRate.Enabled = True
        chkTaxStatus.Enabled = True
        cmbInstructionsDeposit.Enabled = True
        txtOtherInsDeposit.Enabled = True
        cmbInstructionsDepositM.Enabled = True
        txtOtherInsDepositM.Enabled = True
        StartDateDeposit.Enabled = True
        MaturityDateDeposit.Enabled = True
        RollBool = False
        'cmdShort.Enabled = True
        'cmbSecureDeposit.Enabled = True
        cmbtax1.Enabled = True
        'txtDealAmountDeposit.BackColor = Color.White
        'txtIntRateDeposit.BackColor = Color.White
        'txtTenorDeposit.BackColor = Color.White
        'txtTaxRate.BackColor = Color.White

        Call GetIntBasisValue()
    End Sub


    Protected Sub chkTaxStatus_CheckedChanged(sender As Object, e As EventArgs) Handles chkTaxStatus.CheckedChanged
        cmbtax1.Enabled = True
        gettaxcode()
    End Sub
    Private Sub gettaxcode()

        Try
            strSQL = "select taxcode,taxrate from rates where applicable = 'Applicable'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                cmbtax1.Items.Add(New ListItem(Trim(drSQL.Item(0).ToString) + " = " + Trim(drSQL.Item(1).ToString), Trim(drSQL.Item(1).ToString)))
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException
             'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Sub

    Private Function gettaxcode(ByVal taxrate As String)

        Try
            strSQL = "select taxcode from rates where applicable = 'Applicable' and taxRate='" & taxrate & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                Return drSQL.Item(0)
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Function

    Protected Sub continue_Click(sender As Object, e As EventArgs) Handles [continue].Click
        cmdResetDeposit.Enabled = True
        btnValidateDeposit.Enabled = True
    End Sub

    Protected Sub rolloverDays_TextChanged(sender As Object, e As EventArgs) Handles rolloverDays.TextChanged
        dtRollover.Text = DateAdd(DateInterval.Day, CDec(txtTenorDeposit.Text), CDate(Session("SysDate")))
    End Sub

    Protected Sub dtRollover_TextChanged(sender As Object, e As EventArgs) Handles dtRollover.TextChanged
        txtTenorDeposit.Text = Math.Abs(DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDateDeposit.Text)))
    End Sub

End Class
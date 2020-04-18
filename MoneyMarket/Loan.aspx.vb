
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports sys_ui
Imports mmDeal

Public Class frmMain
    Inherits System.Web.UI.Page
    Private RatesCheck As New usrfunc.usrfunc
    Private SavDls As New csvptt.csvptt
    Private checkacc As New mmDeal.DealInstructions
    Private saveDash As New mmDeal.SaveDashBoardDeal
    Dim limitsch As New usrlmt.usrlmt
    Private CustName As String ' Variable to store customer name
    Private ChangedDaysBasis As Boolean = False 'Value indicating if the days basis default has been manually changed
    Private RollBool As Boolean
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class
    Private globalvars_mmdeal As New GlobalVars.mmDeal   'instance of the userlogins class
    Public CurrencyX As String
    Dim checkMaturity As New mmDeal.DealMaturityCheck
    Dim NonBusiness As New mmDeal.DealMaturityCheck
    Dim Holidays As New mmDeal.DealMaturityCheck
    Dim objectDealNumbers As New mmDeal.DeaNumbers
    'Private dbinfo As New dbparm.dbinfo
    Private object_userlog As New usrlog.usrlog
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    'Public customerN As String 'custmer name from search
    'Public customerA As String 'customer account number from search
    Private crStatus As String 'Stores counterparty limit status 1=exceeded 2= ok
    Private InterestBasisValue As Integer 'Stores the current value for use as the interest dayas basis
    Private contactTooTip As String
    Private FundingBasic As String
    Private FundingSuffix As String
    Private intOption As Integer
    Private SecureUsingOther As Boolean
    'Private RatesCheck As New usrfunc.usrfunc
    Private WarningMessage As String = ""
    Private intAccruedToDate As Decimal
    Private daystomature As Decimal
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
    'Private serverName As String
    'Private Session("dataBaseName") As String
    '*****************************************************
    'url variables
    Private portifolioid As String
    Private portifolio As String
    Private Dash As String
    Private dealCode As String
    Private currency As String
    Private product As String

    '**********************************************

    '**********************************************

    Private Sub LoadCustomers()


        'listCustomers.Columns(1).TextAlign = HorizontalAlignment.Left
        Try
            strSQL = "select customer_Number,fullName from customer where frontoffice='Y' and len(customer_number)<=9 order by fullname"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            'listCustomers.Invoke(CType(AddressOf listCustomersClear, MethodInvoker))
            'lstnumbers.Invoke(CType(AddressOf lstnumbersclear, MethodInvoker))

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    'txtCustomerNameLoan.Items.Add(Trim(dbinfo.drSQL.Item(1).ToString) + " " + Trim(dbinfo.drSQL.Item(0).ToString))
                    cmbCustomerLoan.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
                Loop
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            '' CusDownloadStart.Suspend()

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ''Session("Security") = False
            tabLoan.ActiveTabIndex = 0
            btnSaveLoan.Enabled = False
            'If Not IsPostBack Then ' Get current ScriptManager if exist
            '    Dim ScriptManagerAjax As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            '    ' If not exist
            '    If ScriptManagerAjax Is Nothing Then
            '        ' Create one
            '        ScriptManagerAjax = New ScriptManager
            '        ' Get page form
            '        Dim oForm As HtmlForm = Me.Page.Form

            '        ' Add it at first position
            '        oForm.Controls.AddAt(0, ScriptManagerAjax)

        End If
        GetCustomerCollateral(cmbCustomerLoan.SelectedValue.ToString())



        'tabLoan.ActiveTabIndex = 0
        'End If
        StartDateLoan.Text = CDate(Session("SysDate"))
        'MaturityDateLoan.Text = CDate(Session("SysDate")).ToString
        Session("customerA") = cmbCustomerLoan.SelectedValue.ToString()
        Call GetIntBasisValue()
        If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then

            portifolioid = Request.QueryString("portifolioid")
            portifolio = Request.QueryString("portifolio")
            Dash = Request.QueryString("dash")
            dealCode = Request.QueryString("dealcode")
            currency = Request.QueryString("currency")
            product = Request.QueryString("product")
        Else
            lblError.Text = alert("Please make sure all fields are selected On the newDeal Page", "Incomplete informaton")

        End If
        lblportfolioLoan.Text = portifolio
        lblDescription.Text = product
        lblDealtypeLoan.Text = dealCode
        txtCurrency.Text = currency
        txtIntDays.Text = InterestBasisValue

        Call Deal_Instructions()
        Call LoadCustomers()

        Call load_TBs()
        'End If
    End Sub
    Protected Sub txtPresentValue_TextChanged(sender As Object, e As EventArgs) Handles txtDealAmount.TextChanged
        On Error Resume Next
        txtMaturityAmountLoan.Text = txtDealAmount.Text
    End Sub

    Protected Sub txtMaturityAmountLoan_TextChanged(sender As Object, e As EventArgs) Handles txtMaturityAmountLoan.TextChanged
        On Error Resume Next
        txtMaturityAmountLoan.Text = Format(CDec(txtMaturityAmountLoan.Text), "###,###,###.00").ToString
    End Sub

    Protected Sub txtNetInterest_TextChanged(sender As Object, e As EventArgs) Handles txtNetInterestLoan.TextChanged
        On Error Resume Next
        txtNetInterestLoan.Text = Format(CDec(txtNetInterestLoan.Text), "###,###,###.00").ToString
    End Sub

    Protected Sub txtGrossInterest_TextChanged(sender As Object, e As EventArgs) Handles txtGrossLoan.TextChanged
        On Error Resume Next
        txtGrossLoan.Text = Format(CDec(txtGrossLoan.Text), "###,###,###.00").ToString
    End Sub

    Protected Sub txtTaxAmount_TextChanged(sender As Object, e As EventArgs) Handles txtTaxAmountL.TextChanged
        On Error Resume Next
        txtTaxAmountL.Text = Format(CDec(txtTaxAmountL.Text), "###,###,###.00").ToString
    End Sub
    Protected Sub cmdExit_Click(sender As Object, e As EventArgs) Handles cmdExit.Click
        Response.Redirect("newdeal.aspx")
    End Sub
    Protected Sub cmdReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        btnIntDays.Enabled = False
        btnSaveLoan.Enabled = False
        'btnSaveMatured.Enabled = False

        txtDealAmount.Enabled = True
        txtNetInterestLoan.Enabled = True
        txtTenorLoan.Enabled = True
        startDateLoan.Enabled = True
        MaturityDateLoan.Enabled = True
        cmbInstructionLoan.Enabled = True
        txtOtherLoan.Enabled = True
        cmbInstructionMaturityL.Enabled = True
        txtOtherMaturity.Enabled = True
        'CheckBox1.Enabled = True
        RollBool = False
        'cmdShortsL.Enabled = True
        txtDealAccountLoan.Enabled = True

        btnValidate.Enabled = True
        txtDealAmount.BackColor = Color.White
        txtNetInterestLoan.BackColor = Color.White
        txtTenorLoan.BackColor = Color.White
        txtMaturityAmountLoan.BackColor = Color.White

        ChangedDaysBasis = False
    End Sub
    Protected Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        'Dim dt1 As DateTime = DateTime.Parse(StartDateLoan.Text)
        'Dim dt2 As DateTime = DateTime.Parse(MaturityDateLoan.Text)
        'Dim daystomature As Integer = (dt2 - dt1).Days
        'txtTenorLoan.Text = daystomature
        Dim calc As New mmDeal.CalculationFunctions

        Dim ID As String
       
        ID = portifolioid

        If checkMaturity.CheckBackValueThreshhold("mmbkval") < Math.Abs(DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateLoan.Text))) Then
            'MsgBox("You have exceeded maximum back value threshold", MsgBoxStyle.Exclamation)
            lblError.Text = alert("You have exceeded maximum back value threshold", "Maxmum Threshold")
            Exit Sub
        End If

        limitsch.clients = Session("client")
        limitsch.ClearVariables()
        limitsch.ConnectionString = Session("ConnectionString")
        Limitauthoriser = ""
        RefMessageDaily = ""
        RefMessageDealsize = ""
        RefMessageCounterparty = ""
        RefMessageProduct = ""
        RefMessagePortfolio = ""
        dealsizeBroken = False
        dailyBroken = False
        CounterPartyBroken = False
        ProductBroken = False
        PortfolioBroken = False

        'Request for users online
        object_userlog.SendData("REQUESTUSERS|" & Session("username"), Session("serverName"), Session("client"))

        On Error GoTo err2
        'Dim x1 As Integer
        'Dim ID As String
        ''Get selected portfolio id
        'x1 = c.SelectedIndex


        On Error GoTo err1

        Dim x As Decimal

        If getSysParam("fundrequired") = "Y" Then 'Check if the accounts are required

            If Trim(GetInterestAccount(lblDealtypeLoan.Text)) = "" Then
                'MsgBox("interest account not set for product, dealing will be disabled", MsgBoxStyle.Critical)
                lblError.Text = alert("interest account not set for product, dealing will be disabled", "Interest Account Not Set")
                Exit Sub
            End If

            If Trim(txtFundingLoan.Text) = "" Then
                'MsgBox("Please select Product Deal Account", MsgBoxStyle.Critical)
                lblError.Text = alert("Please select Product Deal Account", "Product Deal Account")
                'TabControl1.SelectedTab = TabControl1.TabPages(0)
                tabLoan.ActiveTab = TabPanel1
                Exit Sub
            End If

            If Trim(txtDealAccountLoan.Text) = "" Then
                'MsgBox("Please select customer Recieving Account", MsgBoxStyle.Critical)
                lblError.Text = alert("Please select customer Recieving Account", "Recieving Account")
                tabLoan.ActiveTab = TabPanel1
                Exit Sub
            End If


            If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDateLoan.Text)) < 0 Then
                'MsgBox("Maturity date cannot be less than business date", MsgBoxStyle.Information)
                lblError.Text = alert("Maturity date cannot be less than business date", "Maturity Date")
                tabLoan.ActiveTab = TabPanel2
                Exit Sub
            End If

            If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDateLoan.Text)) = 0 Then
                'MsgBox("Maturity date cannot be the same as deal start date.", MsgBoxStyle.Information)
                lblError.Text = alert("Maturity date cannot be the same as deal start date", "Maturity Date")
                tabLoan.ActiveTab = TabPanel2
                Exit Sub
            End If

            If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateLoan.Text)) > 0 Then
                'MsgBox("Start date cannot be greater than business date", MsgBoxStyle.Information)
                lblError.Text = alert("Start date cannot be greater than business date", "Start Date")
                tabLoan.ActiveTab = TabPanel2
                Exit Sub
            End If

            If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDateLoan.Text)) = 0 Then
                'MsgBox("Tenor cannot be zero.", MsgBoxStyle.Information)
                lblError.Text = alert("Tenor cannot be zero", "Tenor")
                tabLoan.ActiveTab = TabPanel2
                Exit Sub
            End If

            If checkacc.checkDealAccount() = "Y" Then
                If txtDealAccountLoan.Text = "" Then
                    'MsgBox("Deal account not selected", MsgBoxStyle.Exclamation, "Warning")
                    lblError.Text = warning("Deal account not selected", "Deal Account")
                End If
            End If
            'check fields
            If lblportfolioLoan.Text = "" Then
                'MsgBox("Select portfolio.", MsgBoxStyle.Critical, "Incomplete information")
                lblError.Text = alert("Select portfolio", "Incomplete information")
                tabLoan.ActiveTab = TabPanel1
                Exit Sub
            End If
            If lblDealtypeLoan.Text = "" Then
                'MsgBox("Select the product.", MsgBoxStyle.Critical, "Incomplete information")
                lblError.Text = alert("Select the product.", "Incomplete information")
                tabLoan.ActiveTab = TabPanel1
                Exit Sub
            End If
            If txtCurrency.Text = "" Then
                'MsgBox("Select Currency for deal.", MsgBoxStyle.Critical, "Currency")
                lblError.Text = alert("Select Currency for deal.", "Currency")
                tabLoan.ActiveTab = TabPanel1
                Exit Sub
            End If
            If cmbCustomerLoan.Text = "" Then
                'MsgBox("Select Customer.", MsgBoxStyle.Critical, "Incomplete information")
                lblError.Text = alert("Select Customer", "Incomplete information")
                tabLoan.ActiveTab = TabPanel1
                Exit Sub
            End If

            If txtDealAmount.Text = "" Then
                'MsgBox("Enter deal amount.", MsgBoxStyle.Critical, "Incomplete information")
                lblError.Text = alert("Enter deal amount.", "Incomplete information")
                tabLoan.ActiveTab = TabPanel2
                Exit Sub
            End If

            If txtInterestRateLoan.Text = "" Then
                'MsgBox("Enter the interest rate", MsgBoxStyle.Critical, "Incomplete information")
                lblError.Text = alert("Enter the interest rate", "Incomplete information")
                tabLoan.ActiveTab = TabPanel2
            End If
            If txtTenorLoan.Text = "" Then
                'MsgBox("Specify loan tenor.", MsgBoxStyle.Critical, "Incomplete information")
                lblError.Text = alert("Specify loan tenor.", "Incomplete information")
                tabLoan.ActiveTab = TabPanel2
                Exit Sub
            End If
            If cmbInstructionLoan.Text = "" Then
                'MsgBox("Enter instruction for deal inception.", MsgBoxStyle.Information, "Incomplete information")
                lblError.Text = alert("Enter instruction for deal inception.", "Incomplete information")
                tabLoan.ActiveTab = TabPanel5
                Exit Sub
            End If
            If cmbInstructionLoan.Text = "Other" And txtOtherLoan.Text = "" Then
                'MsgBox("Please enter other instructions for deal inception.", MsgBoxStyle.Information)
                lblError.Text = alert("Please enter other instructions for deal inception.", "Incomplete information")
                tabLoan.ActiveTab = TabPanel5
                Exit Sub
            End If

            If cmbInstructionMaturityL.Text = "" Then
                'MsgBox("Enter instruction for deal maturity.", MsgBoxStyle.Information, "Incomplete information")
                lblError.Text = alert("Enter instruction for deal maturity.", "Incomplete information")
                tabLoan.ActiveTab = TabPanel5
                Exit Sub
            End If
            If cmbInstructionMaturityL.Text = "Other" And txtOtherMaturity.Text = "" Then
                'MsgBox("Please enter other instructions for deal maturity.", MsgBoxStyle.Information)
                lblError.Text = alert("Please enter other instructions for deal maturity.", "Incomplete information")
                tabLoan.ActiveTab = TabPanel5
                Exit Sub
            End If


            'Check if proposed maturity date is not a non-business day
            If NonBusiness.NonBusinessDay(CDate(MaturityDateLoan.Text)) = True Then
                'MsgBox("The selected maturity date is a not a business day.", "Non-Business Day")
                lblError.Text = alert("The selected maturity date is a not a business day.", "Non-Business Day")
                tabLoan.ActiveTab = TabPanel2
                Exit Sub
            End If
            If Holidays.Holidays(CDate(MaturityDateLoan.Text), getBaseCurrency) = True Then
                'MsgBox("The selected maturity date is a not a business day.", "Non-Business Day")
                lblError.Text = alert("The selected maturity date is a not a business day.", "Non-Business Day")
                tabLoan.ActiveTab = TabPanel2
                Exit Sub
            End If
        End If
        'check Security
      

            'Check expiry dates of the collateral assigned

            'Dim ttr As ListViewItem

            'For Each ttr In lstLoanSecurity.Items

            '    If CDate(ttr.SubItems(3).Text) < CDate(MaturityDateLoan.Value) Then
            '        ttr.BackColor = Color.Red
            '        ttr.ForeColor = Color.White

            '        'MsgBox("Collateral item ref: " & Trim(ttr.Text) & " Expires before deal matures", MsgBoxStyle.Information)
            '        alert("Collateral item ref: " & Trim(ttr.Text) & " Expires before deal matures", " Information")
            '    End If

            'Next



        '*****************End

        If ChangedDaysBasis = True Then
            Call calc.CalcEngine1(CDec(txtTenorLoan.Text), CDec(txtInterestRateLoan.Text), CDec(txtDealAmount.Text), CDec(txtTaxRateL.Text), InterestBasisValue)
        Else
            Call calc.CalcEngine(CDec(txtTenorLoan.Text), CDec(txtInterestRateLoan.Text),
            CDec(txtDealAmount.Text), CDec(txtTaxRateL.Text), Trim(currency))
            calc.IntBasis(Trim(currency))
        End If


        txtMaturityAmountLoan.Text = calc.maturityAmount.ToString
        txtNetInterestLoan.Text = calc.netInt.ToString
        txtTaxAmountL.Text = calc.taxAmount
        txtGrossLoan.Text = calc.grossInt

        x = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateLoan.Text), )
        If x < 0 Then
            x = Math.Abs(x)
            daystomature = CDec(txtTenorLoan.Text) - x
            calc.CalcEngine(x, CDec(txtInterestRateLoan.Text), CDec(txtDealAmount.Text), 0, Trim(currency))
            intAccruedToDate = Session("intAccruedToDateCal")
        Else
            intAccruedToDate = 0
            daystomature = CDec(txtTenorLoan.Text)
        End If


        '-------------------------------------
        're-initialize variablse
        calc.netInt = 0
        calc.maturityAmount = 0
        calc.taxAmount = 0
        calc.grossInt = 0
        calc.DiscountRateDerived = 0
        calc.YieldRateDerived = 0
        '-------------------------------------



        txtDealAmount.Enabled = False
        txtInterestRateLoan.Enabled = False
        txtTenorLoan.Enabled = False
        StartDateLoan.Enabled = False
        MaturityDateLoan.Enabled = False
        cmbInstructionLoan.Enabled = False
        txtOtherLoan.Enabled = False
        cmbInstructionMaturityL.Enabled = False
        txtOtherMaturity.Enabled = False
        txtTaxRateL.Enabled = False
        cmbCustomerLoan.Enabled = False
        'CheckBox1.Enabled = False
        'cmdShortsL.Enabled = False

        txtDealAmount.BackColor = Color.LightYellow
        txtInterestRateLoan.BackColor = Color.LightYellow
        txtTenorLoan.BackColor = Color.LightYellow
        txtMaturityAmountLoan.BackColor = Color.LightYellow


        ''************************************************************************

        'Check rate for deal '************************************************************************
        RatesCheck.StartApplication(Session("serverName"), Session("dataBaseName"), Session("loggedUserLog"), Session("client"))
        Dim response As String
        Dim res As String()
        response = RatesCheck.SysRates("D", txtTenorLoan.Text, CDec(txtInterestRateLoan.Text))
        res = Split(response, "|")
        Select Case res(0)
            Case "OK"

                'Case "WARN"
                '    If MessageBox.Show(res(1) & ". Continue with rate.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then
                '        Exit Sub
                '    Else
                '        WarningMessage = res(1)
                '    End If

            Case "ERROR"
                MsgBox(res(1), MsgBoxStyle.Critical)
                Exit Sub
        End Select

        '************************************************************************
        'Check Limits
        'Check Dealer Daily limit
        If UCase(Trim(Dash)) = "FALSE" Then 'Check limits only if the deal is not on the dash board

            'Check Trnasaction Limits if implemented this function will return 1 to indicate that 
            'limits have been exceeded and 2 to indicate that transaction is within the limit
            '0 means Limit checking not implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount

            TranxLimitVal = limitsch.PeriodLimitValidation(Int(txtTenorLoan.Text), "LoanLimit", Session("loggedUserLog"), _
                            CDbl(txtDealAmount.Text), Trim(currency), Trim(lblDealtypeLoan.Text))

            If TranxLimitVal(0) = "1" Then
                lblError.Text = alert("Transaction Limit exceeded" & vbCrLf & "****************************************************" & vbCrLf & _
                    TranxLimitVal(1) & vbCrLf & "****************************************************" & vbCrLf & "Limit Amount             :   " & Format(CDbl(TranxLimitVal(2)), "###,###.00") & vbCrLf & "Transaction Amount :   " & txtDealAmount.Text, "Limit")


                If getSysParam("tranlmt") = "warn" Then
                    lblwarning.Text = warning("Limit violation will be recorded", "Transaction Limit exceeded")
                ElseIf getSysParam("tranlmt") = "stop" Then
                    MsgBox("Transaction Limit exceeded, you cannot proceed with the transaction", MsgBoxStyle.Critical, "Transaction Limit exceeded")
                    Exit Sub
                End If
            End If

            'Counterparty limits validation
            If limitsch.IsLimitSet(Session("customerA"), "Loan") = True Then
                If limitsch.CheckCounterpartyLimit(Session("customerA"), Session("DealStructure"), CDec(txtDealAmount.Text), Trim(currency), CDate(MaturityDateLoan.Text)) = False Then

                    crStatus = "1"
                    If CounterpartyLimitViolation(CDec(txtDealAmount.Text), Session("DealStructure"), currency, CDate(MaturityDateLoan.Text)) = "stop" Then
                        Exit Sub
                    End If
                Else
                    crStatus = "2"
                End If
            Else
                crStatus = "3"
            End If


            CustName = cmbCustomerLoan.Text
            If limitsch.CheckDailyLimit(Session("loggedUser"), Trim(lblDealtypeLoan.Text), CDec(txtDealAmount.Text), Trim(lblDescription.Text), Trim(currency)) = False Or _
               limitsch.CheckdealSizeLimit(Session("loggedUser"), Trim(lblDealtypeLoan.Text), CDec(txtDealAmount.Text), Trim(lblDescription.Text), Trim(currency)) = False Or _
               limitsch.CheckProductPos(Trim(lblDealtypeLoan.Text), CDec(txtDealAmount.Text), "-", Trim(currency)) = False Or _
               limitsch.CheckPortfolioPos(ID, CDec(txtDealAmount.Text), "-", Trim(currency)) = False Then



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

                'If MessageBox.Show("Limits have been exceeded. Do you want to send a referal?", "Limits", MessageBoxIcon.Exclamation) = DialogResult.Yes Then

                'Dim ref As New Referal
                'ref.RefMessageDealsize = limitsch.RefMessageDealsize
                'ref.RefMessageDaily = limitsch.RefMessageDaily
                'ref.refAmount = limitsch.refAmount
                'ref.RefDealType = limitsch.RefDealType
                'ref.Limitauthoriser = limitsch.Limitauthoriser
                'ref.RefMessagePortfolio = limitsch.RefMessagePortfolio
                'ref.RefMessageProduct = limitsch.RefMessageProduct
                'ref.RefMessageCounterparty = limitsch.RefMessageCounterparty

                'ref.DealCustomer = Trim(cmbCustomerLoan.Text)
                'ref.DealDealAmount = CDec(txtDealAmount.Text)
                'ref.DealDealInterestRate = Trim(txtInterestRateLoan.Text)
                'ref.DealTenor = Int(txtTenorLoan.Text)
                'ref.DealMaturityAmount = CDec(txtMaturityAmountLoan.Text)
                'ref.dealGrossInterest = CDec(txtGrossLoan.Text)
                'ref.dealPortfolio = Trim(lblportfolioLoan.Text)
                'ref.dealDealType = Trim(lblDealtypeLoan.Text)
                'ref.DealCurrency = Trim(currency)
                'ref.DealTransType = "Basic Loan"
                'ref.CustName = customerN

                '    ref.ShowDialog()

                '    Limitauthoriser = ref.Limitauthoriser

                '    ref.Close()

                '    'If btnstatus = "N" Then Exit Sub 'if Dont send referal
                '        If referalResponse <> 1 Then
                '            referalResponse = 3
                '            Exit Sub 'response was no
                '        End If
                '    Else
                '        'Get the refID
                '        Exit Sub
                '    End If
                '    referalResponse = 3 're-initialise variable
            End If
        End If
        '************************************************************************************

        'refIDRecieve = refid
        'tabLoan.ActiveTab = TabPanel2
        ''TabControl1.SelectedTab = TabControl1.TabPages(1)
        'cmdSaveLoan.Enabled = True
        'Get ref id for this transaction

        btnSaveLoan.Enabled = True
        tabLoan.ActiveTabIndex = 1
        lblsuccess.Text = success("Deal Testing Complete  <br> Dealer Can Procced To Deal", "Deal Validation")
        Exit Sub
err1:
        Dim ex, ex3 As String
        ex3 = Err.GetException.Message
        ex = Err.GetException.Message

        'Log the event *****************************************************
        object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex3 & " " & ex, Session("serverName"), Session("client"))
        '************************END****************************************

        MsgBox(ex3 & " " & ex, MsgBoxStyle.Critical, "Error")

        Exit Sub

err2:
        'lblsuccess.Text = success("Deal ValidatedSuccessfully.You can now Deal", "Deal Validated")
    End Sub

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
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " CheckBaseCurrency", Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function
   
    Private Sub cmdDealtypeLoan_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDealtypeLoan.Load
        Dim limitsch As New usrlmt.usrlmt
        limitsch.clients = Session("client")
        limitsch.ConnectionString = Session("ConnectionString")

        Call limitsch.getDealTypeStructure(Trim(lblDealtypeLoan.Text))
        Session("DealStructure") = limitsch.DealStructure

    End Sub
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
            MsgBox(ec.Message, MsgBoxStyle.Information)

        End Try

        Return Trim(x)

    End Function
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

        End Try

        Return intAccount
    End Function

    Protected Sub cmdSaveLoan_Click(sender As Object, e As EventArgs) Handles btnSaveLoan.Click
        'Dim dt1 As DateTime = DateTime.Parse(StartDateLoan.Text)
        'Dim dt2 As DateTime = DateTime.Parse(MaturityDateLoan.Text)
        'Dim daystomature As Integer = CDec(txtTenorLoan.Text)
        'Dim x As Integer
        Dim ID As String
        crStatus = "3"
        TranxLimitVal = limitsch.PeriodLimitValidation(Int(txtTenorLoan.Text), "LoanLimit", Session("loggedUserLog"), _
                          CDbl(txtDealAmount.Text), Trim(currency), Trim(lblDealtypeLoan.Text))
        'x = c.SelectedIndex
        ID = portifolioid
        '************************************************************************



        '************************************************************************************


        Dim Instruction As String = ""
        Dim InstructionM As String = ""

        If Trim(cmbInstructionLoan.Text) = "Other" Then
            Instruction = Replace(Trim(txtOtherLoan.Text), "’", "")
            Instruction = Replace(Trim(txtOtherLoan.Text), "'", "")
            Instruction = Replace(Trim(txtOtherLoan.Text), "&", "and")
        Else
            Instruction = Replace(Trim(cmbInstructionLoan.Text) & " -" & Trim(txtOtherLoan.Text), "’", "")
            Instruction = Replace(Trim(cmbInstructionLoan.Text) & " -" & Trim(txtOtherLoan.Text), "'", "")
            Instruction = Replace(Trim(cmbInstructionLoan.Text) & " -" & Trim(txtOtherLoan.Text), "&", "and")
        End If

        If Trim(cmbInstructionMaturityL.Text) = "Other" Then
            InstructionM = Replace(Trim(txtOtherMaturity.Text), "’", "")
            InstructionM = Replace(Trim(txtOtherMaturity.Text), "'", "")
            InstructionM = Replace(Trim(txtOtherMaturity.Text), "&", "and")
        Else
            InstructionM = Replace(Trim(cmbInstructionMaturityL.Text) & " -" & Trim(txtOtherMaturity.Text), "’", "")
            InstructionM = Replace(Trim(cmbInstructionMaturityL.Text) & " -" & Trim(txtOtherMaturity.Text), "'", "")
            InstructionM = Replace(Trim(cmbInstructionMaturityL.Text) & " -" & Trim(txtOtherMaturity.Text), "&", "and")
        End If

        Dim dealNum As String = ""
        Dim Secured As String

        'get deal number
        'if deal is not the dashboard
        If UCase(Trim(Dash)) = "FALSE" Then
            dealNum = objectDealNumbers.GetDealNumber(Trim(lblDealtypeLoan.Text))
        Else
            dealNum = objectDealNumbers.GetDealNumberDash(Trim(lblDealtypeLoan.Text))
        End If


        'Check if the dealreference is ok
        If Trim(dealNum) = "" Then
            'MsgBox(DealRefError, MsgBoxStyle.Critical, "Deal reference not generated")
            lblError.Text = alert("Deal reference not generated", "DealRefError")
            Exit Sub
        End If


        'save deal security if any
        If Session("ernest") = True Then
            AttachSecurity(dealNum)
            Secured = "Y"

            'Else 'secured using other collateral
            ' AttachSecurity(dealNum)

            '    secured = "Y"
        End If

        If Session("ernest") = True Then
            If txtTotalSecurityLoan.Text = "" Then txtTotalSecurityLoan.Text = "0"
            If CDbl(txtTotalSecurityLoan.Text) < CDbl(txtTotalSecurityLoan.Text) Then
                lblError.Text = alert("Insuficient Security for this deal", "Security")
                tabLoan.ActiveTabIndex = 1
            End If
        End If

        'save deal
        'Determine if deal is on dashboard or not
        'if deal is not the dashboard

        If UCase(Trim(Dash)) = "FALSE" Then

            ' 1 to indicate that limits have been exceeded and 
            ' 2 to indicate that transaction is within the limit
            ' 0 means Limit checking not implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount
            'msg(3) = Limit period
            Dim limitsch As New usrlmt.usrlmt
            limitsch.clients = Session("client")
            limitsch.ConnectionString = Session("ConnectionString")




            'Save counterparty limit information and transaction period limits detail here
            'limitsch.SaveCounterpartyLimitsDetails(Trim(Session("loggedUser")), dealNum, CDbl(txtDealAmount.Text), GetFieldVal(Session("customerA"), "limit", "Loan") _
            '  , CDate(Session("SysDate")), crStatus, Trim(currency), "L", Session("customerA"), Int(TranxLimitVal(3)), Trim(TranxLimitVal(1)), CDbl(TranxLimitVal(2)), _
            '      Int(TranxLimitVal(0)), " ")


        

            'SavDls.clients = Session("client")
            SavDls.strCurrentD = clientlogin_vars.strCurrentDirectory
            'SavDls.dataBaseName = Session("dataBaseName")
            Call SavDls.saveLoan(CDec(txtGrossLoan.Text), CDec(txtTaxAmountL.Text), daystomature, Session("SysDate"), RequireFrontAuthoriser, Trim(ID), Trim(lblDealtypeLoan.Text), _
            Session("customerA"), CDate(StartDateLoan.Text), CDate(MaturityDateLoan.Text), CDec(Trim(txtDealAmount.Text)), CDec(Trim(txtMaturityAmountLoan.Text)), CDec(Trim(txtInterestRateLoan.Text)) _
            , CDec(Trim(txtNetInterestLoan.Text)), Int(Trim(txtTenorLoan.Text)), Instruction, dealNum, "Basic Loan", Secured, intAccruedToDate, Trim(currency) _
            , Session("IsDealer"), InterestBasisValue, Session("serverName"), Session("dataBaseName"), Session("loggedUserLog"), globalvars_mmdeal.PrintPages, InstructionM, WarningMessage, Trim(txtDealAccountLoan.Text))




            'if deal is from dashboard the set it to expired
            Call ExpireDashDeal(Session("username"), Trim(txtDashRef.Text))
            intAccruedToDate = 0         'Reinitialise interest accrued to date

            'if the deal is a roll from matured
            If RollBool = True Then
                Call ExpireRollDeal(Trim(txtRollRef.Text), dealNum)
            End If

            Call GetIntBasisValue() 'Reinitialise interest days basis


            'Apply Default Settlement Details

            tdsdebitAccount = getDealAccountTDS(Trim(lblDealtypeLoan.Text), "FUN")
            tdsCreditAccount = Trim(lblDealtypeLoan.Text) & "-" & Trim(Session("customerA")) 'getDealAccountTDS(Trim(cmdDealtypeLoan.Text), "DAL")
            tdscommissionAccount = getDealAccountTDS(Trim(lblDealtypeLoan.Text), "COM")
            tdsinterestaccount = getDealAccountTDS(Trim(lblDealtypeLoan.Text), "INT")
            ethixdebitAccount = Trim(txtFundingLoan.Text) 'getDealAccountEthix(tdsdebitAccount)
            ethixCreditAccount = Trim(txtDealAccountLoan.Text) 'getDealAccountEthix(tdsCreditAccount)
            ethixcommissionAccount = getDealAccountEthix(tdscommissionAccount)
            ethixinterestaccount = getDealAccountEthix(tdsinterestaccount)
            EthixAccrualDebit = getEthixAccrualAccount(Trim(lblDealtypeLoan.Text), "debitAccount")
            EthixAccrualCredit = getEthixAccrualAccount(Trim(lblDealtypeLoan.Text), "creditAccount")
            'Apply Default Settlement Details

            SetDefaultSettlementAccounts(dealNum, tdsdebitAccount, tdsCreditAccount, _
            tdsinterestaccount, tdscommissionAccount, ethixdebitAccount, ethixCreditAccount, _
            ethixinterestaccount, ethixcommissionAccount, EthixAccrualDebit, EthixAccrualCredit)


            ''Set settlement instructions / accounts if true
            'If GetSettlementDetailsPar() = True Then
            '    Dim settlem As New frmSettlement
            '    settlem.seqq = "1"
            '    settlem.typOFDeal.Text = "MM"
            '    settlem.lblReference.Text = dealNum
            '    settlem.ShowDialog()
            'End If


        Else
            'save the dashboard deal
            saveDash.saveDashboardLoan(Trim(ID), Trim(lblDealtypeLoan.Text), Session("customerA"), StartDateLoan.Text, MaturityDateLoan.Text, CDec(Trim(txtDealAmount.Text)) _
            , CDec(Trim(txtInterestRateLoan.Text)), Int(Trim(txtTenorLoan.Text)), Instruction, dealNum, "Basic Loan", Session("SysDate"), Trim(currency), Trim(txtDealAccountLoan.Text), Trim(txtFundingLoan.Text))
        End If

        'save information about broken limits
        'Dim limitsch As New usrlmt.usrlmt
        If dealsizeBroken = True Or dailyBroken = True Or PortfolioBroken = True Or ProductBroken = True Or CounterPartyBroken = True Then

            limitsch.clients = Session("client")
            limitsch.ConnectionString = Session("ConnectionString")
            Call limitsch.LimitBroken(dealNum, refIDRecieve)
        End If


        'Reset controls
        btnIntDays.Enabled = False
        btnSaveLoan.Enabled = False

        txtDealAmount.Enabled = True
        txtNetInterestLoan.Enabled = True
        txtTenorLoan.Enabled = True
        StartDateLoan.Enabled = True
        MaturityDateLoan.Enabled = True
        cmbInstructionLoan.Enabled = True
        cmbInstructionMaturityL.Enabled = True
        txtOtherLoan.Enabled = True
        'CheckBox1.CheckState = CheckState.Unchecked
        'CheckBox1.Enabled = True

        txtOtherLoan.Text = ""
        txtOtherMaturity.Text = ""
        txtDealAmount.Text = ""
        txtNetInterestLoan.Text = ""
        txtNetInterestLoan.Text = ""
        'cmbSecureLoan.Text = ""
        txtTenorLoan.Text = ""
        txtGrossLoan.Text = ""
        StartDateLoan.Text = CDate(Session("SysDate")).ToString
        MaturityDateLoan.Text = CDate(Session("SysDate")).ToString
        'cmbCustomerLoan.Text = ""
        'txtRef.Text = ""
        'txtAmt.Text = ""
        txtDealAccountLoan.Text = ""
        txtFundingLoan.Text = ""
        txtTotalSecurityLoan.Text = "0"

        'ListHistLoans.Items.Clear()
        'btnAvgRateLoan.Text = "avg rate"
        'btnAvgSizeLoan.Text = "avg size"
        'btnAvgTenorLoan.Text = "avg tenor"

        txtDealAmount.BackColor = Color.White
        txtNetInterestLoan.BackColor = Color.White
        txtTenorLoan.BackColor = Color.White
        txtMaturityAmountLoan.BackColor = Color.White


        'Determine if deal is on dashboard or not
        'if deal is not the dashboard
        If UCase(Trim(Dash)) = "FALSE" Then

            'If SetEmailNotifications() = True Then 'Checks if the parameter is defined for this action
            '    'Setup deal notifications
            '    If MessageBox.Show("Do you want to setup notifications for this deal?", "Notifications", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
            '        Dim notify As New Notifications
            '        notify.lblRef.Text = Trim(dealNum)
            '        notify.btnfind.Visible = False
            '        notify.txtSearch.Visible = False
            '        notify.GetEmailAddress(loggedUser, Trim(customerA))
            '        notify.ShowDialog()
            '    End If
        End If
        lblsuccess.Text = success("Deal saved.", "Deal saved")
        '

        Response.AddHeader("REFRESH", "2;URL=newdeal.aspx")

    End Sub
    'Returns the requested field as specified for the returnval parameter
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
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try

        Return x

    End Function
    Private Sub SavePlacementCollateral(ByVal dealref As String)
       
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
                Dim res1 As String = drSQLX1.Item(0).ToString
                If Trim(res1) = "Y" Then
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
    Public Function ExpireDashDeal(ByVal dealer As String, ByVal ref As String)
        Try
            'Save deal and the record for online processing
            strSQL = "update dealsdashboard set expired='Y' where dealreference='" & ref & "' and dealcapturer = '" & Session("username") & "' "

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
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Function

    Public Function ExpireRollDeal(ByVal ref As String, newref As String)
        Dim cnSQL2 As SqlConnection
        Dim cmSQL2 As SqlCommand
        Dim drSQL2 As SqlDataReader
        Dim strSQL2 As String

        Try
            'Save deal and the record for online processing
            strSQL2 = "update deals_matured set dealcancelled='R' where dealreference='" & ref & "'  "

            cnSQL2 = New SqlConnection(Session("ConnectionString"))
            cnSQL2.Open()
            cmSQL2 = New SqlCommand(strSQL2, cnSQL2)
            drSQL2 = cmSQL2.ExecuteReader()

            ' Close and Clean up objects
            drSQL2.Close()
            cnSQL2.Close()
            cmSQL2.Dispose()
            cnSQL2.Dispose()

            If ref <> "" Then
                'Save deal and the record for online processing
                strSQL2 = "insert RollHist values('" & ref & "','" & newref & "') "

                cnSQL2 = New SqlConnection(Session("ConnectionString"))
                cnSQL2.Open()
                cmSQL2 = New SqlCommand(strSQL2, cnSQL2)
                drSQL2 = cmSQL2.ExecuteReader()

                ' Close and Clean up objects
                drSQL2.Close()
                cnSQL2.Close()
                cmSQL2.Dispose()
                cnSQL2.Dispose()
            End If

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Function
    'Retrieves the interest days basis value
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
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetIntBasisValue", Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub
    'Returns account using dealcode and apptype
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
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

        Return x
    End Function


    'Returns account using dealcode and apptype
    Private Function getDealAccount(ByVal dealcode As String, ByVal typ As String) As String
        Dim x As String = ""
        Try
            'validate username first
            strSQL = "select EthixAccount from BRDEFAULTS where dealcode='" & Trim(dealcode) & "' and AccountType='" & typ & "' and isdefault='Y'"
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
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

        Return x
    End Function
    'Returns ethix account for relavant transaction type
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
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

        Return x
    End Function


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
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
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
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "SetDefaultSettlementAccounts -- Update / Insert", Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

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
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetIntBasisValue", Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Function
    
    Private Function SetEmailNotifications() As Boolean
        Dim x As Boolean
        Try
            strSQL = "select PromptNotifications from alertrunning "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
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

            Return x
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Function
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
                        cmbInstructionMaturityL.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                    ElseIf drSQL.Item("purpose").ToString.Equals("I") Then
                        cmbInstructionLoan.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                    End If
                Loop


            End If

            cmbInstructionLoan.Items.Add("Other")
            cmbInstructionMaturityL.Items.Add("Other")

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

    End Sub

    Protected Sub StartDateLoan_TextChanged(sender As Object, e As EventArgs) Handles StartDateLoan.TextChanged
        StartDateLoan.Text = CDate(Session("SysDate"))
    End Sub
    'Calculations for non-discounted deals where days basis must be supplied

    Public Function alert(ByVal errorstring As String, ByVal alertname As String)
        Return "<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>  <h4><i class='icon fa fa-ban'></i> " + alertname + "!</h4>" & errorstring & "</div>"
    End Function
    Public Function success(ByVal success_string As String, ByVal successname As String)
        Return "<div class='alert alert-success alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>  <h4>	<i class='icon fa fa-check'></i>" + successname + "!</h4>" & success_string & "    </div>"
    End Function
    Public Function warning(ByVal warning_string As String, ByVal warningname As String)
        Return "<div class='alert alert-warning alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>  <h4>	<i class='icon fa fa-check'></i> " + warningname + "!</h4>             " & warning_string & "    </div>"
    End Function
    Private Function CounterpartyLimitViolation(dealamt As Double, dealtype As String, curr As String, matDate As String) As String
        Dim x As String = ""
        Dim myLimit As Decimal = CDbl(GetFieldVal(Session("customerA"), "limit", dealtype))
        Dim mycumulativeTotal As Decimal = CDbl(GetFieldVal(Session("customerA"), "cumulativetotal", dealtype))

        x = limitExpires(Session("customerA"), dealtype, curr, CDate(matDate))
        If x <> "" Then
            MsgBox(x, MsgBoxStyle.Exclamation)
            x = "stop"
            ' Exit Function
        Else

            MsgBox("Counterparty Limit exceeded" & vbCrLf & _
                      "_____________________________________________________" & vbCrLf & _
                      "Excess : " & Format(myLimit - (CDbl(dealamt) + mycumulativeTotal), "###,###.00") & vbCrLf & _
                      "Limit Amount :   " & Format(myLimit, "###,###.00") & vbCrLf & _
                      "_____________________________________________________" & vbCrLf & _
                      "Cumulative Total :   " & Format(mycumulativeTotal, "###,###.00") & vbCrLf & _
                      "Transaction Amount :   " & Format(dealamt, "###,###.00"), MsgBoxStyle.Information, "Limits")
        End If

        If getSysParam("tranlmt") = "warn" Then
            MsgBox("Counterparty Limit violation will be recorded", MsgBoxStyle.Exclamation, "Counterparty Limit exceeded")
            x = "warn"
        ElseIf getSysParam("tranlmt") = "stop" Then
            MsgBox("Counterparty Limit exceeded, you cannot proceed with the transaction", MsgBoxStyle.Critical, "Counterparty Limit exceeded")
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

    Protected Sub txtTenorLoan_TextChanged(sender As Object, e As EventArgs) Handles txtTenorLoan.TextChanged

        Dim x As Long
        Try

            MaturityDateLoan.Text = DateAdd(DateInterval.Day, CDec(txtTenorLoan.Text), CDate(StartDateLoan.Text))
            If x < 0 Then
                lblError.Text = alert("Maturity date cannot be less than start date", "Maturity Date")
                MaturityDateLoan.Text = CDate(Session("SysDate"))
                MaturityDateLoan.Focus()
                Exit Sub
            End If

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub


    Protected Sub MaturityDateLoan_TextChanged(sender As Object, e As EventArgs) Handles MaturityDateLoan.TextChanged
        Dim dt1 As DateTime = DateTime.Parse(StartDateLoan.Text)
        Dim dt2 As DateTime = DateTime.Parse(MaturityDateLoan.Text)
        Dim daystomature As Integer = (dt2 - dt1).Days
        txtTenorLoan.Text = daystomature
    End Sub

    Protected Sub btnIntDays_Click(sender As Object, e As EventArgs) Handles btnIntDays.Click
        Call setBasisValue("Placement")
        btnSaveLoan.Enabled = False

    End Sub
    Private Sub setBasisValue(ByVal AppType As String)
        '    Dim intbasisval As New IntBasis
        '    intbasisval.ShowDialog()
        '    If intbasisval.cmdOK.DialogResult = Windows.Forms.DialogResult.OK Then
        '        InterestBasisValue = intbasisval.txtIntBasis.Text
        '        ChangedDaysBasis = True

        '        Select Case AppType
        '            Case "Placement"
        '                'set interest days basis for currency
        '                txtDaysBasisLoan.Text = intbasisval.txtIntBasis.Text
        '                'Case "Deposit"
        '                '    'set interest days basis for currency
        '                '    txtDaysBasisDeposit.Text = intbasisval.txtIntBasis.Text
        '                'Case "Purchase"
        '                '    'set interest days basis for currency
        '                '    txtDaysBasisPurchase.Text = intbasisval.txtIntBasis.Text
        '                'Case "Sale"
        '                '    'set interest days basis for currency
        '                '    txtDaysBasisSale.Text = intbasisval.txtIntBasis.Text

        '        End Select
        '    Else

        '    End If
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


                If IsDBNull(drSQL.Item("custage")) = False Then
                    custAge.Text = drSQL.Item("custage")
                Else
                    custAge.Text = "Not Specified"
                End If

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            lblError.Text = alert(ex.Message, "Error")
        End Try


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
            lblError.Text = alert(ex.Message, "Error")
        End Try

    End Sub

    Protected Sub cmdCancelLoan_Click(sender As Object, e As EventArgs) Handles cmdCancelLoan.Click
        cmdCancelLoan.Enabled = False
        btnSaveLoan.Enabled = False
        'btnSaveMatured.Enabled = False

        txtDealAmount.Enabled = True
        txtInterestRateLoan.Enabled = True
        txtTenorLoan.Enabled = True
        StartDateLoan.Enabled = True
        MaturityDateLoan.Enabled = True
        cmbInstructionLoan.Enabled = True
        txtOtherLoan.Enabled = True
        cmbInstructionMaturityL.Enabled = True
        txtOtherMaturity.Enabled = True
        'CheckBox1.Enabled = True
        RollBool = False
        'cmdShortsL.Enabled = True
        txtDealAccountLoan.Enabled = True

        btnValidate.Enabled = True
        txtDealAmount.BackColor = Color.White
        txtInterestRateLoan.BackColor = Color.White
        txtTenorLoan.BackColor = Color.White
        txtMaturityAmountLoan.BackColor = Color.White

        ChangedDaysBasis = False
    End Sub

    Protected Sub cmbCustomerLoan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCustomerLoan.SelectedIndexChanged
        Call GetHist(cmbCustomerLoan.SelectedValue.ToString(), "Basic Loan", "USD")
        Call GetCustomerAddressInfo(cmbCustomerLoan.SelectedValue.ToString())
    End Sub
    Private Function Get_TB(ByVal dealref As String)
        Try
            'validate username first
            strSQL = "select securitypurchase.tb_id from securitypurchase join deals_live on securitypurchase.dealreference=deals_live.dealreference" & _
                     "  where matured = 'N' and authorisationstatus='A' and securitypurchase.dealreference='" & Trim(dealref) & "' and  currency='" & Trim(Request.QueryString("currency")) & "'"
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
                     "  where matured = 'N' and authorisationstatus='A' and currency='" & Trim(currency) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                cmbTB.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(1).ToString)))
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
    Protected Sub cmbTB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTB.SelectedIndexChanged
        Try
            'validate username first
            strSQL = "select * from securitypurchase join deals_live on securitypurchase.tb_id=deals_live.TB_ID  " & _
                                   " where securitypurchase.tb_id ='" & Get_TB(cmbTB.SelectedValue.ToString()) & "' and deals_live.TB_ID ='" & Get_TB(cmbTB.SelectedValue.ToString()) & "'" & _
                                   " and securitypurchase.dealreference='" & cmbTB.SelectedValue.ToString() & "' and" & _
                                   " deals_live.dealreference='" & cmbTB.SelectedValue.ToString() & "' and deals_live.othercharacteristics='Discount Purchase' "


            'strSQL = "select * from securitypurchase join deals_live on securitypurchase.tb_id=deals_live.TB_ID  where securitypurchase.tb_id ='" & _
            'ListTbs.FocusedItem.Text & "' and deals_live.TB_ID ='" & ListTbs.FocusedItem.Text & "' and deals_live.othercharacteristics='Discount Purchase' "

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                'MsgBox(Format(CDbl(drSQL.Item(2).ToString), "###,###.00"))
                'MsgBox(CDate(drSQL.Item(4)))
                PurValue.Text = Format(CDbl(drSQL.Item(2).ToString), "###,###.00")
                IntRate.Text = drSQL.Item(3).ToString
                AvaliableForSale.Text = Format(CDec(drSQL.Item(12).ToString) - SecurityAttached(Trim(cmbTB.SelectedValue.ToString()), Trim(Get_TB(cmbTB.SelectedValue.ToString()))), "###,###.00")
                'DaysMaturity.Text = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(drSQL.Item("MaturityDate")))
                ' maturity = drSQL.Item(4).ToString
                'txtMaturityDate.Text = CDate(drSQL.Item("MaturityDate").ToString)
            Loop
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
    End Sub

    Protected Sub cmdSale_Click(sender As Object, e As EventArgs) Handles cmdSale.Click

        InsertSecurity(Trim(cmbTB.SelectedValue.ToString), Trim(Get_TB(cmbTB.SelectedValue.ToString)), txtSale.Text, txtExpiry.Text, Trim(Session("username")), Trim(cmbCustomerLoan.SelectedValue.ToString))
        Dim message As String = "Security Has Been Added"""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "MsgBox('" & message & "');", True)
        Session("ernest") = "True"
        Try
            strSQL = " SELECT * FROM SECURITYTEMP  where Customer='" & Trim(cmbCustomerLoan.SelectedValue.ToString) & "' and Dealer='" & Trim(Session("username")) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GridSecurity.DataSource = drSQL
            GridSecurity.DataBind()


            MsgBox(GetTotalSecurity())
            txtTotalSecurityLoan.Text = GetTotalSecurity()

        Catch ex As Exception
            lblError.Text = alert(ex.Message, "Error")
        End Try
    End Sub
    Public Sub GetCustomerCollateral(ByVal CustNumber As String)

        Dim x As String
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Try
            'Deposit Deals
            strSQL1 = "select * from COLLATERAL_ITEMS join COLL_COLLATERAL_TYPES on COLL_COLLATERAL_TYPES.collateralID=" & _
            "COLLATERAL_ITEMS.collateralType where collateralapproved='Y' and expired='N' and collateralCancelled='N'" & _
            " and secureDeposit='Y'"


            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()



            Do While drSQL1.Read
                'Dim itmx As New ListViewItem(drSQL1.Item("collateralReference").ToString)
                'itmx.SubItems.Add(Trim(drSQL1.Item(22).ToString))
                'itmx.SubItems.Add(Trim(drSQL1.Item(4).ToString))
                cmbSecurity.Items.Add(New ListItem(Trim(drSQL1.Item("collateralReference").ToString) + " " + Trim(drSQL1.Item("collateralID").ToString) + " " + Trim(drSQL1.Item("CollateralDescription").ToString), Trim(drSQL1.Item("collateralReference").ToString)))

            Loop
            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()

        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            lblError.Text = alert(ex.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ListTbs_Click : AttachSecurity" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub
    Private Function CollateralDescription(ByVal dealref As String)
        Try
            'validate username first
            strSQL = "select COLL_COLLATERAL_TYPES.CollateralDescription  from COLLATERAL_ITEMS join COLL_COLLATERAL_TYPES on COLL_COLLATERAL_TYPES.collateralID=" & _
                "COLLATERAL_ITEMS.collateralType where collateralapproved='Y' and expired='N' and collateralCancelled='N'" & _
                " and secureDeposit='Y' and collateralReference='" & Trim(dealref) & "'"
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
    Protected Sub btnAddCollateralLoan_Click(sender As Object, e As EventArgs) Handles btnAddCollateralLoan.Click
        '' MsgBox("Security amount entered is not valid.", MsgBoxStyle.Critical, "Security")
        ' lblModalError.Text = alert("Security amount cannot be greater that amount available.", "Security")

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

        InsertSecurity(Trim(cmbSecurity.SelectedValue.ToString), Trim(CollateralDescription(cmbSecurity.SelectedValue.ToString)), txtCollateralLoan.Text, txtExpiry.Text, Trim(Session("username")), Trim(cmbCustomerLoan.SelectedValue.ToString))
        Dim message As String = "Security Has Been Added"""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "MsgBox('" & message & "');", True)
        Session("ernest") = "True"
        Try
            strSQL = " SELECT * FROM SECURITYTEMP  where Customer='" & Trim(cmbCustomerLoan.SelectedValue.ToString) & "' and Dealer='" & Trim(Session("username")) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GridSecurity.DataSource = drSQL
            GridSecurity.DataBind()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            lblError.Text = alert(ex.Message, "Error")
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
    Protected Sub cmbSecurity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSecurity.SelectedIndexChanged
        ''MsgBox(cmbSecurity.SelectedValue.ToString())
        Dim drSQL11 As SqlDataReader
        Try
            'validate username first
            strSQL = "select * from COLLATERAL_ITEMS where collateralReference='" & Trim(cmbSecurity.SelectedValue.ToString()) & " ' and collateralapproved='Y'" & _
            " and expired='N' and collateralCancelled='N' "

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL11 = cmSQL.ExecuteReader()
            Do While drSQL11.Read
                txtCollCurrency.Text = (drSQL11.Item("collateralCurrency").ToString)
                txtValue.Text = Format(CDbl(drSQL11.Item("collateralBankValue").ToString), "###,###.00")
                txtExpiry.Text = (Trim(drSQL11.Item("collateralExpiry").ToString))
                txtAvailble.Text = Format((CDbl(drSQL11.Item("collateralBankValue").ToString) - GetLoanCollateral()), "###,###.00")

                If Trim(drSQL11.Item("assignment").ToString) = "Full" Then
                    txtCollateralLoan.Text = txtAvailble.Text
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
    Private Sub AttachSecurity(ByVal dealNum As String)

        Dim x As String = ""
        Dim cnSQLdd As SqlConnection
        Dim cmSQLdd As SqlCommand
        Dim drSQLdd As SqlDataReader
        Dim strSQLdd As String

        Try
            strSQLdd = "SELECT Security,DealRef,Amount,id FROM SECURITYTEMP  where Customer='" & Trim(cmbCustomerLoan.SelectedValue.ToString) & "' and Dealer='" & Trim(Session("username")) & "'"
            cnSQLdd = New SqlConnection(Session("ConnectionString"))
            cnSQLdd.Open()
            cmSQLdd = New SqlCommand(strSQLdd, cnSQLdd)
            drSQLdd = cmSQLdd.ExecuteReader

            Do While drSQLdd.Read
                Call SavDls.SaveDealSecurity(dealNum, Trim(drSQLdd.Item(0)), CDec(Trim(drSQLdd.Item(2))), Trim(drSQLdd.Item(1)), Session("serverName"), Session("dataBaseName"), Session("loggedUserLog"))
                DeleteSecurityTemp(Trim(drSQLdd.Item(3)))
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

            strSQL2 = "select sum(amount) FROM SECURITYTEMP  where Customer='" & Trim(cmbCustomerLoan.SelectedValue.ToString) & "' and Dealer='" & Trim(Session("username")) & "'"

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
    Protected Sub GrdDealsMM_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        If e.CommandName = "select" Then
            MsgBox(txtdealref.Text)
            txtdealref.Text = e.Item.Cells(0).Value.ToString()
            'lblInfo.Text = txtdealref.Text
            Call DeleteSecurityTemp(txtdealref.Text)
        End If
    End Sub
    Public Sub DeleteSecurityTemp(ByVal id As Integer)
        Dim cnSQL5 As SqlConnection
        Dim cmSQL5 As SqlCommand
        Dim drSQL5 As SqlDataReader
        Dim strSQL5 As String

        Try
            strSQL5 = "delete SECURITYTEMP where id='" & Trim(id) & "'"


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



    Protected Sub GridSecurity_ItemDeleted(sender As Object, e As Global.EO.Web.GridItemEventArgs)


        txtdealref.Text = e.Item.Cells(0).Value.ToString()
        MsgBox(txtdealref.Text)
        Call DeleteSecurityTemp(txtdealref.Text)

    End Sub
End Class
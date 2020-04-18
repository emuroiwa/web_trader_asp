Imports System.Data.SqlClient
Imports System.Drawing
Imports sys_ui

Public Class SecurityPurchase
    Inherits System.Web.UI.Page
    Private object_userlog As New usrlog.usrlog
    Private intAccruedToDate As Decimal
    Dim limitsch As New usrlmt.usrlmt
    Public crStatus As String 'Stores counterparty limit status 1=exceeded 2= ok
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private CustName As String
    Private referalResponse As Integer = 3 'referal response
    Private ResponseReason As String
    Dim objectDealNumbers As New mmDeal.DeaNumbers
    Private daystomature As Decimal
    Private refid As Integer
    Private RatesCheck As New usrfunc.usrfunc
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    'Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private globalvars_mmdeal As New GlobalVars.mmDeal   'instance of the userlogins class
    Private SavDls As New csvptt.csvptt
    Dim checkMaturity As New mmDeal.DealMaturityCheck
    Dim NonBusiness As New mmDeal.DealMaturityCheck
    Dim Holidays As New mmDeal.DealMaturityCheck
    Private InterestBasisValue As Integer 'Stores the current value for use as the interest dayas basis
    Private ChangedDaysBasis As Boolean = False 'Value indicating if the days basis default has been manually changed
    Private checkacc As New mmDeal.DealInstructions
    Private saveDash As New mmDeal.SaveDashBoardDeal
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

   
    'url variables
    Private portifolioid As String
    Private portifolio As String
    Private Dash As String
    Private dealCode As String
    Private currency As String
    Private product As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        startDatePur.Text = CDate(Session("SysDate"))
      
        Call GetIntBasisValue()
        If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then

            portifolioid = Request.QueryString("portifolioid")
            portifolio = Request.QueryString("portifolio")
            Dash = Request.QueryString("dash")
            dealCode = Request.QueryString("dealcode")
            currency = Request.QueryString("currency")

            product = Request.QueryString("product")
        Else
            MsgBox("Please make sure all fields are selected")

        End If
        lblportfolioPur.Text = portifolio
        lblDescriptionPur.Text = product
        lblDealtypePur.Text = dealCode
        txtCurrency.Text = currency
        txtIntDays.Text = InterestBasisValue
        '' Response.Write(Request.QueryString[0])
        '' haya.tt()
        ''sys_admin.hayaa()
        '' MsgBox(sys_admin.Test(8, 8))
        Call Deal_Instructions()
        Call LoadCustomers()
    End Sub
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
                    cmbCustomerPur.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
                    cmbIssurer.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
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
                        cmbInstructionMaturityPur.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                    ElseIf drSQL.Item("purpose").ToString.Equals("I") Then
                        cmbInstructionPur.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                    End If
                Loop


            End If

            cmbInstructionPur.Items.Add("Other")
            cmbInstructionMaturityPur.Items.Add("Other")

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

    Protected Sub cmbInstructionPur_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbInstructionPur.SelectedIndexChanged
        If cmbInstructionPur.Text = "Other" Then
            txtOtherPur.BackColor = Color.White
            txtOtherPur.Enabled = True
        Else
            'txtOtherPr.BackColor = Color.LightYellow
            'txtOtherPr.Enabled = False
        End If
    End Sub

    Protected Sub RadioDiscYield_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioDiscYield.SelectedIndexChanged
        If RadioDiscYield.SelectedValue = "discount" Then
            txtMaturityAmountPur.Enabled = True
            CheckComRatePur.Enabled = True
            txtTenorPur.Enabled = True
            txtDealAmountPur.Enabled = False
            txtDealAmountPur.Text = ""
            txtYieldRatePur.Enabled = False
            txtDiscountRatePur.Enabled = True
            txtCommRate.Enabled = False
            txtCommAmount.Enabled = True
            txtCommAmount.Enabled = False
            CheckComRatePur.Checked = False
            CheckComRatePur.Enabled = True
            txtCommRate.Text = "0"
            Label20.Text = "Net Proceeds"

            'Set background color
            txtMaturityAmountPur.BackColor = Color.White
            txtTenorPur.BackColor = Color.White
            txtDiscountRatePur.BackColor = Color.White
            txtTenorPur.BackColor = Color.White

            txtDealAmountPur.BackColor = Color.LightYellow
            txtYieldRatePur.BackColor = Color.LightYellow
            txtTenorPur.BackColor = Color.White


        ElseIf RadioDiscYield.SelectedValue = "yield" Then
            txtMaturityAmountPur.Enabled = False
            txtDealAmountPur.Enabled = True
            txtMaturityAmountPur.Text = ""
            txtYieldRatePur.Enabled = True
            txtDiscountRatePur.Enabled = False
            txtCommRate.Enabled = False
            txtCommAmount.Enabled = False
            CheckComRatePur.Checked = False
            CheckComRatePur.Enabled = False
            txtCommRate.Text = "0"
            txtTenorPur.Enabled = True
            Label20.Text = "Amount Invested"

            txtDealAmountPur.BackColor = Color.White
            txtYieldRatePur.BackColor = Color.White
            txtTenorPur.BackColor = Color.White

            txtMaturityAmountPur.BackColor = Color.LightYellow
            txtTenorPur.BackColor = Color.LightYellow
            txtDiscountRatePur.BackColor = Color.LightYellow
            txtCommAmount.BackColor = Color.LightYellow
            txtTenorPur.BackColor = Color.White

        End If
    End Sub

    Protected Sub CheckComRatePur_CheckedChanged(sender As Object, e As EventArgs) Handles CheckComRatePur.CheckedChanged
        If CheckComRatePur.Checked = True Then
            txtCommRate.Text = ""
            txtCommRate.Enabled = True
            txtCommRate.BackColor = Color.White
        Else
            txtCommRate.Text = "0"
            txtCommRate.Enabled = False
            txtCommRate.BackColor = Color.LightYellow
        End If
    End Sub

    Protected Sub txtMaturityAmountPur_TextChanged(sender As Object, e As EventArgs) Handles txtMaturityAmountPur.TextChanged
        On Error Resume Next
        txtMaturityAmountPur.Text = Format(CDec(txtMaturityAmountPur.Text), "###,###,###.00").ToString
    End Sub

    Protected Sub txtDealAmountPur_TextChanged(sender As Object, e As EventArgs) Handles txtDealAmountPur.TextChanged
        On Error Resume Next
        txtDealAmountPur.Text = Format(CDec(txtDealAmountPur.Text), "###,###,###.00").ToString
    End Sub

    Protected Sub txtCommAmount_TextChanged(sender As Object, e As EventArgs) Handles txtCommAmount.TextChanged
        On Error Resume Next
        txtCommAmount.Text = Format(CDec(txtCommAmount.Text), "###,###,###.00").ToString
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("newdeal.aspx")
    End Sub

    Protected Sub btnCancelPur_Click(sender As Object, e As EventArgs) Handles btnCancelPur.Click

    End Sub
    Private Sub ResetPurchase()
        ' Call DisableControlsPur() 'enable disabled controls
        ' Call clearfieldspur() 'Clear all controls
        btnSavePurchase.Enabled = False
        btnValidatePur.Enabled = True
        Call EnableControlsPur()
        'Set control colors
        txtMaturityAmountPur.BackColor = Color.White
        txtTenorPur.BackColor = Color.White
        txtDiscountRatePur.BackColor = Color.White
        txtCommAmount.BackColor = Color.LightYellow
        txtDealAmountPur.BackColor = Color.White
        txtYieldRatePur.BackColor = Color.White

        Call GetIntBasisValue()
    End Sub
    Private Sub EnableControlsPur()
        txtDealAccountPurchase.Enabled = True
        btnSavePurchase.Enabled = False
        txtMaturityAmountPur.Enabled = True
        txtDiscountRatePur.Enabled = True
        startDatePur.Enabled = True
        txtCommRate.Enabled = True
        CheckComRatePur.Enabled = True
        txtTenorPur.Enabled = True
        txtDealAmountPur.Enabled = True
        MaturityDatePur.Enabled = True
        txtYieldRatePur.Enabled = True
        txtCommAmount.Enabled = True
        btnValidatePur.Enabled = True
        cmbInstructionPur.Enabled = True
        txtOtherPur.Enabled = True
        cmbInstructionMaturityPur.Enabled = True
        txtOtherMaturityPur.Enabled = True
        'ShortsP.Enabled = True
    End Sub

    Protected Sub btnIntDaysPur_Click(sender As Object, e As EventArgs) Handles btnIntDaysPur.Click
        Call setBasisValue("Purchase")
        btnSavePurchase.Enabled = False
    End Sub
    Private Sub setBasisValue(ByVal AppType As String)
        'Dim intbasisval As New IntBasis
        'intbasisval.ShowDialog()
        'If intbasisval.cmdOK.DialogResult = Windows.Forms.DialogResult.OK Then
        '    InterestBasisValue = intbasisval.txtIntBasis.Text
        '    ChangedDaysBasis = True

        '    Select Case AppType
        '        'Case "Placement"
        '        '    'set interest days basis for currency
        '        '    txtDaysBasisLoan.Text = intbasisval.txtIntBasis.Text
        '        'Case "Deposit"
        '        '    'set interest days basis for currency
        '        '    txtDaysBasisDeposit.Text = intbasisval.txtIntBasis.Text
        '        Case "Purchase"
        '            'set interest days basis for currency
        '            txtIntDays.Text = intbasisval.txtIntBasis.Text
        '            'Case "Sale"
        '            '    'set interest days basis for currency
        '            '    txtDaysBasisSale.Text = intbasisval.txtIntBasis.Text

        '    End Select
        'Else

        'End If
    End Sub

    Protected Sub btnValidatePur_Click(sender As Object, e As EventArgs) Handles btnValidatePur.Click
        Dim dt1 As DateTime = DateTime.Parse(startDatePur.Text)
        Dim dt2 As DateTime = DateTime.Parse(MaturityDatePur.Text)
        Dim daystomature As Integer = (dt2 - dt1).Days
        txtTenorPur.Text = daystomature
        Dim calc As New mmDeal.CalculationFunctions
        Session("customerNumber") = cmbCustomerPur.SelectedValue.ToString()
        Session("IssurerNumber") = cmbIssurer.SelectedValue.ToString()
        Dim ID As String

        ID = portifolioid
        If checkMaturity.CheckBackValueThreshhold("mmbkval") < Math.Abs(DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(startDatePur.Text))) Then
            MsgBox("You have exceeded maximum back value threshold", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If getSysParam("fundrequired") = "Y" Then 'Check if the accounts are required
            'if its a discount deal check if the interest recievable account has been setup for the product first
            If RadioDiscYield.SelectedValue = "discount" Then
                If Trim(GetInterestAccount(dealCode)) = "" Then
                    MsgBox("interest account not set for product, dealing will be disabled", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            End If

            If CheckComRatePur.Checked = True Then
                If Trim(GetCommissionAccount(dealCode)) = "" Then
                    MsgBox("commission account not set for product, dealing will be disabled", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            End If
        End If



        limitsch.clients = Session("client")
        limitsch.ConnectionString = Session("ConnectionString")

        limitsch.ClearVariables()
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
        object_userlog.SendData("REQUESTUSERS|" & Session("LoggedUser"), Session("serverName"), Session("client"))

        'On Error GoTo err2

        'Dim x1 As Integer
        'Dim ID As String
        'Get selected portfolio id
        'x1 = cmdPortfolioPur.SelectedIndex
        'ID = MyPortfolioCollectionID.Item(x1 + 1).ToString


        'On Error GoTo err1

        If getSysParam("fundrequired") = "Y" Then 'Check if the accounts are required

            If Trim(txtPurchaseLoan.Text) = "" Then
                MsgBox("Please select Funding Account", MsgBoxStyle.Critical)
                'TabControl1.SelectedTab = TabControl1.TabPages(0)
                Exit Sub
            End If


            If Trim(txtDealAccountPurchase.Text) = "" Then
                MsgBox("Please select product Deal Account", MsgBoxStyle.Critical)
                'TabControl1.SelectedTab = TabControl1.TabPages(0)
                Exit Sub
            End If
        End If

        If Trim(cmbIssurer.Text) = "" Then
            MsgBox("Please select the Issurer of the Security", MsgBoxStyle.Critical)
            'TabControl1.SelectedTab = TabControl1.TabPages(0)
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDatePur.Text)) < 0 Then
            MsgBox("Maturity date cannot be less than business date", MsgBoxStyle.Information)
            'TabControl3.SelectedTab = TabControl3.TabPages(1)
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDatePur.Text)) = 0 Then
            MsgBox("Maturity date cannot be the same as deal start date", MsgBoxStyle.Information)
            'TabControl3.SelectedTab = TabControl3.TabPages(1)
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(startDatePur.Text)) > 0 Then
            MsgBox("Start date cannot be greater than business date", MsgBoxStyle.Information)
            'TabControl3.SelectedTab = TabControl3.TabPages(1)
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDatePur.Text)) = 0 Then
            MsgBox("Tenor cannot be zero.", MsgBoxStyle.Information)
            'TabControl3.SelectedTab = TabControl3.TabPages(1)
            Exit Sub
        End If

        If checkacc.checkDealAccount() = "Y" Then
            If txtDealAccountPurchase.Text = "" Then
                MsgBox("Deal account not selected", MsgBoxStyle.Exclamation, "Stop")
                'ElseIf Len(txtDealAccountPurchase.Text) > 0 And Len(txtDealAccountPurchase.Text) <> 13 Then
                '    MsgBox("Invalid account format", MsgBoxStyle.Critical, "Deal Account")
                '    TabControl3.SelectedTab = TabControl3.TabPages(1)
                Exit Sub
            End If
        End If

        Dim x As Decimal
        'Check if proposed maturity date is not a non-business day
        If NonBusiness.NonBusinessDay(CDate(MaturityDatePur.Text)) = True Then
            MsgBox("The selected maturity date is a not a business day.", "Non-Business Day")
            Exit Sub
        End If

        If Holidays.Holidays(CDate(MaturityDatePur.Text), getBaseCurrency) = True Then
            MsgBox("The selected maturity date is a holiday.", "Non-Business Day")
            'TabControl3.SelectedTab = TabControl3.TabPages(1)
            Exit Sub
        End If
        '****************************************end

        If RadioDiscYield.SelectedValue = "discount" Then 'main if
            'check fields


            If cmbCustomerPur.Text = "" Then
                MsgBox("Select customer", MsgBoxStyle.Information)
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If
            If txtTBID.Text = "" Then
                MsgBox("Enter the Security Reference.", MsgBoxStyle.Information)
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If
            If lblportfolioPur.Text = "" Then
                MsgBox("Select portfolio.", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If
            If lblDealtypePur.Text = "" Then
                MsgBox("Select deal type", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If

            If currency = "" Then
                MsgBox("Select Currency for deal.", MsgBoxStyle.Critical, "Currency")
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If

            If cmbCustomerPur.Text = "Select customer." Then
                MsgBox("", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If
            If txtMaturityAmountPur.Text = "" Then
                MsgBox("Insert maturity amount", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(1)
                Exit Sub
            End If
            If txtDiscountRatePur.Text = "" Then
                MsgBox("Insert discount rate", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(1)
                Exit Sub
            End If
            If txtTenorPur.Text = "" Then
                MsgBox("Specify deal running period.", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(1)
                Exit Sub
            End If
            If txtCommRate.Text = "" Then
                txtCommRate.Text = "0"
            End If
            If cmbInstructionPur.Text = "" Then
                MsgBox("Enter Instruction for deal inception.", MsgBoxStyle.Exclamation)
                'TabControl3.SelectedTab = TabControl3.TabPages(2)
                Exit Sub
            End If
            If cmbInstructionMaturityPur.Text = "" Then
                MsgBox("Enter Instruction for deal maturity.", MsgBoxStyle.Exclamation)
                'TabControl3.SelectedTab = TabControl3.TabPages(2)
                Exit Sub
            End If
            If cmbInstructionPur.Text = "Other" And txtOtherPur.Text = "" Then
                MsgBox("Please enter other instructions for deal inception.", MsgBoxStyle.Information)
                'TabControl3.SelectedTab = TabControl3.TabPages(2)
                Exit Sub
            End If
            If cmbInstructionMaturityPur.Text = "Other" And txtOtherMaturityPur.Text = "" Then
                MsgBox("Please enter other instructions for deal maturity.", MsgBoxStyle.Information)
                'TabControl3.SelectedTab = TabControl3.TabPages(2)
                Exit Sub
            End If



            'Set control colors
            txtMaturityAmountPur.BackColor = Color.LightYellow
            txtTenorPur.BackColor = Color.LightYellow
            txtDiscountRatePur.BackColor = Color.LightYellow
            txtCommAmount.BackColor = Color.LightYellow
            txtDealAmountPur.BackColor = Color.LightYellow
            txtYieldRatePur.BackColor = Color.LightYellow
            txtTenorPur.BackColor = Color.LightYellow
            txtCommRate.BackColor = Color.LightYellow


            If ChangedDaysBasis = True Then
                calc.calcEngineDiscount1(CDec(txtTenorPur.Text), 0, CDec(txtDiscountRatePur.Text) _
                , CDec(txtMaturityAmountPur.Text), CDec(txtCommRate.Text), InterestBasisValue)
            Else
                calc.calcEngineDiscount(CDec(txtTenorPur.Text), 0, CDec(txtDiscountRatePur.Text) _
                 , CDec(txtMaturityAmountPur.Text), CDec(txtCommRate.Text), Trim(currency))

                IntBasis(Trim(currency))
            End If

            txtNetInterestPur.Text = calc.netInt.ToString
            txtGrossIntPur.Text = calc.grossInt.ToString
            'txtTaxAmountSale.Text = taxAmount.ToString
            txtDealAmountPur.Text = Format(CDec(calc.maturityAmount), "###,###,###.00").ToString
            txtYieldRatePur.Text = calc.YieldRateDerived.ToString
            txtCommAmount.Text = calc.commision

            'Check for invalid or out of valid range discount rates
            If CDec(txtNetInterestPur.Text) >= CDec(txtMaturityAmountPur.Text) Then
                MsgBox("The discount rate entered is not valid. Change the Yield or the discount rate.", MsgBoxStyle.Exclamation, "Invalid Discount Rate")
                Exit Sub
            End If


            'Check Limits
            'Check Dealer Daily limit
            If UCase(Trim(Dash)) = "FALSE" Then 'Check limits only if the deal is not on the dash board


                'Check Trnasaction Limits if implemented this function will return 1 to indicate that 
                'limits have been exceeded and 2 to indicate that transaction is within the limit
                '0 means Limit checking not implemented 

                'msg(0) = is the limit status value
                'msg(1) = Decription of limit
                'msg(2) = Limit Amount

                TranxLimitVal = limitsch.PeriodLimitValidation(Int(txtTenorPur.Text), "LoanLimit", Session("loggedUserLog"), _
                                CDbl(txtDealAmountPur.Text), Trim(currency), Trim(lblDealtypePur.Text))

                If TranxLimitVal(0) = "1" Then
                    MsgBox("Transaction Limit exceeded" & vbCrLf & _
                          "****************************************************" & vbCrLf & _
                           TranxLimitVal(1) & vbCrLf & _
                          "****************************************************" & vbCrLf & _
                           "Limit Amount             :   " & Format(CDbl(TranxLimitVal(2)), "###,###.00") & vbCrLf & _
                           "Transaction Amount :   " & txtDealAmountPur.Text, MsgBoxStyle.Information, "Limits")

                    If getSysParam("tranlmt") = "warn" Then
                        MsgBox("Limit violation will be recorded", MsgBoxStyle.Exclamation, "Transaction Limit exceeded")
                    ElseIf getSysParam("tranlmt") = "stop" Then
                        MsgBox("Limit exceeded, you cannot proceed with the transaction", MsgBoxStyle.Critical, "Transaction Limit exceeded")
                        Exit Sub
                    End If

                End If



                'Counterparty limits validation
                If limitsch.IsLimitSet(Session("customerNumber"), "Loan") = True Then
                    If limitsch.CheckCounterpartyLimit(Session("customerNumber"), Session("DealStructure"), CDec(txtDealAmountPur.Text), Trim(currency), CDate(MaturityDatePur.Text)) = False Then

                        crStatus = "1" 'LIMIT EXCEEDED

                        If CounterpartyLimitViolation(CDec(txtDealAmountPur.Text), Session("DealStructure"), currency, CDate(MaturityDatePur.Text)) = "stop" Then
                            Exit Sub
                        End If

                    Else
                        crStatus = "2" 'WITHIN LIMIT
                    End If

                Else
                    crStatus = "3" 'NO LIMIT
                End If


                CustName = cmbCustomerPur.Text

                If limitsch.CheckDailyLimit(Session("LoggedUser"), Trim(lblDealtypePur.Text), CDec(txtDealAmountPur.Text), Trim(lblDescriptionPur.Text), Trim(currency)) = False Or _
             limitsch.CheckdealSizeLimit(Session("LoggedUser"), Trim(lblDealtypePur.Text), CDec(txtDealAmountPur.Text), Trim(lblDescriptionPur.Text), Trim(currency)) = False Or _
             limitsch.CheckProductPos(Trim(lblDealtypePur.Text), CDec(txtDealAmountPur.Text), "-", Trim(currency)) = False Or _
             limitsch.CheckPortfolioPos(ID, CDec(txtDealAmountPur.Text), "-", Trim(currency)) = False Then

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

                    'If MessageBox.Show("Limits have been exceeded. Do you want to send a referal?", "Limits", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                    '    Dim ref As New Referal
                    '    ref.RefMessageDealsize = limitsch.RefMessageDealsize
                    '    ref.RefMessageDaily = limitsch.RefMessageDaily
                    '    ref.refAmount = limitsch.refAmount
                    '    ref.RefDealType = limitsch.RefDealType
                    '    ref.Limitauthoriser = limitsch.Limitauthoriser
                    '    ref.RefMessagePortfolio = limitsch.RefMessagePortfolio
                    '    ref.RefMessageProduct = limitsch.RefMessageProduct
                    '    ref.RefMessageCounterparty = limitsch.RefMessageCounterparty

                    '    ref.DealCustomer = Trim(txtCustomerPur.Text)
                    '    ref.DealDealAmount = CDec(txtDealAmountPur.Text)
                    '    ref.DealDealInterestRate = Trim(txtDiscountRatePur.Text)
                    '    ref.DealTenor = Int(txtTenorPur.Text)
                    '    ref.DealMaturityAmount = CDec(txtMaturityAmountPur.Text)
                    '    ref.dealGrossInterest = CDec(txtNetInterestPur.Text)
                    '    ref.dealPortfolio = Trim(cmdPortfolioPur.Text)
                    '    ref.dealDealType = Trim(lblDealtypePur.Text)
                    '    ref.DealDiscount = "Y"
                    '    ref.DealSecurityID = Trim(TBID.Text)
                    '    ref.DealCurrency = Trim(currency)
                    '    ref.DealTransType = "Security Purchase"
                    '    ref.CustName = customerN

                    '    ref.ShowDialog()

                    '    Limitauthoriser = ref.Limitauthoriser

                    '    ref.Close()
                    '    If btnstatus = "N" Then Exit Sub 'if Dont send referal
                    '    If referalResponse <> 1 Then
                    '        referalResponse = 3
                    '        Exit Sub 'response was no
                    '    End If
                    'Else
                    '    Exit Sub
                    'End If
                    referalResponse = 3 're-initialise variable
                End If
            End If
            '************************************************************************************
            refIDRecieve = refid

            Call DisableControlsPur()


            'Calculate interest accrued to date for back valued deals
            x = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(startDatePur.Text))
            If x < 0 Then
                x = Math.Abs(x)
                daystomature = CDec(txtTenorPur.Text) - x

                If ChangedDaysBasis = True Then
                    calc.calcEngineDiscount1(x, 0, CDec(txtYieldRatePur.Text) _
                , CDec(txtDealAmountPur.Text), CDec(txtCommRate.Text), InterestBasisValue)
                Else
                    calc.calcEngineDiscount(x, 0, CDec(txtYieldRatePur.Text) _
                    , CDec(txtDealAmountPur.Text), CDec(txtCommRate.Text), Trim(currency))

                End If

                intAccruedToDate = Session("intAccruedToDateCal")
            Else
                intAccruedToDate = 0
                daystomature = CDec(txtTenorPur.Text)
            End If

            'Else

            'check fields
            If txtTBID.Text = "" Then
                MsgBox("Enter the Security Reference.", MsgBoxStyle.Information)
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If

            If cmbCustomerPur.Text = "" Then
                MsgBox("Select customer", MsgBoxStyle.Information)
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If
            If lblportfolioPur.Text = "" Then
                MsgBox("Select portfolio.", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If
            If lblDealtypePur.Text = "" Then
                MsgBox("Select deal type", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If

            If currency = "" Then
                MsgBox("Select Currency for deal.", MsgBoxStyle.Critical, "Currency")
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If

            If cmbCustomerPur.Text = "Select customer." Then
                MsgBox("", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(0)
                Exit Sub
            End If
            If txtDealAmountPur.Text = "" Then
                MsgBox("Enter deal amount", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(1)
                Exit Sub
            End If
            If txtYieldRatePur.Text = "" Then
                MsgBox("Enter yield rate", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(1)
                Exit Sub
            End If
            If txtTenorPur.Text = "" Then
                MsgBox("Specify deal running period.", MsgBoxStyle.Critical, "Incomplete Information")
                'TabControl3.SelectedTab = TabControl3.TabPages(1)
                Exit Sub
            End If
            If cmbInstructionPur.Text = "" Then
                MsgBox("Enter Instruction for deal inception.", MsgBoxStyle.Exclamation)
                'TabControl3.SelectedTab = TabControl3.TabPages(2)
                Exit Sub
            End If
            If cmbInstructionMaturityPur.Text = "" Then
                MsgBox("Enter Instruction for deal maturity.", MsgBoxStyle.Exclamation)
                'TabControl3.SelectedTab = TabControl3.TabPages(2)
                Exit Sub
            End If
            If cmbInstructionPur.Text = "Other" And txtOtherPur.Text = "" Then
                MsgBox("Please enter other instructions for deal inception.", MsgBoxStyle.Information)
                'TabControl3.SelectedTab = TabControl3.TabPages(2)
                Exit Sub
            End If
            If cmbInstructionMaturityPur.Text = "Other" And txtOtherMaturityPur.Text = "" Then
                MsgBox("Please enter other instructions for deal maturity.", MsgBoxStyle.Information)
                'TabControl3.SelectedTab = TabControl3.TabPages(2)
                Exit Sub
            End If

            'Check if proposed maturity date is not a non-business day
            If NonBusiness.NonBusinessDay(CDate(MaturityDatePur.Text)) = True Then
                MsgBox("The selected maturity date is a not a business day.", "Non-Business Day")
                Exit Sub
            End If

            If Holidays.Holidays(CDate(MaturityDatePur.Text), getBaseCurrency) = True Then
                MsgBox("The selected maturity date is a holiday.", "Non-Business Day")
                'TabControl3.SelectedTab = TabControl3.TabPages(1)
                Exit Sub
            End If
        End If
        '****************************************end



        'Set control colors
        txtMaturityAmountPur.BackColor = Color.LightYellow
        txtTenorPur.BackColor = Color.LightYellow
        txtDiscountRatePur.BackColor = Color.LightYellow
        txtCommAmount.BackColor = Color.LightYellow
        txtDealAmountPur.BackColor = Color.LightYellow
        txtYieldRatePur.BackColor = Color.LightYellow
        txtTenorPur.BackColor = Color.LightYellow
        txtCommRate.BackColor = Color.LightYellow

        If ChangedDaysBasis = True Then
            calc.CalcEngineYield1(CDec(txtTenorPur.Text), 0, CDec(txtYieldRatePur.Text), CDec(txtDealAmountPur.Text), InterestBasisValue)
        Else
            calc.CalcEngineYield(CDec(txtTenorPur.Text), 0, CDec(txtYieldRatePur.Text), CDec(txtDealAmountPur.Text), Trim(currency))
            IntBasis(Trim(currency))
        End If
        txtNetInterestPur.Text = calc.netInt.ToString
        txtGrossIntPur.Text = calc.grossInt.ToString
        'txtTaxAmountSale.Text = taxAmount.ToString
        txtMaturityAmountPur.Text = Format(CDec(calc.maturityAmount), "###,###,###.00").ToString
        txtDiscountRatePur.Text = calc.DiscountRateDerived.ToString


        'Calculate interest accrued to date for back valued deals
        x = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(startDatePur.Text))
        If x < 0 Then
            x = Math.Abs(x)
            daystomature = CDec(txtTenorPur.Text) - x

            If ChangedDaysBasis = True Then
                calc.CalcEngineYield1(x, 0, CDec(txtYieldRatePur.Text), CDec(txtDealAmountPur.Text), InterestBasisValue)
                intAccruedToDate = Session("intAccruedToDateCal")
            Else
                calc.CalcEngineYield(x, 0, CDec(txtYieldRatePur.Text), CDec(txtDealAmountPur.Text), Trim(currency))
                intAccruedToDate = Session("intAccruedToDateCal")
            End If

        Else
            intAccruedToDate = 0
            daystomature = CDec(txtTenorPur.Text)
        End If

        'Check for invalid or out of valid range discount rates
        If CDec(txtNetInterestPur.Text) >= CDec(txtMaturityAmountPur.Text) Then
            MsgBox("The discount rate entered is not valid. Change the Yield or the discount rate.", MsgBoxStyle.Exclamation, "Invalid Discount Rate")
            'TabControl3.SelectedTab = TabControl3.TabPages(1)
            Exit Sub
        End If



        'Check Limits
        'Check Dealer Daily limit
        If UCase(Trim(Dash)) = "FALSE" Then 'Check limits only if the deal is not on the dash board

            'Check Trnasaction Limits if implemented this function will return 1 to indicate that 
            'limits have been exceeded and 2 to indicate that transaction is within the limit
            '0 means Limit checking not implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount

            TranxLimitVal = limitsch.PeriodLimitValidation(Int(txtTenorPur.Text), "LoanLimit", Session("loggedUserLog"), _
                            CDbl(txtDealAmountPur.Text), Trim(currency), Trim(lblDealtypePur.Text))

            If TranxLimitVal(0) = "1" Then
                MsgBox("Transaction Limit exceeded" & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       TranxLimitVal(1) & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       "Limit Amount             :   " & Format(CDbl(TranxLimitVal(2)), "###,###.00") & vbCrLf & _
                       "Transaction Amount :   " & txtDealAmountPur.Text, MsgBoxStyle.Information, "Limits")


                If getSysParam("tranlmt") = "warn" Then
                    MsgBox("Limit violation will be recorded", MsgBoxStyle.Exclamation, "Transaction Limit exceeded")
                ElseIf getSysParam("tranlmt") = "stop" Then
                    MsgBox("Limit exceeded, you cannot proceed with the transaction", MsgBoxStyle.Critical, "Transaction Limit exceeded")
                    Exit Sub
                End If
            End If

            'Counterparty limits validation
            If limitsch.CheckCounterpartyLimit(Session("customerNumber"), Session("DealStructure"), CDec(txtDealAmountPur.Text), Trim(currency), CDate(MaturityDatePur.Text)) = False Then

                crStatus = "1"

                If CounterpartyLimitViolation(CDec(txtDealAmountPur.Text), Session("DealStructure"), currency, CDate(MaturityDatePur.Text)) = "stop" Then
                    Exit Sub
                End If

            Else
                crStatus = "2"
            End If

            CustName = cmbCustomerPur.Text

            If limitsch.CheckDailyLimit(Session("LoggedUser"), Trim(lblDealtypePur.Text), CDec(txtDealAmountPur.Text), Trim(lblDescriptionPur.Text), Trim(currency)) = False Or _
            limitsch.CheckdealSizeLimit(Session("LoggedUser"), Trim(lblDealtypePur.Text), CDec(txtDealAmountPur.Text), Trim(lblDescriptionPur.Text), Trim(currency)) = False Or _
            limitsch.CheckProductPos(Trim(lblDealtypePur.Text), CDec(txtDealAmountPur.Text), "-", Trim(currency)) = False Or _
            limitsch.CheckPortfolioPos(ID, CDec(txtDealAmountPur.Text), "-", Trim(currency)) = False Then

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

                'If MessageBox.Show("Limits have been exceeded. Do you want to send a referal?", "Limits", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                '    Dim ref As New Referal
                'ref.RefMsg(RefMessageDaily, RefMessageDealsize, RefMessageCounterparty, RefMessageProduct, RefMessagePortfolio, CustName, refAmount, RefDealType)
                '            ref.RefMessageDealsize = limitsch.RefMessageDealsize
                '            ref.RefMessageDaily = limitsch.RefMessageDaily
                '            ref.refAmount = limitsch.refAmount
                '            ref.RefDealType = limitsch.RefDealType
                '            ref.Limitauthoriser = limitsch.Limitauthoriser
                '            ref.RefMessagePortfolio = limitsch.RefMessagePortfolio
                '            ref.RefMessageProduct = limitsch.RefMessageProduct
                '            ref.RefMessageCounterparty = limitsch.RefMessageCounterparty

                '            ref.DealCustomer = Trim(txtCustomerPur.Text)
                '            ref.DealDealAmount = CDec(txtDealAmountPur.Text)
                '            ref.DealDealInterestRate = Trim(txtYieldRatePur.Text)
                '            ref.DealTenor = Int(txtTenorPur.Text)
                '            ref.DealMaturityAmount = CDec(txtMaturityAmountPur.Text)
                '            ref.dealGrossInterest = CDec(txtNetInterestPur.Text)
                '            ref.dealPortfolio = Trim(cmdPortfolioPur.Text)
                '            ref.dealDealType = Trim(lblDealtypePur.Text)
                '            ref.DealDiscount = "N"
                '            ref.DealSecurityID = Trim(TBID.Text)
                '            ref.DealCurrency = Trim(currency)
                '            ref.DealTransType = "Security Purchase"
                '            ref.CustName = customerN

                '            ref.ShowDialog()

                '            Limitauthoriser = ref.Limitauthoriser

                '            ref.Close()

                '            If btnstatus = "N" Then Exit Sub 'if Dont send referal
                '            If referalResponse <> 1 Then
                '                referalResponse = 3
                '                Exit Sub 'response was no
                '            End If
                '        Else
                '            Exit Sub
                '        End If
                '        referalResponse = 3 're-initialise variable
                'End If
            End If
            '    '************************************************************************************
            '    refIDRecieve = refid
            Call DisableControlsPur()
            btnSavePurchase.Enabled = True


        End If  'main if
        '-------------------------------------
        're-initialize variablse
        calc.netInt = 0
        calc.maturityAmount = 0
        calc.taxAmount = 0
        calc.grossInt = 0
        calc.DiscountRateDerived = 0
        calc.YieldRateDerived = 0
        '-------------------------------------

        Exit Sub
err1:
        Dim ex, ex3 As String
        ex3 = Err.GetException.InnerException.Message
        ex = Err.GetException.Message

        'Log the event *****************************************************
        object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex3 & " " & ex, Session("severName"), Session("client"))
        '************************END****************************************

        MsgBox(ex3 & " " & ex, MsgBoxStyle.Critical, "Error")

        Exit Sub

err2:
        lblsuccess.Text = success("Deal ValidatedSuccessfully.You can now Deal", "Deal Validated")

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

    Private Function GetCommissionAccount(ByVal dealCode As String) As String
        Dim commAccount As String = ""
        Try
            'save info for dealslip re-print
            strSQL = "select  EthixAccount from brdefaults where dealcode='" & dealCode & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                commAccount = drSQL.Item(0).ToString
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception

        End Try

        Return commAccount
    End Function
    'This function returns the interestdays basis for the currency
    Private Sub IntBasis(ByVal CurrencyCode As String)
        Dim x As Integer = 0

        Try
            strSQL = "select daysbasis from currencies where currencycode='" & Trim(CurrencyCode) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read

                If drSQL.IsDBNull(0) = False Then
                    InterestBasisValue = drSQL.Item("daysbasis")
                Else
                    GetIntBasisValue()
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
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "Function IntBasis", Session("severName"), Session("client"))
            '************************END****************************************
        End Try
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
    Private Function CounterpartyLimitViolation(dealamt As Double, dealtype As String, curr As String, matDate As String) As String
        Dim x As String = ""
        Dim myLimit As Decimal = CDbl(GetFieldVal(Session("customerNumber"), "limit", dealtype))
        Dim mycumulativeTotal As Decimal = CDbl(GetFieldVal(Session("customerNumber"), "cumulativetotal", dealtype))

        x = limitExpires(Session("customerNumber"), dealtype, curr, CDate(matDate))
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
    Private Sub DisableControlsPur()
        'cmdSaveDealPur.Enabled = False
        txtMaturityAmountPur.Enabled = False
        txtDiscountRatePur.Enabled = False
        startDatePur.Enabled = False
        txtCommRate.Enabled = False
        CheckComRatePur.Enabled = False
        txtTenorPur.Enabled = False
        txtDealAmountPur.Enabled = False
        MaturityDatePur.Enabled = False
        txtYieldRatePur.Enabled = False
        txtCommAmount.Enabled = False
        btnValidatePur.Enabled = True
        btnSavePurchase.Enabled = True
        cmbInstructionPur.Enabled = False
        txtOtherPur.Enabled = False
        cmbInstructionMaturityPur.Enabled = False
        txtOtherMaturityPur.Enabled = False
        'ShortsP.Enabled = False
        txtDealAccountPurchase.Enabled = False
        'TabControl3.SelectedTab = TabControl3.TabPages(1)
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

    Protected Sub btnSavePurchase_Click(sender As Object, e As EventArgs) Handles btnSavePurchase.Click
        TranxLimitVal = limitsch.PeriodLimitValidation(Int(txtTenorPur.Text), "LoanLimit", Session("loggedUserLog"), _
                           CDbl(txtDealAmountPur.Text), Trim(currency), Trim(lblDealtypePur.Text))
        crStatus = "3"
        Dim dt1 As DateTime = DateTime.Parse(startDatePur.Text)
        Dim dt2 As DateTime = DateTime.Parse(MaturityDatePur.Text)
        Dim daystomature As Integer = (dt2 - dt1).Days
        txtTenorPur.Text = daystomature
        Try
            Dim x As Integer
            Dim ID As String = portifolioid
            Dim entryType As String

            If RadioDiscYield.SelectedValue = "discount" Then
                entryType = "D"
            Else
                entryType = "Y"
            End If

            'Get selected portfolio id
            'x = cmdPortfolioPur.SelectedIndex
            'ID = MyPortfolioCollectionID.Item(x + 1).ToString

            Dim Instruction As String
            If Trim(cmbInstructionPur.Text) = "Other" Then
                Instruction = Replace(Trim(txtOtherPur.Text), "’", "")
                Instruction = Replace(Trim(txtOtherPur.Text), "'", "")
                Instruction = Replace(Trim(txtOtherPur.Text), "&", "and")

            Else
                Instruction = Replace(Trim(cmbInstructionPur.Text) & " -" & Trim(txtOtherPur.Text), "’", "")
                Instruction = Replace(Trim(cmbInstructionPur.Text) & " -" & Trim(txtOtherPur.Text), "'", "")
                Instruction = Replace(Trim(cmbInstructionPur.Text) & " -" & Trim(txtOtherPur.Text), "&", "and")
            End If

            Dim InstructionM As String
            If Trim(cmbInstructionMaturityPur.Text) = "Other" Then
                InstructionM = Replace(Trim(txtOtherMaturityPur.Text), "’", "")
                InstructionM = Replace(Trim(txtOtherMaturityPur.Text), "'", "")
                InstructionM = Replace(Trim(txtOtherMaturityPur.Text), "&", "and")
            Else
                InstructionM = Replace(Trim(cmbInstructionMaturityPur.Text) & " -" & Trim(txtOtherMaturityPur.Text), "’", "")
                InstructionM = Replace(Trim(cmbInstructionMaturityPur.Text) & " -" & Trim(txtOtherMaturityPur.Text), "'", "")
                InstructionM = Replace(Trim(cmbInstructionMaturityPur.Text) & " -" & Trim(txtOtherMaturityPur.Text), "&", "and")
            End If

            Dim dealNum As String = ""
            'get deal number
            'if deal is not the dashboard
            If UCase(Trim(Dash)) = "FALSE" Then
                dealNum = objectDealNumbers.GetDealNumber(Trim(lblDealtypePur.Text))
            Else
                dealNum = objectDealNumbers.GetDealNumberDash(Trim(lblDealtypePur.Text))
            End If

            'Save the deal

            'Check if the dealreference is ok
            If Trim(dealNum) = "" Then
                lblError.Text = alert("Deal reference not generated", "DealRefError")
                Exit Sub
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

                'If TranxLimitVal(0) <> "0" Then
                '    limitsch.SaveTranxLimitsDetails(Trim(LoggedUser), IntTranxLimitVal(3)), TrimTranxLimitVal(1)), dealNum _
                '    , CDbl(MaturityAmountPur.Text), CDblTranxLimitVal(2)), CDate(SysDate), Int(TranxLimitVal(0)), Trim(currency))
                'End If

                'Save counterparty limit information and transaction limits details for period settings detail here
                'limitsch.SaveCounterpartyLimitsDetails(Trim(Session("LoggedUser")), dealNum, CDbl(txtDealAmountPur.Text), GetFieldVal(Session("customerNumber"), "limit", "Loan") _
                '  , CDate(Session("SysDate")), crStatus, Trim(currency), "L", Session("customerNumber"), Int(TranxLimitVal(3)), Trim(TranxLimitVal(1)), CDbl(TranxLimitVal(2)), _
                '  Int(TranxLimitVal(0)), "")


                'SavDls.clients = client
                SavDls.strCurrentD = clientlogin_vars.strCurrentDirectory
                'SavDls.dataBaseName = dataBaseName

                Call SavDls.saveDiscountedPurchase(daystomature, Session("SysDate"), RequireFrontAuthoriser, Trim(ID), Trim(lblDealtypePur.Text), Session("customerNumber"), CDate(startDatePur.Text), CDate(MaturityDatePur.Text), _
                 CDec(txtDealAmountPur.Text), CDec(txtMaturityAmountPur.Text), CDec(txtDiscountRatePur.Text), CDec(txtYieldRatePur.Text), _
                 CDec(txtNetInterestPur.Text), Int(txtTenorPur.Text), Instruction, dealNum, "Discount Purchase", CDec(txtYieldRatePur.Text), CDec(txtCommAmount.Text) _
                , CDec(txtGrossIntPur.Text), intAccruedToDate, txtTBID.Text, entryType, Trim(currency) _
                , Session("IsDealer"), Session("loggedUserLog"), InterestBasisValue, Session("serverName"), Session("dataBaseName"), globalvars_mmdeal.PrintPages, InstructionM, Trim(txtDealAccountPurchase.Text), Trim(cmbIssurer.Text))



                intAccruedToDate = 0         'Reinitialise interest accrued to date

                Call GetIntBasisValue() 'Reinitialise interest days basis
                'if deal is from dashboard the set it to expired
                Call ExpireDashDeal(Session("LoggedUser"), Trim(txtDashRef.Text))


                'Apply Default Settlement Details

                tdsdebitAccount = getDealAccountTDS(Trim(lblDealtypePur.Text), "FUN")
                tdsCreditAccount = Trim(lblDealtypePur.Text) & "-" & Trim(Session("customerNumber")) 'getDealAccountTDS(Trim(lblDealtypePur.Text), "DAL")
                tdscommissionAccount = getDealAccountTDS(Trim(lblDealtypePur.Text), "COM")
                tdsinterestaccount = getDealAccountTDS(Trim(lblDealtypePur.Text), "INT")
                ethixdebitAccount = Trim(txtPurchaseLoan.Text) 'getDealAccountEthix(tdsdebitAccount)
                ethixCreditAccount = Trim(txtDealAccountPurchase.Text) 'getDealAccountEthix(tdsCreditAccount)
                ethixcommissionAccount = getDealAccountEthix(tdscommissionAccount)
                ethixinterestaccount = getDealAccountEthix(tdsinterestaccount)
                EthixAccrualDebit = getEthixAccrualAccount(Trim(lblDealtypePur.Text), "debitAccount")
                EthixAccrualCredit = getEthixAccrualAccount(Trim(lblDealtypePur.Text), "creditAccount")


                'Apply Default Settlement Details

                SetDefaultSettlementAccounts(dealNum, tdsdebitAccount, tdsCreditAccount, tdsinterestaccount, _
                tdscommissionAccount, ethixdebitAccount, ethixCreditAccount, ethixinterestaccount, _
                ethixcommissionAccount, EthixAccrualDebit, EthixAccrualCredit)


                'Set settlement instructions / accounts if true
                'If GetSettlementDetailsPar() = True Then
                '    Dim settlem As New frmSettlement
                '    settlem.seqq = "1"
                '    settlem.typOFDeal.Text = "MM"
                '    settlem.lblReference.Text = dealNum
                '    settlem.ShowDialog()
                'End If

            Else
                'save the dashboard deal
                saveDash.saveDashboardPurchase(Trim(ID), Trim(lblDealtypePur.Text), Session("customerNumber"), CDate(startDatePur.Text), CDate(MaturityDatePur.Text), CDec(Trim(txtDealAmountPur.Text)) _
                , CDec(Trim(txtDiscountRatePur.Text)), CDec(Trim(txtYieldRatePur.Text)), Int(Trim(txtTenorPur.Text)), Instruction, dealNum, "Discount Purchase", Session("SysDate") _
                , Trim(txtTBID.Text), CDec(txtCommRate.Text), CDec(txtMaturityAmountPur.Text), entryType, Trim(currency), Trim(txtDealAccountPurchase.Text), Trim(txtPurchaseLoan.Text))

                Call GetIntBasisValue()

            End If

            'save information about broken limits
            If dealsizeBroken = True Or dailyBroken = True Or PortfolioBroken = True Or ProductBroken = True Or CounterPartyBroken = True Then
                Dim limitsch As New usrlmt.usrlmt
                limitsch.clients = Session("client")
                limitsch.ConnectionString = Session("ConnectionString")
                Call limitsch.LimitBroken(dealNum, refIDRecieve)
            End If


            Call DisableControlsPur() 'enable disabled controls
            Call clearfieldspur() 'Clear all controls


            'Determine if deal is on dashboard or not
            'if deal is not the dashboard
            If UCase(Trim(Dash)) = "FALSE" Then
                'Setup deal notifications

                'If SetEmailNotifications() = True Then 'Checks if the parameter is defined for this action
                '    If MessageBox.Show("Do you want to setup notifications for this deal?", "Notifications", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                '        Dim notify As New Notifications
                '        notify.lblRef.Text = Trim(dealNum)
                '        notify.btnfind.Visible = False
                '        notify.txtSearch.Visible = False
                '        notify.GetEmailAddress(Session("LoggedUser"), Trim(Session("customerNumber")))
                '        notify.ShowDialog()
                '    End If
                'End If
            End If



            Call ResetPurchase() 'Reset Controls

        Catch ex As Exception
            MsgBox(ex.Message)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
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
    Private Sub clearfieldspur()
        txtMaturityAmountPur.Text = ""
        txtDiscountRatePur.Text = ""
        startDatePur.Text = CDate(Session("SysDate"))
        MaturityDatePur.Text = CDate(Session("SysDate"))
        txtTenorPur.Text = ""
        txtOtherPur.Text = ""
        txtOtherMaturityPur.Text = ""
        txtDealAmountPur.Text = ""
        MaturityDatePur.Text = ""
        txtYieldRatePur.Text = ""
        txtCommAmount.Text = "0"
        txtCommRate.Text = "0"
        CheckComRatePur.Checked = False
        
        txtGrossIntPur.Text = ""
        txtNetInterestPur.Text = ""
        'cmbCustomerPur.Text = ""
        txtDealAccountPurchase.Text = ""
        txtPurchaseLoan.Text = ""
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

    Protected Sub cmbCustomerPur_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCustomerPur.SelectedIndexChanged
        Call GetHist(cmbCustomerPur.SelectedValue.ToString(), "Discount Purchase", "USD")
        Call GetCustomerAddressInfo(cmbCustomerPur.SelectedValue.ToString())
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

        End Try


    End Sub
End Class
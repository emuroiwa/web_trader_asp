Imports System.Data.SqlClient
Imports System.Drawing
Imports sys_ui
Public Class MaturedLoan
    Inherits System.Web.UI.Page
    Private RollBool As Boolean
    Private object_userlog As New usrlog.usrlog
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private checkacc As New mmDeal.DealInstructions
    Private RatesCheck As New usrfunc.usrfunc
    Private saveDash As New mmDeal.SaveDashBoardDeal
    Dim objectDealNumbers As New mmDeal.DeaNumbers
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private globalvars_mmdeal As New GlobalVars.mmDeal   'instance of the userlogins class
    Private SavDls As New csvptt.csvptt
    Dim checkMaturity As New mmDeal.DealMaturityCheck
    Dim NonBusiness As New mmDeal.DealMaturityCheck
    Dim Holidays As New mmDeal.DealMaturityCheck
    'Public customerN As String 'custmer name from searchSession("customerA")
    'Public customerA As String 'customer account number from search
    Private crStatus As String 'Stores counterparty limit status 1=exceeded 2= ok
    Private InterestBasisValue As Integer 'Stores the current value for use as the interest dayas basis
    Private ChangedDaysBasis As Boolean = False 'Value indicating if the days basis default has been manually changed
    'Private contactTooTip As String
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

    '*****************************************************
    'url variables
    Private portifolioid As String
    Private portifolio As String
    Private Dash As String
    Private dealCode As String
    Private currency As String
    Private product As String

    '**********************************************

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        StartDateLoan.Text = CDate(Session("SysDate"))
        tabMaturedLoan.ActiveTabIndex = 0
        Session("customerA") = cmbCustomerLoanM.SelectedValue.ToString()

        Call GetIntBasisValue()
        If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then

            portifolioid = Request.QueryString("portifolioid")
            portifolio = Request.QueryString("portifolio")
            Dash = Request.QueryString("dash")
            dealCode = Request.QueryString("dealcode")
            currency = Request.QueryString("currency")
            'currency=txtCurrency.Text
            product = Request.QueryString("product")
        Else
            MsgBox("Please make sure all fields are selected")

        End If
        lblportfolioLoan.Text = portifolio
        lblDescription.Text = product
        lblDealtypeLoan.Text = dealCode
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
                    'cmbCustomerLoanM.Items.Add(Trim(dbinfo.drSQL.Item(1).ToString) + " " + Trim(dbinfo.drSQL.Item(0).ToString))
                    cmbCustomerLoanM.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
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

    Protected Sub cmdReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        btnIntDays.Enabled = False
        btnSaveMaturedLoan.Enabled = False
        'btnSaveMatured.Enabled = False

        txtDealAmount.Enabled = True
        txtNetInterestLoan.Enabled = True
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
        txtNetInterestLoan.BackColor = Color.White
        txtTenorLoan.BackColor = Color.White
        txtMaturityAmountLoan.BackColor = Color.White

        ChangedDaysBasis = False
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

    
    Protected Sub btnSaveMaturedLoan_Click(sender As Object, e As EventArgs) Handles btnSaveMaturedLoan.Click
        Dim dt1 As DateTime = DateTime.Parse(StartDateLoan.Text)
        Dim dt2 As DateTime = DateTime.Parse(MaturityDateLoan.Text)
        Dim daystomature As Integer = (dt2 - dt1).Days
        txtTenorLoan.Text = daystomature
        Dim ID As String
        ID = portifolioid
        '************************************************************************



        '************************************************************************************

        Dim Instruction As String = ""
        Dim InstructionM As String = ""

        If Trim(cmbInstructionLoan.Text) = "Other" Then
            Instruction = Replace(Trim(txtOtherLoan.Text), "'", """")
            Instruction = Replace(Trim(txtOtherLoan.Text), "&", "and")
        Else
            Instruction = Replace(Trim(cmbInstructionLoan.Text) & " -" & Trim(txtOtherLoan.Text), "'", """")
            Instruction = Replace(Trim(cmbInstructionLoan.Text) & " -" & Trim(txtOtherLoan.Text), "&", "and")
        End If

        If Trim(cmbInstructionMaturityL.Text) = "Other" Then
            InstructionM = Replace(Trim(txtOtherMaturity.Text), "'", """")
            InstructionM = Replace(Trim(txtOtherMaturity.Text), "&", "and")
        Else
            InstructionM = Replace(Trim(cmbInstructionMaturityL.Text) & " -" & Trim(txtOtherMaturity.Text), "'", """")
            InstructionM = Replace(Trim(cmbInstructionMaturityL.Text) & " -" & Trim(txtOtherMaturity.Text), "&", "and")
        End If


        Dim dealNum As String
        Dim Secured As String

        'get deal number
        'if deal is not the dashboard
        If UCase(Trim(Dash)) = "FALSE" Then
            dealNum = objectDealNumbers.GetDealNumber(Trim(lblDealtypeLoan.Text))
        Else
            dealNum = objectDealNumbers.GetDealNumberDash(Trim(lblDealtypeLoan.Text))
        End If



        'save deal
        'Determine if deal is on dashboard or not
        'if deal is not the dashboard

        If UCase(Trim(Dash)) = "FALSE" Then
            SavDls.strCurrentD = clientlogin_vars.strCurrentDirectory
            'SavDls.clients = client
            'SavDls.dataBaseName = dataBaseName

            Call SavDls.saveMaturedLoan(CDec(txtGrossLoan.Text), CDec(txtTaxAmountL.Text), daystomature, Session("SysDate"), RequireFrontAuthoriser, Trim(ID), Trim(lblDealtypeLoan.Text), Session("customerA"), CDate(StartDateLoan.Text) _
            , CDate(MaturityDateLoan.Text), CDec(Trim(txtDealAmount.Text)), CDec(Trim(txtMaturityAmountLoan.Text)), CDec(Trim(txtInterestRateLoan.Text)) _
            , CDec(Trim(txtNetInterestLoan.Text)), Int(Trim(txtTenorLoan.Text)), Instruction, dealNum, "Basic Loan", Secured, intAccruedToDate, Trim(currency) _
            , Session("loggedUserLog"), InterestBasisValue, Session("IsDealer"), globalvars_mmdeal.PrintPages, Session("serverName"), Session("dataBaseName"), InstructionM, "0")

        End If


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

        intAccruedToDate = 0         'Reinitialise interest accrued to date

        Call GetIntBasisValue() 'Reinitialise interest days basis

        'Reset controls
        btnCancelLoan.Enabled = False
        btnSaveMaturedLoan.Enabled = False

        txtDealAmount.Enabled = True
        txtInterestRateLoan.Enabled = True
        txtTenorLoan.Enabled = True
        StartDateLoan.Enabled = True
        MaturityDateLoan.Enabled = True
        cmbInstructionLoan.Enabled = True
        txtOtherLoan.Enabled = True
        cmbInstructionMaturityL.Enabled = True
        txtOtherMaturity.Enabled = True
        'CheckBox1.CheckState = CheckState.Unchecked
        'CheckBox1.Enabled = True

        txtDealAmount.Text = ""
        txtInterestRateLoan.Text = ""
        txtNetInterestLoan.Text = ""
        'cmbSecureLoan.Text = ""
        txtTenorLoan.Text = ""
        txtGrossLoan.Text = ""
        StartDateLoan.Text = CDate(Session("SysDate"))
        MaturityDateLoan.Text = CDate(Session("SysDate"))
        'cmbCustomerLoanM.Text = ""

        txtDealAmount.BackColor = Color.White
        txtInterestRateLoan.BackColor = Color.White
        txtTenorLoan.BackColor = Color.White
        txtMaturityAmountLoan.BackColor = Color.White


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

    Protected Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        Dim calc As New mmDeal.CalculationFunctions
        Dim dt1 As DateTime = DateTime.Parse(StartDateLoan.Text)
        Dim dt2 As DateTime = DateTime.Parse(MaturityDateLoan.Text)
        Dim daystomature As Integer = (dt1 - dt2).Days
        txtTenorLoan.Text = daystomature

        If checkMaturity.CheckBackValueThreshhold("mmbkval") < Math.Abs(DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateLoan.Text))) Then
            'MsgBox("You have exceeded maximum back value threshold", MsgBoxStyle.Exclamation)
            lblsuccess.Text = alert("You have exceeded maximum back value threshold", "MaturedLoan ")
            Exit Sub
        End If

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
        


        'On Error GoTo err1

        Dim x As Decimal

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(MaturityDateLoan.Text)) > 0 Then
            'MsgBox("Maturity date cannot be greater than business date if the deal has matured.", MsgBoxStyle.Information)
            lblsuccess.Text = alert("Maturity date cannot be greater than business date if the deal has matured.", "Maturity Date ")
            Exit Sub
        End If


        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateLoan.Text)) > 0 Then
            'MsgBox("Start date cannot be greater than business date", MsgBoxStyle.Information)
            lblsuccess.Text = alert("Start date cannot be greater than business date.", "Start Date ")
            Exit Sub
        End If


        'check fields
        If txtCurrency.Text = "" Then
            'MsgBox("Select Currency for deal.", MsgBoxStyle.Critical, "Currency")
            lblsuccess.Text = alert("Select Currency for deal..", "Currency ")
            Exit Sub
        End If

        If lblportfolioLoan.Text = "" Then
            'MsgBox("Select portfolio.", MsgBoxStyle.Critical, "Incomplete information")
            lblsuccess.Text = alert("Select portfolio..", "Incomplete information ")
            Exit Sub
        End If
        If lblDealtypeLoan.Text = "" Then
            'MsgBox("Select the product.", MsgBoxStyle.Critical, "Incomplete information")
            lblsuccess.Text = alert("Select the product.", "Incomplete information ")
            Exit Sub
        End If
        If cmbCustomerLoanM.Text = "" Then
            'MsgBox("Select Customer.", MsgBoxStyle.Critical, "Incomplete information")
            lblsuccess.Text = alert("Select Customer...", "Incomplete information ")
            Exit Sub
        End If

        If txtDealAmount.Text = "" Then
            'MsgBox("Insert deal amount.", MsgBoxStyle.Critical, "Incomplete information")
            lblsuccess.Text = alert("Insert deal amount...", "Incomplete information ")
            Exit Sub
        End If
        If txtInterestRateLoan.Text = "" Then
            'MsgBox("Insert the interest rate", MsgBoxStyle.Critical, "Incomplete information")
            lblsuccess.Text = alert("Insert the interest rate..", "Incomplete information ")
        End If
        If txtTenorLoan.Text = "" Then
            'MsgBox("Specify loan running period.", MsgBoxStyle.Critical, "Incomplete information")
            lblsuccess.Text = alert("Specify loan running period...", "Incomplete information ")
            Exit Sub
        End If
        If cmbInstructionLoan.Text = "" Then
            'MsgBox("Enter instruction for deal inception.", MsgBoxStyle.Information, "Incomplete information")
            lblsuccess.Text = alert("Enter instruction for deal inception...", "Incomplete information ")
            Exit Sub
        End If

        If checkacc.checkDealAccount() = "Y" Then
            If txtDealAccountLoan.Text = "" Then
                MsgBox("Deal account not selected", MsgBoxStyle.Exclamation, "Warning")
            End If
        End If

        'Check if proposed maturity date is not a non-business day
        If NonBusiness.NonBusinessDay(CDate(MaturityDateLoan.Text)) = True Then
            'MessageBox.Show("The selected maturity date is a not a business day.", "Non-Business Day")
            lblError.Text = alert("The selected maturity date is a not a business day.", "Non-Business Day")
            Exit Sub
        End If
        If Holidays.Holidays(CDate(MaturityDateLoan.Text), getBaseCurrency) = True Then
            'MessageBox.Show("The selected maturity date is a not a business day.", "Non-Business Day"
            lblError.Text = alert("The selected maturity date is a not a business day.", "Non-Business Day")
            Exit Sub
        End If


        '*****************End
        If ChangedDaysBasis = True Then
            Call calc.CalcEngine1(CDec(txtTenorLoan.Text), CDec(txtInterestRateLoan.Text), CDec(txtDealAmount.Text), CDec(txtTaxRateL.Text), InterestBasisValue)
        Else
            Call calc.CalcEngine(CDec(txtTenorLoan.Text), CDec(txtInterestRateLoan.Text), CDec(txtDealAmount.Text), CDec(txtTaxRateL.Text), Trim(currency))
            calc.IntBasis(Trim(currency))
        End If

        txtMaturityAmountLoan.Text = calc.maturityAmount.ToString
        txtNetInterestLoan.Text = calc.netInt.ToString
        txtTaxAmountL.Text = calc.taxAmount
        txtGrossLoan.Text = calc.grossInt

        x = DateDiff(DateInterval.Day, CDate(MaturityDateLoan.Text), CDate(StartDateLoan.Text), )
        If x < 0 Then
            x = Math.Abs(x)
            daystomature = CDec(txtTenorLoan.Text) - x

            If ChangedDaysBasis = True Then
                calc.CalcEngine1(x, CDec(txtInterestRateLoan.Text), CDec(txtDealAmount.Text), 0, InterestBasisValue)
            Else
                calc.CalcEngine(x, CDec(txtInterestRateLoan.Text), CDec(txtDealAmount.Text), 0, Trim(currency))
            End If

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



        'Reset controls
        btnCancelLoan.Enabled = False
        btnSaveMaturedLoan.Enabled = True

        txtDealAmount.Enabled = False
        txtInterestRateLoan.Enabled = False
        txtTenorLoan.Enabled = False
        StartDateLoan.Enabled = False
        MaturityDateLoan.Enabled = False
        cmbInstructionLoan.Enabled = False
        txtOtherLoan.Enabled = False
        txtTaxRateL.Enabled = False
        cmbInstructionMaturityL.Enabled = False
        txtOtherMaturity.Enabled = False
        'CheckBox1.Enabled = False
        'cmdShortsL.Enabled = False
        txtDealAccountLoan.Enabled = False

        txtDealAmount.BackColor = Color.LightYellow
        txtInterestRateLoan.BackColor = Color.LightYellow
        txtTenorLoan.BackColor = Color.LightYellow
        txtMaturityAmountLoan.BackColor = Color.LightYellow

        '************************************************************************

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

        lblsuccess.Text = success("Deal Validated successfully", "MaturedLoan Validated")

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
    'Public Function checkDealAccount() As String
    '    Dim x As String
    '    Try
    '        'validate username first
    '        strSQL = "Select [value] from systemparameters where [parameter]='promptacc'"
    '        cnSQL = New SqlConnection(Session("ConnectionString"))
    '        cnSQL.Open()
    '        cmSQL = New SqlCommand(strSQL, cnSQL)
    '        drSQL = cmSQL.ExecuteReader()

    '        If drSQL.HasRows = True Then
    '            Do While drSQL.Read
    '                x = Trim(drSQL.Item(0).ToString)
    '            Loop
    '        End If


    '        ' Close and Clean up objects
    '        drSQL.Close()
    '        cnSQL.Close()
    '        Dispose()
    '        cnSQL.Dispose()

    '    Catch ex As SqlException
    '        MsgBox(ex.Message, MsgBoxStyle.Critical)

    '        'Log the event *****************************************************
    '        object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
    '        '************************END****************************************
    '    End Try
    '    Return x
    'End Function

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("newdeal.aspx")
    End Sub

    Protected Sub btnCancelLoan_Click(sender As Object, e As EventArgs) Handles btnCancelLoan.Click
        btnCancelLoan.Enabled = False
        btnSaveMaturedLoan.Enabled = False
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
    Protected Sub GridSecurity_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        If e.CommandName = "select" Then
            MsgBox(txtdealref.Text)
            txtdealref.Text = e.Item.Cells(0).Value.ToString()
            'lblInfo.Text = txtdealref.Text
            'Call DeleteSecurityTemp(txtdealref.Text)
        End If
    End Sub
    Protected Sub cmbCustomerLoanM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCustomerLoanM.SelectedIndexChanged
        'MsgBox("fgjj")
        Call GetHist(cmbCustomerLoanM.SelectedValue.ToString(), "Basic Loan", "USD")
        Call GetCustomerAddressInfo(cmbCustomerLoanM.SelectedValue.ToString())
    End Sub
    Protected Sub GridSecurity_ItemDeleted(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        If e.CommandName = "select" Then
            ' MsgBox(txtdealref.Text)
            txtdealref.Text = e.Item.Cells(0).Value.ToString()
            'lblInfo.Text = txtdealref.Text
            '  Call DeleteSecurityTemp(txtdealref.Text)
        End If
    End Sub
End Class
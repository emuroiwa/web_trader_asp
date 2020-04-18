Imports System.Data.SqlClient
Imports sys_ui
Imports System.Drawing

Public Class SecuritySell
    Inherits System.Web.UI.Page
    Public intAccruedToDateCal As Decimal
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private object_userlog As New usrlog.usrlog
    Private SavDls As New csvptt.csvptt
    Private CustName As String
    Private globalvars_mmdeal As New GlobalVars.mmDeal   'instance of the userlogins class
    Private InterestBasisValue As Integer 'Stores the current value for use as the interest dayas basis
    Private checkMaturity As New mmDeal.DealMaturityCheck
    Private intAccruedToDate As Decimal
    Private daystomature As Decimal
    Private ChangedDaysBasis As Boolean = False 'Value indicating if the days basis default has been manually changed
    Private SecureUsingOther As Boolean
    'Private crStatus As String 'Stores counterparty limit status 1=exceeded 2= ok
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
    Private limitsch As New usrlmt.usrlmt
    Dim objectDealNumbers As New mmDeal.DeaNumbers
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim pg As Multisell = TryCast(Page.PreviousPage, Multisell)
        'If pg IsNot Nothing Then
        '    Dim grid As GridView = pg.Grid1
        '    form1.Controls.Add(grid)
        'End If
        If Not IsPostBack Then
            If Session("Single") = "Single" Then
                Call Details()
            Else

                MsgBox(Session("loadGrid"))

                Call Detailsmulti()
                MsgBox(Session("Portname"))
                'cmbportfolio.Items.FindByText(Session("Portname")).Selected = True
                cmbportfolio.Visible = False
                lblportfolio.Visible = True
                lblportfolio.Text = Session("Portname")
                'cmbportfolio.Text = Session("Portname")
                Call GetProduct(Session("portid"))
                cmbportfolio.Enabled = False
                txtIntDays.Text = GetIntBasis(Trim(txtCurrency.Text))
                Call getmaturity()
                Call populateGrid()
            End If

            Call GetDealPortfolio(lblRef.Text)
            Call Deal_Instructions()
            Call LoadCustomers()
        End If
    End Sub
    Private Sub populateGrid()
        Dim dt As New DataTable()
        dt.Columns.Add(New DataColumn("Item #", GetType(Integer)))
        dt.Columns.Add(New DataColumn("SecRef", GetType(String)))
        dt.Columns.Add(New DataColumn("dealref", GetType(String)))
        dt.Columns.Add(New DataColumn("SelAmt", GetType(String)))
        dt.Columns.Add(New DataColumn("cost", GetType(String)))
        dt.Columns.Add(New DataColumn("profit", GetType(String)))
        dt.Columns.Add(New DataColumn("presentV", GetType(String)))
        dt.Columns.Add(New DataColumn("matamt", GetType(String)))


        'Session("loadGrid") = Session("loadGrid") & "|" & lblpuref.Text & "," & lblreff.Text & "," & txtSellAmt.Text & "," & cost.Text & "," & profit.Text & "," & sellpv.Text & "," & maturityAmt
        Dim arr() As String = Split(Session("loadGrid"), "|")
        'arr.ElementAt(1).Remove(1)

        Dim arrlen As Integer = arr.Length
        Dim x As Integer

        For x = 1 To arr.Length - 1
            Dim arr2() As String = Split(arr(x), ",")
            Dim dr As DataRow = dt.NewRow()
            dr("SecRef") = arr2(0)
            dr("dealref") = arr2(1)
            dr("SelAmt") = arr2(2)
            dr("cost") = arr2(3)
            dr("profit") = arr2(4)
            dr("presentV") = arr2(5)
            dr("matamt") = arr2(6)
            dt.Rows.Add(dr)
        Next

        Me.Grid1.Visible = True

        Grid1.DataSource = dt

        Grid1.DataBind()
    End Sub
    Private Sub getmaturity()
        Dim x As Long
        Try

            EndDateSale.Text = DateAdd(DateInterval.Day, CDec(txtTenorSale.Text), CDate(StartDateSale.Text))
            If x < 0 Then
                lblError.Text = alert("Maturity date cannot be less than start date", "Maturity Date")
                StartDateSale.Text = CDate(Session("SysDate"))
                EndDateSale.Focus()
                Exit Sub
            End If

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
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
                    cmbCustomer.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
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
    Private Sub Detailsmulti()

        StartDateSale.Text = Session("SysDate")
        ' EndDateSale.Text = Session("saleMaturityD")
        txtMaturityAmountSale.Text = Session("MultiFutureValue")
        txtTenorSale.Text = Session("MultiTenor")
        txtDiscountRateSale.Text = Session("MultiDiscount")
        SellingCaption.Text = Session("saleTB")
        lblRef.Text = Session("saleDealRef")
        txtIntDays.Text = Session("DaysBasis")
        txtCurrency.Text = Session("MultiSAle")
        'commRate.Text = Session("commrate")
        'lblPurchaseStart.Text = Session("salestartD")
        txtPL.Text = Session("MultiGain")
        txtCV.Text = Session("MultiCost")
        txtPV.Text = Session("MultiPV")
        'commRate = Session("commrate")
        lblPurchaseRate.Text = Session(" lblbreakeven")
        If Session("MultiOPT") = "D" Then
            RdSellOPT.SelectedValue = "discount"
        Else
            RdSellOPT.SelectedValue = "yield"
        End If

    End Sub
    Private Sub Details()
        StartDateSale.Text = Session("salestartD")
        EndDateSale.Text = Session("saleMaturityD")
        txtMaturityAmountSale.Text = Session("saleFutureValue")
        txtTenorSale.Text = Session("saleTenor")
        txtDiscountRateSale.Text = Session("saleDiscount")
        SellingCaption.Text = Session("saleTB")
        lblRef.Text = Session("saleDealRef")
        txtIntDays.Text = Session("DaysBasis")
        txtCurrency.Text = Session("currencySAle")
        commRate.Text = Session("commrate")
        lblPurchaseStart.Text = Session("salestartD")
        txtPL.Text = Session("Gain")
        txtCV.Text = Session("Cost")
        txtPV.Text = Session("PV")
        'commRate = Session("commrate")
        lblPurchaseRate.Text = Session(" lblbreakeven")
        If Session("saleOPT") = "Discount" Then
            RdSellOPT.SelectedValue = "discount"
        Else
            RdSellOPT.SelectedValue = "yield"
        End If

    End Sub
    Public Sub GetDealPortfolio(ByVal dealref As String)

        Try


            strSQL = "Select distinct DEALS_LIVE.portfolioid,PORTFOLIOSTRUCTURE.portfolioname from  portfoliostructure join DEALS_LIVE" & _
          " on DEALS_LIVE.portfolioid=portfoliostructure.portfolioid where DEALS_LIVE.DealReference='" & Trim(dealref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    cmbportfolio.Items.Add(New ListItem(Trim(drSQL.Item("portfolioname").ToString) + "  " + Trim(drSQL.Item("portfolioid").ToString), Trim(drSQL.Item("portfolioid").ToString)))
                Loop
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            'SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message)
            '************************END****************************************
        End Try
    End Sub

    Protected Sub cmbportfolio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbportfolio.SelectedIndexChanged
        Call GetProduct(cmbportfolio.SelectedValue.ToString)
    End Sub
    Private Sub GetProduct(ByVal currentportfolio As String)

        '' Dim itmx As ListViewItem

        Try
            strSQL = "Select dealtype,dealtypedescription from portfolioinformation join dealtypes  on dealtypes.deal_code=portfolioinformation.dealtype" & _
            "   where portfolioinformation.portfolioid = '" & currentportfolio & "' and dealtypes.dealbasictype = 'D' and  dealtypes.discount = 'Y' and currency='" & Trim(txtCurrency.Text) & "'"

            '      strSQL = "Select dealtype,dealtypeDescription from portfolioinformation join dealtypes  on dealtypes.deal_code=portfolioinformation.dealtype" & _
            '"   where portfolioinformation.portfolioid = '" & currentportfolio & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            ''lstCodes.Items.Clear()
            ''linkProductCode.Text = ""

            Do While drSQL.Read

                cmbproduct.Items.Add(New ListItem(Trim(drSQL.Item(0).ToString) + "  " + Trim(drSQL.Item(1).ToString), Trim(drSQL.Item(0).ToString)))
                txtdealdescSale.Text = drSQL.Item(1).ToString
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

        Catch er As Exception

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
                        cmdInstrucSaleM.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                    ElseIf drSQL.Item("purpose").ToString.Equals("I") Then
                        cmdInstrucSale.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                    End If
                Loop


            End If

            cmdInstrucSale.Items.Add("Other")
            cmdInstrucSaleM.Items.Add("Other")

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
    Public Function GetIntBasis(ByVal ccy As String) As String
        Dim x As String = ""
        Try

            strSQL = "Select daysbasis from currencies where currencycode ='" & Trim(ccy) & "'"
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


            Return x

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try

    End Function

    Protected Sub chkTaxStatus_CheckedChanged(sender As Object, e As EventArgs) Handles chkTaxStatus.CheckedChanged
        If chkTaxStatus.Checked = True Then
            cmbTaxSell.Enabled = True
        End If
        If chkTaxStatus.Checked = False Then
            txtTaxRateSale.Text = ""
            cmbTaxSell.Enabled = False
        End If
    End Sub

    Protected Sub cmbTaxSell_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTaxSell.SelectedIndexChanged
        If cmbTaxSell.SelectedValue.ToString = "TAXL" Then
            txtTaxRateSale.Text = Trim(CDec(15))
        Else
            txtTaxRateSale.Text = Trim(CDec(5))
        End If
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        btnValidateSale.Enabled = True
        btnSaveSale.Enabled = False

        Call EnableFieldsSale()
        'set control back color
        DealAmountSale.BackColor = Color.LightYellow
        txtYieldSale.BackColor = Color.LightYellow
        txtTenorSale.BackColor = Color.LightYellow
        txtDiscountRateSale.BackColor = Color.LightYellow
        txtMaturityAmountSale.BackColor = Color.LightYellow
        txtTenorSale.BackColor = Color.LightYellow

        Call GetIntBasisValue()
    End Sub
    Private Sub EnableFieldsSale()
        txtDealAccountSale.Enabled = True

        cmdInstrucSaleM.Enabled = True

        cmdInstrucSale.Enabled = True
        btnSaveSale.Enabled = False
        btnReset.Enabled = False
        btnValidateSale.Enabled = True
        chkTaxStatus.Enabled = True


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
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetIntBasisValue", Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub

    Protected Sub RdSellOPT_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RdSellOPT.SelectedIndexChanged
        If RdSellOPT.SelectedValue = "discount" Then
            btnValidateSale.Enabled = True
            DealAmountSale.Enabled = False
            txtYieldSale.Enabled = False
            cmdInstrucSale.Enabled = True

            DealAmountSale.Text = ""
            txtYieldSale.Text = ""

            'set control color
            txtDiscountRateSale.BackColor = Color.White
            txtMaturityAmountSale.BackColor = Color.White
            txtTenorSale.BackColor = Color.White

            DealAmountSale.BackColor = Color.LightYellow
            txtYieldSale.BackColor = Color.LightYellow
        Else
            btnValidateSale.Enabled = True
            txtDiscountRateSale.Enabled = False
            txtMaturityAmountSale.Enabled = False
            txtMaturityAmountSale.Text = ""
            txtDiscountRateSale.Text = ""
            cmdInstrucSale.Enabled = True
            'txttxtOtherSale.Enabled = True
            'set control back color
            DealAmountSale.BackColor = Color.White
            txtYieldSale.BackColor = Color.White
            txtTenorSale.BackColor = Color.White
            txtDiscountRateSale.BackColor = Color.LightYellow
            txtMaturityAmountSale.BackColor = Color.LightYellow
        End If
    End Sub

    Protected Sub cmbCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCustomer.SelectedIndexChanged
        Call GetHist(cmbCustomer.SelectedValue.ToString(), "Discount Sale ", "USD")
        Call GetCustomerAddressInfo(cmbCustomer.SelectedValue.ToString())
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
            lblError.Text = alert(ex.Message, "Error")
        End Try


    End Sub

    Protected Sub btnValidateSale_Click(sender As Object, e As EventArgs) Handles btnValidateSale.Click
        Dim calc As New mmDeal.CalculationFunctions

        'If checkMaturity.CheckBackValueThreshhold("mmbkval") < Math.Abs(DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateSale.Text))) Then
        '    MsgBox("You have exceeded maximum back value threshold", MsgBoxStyle.Exclamation)
        '    Exit Sub
        'End If

        If getSysParam("fundrequired") = "Y" Then 'Check if the accounts are required
            'if its a discount deal check if the interest recievable account has been setup for the product first
            If RdSellOPT.SelectedValue = "discount" Then
                If Trim(GetInterestAccount(cmbproduct.SelectedValue.ToString)) = "" Then
                    MsgBox("interest account not set for product, dealing will be disabled", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            End If
        End If


        Dim limitsch As New usrlmt.usrlmt
        limitsch.clients = client
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
        object_userlog.SendData("REQUESTUSERS|" & Session("username"), Session("serverName"), Session("client"))

        On Error GoTo err2

        'Dim x1 As Integer
        'Dim ID As String
        ''Get selected portfolio id
        'x1 = cmbPortfolioDiscSale.SelectedIndex
        'ID = Trim(portIDxx.Text) 'MyPortfolioCollectionID.Item(x1 + 1).ToString

        On Error GoTo err1
        If getSysParam("fundrequired") = "Y" Then 'Check if the accounts are required

            If Trim(txtDealAccountSale.Text) = "" Then
                MsgBox("Please select Funding Account", MsgBoxStyle.Critical)
                'TabControl1.SelectedTab = TabControl1.TabPages(0)
                Exit Sub
            End If

            If Trim(txtSellRecieve.Text) = "" Then
                MsgBox("Please select product Deal Account", MsgBoxStyle.Critical)
                'TabControl1.SelectedTab = TabControl1.TabPages(0)
                Exit Sub
            End If
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(EndDateSale.Text)) < 0 Then
            MsgBox("Maturity date cannot be less than business date", MsgBoxStyle.Information)
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(EndDateSale.Text)) = 0 Then
            MsgBox("This deal has already matured.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateSale.Text)) > 0 Then
            MsgBox("Start date cannot be greater than business date", MsgBoxStyle.Information)
            Exit Sub
        End If
        If Session("Single") = "Single" Then
            If Format(CDate(StartDateSale.Text), "Short Date") < CDate(Format(lblPurchaseStart.Text, "Short Date")) Then
                MsgBox("Start date of sell cannot be less than start date of the purchase you are selling", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(EndDateSale.Text)) = 0 Then
            MsgBox("Tenor cannot be zero.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If checkDealAccount() = "Y" Then
            If txtDealAccountSale.Text = "" Then
                MsgBox("Deal account ", MsgBoxStyle.Exclamation, "Stop")
                Exit Sub
            End If
        End If

        Dim x As Decimal
        If RdSellOPT.SelectedValue = "discount" Then
            '    'Check fields
            '    If Session("Single") = "Single" Then
            '        'do not check this condition for multi sell of securities
            '        Dim TenorOGGG As Integer
            '        TenorOGGG = Trim(TenorOG.Text)
            '        'Check if maturity date is not greater that maturity of purchase
            '        If Format(CDate(StartDateSale.Text), "Short Date") < CDate(Session("SysDate")) Then
            '            TenorOG.Text = Int(TenorOG.Text) + (DateDiff(DateInterval.Day, CDate(StartDateSale.Text), CDate(Session("SysDate"))))
            '        End If

            '        If Int(TenorOG.Text) < Int(txtTenorSale.Text) Then
            '            MsgBox("Maturity date of sale cannot be greater than that of purchase", MsgBoxStyle.Critical, "Maturity")
            '            TenorOG.Text = TenorOGGG
            '            Exit Sub
            '        End If
            '    End If
            If Session("Single") = "Single" Then
                If Trim(cmbportfolio.SelectedValue.ToString) = "" Then
                    MsgBox("Select Portfolio", MsgBoxStyle.Critical, "Incomplete information")
                    Exit Sub
                End If
            Else
                If lblportfolio.Text = "" Then
                    MsgBox("Select Portfolio", MsgBoxStyle.Critical, "Incomplete information")
                    Exit Sub
                End If
            End If
            If cmbproduct.SelectedValue.ToString = "" Then
                MsgBox("Select the product", MsgBoxStyle.Critical, "Incomplete information")
                Exit Sub
            End If

            If txtCurrency.Text = "" Then
                MsgBox("Select Currency for deal.", MsgBoxStyle.Critical, "Currency")
                Exit Sub
            End If

            'If cmbCustomer.SelectedValue.ToString = "" Then
            '    MsgBox("Select customer", MsgBoxStyle.Critical, "Incomplete information")
            '    Exit Sub
            'End If
            If txtMaturityAmountSale.Text = "" Then
                MsgBox("Enter maturity amount", MsgBoxStyle.Critical, "Incomplete information")
                Exit Sub
            End If
            If txtDiscountRateSale.Text = "" Then
                MsgBox("Enter discount rate", MsgBoxStyle.Critical, "Incomplete information")
                Exit Sub
            End If
            If txtTenorSale.Text = "" Then
                MsgBox("Input tenor", MsgBoxStyle.Critical, "Incomplete information")
                Exit Sub
            End If
            If cmdInstrucSale.Text = "" Then
                MsgBox("Enter instruction fro deal inception.", MsgBoxStyle.Information)
                Exit Sub
            End If
            If cmdInstrucSale.Text = "Other" And txtOtherSale.Text = "" Then
                MsgBox("Please specify other instructions for deal inception", MsgBoxStyle.Information)
                Exit Sub
            End If
            If cmdInstrucSaleM.Text = "" Then
                MsgBox("Enter instruction for deal maturity.", MsgBoxStyle.Information)
                Exit Sub
            End If
            If cmdInstrucSaleM.Text = "Other" And txtOtherSaleM.Text = "" Then
                MsgBox("Please specify other instructions for deal maturity", MsgBoxStyle.Information)
                Exit Sub
            End If

            'Check if proposed maturity date is not a non-business day
            If checkMaturity.NonBusinessDay(CDate(EndDateSale.Text)) = True Then
                MsgBox("The selected maturity date is a not a business day.", "Non-Business Day")
                Exit Sub
            End If
            'If checkMaturity.Holidays(CDate(EndDateSale.Text), getBaseCurrency) = True Then
            '    MsgBox("The selected maturity date is a holiday.", "Non-Business Day")
            '    Exit Sub
            'End If
            '****************************************end

            If ChangedDaysBasis = True Then
                calc.calcEngineDiscount1(CDec(txtTenorSale.Text), CDec(txtTaxRateSale.Text), CDec(txtDiscountRateSale.Text) _
                , CDec(txtMaturityAmountSale.Text), CDec(commRate.Text), InterestBasisValue)
            Else
                calc.calcEngineDiscount(CDec(txtTenorSale.Text), CDec(txtTaxRateSale.Text), CDec(txtDiscountRateSale.Text) _
                , CDec(txtMaturityAmountSale.Text), CDec(commRate.Text), Trim(txtCurrency.Text))
                IntBasis(Trim(txtCurrency.Text))
            End If

            netInterestSale.Text = calc.netInt.ToString
            txtGrossSale.Text = calc.grossInt.ToString
            txtTaxAmountSale.Text = calc.taxAmount.ToString
            DealAmountSale.Text = Format(CDec(calc.maturityAmount), "###,###,###.00").ToString
            txtYieldSale.Text = calc.YieldRateDerived.ToString

            'Calculate interest accrued to date for back valued deals
            x = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateSale.Text), )
            If x < 0 Then
                x = Math.Abs(x)
                daystomature = CDec(txtTenorSale.Text) - x
                If ChangedDaysBasis = True Then
                    calc.calcEngineDiscount1(x, CDec(txtTaxRateSale.Text), CDec(txtDiscountRateSale.Text) _
                , CDec(txtMaturityAmountSale.Text), CDec(commRate.Text), InterestBasisValue)
                    intAccruedToDate = intAccruedToDateCal
                Else
                    calc.calcEngineDiscount(x, CDec(txtTaxRateSale.Text), CDec(txtDiscountRateSale.Text) _
                                   , CDec(txtMaturityAmountSale.Text), CDec(commRate.Text), Trim(txtCurrency.Text))
                    intAccruedToDate = intAccruedToDateCal
                End If
            Else
                intAccruedToDate = 0
                daystomature = CDec(txtTenorSale.Text)
            End If

            'Check if entered discount rate is not less than the purchase rate
            'do not check this condition for multi sell of securities

            'If lstSell.Visible = False Then
            '    If CDec(txtDiscountRateSale.Text) > CDec(lblPurchaseRate.Text) Then
            '        WarningMessage = "The discount rate entered is greater than the MTM rate. Sell made at a loss. Sell rate : " & Trim(txtDiscountRateSale.Text) & "% MTM rate : " & Trim(lblPurchaseRate.Text)
            '        If MessageBox.Show(WarningMessage & "% Proceed with rate?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Yes Then

            '        Else
            '            Exit Sub
            '        End If
            '    Else
            '        WarningMessage = ""
            '    End If
            'End If

            'Check for invalid or out of valid range discount rates
            If CDec(netInterestSale.Text) >= CDec(DealAmountSale.Text) Then
                MsgBox("The discount rate entered is not valid. Change the yield or discount rate", MsgBoxStyle.Exclamation, "Invalid Discount Rate")
                Exit Sub
            End If


            TranxLimitVal = limitsch.PeriodLimitValidation(Int(txtTenorSale.Text), "DepositLimit", Session("username"), _
                            CDbl(DealAmountSale.Text), Trim(txtCurrency.Text), Trim(cmbproduct.SelectedValue.ToString))


            If TranxLimitVal(0) = "1" Then
                MsgBox("Transaction Limit exceeded" & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       TranxLimitVal(1) & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       "Limit Amount             :   " & Format(CDbl(TranxLimitVal(2)), "###,###.00") & vbCrLf & _
                       "Transaction Amount :   " & DealAmountSale.Text, MsgBoxStyle.Information, "Limits")

                If getSysParam("tranlmt") = "warn" Then
                    MsgBox("Limit violation will be recorded", MsgBoxStyle.Exclamation, "Transaction Limit exceeded")
                ElseIf getSysParam("tranlmt") = "stop" Then
                    MsgBox("Limit exceeded, you cannot proceed with the transaction", MsgBoxStyle.Critical, "Transaction Limit exceeded")
                    Exit Sub
                End If

            End If


            'Counterparty limits validation
            If limitsch.IsLimitSet(Trim(cmbCustomer.SelectedValue.ToString), "Deposit") = True Then
                If limitsch.CheckCounterpartyLimit(Trim(cmbCustomer.SelectedValue.ToString), Session("DealStructure"), CDec(DealAmountSale.Text), Trim(txtCurrency.Text), CDate(CDate(EndDateSale.Text))) = False Then

                    Session("crStatus") = "1"

                    If CounterpartyLimitViolation(CDec(DealAmountSale.Text), Session("DealStructure"), txtCurrency.Text, CDate(CDate(EndDateSale.Text))) = "stop" Then
                        Exit Sub
                    End If

                Else
                    Session("crStatus") = "2"
                End If
            Else
                Session("crStatus") = "3"
            End If


            CustName = cmbCustomer.SelectedValue.ToString

            '   If limitsch.CheckDailyLimit(Session("username"), Trim(cmbproduct.SelectedValue.ToString), CDec(DealAmountSale.Text), Trim(txtdealdescSale.Text), Trim(txtCurrency.Text)) = False Or _
            'limitsch.CheckdealSizeLimit(Session("username"), Trim(cmbproduct.SelectedValue.ToString), CDec(DealAmountSale.Text), Trim(txtdealdescSale.Text), Trim(txtCurrency.Text)) = False Or _
            'limitsch.CheckProductPos(Trim(cmbproduct.SelectedValue.ToString), CDec(DealAmountSale.Text), "+", Trim(txtCurrency.Text)) = False Or _
            'limitsch.CheckPortfolioPos(ID, CDec(DealAmountSale.Text), "+", Trim(txtCurrency.Text)) = False Or _
            'WarningMessage <> "" Then


            '       RefMessageDealsize = limitsch.RefMessageDealsize
            '       RefMessageDaily = limitsch.RefMessageDaily
            '       referalRequired = limitsch.referalRequired
            '       refAmount = limitsch.refAmount
            '       RefDealType = limitsch.RefDealType
            '       dealsizeBroken = limitsch.dealsizeBroken
            '       dailyBroken = limitsch.dailyBroken
            '       PortfolioBroken = limitsch.PortfolioBroken
            '       ProductBroken = limitsch.ProductBroken
            '       CounterPartyBroken = limitsch.CounterPartyBroken
            '       Limitauthoriser = limitsch.Limitauthoriser
            '       RefMessagePortfolio = limitsch.RefMessagePortfolio
            '       RefMessageProduct = limitsch.RefMessageProduct
            '       RefMessageCounterparty = limitsch.RefMessageCounterparty

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
            '    ref.RefMessageCapitalLoss = "Capital loss, Sell rate : " & Trim(txtDiscountRateSale.Text) & "% MTM rate : " & Trim(lblPurchaseRate.Text) & "%"

            '    ref.DealCustomer = Trim(cmbCustomer.SelectedValue.ToString)
            '    ref.DealDealAmount = CDec(DealAmountSale.Text)
            '    ref.DealDealInterestRate = Trim(txtDiscountRateSale.Text)
            '    ref.DealTenor = Int(txtTenorSale.Text)
            '    ref.DealMaturityAmount = CDec(txtMaturityAmountSale.Text)
            '    ref.dealGrossInterest = CDec(txtGrossSale.Text)
            '    ref.dealPortfolio = Trim(Trim(cmbportfolio.SelectedValue.ToString))
            '    ref.dealDealType = Trim(cmbproduct.SelectedValue.ToString)
            '    ref.DealDiscount = "Y"
            '    ref.DealSecurityID = Trim(lblRef.Text)
            '    ref.DealCurrency = Trim(txtCurrency.Text)
            '    ref.CustName = customerN

            '    'do not check this condition for multi sell of securities
            '    If lstSell.Visible = True Then
            '        txtPL.Text = "0"
            '    End If

            '    ref.DealProfit = CDec(txtPL.Text)
            '    ref.DealTransType = "Security Sell"

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
            '    referalResponse = 3 're-initialise variable
            'End If
            '' End If
            '************************************************************************************
            'refIDRecieve = refid
            Call disableControlsSale()

        Else

            'Check fields
            If Session("Single") = "Single" Then
                'do not check this condition for multi sell of securities
                Dim TenorOGG As Integer
                TenorOGG = CDec(TenorOG.Text)
                'Check if maturity date is not greater that maturity of purchase
                If Format(CDate(StartDateSale.Text), "Short Date") < CDate(Session("SysDate")) Then
                    TenorOG.Text = CDec(TenorOG.Text) + (DateDiff(DateInterval.Day, CDate(StartDateSale.Text), CDate(Session("SysDate"))))
                End If

                If CDec(TenorOG.Text) < CDec(txtTenorSale.Text) Then
                    MsgBox("Maturity date of sale cannot be greater than that of purchase", MsgBoxStyle.Critical, "Maturity")
                    TenorOG.Text = TenorOGG
                    Exit Sub
                End If


                If Trim(cmbportfolio.SelectedValue.ToString) = "" Then
                    MsgBox("Select Portfolio", MsgBoxStyle.Critical, "Incomplete information")
                    Exit Sub
                End If
            End If
            If cmbproduct.SelectedValue.ToString = "" Then
                MsgBox("Select the product", MsgBoxStyle.Critical, "Incomplete information")
                Exit Sub
            End If

            If txtCurrency.Text = "" Then
                MsgBox("Select Currency for deal.", MsgBoxStyle.Critical, "Currency")
                Exit Sub
            End If

            If cmbCustomer.SelectedValue.ToString = "" Then
                MsgBox("Select customer", MsgBoxStyle.Critical, "Incomplete information")
                Exit Sub
            End If
            If DealAmountSale.Text = "" Then
                MsgBox("Enter deal amount", MsgBoxStyle.Critical, "Incomplete information")
                Exit Sub
            End If
            If txtYieldSale.Text = "" Then
                MsgBox("Enter yield rate", MsgBoxStyle.Critical, "Incomplete information")
                Exit Sub
            End If
            If txtTenorSale.Text = "" Then
                MsgBox("Input tenor", MsgBoxStyle.Critical, "Incomplete information")
                Exit Sub
            End If
            If cmdInstrucSaleM.Text = "" Then
                MsgBox("Enter instruction for deal maturity.", MsgBoxStyle.Information)
                Exit Sub
            End If
            If cmdInstrucSaleM.Text = "Other" And txtOtherSaleM.Text = "" Then
                MsgBox("Please specify other instructions for deal maturity", MsgBoxStyle.Information)
                Exit Sub
            End If

            'Check if proposed maturity date is not a non-business day
            If checkMaturity.NonBusinessDay(CDate(EndDateSale.Text)) = True Then
                MsgBox("The selected maturity date is a not a business day.", "Non-Business Day")
                Exit Sub
            End If
            If checkMaturity.Holidays(CDate(EndDateSale.Text), getBaseCurrency) = True Then
                MsgBox("The selected maturity date is a holiday.", "Non-Business Day")
                Exit Sub
            End If
            '****************************************end

            If ChangedDaysBasis = True Then
                calc.CalcEngineYield1(CDec(txtTenorSale.Text), CDec(txtTaxRateSale.Text), CDec(txtYieldSale.Text), CDec(DealAmountSale.Text), InterestBasisValue)
            Else
                calc.CalcEngineYield(CDec(txtTenorSale.Text), CDec(txtTaxRateSale.Text), CDec(txtYieldSale.Text), CDec(DealAmountSale.Text), Trim(txtCurrency.Text))
                calc.IntBasis(Trim(txtCurrency.Text))
            End If

            netInterestSale.Text = calc.netInt.ToString
            txtGrossSale.Text = calc.grossInt.ToString
            txtTaxAmountSale.Text = calc.taxAmount.ToString
            txtMaturityAmountSale.Text = Format(CDec(calc.maturityAmount), "###,###,###.00").ToString
            txtDiscountRateSale.Text = calc.DiscountRateDerived.ToString


            'Calculate interest accrued to date for back valued deals
            x = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(StartDateSale.Text))
            If x < 0 Then
                x = Math.Abs(x)
                daystomature = CDec(txtTenorSale.Text) - x

                If ChangedDaysBasis = True Then
                    calc.CalcEngineYield1(x, CDec(txtTaxRateSale.Text), CDec(txtYieldSale.Text), CDec(DealAmountSale.Text), InterestBasisValue)
                    intAccruedToDate = intAccruedToDateCal
                Else
                    calc.CalcEngineYield(x, CDec(txtTaxRateSale.Text), CDec(txtYieldSale.Text), CDec(DealAmountSale.Text), Trim(txtCurrency.Text))
                    intAccruedToDate = intAccruedToDateCal
                End If
            Else
                intAccruedToDate = 0
                daystomature = CDec(txtTenorSale.Text)
            End If
            ''''''''''''''''''
            'End If
            'Check if entered discount rate is not less than the purchase rate
            'If lstSell.Visible = False Then
            '    'do not check this condition for multi sell of securities
            '    If CDec(txtYieldSale.Text) > CDec(lblPurchaseRate.Text) Then
            '        WarningMessage = "The yield rate entered is greater than the MTM rate. Sell made at a loss. Sell rate : " & Trim(txtYieldSale.Text) & "% MTM rate : " & Trim(lblPurchaseRate.Text)
            '        If MessageBox.Show(WarningMessage & "% Proceed with rate?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Yes Then

            '        Else
            '            Exit Sub
            '        End If
            '    Else
            '        WarningMessage = ""
            '    End If
            'End If

            CustName = cmbCustomer.SelectedValue.ToString

            'Check for invalid or out of valid range discount rates
            If CDec(netInterestSale.Text) >= CDec(DealAmountSale.Text) Then
                MsgBox("The discount rate entered is not valid. Change the yield or discount rate", MsgBoxStyle.Exclamation, "Invalid Discount Rate")
                Exit Sub
            End If


            TranxLimitVal = limitsch.PeriodLimitValidation(Int(txtTenorSale.Text), "DepositLimit", Session("username"), _
                            CDbl(DealAmountSale.Text), Trim(txtCurrency.Text), Trim(cmbproduct.SelectedValue.ToString))

            If TranxLimitVal(0) = "1" Then
                MsgBox("Transaction Limit exceeded" & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       TranxLimitVal(1) & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       "Limit Amount             :   " & Format(CDbl(TranxLimitVal(2)), "###,###.00") & vbCrLf & _
                       "Transaction Amount :   " & DealAmountSale.Text, MsgBoxStyle.Information, "Limits")


                If getSysParam("tranlmt") = "warn" Then
                    MsgBox("Limit violation will be recorded", MsgBoxStyle.Exclamation, "Transaction Limit exceeded")
                ElseIf getSysParam("tranlmt") = "stop" Then
                    MsgBox("Limit exceeded, you cannot proceed with the transaction", MsgBoxStyle.Critical, "Transaction Limit exceeded")
                    Exit Sub
                End If
            End If

            'Counterparty limits validation
            If limitsch.CheckCounterpartyLimit(cmbCustomer.SelectedValue.ToString, Session("DealStructure"), CDec(DealAmountSale.Text), Trim(txtCurrency.Text), CDate(CDate(EndDateSale.Text))) = False Then

                Session("crStatus") = "1"

                If CounterpartyLimitViolation(CDec(DealAmountSale.Text), Session("DealStructure"), txtCurrency.Text, CDate(CDate(EndDateSale.Text))) = "stop" Then
                    Exit Sub
                End If

            Else
                Session("crStatus") = "2"
            End If

            CustName = cmbCustomer.SelectedValue.ToString

            If limitsch.CheckDailyLimit(Session("username"), Trim(cmbproduct.SelectedValue.ToString), CDec(DealAmountSale.Text), Trim(txtdealdescSale.Text), Trim(txtCurrency.Text)) = False Or _
       limitsch.CheckdealSizeLimit(Session("username"), Trim(cmbproduct.SelectedValue.ToString), CDec(DealAmountSale.Text), Trim(txtdealdescSale.Text), Trim(txtCurrency.Text)) = False Or _
       limitsch.CheckProductPos(Trim(cmbproduct.SelectedValue.ToString), CDec(DealAmountSale.Text), "+", Trim(txtCurrency.Text)) = False Or _
       limitsch.CheckPortfolioPos(ID, CDec(DealAmountSale.Text), "+", Trim(txtCurrency.Text)) Then
                'WarningMessage <> "" Then

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
                '        ref.RefMessageDealsize = limitsch.RefMessageDealsize
                '        ref.RefMessageDaily = limitsch.RefMessageDaily
                '        ref.refAmount = limitsch.refAmount
                '        ref.RefDealType = limitsch.RefDealType
                '        ref.Limitauthoriser = limitsch.Limitauthoriser
                '        ref.RefMessagePortfolio = limitsch.RefMessagePortfolio
                '        ref.RefMessageProduct = limitsch.RefMessageProduct
                '        ref.RefMessageCounterparty = limitsch.RefMessageCounterparty
                '        ref.RefMessageCapitalLoss = "Capital loss, Sell rate : " & Trim(txtYieldSale.Text) & "% MTM rate : " & Trim(lblPurchaseRate.Text & "%")

                '        ref.DealCustomer = Trim(cmbCustomer.SelectedValue.ToString)
                '        ref.DealDealAmount = CDec(DealAmountSale.Text)
                '        ref.DealDealInterestRate = Trim(txtDiscountRateSale.Text)
                '        ref.DealTenor = Int(txtTenorSale.Text)
                '        ref.DealMaturityAmount = CDec(txtMaturityAmountSale.Text)
                '        ref.dealGrossInterest = CDec(txtGrossSale.Text)
                '        ref.dealPortfolio = Trim(Trim(cmbportfolio.SelectedValue.ToString))
                '        ref.dealDealType = Trim(cmbproduct.SelectedValue.ToString)
                '        ref.DealDiscount = "N"
                '        ref.DealSecurityID = Trim(lblRef.Text)
                '        ref.DealCurrency = Trim(txtCurrency.Text)
                '        ref.CustName = customerN

                '        'do not check this condition for multi sell of securities
                '        If lstSell.Visible = True Then
                '            txtPL.Text = "0"
                '        End If

                '        ref.DealProfit = CDec(txtPL.Text)
                '        ref.DealTransType = "Security Sell"

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
                ''************************************************************************************
                'End If
                'refIDRecieve = refid
                Call disableControlsSale()
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

            Exit Sub

err1:
            Dim ex, ex3 As String
            ex3 = Err.GetException.Message
            ex = Err.GetException.Message

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex3 & " " & ex, Session("serverName"), Session("client"))
            '************************END****************************************

            MsgBox(ex3 & " " & ex, MsgBoxStyle.Critical, "Error")

            Exit Sub

err2:

        End If
    End Sub
    Private Sub disableControlsSale()
        DealAmountSale.Enabled = False
        txtDiscountRateSale.Enabled = False
        txtYieldSale.Enabled = False
        txtTenorSale.Enabled = False
        txtMaturityAmountSale.Enabled = False
        txtTaxRateSale.Enabled = False
        txtTaxAmountSale.Enabled = False
        cmdInstrucSale.Enabled = False
        txtOtherSale.Enabled = False
        btnSaveSale.Enabled = True
        btnReset.Enabled = True

        StartDateSale.Enabled = False
        EndDateSale.Enabled = False
        chkTaxStatus.Enabled = False
        cmdInstrucSaleM.Enabled = False
        txtOtherSaleM.Enabled = False
        txtDealAccountSale.Enabled = False
        'TabSecuritySell.SelectedIndex = 1
      

        'set control back color
        DealAmountSale.BackColor = Color.LightYellow
        txtYieldSale.BackColor = Color.LightYellow
        txtTenorSale.BackColor = Color.LightYellow
        txtDiscountRateSale.BackColor = Color.LightYellow
        txtMaturityAmountSale.BackColor = Color.LightYellow
        txtTenorSale.BackColor = Color.LightYellow
    End Sub
    Public Function checkDealAccount() As String
        Dim x As String
        Try
            'validate username first
            strSQL = "Select [value] from systemparameters where [parameter]='promptacc'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    x = Trim(drSQL.Item(0).ToString)
                Loop
            End If


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
        Return x
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
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "Function IntBasis", Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
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

    Protected Sub cmbproduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbproduct.SelectedIndexChanged
        Session("DealStructure") = limitsch.DealStructure

    End Sub

    Private Function CounterpartyLimitViolation(dealamt As Double, dealtype As String, curr As String, matDate As String) As String
        Dim x As String = ""
        Dim myLimit As Decimal = CDbl(GetFieldVal(Trim(cmbCustomer.SelectedValue.ToString), "limit", dealtype))
        Dim mycumulativeTotal As Decimal = CDbl(GetFieldVal(Trim(cmbCustomer.SelectedValue.ToString), "cumulativetotal", dealtype))

        x = limitExpires(Trim(cmbCustomer.SelectedValue.ToString), dealtype, curr, CDate(matDate))
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
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " CheckBaseCurrency", Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function
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

    Protected Sub btnSaveSale_Click(sender As Object, e As EventArgs) Handles btnSaveSale.Click
        Dim x As Integer
        Dim ID As String
        Dim entryType As String

        If RdSellOPT.SelectedValue = "discount" Then
            entryType = "D"
        Else
            entryType = "Y"
        End If
        'Get selected portfolio id
        'x = cmbPortfolioDiscSale.SelectedIndex
        'ID = Trim(portIDxx.Text) 'MyPortfolioCollectionID.Item(x + 1).ToString


        '************************************************************************************

        Dim Instruction As String = ""
        If Trim(cmdInstrucSale.Text) = "Other" Then
            Instruction = Replace(Trim(txtOtherSale.Text), "’", "")
            Instruction = Replace(Trim(txtOtherSale.Text), "'", "")
            Instruction = Replace(Trim(txtOtherSale.Text), "&", "and")
        Else
            Instruction = Replace(Trim(cmdInstrucSale.Text) & " -" & Trim(txtOtherSale.Text), "’", "")
            Instruction = Replace(Trim(cmdInstrucSale.Text) & " -" & Trim(txtOtherSale.Text), "'", "")
            Instruction = Replace(Trim(cmdInstrucSale.Text) & " -" & Trim(txtOtherSale.Text), "&", "and")
        End If

        Dim InstructionM As String = ""
        If Trim(cmdInstrucSaleM.Text) = "Other" Then
            InstructionM = Replace(Trim(txtOtherSaleM.Text), "’", "")
            InstructionM = Replace(Trim(txtOtherSaleM.Text), "'", "")
            InstructionM = Replace(Trim(txtOtherSaleM.Text), "&", "and")
        Else
            InstructionM = Replace(Trim(cmdInstrucSaleM.Text) & " -" & Trim(txtOtherSaleM.Text), "’", "")
            InstructionM = Replace(Trim(cmdInstrucSaleM.Text) & " -" & Trim(txtOtherSaleM.Text), "'", "")
            InstructionM = Replace(Trim(cmdInstrucSaleM.Text) & " -" & Trim(txtOtherSaleM.Text), "&", "and")
        End If


        Dim dealNum As String = ""
        'get deal number
        dealNum = objectDealNumbers.GetDealNumber(Trim(cmbproduct.SelectedValue.ToString))



        'Check if the dealreference is ok
        If Trim(dealNum) = "" Then
            MsgBox("Deal reference not generated")
            Exit Sub
        End If



        '******************************************************************
        'Step1
        'Update original deal amount 
        If Session("Single") <> "Single" Then
            Call UpdateFiledsMulti()

        Else
            Call UpdateFileds(Trim(SellingCaption.Text), Trim(lblRef.Text))
        End If

      
        Dim limitsch As New usrlmt.usrlmt
        limitsch.clients = client
        limitsch.ConnectionString = Session("ConnectionString")

       

        'Save counterparty limit and transaction periods limits information detail here
        'limitsch.SaveCounterpartyLimitsDetails(Trim(Session("username")), dealNum, CDbl(DealAmountSale.Text), GetFieldVal(cmbCustomer.SelectedValue.ToString, "limit", "Deposit") _
        '  , CDate(Session("SysDate")), Session("crStatus"), Trim(txtCurrency.Text), "D", cmbCustomer.SelectedValue.ToString, Int(TranxLimitVal(3)), Trim(TranxLimitVal(1)), CDbl(TranxLimitVal(2)), _
        '          Int(TranxLimitVal(0)), "")

        'Step2       
        'save deal
        'SavDls.clients = client
        'SavDls.strCurrentD = strCurrentDirectory
        'SavDls.dataBaseName = dataBaseName

        Call SavDls.saveDiscountedSale(daystomature, Session("SysDate"), RequireFrontAuthoriser, ID, cmbproduct.SelectedValue.ToString, cmbCustomer.SelectedValue.ToString, CDate(StartDateSale.Text), CDate(EndDateSale.Text), CDec(DealAmountSale.Text) _
        , CDec(txtMaturityAmountSale.Text), CDec(txtDiscountRateSale.Text), CDec(txtYieldSale.Text), CDec(netInterestSale.Text) _
        , Int(txtTenorSale.Text), Instruction, dealNum, "Discount Sale", CDec(txtTaxRateSale.Text), CDec(txtTaxAmountSale.Text) _
        , CDec(txtGrossSale.Text), intAccruedToDate, Trim(lblRef.Text), entryType, Trim(txtCurrency.Text) _
        , Session("IsDealer"), Session("username"), InterestBasisValue, Session("serverName"), Session("dataBaseName"), globalvars_mmdeal.PrintPages, InstructionM, "", _
        getDealCodePur(Trim(lblRef.Text)), CDec(txtCV.Text), Trim(cmbTaxSell.Text), Trim(txtDealAccountSale.Text))


        intAccruedToDate = 0         'Reinitialise interest accrued to date

        Call GetIntBasisValue() 'Reinitialise interest days basis

        'Step 3
        'Save details[Present Value, Cost,Profit]
        Call saveProfitDetails(dealNum)


        'save information about broken limits
        If dealsizeBroken = True Or dailyBroken = True Or PortfolioBroken = True Or ProductBroken = True Or CounterPartyBroken = True Then
            'Dim limitsch As New usrlmt.usrlmt
            limitsch.clients = client
            limitsch.ConnectionString = Session("ConnectionString")

            Call limitsch.LimitBroken(dealNum, refIDRecieve)
        End If

        'Apply Default Settlement Details
        'SetDefaultSettlementAccounts(dealNum, Trim(cmbproduct.SelectedValue.ToString.Text))

        tdsdebitAccount = getDealAccountTDS(Trim(cmbproduct.SelectedValue.ToString), "FUN")
        tdsCreditAccount = Trim(cmbproduct.SelectedValue.ToString) & "-" & Trim(cmbCustomer.SelectedValue.ToString) 'getDealAccountTDS(Trim(cmbproduct.SelectedValue.ToString.Text), "DAL")
        tdscommissionAccount = getDealAccountTDS(Trim(cmbproduct.SelectedValue.ToString), "COM")
        tdsinterestaccount = getDealAccountTDS(Trim(cmbproduct.SelectedValue.ToString), "INT")

        ethixdebitAccount = Trim(txtDealAccountSale.Text) 'getDealAccountEthix(tdsdebitAccount)
        ethixCreditAccount = Trim(txtSellRecieve.Text) 'getDealAccountEthix(tdsCreditAccount)

        ethixcommissionAccount = getDealAccountEthix(tdscommissionAccount)
        ethixinterestaccount = getDealAccountEthix(tdsinterestaccount)
        EthixAccrualDebit = getEthixAccrualAccount(Trim(cmbproduct.SelectedValue.ToString), "debitAccount")
        EthixAccrualCredit = getEthixAccrualAccount(Trim(cmbproduct.SelectedValue.ToString), "creditAccount")

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

        Call disableControlsSale() 'enable disabled controls
        Call clearfieldsSale() 'Clear all controls
        'deselect all radio buttons
        'RdSellOPT.SelectedValue = ""


        'Setup deal notifications
        'If SetEmailNotifications() = True Then 'Checks if the parameter is defined for this action
        '    If MessageBox.Show("Do you want to setup notifications for this deal?", "Notifications", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
        '        Dim notify As New Notifications
        '        notify.lblRef.Text = Trim(dealNum)
        '        notify.btnfind.Visible = False
        '        notify.txtSearch.Visible = False
        '        notify.GetEmailAddress(LoggedUser, Trim(cmbCustomer.SelectedValue.ToString))
        '        notify.ShowDialog()
        '    End If
        'End If

        'SecuritySellForm.Dispose()
        'Me.Close()
    End Sub
    'Clear all controls
    Private Sub clearfieldsSale()
        StartDateSale.Text = CDate(Session("SysDate"))
        EndDateSale.Text = CDate(Session("SysDate"))
        DealAmountSale.Text = ""
        txtDiscountRateSale.Text = ""
        txtYieldSale.Text = ""
        txtTenorSale.Text = ""
        txtMaturityAmountSale.Text = ""
        txtTaxRateSale.Text = "0"
        txtTaxAmountSale.Text = ""
        'cmdInstrucSale.Text = ""
        txtOtherSale.Text = ""
        txtOtherSaleM.Text = ""
        txtGrossSale.Text = ""
        netInterestSale.Text = ""
        chkTaxStatus.Checked = False
        'cmbCustomer.Text = ""
        btnSaveSale.Enabled = False
        txtDealAccountSale.Text = ""


        avgRate.Text = "avg rate"
        avgSize.Text = "avg size"
        avgTenor.Text = "avg tenor"

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
    'Update original deal amount and start date and interest accrued
    Private Sub UpdateFileds(ByVal tbid As String, ByVal dealref As String)
        Dim TenorSaleDays As Integer
        Dim DealAmount As Decimal
        Dim intBasis As Integer
        Dim intrate As Decimal
        Dim Accruedtodatex As Decimal
        Dim netIntPurchase As Decimal
        Dim netInt As Decimal
        Dim acceptrate As Decimal
        Dim acceptAmount As Decimal
        Dim tenorPurchase As Integer


        Dim DealPortfolio As String
        'Get details about the TB
        'Determine Deal code Portfolio
        Try
            'validate username first
            strSQL = "select entrytype from deals_live " & _
                     " where dealreference = '" & dealref & "'  "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                DealPortfolio = UCase(Trim(drSQL.Item(0).ToString))
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


        Dim Amount As Decimal
        If RdSellOPT.SelectedValue = "discount" Then
            Amount = CDec(txtMaturityAmountSale.Text)
            Accruedtodatex = CDec(txtPV.Text) - CDec(txtCV.Text)
        Else
            Amount = CDec(txtCV.Text)
            Accruedtodatex = CDec(txtPV.Text) - CDec(txtCV.Text)
        End If


        'Execution for yield
        If DealPortfolio <> "D" Then
            'Revise dealamount/maturity amount
            Dim v As Decimal = (CDec(txtPV.Text) - CDec(txtCV.Text))
            Try
                strSQL = "update deals_live set dealamount = dealamount-'" & Amount & "',intaccruedtodate=intaccruedtodate-'" & v & "' where tb_id = '" & tbid & "' and dealreference='" & dealref & "' "
                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()

                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()


                'Retrive information for revising net interest
                strSQL = "select interestrate,dealamount,intdaysbasis,tenor,intaccruedtodate,netinterest from deals_live where tb_id = '" & tbid & "' and dealreference='" & dealref & "'"
                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()

                Do While drSQL.Read
                    TenorSaleDays = Int(txtTenorSale.Text) '(drSQL.Item("tenor").ToString)
                    DealAmount = CDec(DealAmountSale.Text) 'CDec(drSQL.Item("dealamount").ToString)
                    intBasis = Int(drSQL.Item("intdaysbasis").ToString)
                    intrate = CDec(txtYieldSale.Text) 'CDec(drSQL.Item("interestrate").ToString)
                    netIntPurchase = CDec(drSQL.Item("netinterest").ToString)
                Loop

                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()

                'revise net interest
                netInt = CDec(txtMaturityAmountSale.Text) - CDec(txtCV.Text) 'netIntPurchase - (DealAmount * intrate * TenorSaleDays / (intBasis * 100)) 'Net Interest
                'Update
                strSQL = "update deals_live set grossinterest =grossinterest-" & netInt & ",netinterest =netinterest-" & netInt & ",maturityamount=maturityamount-'" & CDec(txtMaturityAmountSale.Text) & "' where tb_id = '" & tbid & "' and dealreference ='" & dealref & "' "
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
                object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************
            End Try


            'Execution for discounted deals
        Else

            'Revise dealamount/maturity amount
            Try
                strSQL = "update deals_live set maturityamount = maturityamount-'" & Amount & "' where tb_id = '" & tbid & "' and dealreference='" & dealref & "'"
                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()

                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()


                'Retrive information for revising net interest
                strSQL = "select acceptancerate,discountrate,maturityamount,intdaysbasis,tenor,intaccruedtodate,netinterest from deals_live where tb_id = '" & tbid & "' and dealreference='" & dealref & "'"
                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()

                Do While drSQL.Read
                    TenorSaleDays = Int(txtTenorSale.Text) '(drSQL.Item("tenor").ToString)
                    DealAmount = CDec(DealAmountSale.Text) 'CDec(drSQL.Item("dealamount").ToString)
                    intBasis = Int(drSQL.Item("intdaysbasis").ToString)
                    intrate = CDec(txtYieldSale.Text) 'CDec(txtDiscountRateSale.Text) 'CDec(drSQL.Item("interestrate").ToString)
                    netIntPurchase = CDec(drSQL.Item("netinterest").ToString)
                    acceptrate = CDec(drSQL.Item("acceptancerate").ToString)
                    tenorPurchase = Int(drSQL.Item("tenor").ToString)
                Loop
                acceptAmount = Math.Round(CDec(txtMaturityAmountSale.Text) * acceptrate * tenorPurchase / (intBasis * 100), 2)

                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()

                'revise net interest and deal amount of purchase
                'netInt = netIntPurchase - (DealAmount * intrate * TenorSaleDays / (intBasis * 100)) 'Net Interest
                netInt = CDec(txtMaturityAmountSale.Text) - CDec(txtCV.Text)
                'Update
                strSQL = "update deals_live set grossinterest =grossinterest-" & netInt & ",netinterest =netinterest-" & netInt & " ,dealamount=dealamount-'" & CDec(txtCV.Text) & "'+" & acceptAmount & ",intaccruedtodate=intaccruedtodate-" & Accruedtodatex & " where tb_id = '" & tbid & "' and dealreference ='" & dealref & "'"
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
                object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************

            End Try

        End If

    End Sub
    'Update original deal amount and start date and interest accrued
    Private Sub UpdateFiledsMulti()
        Dim TenorSaleDays As Integer
        Dim DealAmount As Decimal
        Dim intBasis As Integer
        Dim intrate As Decimal
        Dim Accruedtodatex As Decimal
        Dim netIntPurchase As Decimal
        Dim netInt As Decimal
        Dim acceptrate As Decimal
        Dim acceptAmount As Decimal
        Dim tenorPurchase As Integer
        Dim DealPortfolio As String
        Dim Amount As Decimal
        Dim x As Integer

        Dim dt As New DataTable()
        dt.Columns.Add(New DataColumn("Item #", GetType(Integer)))
        dt.Columns.Add(New DataColumn("SecRef", GetType(String)))
        dt.Columns.Add(New DataColumn("dealref", GetType(String)))
        dt.Columns.Add(New DataColumn("SelAmt", GetType(String)))
        dt.Columns.Add(New DataColumn("cost", GetType(String)))
        dt.Columns.Add(New DataColumn("profit", GetType(String)))
        dt.Columns.Add(New DataColumn("presentV", GetType(String)))
        dt.Columns.Add(New DataColumn("matamt", GetType(String)))
        Dim arr() As String = Split(Session("loadGrid"), "|")
        'arr.ElementAt(1).Remove(1)

        Dim arrlen As Integer = arr.Length
        Dim xx As Integer

        For xx = 1 To arr.Length - 1
            Dim arr2() As String = Split(arr(xx), ",")
            'Dim dr As DataRow = dt.NewRow()
            'dr("SecRef") = arr2(0)
            'dr("dealref") = arr2(1)
            'dr("SelAmt") = arr2(2)
            'dr("cost") = arr2(3)
            'dr("profit") = arr2(4)
            'dr("presentV") = arr2(5)
            'dr("matamt") = arr2(6)
            'dt.Rows.Add(dr)
            If RdSellOPT.SelectedValue = "discount" Then
                Amount = CDec(arr2(2))
                Accruedtodatex = CDec(arr2(5)) - CDec(arr2(3)) ' Present Value less Cost
                DealPortfolio = "D"
            Else
                Amount = CDec(arr2(3))
                Accruedtodatex = CDec(arr2(5)) - CDec(arr2(3)) ' Present Value less Cost
                DealPortfolio = "Y"
            End If


            'Execution for yield
            If DealPortfolio <> "D" Then
                'Revise dealamount/maturity amount
                Dim v As Decimal = Accruedtodatex
                Try
                    strSQL = "update deals_live set dealamount = dealamount-'" & Amount & "',intaccruedtodate=intaccruedtodate-'" & v & "' where tb_id = '" & Trim(arr2(0)) & "' and dealreference='" & arr2(1) & "' "
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader()

                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()


                    'Retrive information for revising net interest
                    strSQL = "select interestrate,dealamount,intdaysbasis,tenor,intaccruedtodate,netinterest from deals_live where tb_id = '" & Trim(arr2(0)) & "' and dealreference='" & arr2(1) & "'"
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader()

                    Do While drSQL.Read
                        TenorSaleDays = Int(txtTenorSale.Text) '(drSQL.Item("tenor").ToString)
                        DealAmount = CDec(DealAmountSale.Text) 'CDec(drSQL.Item("dealamount").ToString)
                        intBasis = Int(drSQL.Item("intdaysbasis").ToString)
                        intrate = CDec(txtYieldSale.Text) 'CDec(drSQL.Item("interestrate").ToString)
                        netIntPurchase = CDec(drSQL.Item("netinterest").ToString)
                    Loop

                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()

                    'revise net interest
                    netInt = CDec(arr2(6)) - CDec(arr2(3))  'Net Interest
                    'Update
                    strSQL = "update deals_live set grossinterest =grossinterest-" & netInt & ",netinterest =netinterest-" & netInt & ",maturityamount=" & _
                             "maturityamount-'" & CDec(arr2(6)) & "' where tb_id = '" & Trim(arr2(0)) & "' and " & _
                             " dealreference='" & arr2(1) & "'"

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
                    object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                    '************************END****************************************
                End Try


                'Execution for discounted deals
            Else

                'Revise dealamount/maturity amount
                Try
                    strSQL = "update deals_live set maturityamount = maturityamount-'" & Amount & "' where tb_id = '" & Trim(arr2(0)) & "' and dealreference='" & arr2(1) & "'"
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader()

                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()


                    'Retrive information for revising net interest
                    strSQL = "select acceptancerate,discountrate,maturityamount,intdaysbasis,tenor,intaccruedtodate,netinterest from deals_live " & _
                             " where tb_id = '" & Trim(arr2(0)) & "' and dealreference='" & arr2(1) & "'"
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader()

                    Do While drSQL.Read
                        TenorSaleDays = Int(txtTenorSale.Text) '(drSQL.Item("tenor").ToString)
                        'DealAmount = CDec(DealAmountSale.Text) 'CDec(drSQL.Item("dealamount").ToString)
                        intBasis = Int(drSQL.Item("intdaysbasis").ToString)
                        intrate = CDec(txtYieldSale.Text) 'CDec(txtDiscountRateSale.Text) 'CDec(drSQL.Item("interestrate").ToString)
                        netIntPurchase = CDec(drSQL.Item("netinterest").ToString)
                        acceptrate = CDec(drSQL.Item("acceptancerate").ToString)
                        tenorPurchase = Int(drSQL.Item("tenor").ToString)
                    Loop
                    acceptAmount = Math.Round(CDec(arr2(6)) * acceptrate * tenorPurchase / (intBasis * 100), 2)

                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()

                    'revise net interest and deal amount of purchase
                    'netInt = netIntPurchase - (DealAmount * intrate * TenorSaleDays / (intBasis * 100)) 'Net Interest
                    netInt = CDec(arr2(6)) - CDec(arr2(3))
                    'Update
                    strSQL = "update deals_live set grossinterest =grossinterest-" & netInt & ",netinterest =netinterest-" & netInt & " ,dealamount=dealamount-'" & CDec(arr2(3)) & "'+" & acceptAmount & ",intaccruedtodate=" & _
                             "intaccruedtodate-" & Accruedtodatex & " where tb_id = '" & Trim(arr2(0)) & "' and dealreference='" & arr2(1) & "'"
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
                    object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                    '************************END****************************************

                End Try

            End If


        Next
        'For x = 0 To lstSell.Items.Count - 1

        'Next

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
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

        Return x
    End Function
    Private Function getDealCodePur(ByVal ref As String) As String

        Dim x As String

        Try

            strSQL = "select dealtype from deals_live where dealreference='" & Trim(ref) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = drSQL.Item(0).ToString
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            Return x

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "getDealCodePur")

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Function

    Private Sub saveProfitDetails(ByVal refSell As String)

        'Check if its a multi sell
        If Session("Single") = "Single" Then
            Try

                strSQL = "insert into selldetail values('" & Trim(refSell) & "','" & Trim(lblRef.Text) & "','" & Trim(SellingCaption.Text) & "','" & CDec(txtPL.Text) & "','" & CDec(txtCV.Text) & "','" & CDec(txtPV.Text) & "','" & CDec(txtMaturityAmountSale.Text) & "')"
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
                MsgBox(ex.Message, MsgBoxStyle.Critical, "saveProfitDetails")

                'Log the event *****************************************************
                object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************

            End Try

        Else

            'Dim x As Integer
            Dim dt As New DataTable()
            dt.Columns.Add(New DataColumn("Item #", GetType(Integer)))
            dt.Columns.Add(New DataColumn("SecRef", GetType(String)))
            dt.Columns.Add(New DataColumn("dealref", GetType(String)))
            dt.Columns.Add(New DataColumn("SelAmt", GetType(String)))
            dt.Columns.Add(New DataColumn("cost", GetType(String)))
            dt.Columns.Add(New DataColumn("profit", GetType(String)))
            dt.Columns.Add(New DataColumn("presentV", GetType(String)))
            dt.Columns.Add(New DataColumn("matamt", GetType(String)))
            Dim arr() As String = Split(Session("loadGrid"), "|")
            'arr.ElementAt(1).Remove(1)
            'Dim dr As DataRow = dt.NewRow()
            'dr("SecRef") = arr2(0)
            'dr("dealref") = arr2(1)
            'dr("SelAmt") = arr2(2)
            'dr("cost") = arr2(3)
            'dr("profit") = arr2(4)
            'dr("presentV") = arr2(5)
            'dr("matamt") = arr2(6)
            'dt.Rows.Add(dr)
            Dim arrlen As Integer = arr.Length
            Dim xxx As Integer

            For xxx = 1 To arr.Length - 1
                Dim arr2() As String = Split(arr(xxx), ",")


                Try

                    strSQL = "insert into selldetail values('" & Trim(refSell) & "','" & arr2(1) & "','" & Trim(arr2(0)) & "','" & CDec(arr2(4)) & "','" & CDec(arr2(3)) & "','" & CDec(arr2(5)) & "','" & CDec(arr2(6)) & "')"

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
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "saveProfitDetails")

                    'Log the event *****************************************************
                    object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                    '************************END****************************************

                End Try

            Next xxx

        End If
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
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "SetDefaultSettlementAccounts -- Update / Insert", Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

    End Sub

End Class
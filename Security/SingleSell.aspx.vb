Imports System.Data.SqlClient
Imports sys_ui
Imports System.Drawing

Public Class SingleSell
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private object_userlog As New usrlog.usrlog
    Private currency As String
    Private Days As String
    Private MatAmount As String
    Private selloption As String
    Private maturity As String
    Private selectedItem As Integer
    Private cost As Decimal = 0
    'Private SellPV As Decimal = 0
    Private TotalProfit As Decimal = 0
    Private maturityAmt As Decimal = 0
    Private PortID As Integer
    Dim Buyback As String = ""
    Private SellingOption As String
    Private securityRef As String
    Private dealRef As String
    Private backvalue As Integer
    Dim comm As Decimal
    Dim interest As Decimal
    'Private lblFV As Decimal
    'Private PV As Decimal
    Private commrate As Decimal
    Private PresentV, MaturityV, intToDate ', cost As Decimal
    Private DealPortfolio As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then

                currency = Request.QueryString("currency")
                Days = Request.QueryString("Days")
                MatAmount = Request.QueryString("MatAmount")

                selloptionValue.Text = Request.QueryString("selloption")

            Else
                lblError.Text = alert("Please make sure all fields are selected On the newDeal Page", "Incomplete informaton")

            End If


            Call load_TBs(currency, MatAmount, Days, selloptionValue.Text)
        End If


    End Sub
    Public Sub load_TBs(ByVal ccy As String, ByVal amt As Double, ByVal daysR As Integer, ByVal sellOpt As String)
        Try

            If sellOpt = "Discount" Then
                strSQL = "select securitypurchase.tb_id,securitypurchase.dealreference from securitypurchase join" & _
                         " deals_live on securitypurchase.tb_id=deals_live.TB_ID and deals_live.dealreference=" & _
                         " securitypurchase.dealreference join dealtypes on dealtypes.deal_code=deals_live.dealtype " & _
                         " where dealtypes.othercharacteristics='Trading' and matured = 'N' and daystomaturity >2 and" & _
                         " authorisationstatus='A' and dealtypes.currency='" & Trim(ccy) & "' and daystomaturity >= " & daysR & "" & _
                         " and deals_live.maturityamount>=" & amt & " and entrytype='D'"

            ElseIf sellOpt = "Yield" Then

                strSQL = "select securitypurchase.tb_id,securitypurchase.dealreference from securitypurchase join" & _
                        " deals_live on securitypurchase.tb_id=deals_live.TB_ID and deals_live.dealreference=" & _
                        " securitypurchase.dealreference join dealtypes on dealtypes.deal_code=deals_live.dealtype " & _
                        " where dealtypes.othercharacteristics='Trading' and matured = 'N' and daystomaturity >2 and" & _
                        " authorisationstatus='A' and dealtypes.currency='" & Trim(ccy) & "' and daystomaturity >= " & daysR & "" & _
                        " and deals_live.dealamount>=" & amt & " and entrytype='Y'"

            Else

                strSQL = "select securitypurchase.tb_id,securitypurchase.dealreference from securitypurchase join" & _
                       " deals_live on securitypurchase.tb_id=deals_live.TB_ID and deals_live.dealreference=" & _
                       " securitypurchase.dealreference join dealtypes on dealtypes.deal_code=deals_live.dealtype " & _
                       " where dealtypes.othercharacteristics='Trading' and matured = 'N' and daystomaturity >2 and" & _
                       " authorisationstatus='A' and dealtypes.currency='" & Trim(ccy) & "' and daystomaturity >= " & daysR & ""
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            'ListTbs.Items.Clear()

            Do While drSQL.Read

                If Checkamount(Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(1).ToString)) = True Then

                    'Dim itms As New ListViewItem(Trim(drSQL.Item(0).ToString))
                    'itms.SubItems.Add(Trim(drSQL.Item(1).ToString))
                    'ListTbs.Items.Add(itms)
                    cmbTB.Items.Add(New ListItem(Trim(drSQL.Item(0).ToString) + "  " + Trim(drSQL.Item(1).ToString), Trim(drSQL.Item(0).ToString)))
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
    Private Function Checkamount(ByVal TBid As String, ByVal DealRef As String) As Boolean

        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Try
            'validate username first
            strSQL1 = "select Dealamount,maturityamount,entrytype from deals_live where tb_id='" & TBid & "' and dealreference='" & DealRef & "'"

            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()

            Do While drSQL1.Read
                If Trim(drSQL1.Item(2)).ToString = "D" Then
                    If CDec(drSQL1.Item(1)) <= 0 Then
                        Return False
                    End If
                Else
                    If CDec(drSQL1.Item(0)) <= 0 Then
                        Return False
                    End If
                End If

            Loop

            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()

            Return True
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try

    End Function

    Protected Sub cmbTB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTB.SelectedIndexChanged
        'Get details about the security
        Dim cnSQL1 As SqlConnection
        Dim strSQL1 As String
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim accruedInt As Decimal

        'Get details about the security
        If cmbTB.SelectedValue.ToString <> "" Then

            Try


                strSQL1 = "select deals_live.dealamount as amt,securitypurchase.amtavailable,securitypurchase.dealamount as secamt,securitypurchase.interestRate as Rate,deals_live.entrytype,deals_live.Currency as curr,securitypurchase.maturitydate as matdate,deals_live.MaturityAmount as matAmt,deals_live.tenor,deals_live.daystomaturity,deals_live.Discountrate as disc,deals_live.intdaysbasis,deals_live.StartDate as start ,deals_live.intaccruedtodate,deals_live.Acceptancerate  from securitypurchase join deals_live on securitypurchase.tb_id=deals_live.TB_ID  " & _
                          " where securitypurchase.tb_id ='" & cmbTB.SelectedValue.ToString & "' and deals_live.TB_ID ='" & cmbTB.SelectedValue.ToString & "'" & _
                          " and securitypurchase.dealreference='" & getRef(Trim(cmbTB.SelectedValue.ToString)) & "' and" & _
                          " deals_live.dealreference='" & getRef(Trim(cmbTB.SelectedValue.ToString)) & "' and deals_live.othercharacteristics='Discount Purchase' "


                cnSQL1 = New SqlConnection(Session("ConnectionString"))
                cnSQL1.Open()
                cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
                drSQL1 = cmSQL1.ExecuteReader()
                Do While drSQL1.Read
                    If drSQL1.Item("entrytype").ToString = "D" Then
                        avalAtPV.Text = "Amount Available"
                        lblCostDesc2.Visible = False
                        txtAvalableForSale.Visible = False
                        accruedInt = CDec(drSQL1.Item("matAmt").ToString) * (CInt(drSQL1.Item("tenor")) - CInt(drSQL1.Item("daystomaturity"))) * CDec(drSQL1.Item("disc")) / (CDec(drSQL1.Item("intdaysbasis")) * 100)
                        txtAvalableForSale.Text = CDec(drSQL1.Item("matAmt").ToString) - SecurityAttached()
                        lblAvalableForSalePV.Text = CDec(drSQL1.Item("matAmt").ToString) - SecurityAttached()
                        txtPurValue.Text = drSQL1.Item("secamt").ToString
                        txtIntRate.Text = drSQL1.Item("Rate").ToString
                        lblDesc.Text = "Maturity Value"
                        rate.Text = "Discount Rate"
                        DealPortfolio = "D"
                    Else
                        txtAvalableForSale.Text = CDec(drSQL1.Item("amt").ToString) - SecurityAttached()
                        accruedInt = CDec(drSQL1.Item("amt").ToString) * (CInt(drSQL1.Item("tenor")) - CInt(drSQL1.Item("daystomaturity"))) * CDec(drSQL1.Item("interestrate")) / (CDec(drSQL1.Item("intdaysbasis")) * 100)
                        lblAvalableForSalePV.Text = CDec(drSQL1.Item("amt").ToString) + accruedInt - SecurityAttached()
                        txtPurValue.Text = drSQL1.Item("secamt").ToString
                        txtIntRate.Text = drSQL1.Item("Rate").ToString
                        lblDesc.Text = "Purchase Value"
                        rate.Text = "Yield Rate"
                        DealPortfolio = "Y"
                    End If
                    'MsgBox(CDec(drSQL1.Item("matAmt").ToString))

                    lbltb.Text = Trim(cmbTB.SelectedValue.ToString)
                    lblAvalableForSalePV.Text = Format(CDec(lblAvalableForSalePV.Text), "###,###,###.00")
                    txtAvalableForSale.Text = Format(CDec(txtAvalableForSale.Text), "###,###,###.00")
                    lblBasis.Text = drSQL1.Item("intdaysbasis").ToString
                    lblPurchaseStart.Text = drSQL1.Item("start").ToString
                    lblCurrency.Text = drSQL1.Item("curr").ToString
                    txtDaysMaturity.Text = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(drSQL1.Item("matdate")))
                    txtmaturity.Text = drSQL1.Item("matdate").ToString
                    lblTenor.Text = drSQL1.Item("tenor").ToString
                    txtSellTenor.Text = txtDaysMaturity.Text
                    commrate = CDec(drSQL1.Item("Acceptancerate"))
                    comm = CDec(drSQL1.Item("amt")) * CDec(drSQL1.Item("Acceptancerate")) * Int(drSQL1.Item("tenor")) / (Int(lblBasis.Text) * 100)
                    PresentV = CDec(drSQL1.Item("amt").ToString)
                    interest = PresentV * CDec(lblTenor.Text) * CDec(txtIntRate.Text) / (Int(lblBasis.Text) * 100)
                    MaturityV = PresentV + interest
                    'lblSellingOPT.Text = selloption
                    'MaturityV = CDec(drSQL.Item(12).ToString)
                    intToDate = CDec(drSQL1.Item("intaccruedtodate").ToString)
                    lblDealAmount.Text = CDec(drSQL1.Item("amt").ToString)
                    lblfv.Text = CDec(drSQL1.Item("matAmt").ToString)
                    lblpv.Text = CDec(drSQL1.Item("amt").ToString) + comm + accruedInt
                    If Trim(rate.Text) = "Discount Rate" Then
                        lblbreakeven.Text = BreakEvenRate(CDec(drSQL1.Item("matAmt").ToString), CDec(drSQL1.Item("amt").ToString) + comm + accruedInt, Int(drSQL1.Item("intdaysbasis").ToString), Int(drSQL1.Item("daystomaturity").ToString), DealPortfolio)
                        'MaturityV = CDec(drSQL.Item(12).ToString)
                    Else
                        lblbreakeven.Text = BreakEvenRate(MaturityV, CDec(drSQL1.Item("amt").ToString) + comm + accruedInt, Int(drSQL1.Item("intdaysbasis").ToString), Int(drSQL1.Item("daystomaturity").ToString), DealPortfolio)
                    End If
                Loop
                ' Close and Clean up objects
                drSQL1.Close()
                cnSQL1.Close()
                cmSQL1.Dispose()
                cnSQL1.Dispose()


            Catch xx As NullReferenceException


            Catch ex As SqlException
                MsgBox(ex.Message, MsgBoxStyle.Critical)
                'Log the event *****************************************************
                object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************

            End Try
        End If

    End Sub
    Private Sub checkAmtAvailable()
        Dim accruedInt As Decimal

        'Get details about the security
        If Trim(cmbTB.SelectedValue.ToString) <> "" Then

            Try


                strSQL = "select * from securitypurchase join deals_live on securitypurchase.tb_id=deals_live.TB_ID  " & _
                         " where securitypurchase.tb_id ='" & Trim(cmbTB.SelectedValue.ToString) & "' and deals_live.TB_ID ='" & Trim(cmbTB.SelectedValue.ToString) & "'" & _
                         " and securitypurchase.dealreference='" & getRef(Trim(cmbTB.SelectedValue.ToString)) & "' and" & _
                         " deals_live.dealreference='" & getRef(Trim(cmbTB.SelectedValue.ToString)) & "' and deals_live.othercharacteristics='Discount Purchase' "


                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()
                Do While drSQL.Read
                    If drSQL.Item("entrytype").ToString = "D" Then
                        avalAtPV.Text = "Amount Available"
                        lblCostDesc2.Visible = False
                        txtAvalableForSale.Visible = False
                        accruedInt = CDec(drSQL.Item(13).ToString) * (CInt(drSQL.Item("tenor")) - CInt(drSQL.Item("daystomaturity"))) * CDec(drSQL.Item("Discountrate")) / (CDec(drSQL.Item("intdaysbasis")) * 100)
                        txtAvalableForSale.Text = CDec(drSQL.Item(13).ToString) - SecurityAttached()
                        lblAvalableForSalePV.Text = CDec(drSQL.Item(13).ToString) - SecurityAttached()
                        txtPurValue.Text = drSQL.Item(2).ToString
                        txtIntRate.Text = drSQL.Item(3).ToString
                        lblDesc.Text = "Maturity Value"
                        rate.Text = "Discount Rate"
                        DealPortfolio = "D"
                    Else
                        txtAvalableForSale.Text = CDec(drSQL.Item(12).ToString) - SecurityAttached()
                        accruedInt = CDec(drSQL.Item(12).ToString) * (CInt(drSQL.Item("tenor")) - CInt(drSQL.Item("daystomaturity"))) * CDec(drSQL.Item("interestrate")) / (CDec(drSQL.Item("intdaysbasis")) * 100)
                        lblAvalableForSalePV.Text = CDec(drSQL.Item(12).ToString) + accruedInt - SecurityAttached()
                        txtPurValue.Text = drSQL.Item(2).ToString
                        txtIntRate.Text = drSQL.Item(3).ToString
                        lblDesc.Text = "Purchase Value"
                        rate.Text = "Yield Rate"
                        DealPortfolio = "Y"
                    End If


                    lbltb.Text = Trim(cmbTB.SelectedValue.ToString)
                    lblAvalableForSalePV.Text = Format(CDec(lblAvalableForSalePV.Text), "###,###,###.00")
                    txtAvalableForSale.Text = Format(CDec(txtAvalableForSale.Text), "###,###,###.00")
                    txtAvalableForSale.ForeColor = Color.Blue
                    lblBasis.Text = drSQL.Item("intdaysbasis").ToString
                    lblPurchaseStart.Text = drSQL.Item("startdate").ToString
                    lblCurrency.Text = drSQL.Item("currency").ToString
                    txtDaysMaturity.Text = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(drSQL.Item(4)))
                    maturity = drSQL.Item(4).ToString
                    lblTenor.Text = drSQL.Item("tenor").ToString
                    txtSellTenor.Text = txtDaysMaturity.Text
                    commrate = CDec(drSQL.Item("Acceptancerate"))
                    comm = CDec(drSQL.Item(12)) * CDec(drSQL.Item("Acceptancerate")) * Int(drSQL.Item("tenor")) / (Int(lblBasis.Text) * 100)
                    PresentV = CDec(drSQL.Item(12).ToString)
                    interest = PresentV * CDec(lblTenor.Text) * CDec(txtIntRate.Text) / (Int(lblBasis.Text) * 100)
                    MaturityV = PresentV + interest

                    'MaturityV = CDec(drSQL.Item(12).ToString)
                    intToDate = CDec(drSQL.Item("intaccruedtodate").ToString)
                    lblDealAmount.Text = CDec(drSQL.Item(12).ToString)
                    lblfv.Text = CDec(drSQL.Item(13).ToString)
                    lblpv.Text = CDec(drSQL.Item(12).ToString) + comm + accruedInt
                    If Trim(rate.Text) = "Discount Rate" Then
                        lblbreakeven.Text = BreakEvenRate(CDec(drSQL.Item(13).ToString), CDec(drSQL.Item(12).ToString) + comm + accruedInt, Int(drSQL.Item("intdaysbasis").ToString), Int(drSQL.Item("daystomaturity").ToString), DealPortfolio)
                        lblbreakeven.ForeColor = Color.Green
                        'MaturityV = CDec(drSQL.Item(12).ToString)
                    Else
                        lblbreakeven.Text = BreakEvenRate(MaturityV, CDec(drSQL.Item(12).ToString) + comm + accruedInt, Int(drSQL.Item("intdaysbasis").ToString), Int(drSQL.Item("daystomaturity").ToString), DealPortfolio)
                        lblbreakeven.ForeColor = Color.Green
                    End If
                Loop
                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()


            Catch xx As NullReferenceException


            Catch ex As SqlException
                MsgBox(ex.Message, MsgBoxStyle.Critical)
                'Log the event *****************************************************
                object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************

            End Try
        End If
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
    Private Function SecurityAttached() As Decimal
        Dim x As String
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Try
            'validate username first
            strSQL1 = "select sum(securityamount)[hhh] from attachedsecurities where tb_id = '" & Trim(cmbTB.SelectedValue.ToString) & "' and dealreferencesecurity='" & getRef(Trim(cmbTB.SelectedValue.ToString)) & "' "

            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()
            If drSQL.HasRows = True Then
                Do While drSQL1.Read
                    x = drSQL1.Item(0).ToString
                Loop
            End If
            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()
            If x = "" Then x = "0"
            Return CDec(x)
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtSellTenor.Enabled = True
        txtSaleRate.Enabled = True
        txtSale.Enabled = True
        btnSale.Enabled = False
        cmbTB.Enabled = True
    End Sub

    Protected Sub cmdExit_Click(sender As Object, e As EventArgs) Handles cmdExit.Click
        Response.Redirect("newsell.aspx")
    End Sub

    Protected Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click

        Dim SellingAmt As Decimal
        Dim amount As Decimal
        If Trim(lblSellingOPT.Text) = "" Then
            MsgBox("Select Selling option.", MsgBoxStyle.Information, "Sell Option")
            lblSellingOPT.Focus()
            Exit Sub
        End If

        If Trim(lblAvalableForSalePV.Text) = "" Then
            Exit Sub
        End If

        If Trim(lblSellingOPT.Text).Equals("Cost") Then
            SellingAmt = CDec(txtAvalableForSale.Text)
        Else
            SellingAmt = CDec(lblAvalableForSalePV.Text)
        End If
        lblSellingOPT.ForeColor = Color.Green

        'If Trim(Label4.Text) = "Interest Rate" Then
        'amount = CDec(Label11.Text)
        'Else
        If txtSale.Text = "" Then
            MsgBox("Enter Sale amount.", MsgBoxStyle.Critical, "Sale")
            Exit Sub
        End If

        amount = CDec(txtSale.Text)

        'End If


        If CDec(txtSale.Text) = 0 Then
            MsgBox("Sale amount cannot be zero.", MsgBoxStyle.Critical, "Inconsistent operation")
            Exit Sub
        End If


        Try
            If Int(txtSellTenor.Text) > Int(lblTenor.Text) Then
                MsgBox("Tenor for sale cannot be greater than tenor purchase", MsgBoxStyle.Critical, "Tenor")
                Exit Sub
            End If
            'check if amount is not greater than what is available to sale
            If SellingAmt - amount < 0 Then
                MsgBox("Sale amount is greater than what is available.", MsgBoxStyle.Critical, "Sale")
                Exit Sub
            End If

            If rate.Text = "Discount Rate" Then
                DiscountMethod()
            Else
                YieldMethod()
            End If

            cmbTB.Enabled = False
            btnSale.Enabled = True

        Catch xs As Exception
            MsgBox(xs.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub DiscountMethod()
        Dim TotalProfit As Decimal
        Dim IntClient As Decimal
        Dim TotalInterest As Decimal
        Dim days As Integer
        Dim AccruedOnSaleAmt As Decimal
        Dim EffectiveRate As Decimal
        Dim intToDate1 As Decimal
        Dim comm As Decimal

        btnSale.Enabled = False

        'Holding period Yield Variables
        Dim HoldingReturn As Decimal ' Holding return - Accrued interest Plus the Profit/Loss on the sale
        Dim AHPY As Decimal ' Annual Holding Preiod Yield


        If txtSellTenor.Text = "" Then
            Exit Sub
        End If

        If txtSaleRate.Text = "" Then
            MsgBox("Enter the interest rate", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If txtSale.Text = "" Then
            Exit Sub
        End If

        comm = CDec(lblfv.Text) * commrate * Int(lblTenor.Text) / (Int(lblBasis.Text) * 100)
        If Int(txtSellTenor.Text) > Int(txtDaysMaturity.Text) Then
            MsgBox("Transaction will be backvalued and Breakeven rate will change.", MsgBoxStyle.Information, "Sell")
            intToDate1 = (CDec(lblfv.Text) * (CDec(txtIntRate.Text)) * (CDec(lblTenor.Text) - Int(txtSellTenor.Text))) / (Int(lblBasis.Text) * 100)
            lblbreakeven.Text = BreakEvenRate(CDec(lblfv.Text), CDec(lblpv.Text) + comm + intToDate1, Int(lblBasis.Text), Int(txtSellTenor.Text), "D")
        ElseIf Int(txtSellTenor.Text) < Int(txtDaysMaturity.Text) Then
            intToDate1 = (CDec(lblfv.Text) * (CDec(txtIntRate.Text)) * (CDec(lblTenor.Text) - Int(txtDaysMaturity.Text))) / (Int(lblBasis.Text) * 100)
            lblbreakeven.Text = BreakEvenRate(CDec(lblfv.Text), CDec(lblpv.Text), Int(lblBasis.Text), Int(txtSellTenor.Text), "D")
        Else
            lblbreakeven.Text = BreakEvenRate(CDec(lblfv.Text), CDec(lblpv.Text), Int(lblBasis.Text), Int(txtDaysMaturity.Text), "D")
        End If


        'Get accrual on sell amount 
        If Int(txtDaysMaturity.Text) < Int(txtSellTenor.Text) Then
            backvalue = Int(txtSellTenor.Text) - Int(txtDaysMaturity.Text)
            days = (Int(lblTenor.Text) - Int(txtSellTenor.Text))
            'AccruedOnSaleAmt = (CDec(txtSale.Text) * (CDec(txtIntRate.Text)) * (Int(lblTenor.Text) - Int(txtSellTenor.Text))) / (Int(lblBasis.Text) * 100)
        Else
            'AccruedOnSaleAmt = (CDec(txtSale.Text) * (CDec(txtIntRate.Text)) * (CDec(lblTenor.Text) - Int(DaysMaturity.Text))) / (Int(lblBasis.Text) * 100)
            days = (CDec(lblTenor.Text) - Int(txtDaysMaturity.Text))
            backvalue = 0
        End If


        'get present value of sell amount 
        txtSellPV.text = CDec(txtSale.Text) * (1 - (CDec(txtSaleRate.Text) * days / (Int(lblBasis.Text) * 100)))

        'SellPV = CDec(txtSale.Text) - AccruedOnSaleAmt

        'Calculate total interest to be accrued on sell amount for days to maturity
        'TotalInterest = CDec(txtSale.Text) * CDec(mmt.Text) * Int(txtSellTenor.Text) / (Int(lblBasis.Text) * 100)
        TotalInterest = (CDec(txtSale.Text) * (CDec(txtIntRate.Text)) * Int(txtSellTenor.Text)) / (Int(lblBasis.Text) * 100)

        'Calculate interest due to client
        IntClient = CDec(txtSale.Text) * CDec(txtSaleRate.Text * Int(txtSellTenor.Text)) / (Int(lblBasis.Text) * 100)

        'Calculate own profit
        TotalProfit = TotalInterest - IntClient
        If TotalProfit < 0 Then
            lblprofit.Text = "Capital Loss"
        Else
            lblprofit.Text = "Capital gain"
        End If

        'Calculate the effective rate for the client
        'lblEffectiveRate.Text = Math.Round((IntClient * Int(lblBasis.Text) * 100) / (CDec(txtSale.Text) * Int(txtSellTenor.Text)), 9)

        txtProfit.Text = Format(TotalProfit, "###,###,###.00")
        'Label11.Text = Format(SellPV, "###,###,###.00")
        TotalInterest = (CDec(txtSale.Text) * (CDec(txtIntRate.Text)) * Int(lblTenor.Text)) / (Int(lblBasis.Text) * 100)
        cost = CDec(txtSale.Text) - TotalInterest
        lblCost.Text = Format(cost, "###,###,###.00")
        lblCost.ForeColor = Color.Red


        'Compute the holding period Yieldtxt
        HoldingReturn = ((Int(lblTenor.Text) - Int(txtDaysMaturity.Text)) * (CDec(txtIntRate.Text) * CDec(txtSale.Text) / 100) / CDec(lblBasis.Text)) + TotalProfit

        If (Int(lblTenor.Text) - Int(txtDaysMaturity.Text)) = 0 Then
            AHPY = 0
        Else

            AHPY = (HoldingReturn / cost) * (CDec(lblBasis.Text) * 100 / (Int(lblTenor.Text) - Int(txtDaysMaturity.Text))) ' (return/Amount Invested)* (DaysBasis/Number of days invested)
        End If

        AnnualisedHPY.Text = Format(AHPY, "###,###.00")
        'End of AHPY Calculations


        txtSellTenor.Enabled = False
        txtSaleRate.Enabled = False
        txtSale.Enabled = False
        btnSale.Enabled = True
    End Sub
    'calculate break even rate for purchase on sell
    Private Function BreakEvenRate(ByVal maturityvalue As Decimal, ByVal presentvalue As Decimal, ByVal intdaysB As Integer, ByVal daysTomaturity As Integer, ByVal RateType As String) As Decimal
        Dim remainAccrual As Decimal = 0
        Dim brkRate As Decimal
        Dim x As Decimal

        If RateType = "Y" Then
            'Formula: FV=PV(1+(Rate*Ttime/DaysBasis*100))
            brkRate = ((maturityvalue / presentvalue) - 1) * (intdaysB * 100 / daysTomaturity)
        Else
            'Formula: PV = FV(1-(Rate*Ttime/DaysBasis*100))
            brkRate = (1 - (presentvalue / maturityvalue)) * (intdaysB * 100 / daysTomaturity)

        End If

        Return Math.Round(brkRate, 9)

    End Function
    Private Sub YieldMethod()
        Dim TotalProfit As Decimal
        Dim IntClient As Decimal
        Dim TotalInterest As Decimal
        Dim AccruedOnSaleAmt As Decimal
        'Dim SellPV As Decimal
        Dim EffectiveRate As Decimal
        Dim intToDate1 As Decimal
        Dim interest As Decimal
        Dim DaysRun As Integer
        Dim MaturityAmnt As Decimal

        btnSale.Enabled = False

        'Holding period Yield Variables
        Dim HoldingReturn As Decimal ' Holding return - Accrued interest Plus the Profit/Loss on the sale
        Dim AHPY As Decimal ' Annual Holding Preiod Yield

        If txtSellTenor.Text = "" Then

            Exit Sub
        End If

        If txtSaleRate.Text = "" Then
            MsgBox("Enter the interest rate", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If txtSale.Text = "" Then
            Exit Sub
        End If
        interest = PresentV * CDec(lblTenor.Text) * CDec(txtIntRate.Text) / (Int(lblBasis.Text) * 100)
        lblfv = PresentV + interest
        If Int(txtSellTenor.Text) > Int(txtDaysMaturity.Text) Then 'tenor of sale is greater than days to maturity of purchase
            MsgBox("Transaction will be backvalued and breakeven rate will change.", MsgBoxStyle.Information, "Sell")
            intToDate1 = (PresentV * (CDec(txtIntRate.Text)) * (CDec(lblTenor.Text) - Int(txtSellTenor.Text))) / (Int(lblBasis.Text) * 100)
            lblbreakeven.Text = BreakEvenRate(CDec(lblfv.Text), PresentV + intToDate1, Int(lblBasis.Text), Int(txtSellTenor.Text), "Y")
        ElseIf Int(txtSellTenor.Text) < Int(txtDaysMaturity.Text) Then 'tenor of sale is less than days to maturity of purchase

            intToDate1 = (PresentV * (CDec(txtIntRate.Text)) * (CDec(lblTenor.Text) - Int(txtDaysMaturity.Text))) / (Int(lblBasis.Text) * 100)
            lblbreakeven.Text = BreakEvenRate(CDec(lblfv.Text), PresentV + intToDate1, Int(lblBasis.Text), Int(txtSellTenor.Text), "Y")
        Else 'tenor of sale is equal to days to maturity of purchase
            lblbreakeven.Text = BreakEvenRate(CDec(lblfv.Text), PresentV + intToDate, Int(lblBasis.Text), Int(txtDaysMaturity.Text), "Y")
        End If

        'Get accrual on sell amount 
        If Int(txtDaysMaturity.Text) < Int(txtSellTenor.Text) Then
            backvalue = Int(txtSellTenor.Text) - Int(txtDaysMaturity.Text)
            DaysRun = (CDec(lblTenor.Text) - Int(txtSellTenor.Text))
            'AccruedOnSaleAmt = (CDec(txtSale.Text) * (CDec(txtIntRate.Text)) * (CDec(lblTenor.Text) - Int(txtSellTenor.Text))) / (Int(lblBasis.Text) * 100)
            DaysRun = (CDec(lblTenor.Text) - Int(txtSellTenor.Text))
        Else
            'AccruedOnSaleAmt = (CDec(txtSale.Text) * (CDec(txtIntRate.Text)) * (CDec(lblTenor.Text) - Int(txtDaysMaturity.Text))) / (Int(lblBasis.Text) * 100)
            DaysRun = (CDec(lblTenor.Text) - Int(txtDaysMaturity.Text))
            backvalue = 0
        End If

        txtSellPV.Text = CDec(txtSale.Text)
        cost = CDec(txtSellPV.Text) / (1 + ((DaysRun * CDec(txtIntRate.Text)) / (Int(lblBasis.Text) * 100)))
        AccruedOnSaleAmt = CDec(txtSellPV.Text) - cost

        'Calculate total interest to be accrued from inception to maturity 
        TotalInterest = cost * Int(lblTenor.Text) * CDec(txtIntRate.Text) / (Int(lblBasis.Text) * 100)

        'Client interest
        IntClient = CDec(txtSellPV.Text) * CDec(txtSaleRate.Text * Int(txtSellTenor.Text)) / (Int(lblBasis.Text) * 100)

        'Capital gain
        TotalProfit = TotalInterest - IntClient - AccruedOnSaleAmt

        'Calculate the effective rate for the client
        'lblEffectiveRate.Text = Math.Round((IntClient * Int(lblBasis.Text) * 100) / (CDec(txtSellPV.Text) * Int(txtSellTenor.Text)), 9)

        'Calculate client maturity amount
        MaturityAmnt = CDec(txtSale.Text) + IntClient

        '**************************************************************************************

        ''Calculate own profit
        'TotalProfit = TotalInterest - IntClient
        If TotalProfit < 0 Then
            lblprofit.Text = "Capital Loss"
        Else
            lblprofit.Text = "Capital gain"
        End If

        txtProfit.Text = Format(TotalProfit, "###,###,###.00")
        txtProfit.ForeColor = Color.BlueViolet
        'Label11.Text = Format(CDec(txtSellPV.Text), "###,###,###.00")
        lblCost.Text = Format(cost, "###,###,###.00")
        lblCost.ForeColor = Color.Red
        'Compute the holding period Yield
        HoldingReturn = ((Int(lblTenor.Text) - Int(txtDaysMaturity.Text)) * (CDec(txtIntRate.Text) * CDec(txtSale.Text) / 100) / CDec(lblBasis.Text)) + TotalProfit

        If (Int(lblTenor.Text) - Int(txtDaysMaturity.Text)) = 0 Then
            AHPY = 0
        Else

            AHPY = (HoldingReturn / cost) * (CDec(lblBasis.Text) * 100 / (Int(lblTenor.Text) - Int(txtDaysMaturity.Text))) ' (return/Amount Invested)* (DaysBasis/Number of days invested)
        End If

        AnnualisedHPY.Text = Format(AHPY, "###,###.00")
        'End of AHPY Calculations


        txtSellTenor.Enabled = False
        txtSaleRate.Enabled = False
        txtSale.Enabled = False
        btnSale.Enabled = True
    End Sub
   

    Protected Sub btnSale_Click(sender As Object, e As EventArgs) Handles btnSale.Click
        On Error Resume Next
        'Call this routing to re-validate amount available for sell

        checkAmtAvailable()


        Dim SellingAmt As Decimal
        Dim amount As Decimal
        If Trim(lblSellingOPT.Text) = "" Then
            MsgBox("Select Selling option.", MsgBoxStyle.Information, "Sell Option")
            lblSellingOPT.Focus()
            Exit Sub
        End If

        If Trim(lblSellingOPT.Text).Equals("Cost") Then
            SellingAmt = CDec(txtAvalableForSale.Text)

        Else
            SellingAmt = CDec(lblAvalableForSalePV.Text)

        End If


        If Trim(rate.Text) = "Interest Rate" Then
            amount = CDec(lblCost.Text)
        Else
            amount = CDec(txtSale.Text)

        End If

        If Trim(txtSaleRate.Text) = "" Then
            MsgBox("Enter the interest rate.", MsgBoxStyle.Critical, "Sale")
            Exit Sub
        End If

        If txtSale.Text = "" Then
            MsgBox("Enter Sale amount.", MsgBoxStyle.Critical, "Sale")
            Exit Sub
        End If

        'check if amount is not greater than what is available to sale
        If SellingAmt - amount < 0 Then
            MsgBox("Sale amount is greater than what is available.", MsgBoxStyle.Critical, "Sale")
            Exit Sub
        End If



        If CDec(txtSale.Text) = 0 Then
            MsgBox("Sale amount cannot be zero.", MsgBoxStyle.Critical, "Inconsistent operation")
            Exit Sub
        End If


        'If DealPortfolio = "D" Then
        '    DiscSale.Checked = True
        'Else
        '    YieldSale.Checked = True
        'End If

        ''Get the TB ID
        securityRef = Trim(cmbTB.SelectedValue.ToString)
        dealRef = getRef(Trim(cmbTB.SelectedValue.ToString))

        SellingOption = "Security Sale - Single"
        Call saleDetails()
        Response.Redirect("SecuritySell.aspx")
    End Sub
    Private Sub saleDetails()
        Session("salestartD") = lblPurchaseStart.Text
        Session("saleMaturityD") = txtmaturity.Text
        Session("saleFutureValue") = txtSale.Text
        Session("saleTenor") = txtSellTenor.Text
        Session("saleDiscount") = txtSaleRate.Text
        Session("saleTB") = lbltb.Text
        Session("saleDealRef") = Trim(getRef(cmbTB.SelectedValue.ToString))
        Session("saleOPT") = selloptionValue.Text
        Session("DaysBasis") = lblBasis.Text
        Session("currencySAle") = lblCurrency.Text
        Session("commrate") = commrate
        Session(" lblbreakeven") = lblbreakeven.Text
        Session("Gain") = txtProfit.Text
        Session("Cost") = lblCost.Text
        Session("PV") = CDec(txtSellPV.Text)
        Session("Single") = "Single"
    End Sub
End Class
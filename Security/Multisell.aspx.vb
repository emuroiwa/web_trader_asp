Imports System.Data.SqlClient
Imports sys_ui
Public Class Multisell
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
    'Private portidd As String
    Dim Buyback As String = ""
    Private checkMaturity As New mmDeal.DealMaturityCheck
    Private maturityAmt As Decimal = 0


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then
            Session("loadGrid") = Nothing
            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then

                currency = Request.QueryString("currency")
                Days = Request.QueryString("Days")
                MatAmount = Request.QueryString("MatAmount")
                portidd.text = Request.QueryString("portID")
                If Request.QueryString("selloption") = "Discount" Then
                    selloptionValue.Text = "D"

                Else
                    selloptionValue.Text = "Y"


                End If
                Session("MultiSellOPT") = Request.QueryString("selloption")

                Call GetSecurityDetailmulti(currency, portidd.Text, Days, selloptionValue.Text)
            Else
                lblError.Text = alert("Please make sure all fields are selected On the newDeal Page", "Incomplete informaton")
            End If




        End If
    End Sub
    Private Sub GetSecurityDetailmulti(ByVal ccy As String, ByVal portid As Double, ByVal daysR As Integer, ByVal sellOpt As String)

        'Dim SelOption As String

        'If RadioDiscount.Checked = True Then
        '    SelOption = "D"
        'Else
        '    SelOption = "Y"
        'End If



        Try
            'validate username first
            strSQL = "select securitypurchase.tb_id,securitypurchase.dealreference,entrytype,deals_live.dealamount,deals_live.maturityamount from " & _
                     " securitypurchase join deals_live on" & _
                     " deals_live.dealreference=securitypurchase.dealreference join dealtypes on" & _
                     " dealtypes.deal_code=deals_live.dealtype where dealtypes.othercharacteristics='Trading'" & _
                     " and matured = 'N' and daystomaturity > " & daysR & "  and authorisationstatus='A'" & _
                     " and entrytype='" & sellOpt & "' and portfolioid='" & portid & "' and dealtypes.currency='" & Trim(ccy) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()



            Do While drSQL.Read

                If drSQL.Item("entrytype").ToString = "D" Then


                    If (CDec(drSQL.Item("maturityamount").ToString) - SecurityAttachedMulti(drSQL.Item(0).ToString, drSQL.Item(1).ToString)) > 0 Then
                        'Dim itms As New ListViewItem(Trim(drSQL.Item(0).ToString))
                        'itms.SubItems.Add(Trim(drSQL.Item(1).ToString))

                        cmbTB.Items.Add(New ListItem(Trim(drSQL.Item(0).ToString) + "  " + Trim(drSQL.Item(1).ToString), Trim(drSQL.Item(0).ToString)))

                    End If

                Else
                    If (CDec(drSQL.Item("dealamount").ToString) - SecurityAttachedMulti(drSQL.Item(0).ToString, drSQL.Item(1).ToString)) > 0 Then
                        'Dim itms As New ListViewItem(Trim(drSQL.Item(0).ToString))
                        'itms.SubItems.Add(Trim(drSQL.Item(1).ToString))

                        cmbTB.Items.Add(New ListItem(Trim(drSQL.Item(0).ToString) + "  " + Trim(drSQL.Item(1).ToString), Trim(drSQL.Item(0).ToString)))

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
    Private Function SecurityAttachedMulti(ByVal secID As String, ByVal DealRef As String) As Decimal
        Dim x As String
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Try
            'validate username first
            strSQL1 = "select sum(securityamount)[hhh] from attachedsecurities where tb_id = '" & secID & "' and dealreferencesecurity='" & DealRef & "' "

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

    Protected Sub cmbTB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTB.SelectedIndexChanged
        Call GetSecurityDetailAmounts()
    End Sub
    Private Sub GetSecurityDetailAmounts()
        Try
            'validate username first
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
                    lblAvail.Text = Format((CDec(drSQL.Item(12).ToString) - SecurityAttachedMulti(drSQL.Item(0).ToString, drSQL.Item(1).ToString)), "###,###,###.00")
                Else
                    lblAvail.Text = Format((CDec(drSQL.Item(11).ToString) - SecurityAttachedMulti(drSQL.Item(0).ToString, drSQL.Item(1).ToString)), "###,###,###.00")
                End If

                lblDaysToMaturity.Text = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(drSQL.Item(4)))
                lblPurchaseRate.Text = drSQL.Item(3).ToString
                lblTenorMulti.Text = Trim(drSQL.Item("tenor").ToString)
                lblBasisMulti.Text = Trim(drSQL.Item("intdaysbasis").ToString)
                lblCurrency.Text = Trim(drSQL.Item("currency").ToString)
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

        Catch aa As NullReferenceException


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

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("newsell.aspx")
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtSellTenorMulti.Enabled = True
        txtRate.Enabled = True
        lblAvail.Text = ""
        lblPurchaseRate.Text = ""
        lblDaysToMaturity.Text = ""
        lblTenorMulti.Text = ""
        lblBasisMulti.Text = ""
        lblTotal.Text = 0
        txtSellTenorMulti.Text = ""
        txtRate.Text = ""

        Buyback = ""
        Grid1.Items.Clear()
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        'check if sell amount has been entered
        If txtSellAmt.Text = "" Then
            MsgBox("Enter sell amount", MsgBoxStyle.Information, "Sell")
            Exit Sub
        End If

        'check if tenor has been entered
        If txtSellTenorMulti.Text = "" Then
            MsgBox("Enter tenor", MsgBoxStyle.Information, "Sell")
            Exit Sub
        End If


        'check if rate has been entered
        If txtRate.Text = "" Then
            MsgBox("Enter rate", MsgBoxStyle.Information, "Sell")
            Exit Sub
        End If

        'check if there are sells in the list
        If Trim(cmbTB.SelectedValue.ToString) = "" Then
            MsgBox("No purchase to sell", MsgBoxStyle.Information, "Sell")
            Exit Sub
        End If

        If lblDaysToMaturity.Text = "" Then
            Exit Sub
        End If

        'Check if proposed maturity date is not a non-business day
        If checkMaturity.NonBusinessDay(DateAdd(DateInterval.Day, Int(txtSellTenorMulti.Text), CDate(Session("SysDate")))) = True Then
            MsgBox("The selected maturity date is a not a business day.", "Non-Business Day")
            Exit Sub
        End If
        'If checkMaturity.Holidays(DateAdd(DateInterval.Day, Int(txtSellTenorMulti.Text), CDate(Session("SysDate"))), getBaseCurrency) = True Then
        '    MsgBox("The selected maturity date is a holiday.", "Non-Business Day")
        '    Exit Sub
        'End If


        If Int(txtSellTenorMulti.Text) < Int(lblDaysToMaturity.Text) Then
            If Buyback = "" Then
                Buyback = "1" ' First Sell is a buyback
            End If
        Else
            If Buyback = "" Then
                Buyback = "2" 'First Sell is Outright
            End If
        End If


        If Int(txtSellTenorMulti.Text) < Int(lblDaysToMaturity.Text) And Buyback = "2" Then
            MsgBox("All sells must be Outright.", MsgBoxStyle.Information, "Invalid Sell") ' If first Sell is Buy back all subsequent Sells must be buy backs
            Exit Sub
        ElseIf Int(txtSellTenorMulti.Text) = Int(lblDaysToMaturity.Text) And Buyback = "1" Then
            MsgBox("All sells must be BuyBack.", MsgBoxStyle.Information, "Invalid Sell") 'If first Sell is Outright all subsequent Sells must be Outright
            Exit Sub
        End If
        'Disable rate and tenor fields so that the same values are used for subsiquent adds
        txtSellTenorMulti.Enabled = False
        txtRate.Enabled = False



        Try


            'Check if the amount entered is not greater than amount available
            If CDec(txtSellAmt.Text) > CDec(lblAvail.Text) Then
                MsgBox("Sell amount entered is greater than amount available", MsgBoxStyle.Information, "Sell")
                Exit Sub
            End If

            'check tenor
            If Int(txtSellTenorMulti.Text) > Int(lblDaysToMaturity.Text) Then
                MsgBox("Selected tenor is greater than days remaining before security matures", MsgBoxStyle.Information, "Sell")
                Exit Sub
            End If

            If Session("MultiSellOPT") = "Discount" Then
                DiscountMethodMulti()
            Else
                YieldMethodMulti()
            End If
            lblpuref.Text = Trim(cmbTB.SelectedValue.ToString)
            lblreff.Text = getRef(Trim(cmbTB.SelectedValue.ToString))
            Dim dt As New DataTable()
            dt.Columns.Add(New DataColumn("Item #", GetType(Integer)))
            dt.Columns.Add(New DataColumn("SecRef", GetType(String)))
            dt.Columns.Add(New DataColumn("dealref", GetType(String)))
            dt.Columns.Add(New DataColumn("SelAmt", GetType(String)))
            dt.Columns.Add(New DataColumn("cost", GetType(String)))
            dt.Columns.Add(New DataColumn("profit", GetType(String)))
            dt.Columns.Add(New DataColumn("presentV", GetType(String)))
            dt.Columns.Add(New DataColumn("matamt", GetType(String)))



            Session("loadGrid") = Session("loadGrid") & "|" & lblpuref.Text & "," & lblreff.Text & "," & txtSellAmt.Text & "," & cost.Text & "," & profit.Text & "," & sellpv.Text & "," & maturityAmt
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



            If lblTotal.Text = "" Then
                lblTotal.Text = "0"
            End If

            lblTotal.Text = Format(CDec(lblTotal.Text) + CDec(txtSellAmt.Text), "###,###,###.00")

            txtSellAmt.Text = ""

        Catch ex As NullReferenceException
            MsgBox("Nothing Selected", MsgBoxStyle.Exclamation, "Sell")

        Catch es As Exception

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
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " CheckBaseCurrency", Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function
    Private Sub DiscountMethodMulti()
        'cost = 0
        'CDec(sellpv.Text) = 0
        'TotalProfit = 0

        Dim IntClient As Decimal
        Dim TotalInterest As Decimal
        Dim AccruedOnSaleAmt As Decimal
        Dim backvalue As Integer

        'Get accrual on sell amount 
        If Int(lblDaysToMaturity.Text) < Int(txtSellTenorMulti.Text) Then
            backvalue = Int(txtSellTenorMulti.Text) - Int(lblDaysToMaturity.Text)

            AccruedOnSaleAmt = (CDec(txtSellAmt.Text) * (CDec(lblPurchaseRate.Text)) * (Int(lblTenorMulti.Text) - Int(txtSellTenorMulti.Text))) / (Int(lblBasisMulti.Text) * 100)
        Else
            AccruedOnSaleAmt = (CDec(txtSellAmt.Text) * (CDec(lblPurchaseRate.Text)) * (CDec(lblTenorMulti.Text) - Int(lblDaysToMaturity.Text))) / (Int(lblBasisMulti.Text) * 100)
            backvalue = 0
        End If

        'get present value of sell amount
        sellpv.Text = CDec(txtSellAmt.Text) - AccruedOnSaleAmt

        'Calculate total interest to be accrued on sell amount for days to maturity
        TotalInterest = (CDec(txtSellAmt.Text) * (CDec(lblPurchaseRate.Text)) * Int(txtSellTenorMulti.Text)) / (Int(lblBasisMulti.Text) * 100)

        'Calculate interest due to client
        IntClient = CDec(txtSellAmt.Text) * CDec(txtRate.Text * Int(txtSellTenorMulti.Text)) / (Int(lblBasisMulti.Text) * 100)

        'Calculate own profit
        profit.Text = TotalInterest - IntClient


        TotalInterest = (CDec(txtSellAmt.Text) * (CDec(lblPurchaseRate.Text)) * Int(lblTenorMulti.Text)) / (Int(lblBasisMulti.Text) * 100)
        cost.Text = CDec(txtSellAmt.Text) - TotalInterest
        maturityAmt = CDec(txtSellAmt.Text)

    End Sub

    Private Sub YieldMethodMulti()
        'cost = 0
        'SellPV = 0
        'TotalProfit = 0

        Dim IntClient As Decimal
        Dim TotalInterest As Decimal
        Dim AccruedOnSaleAmt As Decimal
        Dim EffectiveRate As Decimal
        Dim intToDate1 As Decimal
        Dim interest As Decimal
        Dim DaysRun As Integer
        Dim backvalue As Integer

        'interest = PresentV * CDec(lblTenor.Text) * CDec(lblPurchaseRate.Text) / (Int(lblBasis.Text) * 100)
        ' MaturityV = PresentV + interest

        'Get accrual on sell amount 
        If Int(lblDaysToMaturity.Text) < Int(txtSellTenorMulti.Text) Then
            backvalue = Int(txtSellTenorMulti.Text) - Int(lblDaysToMaturity.Text)

            AccruedOnSaleAmt = (CDec(txtSellAmt.Text) * (CDec(lblPurchaseRate.Text)) * (Int(lblTenorMulti.Text) - Int(txtSellTenorMulti.Text))) / (Int(lblBasisMulti.Text) * 100)
        Else
            AccruedOnSaleAmt = (CDec(txtSellAmt.Text) * (CDec(lblPurchaseRate.Text)) * (CDec(lblTenorMulti.Text) - Int(lblDaysToMaturity.Text))) / (Int(lblBasisMulti.Text) * 100)
            backvalue = 0
        End If

        sellpv.Text = CDec(txtSellAmt.Text)
        cost.Text = CDec(sellpv.Text) / (1 + (((Int(lblTenorMulti.Text) - Int(txtSellTenorMulti.Text)) * CDec(lblPurchaseRate.Text)) / (Int(lblBasisMulti.Text) * 100)))
        AccruedOnSaleAmt = CDec(sellpv.Text) - CDec(cost.Text)
        '***************************************************************************************

        'Calculate interest to be accrued for duration of sell
        TotalInterest = cost * Int(lblTenorMulti.Text) * CDec(lblPurchaseRate.Text) / (Int(lblBasisMulti.Text) * 100)

        'Client interest
        IntClient = CDec(sellpv.Text) * CDec(txtRate.Text * Int(txtSellTenorMulti.Text)) / (Int(lblBasisMulti.Text) * 100)

        'Profit
        profit.Text = TotalInterest - IntClient - AccruedOnSaleAmt

        maturityAmt = (CDec(sellpv.Text) * CDec(txtRate.Text) * Int(txtSellTenorMulti.Text)) / (Int(lblBasisMulti.Text) * 100)

    End Sub
    Private Sub MiltiDetails()
        'Session("salestartD") = lblPurchaseStart.Text
        'Session("saleMaturityD") = txtmaturity.Text
        Session("MultiFutureValue") = lblTotal.Text
        Session("MultiTenor") = txtSellTenorMulti.Text
        Session("MultiDiscount") = txtRate.Text
        'Session("MultiTB") = lbltb
        'Session("MultiDealRef") = Trim(getRef(cmbTB.SelectedValue.ToString))
        Session("MultiOPT") = selloptionValue.Text
        'Session("DaysBasis") = lblBasis.Text
        Session("MultiSAle") = lblCurrency.Text
        Session("portid") = portidd.Text
        'Session(" lblbreakeven") = lblbreakeven.Text
        Session("MultiGain") = profit.Text
        Session("MultiCost") = cost.Text
        Session("MultiPV") = CDec(sellpv.Text)
        Session("Single") = "Multi"
        Dim selectedCollateral As String
        'For Each item As Global.EO.Web.GridItem In Grid1.Items
        '    selectedCollateral = item.Cells(1).Value
        '    'If Trim(selectedCollateral) = Trim(lblsecref.Text) Then
        '    '    'messagebox required  Security has already been added to the list. Do you want to amend the security amount with this amount?
        '    '    ' If MessageBox.Show("Security has already been added to the list. Do you want to amend the security amount with this amount?" & _
        '    '    '" .", "Security", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
        '    '    '     Exit Sub
        '    '    ' Else
        '    '    '     checkref = True
        '    '    ' End If

        '    '    checkref = True
        '    'End If
        'Next
    End Sub

    Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Call MiltiDetails()
        Response.Redirect("SecuritySell.aspx")
    End Sub

    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click

        Session("loadGrid") = Nothing
        lblTotal.Text = 0
        Dim x As Integer

        For x = 1 To Grid1.Items.Count - 1
            Session("loadGrid") = Session("loadGrid") & "|" & Grid1.Items(x).Cells(1).Value & "," & Grid1.Items(x).Cells(2).Value & "," & Grid1.Items(x).Cells(3).Value & "," & _
                Grid1.Items(x).Cells(4).Value & "," & Grid1.Items(x).Cells(5).Value & "," & Grid1.Items(x).Cells(6).Value & "," & Grid1.Items(x).Cells(7).Value
        Next


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
      
        Dim arrlen As Integer = arr.Length
        Dim x1 As Integer

        For x1 = 1 To arr.Length - 1
            Dim arr2() As String = Split(arr(x1), ",")
            If Trim(Session("removedItem")) <> x1.ToString Then
                Dim dr As DataRow = dt.NewRow()
                dr("SecRef") = arr2(0)
                dr("dealref") = arr2(1)
                dr("SelAmt") = arr2(2)
                dr("cost") = arr2(3)
                dr("profit") = arr2(4)
                dr("presentV") = arr2(5)
                dr("matamt") = arr2(6)
                dt.Rows.Add(dr)
                lblTotal.Text += CDec(arr2(2))
            End If
        Next

        Me.Grid1.Visible = True

        Grid1.DataSource = dt

        Grid1.DataBind()
        'Format(CDec(Grid1.Items(x).Cells(2).Value), "###,###,###.00")
    End Sub
End Class
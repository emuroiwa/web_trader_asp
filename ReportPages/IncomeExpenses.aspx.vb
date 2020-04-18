Imports System.Data.SqlClient
Imports EO.Web

Public Class IncomeExpenses
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private cnSQLX As SqlConnection
    Private strSQLX As String
    Private cmSQLX As SqlCommand
    Private drSQLX As SqlDataReader



    Private selectedDeals As String
    Private selectedDealer As String
    Private TotalIncome As Decimal = 0
    Private TotalExpense As Decimal = 0
    Private TotalTax As Decimal = 0
    Private TotalComm As Decimal = 0
    Private commision As Decimal
    Private object_userlog As New usrlog.usrlog

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call GetDealerNames()
            Call loadCurrencies()
            GetDealCodes()
           

        End If
    End Sub

    Private Sub GetDealerNames()

        Try
            strSQLX = "select user_id from users "
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

           
            GridDealer.DataSource = drSQLX
            GridDealer.DataBind()

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As SqlException
                      'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try

    End Sub

    Protected Sub cmbCurrency_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCurrency.SelectedIndexChanged
        Call GetDealCodes()
    End Sub
    Private Sub GetDealCodes()
        Try
            strSQLX = ("select Deal_code,dealBasictype from dealtypes where [application] in('MM','SS')")
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            GridDeals.DataSource = drSQLX
            GridDeals.DataBind()

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try

    End Sub
    Private Sub loadCurrencies()
        Try
            strSQL = "select currencycode from astval"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            cmbCurrency.Items.Clear()
            Do While drSQL.Read
                cmbCurrency.Items.Add(Trim(drSQL.Item(0).ToString))
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
    End Sub

    Protected Sub bntGet_Click(sender As Object, e As EventArgs) Handles bntGet.Click
        Lblcomm.Text = Format(0, "###,###,###.00")
        lblTax.Text = Format(0, "###,###,###.00")
        lblIntIncome.Text = Format(0, "###,###,###.00")
        lnlIntExpense.Text = Format(0, "###,###,###.00")
        lblNetPos.Text = Format(0, "###,###,###.00")

        TotalIncome = 0
        TotalExpense = 0
        TotalTax = 0
        TotalComm = 0
        getdealer()
        getdeals()
        'If cmbCurrency.Text = "" Then
        '    Response.Write("<script>  alert('select currency . Error.') </script>")


        '    Exit Sub
        'End If

        'If selectedDeals = "" Then

        '    Response.Write("<script>  alert('Select Deal codes') </script>")
        '    Exit Sub
        'End If


        'If selectedDealer = "" Then

        '    Response.Write("<script>  alert('Select Deal codes') </script>")
        '    Exit Sub
        'End If

        'get deals starting within selected period but matuaring outside the selected period
        Call DealsInPeriod("deals_live", "Live", selectedDeals, selectedDealer, Trim(cmbCurrency.Text))
        Call DealsInPeriod("deals_matured", "Matured", selectedDeals, selectedDealer, Trim(cmbCurrency.Text))
        '____________________________________________________________________________________
        'deals with a maturity date within the selected period and the startdate outside the selected period
        Call DealsEndDateInPeriod("deals_live", "Live", selectedDeals, selectedDealer, Trim(cmbCurrency.Text))
        Call DealsEndDateInPeriod("deals_matured", "Matured", selectedDeals, selectedDealer, Trim(cmbCurrency.Text))
        '____________________________________________________________________________________

        'Deal start date and maturity date in period
        Call DealStartDealEndInPeriod("deals_live", "Live", selectedDeals, selectedDealer, Trim(cmbCurrency.Text))
        DealStartDealEndInPeriod("deals_matured", "Matured", selectedDeals, selectedDealer, Trim(cmbCurrency.Text))
        '____________________________________________________________________________________

        'Deal start date is less than the start period and deal maturity is greater than end period
        Call StartLessEndGreater("deals_live", "Live", selectedDeals, selectedDealer, Trim(cmbCurrency.Text))
        Call StartLessEndGreater("deals_matured", "Matured", selectedDeals, selectedDealer, Trim(cmbCurrency.Text))
        '____________________________________________________________________________________

        'Format the list items
        ' Call formatList()

        'Get Summary for deal type
        'Call GetDealtypeSumary()

        Lblcomm.Text = Format(TotalComm, "###,###,###.00")
        lblTax.Text = Format(TotalTax, "###,###,###.00")
        lblIntIncome.Text = Format(TotalIncome, "###,###,###.00")
        lnlIntExpense.Text = Format(TotalExpense, "###,###,###.00")
        lblNetPos.Text = Format((TotalIncome + TotalComm) + TotalExpense, "###,###,###.00")

        'Get Summary for dealer
        ' Call GetSumForDealer()

        'Load details into database for report
        ' Call SaveData()
    End Sub
    'Load details into database for report
    Private Sub SaveData(ByVal dealref As String, ByVal dealamount As Decimal, ByVal incexp As Decimal, ByVal start As String, ByVal maturity As String, ByVal interestrate As Integer, ByVal taxamount As Integer, ByVal tenor As Integer, ByVal commisionrate As Integer, ByVal commisionamount As Integer, ByVal dealstatus As String, ByVal dealcode As String, ByVal dealer As String, ByVal dataDate As String)
        Dim strSQLX1 As String
        Dim cnSQLX1 As SqlConnection
        Dim cmSQLX1 As SqlCommand
        Dim drSQLX1 As SqlDataReader

        Try
            strSQLX1 = "delete gpwork where userid='" & Trim(Session("username")) & "'"
            cnSQLX1 = New SqlConnection(Session("ConnectionString"))
            cnSQLX1.Open()
            cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
            drSQLX1 = cmSQLX1.ExecuteReader

            ' Close and Clean up objects
            drSQLX1.Close()
            cnSQLX1.Close()
            cmSQLX1.Dispose()
            cnSQLX1.Dispose()

        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

     

            Try
            strSQLX1 = "insert into gpwork (dealref,dealamount,incexp,start,maturity,interestrate,taxamount" & _
                       ",tenor,commisionrate,commisionamount,dealstatus,dealcode,dealer,userid,dataDate,periodstart,periodend) values('" & _
                        dealref & "','" & dealamount & "','" & incexp & "','" & start & "','" & maturity & "','" & interestrate & "','" & _
                        taxamount & "' ,'" & tenor & "' ,'" & commisionrate & "','" & commisionamount & "','" & dealstatus & "' ,'" & _
                        dealcode & "','" & dealer & "','" & Session("username") & "','" & dataDate & "','" & dtStart.Text & "','" & dtEnd.Text & "')"
                cnSQLX1 = New SqlConnection(Session("ConnectionString"))
                cnSQLX1.Open()
                cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
                drSQLX1 = cmSQLX1.ExecuteReader

                ' Close and Clean up objects
                drSQLX1.Close()
                cnSQLX1.Close()
                cmSQLX1.Dispose()
                cnSQLX1.Dispose()


            Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

            End Try

    End Sub
    Private Sub DealStartDealEndInPeriod(ByVal table As String, ByVal dealStatus As String, ByVal dealcodes As String, ByVal dealerz As String, ByVal ccy As String)
        Dim periodAffected As Integer = 0
        Dim dealRate As Decimal = 0
        Dim InterestIncome As Decimal = 0
        Dim DealAmnt As Decimal = 0
        Dim SaleAccrual() As Decimal = {0, 0}
        Dim intDaysBasis As Integer = 0
        Dim Periodaccrual As Decimal = 0
        Dim taxrate As Decimal = 0
        Dim income As Decimal = 0
        Dim expense As Decimal = 0
        Dim taxamount As Decimal = 0
        Dim Commission As Decimal = 0
        Dim comRate As Decimal = 0
        Dim Dealamount As Decimal = 0
        Dim acrual As Decimal = 0
        Dim tax As Decimal = 0
        Dim commison As Decimal = 0
        Dim incomeExp As Decimal = 0
        Try
            strSQLX = " select * from " & table & " where maturitydate between '" & Format(dtStart.Text, "Short Date") & _
                      "' and '" & Format(dtEnd.Text, "Short Date") & "' and startdate  between '" & _
                         Format(dtStart.Text, "Short Date") & "' and '" & Format(dtEnd.Text, "Short Date") & _
                         "'and dealtype in (" & dealcodes & ") and dealcapturer in (" & dealerz & ") and othercharacteristics <> 'Discount Sale' and currency='" & ccy & "'"

            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader
            Dim dt As New DataTable()
            dt.Columns.Add(New DataColumn("dealref", GetType(String)))
            dt.Columns.Add(New DataColumn("dealamnt", GetType(String)))
            dt.Columns.Add(New DataColumn("periodaccrual", GetType(String)))
            dt.Columns.Add(New DataColumn("tenor", GetType(String)))
            dt.Columns.Add(New DataColumn("startdate", GetType(String)))
            dt.Columns.Add(New DataColumn("maturitydate", GetType(String)))
            dt.Columns.Add(New DataColumn("taxamount", GetType(String)))
            dt.Columns.Add(New DataColumn("comrate", GetType(String)))
            dt.Columns.Add(New DataColumn("commision", GetType(String)))
            dt.Columns.Add(New DataColumn("dealstatus", GetType(String)))
            dt.Columns.Add(New DataColumn("dealcapturer", GetType(String)))
            dt.Columns.Add(New DataColumn("dealtype", GetType(String)))

            Dim dt2 As New DataTable()
            dt2.Columns.Add(New DataColumn("dealref", GetType(String)))
            dt2.Columns.Add(New DataColumn("dealamnt", GetType(String)))
            dt2.Columns.Add(New DataColumn("periodaccrual", GetType(String)))
            dt2.Columns.Add(New DataColumn("taxamount", GetType(String)))
            dt2.Columns.Add(New DataColumn("commision", GetType(String)))

            Dim dt3 As New DataTable()
            dt3.Columns.Add(New DataColumn("dealer", GetType(String)))
            dt3.Columns.Add(New DataColumn("income", GetType(String)))
            dt3.Columns.Add(New DataColumn("expense", GetType(String)))
            dt3.Columns.Add(New DataColumn("incexp", GetType(String)))
            Do While drSQLX.Read
                periodAffected = drSQLX.Item("tenor")
                dealRate = drSQLX.Item("interestrate")
                intDaysBasis = drSQLX.Item("intdaysbasis")
                DealAmnt = drSQLX.Item("dealamount")
                taxrate = drSQLX.Item("taxrate")
                comRate = drSQLX.Item("acceptancerate")

                If Trim(drSQLX.Item("othercharacteristics").ToString).Equals("Discount Purchase") = True Then
                    If checkForSells(Trim(drSQLX.Item("tb_id").ToString), Trim(drSQLX.Item("Dealreference").ToString)) = True Then
                        'Get the original purchase value
                        DealAmnt = GetDealAmountPurchase(Trim(drSQLX.Item("Dealreference").ToString))
                        'calculate contributions of sales to period for purchase
                        SaleAccrual = SellCalculations(Trim(drSQLX.Item("tb_id").ToString), Trim(drSQLX.Item("dealreference").ToString))
                    End If
                End If
                'Rollovers
                If checkForRolloverSetting(Trim(drSQLX.Item("Dealreference").ToString)) = True Then
                    SaleAccrual = RollOverCalculations(Trim(drSQLX.Item("Dealreference").ToString))
                    Periodaccrual = SaleAccrual(1)
                    taxamount = SaleAccrual(0)
                Else
                    Periodaccrual = ((DealAmnt * periodAffected * dealRate) / (intDaysBasis * 100))
                    taxamount = ((taxrate * Periodaccrual) / 100) + SaleAccrual(0)
                    Periodaccrual = ((DealAmnt * periodAffected * dealRate) / (intDaysBasis * 100)) - SaleAccrual(1)
                    Periodaccrual = Periodaccrual - taxamount
                End If
                commision = (comRate * drSQLX.Item("Maturityamount") * periodAffected) / (intDaysBasis * 100)

                Dim dr As DataRow = dt.NewRow()
                dr("dealref") = drSQLX.Item("dealreference").ToString
                dr("dealamnt") = Format(DealAmnt, "###,###,###.00")
                dr("periodaccrual") = Format(Periodaccrual, "###,###,###.00")
                dr("tenor") = drSQLX.Item("tenor").ToString
                dr("startdate") = drSQLX.Item("startdate").ToString
                dr("maturitydate") = drSQLX.Item("maturitydate").ToString
                dr("taxamount") = Format(taxamount, "###,###,###.00")
                dr("comrate") = comRate
                dr("commision") = Format(commision, "###,###,###.00")
                dr("dealstatus") = dealStatus
                dr("dealcapturer") = drSQLX.Item("dealcapturer").ToString
                dr("dealtype") = drSQLX.Item("dealtype").ToString
                dt.Rows.Add(dr)


                Me.GridCustomerDeals.Visible = True
                GridCustomerDeals.DataSource = dt
                GridCustomerDeals.DataBind()
                '****************************************************************************************************************************

                Dealamount = Dealamount + CDec(DealAmnt)
                incomeExp = incomeExp + CDec(Periodaccrual)
                tax = tax + CDec(taxamount)
                commison = commison + CDec(commision)


                If Trim(drSQLX.Item("dealtype").ToString).Equals("D") = True Then
                    Periodaccrual = Periodaccrual * -1
                    TotalExpense = TotalExpense + Periodaccrual
                Else
                    TotalIncome = TotalIncome + Periodaccrual
                End If

                TotalTax = TotalTax + taxamount
                TotalComm = TotalComm + comRate
                Dim dr1 As DataRow = dt2.NewRow()
                dr1("dealref") = drSQLX.Item("dealtype").ToString
                dr1("dealamnt") = Format(Dealamount, "###,###,###.00")
                dr1("periodaccrual") = Format(incomeExp, "###,###,###.00")
                dr1("taxamount") = Format(tax, "###,###,###.00")
                dr1("commision") = Format(commision, "###,###,###.00")

                dt2.Rows.Add(dr1)


                Me.GridDealSum.Visible = True
                GridDealSum.DataSource = dt2
                GridDealSum.DataBind()

                '*************************************************************************************************************************************************
                If Trim(drSQLX.Item("dealtype").ToString).Equals("L") = True Then
                    income = income + CDec(Periodaccrual)
                Else
                    expense = expense + CDec(Periodaccrual)
                End If

                Dim dr2 As DataRow = dt3.NewRow()
                dr2("dealer") = drSQLX.Item("dealcapturer").ToString
                dr2("income") = Format(income, "###,###,###.00")
                dr2("expense") = Format(expense, "###,###,###.00")
                dr2("incexp") = Format(income - expense, "###,###,###.00")

                dt3.Rows.Add(dr2)



                Me.GridDealerSum.Visible = True
                GridDealerSum.DataSource = dt3
                GridDealerSum.DataBind()

                'reinitialise variables
                periodAffected = 0
                dealRate = 0
                InterestIncome = 0
                DealAmnt = 0
                SaleAccrual(0) = 0
                SaleAccrual(1) = 0
                intDaysBasis = 0
                Periodaccrual = 0
                taxrate = 0
                taxamount = 0
            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try

    End Sub
    'Deal code summary
   

    
    'get deals starting within selected period but matuaring outside the selected period
    Private Sub DealsInPeriod(ByVal table As String, ByVal dealStatus As String, ByVal dealcodes As String, ByVal dealerz As String, ByVal ccy As String)
        Dim periodAffected As Integer = 0
        Dim dealRate As Decimal = 0
        Dim InterestIncome As Decimal = 0
        Dim DealAmnt As Decimal = 0
        Dim SaleAccrual() As Decimal = {0, 0}
        Dim intDaysBasis As Integer = 0
        Dim Periodaccrual As Decimal = 0
        Dim taxrate As Decimal = 0
        Dim taxamount As Decimal = 0
        Dim Commission As Decimal = 0
        Dim comRate As Decimal = 0

    
        Try
            strSQLX = "select * from " & table & " where startdate" & _
                      " between '" & Format(dtStart.Text, "Short Date") & "' and '" & _
                      Format(dtEnd.Text, "Short Date") & "' and maturitydate not between '" & _
                      Format(dtStart.Text, "Short Date") & "' and '" & Format(dtEnd.Text, "Short Date") & _
                      "' and othercharacteristics <> 'Discount Sale' and dealtype in (" & dealcodes & ") and dealcapturer in (" & dealerz & ") and currency='" & ccy & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader

            Do While drSQLX.Read
                periodAffected = Math.Abs(DateDiff(DateInterval.Day, CDate(dtEnd.Text), drSQLX.Item("startdate")))
                dealRate = drSQLX.Item("interestrate")
                intDaysBasis = drSQLX.Item("intdaysbasis")
                DealAmnt = drSQLX.Item("dealamount")
                taxrate = drSQLX.Item("taxrate")
                comRate = drSQLX.Item("acceptancerate")

                'Security sales/purchase
                If Trim(drSQLX.Item("othercharacteristics").ToString).Equals("Discount Purchase") = True Then

                    If checkForSells(Trim(drSQLX.Item("tb_id").ToString), Trim(drSQLX.Item("Dealreference").ToString)) = True Then

                        'Get the original purchase Text
                        DealAmnt = GetDealAmountPurchase(Trim(drSQLX.Item("Dealreference").ToString))

                        'calculate contributions of sales to period for purchase
                        SaleAccrual = SellCalculations(Trim(drSQLX.Item("tb_id").ToString), Trim(drSQLX.Item("dealreference").ToString))
                    End If

                End If

                'Rollovers
                If checkForRolloverSetting(Trim(drSQLX.Item("Dealreference").ToString)) = True Then
                    SaleAccrual = RollOverCalculations(Trim(drSQLX.Item("Dealreference").ToString))
                    Periodaccrual = SaleAccrual(1)
                    taxamount = SaleAccrual(0)
                Else


                    Periodaccrual = ((DealAmnt * periodAffected * dealRate) / (intDaysBasis * 100))
                    taxamount = ((taxrate * Periodaccrual) / 100) + SaleAccrual(0)
                    Periodaccrual = ((DealAmnt * periodAffected * dealRate) / (intDaysBasis * 100)) - SaleAccrual(1)

                    Periodaccrual = Periodaccrual - taxamount
                End If

                commision = (comRate * drSQLX.Item("Maturityamount") * periodAffected) / (intDaysBasis * 100)

                'Dim itm As New ListViewItem(Trim(drSQLX.Item("dealreference").ToString))
                'itm.SubItems.Add(Format(DealAmnt, "###,###,###.00"))
                'itm.SubItems.Add(Format(Periodaccrual, "###,###,###.00"))
                'itm.SubItems.Add(periodAffected)
                'itm.SubItems.Add(drSQLX.Item("tenor").ToString)
                'itm.SubItems.Add(Format(drSQLX.Item("startdate").ToString, "Short Date"))
                'itm.SubItems.Add(Format(drSQLX.Item("maturitydate").ToString, "Short Date"))
                'itm.SubItems.Add(dealRate)
                'itm.SubItems.Add(Format(taxamount, "###,###,###.00"))
                'itm.SubItems.Add(comRate)
                'itm.SubItems.Add(Format(commision, "###,###,###.00"))
                'itm.SubItems.Add(dealStatus)
                'itm.SubItems.Add(drSQLX.Item("dealcapturer").ToString)
                'itm.SubItems.Add(drSQLX.Item("dealtype").ToString)

                'lstDetails.Items.Add(itm)

                'reinitialise variables
                periodAffected = 0
                dealRate = 0
                InterestIncome = 0
                DealAmnt = 0
                SaleAccrual(0) = 0
                SaleAccrual(1) = 0
                intDaysBasis = 0
                Periodaccrual = 0
                taxrate = 0
                taxamount = 0
            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try


    End Sub

    'Deal start date is less than the start period and deal maturity is greater than end period
    Private Sub StartLessEndGreater(ByVal table As String, ByVal dealStatus As String, ByVal dealcodes As String, ByVal dealerz As String, ByVal ccy As String)
        Dim periodAffected As Integer = 0
        Dim dealRate As Decimal = 0
        Dim InterestIncome As Decimal = 0
        Dim DealAmnt As Decimal = 0
        Dim SaleAccrual() As Decimal = {0, 0}
        Dim intDaysBasis As Integer = 0
        Dim Periodaccrual As Decimal = 0
        Dim taxrate As Decimal = 0
        Dim taxamount As Decimal = 0
        Dim Commission As Decimal = 0
        Dim comRate As Decimal = 0


        Try
            strSQLX = " select * from " & table & " where startdate < '" & Format(dtStart.Text, "Short Date") & _
                       "' and  maturitydate >'" & Format(dtEnd.Text, "Short Date") & "'and dealtype in (" & dealcodes & ") and dealcapturer in (" & dealerz & ") and othercharacteristics <> 'Discount Sale' and currency='" & ccy & "'"

            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader

            Do While drSQLX.Read
                periodAffected = DateDiff(DateInterval.Day, CDate(dtStart.Text), CDate(dtEnd.Text))
                dealRate = drSQLX.Item("interestrate")
                intDaysBasis = drSQLX.Item("intdaysbasis")
                DealAmnt = drSQLX.Item("dealamount")
                taxrate = drSQLX.Item("taxrate")
                comRate = drSQLX.Item("acceptancerate")

                If Trim(drSQLX.Item("othercharacteristics").ToString).Equals("Discount Purchase") = True Then

                    If checkForSells(Trim(drSQLX.Item("tb_id").ToString), Trim(drSQLX.Item("Dealreference").ToString)) = True Then

                        'Get the original purchase value
                        DealAmnt = GetDealAmountPurchase(Trim(drSQLX.Item("Dealreference").ToString))

                        'calculate contributions of sales to period for purchase
                        SaleAccrual = SellCalculations(Trim(drSQLX.Item("tb_id").ToString), Trim(drSQLX.Item("dealreference").ToString))
                    End If

                End If

                'Rollovers
                If checkForRolloverSetting(Trim(drSQLX.Item("Dealreference").ToString)) = True Then
                    SaleAccrual = RollOverCalculations(Trim(drSQLX.Item("Dealreference").ToString))
                    Periodaccrual = SaleAccrual(1)
                    taxamount = SaleAccrual(0)
                Else

                    Periodaccrual = ((DealAmnt * periodAffected * dealRate) / (intDaysBasis * 100))
                    taxamount = ((taxrate * Periodaccrual) / 100) + SaleAccrual(0)
                    Periodaccrual = ((DealAmnt * periodAffected * dealRate) / (intDaysBasis * 100)) - SaleAccrual(1)

                    Periodaccrual = Periodaccrual - taxamount
                End If

                commision = (comRate * drSQLX.Item("Maturityamount") * periodAffected) / (intDaysBasis * 100)

                '' Dim itm As New ListViewItem(Trim(drSQLX.Item("dealreference").ToString))
                'itm.SubItems.Add(Format(DealAmnt, "###,###,###.00"))
                'itm.SubItems.Add(Format(Periodaccrual, "###,###,###.00"))
                'itm.SubItems.Add(periodAffected)
                'itm.SubItems.Add(drSQLX.Item("tenor").ToString)
                'itm.SubItems.Add(Format(drSQLX.Item("startdate").ToString, "Short Date"))
                'itm.SubItems.Add(Format(drSQLX.Item("maturitydate").ToString, "Short Date"))
                'itm.SubItems.Add(dealRate)
                'itm.SubItems.Add(Format(taxamount, "###,###,###.00"))
                'itm.SubItems.Add(comRate)
                'itm.SubItems.Add(Format(commision, "###,###,###.00"))
                'itm.SubItems.Add(dealStatus)
                'itm.SubItems.Add(drSQLX.Item("dealcapturer").ToString)
                'itm.SubItems.Add(drSQLX.Item("dealtype").ToString)

                'lstDetails.Items.Add(itm)

                'reinitialise variables
                periodAffected = 0
                dealRate = 0
                InterestIncome = 0
                DealAmnt = 0
                SaleAccrual(0) = 0
                SaleAccrual(1) = 0
                intDaysBasis = 0
                Periodaccrual = 0
                taxrate = 0
                taxamount = 0
            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try


    End Sub
    'deals with a maturity date within the selected period and the startdate outside the selected period
    Private Sub DealsEndDateInPeriod(ByVal table As String, ByVal dealStatus As String, ByVal dealcodes As String, ByVal dealerz As String, ByVal ccy As String)
        Dim periodAffected As Integer = 0
        Dim dealRate As Decimal = 0
        Dim InterestIncome As Decimal = 0
        Dim DealAmnt As Decimal = 0
        Dim SaleAccrual() As Decimal = {0, 0}
        Dim intDaysBasis As Integer = 0
        Dim Periodaccrual As Decimal = 0
        Dim taxrate As Decimal = 0
        Dim taxamount As Decimal = 0
        Dim Commission As Decimal = 0
        Dim comRate As Decimal = 0

        Try
            strSQLX = " select * from " & table & " where maturitydate between '" & Format(dtStart.Text, "Short Date") & _
                      "' and '" & Format(dtEnd.Text, "Short Date") & "' and startdate not between '" & _
                        Format(dtStart.Text, "Short Date") & "' and '" & Format(dtEnd.Text, "Short Date") & _
                        "'and dealtype in (" & dealcodes & ") and dealcapturer in(" & dealerz & ") and othercharacteristics <> 'Discount Sale' and currency='" & ccy & "'"

            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader

            Do While drSQLX.Read
                periodAffected = DateDiff(DateInterval.Day, CDate(dtStart.Text), drSQLX.Item("maturitydate"))
                dealRate = drSQLX.Item("interestrate")
                intDaysBasis = drSQLX.Item("intdaysbasis")
                DealAmnt = drSQLX.Item("dealamount")
                taxrate = drSQLX.Item("taxrate")
                comRate = drSQLX.Item("acceptancerate")

                If Trim(drSQLX.Item("othercharacteristics").ToString).Equals("Discount Purchase") = True Then

                    If checkForSells(Trim(drSQLX.Item("tb_id").ToString), Trim(drSQLX.Item("Dealreference").ToString)) = True Then

                        'Get the original purchase value
                        DealAmnt = GetDealAmountPurchase(Trim(drSQLX.Item("Dealreference").ToString))

                        'calculate contributions of sales to period for purchase
                        SaleAccrual = SellCalculations(Trim(drSQLX.Item("tb_id").ToString), Trim(drSQLX.Item("dealreference").ToString))
                    End If

                End If


                'Rollovers
                If checkForRolloverSetting(Trim(drSQLX.Item("Dealreference").ToString)) = True Then
                    SaleAccrual = RollOverCalculations(Trim(drSQLX.Item("Dealreference").ToString))
                    Periodaccrual = SaleAccrual(1)
                    taxamount = SaleAccrual(0)
                Else

                    Periodaccrual = ((DealAmnt * periodAffected * dealRate) / (intDaysBasis * 100))
                    taxamount = ((taxrate * Periodaccrual) / 100) + SaleAccrual(0)
                    Periodaccrual = ((DealAmnt * periodAffected * dealRate) / (intDaysBasis * 100)) - SaleAccrual(1)

                    Periodaccrual = Periodaccrual - taxamount
                End If

                commision = (comRate * drSQLX.Item("Maturityamount") * periodAffected) / (intDaysBasis * 100)


                ' Dim itm As New ListViewItem(Trim(drSQLX.Item("dealreference").ToString))

                'GridCustomerDeals.ClientSideOnCellOver(1) = "t"
                'itm.SubItems.Add(Format(DealAmnt, "###,###,###.00"))
                'itm.SubItems.Add(Format(Periodaccrual, "###,###,###.00"))
                'itm.SubItems.Add(periodAffected)
                'itm.SubItems.Add(drSQLX.Item("tenor").ToString)
                'itm.SubItems.Add(Format(drSQLX.Item("startdate").ToString, "Short Date"))
                'itm.SubItems.Add(Format(drSQLX.Item("maturitydate").ToString, "Short Date"))
                'itm.SubItems.Add(dealRate)
                'itm.SubItems.Add(Format(taxamount, "###,###,###.00"))
                'itm.SubItems.Add(comRate)
                'itm.SubItems.Add(Format(commision, "###,###,###.00"))
                'itm.SubItems.Add(dealStatus)
                'itm.SubItems.Add(drSQLX.Item("dealcapturer").ToString)
                'itm.SubItems.Add(drSQLX.Item("dealtype").ToString)

                'lstDetails.Items.Add(itm)

                'reinitialise variables
                periodAffected = 0
                dealRate = 0
                InterestIncome = 0
                DealAmnt = 0
                SaleAccrual(0) = 0
                SaleAccrual(1) = 0
                intDaysBasis = 0
                Periodaccrual = 0
                taxrate = 0
                taxamount = 0
            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try

    End Sub
    Private Function RollOverCalculations(ByVal dealref As String) As Decimal()
        Dim periodAffected As Integer
        Dim dealamount As Decimal
        Dim interestrate As Decimal
        Dim intDaysBasis As Integer
        Dim Periodaccrual As Decimal
        Dim taxrate As Decimal = 0
        Dim taxamount As Decimal = 0
        Dim result As Decimal() = {0, 0}
        Dim strSQLX4 As String
        Dim cnSQLX4 As SqlConnection
        Dim cmSQLX4 As SqlCommand
        Dim drSQLX4 As SqlDataReader


        Try

            strSQLX4 = " select * from dealrollover join deals_live on dealrollover.dealref" & _
                       "=deals_live.dealreference where dealref='" & dealref & "' order by rolloverdate desc"
            cnSQLX4 = New SqlConnection(Session("ConnectionString"))
            cnSQLX4.Open()
            cmSQLX4 = New SqlCommand(strSQLX4, cnSQLX4)
            drSQLX4 = cmSQLX4.ExecuteReader

            Do While drSQLX4.Read
                taxrate = drSQLX4.Item("taxrate")

                'rollover deal in period
                If drSQLX4.Item("rollstart") >= dtStart.Text And drSQLX4.Item("rollstart") <= dtEnd.Text And drSQLX4.Item("rollend") > dtEnd.Text Then
                    periodAffected = DateDiff(DateInterval.Day, CDate(dtEnd.Text), drSQLX4.Item("rollstart"))
                    dealamount = drSQLX4.Item("dealamt")
                    interestrate = drSQLX4.Item("intrate")
                    intDaysBasis = drSQLX4.Item("intdaysbasis")
                End If

                'deals with a maturity date within the selected period and the startdate outside the selected period
                If drSQLX4.Item("rollend") >= dtStart.Text And drSQLX4.Item("rollend") <= dtEnd.Text And drSQLX4.Item("rollstart") < dtStart.Text Then
                    periodAffected = DateDiff(DateInterval.Day, CDec(dtStart.Text), drSQLX4.Item("rollend"))
                    dealamount = drSQLX4.Item("dealamt")
                    interestrate = drSQLX4.Item("intrate")
                    intDaysBasis = drSQLX4.Item("intdaysbasis")
                End If

                'Deal start date and maturity date in period
                If drSQLX4.Item("rollstart") >= dtStart.Text And drSQLX4.Item("rollstart") <= dtEnd.Text And drSQLX4.Item("rollend") >= dtStart.Text And drSQLX4.Item("rollend") <= dtEnd.Text Then
                    periodAffected = Math.Abs(DateDiff(DateInterval.Day, drSQLX4.Item("rollstart"), drSQLX4.Item("rollend")))
                    dealamount = drSQLX4.Item("dealamt")
                    interestrate = drSQLX4.Item("intrate")
                    intDaysBasis = drSQLX4.Item("intdaysbasis")
                End If

                'Deal start date is less than the start period and deal maturity is greater than end period
                If drSQLX4.Item("rollstart") < dtStart.Text And drSQLX4.Item("rollend") > dtEnd.Text Then
                    periodAffected = DateDiff(DateInterval.Day, CDate(dtStart.Text), CDate(dtEnd.Text))
                    dealamount = drSQLX4.Item("dealamt")
                    interestrate = drSQLX4.Item("intrate")
                    intDaysBasis = drSQLX4.Item("intdaysbasis")
                End If

                'calculate the acrrual for period

                taxamount = taxamount + (dealamount * taxrate * interestrate * periodAffected) / (intDaysBasis * 100 * 100)

                Periodaccrual = Periodaccrual + (dealamount * periodAffected * interestrate) / (intDaysBasis * 100)


                periodAffected = 0


            Loop

            ' Close and Clean up objects
            drSQLX4.Close()
            cnSQLX4.Close()
            cmSQLX4.Dispose()
            cnSQLX4.Dispose()



            result(0) = taxamount
            result(1) = Periodaccrual - taxamount

            Return result

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try

    End Function
    Private Function checkForRolloverSetting(ByVal dealref As String) As Boolean
        Dim x As Boolean = False
        Dim strSQLX4 As String
        Dim cnSQLX4 As SqlConnection
        Dim cmSQLX4 As SqlCommand
        Dim drSQLX4 As SqlDataReader
        Try
            strSQLX4 = ("select * from dealrollover where dealref='" & dealref & "'")
            cnSQLX4 = New SqlConnection(Session("ConnectionString"))
            cnSQLX4.Open()
            cmSQLX4 = New SqlCommand(strSQLX4, cnSQLX4)
            drSQLX4 = cmSQLX4.ExecuteReader

            If drSQLX4.HasRows = True Then
                x = True
            End If

            ' Close and Clean up objects
            drSQLX4.Close()
            cnSQLX4.Close()
            cmSQLX4.Dispose()
            cnSQLX4.Dispose()

            Return x
        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try
    End Function
    Private Function checkForSells(ByVal tbid As String, ByVal dealref As String) As Boolean
        Dim x As Boolean = False
        Dim strSQLX1 As String
        Dim cnSQLX1 As SqlConnection
        Dim cmSQLX1 As SqlCommand
        Dim drSQLX1 As SqlDataReader
        Try
            strSQLX1 = "select * from selldetail where tbid = '" & tbid & "' and refpurchase='" & dealref & "'"
            cnSQLX1 = New SqlConnection(Session("ConnectionString"))
            cnSQLX1.Open()
            cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
            drSQLX1 = cmSQLX1.ExecuteReader
            If drSQLX1.HasRows = True Then x = True
            ' Close and Clean up objects
            drSQLX1.Close()
            cnSQLX1.Close()
            cmSQLX1.Dispose()
            cnSQLX1.Dispose()

            Return x

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
    End Function
    'returns the original deal amount for a purchase with sales
    Private Function GetDealAmountPurchase(ByVal dealref As String) As Decimal
        Dim x As Boolean = False
        Dim strSQLX1 As String
        Dim cnSQLX1 As SqlConnection
        Dim cmSQLX1 As SqlCommand
        Dim drSQLX1 As SqlDataReader
        Dim amt As Decimal
        Try
            strSQLX1 = "select dealamount from SecuritiesConfirmations where dealreference = '" & dealref & "'"
            cnSQLX1 = New SqlConnection(Session("ConnectionString"))
            cnSQLX1.Open()
            cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
            drSQLX1 = cmSQLX1.ExecuteReader
            Do While drSQLX1.Read
                amt = drSQLX1.Item(0)
            Loop
            ' Close and Clean up objects
            drSQLX1.Close()
            cnSQLX1.Close()
            cmSQLX1.Dispose()
            cnSQLX1.Dispose()

            Return amt

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try
    End Function
    Private Function SellCalculations(ByVal tbid As String, ByVal refsource As String) As Decimal()
        Dim periodAffected As Integer
        Dim dealamount As Decimal
        Dim interestrate As Decimal
        Dim intDaysBasis As Integer
        Dim Periodaccrual As Decimal
        Dim taxrate As Decimal = 0
        Dim taxamount As Decimal = 0
        Dim result As Decimal() = {0, 0}
        Dim strSQLX1 As String
        Dim cnSQLX1 As SqlConnection
        Dim cmSQLX1 As SqlCommand
        Dim drSQLX1 As SqlDataReader
        Dim strSQLX2 As String
        Dim cnSQLX2 As SqlConnection
        Dim cmSQLX2 As SqlCommand
        Dim drSQLX2 As SqlDataReader
        Dim x As Integer

        Try

            ' Look for sales made on the security purchase in SellDetail

            strSQLX2 = " select * from selldetail where refpurchase='" & refsource & "'"
            cnSQLX2 = New SqlConnection(Session("ConnectionString"))
            cnSQLX2.Open()
            cmSQLX2 = New SqlCommand(strSQLX2, cnSQLX2)
            drSQLX2 = cmSQLX2.ExecuteReader

            Do While drSQLX2.Read
                ' For each sale look for the corresponding entry in deals live to get the interest rate & affected period. This is necessary to cater for bulked sales
                For x = 1 To 2
                    If x = 1 Then
                        strSQLX1 = " select * from deals_live where dealreference='" & drSQLX2.Item("refsell") & "'"
                    Else
                        strSQLX1 = " select * from deals_matured where dealreference='" & drSQLX2.Item("refsell") & "'"
                    End If

                    cnSQLX1 = New SqlConnection(Session("ConnectionString"))
                    cnSQLX1.Open()
                    cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
                    drSQLX1 = cmSQLX1.ExecuteReader

                    Do While drSQLX1.Read

                        taxrate = drSQLX1.Item("taxrate")

                        'sale deal in period
                        If drSQLX1.Item("startdate") >= dtStart.Text And drSQLX1.Item("startdate") <= dtEnd.Text And drSQLX1.Item("maturitydate") > dtEnd.Text Then
                            periodAffected = DateDiff(DateInterval.Day, CDate(dtStart.Text), drSQLX1.Item("startdate"))
                            dealamount = drSQLX2.Item("PresentValue")
                            interestrate = drSQLX1.Item("interestrate")
                            intDaysBasis = drSQLX1.Item("intdaysbasis")
                        End If

                        'deals with a maturity date within the selected period and the startdate outside the selected period
                        If drSQLX1.Item("maturitydate") >= dtStart.Text And drSQLX1.Item("maturitydate") <= dtEnd.Text And drSQLX1.Item("startdate") < dtStart.Text Then
                            periodAffected = DateDiff(DateInterval.Day, CDate(dtStart.Text), drSQLX1.Item("maturitydate"))
                            dealamount = drSQLX2.Item("PresentValue")
                            interestrate = drSQLX1.Item("interestrate")
                            intDaysBasis = drSQLX1.Item("intdaysbasis")
                        End If

                        'Deal start date and maturity date in period
                        If drSQLX1.Item("startdate") >= dtStart.Text And drSQLX1.Item("startdate") <= dtEnd.Text And drSQLX1.Item("maturitydate") >= dtStart.Text And drSQLX1.Item("maturitydate") <= dtEnd.Text Then
                            periodAffected = drSQLX1.Item("tenor")
                            dealamount = drSQLX2.Item("PresentValue")
                            interestrate = drSQLX1.Item("interestrate")
                            intDaysBasis = drSQLX1.Item("intdaysbasis")
                        End If

                        'Deal start date is less than the start period and deal maturity is greater than end period
                        If drSQLX1.Item("startdate") < dtStart.Text And drSQLX1.Item("maturitydate") > dtEnd.Text Then
                            periodAffected = DateDiff(DateInterval.Day, CDate(dtStart.Text), CDate(dtEnd.Text))
                            dealamount = drSQLX2.Item("PresentValue")
                            interestrate = drSQLX1.Item("interestrate")
                            intDaysBasis = drSQLX1.Item("intdaysbasis")
                        End If

                        'calculate the acrrual for period

                        taxamount = taxamount + (dealamount * taxrate * interestrate * periodAffected) / (intDaysBasis * 100 * 100)

                        Periodaccrual = Periodaccrual + (dealamount * periodAffected * interestrate) / (intDaysBasis * 100)


                        periodAffected = 0

                    Loop


                    ' Close and Clean up objects
                    drSQLX1.Close()
                    cnSQLX1.Close()
                    cmSQLX1.Dispose()
                    cnSQLX1.Dispose()

                Next
            Loop

            drSQLX2.Close()
            cnSQLX2.Close()
            cmSQLX2.Dispose()
            cnSQLX2.Dispose()

            result(0) = taxamount
            result(1) = Periodaccrual - taxamount

            Return result

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try

    End Function
    Private Sub getdeals()


        For Each item As EO.Web.GridItem In GridDeals.CheckedItems
            selectedDeals = selectedDeals & ",'" & item.Cells(1).Value & "'"

        Next
        selectedDeals = Trim(Mid(selectedDeals, 2, Len(selectedDeals)))
    End Sub
    Private Sub getdealer()


        For Each item As EO.Web.GridItem In GridDealer.CheckedItems
            selectedDealer = selectedDealer & ",'" & item.Cells(1).Value & "'"
        Next
        selectedDealer = Trim(Mid(selectedDealer, 2, Len(selectedDealer)))
    End Sub

  
End Class
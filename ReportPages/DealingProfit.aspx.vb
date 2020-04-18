Imports System.Data.SqlClient

Public Class DealingProfit
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader

    Private curr As String
    Private object_userlog As New usrlog.usrlog
    Private AllCurrencies As String
    Private mmdeal_object As New mmDeal.DealMaturityCheck
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetCurrency()
            dtStart.Text = Session("SysDate")
            dtEnd.Text = Session("SysDate")
        End If
    End Sub
    Private Sub GetCurrency()
        Try

            strSQL = "select currencycode from astval"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader



            Do While drSQL.Read
                cmbCurrency.Items.Add(Trim(drSQL.Item(0)))
            Loop
            cmbCurrency.Items.Add("Consolidated")
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Sub
    Private Sub DoForAssetDeals()

        Try
            Dim Totallive As Integer = 0
            Dim TotalMatured As Integer = 0

            Dim curr As String

            If cmbCurrency.Text = "Consolidated" Then
                curr = "'" & Mid(AllCurrencies, 2, Len(AllCurrencies))
                curr = Replace(curr, ",", "','")
                curr = curr & "'"
            Else
                curr = "'" & Trim(cmbCurrency.Text) & "'"
            End If

            'Step 1
            'lets look for the deals that are running for the selected period
            'First for Liabilities as defined in Accruals analysis groups by the user

            strSQL = "select count(dealreference) from alldeals where alldeals.dealtype" & _
                     " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                     " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                     " ACCRUALANALYSISGRP.type='Asset') and " & _
                     " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                     " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                     " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                     " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                     " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')" & _
                     " ) and daystomaturity>0 and currency in(" & Trim(curr) & ")"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read()

                Totallive = drSQL.Item(0)

            Loop



            strSQL = "select count(dealreference) from alldeals where alldeals.dealtype" & _
                     " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                     " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                     " ACCRUALANALYSISGRP.type='Asset') and " & _
                     " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                     " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                     " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                     " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                     " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')" & _
                     " ) and daystomaturity=0 and currency in(" & Trim(curr) & ")"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read()
                TotalMatured = drSQL.Item(0)
            Loop



            LblLiveA.Text = Totallive
            lblMaturedA.Text = TotalMatured
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try

    End Sub

    Protected Sub btnDailyDealingProfit_Click(sender As Object, e As EventArgs) Handles btnDailyDealingProfit.Click

        If cmbCurrency.Text = "" Then
            MsgBox("Select a currency", MsgBoxStyle.Information)
            Exit Sub
        End If

        If cmbPeriodEnd.Text = "" Then
            MsgBox("Select the Run To option ", MsgBoxStyle.Information)
            Exit Sub
        End If

        If Trim(cmbPeriodEnd.Text) = "Current Business Date" Then
            dtEnd.Text = CDate(Session("SysDate"))
        End If

        Call DoForLiabilityDeals()
        Call DoForAssetDeals()
        Call DoUnclassifiedDeals()

        'Prepare the dealing profit report
        Call dealingProfitPrep()

        dtStart.Enabled = False
        dtEnd.Enabled = False
        cmbCurrency.Enabled = False

        LblLiveA.Enabled = True
        lblMaturedA.Enabled = True
        lblLiveL.Enabled = True
        lblMaturedL.Enabled = True

        Try
            strSQL = "SELECT dt,dealingprofit, accrualAsset, accrualLiability, (accrualAsset+accrualLiability) AS DailyProfit FROM ACCRUALDEALINGPROFIT WHERE usr = '" & Trim(Session("username")) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            ' GridDeals.DataSource = drSQL
            ' GridDeals.DataBind()

            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "LinkDealingProfit_LinkClicked", "error")
            '************************END****************************************
        End Try

        btnPrintDailyDealingProfitExtract.Enabled = True



    End Sub
    Protected Sub btnYeartoDateDealingProfit_Click(sender As Object, e As EventArgs) Handles btnYeartoDateDealingProfit.Click

        If cmbCurrency.Text = "" Then
            MsgBox("Select a currency", MsgBoxStyle.Information)
            Exit Sub
        End If

        If cmbPeriodEnd.Text = "" Then
            MsgBox("Select the Run To option ", MsgBoxStyle.Information)
            Exit Sub
        End If

        dtStart.Text = "1/1/" & DatePart(DateInterval.Year, CDate(Session("SysDate")))

        If Trim(cmbPeriodEnd.Text) = "End Of Selected Period" Then
            dtEnd.Text = "12/31/" & DatePart(DateInterval.Year, CDate(Session("SysDate")))
        Else
            dtEnd.Text = CDate(Session("SysDate"))
        End If

        Call DoForLiabilityDeals()
        Call DoForAssetDeals()
        Call DoUnclassifiedDeals()

        'Prepare the dealing profit report
        Call dealingProfitPrep()


        Try
            strSQL = "SELECT month(dt),datename(month,dt) as dt,sum(dealingprofit) as dealingprofit,sum(accrualAsset)+ sum(accrualLiability) as netAccrual , " & _
                      " sum((dealingprofit+accrualAsset+accrualLiability)) AS TotalProfit FROM ACCRUALDEALINGPROFIT WHERE usr = '" & Trim(Session("username")) & "'" & _
                      " group by datename(month,dt), month(dt)"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            '  GridGap.DataSource = drSQL
            ' GridGap.DataBind()

            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "LinkYearToDate_Link", "error")
            '************************END****************************************
        End Try

        ' grpDetails.Visible = True
        dtStart.Enabled = False
        dtEnd.Enabled = False
        btnPrintYeartoDateDealingProfitExtract.Enabled = True

    End Sub
    Private Sub dealingProfitPrep()
        'Clear work files first
        Call ClearFiles()

        'Update the work tables with all the specific dates to analyse
        Call UpdateWorkTableWithDays("Asset", "Liability")
    End Sub
    'Update the work tables with all the specific dates to analyse
    Private Sub UpdateWorkTableWithDays(ByVal typeofdealA As String, ByVal typeofdealL As String)
        Dim x, y, p As Integer
        Dim startDate As Date = DateAdd(DateInterval.Day, -1, CDate(dtStart.Text))
        Dim assetsTotals As Decimal() = {0, 0}
        Dim assetsTotalsTemp As Decimal() = {0, 0}
        Dim liabilityTotals As Decimal() = {0, 0}
        Dim liabilityTotalsTemp As Decimal() = {0, 0}
        Dim DealingProfitVal As Decimal = 0
        Dim start As Date
        Dim startTemp As Date
        Dim currencyTemp As String() = Split(AllCurrencies, ",")
        y = currencyTemp.Length
        Dim Holi, NonBusi As Boolean

        'start = Format(dtStart.Text.ToString, "MM/dd/yyyy")
        'startTemp = Format(dtStart.Text.ToString, "MM/dd/yyyy")
        start = dtStart.Text
        startTemp = dtStart.Text.ToString
        For x = 1 To DateDiff(DateInterval.Day, start, CDate(dtEnd.Text)) + 1

            'If x > 1 Then 'skip for first day

            'Check if  date is not a non-business day
            NonBusi = mmdeal_object.NonBusinessDay(startTemp)
            'Check if  date is not a holiday day
            Holi = mmdeal_object.Holidays(startTemp, getBaseCurrency)

            If NonBusi = True Or Holi = True Then
                start = start
            End If


            If NonBusi = False And Holi = False Then

                assetsTotals(0) = 0
                assetsTotals(1) = 0

                liabilityTotals(0) = 0
                liabilityTotals(1) = 0

                If cmbCurrency.Text = "Consolidated" Then

                    For p = 1 To y - 1

                        assetsTotalsTemp = GetTotalForAccruals(GetNextCalandarDay(startDate), typeofdealA, 0, 0, currencyTemp(p))
                        assetsTotals(0) = assetsTotals(0) + assetsTotalsTemp(0)
                        assetsTotals(1) = assetsTotals(1) + assetsTotalsTemp(1)

                        liabilityTotalsTemp = GetTotalForAccruals(GetNextCalandarDay(startDate), typeofdealL, 0, 0, currencyTemp(p))
                        liabilityTotals(0) = liabilityTotals(0) + liabilityTotalsTemp(0)
                        liabilityTotals(1) = liabilityTotals(1) + liabilityTotalsTemp(1)

                        DealingProfitVal = GetDealingProfitvalue(GetNextCalandarDay(startDate))

                    Next

                Else

                    assetsTotals = GetTotalForAccruals(GetNextCalandarDay(startDate), typeofdealA, 0, 0, Trim(cmbCurrency.Text))
                    liabilityTotals = GetTotalForAccruals(GetNextCalandarDay(startDate), typeofdealL, 0, 0, Trim(cmbCurrency.Text))
                    DealingProfitVal = GetDealingProfitValue(GetNextCalandarDay(startDate))

                End If

                If liabilityTotals(0) <> 0 Then
                    liabilityTotals(0) = liabilityTotals(0) * -1
                End If

            End If


            Try
                strSQL = "insert ACCRUALDEALINGPROFIT(dt,accrualasset,numDealsAsset,AccrualLiability,numDealsLiability,usr,dealingprofit) values" & _
                         "('" & GetNextCalandarDay(startDate) & "'," & Math.Round(assetsTotals(0), 2) & " ,'" & assetsTotals(1) & " '," & _
                          Math.Round(liabilityTotals(0), 2) & "," & liabilityTotals(1) & ",'" & Trim(Session("username")) & "'," & DealingProfitVal & ")"


                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()

                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()

                startDate = DateAdd(DateInterval.Day, 1, startDate)
                startTemp = DateAdd(DateInterval.Day, 1, startTemp)


            Catch ex As Exception
                'Log the event *****************************************************
                object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "LinkYeaUpdateWorkTableWithDaysrToDate_Link", "error")
                '************************END****************************************

            End Try

        Next x
    End Sub
    Private Function OutrightStatus(ByVal maturityDate As Date, ByVal dealref As String) As Boolean
        Dim status As Boolean

        Dim cnSQLd As SqlConnection
        Dim cmSQLd As SqlCommand
        Dim drSQLd As SqlDataReader
        Dim strSQLd As String

        Try
            strSQLd = "select maturitydate from alldeals where dealreference='" & Trim(dealref) & "'"

            cnSQLd = New SqlConnection(Session("ConnectionString"))
            cnSQLd.Open()
            cmSQLd = New SqlCommand(strSQLd, cnSQLd)
            drSQLd = cmSQLd.ExecuteReader()

            'if there are records we need to determine if the deal was an outright sell
            Do While drSQLd.Read
                If Format(maturityDate, "shortdate") = Format(CDate(drSQLd.Item("maturitydate")), "shortdate") Then
                    status = True
                Else
                    status = False
                End If
            Loop

            ' Close and Clean up objects
            drSQLd.Close()
            cnSQLd.Close()
            cmSQLd.Dispose()
            cnSQLd.Dispose()


            Return status

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetDealingProfitValue", "error")
            '************************END****************************************

        End Try
    End Function
    'Will return the dealing profit for the day for outright sells only
    Private Function GetDealingProfitValue(ByVal dt As Date)
        Dim Val As Decimal = 0

        Try
            strSQL = "select * from alldeals where alldeals.othercharacteristics='Discount Sale'" & _
                     " and dateentered= '" & Format(dt, "yyyy-MM-dd") & "' and currency='" & Trim(cmbCurrency.Text) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            'if there are records we need to determine if the deal was an outright sell
            Do While drSQL.Read

                If OutrightStatus(CDate(drSQL.Item("maturitydate")), Trim(drSQL.Item("purchaseref"))) = True Then
                    'if the deal is an outright sell get the dealing profit figure profit
                    Val = Val + ProfitVal(Trim(drSQL.Item("dealreference")))
                End If

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


            Return Val

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetDealingProfitValue", "error")
            '************************END****************************************


        End Try
    End Function
    Private Function ProfitVal(ByVal sellRef As String) As Decimal
        Dim profitV As Decimal = 0

        Dim cnSQLp As SqlConnection
        Dim cmSQLp As SqlCommand
        Dim drSQLp As SqlDataReader
        Dim strSQLp As String

        Try
            strSQLp = "select profit from selldetail where refsell='" & Trim(sellRef) & "'"

            cnSQLp = New SqlConnection(Session("ConnectionString"))
            cnSQLp.Open()
            cmSQLp = New SqlCommand(strSQLp, cnSQLp)
            drSQLp = cmSQLp.ExecuteReader()

            'if there are records we need to determine if the deal was an outright sell
            Do While drSQLp.Read
                profitV = CDec(drSQLp.Item("profit"))
            Loop

            ' Close and Clean up objects
            drSQLp.Close()
            cnSQLp.Close()
            cmSQLp.Dispose()
            cnSQLp.Dispose()

            Return profitV

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetDealingProfitValue", "error")
            '************************END****************************************
        End Try

    End Function
    Private Function GetNextCalandarDay(ByVal currentDay As Date) As Date
        Dim NextDay As Date

        NextDay = DateAdd(DateInterval.Day, 1, currentDay)

        Return NextDay
    End Function
    Private Function GetTotalForAccruals(ByVal dt As Date, ByVal typeofdeal As String, ByVal PrevTotal1 As Decimal, ByVal Prevtotal2 As Decimal, ByVal curr As String) As Decimal()
        Dim Totals As Decimal() = {0, 0}
        Dim exchangerate As Decimal = 0

        Try
            If Format(dt, "short date") = CDate(Session("sysDate")) Then
                strSQL = "select sum((deals_live.dealamount*deals_live.interestrate)/(deals_live.intdaysbasis*100)),count(dealreference),currency,exchange,reciprocal from Deals_Live" & _
                           " join exchange_rates on exchange_rates.currencycode=Deals_Live.currency " & _
                           " where Deals_Live.dealtype" & _
                           " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                           " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                           " ACCRUALANALYSISGRP.type='" & typeofdeal & "') and " & _
                           " startdate<= '" & Format(dt, "short date") & "' and " & _
                           " maturitydate>='" & Format(dt, "short date") & "' and currency='" & Trim(curr) & "' group by currency,exchange,reciprocal"

            Else
                strSQL = "select sum(intaccruedtodate/(tenor-daystomaturity)),count(dealreference),currency,exchange,reciprocal from alldeals" & _
                " join exchange_rates on exchange_rates.currencycode=alldeals.currency " & _
                " where alldeals.dealtype" & _
                " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                " ACCRUALANALYSISGRP.type='" & typeofdeal & "') and " & _
                " startdate<= '" & Format(dt, "short date") & "' and " & _
                " maturitydate>='" & Format(dt, "short date") & "' and currency='" & Trim(curr) & "' group by currency,exchange,reciprocal"

            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read

                exchangerate = drSQL.Item("exchange")

                If drSQL.Item("reciprocal").ToString = "Y" Then
                    exchangerate = 1 / exchangerate
                End If


                If drSQL.Item(0) Is DBNull.Value Then
                    Totals(0) = (0 + PrevTotal1) / exchangerate
                Else
                    Totals(0) = (drSQL.Item(0) + PrevTotal1) / exchangerate
                End If
                If drSQL.Item(1) Is DBNull.Value Then
                    Totals(1) = (0 + Prevtotal2)
                Else
                    Totals(1) = (drSQL.Item(1) + Prevtotal2)
                End If
            Loop

            'If typeofdeal = "Liability" Then
            '    If Totals(0) <> 0 Then
            '        Totals(0) = Totals(0) * -1
            '    End If
            'End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "GetTotalForAccruals", "error")
            '************************END****************************************

            Exit Function
        End Try

        Return Totals

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
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "CheckBaseCurrency", "error")
            '************************END****************************************


        End Try
    End Function
    Private Sub ClearFiles()
        Try
            strSQL = " begin tran X" & _
                     " delete ACCRUALDEALINGPROFIT where usr='" & Trim(Session("username")) & "'" & _
                     " commit tran X"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************


        End Try
    End Sub
    Private Sub DoForLiabilityDeals()
        Try
            Dim Totallive As Integer = 0
            Dim TotalMatured As Integer = 0
            Dim curr As String

            If cmbCurrency.Text = "Consolidated" Then
                curr = "'" & Mid(AllCurrencies, 2, Len(AllCurrencies))
                curr = Replace(curr, ",", "','")
                curr = curr & "'"
            Else
                curr = "'" & Trim(cmbCurrency.Text) & "'"
            End If


            'Step 1
            'lets look for the deals that are running for the selected period
            'First for Liabilities as defined in Accruals analysis groups by the user

            'Live deals
            strSQL = "select count(dealreference) from alldeals where alldeals.dealtype" & _
                      " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                      " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                      " ACCRUALANALYSISGRP.type='Liability') and " & _
                      " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                      " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                      " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                      " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                      " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')" & _
                      " ) and daystomaturity>0 and currency in (" & curr & ")"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read()
                Totallive = drSQL.Item(0)

            Loop


            strSQL = "select count(dealreference) from alldeals where alldeals.dealtype" & _
                     " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                     " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                     " ACCRUALANALYSISGRP.type='Liability') and " & _
                     " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                     " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                     " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                     " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                     " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')" & _
                     " ) and daystomaturity=0 and currency in(" & Trim(curr) & ")"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read()
                TotalMatured = drSQL.Item(0)

            Loop



            lblLiveL.Text = Totallive
            lblMaturedL.Text = TotalMatured
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()



        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoforLiabilitydeals", "error")
            '************************END****************************************


        End Try

    End Sub
    'count unclasified deals
    Private Sub DoUnclassifiedDeals()
        Try

            Dim Total As Integer = -1
            Dim curr As String

            If cmbCurrency.Text = "Consolidated" Then
                curr = "'" & Mid(AllCurrencies, 2, Len(AllCurrencies))
                curr = Replace(curr, ",", "','")
                curr = curr & "'"
            Else
                curr = "'" & Trim(cmbCurrency.Text) & "'"
            End If


            ' Deals
            strSQL = " select count(dealreference) from alldeals where dealtype" & _
                       " not in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                       " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where ACCRUALANALYSISGRP.type='Liability' or ACCRUALANALYSISGRP.type='Asset') and " & _
                       " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                       " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                       " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                       " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                       " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')) and currency in(" & curr & ")"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read()
                'count number of live and matured deals
                Total = drSQL.Item(0)

            Loop

            lblUnclassified.Text = Total

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************


        End Try

    End Sub

  
    Protected Sub LblLiveA_Click(sender As Object, e As EventArgs) Handles LblLiveA.Click
        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        If LblLiveA.Text <> "" Then
            'grpDetails.Visible = True
            grpDetails.Text = "Detailed View Live Deals [Assets]"

            Dim curr As String

            If cmbCurrency.Text = "Consolidated" Then
                curr = "'" & Mid(AllCurrencies, 2, Len(AllCurrencies))
                curr = Replace(curr, ",", "','")
                curr = curr & "'"
            Else
                curr = "'" & Trim(cmbCurrency.Text) & "'"
            End If

            strSQLx = " select fullname,dealreference,intaccruedtodate/(tenor-daystomaturity) as accrual," & _
                          " intaccruedtodate,dealamount,interestrate,discountrate,maturityamount,tenor,daystomaturity" & _
                          " from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                          " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                          " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                          " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                          " ACCRUALANALYSISGRP.type='Asset') and " & _
                          " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                          " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                          " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')" & _
                          " ) and daystomaturity>0 and alldeals.currency in(" & Trim(curr) & ")"



            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            GridDeals.DataSource = drSQLx
            GridDeals.DataBind()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()
        End If
    End Sub

    Protected Sub lblMaturedA_Click(sender As Object, e As EventArgs) Handles lblMaturedA.Click
        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        If lblMaturedA.Text <> "" Then
            ' grpDetails.Visible = True
            grpDetails.Text = "Detailed View Live Deals [Liabilities]"

            Dim curr As String

            If cmbCurrency.Text = "Consolidated" Then
                curr = "'" & Mid(AllCurrencies, 2, Len(AllCurrencies))
                curr = Replace(curr, ",", "','")
                curr = curr & "'"
            Else
                curr = "'" & Trim(cmbCurrency.Text) & "'"
            End If

            strSQLx = "select fullname,dealreference,intaccruedtodate/(tenor-daystomaturity) as accrual," & _
                          " intaccruedtodate,dealamount,interestrate,discountrate,maturityamount,tenor,daystomaturity" & _
                          " from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                          " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                          " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                          " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                          " ACCRUALANALYSISGRP.type='liability') and " & _
                          " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                          " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                          " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')" & _
                          " ) and daystomaturity>0 and alldeals.currency in(" & Trim(curr) & ")"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            GridDeals.DataSource = drSQLx
            GridDeals.DataBind()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()
        End If
    End Sub
    Protected Sub lblLiveL_Click(sender As Object, e As EventArgs) Handles lblLiveL.Click
        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        If lblMaturedA.Text <> "" Then
            ' grpDetails.Visible = True
            grpDetails.Text = "Detailed View Matured Deals [Liabilities]"""

            Dim curr As String

            If cmbCurrency.Text = "Consolidated" Then
                curr = "'" & Mid(AllCurrencies, 2, Len(AllCurrencies))
                curr = Replace(curr, ",", "','")
                curr = curr & "'"
            Else
                curr = "'" & Trim(cmbCurrency.Text) & "'"
            End If

            strSQLx = "  select fullname,dealreference,intaccruedtodate/(tenor-daystomaturity) as accrual," & _
                          " intaccruedtodate,dealamount,interestrate,discountrate,maturityamount,tenor,daystomaturity" & _
                          " from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                          " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                          " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                          " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                          " ACCRUALANALYSISGRP.type='liability') and " & _
                          " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                          " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                          " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')" & _
                          " ) and daystomaturity=0 and alldeals.currency in(" & Trim(curr) & ")"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            GridDeals.DataSource = drSQLx
            GridDeals.DataBind()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()
        End If
    End Sub
    Protected Sub lblMaturedL_Click(sender As Object, e As EventArgs) Handles lblMaturedL.Click
        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        If lblMaturedA.Text <> "" Then
            ' grpDetails.Visible = True
            grpDetails.Text = "Detailed View Matured Deals [Liabilities]"

            Dim curr As String

            If cmbCurrency.Text = "Consolidated" Then
                curr = "'" & Mid(AllCurrencies, 2, Len(AllCurrencies))
                curr = Replace(curr, ",", "','")
                curr = curr & "'"
            Else
                curr = "'" & Trim(cmbCurrency.Text) & "'"
            End If

            strSQLx = "  select fullname,dealreference,intaccruedtodate/(tenor-daystomaturity) as accrual," & _
                          " intaccruedtodate,dealamount,interestrate,discountrate,maturityamount,tenor,daystomaturity" & _
                          " from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                          " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                          " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                          " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                          " ACCRUALANALYSISGRP.type='liability') and " & _
                          " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                          " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                          " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')" & _
                          " ) and daystomaturity=0 and alldeals.currency in(" & Trim(curr) & ")"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            GridDeals.DataSource = drSQLx
            GridDeals.DataBind()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()
        End If
    End Sub

    Protected Sub lblUnclassified_Click(sender As Object, e As EventArgs) Handles lblUnclassified.Click
        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        If lblMaturedA.Text <> "" Then
            ' grpDetails.Visible = True
            grpDetails.Text = "Detailed View Un - Classified Deals"

            Dim curr As String

            If cmbCurrency.Text = "Consolidated" Then
                curr = "'" & Mid(AllCurrencies, 2, Len(AllCurrencies))
                curr = Replace(curr, ",", "','")
                curr = curr & "'"
            Else
                curr = "'" & Trim(cmbCurrency.Text) & "'"
            End If

            strSQLx = " select fullname,dealreference,intaccruedtodate/(tenor-daystomaturity) as accrual," & _
                          " intaccruedtodate,dealamount,interestrate,discountrate,maturityamount,tenor,daystomaturity" & _
                          ",interestrate,discountrate " & _
                          " from alldeals join customer on alldeals.customernumber=customer.customer_number " & _
                          " where dealtype" & _
                          " not in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                          " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where ACCRUALANALYSISGRP.type='Liability' or ACCRUALANALYSISGRP.type='Asset') and " & _
                          " ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') or " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')" & _
                          " or ((startdate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "') and " & _
                          " (maturitydate between '" & Format(dtStart.Text, "short date") & "' and '" & Format(dtEnd.Text, "short date") & "')) or " & _
                          " (startdate < '" & Format(dtStart.Text, "short date") & "' and maturitydate > '" & Format(dtEnd.Text, "short date") & "')) and alldeals.currency in(" & Trim(curr) & ")"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            GridDeals.DataSource = drSQLx
            GridDeals.DataBind()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()
        End If
    End Sub


    Protected Sub btnPrintDailyDealingProfitExtract_Click(sender As Object, e As EventArgs) Handles btnPrintDailyDealingProfitExtract.Click
        Response.Redirect("ReportViewer.aspx?report=Daily Dealing Profit?currency=" & cmbCurrency.SelectedValue.ToString)

    End Sub

    Protected Sub btnPrintYeartoDateDealingProfitExtract_Click(sender As Object, e As EventArgs) Handles btnPrintYeartoDateDealingProfitExtract.Click
        Response.Redirect("ReportViewer.aspx?report=Dealing Profit-Year to date?currency=" & cmbCurrency.SelectedValue.ToString)

    End Sub

   
   
End Class
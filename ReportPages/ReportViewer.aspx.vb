Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine.Database
Imports System.Data.SqlClient

Public Class ReportViewer
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader

    Private object_userlog As New usrlog.usrlog
    Public CustomerSel As String
    Public custRangeStart, custRangeEnd As String
    Public dealCodesSel As String
    Public Dealersel As String
    Public Date1 As String
    Public date2 As String
    Public SelectedRef As String
    Public TimeYrs As Double
    Public SettlementTyp As String
    Public ViewType As String
    Public PosRef As String
    Public Status1 As String
    Public status2 As String
    Public user_passed As String
    Public DaysTM As Integer
    Private Sub ReportViewer_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                Dim crtableLogoninfos As New TableLogOnInfos()
                Dim crtableLogoninfo As New TableLogOnInfo()
                Dim crConnectionInfo As New ConnectionInfo()
                Dim CrTables As CrystalDecisions.CrystalReports.Engine.Tables
                Dim CrTable As Table
                Dim TableCounter

                Dim CR As New ReportDocument
                'Try

                With crConnectionInfo
                    .ServerName = "tresyscon"
                    'If you are connecting to Oracle there is no DatabaseName. Use an empty string. 
                    'For example, .DatabaseName = ""
                    .DatabaseName = Session("dataBaseName")
                    .UserID = "tresyscon"
                    .Password = ""
                End With
                CR.Load(Server.MapPath("../Reports/" & Request.QueryString("report") & ".rpt"))
                CrTables = CR.Database.Tables

                'MsgBox(Request.QueryString("report"))
                'Loop through each table in the report and apply the LogonInfo information
                For Each CrTable In CrTables
                    crtableLogoninfo = CrTable.LogOnInfo
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo
                    CrTable.ApplyLogOnInfo(crtableLogoninfo)

                    'If your DatabaseName is changing at runtime, specify the table location.
                    'For example, when you are reporting off of a Northwind database on SQL server you should have the following line of code:
                    ' CrTable.Location = My.Settings.db & ".dbo" & CrTable.Location.Substring(CrTable.Location.LastIndexOf("."))
                Next

                For Each subreport As ReportDocument In CR.Subreports
                    For Each CrTable2 As CrystalDecisions.CrystalReports.Engine.Table In subreport.Database.Tables
                        crtableLogoninfo = CrTable2.LogOnInfo
                        crtableLogoninfo.ConnectionInfo = crConnectionInfo
                        CrTable2.ApplyLogOnInfo(crtableLogoninfo)
                    Next
                Next

                'crystalReportViewer1.ExportReport() ;
                ' CrystalReportViewer1.RefreshReport()
                ' CR.PrintOptions.PrinterName = GetDefaultPrinter()
                ' CR.PrintToPrinter(1, False, 0, 0)

                If Request.QueryString("report") = "Maturities DepositsPlacements" Or Request.QueryString("report") = "Maturities Purchases" Or Request.QueryString("report") = "Maturities Sells" Then
                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterValues As ParameterValues
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation3 = crParameterFieldDefinitions.Item("Currency")
                    crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue3.Value = Trim(Request.QueryString("currency"))
                    crParameterValues3.AddValue(crParameterDiscreteValue3.Value)
                    crParameterFieldLocation3.ApplyCurrentValues(crParameterValues3)


                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Days To Mature")
                    crParameterValues = crParameterFieldLocation.CurrentValues
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue.Value = Trim(Request.QueryString("days"))
                    crParameterValues.Add(crParameterDiscreteValue)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)

                ElseIf Request.QueryString("report") = "Gap1" Then
                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterValues As ParameterValues



                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Logged User")
                    crParameterValues = crParameterFieldLocation.CurrentValues
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue.Value = Trim(Session("username"))
                    crParameterValues.Add(crParameterDiscreteValue)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)


                ElseIf Request.QueryString("report") = "Cashflow" Then
                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterValues As ParameterValues


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterFieldLocation = crParameterFieldDefinitions.Item("logged")
                    crParameterValues = crParameterFieldLocation.CurrentValues
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue.Value = Trim(Session("username"))
                    crParameterValues.Add(crParameterDiscreteValue)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)



                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "Cashflowanalysys2" Then
                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterValues As ParameterValues


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterFieldLocation = crParameterFieldDefinitions.Item("logged")
                    crParameterValues = crParameterFieldLocation.CurrentValues
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue.Value = Trim(Session("username"))
                    crParameterValues.Add(crParameterDiscreteValue)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "Top Deposits" Then
                    Dim crParameterDiscreteValue1 As ParameterDiscreteValue

                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation1 As ParameterFieldDefinition
                    Dim crParameterValues1 As ParameterValues
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterValues2 As ParameterValues



                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterFieldLocation1 = crParameterFieldDefinitions.Item("logged")
                    crParameterValues1 = crParameterFieldLocation1.CurrentValues
                    crParameterDiscreteValue1 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue1.Value = Trim(Session("username"))
                    crParameterValues1.Add(crParameterDiscreteValue1)
                    crParameterFieldLocation1.ApplyCurrentValues(crParameterValues1)

                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2.Value = Trim(Request.QueryString("currency"))
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("currency")
                    crParameterValues2 = crParameterFieldLocation2.CurrentValues
                    crParameterValues2.Add(crParameterDiscreteValue2)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)

                ElseIf Request.QueryString("report") = "Accruals Analysis" Then
              

                    Dim crParameterDiscreteValue1 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue4 As ParameterDiscreteValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation1 As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterFieldLocation4 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues

                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields


                    crParameterFieldLocation1 = crParameterFieldDefinitions.Item("Logged User")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("Currency")
                    crParameterFieldLocation4 = crParameterFieldDefinitions.Item("ReportDate")

                    If Request.QueryString("report") = "Accruals Analysis" Then
                        crParameterFieldLocation3 = crParameterFieldDefinitions.Item("status")
                    End If

                    crParameterDiscreteValue1 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue4 = New CrystalDecisions.Shared.ParameterDiscreteValue

                    'If Request.QueryString("report") = "Accruals Analysis" Then
                    '    crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'End If

                    crParameterDiscreteValue1.Value = Trim(Session("username"))
                    crParameterDiscreteValue2.Value = Request.QueryString("currency")
                    crParameterDiscreteValue4.Value = Request.QueryString("date")

                    'If Request.QueryString("report") = "Accruals Analysis" Then
                    'crParameterDiscreteValue3.Value = Request.QueryString("status")
                    '  End If
                    crParameterValues.Add(crParameterDiscreteValue1)
                    crParameterFieldLocation1.ApplyCurrentValues(crParameterValues)
                    crParameterValues.Add(crParameterDiscreteValue2)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues)
                    crParameterValues.Add(crParameterDiscreteValue4)
                    crParameterFieldLocation4.ApplyCurrentValues(crParameterValues)

                    'If Request.QueryString("report") = "Accruals Analysis" Then
                    '    crParameterValues.Add(crParameterDiscreteValue3)
                    '    crParameterFieldLocation3.ApplyCurrentValues(crParameterValues)
                    'End If

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)


                ElseIf Request.QueryString("report") = "Top Depositors" Then
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterValues2 As ParameterValues
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    Dim FormatedDate1 As String
                    FormatedDate1 = DatePart(DateInterval.Month, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Day, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Year, CDate(Request.QueryString("Date1")))

                    crParameterFieldLocation3 = crParameterFieldDefinitions.Item("Deal Date")
                    crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue3.Value = Trim(FormatedDate1)
                    crParameterValues3.AddValue(crParameterDiscreteValue3.Value)
                    crParameterFieldLocation3.ApplyCurrentValues(crParameterValues3)

                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2.Value = Trim(Request.QueryString("currency"))
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("currency")
                    crParameterValues2 = crParameterFieldLocation2.CurrentValues
                    crParameterValues2.Add(crParameterDiscreteValue2)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "Top Depositors Live" Then
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterValues2 As ParameterValues
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues3 As New ParameterValues()



                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2.Value = Trim(Request.QueryString("currency"))
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("currency")
                    crParameterValues2 = crParameterFieldLocation2.CurrentValues
                    crParameterValues2.Add(crParameterDiscreteValue2)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)
                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)


                ElseIf Request.QueryString("report") = "Daily Dealing Profit" Or Request.QueryString("report") = "Dealing Profit-Year to date" Then
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterValues2 As ParameterValues



                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2.Value = Trim(Request.QueryString("currency"))
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("currency")
                    crParameterValues2 = crParameterFieldLocation2.CurrentValues
                    crParameterValues2.Add(crParameterDiscreteValue2)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)

                ElseIf Request.QueryString("report") = "Top Loans" Then
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterValues2 As ParameterValues



                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2.Value = Trim(Request.QueryString("currency"))
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("currency")
                    crParameterValues2 = crParameterFieldLocation2.CurrentValues
                    crParameterValues2.Add(crParameterDiscreteValue2)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Income Expense Return" Or Request.QueryString("report") = "commission return" Or Request.QueryString("report") = "tax return" Or Request.QueryString("report") = "Income Expense Analysis" Or Request.QueryString("report") = "Maturities DepositsPlacements" Or Request.QueryString("report") = "Maturities Purchasess" Or Request.QueryString("report") = "Maturities Sells" Then
                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterValues As ParameterValues



                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Logged User")
                    crParameterValues = crParameterFieldLocation.CurrentValues
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue.Value = Trim(Session("username"))
                    crParameterValues.Add(crParameterDiscreteValue)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Live Deals Customer" Then

                    Dim paramV As String()
                    Dim x, y As Integer
                    paramV = Split(CustomerSel, ",")
                    x = paramV.Length

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()

                    Dim paramField As New ParameterField


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Customer Number")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue

                    'Discreet values
                    For y = 0 To x - 1
                        crParameterDiscreteValue.Value = Trim(paramV(y))
                        crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    Next

                    'range values                
                    crParameterValues.AddRange(custRangeStart, custRangeEnd, RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)

                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)


                ElseIf Request.QueryString("report") = "Live Deals Customer2" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer
                    paramV = Split(CustomerSel, ",")
                    x = paramV.Length

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue1 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation1 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues1 As New ParameterValues()

                    Dim paramField As New ParameterField



                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Customer Number")
                    crParameterFieldLocation1 = crParameterFieldDefinitions.Item("Deal Types")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue1 = New CrystalDecisions.Shared.ParameterDiscreteValue

                    'Discreet values
                    For y = 0 To x - 1
                        crParameterDiscreteValue.Value = Trim(paramV(y))
                        crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    Next

                    'range values                
                    crParameterValues.AddRange(custRangeStart, custRangeEnd, RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)

                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    'Discreet values
                    ParamX = Split(dealCodesSel, ",")
                    x = ParamX.Length

                    For y = 0 To x - 1
                        crParameterDiscreteValue1.Value = Trim(ParamX(y))
                        crParameterValues1.AddValue(crParameterDiscreteValue1.Value)
                    Next

                    crParameterFieldLocation1.ApplyCurrentValues(crParameterValues1)
                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "Live Deals specific Deal type by Date" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer
                    paramV = Split(dealCodesSel, ",")
                    x = paramV.Length

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Deal Type")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue

                    'Discreet values
                    For y = 0 To x - 1
                        crParameterDiscreteValue.Value = Trim(paramV(y))
                        crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    Next

                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "All deals by Customer" Then
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterValues2 As ParameterValues

                    Dim crParameterFieldLocation4 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue4 As ParameterDiscreteValue
                    Dim crParameterValues4 As ParameterValues

                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterValues3 As ParameterValues
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterValues As ParameterValues

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer
                    paramV = Split(dealCodesSel, ",")
                    x = paramV.Length


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2.Value = Trim(Request.QueryString("Date1"))
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("StartDate")
                    crParameterValues2 = crParameterFieldLocation2.CurrentValues
                    crParameterValues2.Add(crParameterDiscreteValue2)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterFieldLocation4 = crParameterFieldDefinitions.Item("status")
                    crParameterDiscreteValue4 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterValues4 = crParameterFieldLocation4.CurrentValues

                    crParameterDiscreteValue4.Value = Trim(Request.QueryString("Status1"))
                    crParameterValues4.AddValue(crParameterDiscreteValue4.Value)

                    crParameterDiscreteValue4.Value = Trim(Request.QueryString("status2"))
                    crParameterValues4.AddValue(crParameterDiscreteValue4.Value)

                    crParameterFieldLocation4.ApplyCurrentValues(crParameterValues4)

                    crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue3.Value = Trim(Request.QueryString("Date2"))
                    crParameterFieldLocation3 = crParameterFieldDefinitions.Item("EndDate")
                    crParameterValues3 = crParameterFieldLocation3.CurrentValues
                    crParameterValues3.Add(crParameterDiscreteValue3)
                    crParameterFieldLocation3.ApplyCurrentValues(crParameterValues3)

                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Deal Types")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterValues = crParameterFieldLocation.CurrentValues
                    'Discreet values
                    For y = 0 To x - 1
                        crParameterDiscreteValue.Value = Trim(paramV(y))
                        crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    Next

                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)



                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "All deals by Customer by Product" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer
                    paramV = Split(dealCodesSel, ",")
                    x = paramV.Length

                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterValues2 As ParameterValues

                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterValues3 As ParameterValues


                    Dim crParameterFieldLocation4 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue4 As ParameterDiscreteValue
                    Dim crParameterValues4 As ParameterValues

                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterValues As ParameterValues



                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2.Value = Trim(Request.QueryString("Date1"))
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("StartDate")
                    crParameterValues2 = crParameterFieldLocation2.CurrentValues
                    crParameterValues2.Add(crParameterDiscreteValue2)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue3.Value = Trim(Request.QueryString("date2"))
                    crParameterFieldLocation3 = crParameterFieldDefinitions.Item("EndDate")
                    crParameterValues3 = crParameterFieldLocation3.CurrentValues
                    crParameterValues3.Add(crParameterDiscreteValue3)
                    crParameterFieldLocation3.ApplyCurrentValues(crParameterValues3)


                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Deal Types")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterValues = crParameterFieldLocation.CurrentValues
                    'Discreet values
                    For y = 0 To x - 1
                        crParameterDiscreteValue.Value = Trim(paramV(y))
                        crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    Next

                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    crParameterFieldLocation4 = crParameterFieldDefinitions.Item("dealstatus")
                    crParameterDiscreteValue4 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterValues4 = crParameterFieldLocation4.CurrentValues

                    crParameterDiscreteValue4.Value = Trim(Request.QueryString("Status1"))
                    crParameterValues4.AddValue(crParameterDiscreteValue4.Value)

                    crParameterDiscreteValue4.Value = Trim(Request.QueryString("status2"))
                    crParameterValues4.AddValue(crParameterDiscreteValue4.Value)


                    crParameterFieldLocation4.ApplyCurrentValues(crParameterValues4)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Live Deals Summary Deal Types By Date" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer


                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation1 As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues1 As New ParameterValues()
                    Dim crParameterDiscreteValue1 As ParameterDiscreteValue
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Start Date")
                    crParameterFieldLocation1 = crParameterFieldDefinitions.Item("Deal Type")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("dt1")
                    crParameterFieldLocation3 = crParameterFieldDefinitions.Item("dt2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue1 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue


                    'Discreet values
                    ParamX = Split(dealCodesSel, ",")
                    x = ParamX.Length

                    For y = 0 To x - 1
                        crParameterDiscreteValue1.Value = Trim(ParamX(y))
                        crParameterValues1.AddValue(crParameterDiscreteValue1.Value)
                    Next

                    crParameterFieldLocation1.ApplyCurrentValues(crParameterValues1)

                    Dim FormatedDate1, FormatedDate2 As String
                    FormatedDate1 = DatePart(DateInterval.Month, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Day, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Year, CDate(Request.QueryString("Date1")))
                    FormatedDate2 = DatePart(DateInterval.Month, CDate(Request.QueryString("date2"))) & "/" & DatePart(DateInterval.Day, CDate(Request.QueryString("date2"))) & "/" & DatePart(DateInterval.Year, CDate(Request.QueryString("date2")))
                    'range values   

                    crParameterDiscreteValue2.Value = Format(CDate(Request.QueryString("Date1")), "short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)
                    crParameterDiscreteValue3.Value = Format(CDate(Request.QueryString("date2")), "Short date")
                    crParameterValues3.AddValue(crParameterDiscreteValue3.Value)
                    crParameterFieldLocation3.ApplyCurrentValues(crParameterValues3)

                    crParameterValues.AddRange(FormatedDate1, FormatedDate2, RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "Live Deals Summary Customer By Date" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer


                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterRangeValueCust As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation1 As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues1 As New ParameterValues()
                    Dim crParameterDiscreteValue1 As ParameterDiscreteValue
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()



                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Start Date")
                    crParameterFieldLocation1 = crParameterFieldDefinitions.Item("Customer Number")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("dt1")
                    crParameterFieldLocation3 = crParameterFieldDefinitions.Item("dt2")
                    crParameterRangeValueCust = New CrystalDecisions.Shared.ParameterRangeValue
                    crParameterRangeValue = New CrystalDecisions.Shared.ParameterRangeValue
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue1 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue


                    'Discreet values
                    ParamX = Split(CustomerSel, ",")
                    x = ParamX.Length

                    For y = 0 To x - 1
                        crParameterDiscreteValue1.Value = Trim(ParamX(y))
                        crParameterValues1.AddValue(crParameterDiscreteValue1.Value)
                    Next

                    crParameterValues1.AddRange(custRangeStart, custRangeEnd, RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation1.ApplyCurrentValues(crParameterValues1)


                    Dim FormatedDate1, FormatedDate2 As String
                    FormatedDate1 = DatePart(DateInterval.Month, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Day, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Year, CDate(Request.QueryString("Date1")))
                    FormatedDate2 = DatePart(DateInterval.Month, CDate(Request.QueryString("date2"))) & "/" & DatePart(DateInterval.Day, CDate(Request.QueryString("date2"))) & "/" & DatePart(DateInterval.Year, CDate(Request.QueryString("date2")))
                    'range values                
                    crParameterDiscreteValue2.Value = Format(CDate(Request.QueryString("Date1")), "short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)
                    crParameterDiscreteValue3.Value = Format(CDate(Request.QueryString("date2")), "Short date")
                    crParameterValues3.AddValue(crParameterDiscreteValue3.Value)
                    crParameterFieldLocation3.ApplyCurrentValues(crParameterValues3)

                    crParameterValues.AddRange(FormatedDate1, FormatedDate2, RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)

                ElseIf Request.QueryString("report") = "Live Deals Summary Dealer By Date" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer
                    paramV = Split(dealCodesSel, ",")
                    x = paramV.Length

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation1 As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues1 As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()
                    Dim crParameterDiscreteValue1 As ParameterDiscreteValue


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Start Date")
                    crParameterFieldLocation1 = crParameterFieldDefinitions.Item("Dealer Name")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("dt1")
                    crParameterFieldLocation3 = crParameterFieldDefinitions.Item("dt2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue1 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue


                    'Discreet values
                    ParamX = Split(Dealersel, ",")
                    x = ParamX.Length

                    For y = 0 To x - 1
                        crParameterDiscreteValue1.Value = Trim(ParamX(y))
                        crParameterValues1.AddValue(crParameterDiscreteValue1.Value)
                    Next

                    crParameterFieldLocation1.ApplyCurrentValues(crParameterValues1)

                    'range values
                    crParameterDiscreteValue2.Value = Format(CDate(Date1), "short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)
                    crParameterDiscreteValue3.Value = Format(CDate(date2), "Short date")
                    crParameterValues3.AddValue(crParameterDiscreteValue3.Value)
                    crParameterFieldLocation3.ApplyCurrentValues(crParameterValues3)

                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)





                ElseIf Request.QueryString("report") = "Live Deals by Deal Reference" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer
                    paramV = Split(SelectedRef, ",")
                    x = paramV.Length

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("ref")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue

                    'Discreet values
                    For y = 0 To x - 1
                        crParameterDiscreteValue.Value = Trim(paramV(y))
                        crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    Next

                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "live deals by maturity date" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer
                    paramV = Split(dealCodesSel, ",")
                    x = paramV.Length

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation1 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Maturity Date")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue

                    'range values                
                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)





                ElseIf Request.QueryString("report") = "Fx Forward deals in Period" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date Entered")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    'crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)

                ElseIf Request.QueryString("report") = "Fx Spot Deals in Period" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date Entered")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("EndPeriod")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    'crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)





                ElseIf Request.QueryString("report") = "Buy Sell Transactions by Date" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date Entered")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    'crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Fx Currency Swaps in period" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date Entered")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("EndPeriod")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    'crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)





                ElseIf Request.QueryString("report") = "Fx Interest Rate Swaps in period" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date Entered")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("EndPeriod")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    'crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)





                ElseIf Request.QueryString("report") = "FxTrading Income Rpt" Or Request.QueryString("report") = "FxTrading Income by Position" Or Request.QueryString("report") = "FxTrading Income Rpt1" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    'Dim paramV, ParamX As String()
                    'Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue4 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterFieldLocation4 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()
                    Dim crParameterValues4 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("PeriodStart")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("EndPeriod")
                    crParameterFieldLocation4 = crParameterFieldDefinitions.Item("viewtype")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue4 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    'Discreet values
                    paramV = Split(ViewType, ",")
                    x = paramV.Length

                    For y = 0 To x - 1
                        crParameterDiscreteValue4.Value = Trim(paramV(y))
                        crParameterValues4.AddValue(crParameterDiscreteValue4.Value)
                    Next
                    crParameterFieldLocation4.ApplyCurrentValues(crParameterValues4)
                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)





                ElseIf Request.QueryString("report") = "Fx Closed positions" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer
                    Dim PairNum As Integer

                    'Dim paramV, ParamX As String()
                    'Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue4 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue5 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterFieldLocation4 As ParameterFieldDefinition
                    Dim crParameterFieldLocation5 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()
                    Dim crParameterValues4 As New ParameterValues()
                    Dim crParameterValues5 As New ParameterValues()

                    PairNum = GetPairNum(Request.QueryString("currency"))


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("PeriodStart")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("EndPeriod")
                    crParameterFieldLocation4 = crParameterFieldDefinitions.Item("viewtype")
                    crParameterFieldLocation3 = crParameterFieldDefinitions.Item("PairNum")
                    crParameterFieldLocation5 = crParameterFieldDefinitions.Item("PosRef")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue4 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue5 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    'Discreet values
                    paramV = Split(ViewType, ",")
                    x = paramV.Length

                    For y = 0 To x - 1
                        crParameterDiscreteValue4.Value = Trim(paramV(y))
                        crParameterValues4.AddValue(crParameterDiscreteValue4.Value)
                    Next

                    crParameterDiscreteValue3.Value = PairNum
                    crParameterValues3.AddValue(crParameterDiscreteValue3.Value)
                    crParameterDiscreteValue5.Value = PosRef
                    crParameterValues5.AddValue(crParameterDiscreteValue5.Value)


                    'Apply Values
                    crParameterFieldLocation4.ApplyCurrentValues(crParameterValues4)
                    crParameterFieldLocation3.ApplyCurrentValues(crParameterValues3)
                    crParameterFieldLocation5.ApplyCurrentValues(crParameterValues5)
                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)

                ElseIf Request.QueryString("report") = "FX Open Position by Ccy pair" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer
                    Dim PairNum As Integer

                    'Dim paramV, ParamX As String()
                    'Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue4 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterFieldLocation4 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()
                    Dim crParameterValues4 As New ParameterValues()

                    PairNum = GetPairNum(Request.QueryString("currency"))

                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterFieldLocation = crParameterFieldDefinitions.Item("PairNum")
                    crParameterValues = crParameterFieldLocation.CurrentValues
                    crParameterValues.Clear()
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue.Value = PairNum
                    crParameterValues.Add(crParameterDiscreteValue)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)






                    crParameterFieldLocation4 = crParameterFieldDefinitions.Item("viewType")
                    crParameterValues4 = crParameterFieldLocation.CurrentValues
                    crParameterValues4.Clear()
                    crParameterDiscreteValue4 = New CrystalDecisions.Shared.ParameterDiscreteValue

                    'Discreet values
                    paramV = Split(ViewType, ",")
                    x = paramV.Length

                    For y = 0 To x - 1
                        crParameterDiscreteValue4.Value = Trim(paramV(y))
                        crParameterValues4.AddValue(crParameterDiscreteValue4.Value)
                    Next

                    crParameterFieldLocation4.ApplyCurrentValues(crParameterValues4)

                    'CR.Load(strCurrentDirectory & "\reports\" & REPORTNAME)
                    'crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    ' crParameterFieldLocation4 = crParameterFieldDefinitions.Item("viewType")
                    'crParameterFieldLocation3 = crParameterFieldDefinitions.Item("PairNum")
                    'crParameterDiscreteValue4 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    ' crParameterDiscreteValue3 = New CrystalDecisions.Shared.ParameterDiscreteValue


                    'crParameterDiscreteValue3.Value = PairNum
                    'crParameterValues3.AddValue(crParameterDiscreteValue3.Value)
                    'Apply Values
                    'crParameterFieldLocation4.ApplyCurrentValues(crParameterValues4)
                    'crParameterFieldLocation3.ApplyCurrentValues(crParameterValues3)
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)

                ElseIf Request.QueryString("report") = "FX Statistics" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date Entered")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("EndPeriod")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    'crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "FxTrading Income FWD" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    'Dim paramV, ParamX As String()
                    'Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue4 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterFieldLocation4 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()
                    'Dim crParameterValues4 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("PeriodStart")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("EndPeriod")
                    'crParameterFieldLocation4 = crParameterFieldDefinitions.Item("viewtype")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'crParameterDiscreteValue4 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    'Discreet values
                    paramV = Split(ViewType, ",")
                    x = paramV.Length

                    'For y = 0 To x - 1
                    'crParameterDiscreteValue4.Value = Trim(paramV(y))
                    'crParameterValues4.AddValue(crParameterDiscreteValue4.Value)
                    'Next
                    ' crParameterFieldLocation4.ApplyCurrentValues(crParameterValues4)
                    crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)



                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "Fx Daily Position Movement - Detailed" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("TransactionDate")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue


                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    'crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    'crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    'crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Fx Reference Rates History by date" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("dateSet")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue


                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    'crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    'crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    'crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Fx Daily Position Movement - Summerized" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("TransactionDate")
                    'crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    'crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    'crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    'crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Fx Placements and Deposits by Date captured" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("DateEntered")
                    'crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    'crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    'crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    'crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "FxSettlements by date" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date entered")
                    'crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    'crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    'crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    'crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Fx Forward Deals by date captured" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date Entered")
                    'crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    'crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    'crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    'crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Fx Spot Deals by date captured" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date Entered")
                    'crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    'crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    'crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    'crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "FxNet Exposure rpt" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date entered")
                    'crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    'crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    'crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    'crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "User Functions One User" Then
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterValues2 As ParameterValues


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2.Value = Trim(Request.QueryString("user"))
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("user_passed")
                    crParameterValues2 = crParameterFieldLocation2.CurrentValues
                    crParameterValues2.Add(crParameterDiscreteValue2)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "Fx Currency ladder" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Cur")
                    'crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Trim(Request.QueryString("currency"))
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    crParameterValues.AddValue(Trim(Request.QueryString("currency")))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Fx Rates trend Daily" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()

                    CR.Load(Server.MapPath("../Reports/" & Request.QueryString("report") & ""))
                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("RatesDate")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("CurPair")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "Short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Request.QueryString("currency")
                    'crParameterValues2.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "Fx Rates trend" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("CurPair")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("loggedUser")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Trim(Request.QueryString("currency"))
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    crParameterDiscreteValue2.Value = Trim(Request.QueryString("username"))
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Trim(Request.QueryString("currency")))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                ElseIf Request.QueryString("report") = "Fx Closing Position per Dealer" Then

                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Date")
                    'crParameterFieldLocation2 = crParameterFieldDefinitions.Item("DateEntered2")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    'range values
                    crParameterDiscreteValue.Value = Format(CDate(Date1), "short date")
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)
                    'crParameterDiscreteValue2.Value = Format(CDate(date2), "Short date")
                    'crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    'crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)

                    crParameterValues.AddValue(Format(CDate(Date1), "MM/dd/yyyy 00:00:00"))
                    'crParameterValues.AddRange(CDate(Date1), CDate(date2), RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)




                ElseIf Request.QueryString("report") = "Live Deals Tenor Remaining" Then

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Days to Mature")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue

                    'Discreet values

                    crParameterDiscreteValue.Value = DaysTM
                    crParameterValues.AddValue(crParameterDiscreteValue.Value)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)





                ElseIf Request.QueryString("report") = "Matured Deals by maturity date" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterDiscreteValue3 As ParameterDiscreteValue
                    Dim crParameterRangeValue As New ParameterRangeValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterFieldLocation3 As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()
                    Dim crParameterValues2 As New ParameterValues()
                    Dim crParameterValues3 As New ParameterValues()


                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("Maturity Date")

                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue

                    Dim FormatedDate1, FormatedDate2 As String
                    FormatedDate1 = DatePart(DateInterval.Month, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Day, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Year, CDate(Request.QueryString("Date1")))
                    FormatedDate2 = DatePart(DateInterval.Month, CDate(Request.QueryString("Date2"))) & "/" & DatePart(DateInterval.Day, CDate(Request.QueryString("Date2"))) & "/" & DatePart(DateInterval.Year, CDate(Request.QueryString("Date2")))
                    'range values   

                    crParameterValues.AddRange(FormatedDate1, FormatedDate2, RangeBoundType.BoundInclusive, RangeBoundType.BoundInclusive)
                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)

                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                    Exit Sub

                ElseIf Request.QueryString("report") = "Customer Balance" Then
                    Dim paramV, ParamX As String()
                    Dim x, y As Integer

                    '
                    Dim FormatedDate1 As String
                    FormatedDate1 = DatePart(DateInterval.Month, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Day, CDate(Request.QueryString("Date1"))) & "/" & DatePart(DateInterval.Year, CDate(Request.QueryString("Date1")))


                    Dim crParameterDiscreteValue As ParameterDiscreteValue
                    Dim crParameterFieldDefinitions As ParameterFieldDefinitions
                    Dim crParameterFieldLocation As ParameterFieldDefinition
                    Dim crParameterValues As New ParameterValues()

                    Dim crParameterDiscreteValue2 As ParameterDiscreteValue
                    Dim crParameterFieldLocation2 As ParameterFieldDefinition
                    Dim crParameterValues2 As New ParameterValues()


                    CR.Load(Server.MapPath("../Reports/" & Request.QueryString("report") & ""))

                    crParameterFieldDefinitions = CR.DataDefinition.ParameterFields
                    crParameterFieldLocation = crParameterFieldDefinitions.Item("customer")
                    crParameterFieldLocation2 = crParameterFieldDefinitions.Item("Start Date")
                    crParameterDiscreteValue = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
                    crParameterDiscreteValue2.Value = Trim(FormatedDate1)
                    crParameterValues2.AddValue(crParameterDiscreteValue2.Value)
                    crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)


                    
                    crParameterDiscreteValue.Value = Trim(Session("selectedCustomer"))
                        crParameterValues.AddValue(crParameterDiscreteValue.Value)


                    crParameterFieldLocation.ApplyCurrentValues(crParameterValues)


                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)



                Else



                    CrystalReportViewer1.ReportSource = CR
                    CrystalReportViewer1.Zoom(100)


                End If



                'Dim CrExportOptions As New ExportOptions
                'Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                'Dim CrFormatTypeOptions As New HTMLFormatOptions()
                'CrExportOptions.ExportFormatType = ExportFormatType.HTML40
                'CrFormatTypeOptions.HTMLBaseFolderName = "C:\Confirmation"
                'CrFormatTypeOptions.HTMLFileName = "test.html"
                'CrFormatTypeOptions.HTMLEnableSeparatedPages = True
                'CrFormatTypeOptions.HTMLHasPageNavigator = True
                'CrExportOptions.FormatOptions = CrFormatTypeOptions
                'CR.Export()


                'Dim CrExportOptions As ExportOptions
                'Dim CrDiskFileDestinationOptions As New  _
                'DiskFileDestinationOptions()
                'Dim CrFormatTypeOptions As New HTMLFormatOptions()
                'CrDiskFileDestinationOptions.DiskFileName = _
                '                            "\\" & serverName & "\Confirmations\test.html"

                'CrFormatTypeOptions.HTMLBaseFolderName = "C:\Confirmation"
                'CrFormatTypeOptions.HTMLFileName = "test.html"
                'CrFormatTypeOptions.HTMLEnableSeparatedPages = True
                'CrFormatTypeOptions.HTMLHasPageNavigator = True


                'CrExportOptions = CR.ExportOptions
                'With CrExportOptions
                '    .ExportDestinationType = ExportDestinationType.DiskFile
                '    .ExportFormatType = ExportFormatType.HTML40
                '    .ExportDestinationOptions = CrDiskFileDestinationOptions
                '    .FormatOptions = CrFormatTypeOptions
                'End With

                'CR.Export()


            Catch ex As Exception
                'Log the event *****************************************************
                object_userlog.Msg(ex.Message & Request.QueryString("report") & " . Please check report parameters.", True, "../index.aspx", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " Report name : " & Request.QueryString("report"), "error")
                '************************END****************************************

                'Response.Write("<script>  alert('" & ex.Message & Request.QueryString("report") & " . Please check report parameters.') </script>")

                '' MsgBox(ex.Message & " Report name : " & Request.QueryString("report") & ". Please check report parameters.", MsgBoxStyle.Exclamation, "Load Report")
                ''Log the event *****************************************************
                'object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " Report name : " & Request.QueryString("report"), Session("serverName"), Session("client"))
                ''************************END****************************************
                'Response.Write("<script> window.location='../index.aspx' </script>")
            End Try

        End If
    End Sub



    Private Function GetPairNum(ByVal Cur As String)
        Dim PairNum As Integer

        Try
            strSQL = "select PairNumber From FXTRADINGPAIRS where BaseCur='" & Mid(Cur, 1, 3) & "' and CounterCur='" & Mid(Cur, 5, 3) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                PairNum = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            Return PairNum

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try

    End Function



End Class
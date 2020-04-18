Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine.Database
'Imports CrystalDecisions.windows.forms

Public Class report
    Inherits System.Web.UI.Page

    Private Sub report_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim crtableLogoninfos As New TableLogOnInfos()
        Dim crtableLogoninfo As New TableLogOnInfo()
        Dim crConnectionInfo As New ConnectionInfo()
        Dim CrTables As CrystalDecisions.CrystalReports.Engine.Tables
        Dim CrTable As Table
        Dim TableCounter

        Dim CR As New ReportDocument
        CR.Load(Server.MapPath("../Reports/" & Request.QueryString("report") & ".rpt"))

        With crConnectionInfo
            .ServerName = "tresyscon"
            'If you are connecting to Oracle there is no DatabaseName. Use an empty string. 
            'For example, .DatabaseName = ""
            .DatabaseName = Session("dataBaseName")
            .UserID = "tresyscon"
            .Password = ""
        End With

        CrTables = CR.Database.Tables


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

        CrystalReportViewer1.ReportSource = CR
        CrystalReportViewer1.Zoom(100)
        'crystalReportViewer1.ExportReport() ;
        CrystalReportViewer1.RefreshReport()
        ' CR.PrintOptions.PrinterName = GetDefaultPrinter()
        CR.PrintToPrinter(1, False, 0, 0)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
    End Sub

End Class
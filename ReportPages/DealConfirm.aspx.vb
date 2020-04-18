Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine.Database
Imports System.IO

'Imports CrystalDecisions.windows.forms

Public Class DealConfirm
    Inherits System.Web.UI.Page

    Private Sub DealConfirm_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        Dim crParameterFieldDefinitions As ParameterFieldDefinitions
        Dim crParameterFieldLocation2 As ParameterFieldDefinition
        Dim crParameterDiscreteValue2 As ParameterDiscreteValue
        Dim crParameterValues2 As ParameterValues

        'CR.Load(Server.MapPath("../Reports/" & Request.QueryString("report") & ".rpt"))
        crParameterFieldDefinitions = CR.DataDefinition.ParameterFields

        crParameterDiscreteValue2 = New CrystalDecisions.Shared.ParameterDiscreteValue
        crParameterDiscreteValue2.Value = Trim(Request.QueryString("Dealref"))
        crParameterFieldLocation2 = crParameterFieldDefinitions.Item("Dealref")
        crParameterValues2 = crParameterFieldLocation2.CurrentValues
        crParameterValues2.Add(crParameterDiscreteValue2)
        crParameterFieldLocation2.ApplyCurrentValues(crParameterValues2)



        CrystalReportViewer1.ReportSource = CR
        CrystalReportViewer1.Zoom(100)
        'crystalReportViewer1.ExportReport() ;
        ' CrystalReportViewer1.RefreshReport()
        ' CR.PrintOptions.PrinterName = GetDefaultPrinter()
        CR.PrintToPrinter(1, False, 0, 0)
        'Export Report to PDF
        'If File.Exists("\\" & Session("serverName") & "\Confirmations\" & Session("dataBaseName") & "\" & Request.QueryString("Dealref") & ".pdf") = True Then
        '    File.Delete("\\" & Session("serverName") & "\Confirmations\" & Session("dataBaseName") & "\" & Request.QueryString("Dealref") & ".pdf")
        'End If
        'If Directory.Exists("\\" & Session("serverName") & "\Confirmations" & Session("dataBaseName")) = False Then
        '    Directory.CreateDirectory("\\" & Session("serverName") & "\Confirmations\" & Session("dataBaseName"))
        'End If


        If Directory.Exists("C:\Confirmation") = False Then
            Directory.CreateDirectory("C:\Confirmation")
        End If
        'Export Report to PDF
        If File.Exists("C:\Confirmation\" & Trim(Request.QueryString("Dealref")) & ".pdf") = True Then
            File.Delete("C:\Confirmation\" & Trim(Request.QueryString("Dealref")) & ".pdf")
        End If


        Dim CrExportOptions As ExportOptions
        Dim CrDiskFileDestinationOptions As New  _
        DiskFileDestinationOptions()
        Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
        CrDiskFileDestinationOptions.DiskFileName = _
                                    "C:\Confirmation\" & Trim(Request.QueryString("Dealref")) & ".pdf"
        CrExportOptions = CR.ExportOptions
        With CrExportOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
            .DestinationOptions = CrDiskFileDestinationOptions
            .FormatOptions = CrFormatTypeOptions
        End With

        CR.Export()

        CrDiskFileDestinationOptions.DiskFileName = _
                                     "C:\Confirmation\" & Request.QueryString("Dealref") & ".pdf"
        CrExportOptions = CR.ExportOptions
        With CrExportOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
            .DestinationOptions = CrDiskFileDestinationOptions
            .FormatOptions = CrFormatTypeOptions
        End With


    End Sub

End Class
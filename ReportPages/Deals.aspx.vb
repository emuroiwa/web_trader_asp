Imports System.Data.SqlClient

Public Class Deals

    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader

    Private curr As String
    Private object_userlog As New usrlog.usrlog
    Private AllCurrencies As String
    Private DealStatus As Integer
    Private selectedDealer As String = ""
    Private selectedDealRef As String = ""
    Private selectedcustomer As String = ""
    Private selectedDealcodes As String = ""


    Private Sub GetChecked()
        'get deal

        For Each item As EO.Web.GridItem In GridDealer.CheckedItems
            selectedDealer = selectedDealer & ",'" & item.Cells(1).Value & "'"
        Next
        selectedDealer = Trim(Mid(selectedDealer, 2, Len(selectedDealer)))
        'get deal ref
        For Each item As EO.Web.GridItem In GridDealRef.CheckedItems
            selectedDealRef = selectedDealRef & ",'" & item.Cells(1).Value & "'"
        Next
        selectedDealRef = Trim(Mid(selectedDealRef, 2, Len(selectedDealRef)))
        ' "get customer"
        For Each item As EO.Web.GridItem In GridCustomer.CheckedItems
            selectedcustomer = selectedcustomer & ",'" & item.Cells(1).Value & "'"
        Next
        selectedcustomer = Trim(Mid(selectedcustomer, 2, Len(selectedcustomer)))
        ' "get deal codes"
        For Each item As EO.Web.GridItem In GridDeals.CheckedItems
            selectedDealcodes = selectedDealcodes & ",'" & item.Cells(1).Value & "'"
        Next
        selectedDealcodes = Trim(Mid(selectedDealcodes, 2, Len(selectedDealcodes)))
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetDeals()
        GetDealCodes()
        LoadCustomers()
        GetDealerNames()


        dt1.Text = Session("SysDate")
        dt2.Text = Session("SysDate")
    End Sub
    Private Sub GetDealCodes()
        Dim cnSQLX As SqlConnection
        Dim strSQLX As String
        Dim cmSQLX As SqlCommand
        Dim drSQLX As SqlDataReader
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
    Private Sub GetDeals()

        Try
            strSQL = "select dealreference from deals_live"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader


            GridDealRef.DataSource = drSQL
            GridDealRef.DataBind()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            'ernest
            '' CusDownloadStart.Suspend()

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try

    End Sub
    Private Sub LoadCustomers()

        Try
            strSQL = "select customer_Number,fullName from customer where frontoffice='Y' and len(customer_number)<=9 order by fullname"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader


            GridCustomer.DataSource = drSQL
            GridCustomer.DataBind()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            'ernest
            '' CusDownloadStart.Suspend()

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try
    End Sub

    Private Sub GetDealerNames()
        Dim cnSQLX As SqlConnection
        Dim strSQLX As String
        Dim cmSQLX As SqlCommand
        Dim drSQLX As SqlDataReader

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


    
    Private Sub DisableAllControls()
        dt1.Enabled = False
        dt2.Enabled = False
        GridCustomer.Enabled = False
        GridDealer.Enabled = False
        GridDealRef.Enabled = False
        GridDeals.Enabled = False
        

        'lstDealer.Enabled = False
        'lstDealer.Items.Clear()
        'cmbDealRef.Enabled = False
        'lstref.Enabled = False
        'btnRm.Enabled = False
        'btnAdd.Enabled = False
        'btnRM1.Enabled = False
        'btnAdd1.Enabled = False
        'lstCust.Enabled = False
        'lstCust.Items.Clear()
        'lstDealtypes.Enabled = False
        'lstDealtypes.Items.Clear()
        'btnSelAll.Enabled = False
        'btnSelNone.Enabled = False
        'btnRmAll.Enabled = False
        'btnSelAllx.Enabled = False
        'btnSelNonex.Enabled = False
        'btnClear.Enabled = False
        'btnRmAllp.Enabled = False
        'txtStartRange.Text = ""
        'txtStartRange.Enabled = False
        'txtEndCust.Text = ""
        'txtEndCust.Enabled = False
    End Sub

    Private Sub EnableAllControls()
        dt1.Enabled = True
        dt2.Enabled = True
        GridCustomer.Enabled = True
        GridDealer.Enabled = True
        GridDealRef.Enabled = True
        GridDeals.Enabled = True

        divdealtypes.Style.Add("display", "none")
        divcustomer.Style.Add("display", "none")
        divdate.Style.Add("display", "none")
        divdealer.Style.Add("display", "none")
        divdealref.Style.Add("display", "none")
        divdealtype.Style.Add("display", "none")
        'lstDealer.Enabled = True
        'lstDealer.Items.Clear()
        'cmbDealRef.Enabled = True
        'lstref.Enabled = True
        'lstref.Items.Clear()
        'btnRm.Enabled = True
        'btnAdd.Enabled = True
        'btnRM1.Enabled = True
        'btnAdd1.Enabled = True
        'lstCust.Enabled = True
        'lstCust.Items.Clear()
        'lstDealtypes.Enabled = True
        'lstDealtypes.Items.Clear()
        grpDealer.Enabled = True
        grpDealref.Enabled = True
        grpNum.Enabled = True
        grpCustomer.Enabled = True
        grpDealtypes.Enabled = True
        grpDateRange.Enabled = True
        'btnSelAll.Enabled = True
        'btnSelNone.Enabled = True
        'btnRmAll.Enabled = True
        'btnSelAllx.Enabled = True
        'btnSelNonex.Enabled = True
        'btnClear.Enabled = True
        'btnRmAllp.Enabled = True
        'txtStartRange.Text = ""
        'txtStartRange.Enabled = True
        'txtEndCust.Text = ""
        'txtEndCust.Enabled = True
        Call GetDealerNames()
        Call GetDealCodes()
        Call LoadCustomers()
        Call GetDeals()
    End Sub
    Protected Sub cmbReport_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbReport.SelectedIndexChanged
        GrpStatus.Enabled = False

        If cmbStatus.SelectedValue.ToString = 0 Then 'Live deals reports
            If cmbReport.SelectedValue.ToString = "Customer Summary on Deal type" Or cmbReport.SelectedValue.ToString = "Summary on Deal Type" Or cmbReport.SelectedValue.ToString = "Dealer Summary" Or cmbReport.SelectedValue.ToString = "Totals of Running Deals" Then
                lblRtpSpec.Text = "This report takes no parameters."
                Call DisableAllControls()
            Else
                'Call EnableAllControls()

                If cmbReport.SelectedValue.ToString = "Deals for client" Then


                    lblRtpSpec.Text = "This report takes two parameters: -  <br>" & vbCrLf & _
                                      "Mandetory -> Customer Number  <br>" & vbCrLf & _
                                      "Optional -> Deal Type"
                    'grpDealer.Enabled = False
                    'grpDealref.Enabled = False
                    'grpNum.Enabled = False
                    'grpDateRange.Enabled = False
                    divdealtype.Style.Add("display", "none")
                    divcustomer.Style.Add("display", "none")
                ElseIf cmbReport.SelectedValue.ToString = "Deals captured on this date" Then
                    lblRtpSpec.Text = "This report takes three sets of parameters: -  <br>" & vbCrLf & _
                                      "Mandetory -> Date  <br>" & vbCrLf & _
                                      "Optional  -> Customer Number <br>" & vbCrLf & _
                                      "Optional  -> Dealer Name <br>" & vbCrLf & _
                                      "Optional  -> Deal type <br>" & vbCrLf & _
                                      "Select at most one optional parameter."
                    'grpDealref.Enabled = False
                    'grpNum.Enabled = False
                    divcustomer.Style.Add("display", "none")
                    divdate.Style.Add("display", "none")
                    divdealer.Style.Add("display", "none")
                    divdealtype.Style.Add("display", "none")
                ElseIf cmbReport.SelectedValue.ToString = "This deal Type" Then
                    lblRtpSpec.Text = "This report takes one parameter: -  <br>" & vbCrLf & _
                                      "Mandetory -> Deal Type"

                    'grpDealer.Enabled = False
                    'grpDealref.Enabled = False
                    'grpNum.Enabled = False
                    'grpDateRange.Enabled = False
                    'grpCustomer.Enabled = False
                    divdealtypes.Style.Add("display", "none")
                   

                ElseIf cmbReport.SelectedValue.ToString = "This deal reference" Then
                    lblRtpSpec.Text = "This report takes one parameter: -  <br>" & vbCrLf & _
                                     "Mandetory -> Deal Reference"
                    'grpDealer.Enabled = False
                    'grpNum.Enabled = False
                    'grpDateRange.Enabled = False
                    'grpCustomer.Enabled = False
                    'grpDealtypes.Enabled = False
                    divdealref.Style.Add("display", "none")
                   

                ElseIf cmbReport.SelectedValue.ToString = "Matuaring on this date" Then
                    lblRtpSpec.Text = "This report takes one parameter: -  <br>" & vbCrLf & _
                                     "Mandetory -> Date"
                    'grpDealer.Enabled = False
                    'grpNum.Enabled = False
                    'grpCustomer.Enabled = False
                    'grpDealtypes.Enabled = False
                    'grpDealref.Enabled = False
                    divdate.Style.Add("display", "none")
                   
                Else
                    lblRtpSpec.Text = "This report takes one parameter: - <br> " & vbCrLf & _
                                    "Mandetory -> Number"
                    'grpDealer.Enabled = False
                    'grpCustomer.Enabled = False
                    'grpDealtypes.Enabled = False
                    'grpDealref.Enabled = False
                    'grpDateRange.Enabled = False
                    divcustomer.Style.Add("display", "none")

                End If
                ' 
            End If

            btnLoad.Enabled = True

        End If


        If cmbStatus.SelectedValue.ToString = 1 Then 'Matured deals reports
            If cmbReport.SelectedValue.ToString = "Customer Summary on Deal TypeM" Or cmbReport.SelectedValue.ToString = "Summary on Deal Type" Or cmbReport.SelectedValue.ToString = "Dealer Summary" Or cmbReport.SelectedValue.ToString = "Totals of Matured Deals" Then
                Call DisableAllControls()
                btnLoad.Enabled = True
            Else
                lblRtpSpec.Text = "This report takes one parameter: - " & vbCrLf & _
                                                       "Mandetory -> Date"
                Call EnableAllControls()
                'grpDealer.Enabled = False
                'grpCustomer.Enabled = False
                'grpDealtypes.Enabled = False
                'grpDealref.Enabled = False
                'grpNum.Enabled = False
                'btnLoad.Enabled = True
                divdate.Style.Add("display", "none")

            End If
        End If


        If cmbStatus.SelectedValue.ToString = 2 Then

            If cmbReport.SelectedValue.ToString = "Deal Status Report" Then

                lblRtpSpec.Text = "This report takes 2 parameters." & vbCrLf & _
                                                           "Mandetory -> Start Date <br>" & vbCrLf & _
                                                           "Mandetory -> End Date  <br>" & vbCrLf & _
                                                           "Mandetory -> Deal Codes"
                'grpDealer.Enabled = False
                'grpCustomer.Enabled = False
                'grpDealtypes.Enabled = False
                'grpDealref.Enabled = False
                'grpNum.Enabled = False
                'btnLoad.Enabled = True
                'grpDateRange.Enabled = True
                'grpDealtypes.Enabled = True
                'GrpStatus.Enabled = True
                divdate.Style.Add("display", "none")
                divdealref.Style.Add("display", "none")
                

            ElseIf cmbReport.SelectedValue.ToString = "Deals by Customer by Product" Then

                lblRtpSpec.Text = "This report takes 3 parameters." & vbCrLf & _
                                                          "Mandetory -> Start Date  <br>" & vbCrLf & _
                                                          "Mandetory -> End Date  <br>" & vbCrLf & _
                                                          "Mandetory -> Deal Codes"
                'grpDealer.Enabled = False
                'grpCustomer.Enabled = False
                'grpDealtypes.Enabled = True
                'grpDealref.Enabled = False
                'grpNum.Enabled = False
                'btnLoad.Enabled = True
                'grpDateRange.Enabled = True
                'GrpStatus.Enabled = True
                divdealtypes.Style.Add("display", "none")
                divcustomer.Style.Add("display", "none")
                divdealer.Style.Add("display", "none")
                divdealtype.Style.Add("display", "none")
            Else
                lblRtpSpec.Text = "This report takes two parameters: -  <br>" & vbCrLf & _
                                   "Mandetory -> Customer Number <br>" & vbCrLf & _
                                   "Optional -> Deal Type"
                'grpDealer.Enabled = False
                'grpDealref.Enabled = False
                'grpNum.Enabled = False
                'grpDateRange.Enabled = True
                'grpCustomer.Enabled = True
               divdate.Style.Add("display", "none")
                divdealref.Style.Add("display", "none")

            End If

            btnLoad.Enabled = True
        End If
    End Sub
    Protected Sub cmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus.SelectedIndexChanged
        cmbReport.Enabled = True
        cmbReport.Items.Clear()
        cmbReport.Text = ""

        If cmbStatus.SelectedValue.ToString = 0 Then
            DealStatus = 1 'live deals
            cmbReport.Items.Add("")

            cmbReport.Items.Add("Customer Summary on Deal type")
            cmbReport.Items.Add("Summary on Deal Type")
            cmbReport.Items.Add("Dealer Summary")
            cmbReport.Items.Add("Totals of Running Deals")
            cmbReport.Items.Add("Deals for client")
            cmbReport.Items.Add("Deals captured on this date")
            cmbReport.Items.Add("This deal Type")
            cmbReport.Items.Add("This deal reference")
            cmbReport.Items.Add("Matuaring on this date")
            cmbReport.Items.Add("Days before deal Matures")

        ElseIf cmbStatus.SelectedValue.ToString = 1 Then
            DealStatus = 2 'matured deals
            cmbReport.Items.Add("")
            cmbReport.Items.Add("Customer Summary on Deal TypeM")
            cmbReport.Items.Add("Summary on Deal Type")
            cmbReport.Items.Add("Dealer Summary")
            cmbReport.Items.Add("Totals of Matured Deals")
            cmbReport.Items.Add("Deals by maturity date")
        ElseIf cmbStatus.SelectedValue.ToString = 2 Then
            DealStatus = 3
            cmbReport.Items.Add("")
            cmbReport.Items.Add("Deal Status Report")
            cmbReport.Items.Add("Customer Balance")
        End If
    End Sub
    Protected Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Dim REPORTNAME As String
        Dim status1 As String
        Dim status2 As String
        Call GetChecked()
        If cmbStatus.SelectedValue.ToString = 0 Then
            MsgBox(cmbReport.SelectedValue.ToString)
            If cmbReport.SelectedValue.ToString = "Customer Summary on Deal type" Then
                Response.Redirect("ReportViewer.aspx?report=Customer summary on Deal Type")


            ElseIf cmbReport.SelectedValue.ToString = "Summary on Deal Type" Then
                Response.Redirect("ReportViewer.aspx?report=Live Deals Summary Deal Type")

            ElseIf cmbReport.SelectedValue.ToString = "Dealer Summary" Then
                Response.Redirect("ReportViewer.aspx?report=Live Deals Summary Dealer")

            ElseIf cmbReport.SelectedValue.ToString = "Totals of Running Deals" Then
                Response.Redirect("ReportViewer.aspx?report=Totals of running deals")


            ElseIf cmbReport.SelectedValue.ToString = "Deals for client" Then
                If selectedDealcodes = "" Then
                    Session("selectedcustomer") = selectedcustomer
                    Session("selectedDealcodes") = selectedDealcodes
                    Response.Redirect("ReportViewer.aspx?report=Live Deals Customer2")

                Else
                    Session("selectedcustomer") = selectedcustomer
                    Session("selectedDealcodes") = selectedDealcodes

                    'selectedDealcodes

                    Response.Redirect("ReportViewer.aspx?report=Live Deals Customer")

                End If

                Response.Redirect("ReportViewer.aspx?report=Totals of running deals")

            ElseIf cmbReport.SelectedValue.ToString = "Deals for client" Then



                If selectedDealcodes = "" Then
                    REPORTNAME = "Live Deals Summary Deal Types By Date.rpt"
                ElseIf selectedcustomer = "" Or txtStartRange.Text <> "" And txtEndCust.Text <> "" Then
                    REPORTNAME = "Live Deals Summary Customer By Date.rpt"
                ElseIf selectedDealer = "" Then

                    REPORTNAME = "Live Deals Summary Dealer By Date.rpt"

                End If
            End If

        End If
        If cmbStatus.SelectedValue.ToString = 1 Then
            If cmbReport.SelectedValue.ToString = "Customer Summary on Deal TypeM" Then
                Response.Redirect("ReportViewer.aspx?report=Customer summary on Deal TypeM")
            ElseIf cmbReport.SelectedValue.ToString = "Summary on Deal Type" Then
                Response.Redirect("ReportViewer.aspx?report=Matured Deals Summary Deal Type")

            ElseIf cmbReport.SelectedValue.ToString = "Dealer Summary" Then
                Response.Redirect("ReportViewer.aspx?report=Matured Deals Summary Dealer")

            ElseIf cmbReport.SelectedValue.ToString = "Totals of Matured Deals" Then
                Response.Redirect("ReportViewer.aspx?report=Totals of Matured Deals")

            ElseIf cmbReport.SelectedValue.ToString = "Deals by maturity date" Then
                Response.Redirect("ReportViewer.aspx?report=Matured Deals by maturity date&Date1=" & dt1.Text & "Date2=" & dt2.Text)
            End If
        End If

        If cmbStatus.SelectedValue.ToString = 2 Then
            If cmbReport.SelectedValue.ToString = "Deal Status Report" Then
                If CheckBoxLive.Checked = True Then
                    Status1 = "live"
                Else
                    status1 = ""
                End If

                If CheckBoxMatured.Checked = True Then
                    status2 = "matured"
                Else
                    status2 = ""
                End If
            End If
            Response.Redirect("ReportViewer.aspx?report=Matured Deals by maturity date&Date1=" & dt1.Text & "Date2=" & dt2.Text & "&Status1=" & status1 & "status2=" & status2 & "&DealTypes=" & selectedDealcodes)
            If cmbReport.SelectedValue.ToString = "Deals by Customer by Product" Then
                If CheckBoxLive.Checked = True Then
                    status1 = "live"
                Else
                    status1 = ""
                End If

                If CheckBoxMatured.Checked = True Then
                    status2 = "matured"
                Else
                    status2 = ""
                End If
                Response.Redirect("ReportViewer.aspx?report=All deals by Customer by Product&Date1=" & dt1.Text & "Date2=" & dt2.Text & "&Status1=" & status1 & "status2=" & status2)

            End If

            If cmbReport.SelectedValue.ToString = "Customer Balance" Then
                If CheckBoxLive.Checked = True Then
                    status1 = "live"
                Else
                    status1 = ""
                End If

                If CheckBoxMatured.Checked = True Then
                    status2 = "matured"
                Else
                    status2 = ""
                End If
                Response.Redirect("ReportViewer.aspx?report=All deals by Customer by Product&Date1=" & dt1.Text & "Date2=" & dt2.Text)

            End If
        End If
    End Sub

End Class
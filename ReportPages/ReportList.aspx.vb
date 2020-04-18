Imports System.Data.SqlClient

Public Class ReportList
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private cnSQLx As SqlConnection
    Private strSQLx As String
    Private cmSQLx As SqlCommand
    Private drSQLx As SqlDataReader
    Public REPORTNAME As String
    Private curr As String
    Private object_userlog As New usrlog.usrlog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call GetReportTypes()
            Call getReportCategories()
            GetCurrency()
            GetDealer()

            txtDate.Text = Session("SysDate")

        End If
    End Sub
    Private Sub GetDealer()
        Try

            strSQL = "select user_id from users"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader



            Do While drSQL.Read
                cmbUser.Items.Add(Trim(drSQL.Item(0)))
            Loop

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
    Private Sub GetCurrency()
        Try

            strSQL = "select currencycode from astval"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader



            Do While drSQL.Read
                cmbCurrency.Items.Add(Trim(drSQL.Item(0)))
                cmbCurrency1.Items.Add(Trim(drSQL.Item(0)))
            Loop

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
    Private Sub getReportCategories()
        Try
            strSQL = "select * from rptCategory where catid not in('syscat')"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            lstCat.Items.Clear()

            Do While drSQL.Read
              
                lstCat.Items.Add(drSQL.Item(1).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'catch any error
        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
    End Sub
    'Get Available report types
    Private Sub GetReportTypes()
        Try
            strSQL = "select classname from RptClass"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            cmbClass.Items.Clear()

            Do While drSQL.Read
                cmbClass.Items.Add(drSQL.Item(0).ToString)
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            cmbClass.SelectedIndex = 1
            'catch any error
        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
    End Sub

    Private Sub ReportsLoad(ByVal catergory As String)
        Try
            strSQL = "select * from UserReports join reports on userreports.reportid = reports.reportid join rptcategory on rptcategory.catid=reports.categoryid  join rptclass on rptclass.classid = reports.classid             where userID = '" & Session("username") & "' and classname='" & Trim(cmbClass.SelectedValue.ToString()) & "' and  CatName='" & Trim(catergory) & "' "
            'and classname='" & Trim(cmbClass.SelectedValue.ToString()) & "' and  CatName='" & Trim(lstCat.SelectedItem.ToString()) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                listReports.Items.Add(drSQL.Item("ReportName").ToString)
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            cmbClass.SelectedIndex = 1

            'catch any error
        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
    End Sub
    Private Function GetReportID(ByVal name As String)
        Dim res As String = ""
        Try
            strSQL = "select * from UserReports join reports on userreports.reportid = reports.reportid join rptcategory on rptcategory.catid=reports.categoryid  join rptclass on rptclass.classid = reports.classid             where userID = '" & Session("username") & "' and classname='" & Trim(cmbClass.SelectedValue.ToString()) & "' and  Reportname='" & Trim(name) & "' "
            'and classname='" & Trim(cmbClass.SelectedValue.ToString()) & "' and  CatName='" & Trim(lstCat.SelectedItem.ToString()) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                res = Trim(drSQL.Item("ReportID").ToString)
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            cmbClass.SelectedIndex = 1

            'catch any error
        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
        Return res
    End Function

    Protected Sub listReports_SelectedIndexChanged(sender As Object, e As EventArgs) Handles listReports.SelectedIndexChanged
        Try
            lblReportID.Text = GetReportID(listReports.SelectedItem.Text.ToString())
        Catch ex As NullReferenceException

        End Try
    End Sub
    Protected Sub lstCat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstCat.SelectedIndexChanged
        listReports.Items.Clear()
        Call ReportsLoad(lstCat.SelectedItem.Text.ToString())
       


    End Sub
    Private Function getCurrencies() As String
        Dim x As String = "USD"
        'Try
        '    'validate username first
        '    strSQL = "Select currencycode from currencies where isbasecurrency='Yes'"
        '    cnSQL = New SqlConnection(ConnectionString)
        '    cnSQL.Open()
        '    cmSQL = New SqlCommand(strSQL, cnSQL)
        '    drSQL = cmSQL.ExecuteReader()

        '    Do While drSQL.Read
        '        x = Trim((drSQL.Item(0).ToString))
        '    Loop

        '    ' Close and Clean up objects
        '    drSQL.Close()
        '    cnSQL.Close()
        '    cmSQL.Dispose()
        '    cnSQL.Dispose()

        '    Return x

        'Catch ex As SqlException
        '    MsgBox(ex.Message, MsgBoxStyle.Critical)

        '    'Log the event *****************************************************
        '    SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message)
        '    '************************END****************************************

        'End Try

        Return x
    End Function
    Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click

        If Trim(cmbReportType.SelectedValue.ToString) = "All Deals - Live / Matured" Then
            Response.Redirect("ReportViewer.aspx?report=Top Depositors&currency=" & cmbCurrency.SelectedValue.ToString & "date=" & txtDate.Text)
        Else
            REPORTNAME = "Top Depositors Live.RPT"

            Response.Redirect("ReportViewer.aspx?report=Top Depositors Live&currency=" & cmbCurrency.SelectedValue.ToString & "date=" & txtDate.Text)
        End If
    End Sub
    Protected Sub btnOk1_Click(sender As Object, e As EventArgs) Handles btnOk1.Click

       Response.Redirect("ReportViewer.aspx?report=User Functions One User&user=" & cmbUser.SelectedValue.ToString)

    End Sub
    Protected Sub btnOk2_Click(sender As Object, e As EventArgs) Handles btnOk2.Click
        'Clear top table before adding new records
        ClearTable()
        'Get the top x deposits and save them to top table
        GetTopDepositors(Int(txtNumber.Text))
        CalculatePerc() ' Calculate % for each deposit
        'Run report to display the top x depositors
        Response.Redirect("ReportViewer.aspx?report=Top Deposits&currency=" & cmbCurrency1.SelectedValue.ToString)

    End Sub

    Protected Sub cmdView_Click(sender As Object, e As EventArgs) Handles cmdView.Click
        Try
            If Trim(lblReportID.Text) = "RTPAXX" Then
                Response.Redirect("Deals.aspx?report=Deal Status Report Money Market")

            ElseIf Mid(Trim(lblReportID.Text), 1, 5) = "RTPAZ" Then ' Fx Reports

                REPORTNAME = Trim(listReports.SelectedValue.ToString()) & ".rpt"
                If REPORTNAME = "Fx Deals Pending Authorisation.rpt" Then
                    Response.Redirect("ReportViewer.aspx?report=Fx Placements and deposit_matured deals")

                ElseIf REPORTNAME = "Fx Deals Pending Verification.rpt" Then
                    Response.Redirect("ReportViewer.aspx?report=Fx Placements and deposit_matured deals")

                ElseIf REPORTNAME = "Fx Placements and deposit_Live deals.rpt" Then
                    Response.Redirect("ReportViewer.aspx?report=Fx Placements and deposit_matured deals")

                ElseIf REPORTNAME = "Fx Placements and deposit_matured deals.rpt" Then
                    Response.Redirect("ReportViewer.aspx?report=Fx Placements and deposit_matured deals")

                ElseIf REPORTNAME = "Fx Gap Report.rpt" Then
                    Response.Redirect("ReportViewer.aspx?report=Fx Placements and deposit_matured deals")

                Else
                    Response.Redirect("FxRptParameters.aspx?report=Fx Placements and deposit_matured deals")
                    'Dim Reportform As New FxRptParameters
                    'Reportform.lblRptName.Text = Trim(listReports.FocusedItem.Text) & ".rpt"
                    'Reportform.ShowDialog()
                End If

            ElseIf Trim(listReports.SelectedValue.ToString()) = "Top Deposits" Then
                Response.Redirect("ReportViewer.aspx?report=Top Deposits")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "openTopDepositors();", True)


      


            ElseIf Trim(listReports.SelectedValue.ToString()) = "Top Depositors" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "openModal();", True)

            ElseIf Trim(listReports.SelectedValue.ToString()) = "User Functions One User" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "openDealer();", True)

            ElseIf Trim(listReports.SelectedValue.ToString()) = "Master Dealer Position" Then


                'If GetParameterValue(1) = "N" Then
                '    Dim xapicall As New XAPI_CALLS.XAPI
                '    xapicall.masterdealerpos(GetParameterValue(2))
                'End If

                'REPORTNAME = Trim(listReports.FocusedItem.Text) & ".RPT"
                'Dim reportView As New ReportViewer
                'reportView.MdiParent = mdiCli
                'reportView.user_passed = User

                'reportView.Show()
            Else

                REPORTNAME = Trim(listReports.SelectedValue.ToString()) & ".rpt"
                Response.Redirect("ReportViewer.aspx?report=" & Trim(listReports.SelectedValue.ToString()))
            End If

        Catch x As NullReferenceException

        Catch s As ArgumentOutOfRangeException
        End Try
    End Sub
    Private Sub CalculatePerc()

        Dim strSQL1 As String
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim DealAmnt As Decimal
        Dim Total As Decimal
        Dim PercofTotal As Decimal


        Total = GetTotalDeposits() ' Calculate the total of all deposits
        Try
            strSQL = "select dealreference, dealamount from topdepositors"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                DealAmnt = CDec(drSQL.Item(1).ToString)

                PercofTotal = Format(DealAmnt * 100 / Total, "###,###,###.00")

                'Save deatails  in TOP table
                strSQL1 = "Update topdepositors set PercTotalDeposits ='" & PercofTotal & "' where dealreference = '" & Trim(drSQL.Item(0).ToString) & "' and loggeduser='" & Session("username") & "'"

                cnSQL1 = New SqlConnection(Session("ConnectionString"))
                cnSQL1.Open()
                cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
                drSQL1 = cmSQL1.ExecuteReader()


                ' Close and Clean up objects
                drSQL1.Close()
                cnSQL1.Close()
                cmSQL1.Dispose()
                cnSQL1.Dispose()


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

    Private Sub ClearTable()

        'Clear top table before adding new records
        Try
            strSQL = "delete from topdepositors where loggeduser='" & Session("username") & "'"

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
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try
    End Sub
    Private Function GetParameterValue(ByVal val As Integer) As String
        Dim res As String = ""
        'Clear top table before adding new records
        Try
            If val = 1 Then
                strSQL = "select [value] from systemparameters where [parameter]='masterdealerpos'"
            Else
                strSQL = "select sqlstring from constr"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                res = Replace(Trim(drSQL.Item(0).ToString), "/", "'")
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

        Return res

    End Function
    Private Sub GetTopDepositors(ByVal topD As Integer)

        Dim strSQL1 As String
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim counter As Integer

        counter = 1
        Try
            strSQL = "select Dealreference,Fullname, dealamount,maturitydate,daystomaturity,interestrate from deals_live" & _
                     " join customer on deals_live.customernumber=customer.customer_number join dealtypes on" & _
                     " deals_live.dealtype=dealtypes.deal_code where dealtypes.dealbasictype='D' and deals_live.currency='" & Trim(curr) & "' order by dealamount desc "

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                'Save deatails  in TOP table
                strSQL1 = "Insert into topdepositors (Dealreference,Fullname, dealamount,maturitydate,daystomaturity,interestrate,loggeduser) " & _
                          " values('" & Trim(drSQL.Item(0).ToString) & "','" & Trim(drSQL.Item(1).ToString) & "','" & CDec(drSQL.Item(2).ToString) & "','" & CDate(drSQL.Item(3).ToString) & "','" & Int(drSQL.Item(4).ToString) & "','" & Trim(drSQL.Item(5).ToString) & "','" & Session("username") & "')"

                cnSQL1 = New SqlConnection(Session("ConnectionString"))
                cnSQL1.Open()
                cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
                drSQL1 = cmSQL1.ExecuteReader()


                ' Close and Clean up objects
                drSQL1.Close()
                cnSQL1.Close()
                cmSQL1.Dispose()
                cnSQL1.Dispose()

                If counter = topD Then Exit Do

                counter = counter + 1
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
    Private Function GetTotalDeposits() As Decimal
        Dim SumTotal As Decimal = 0

        Try
            strSQL = "select sum(DealAmount) from topdepositors where loggeduser='" & Session("username") & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read

                SumTotal = CDec(drSQL.Item(0))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            Return SumTotal

        Catch ex As SqlException
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try
    End Function

End Class
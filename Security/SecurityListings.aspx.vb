Imports System.Data.SqlClient
Imports sys_ui
Public Class SecurityListings

    Inherits System.Web.UI.Page
    Private approved As String
    Private cancelled As String
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private cnSQLx As SqlConnection
    Private strSQLx As String
    Private cmSQLx As SqlCommand
    Private drSQLx As SqlDataReader
    Private object_userlog As New usrlog.usrlog

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CheckUserPermissions()

            Call LoadCollateralItems("pending")
        End If
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("index.aspx")
    End Sub
    Private Sub LoadCollateralItems(ByVal filter As String)
        Try

            Select Case filter
                Case "approved"
                    strSQL = "select * from COLLATERAL_ITEMS  join customer on COLLATERAL_ITEMS.Customernumber=customer.customer_number where collateralApproved='Y' and collateralCancelled='N'" & _
                    "and expired='N'"
                Case "expired"
                    strSQL = "select * from COLLATERAL_ITEMS join customer on COLLATERAL_ITEMS.Customernumber=customer.customer_number where collateralApproved='Y'" & _
                    "and expired='Y'"
                Case "cancelled"
                    strSQL = "select * from COLLATERAL_ITEMS join customer on COLLATERAL_ITEMS.Customernumber=customer.customer_number where collateralCancelled='Y'"
                Case "pending"
                    strSQL = "select * from COLLATERAL_ITEMS join customer on COLLATERAL_ITEMS.Customernumber=customer.customer_number where collateralApproved='N' and collateralCancelled='N'"
            End Select

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader
            GrdListings.DataSource = drSQL
            GrdListings.DataBind()


            Do While drSQL.Read


            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)

        End Try
    End Sub
   
    Private Sub CheckUserPermissions()
        Try
            strSQL = "select function_ID from [USER_FUNCTIONS] where function_ID in('OPT287','OPT288') and user_id='" & Trim(Session("username")) & "'  "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            'Do While drSQL.Read
            '    Select Case Trim(drSQL.Item("function_ID").ToString)
            '        Case "OPT287"
            '            ApproveToolStripMenuItem.Enabled = True
            '        Case "OPT288"
            '            CancelToolStripMenuItem.Enabled = True
            '    End Select
            'Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)

        End Try
    End Sub

    Protected Sub cmbOptions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOptions.SelectedIndexChanged
        Select Case Trim(cmbOptions.SelectedValue.ToString)
            Case "Approved"
                Call LoadCollateralItems("approved")
            Case "Expired"
                Call LoadCollateralItems("expired")
            Case "Cancelled"
                Call LoadCollateralItems("cancelled")
            Case "Pending Approval"
                Call LoadCollateralItems("pending")
        End Select
    End Sub
    Protected Sub GrdListings_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        
        If e.CommandName = "Approve" Then
            Dim s As String = String.Empty
            lblsecref.Text = e.Item.Cells(2).Value.ToString()
            Call getStatus()
            Call Approve()

        End If
        If e.CommandName = "Cancel" Then
            Dim s As String = String.Empty
            lblsecref.Text = e.Item.Cells(2).Value.ToString()
            Call Cancel()

        End If
        If e.CommandName = "Detail" Then
            Dim s As String = String.Empty
            lblsecref.Text = e.Item.Cells(3).Value.ToString()

            Call DetailedReport()
        End If
        If e.CommandName = "Report" Then
            Dim s As String = String.Empty
            lblsecref.Text = e.Item.Cells(3).Value.ToString()
            Call report()

        End If
    End Sub
    Private Sub Approve()
        'If MessageBox.Show("Are you sure you want to approve the collateral item?", "Collateral", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        Try
            If Trim(Session("username")) = Trim(txtcreatedby.Text) Then
                MsgBox("You can not authorise a collateral item that you created", MsgBoxStyle.Information)
                Exit Sub
            End If

            If txtstatus.Text = "N" Then

                strSQL = "update COLLATERAL_ITEMS set collateralApproved='Y',approvedby='" & Trim(Session("username")) & "',dateApproved='" & CDate(Session("SysDate")) & "' where" & _
                " collateralReference='" & Trim(lblsecref.Text) & "'"
                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader

                MsgBox("Collateral Item Approved", MsgBoxStyle.Information, "Collateral")

                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()



                'Log the event *****************************************************loggedUserLog formated for log
                object_userlog.SendDataToLog(Session("username") & "COLT01" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Collateral Item Approved -  Reference : " & _
                Trim(Trim(lblsecref.Text)) & _
                " Customer : -" & Trim(txtcustomer.Text), Session("serverName"), Session("client"))
                '************************END****************************************



                Select Case Trim(cmbOptions.SelectedValue.ToString)
                    Case "Approved"
                        Call LoadCollateralItems("approved")
                    Case "Expired"
                        Call LoadCollateralItems("expired")
                    Case "Cancelled"
                        Call LoadCollateralItems("cancelled")
                    Case "Pending Approval"
                        Call LoadCollateralItems("pending")
                End Select

            End If
        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)

        End Try
        'End If
    End Sub
    Private Sub getStatus()
        Try

          
            strSQL = "select * from COLLATERAL_ITEMS join customer on COLLATERAL_ITEMS.Customernumber=customer.customer_number where collateralreference='" & Trim(lblsecref.Text) & "'"
           
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

          

            Do While drSQL.Read
                txtcreatedby.Text = Trim(drSQL.Item("createdby").ToString)
                txtstatus.Text = Trim(drSQL.Item("collateralCancelled").ToString)
                txtcustomer.Text = Trim(drSQL.Item("fullname").ToString)
                txtexpired.Text = Trim(drSQL.Item("expired").ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)

        End Try
    End Sub

    Private Sub Cancel()
        'If MessageBox.Show("Are you sure you want to cancel the collateral item? You cannot reverse this process", "Collateral", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms. _
        '          DialogResult.Yes Then
        Try
            If CollateralInUse(Trim(lblsecref.Text), 1) = False Or _
            CollateralInUse(Trim(lblsecref.Text), 2) = False Then 'Check if collateral has been assigned to a deal
                If txtexpired.Text = "N" Then

                    strSQL = "update COLLATERAL_ITEMS set collateralCancelled='Y',canncelledBy='" & Trim(Session("username")) & "',dateCancelled='" & CDate(Session("SysDate")) & "'," & _
                    " collateralapproved='N' where collateralReference='" & Trim(lblsecref.Text) & "'"
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader

                    MsgBox("Collateral Item Cancelled", MsgBoxStyle.Information, "Collateral")

                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()



                    'Log the event *****************************************************loggedUserLog formated for log
                    object_userlog.SendDataToLog(Session("username") & "COLT01" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Collateral Item Cancelled -  Reference : " & _
                    Trim(Trim(lblsecref.Text)) & _
                    " Customer : -" & Trim(txtcustomer.Text), Session("serverName"), Session("client"))
                    '************************END****************************************


                    Select Case Trim(cmbOptions.SelectedValue.ToString)
                        Case "Approved"
                            Call LoadCollateralItems("approved")
                        Case "Expired"
                            Call LoadCollateralItems("expired")
                        Case "Cancelled"
                            Call LoadCollateralItems("cancelled")
                        Case "Pending Approval"
                            Call LoadCollateralItems("pending")
                    End Select


                End If
            End If

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)

        End Try

        'End If
    End Sub
    Private Function CollateralInUse(ByVal id As String, ByVal num As Integer) As Boolean
        Dim res As Boolean
        Try

            Select Case num
                Case 1
                    strSQL = "select * from COUNTERPARTYSECURITY where tb_id='" & id & "' and status='Y'"
                Case 2
                    strSQL = "select * from ATTACHEDSECURITIES where tb_id='" & id & "' and matured='N'"
            End Select

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            If drSQL.HasRows = True Then
                res = True

                MsgBox("Collateral Item assigned to a running deal. Remove the assignment first.", MsgBoxStyle.Information, "Collateral")
            Else
                res = False
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try

        Return res

    End Function

    Private Sub Report()
        Try

            'Dim reportview As New Reportviewer
            'reportview.MdiParent = Me.MdiParent
            'reportview.ReportN = "CollateralList.rpt"
            'reportview.dealRefParam = Trim(lblsecref.Text)
            'reportview.Show()

        Catch ex As Exception

        End Try
    End Sub
    Private Sub DetailedReport()


        '     Dim reportview As New Reportviewer
        '    reportview.MdiParent = Me.MdiParent
        '    reportview.ReportN = "CollateralItemDetail.rpt"
        '    reportview.dealRefParam = Trim(lblsecref.Tex)
        '    reportview.Show()
        'Catch ex As Exception


    End Sub
End Class
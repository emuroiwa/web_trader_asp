Imports System.Data.SqlClient
Imports sys_ui


Public Class Rollmaster
    Inherits System.Web.UI.Page
    Private strSQLX As String
    Public cnSQLX As SqlConnection
    Public cmSQLX As SqlCommand
    Public drSQLX As SqlDataReader

    Private object_userlog As New usrlog.usrlog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call getDeals()
    End Sub

    Private Sub getDeals()
        Try
            strSQLX = "select distinct(dealref), freqopt,interestopt,freqdays from dealrollover "
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()


            'Dim x As String = ""
            'Dim y As String = ""
            'Select Case Trim(drSQLX.Item("interestopt").ToString)
            '    Case "NOA"
            '        x = "No Action on interest"
            '    Case "PAY"
            '        x = "Payout Interest On Rollover"
            '    Case "CAP"
            '        x = "Capitalise Interest On Rollover"
            'End Select

            'If (drSQLX.Item("freqopt").ToString).Equals(1) Then
            '    y = "Single Rollover"
            'Else
            '    y = "Recurring on frequency"
            'End If


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

        Catch ex As SqlException
                                  object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try
    End Sub


    Private Sub LoadRollOptions(ByVal dealref As String)
        Try
            strSQLX = "select * from dealrollover join rolloverhist on dealrollover.dealref=" & _
                      " rolloverhist.dealreference and dealrollover.rolloverdate=rolloverhist.datelastmaintained where dealref='" & dealref & "' "
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()
            GrdRollOptions.DataSource = drSQLX
            GrdRollOptions.DataBind()

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ex As SqlException
                                object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try

        Try
            Dim status As String = "Live"
            'validate username first
            strSQLX = "select * from deals_live join customer " & _
                     " on  deals_live.CustomerNumber =customer.Customer_Number " & _
                     " where dealreference = '" & dealref & "'"
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            If drSQLX.HasRows = False Then

                status = "Matured"

                drSQLX.Close()
                cnSQLX.Close()
                cmSQLX.Dispose()
                cnSQLX.Dispose()

                'validate username first
                strSQLX = "select * from deals_matured join customer " & _
                         " on deals_matured.CustomerNumber =customer.Customer_Number " & _
                         " where dealreference = '" & dealref & "'"

                cnSQLX = New SqlConnection(Session("ConnectionString"))
                cnSQLX.Open()
                cmSQLX = New SqlCommand(strSQLX, cnSQLX)
                drSQLX = cmSQLX.ExecuteReader()


            End If

            Do While drSQLX.Read
                lblReference.Text = drSQLX.Item("dealreference").ToString
                lblCustName.Text = drSQLX.Item("fullname").ToString
                lblAmount.Text = Format(CDec(drSQLX.Item("dealamount")), "###,###,###.00")
                lblMaturityAmount.Text = Format(CDec(drSQLX.Item("maturityamount")), "###,###,###.00")
                lblRate.Text = drSQLX.Item("interestrate")
                lblDiscountRate.Text = drSQLX.Item("discountrate")
                lblDateEntered.Text = Format(drSQLX.Item("startdate").ToString, "Short Date")
                lblMaturityDate.Text = Format(drSQLX.Item("maturitydate").ToString, "Short Date")
                lblTenor.Text = drSQLX.Item("tenor")
                lblTaxRate.Text = drSQLX.Item("taxrate")
                lblTaxamt.Text = Format(CDec(drSQLX.Item("taxamount")), "###,###,###.00")
                lblRemain.Text = drSQLX.Item("daystomaturity")
                lblAccrued.Text = Format(CDec(drSQLX.Item("intaccruedtodate")), "###,###,###.00")
                lblDealer.Text = drSQLX.Item("dealcapturer")
                lblDealStatus.Text = status

            Loop

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ez As NullReferenceException

        Catch ec As SqlException
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

        End Try

    End Sub
    Protected Sub GridDeals_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        If e.CommandName = "select" Then

            txtdealref.Text = e.Item.Cells(0).Value.ToString()
            'lblInfo.Text = txtdealref.Text
            '  MsgBox(txtdealref.Text)

            Call LoadRollOptions(txtdealref.Text)
        End If
    End Sub
End Class
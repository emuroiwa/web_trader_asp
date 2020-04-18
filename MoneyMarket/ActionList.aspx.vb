Imports System.Data.SqlClient

Public Class ActionList
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader

    Private object_userlog As New usrlog.usrlog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call getCurrencies()
        CalendarMaturities2.VisibleDate = CDate(Session("Sysdate"))
        CalendarMaturities2.SelectedDate = CDate(Session("Sysdate"))
    End Sub
    Protected Sub GrdDealsMaturity_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        If e.CommandName = "select" Then

            txtdealref.Text = e.Item.Cells(0).Value.ToString()
            'lblInfo.Text = txtdealref.Text
            'MsgBox(txtdealref.Text)

            Call getdetails(txtdealref.Text)
        End If
    End Sub
    Private Sub getdetails(ByVal dealref As String)
        ' grpDetails.Text = ""

        'Select deal details from database
        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Try
            'validate username first
            strSQLx = "select * from Deals_matured join customer " & _
                     " on Deals_matured.CustomerNumber =customer.Customer_Number " & _
                     " join dealtypes on Deals_matured.dealtype=dealtypes.deal_code " & _
                     " where dealreference = '" & Trim(dealref) & "'"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                lblReference.Text = Trim(drSQLx.Item("dealreference").ToString)
                lblCustName.Text = drSQLx.Item("fullname").ToString
                lblAmount.Text = Format(CDec(drSQLx.Item("dealamount")), "###,###,###.00")
                lblMaturityAmount.Text = Format(CDec(drSQLx.Item("maturityamount")), "###,###,###.00")
                lblRate.Text = drSQLx.Item("interestrate")
                lblDiscountRate.Text = drSQLx.Item("discountrate")
                lblDateEntered.Text = Format(drSQLx.Item("startdate").ToString, "Short Date")
                lblMaturityDate.Text = Format(drSQLx.Item("maturitydate").ToString, "Short Date")
                lblTenor.Text = drSQLx.Item("tenor")
                lblTaxRate.Text = drSQLx.Item("taxrate")
                lblTaxamt.Text = Format(CDec(drSQLx.Item("taxamount")), "###,###,###.00") 'Format((100 * CDec(drSQLx.Item("intaccruedtodate"))) / (100 - CDec(drSQLx.Item("taxrate"))) - CDec(drSQLx.Item("intaccruedtodate")), "###,###,###.00") 'Format(CDec(drSQLx.Item("taxamount")), "###,###,###.00")
                lblRemain.Text = drSQLx.Item("daystomaturity")
                lblAccrued.Text = Format(CDec(drSQLx.Item("intaccruedtodate")), "###,###,###.00")
                lblDealer.Text = drSQLx.Item("dealcapturer")
                lblintDaysbasis.Text = drSQLx.Item("intdaysbasis").ToString
                lbldealdescription.Text = drSQLx.Item("dealtypedescription").ToString

                lblCurrency.Text = Trim(drSQLx.Item("currency"))

                txtInstructions.Text = "Inception Instruction : " & vbCrLf
                txtInstructions.Text = txtInstructions.Text & Trim(drSQLx.Item("instruction")) & vbCrLf & vbCrLf
                txtInstructions.Text = txtInstructions.Text & "Maturity Instruction :" & vbCrLf
                txtInstructions.Text = txtInstructions.Text & Trim(drSQLx.Item("instructionmat"))
            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ez As NullReferenceException
            object_userlog.Msg(ez.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ez.Message, "error")

        Catch ec As SqlException
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

        End Try
    End Sub
    Private Sub getCurrencies()
        Try
            'strSQL = "Select currencycode from currencies where isbasecurrency='Yes'"
            strSQL = "Select currencycode from astval"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                cmbCurrency.Items.Add(Trim(drSQL.Item(0).ToString))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
                      object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")


        End Try
    End Sub
    Private Sub LoadDeals(ByVal ActionStatus As String, ByVal table As String, ByVal curr As String, ByVal selecteddate As String)
        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Try


            Select Case ActionStatus
                Case "Actioned"

                    strSQLx = "select * from " & table & "  join customer on " & _
                              " " & table & ".CustomerNumber=customer.Customer_Number where  dealreference" & _
                              " in(select dealref from actionlist) and currency='" & Trim(curr) & "' and maturitydate='" & selecteddate & "' and dealcancelled<>'R'"

                    If CheckShowAll.Checked = True Then
                        strSQLx = "select * from " & table & "  join customer on " & _
                         " " & table & ".CustomerNumber=customer.Customer_Number where  dealreference" & _
                         " in(select dealref from actionlist) and currency='" & Trim(curr) & "' and dealcancelled<>'R'"

                    End If


                Case "Pending"

                    strSQLx = "select * from " & table & "  join customer on " & _
                               " " & table & ".CustomerNumber=customer.Customer_Number where  dealreference" & _
                               " not in(select dealref from actionlist) and currency='" & Trim(curr) & "' and maturitydate='" & selecteddate & "' and dealcancelled<>'R'"

                    If CheckShowAll.Checked = True Then
                        strSQLx = "select * from " & table & "  join customer on " & _
                           " " & table & ".CustomerNumber=customer.Customer_Number where  dealreference" & _
                            " not in(select dealref from actionlist) and currency='" & Trim(curr) & "' and dealcancelled<>'R'"
                    End If


                Case Else

                    Exit Sub

            End Select

  


            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            GrdDealsMaturity.DataSource = drSQLx
            GrdDealsMaturity.DataBind()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch er As ArgumentException

        Catch ex As Exception

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")


        End Try

    End Sub

    Protected Sub CalendarMaturities2_SelectionChanged(sender As Object, e As EventArgs) Handles CalendarMaturities2.SelectionChanged
        LoadDeals(Trim(cmbActionStatus.Text), "deals_matured", Trim(cmbCurrency.Text), CalendarMaturities2.SelectedDate)
    End Sub

    Protected Sub cmbActionStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbActionStatus.SelectedIndexChanged
        LoadDeals(Trim(cmbActionStatus.Text), "deals_matured", Trim(cmbCurrency.Text), CalendarMaturities2.SelectedDate)
    End Sub

    Protected Sub cmbCurrency_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCurrency.SelectedIndexChanged
        LoadDeals(Trim(cmbActionStatus.Text), "deals_matured", Trim(cmbCurrency.Text), CalendarMaturities2.SelectedDate)
    End Sub

    Protected Sub CheckShowAll_CheckedChanged(sender As Object, e As EventArgs) Handles CheckShowAll.CheckedChanged
        LoadDeals(Trim(cmbActionStatus.Text), "deals_matured", Trim(cmbCurrency.Text), CalendarMaturities2.SelectedDate)

    End Sub
End Class
Imports System.Data.SqlClient

Public Class DealMaturities
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Public MaturityPeriods As Integer
    Private object_userlog As New usrlog.usrlog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            loadCurrencies()
            LoadCustomers()
        End If
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
    Private Sub LoadCustomers()

        Try
            strSQL = "select customer_Number,fullName from customer where frontoffice='Y' and len(customer_number)<=9 order by fullname"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    cmbCustomer.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
                Loop
            End If

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
    Private Sub CustomerName(ByVal number As String)

        Try
            strSQL = "select customer_Number,fullName from customer where frontoffice='Y' and len(customer_number)<=9 and customer_Number='" & number & "'  order by fullname"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    lblCustomerName.Text = Trim(drSQL.Item(1).ToString)
                    lblCustomerNumber.Text = Trim(drSQL.Item(0).ToString)
                    lblCustomerNameModal.Text = Trim(drSQL.Item(1).ToString)
                    lblCustomerNumberModal.Text = Trim(drSQL.Item(0).ToString)
                Loop
            End If

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
    Public Sub CustomerMaturityDeals()
        Try
            Dim count As Integer
            count = 0

            strSQL = "SELECT othercharacteristics,currency,dealreference,dealamount,maturityamount,interestrate,Tenor,rolloverdeal,StartDate,maturitydate," & _
                                                          "daystomaturity FROM deals_live where customernumber='" & cmbCustomer.SelectedValue.ToString & "' and currency='" & cmbCurrency.SelectedValue.ToString & "' and daystomaturity<=" & cmbDays.Text & ""

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            GridCustomerDeals.DataSource = drSQL
            GridCustomerDeals.DataBind()
            lblNumberOfDeals.Text = GridCustomerDeals.RecordCount
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            ' Close and Clean up objects
           
        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try
    End Sub

    Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        If Trim(cmbViewOpt.Text) = "Display on screen" Then
            loaddealsMaturing()
        End If

        If Trim(cmbViewOpt.SelectedValue.ToString) = "Printable Report" Then
            MaturityPeriods = Int(cmbDays.Text)
            Select Case Trim(cmbType.SelectedValue.ToString)
                Case "Deposit / Placement Deals"

                    Response.Redirect("ReportViewer.aspx?report=Maturities DepositsPlacements&days=" & MaturityPeriods & "&currency=" & cmbCurrency.SelectedValue.ToString)

                Case "Security Purchases"

                    Response.Redirect("ReportViewer.aspx?report=Maturities Purchases&days=" & MaturityPeriods & "&currency=" & cmbCurrency.SelectedValue.ToString)

                Case "Security Sells"

                    Response.Redirect("ReportViewer.aspx?report=Maturities Sells&days=" & MaturityPeriods & "&currency=" & cmbCurrency.SelectedValue.ToString)

            End Select


        End If
    End Sub
    Private Sub loaddealsMaturing()

        Dim res As Integer
        Dim FormatDataGrid As String
        ' MsgBox(cmbType.SelectedValue.ToString)

        Try

            If cmbType.SelectedValue.ToString = "Deposit / Placement Deals" Then

                strSQL = "select * from deals_live join customer on customer.customer_number=deals_live.customernumber" & _
                               " where  daystomaturity<=" & cmbDays.Text & "  and currency='" & Trim(cmbCurrency.SelectedValue.ToString) & "' and" & _
                               " (othercharacteristics='Basic Deposit' or othercharacteristics='Basic Loan' and currency='" & Trim(cmbCurrency.SelectedValue.ToString) & "')" & _
                                "  order by daystomaturity asc"

                grpMaturities.Text = "Deposit / Placement"
                FormatDataGrid = "FormatDepositsPlacements"
                lblPeriod.Text = "Maximum period of :" & cmbDays.Text & " Days"
            End If
            If cmbType.SelectedValue.ToString = "Security Purchases" Then


                strSQL = "select * from deals_live join customer on customer.customer_number=deals_live.customernumber " & _
                               " where  daystomaturity<=" & cmbDays.Text & " and othercharacteristics='Discount Purchase' " & _
                                "  and currency='" & Trim(cmbCurrency.SelectedValue.ToString) & "' order by daystomaturity asc"

                grpMaturities.Text = "Security Purchase"
                FormatDataGrid = "FormatPurchase"
                lblPeriod.Text = "Maximum period of :" & cmbDays.Text & " Days"

            End If
            If cmbType.SelectedValue.ToString = "Security Sells" Then

                strSQL = "select * from deals_live join customer on customer.customer_number=deals_live.customernumber" & _
                               "  where daystomaturity<=" & cmbDays.Text & " and othercharacteristics='Discount Sale' and currency='" & Trim(cmbCurrency.SelectedValue.ToString) & "' order by daystomaturity asc"

                grpMaturities.Text = "Security Sell"
                FormatDataGrid = "FormatSell"
                lblPeriod.Text = "Maximum period of :" & cmbDays.Text & " Days"
            End If


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            GridDeposit.DataSource = drSQL
            GridDeposit.DataBind()
            lblTotal.Text = "Number Of Records :" & GridDeposit.RecordCount
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            ' Close and Clean up objects

        Catch exc As Exception
            'Log the event *****************************************************
            object_userlog.Msg(exc.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & exc.Message & "DoUnclassified", "error")
            '************************END****************************************
            Exit Sub
        End Try
    End Sub

    Protected Sub btnGetDetails_Click(sender As Object, e As EventArgs) Handles btnGetDetails.Click
        Call CustomerMaturityDeals()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "openModal();", True)
    End Sub

    Protected Sub cmbCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCustomer.SelectedIndexChanged
        CustomerName(cmbCustomer.SelectedValue.ToString)

    End Sub
End Class
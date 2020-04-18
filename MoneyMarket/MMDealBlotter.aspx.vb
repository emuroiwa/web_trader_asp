Imports System.Data.SqlClient

Public Class MMDealBlotter
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private cnSQLx As SqlConnection
    Private strSQLx As String
    Private cmSQLx As SqlCommand
    Private drSQLx As SqlDataReader
    Private object_userlog As New usrlog.usrlog
    'Private dealtable As String
    'Dim variable As String = testvariable.Value

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Call getCurrencies()
            Call loaddealernames("1")

            Call IsFrontAuthoriser()

        End If
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
                cmbcurrency.Items.Add(Trim(drSQL.Item(0).ToString))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub

    Private Sub loaddealernames(ByVal x As String)
        cmbDealer.Items.Clear()

        'If the view all deals function is given then get all dealer names
        If CheckDealer() = True Then
            'if permission view all dealers is ok then load all dealer names
            Try
                If x = "1" Then
                    strSQLx = "select * from users  join logins on logins.user_id=users.user_id where user_department='Treasury Front Office'"
                End If

                If x = "2" Then
                    strSQLx = "select * from users  join logins on logins.user_id=users.user_id where user_department in('Treasury Front Office','Treasury Back Office')"
                End If

                cnSQLx = New SqlConnection(Session("ConnectionString"))
                cnSQLx.Open()
                cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                drSQLx = cmSQLx.ExecuteReader()

                Do While drSQLx.Read
                    cmbDealer.Items.Add(drSQLx.Item(0).ToString)
                Loop

                cmbDealer.Items.Add("All Dealers")

                ' Close and Clean up objects
                drSQLx.Close()
                cnSQLx.Close()
                cmSQLx.Dispose()
                cnSQLx.Dispose()

            Catch ex As SqlException
                MsgBox(ex.Message, MsgBoxStyle.Critical)
                'Log the event *****************************************************
                object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, Session("serverName"), Session("client"))
                '************************END****************************************
            End Try

        Else
            'MsgBox("" + Session("loggedUserLog"))

            'just load the logged user name
            cmbDealer.Items.Add(Session("loggedUserLog").ToString)

        End If

    End Sub
    'Check if user can view all dealer deals
    Private Function CheckDealer() As Boolean
        Dim viewall As Boolean = False
        Try
            strSQLx = "select function_id from user_functions where user_id = '" & Trim(Session("loggedUserLog")) & "' and function_id='OPT34'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            Do While drSQLx.Read
                viewall = True
            Loop
            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            Return viewall

            'catch any error
        Catch e As Exception
            MsgBox(e.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Function
    Public Function IsFrontAuthoriser()
        Dim res As String = ""

        Dim cnSQLX1 As SqlConnection
        Dim cmSQLX1 As SqlCommand
        Dim drSQLX1 As SqlDataReader
        Dim strSQLX1 As String

        Try

            strSQLX1 = "Select * from USER_FUNCTIONS where function_id='LEVEL0' and user_id='" & Trim(Session("loggedUserLog")) & "'"
            cnSQLX1 = New SqlConnection(Session("ConnectionString"))
            cnSQLX1.Open()
            cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
            drSQLX1 = cmSQLX1.ExecuteReader()

            If drSQLX1.HasRows = True Then
                CheckFrontAuthoriser.Checked = True
            Else
                CheckFrontAuthoriser.Checked = False
            End If


            ' Close and Clean up objects
            drSQLX1.Close()
            cnSQLX1.Close()
            cmSQLX1.Dispose()
            cnSQLX1.Dispose()

        Catch ex As Exception

        End Try
    End Function

    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles chkBackoffice.CheckedChanged
        If chkBackoffice.Checked = True Then
            loaddealernames("2")
        Else
            loaddealernames("1")
        End If
    End Sub

    Private Sub LoadDeals(ByVal dealername As String, ByVal table As String, ByVal AuthStatus As String, ByVal curr As String)
        'lstDealsDetails.VirtualMode = False
        'Dim img As Integer
        Try

            Select Case dealername
                Case "All Dealers"
                    If AuthStatus = "NA" Then
                        strSQL = "select * from " & table & "  join customer on " & _
                                  " " & table & ".CustomerNumber=customer.Customer_Number where  currency='" & Trim(curr) & "'  "

                    ElseIf AuthStatus = "CANCELLED" Then 'Cancelled deal
                        strSQL = "select * from " & table & "  join customer on " & _
                         " " & table & ".CustomerNumber=customer.Customer_Number where  currency='" & Trim(curr) & "' and dealcancelled='Y'  "

                    Else
                        strSQL = "select * from " & table & "  join customer on " & _
                                  " " & table & ".CustomerNumber=customer.Customer_Number where  authorisationstatus='" & AuthStatus & "' and currency='" & Trim(curr) & "'"

                    End If

                Case Else
                    If AuthStatus = "NA" Then
                        strSQL = "select * from " & table & "  join customer on " & _
                        " " & table & ".CustomerNumber=customer.Customer_Number where DealCapturer='" & dealername & "' and currency='" & Trim(curr) & "'"

                    ElseIf AuthStatus = "CANCELLED" Then 'Cancelled deal
                        strSQL = "select * from " & table & "  join customer on " & _
                                 " " & table & ".CustomerNumber=customer.Customer_Number where DealCapturer='" & dealername & "'and dealcancelled='Y' and currency='" & Trim(curr) & "' "


                    Else
                        strSQL = "select * from " & table & "  join customer on " & _
                                  " " & table & ".CustomerNumber=customer.Customer_Number where DealCapturer='" & dealername & "'and authorisationstatus='" & AuthStatus & "' and currency='" & Trim(curr) & "'"

                    End If
            End Select

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GrdDealsMM.DataSource = drSQL
            GrdDealsMM.DataBind()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

            'MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try
    End Sub
    Private Sub LoadFilter()
        'lstDeals.Items.Clear() 'clear the list items
        ''grpDetails.Visible = False
        'Expando4.Text = "Details"
        Try
            Select Case Trim(cmbDealStatus.Text)
                Case "Live"
                    If cmbDealer.SelectedIndex <> -1 Then
                        Session("dealtable") = "Deals_live"
                        Call LoadDeals(Trim(cmbDealer.Text), "Deals_live", "A", Trim(cmbcurrency.Text))
                    Else

                    End If

                Case "Matured"
                    If cmbDealer.SelectedIndex <> -1 Then
                        Session("dealtable") = "Deals_Matured"
                        Call LoadDeals(Trim(cmbDealer.Text), "Deals_Matured", "NA", Trim(cmbcurrency.Text))

                    Else

                    End If

                Case "Cancelled"
                    If cmbDealer.SelectedIndex <> -1 Then
                        Session("dealtable") = "AllDealsAll"
                        Call LoadDeals(Trim(cmbDealer.Text), Session("dealtable"), "CANCELLED", Trim(cmbcurrency.Text))
                    Else

                    End If

                Case "Authorisation Pending A"
                    If cmbDealer.SelectedIndex <> -1 Then
                        Call LoadDeals(Trim(cmbDealer.Text), "Deals_live", "B", Trim(cmbcurrency.Text))

                    Else

                    End If

                Case "Authorisation Pending B"
                    If cmbDealer.SelectedIndex <> -1 Then
                        Session("dealtable") = "Deals_live"
                        Call LoadDeals(Trim(cmbDealer.Text), "Deals_live", "V", Trim(cmbcurrency.Text))

                    Else

                    End If

                Case "Verification Pending"
                    If cmbDealer.SelectedIndex <> -1 Then

                        Session("dealtable") = "Deals_live"
                        If CheckFrontAuthoriser.Checked = True Then
                            Call LoadDeals(Trim(cmbDealer.Text), "Deals_live", "P", Trim(cmbcurrency.Text))
                            'Call LoadDeals(Trim(cmbDealer.Text), "Deals_Matured", "P", Trim(cmbcurrency.Text))
                        Else
                            Call LoadDeals(Trim(cmbDealer.Text), "Deals_live", "N", Trim(cmbcurrency.Text))
                            Call LoadDeals(Trim(cmbDealer.Text), "Deals_Matured", "N", Trim(cmbcurrency.Text))
                        End If
                    End If

                Case "Authorisation Declined"
                    If cmbDealer.SelectedIndex <> -1 Then
                        Session("dealtable") = "Deals_live"
                        Call LoadDeals(Trim(cmbDealer.Text), "Deals_live", "DA", Trim(cmbcurrency.Text))
                    Else

                    End If

                Case "Verification Declined"
                    If cmbDealer.SelectedIndex <> -1 Then
                        Session("dealtable") = "Deals_live"
                        Call LoadDeals(Trim(cmbDealer.Text), "Deals_live", "DV", Trim(cmbcurrency.Text))
                        Call LoadDeals(Trim(cmbDealer.Text), "Deals_Matured", "DV", Trim(cmbcurrency.Text))
                    Else

                    End If

            End Select

        Catch ex As NullReferenceException

        End Try
    End Sub

    Protected Sub cmbDealStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDealStatus.SelectedIndexChanged
        Call LoadFilter()
    End Sub

    Protected Sub cmbDealer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDealer.SelectedIndexChanged
        Call LoadFilter()
    End Sub

    Protected Sub cmbcurrency_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbcurrency.SelectedIndexChanged
        Call LoadFilter()
    End Sub

    Protected Sub CheckFrontAuthoriser_CheckedChanged(sender As Object, e As EventArgs) Handles CheckFrontAuthoriser.CheckedChanged

    End Sub



    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Dim variable As String = testvariable.Value
        'lblDealref.Text = variable
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

    End Sub


    'Protected Sub txtdealref_TextChanged(sender As Object, e As EventArgs) Handles txtdealref.TextChanged

    '    Call getdetails(txtdealref.Text)

    'End Sub
    Protected Sub GrdDealsMM_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call
        If e.CommandName = "select" Then
            Dim s As String = String.Empty
            's += "Row Index:" & e.Item.Index.ToString()
            's += "<br />Posted At:" & e.Item.Cells(1).Value.ToString()
            's += "<br />Posted By:" & e.Item.Cells(2).Value.ToString()
            's += "<br />Topic:" & e.Item.Cells(3).Value.ToString()
            txtdealref.Text = e.Item.Cells(1).Value.ToString()

            'lblInfo.Text = txtdealref.Text
            Call getdetails(txtdealref.Text)
            Call getLimitDetails(txtdealref.Text)
            Call GetCounterpartyLimitDetails(txtdealref.Text)
            Call GetAuthorisationInfo(txtdealref.Text)
            Call GetDealHistory(Trim(txtdealref.Text))
            getDealAccountDetails(txtdealref.Text)
            getDealInstructions(txtdealref.Text)
            getSecurityDetails(txtdealref.Text)
        End If

        If e.CommandName = "Accept" Then
            Call Verify(txtdealref.Text)
        End If
        
        If e.CommandName = "Cancel Deal" Then

        End If
        If e.CommandName = "Comment" Then

        End If
        If e.CommandName = "Export" Then

        End If
        If e.CommandName = "Print" Then

        End If


    End Sub
    Private Sub getdetails(ByVal dealref As String)
        lblDealCancelled.Text = ""
        lblOtherStatus.Text = ""
        'grpDetails.Text = ""
        lblOtherStatus.Visible = False
        lblDealCancelled.Visible = False
        lblreason.Visible = False




        'Select deal details from database
        Try

            If dealref = "M" Then
                Session("dealtable") = "deals_matured"
            ElseIf dealref = "c" Then
                Session("dealtable") = "AllDealsAll"
            End If
            'dealtable = "AllDealsAll"
            'validate username first
            strSQLx = "select * from " & Session("dealtable") & " join customer " & _
                     " on " & Session("dealtable") & ".CustomerNumber =customer.Customer_Number " & _
                     " join dealtypes on " & Session("dealtable") & ".dealtype=dealtypes.deal_code " & _
                     " where dealreference = '" & Trim(dealref) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                txtdealref.Text = drSQLx.Item("dealreference").ToString
                lblcustomer.Text = drSQLx.Item("fullname").ToString
                lblAmount.Text = Format(CDec(drSQLx.Item("dealamount")), "###,###,###.00")
                lblMaturityAm.Text = Format(CDec(drSQLx.Item("maturityamount")), "###,###,###.00")
                lblYieldRate.Text = drSQLx.Item("interestrate")
                lblDiscountRate.Text = drSQLx.Item("discountrate")
                lblStart.Text = Format(drSQLx.Item("startdate").ToString, "Short Date")
                lblMaturityDate.Text = Format(drSQLx.Item("maturitydate").ToString, "Short Date")
                lbltenor.Text = drSQLx.Item("tenor")
                lblTaxRate.Text = drSQLx.Item("taxrate")
                lblTaxAmount.Text = Format(CDec(drSQLx.Item("taxamount")), "###,###,###.00") 'Format((100 * CDec(drSQLx.Item("intaccruedtodate"))) / (100 - CDec(drSQLx.Item("taxrate"))) - CDec(drSQLx.Item("intaccruedtodate")), "###,###,###.00") 'Format(CDec(drSQLx.Item("taxamount")), "###,###,###.00")
                lblRemain.Text = drSQLx.Item("daystomaturity")
                lblAccrued.Text = Format(CDec(drSQLx.Item("intaccruedtodate")), "###,###,###.00")
                lblDealCapturer.Text = drSQLx.Item("dealcapturer")
                lblStatus.Text = cmbDealStatus.Text
                lblintDaysbasis.Text = drSQLx.Item("intdaysbasis").ToString
                'Expando4.Text = drSQLx.Item("dealtypedescription").ToString
                lblDealCapturer.Text = drSQLx.Item("dealcapturer").ToString
                'grpDetails.Visible = True
                lblcurrency.Text = Trim(drSQLx.Item("currency"))
                'lbldealtype.Text = Trim(drSQLx.Item("dealtype"))
                'lblCustomerNumber.Text = drSQLx.Item("customernumber").ToString
                'lblPortID.Text = drSQLx.Item("portfolioid").ToString

                If Trim(drSQLx.Item("dealcancelled").ToString) = "Y" Then
                    lblDealCancelled.Text = "--------CANCELLED DEAL-------"
                    lblDealCancelled.Visible = True
                ElseIf Session("dealtable") = "deals_matured" Then
                    lblDealCancelled.Text = "--------MATURED DEAL-------"
                    lblDealCancelled.Visible = True
                Else
                    lblDealCancelled.Visible = False
                    'lblstartdate.Visible = False
                    lblOtherStatus.Visible = False
                End If

            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


            'validate username first
            strSQLx = "select fccdealref from TDS_FCC_REF where tdsdealref='" & Trim(txtdealref.Text) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                If Trim(drSQLx.Item(0).ToString) Is DBNull.Value = False Then
                    fccRef.Text = drSQLx.Item(0).ToString
                Else
                    fccRef.Text = ""
                End If
            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            'Get the reason if the deal has been cancelled

            strSQLx = "select reason from canceldeals where dealreference = '" & Trim(txtdealref.Text) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                lblOtherStatus.Text = drSQLx.Item(0).ToString

            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            If lblOtherStatus.Text = "" Then
                lblOtherStatus.Visible = False
                'lblstartdate.Visible = False
                Call GetStatus(txtdealref.Text)
            Else
                lblOtherStatus.Visible = True
                'lblstartdate.Visible = True
                lblreason.Visible = True
            End If


        Catch ez As NullReferenceException

        Catch ec As SqlException

        End Try
        'MsgBox("" + txtdealref.Text)
    End Sub
    Private Sub GetStatus(ByVal ref As String)


        'dertermine the number of amendments
        Try
            strSQLx = "select changed,reason from earlymaturity where dealreference = '" & ref & "'  and recnumber > 1 order by recnumber desc"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLx, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader

            Do While drSQLX.Read
                lblDealCancelled.Text = Trim(drSQLx.Item("changed").ToString)
                lblOtherStatus.Text = Trim(drSQLx.Item("reason").ToString)
                lblDealCancelled.Visible = True
                lblOtherStatus.Visible = True
                lblstartdate.Visible = True
                Exit Do
            Loop
            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch er As Exception
            MsgBox(er.Message, MsgBoxStyle.Exclamation, "Get Status")
        End Try

    End Sub
    Private Sub getLimitDetails(ByVal dealref As String)
        ' 1 to indicate limits have been exceeded 
        ' 2 to indicate that transaction is within the limit
        '0 means Limit checking not implemented 
        Dim cnSQLx1 As SqlConnection
        Dim cmSQLx1 As SqlCommand
        Dim drSQLx1 As SqlDataReader
        Dim strSQLx1 As String

        Try
            'validate username first
            strSQLx1 = "select top(1) * from TRAN_LIMITS_DETAILS where dealreference='" & Trim(Trim(dealref)) & "' order by id desc  "
            cnSQLx1 = New SqlConnection(Session("ConnectionString"))
            cnSQLx1.Open()
            cmSQLx1 = New SqlCommand(strSQLx1, cnSQLx1)
            drSQLx1 = cmSQLx1.ExecuteReader()

            lblDealerNameLimit.Text = ""
            lblDealerLimit.Text = ""
            lblLimitStatus.Text = ""
            lblLimitPeriod.Text = ""
            lblLimitAuthoriser.Text = ""

            Do While drSQLx1.Read
                lblDealerNameLimit.Text = drSQLx1.Item("userid").ToString
                lblDealerLimit.Text = Format(CDbl(drSQLx1.Item("limitamount").ToString), "###,###.00")
                If drSQLx1.Item("limitStatus") = 1 Then
                    lblLimitStatus.Text = "Yes"
                ElseIf drSQLx1.Item("limitStatus") = 2 Then
                    lblLimitStatus.Text = "No"
                End If
                lblLimitAuthoriser.Text = drSQLx1.Item("authorisedby").ToString

                lblLimitPeriod.Text = GetLimitPeriod(Int(drSQLx1.Item("periodid")))
            Loop

            ' Close and Clean up objects
            drSQLx1.Close()
            cnSQLx1.Close()
            cmSQLx1.Dispose()
            cnSQLx1.Dispose()

        Catch ez As NullReferenceException

        Catch ec As SqlException

        End Try
    End Sub
    Private Function GetLimitPeriod(ByVal id As Integer) As String
        Dim x As String = ""
        Try
            'validate username first
            strSQLx = "select * from SYSLIMITSPERIODS where periodID=" & id & ""
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                x = drSQLx.Item("periodStart").ToString & " to " & drSQLx.Item("periodEnd").ToString & " days"
            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ez As NullReferenceException

        Catch ec As SqlException

        End Try

        Return x

    End Function
    Private Sub GetCounterpartyLimitDetails(ByVal dealref As String)
        ' 1 to indicate limits have been exceeded 
        ' 2 to indicate that transaction is within the limit
        '0 means Limit checking not implemented 
        Dim cnSQLx1 As SqlConnection
        Dim cmSQLx1 As SqlCommand
        Dim drSQLx1 As SqlDataReader
        Dim strSQLx1 As String

        Try
            'validate username first
            strSQLx1 = "select top(1) * from COUNTERPTY_LIMITS_DETAILS where dealreference='" & Trim(Trim(dealref)) & "' order by id desc  "
            cnSQLx1 = New SqlConnection(Session("ConnectionString"))
            cnSQLx1.Open()
            cmSQLx1 = New SqlCommand(strSQLx1, cnSQLx1)
            drSQLx1 = cmSQLx1.ExecuteReader()

            lmtAuthCounterparty.Text = ""
            lmtExceededCounterparty.Text = ""
            lmtCounterparty.Text = ""
            lmtDealerCounterparty.Text = ""
            lmtDealerLimitCounterparty.Text = ""

            Do While drSQLx1.Read
                lmtDealerCounterparty.Text = drSQLx1.Item("userid").ToString
                lmtDealerLimitCounterparty.Text = Format(CDbl(drSQLx1.Item("limitamount").ToString), "###,###.00")
                If drSQLx1.Item("limitStatus") = 1 Then
                    lmtExceededCounterparty.Text = "Yes"
                ElseIf drSQLx1.Item("limitStatus") = 2 Then
                    lmtExceededCounterparty.Text = "No"
                End If
                lmtAuthCounterparty.Text = drSQLx1.Item("authorisedby").ToString

                lmtCounterparty.Text = Format(CDbl(drSQLx1.Item("limitamount").ToString), "###,###.00")
            Loop

            ' Close and Clean up objects
            drSQLx1.Close()
            cnSQLx1.Close()
            cmSQLx1.Dispose()
            cnSQLx1.Dispose()

        Catch ez As NullReferenceException

        Catch ec As SqlException

        End Try
    End Sub
    Private Sub GetAuthorisationInfo(ByVal dealref As String)
        txtFront1.Text = ""
        txtFront2.Text = ""
        txtFrontComment.Text = ""
        txtBack1.Text = ""
        txtBack2.Text = ""

        Try
            strSQL = "select * from [DEALAUTHORISATIONS] where dealreference='" & Trim(dealref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                txtFront1.Text = Trim(drSQL.Item("verify1").ToString)
                txtFront2.Text = Trim(drSQL.Item("verify2").ToString)
                txtFrontComment.Text = Trim(drSQL.Item("verify1comment").ToString)
                txtBack1.Text = Trim(drSQL.Item("authorise1").ToString)
                txtBack2.Text = Trim(drSQL.Item("authorise2").ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As NullReferenceException
        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try
    End Sub


    Private Sub GetDealHistory(dealref As String)
        Try
            'validate username first
            strSQL = "select * from earlymaturity where dealreference='" & dealref & "' order by recnumber asc"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GrdEvents.DataSource = drSQL
            GrdEvents.DataBind()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

            'MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try
    End Sub
    Private Sub getDealAccountDetails(ByVal dealref As String)
        Try
            strSQLx = "select * from  SETTLEDETAILS where dealreference='" & dealref & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            lblIAccrualCredit.Text = ""
            lblIAccrualDebit.Text = ""
            lblInceptionCreditAcc.Text = ""
            lblInceptionDebitAcc.Text = ""
            lbllMaturityInterest.Text = ""


            Do While drSQLx.Read
                lblIAccrualCredit.Text = Trim(drSQLx.Item("ethixcreditaccrual").ToString)
                lblIAccrualDebit.Text = Trim(drSQLx.Item("ethixdebitaccrual").ToString)
                lblInceptionCreditAcc.Text = Trim(drSQLx.Item("ethixcreditaccount").ToString)
                lblInceptionDebitAcc.Text = Trim(drSQLx.Item("ethixdebitaccount").ToString)
                lbllMaturityInterest.Text = Trim(drSQLx.Item("ethixinterestaccount").ToString)
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


        Catch ex As Exception

        End Try
    End Sub
    Private Sub getDealInstructions(ByVal dealref As String)
        Try
            strSQLx = "select instruction,instructionmat from alldealsall where dealreference='" & Trim(dealref) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                txtStartInstr.Text = Trim(drSQLx.Item("instruction").ToString)
                txtEnd.Text = Trim(drSQLx.Item("instructionmat").ToString)
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            strSQLx = "select actiondetail from ACTIONLIST where dealref='" & Trim(dealref) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                txtFinal.Text = Trim(drSQLx.Item("actiondetail").ToString)
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


        Catch ex As Exception

        End Try
    End Sub
    Private Sub getSecurityDetails(ByVal dealref As String)
        'Is it a secured deposit
        If IsSecured(Trim(dealref)) = True Then
            GetSecurityDetailsDeal(txtdealref.Text)
            Exit Sub
        ElseIf NotSecuredOrSecurity(txtdealref.Text) = True Then
            'Deal is a security Purchase or sell
            GetSecurityDetailsSecurity(txtdealref.Text)
        Else
            txtSecurityDetails.Text = vbCrLf & "**********Deal has no security information***********" & "<br>" & vbCrLf
        End If
    End Sub
    Public Function IsSecured(ByVal ref As String) As Boolean
        Dim x As Boolean = False

        Try
            strSQLx = "select securityrequired from alldealsall where dealreference='" & Trim(ref) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                If Trim(drSQLx.Item(0).ToString) = "Y" Then
                    x = True
                Else
                    x = False
                End If
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            Return x

        Catch ex As Exception

        End Try
    End Function
    Private Sub GetSecurityDetailsDeal(ByVal dealref As String)
        Try
            strSQLx = "select * from attachedSecurities join COLLATERAL_ITEMS on COLLATERAL_ITEMS.collateralReference=" & _
            "attachedSecurities.TB_ID where dealreferencedeal='" & Trim(dealref) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()


            Do While drSQLx.Read
                txtSecurityDetails.Text = txtSecurityDetails.Text & _
                "Security Reference : - " & drSQLx.Item("collateralExtReference").ToString & "<br>" & vbCrLf & _
                "Security Description : - " & drSQLx.Item("collateralDescription").ToString & "<br>" & vbCrLf
                txtSecurityDetails.Text = txtSecurityDetails.Text & _
                "Secured Value: - " & Format(CDbl(drSQLx.Item("securityamount")), "###,###,###.00") & vbCrLf

                txtSecurityDetails.Text = txtSecurityDetails.Text & "<br>" & vbCrLf & _
               "**********End of Data**********" & vbCrLf
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


            strSQLx = "select * from COUNTERPARTYSECURITY join COLLATERAL_ITEMS on COLLATERAL_ITEMS.collateralReference=" & _
            "COUNTERPARTYSECURITY.TB_ID where dealreference='" & Trim(dealref) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()


            Do While drSQLx.Read
                txtSecurityDetails.Text = txtSecurityDetails.Text & _
                "Security Reference : - " & drSQLx.Item("collateralExtReference").ToString & vbCrLf & _
                "Security Description : - " & drSQLx.Item("collateralDescription").ToString & vbCrLf
                txtSecurityDetails.Text = txtSecurityDetails.Text & _
                "Secured Value: - " & Format(CDbl(drSQLx.Item("amount")), "###,###,###.00") & vbCrLf

                txtSecurityDetails.Text = txtSecurityDetails.Text & vbCrLf & _
               "**********End of Data************" & vbCrLf
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


        Catch ex As Exception

        End Try
    End Sub
    Private Sub GetSecurityDetailsSecurity(ByVal dealref As String)
        Try
            strSQLx = "select * from alldealsall where dealreference='" & Trim(dealref) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()


            Do While drSQLx.Read
                If Trim(drSQLx.Item("othercharacteristics")) = "Discount Purchase" Then
                    txtSecurityDetails.Text = vbCrLf & "**********Deal is a Security Purchase**********" & "<br>" & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & _
                    "Security Reference : " & Trim(drSQLx.Item("tb_id").ToString) & "<br>" & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & "<br>" & vbCrLf & _
                   "********End of Data*******" & vbCrLf

                ElseIf Trim(drSQLx.Item("othercharacteristics")) = "Discount Sale" Then
                    txtSecurityDetails.Text = vbCrLf & "***********Deal is a Security Sell*********" & "<br>" & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & _
                    "Security Deal Reference : " & Trim(drSQLx.Item("purchaseref").ToString) & "<br>" & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & "<br>" & vbCrLf & _
                    "Security Reference : " & GetSecurityRef(Trim(drSQLx.Item("purchaseref").ToString)) & "<br>" & vbCrLf
                    txtSecurityDetails.Text = txtSecurityDetails.Text & "<br>" & vbCrLf & _
                   "********End of Data*******" & "<br>" & vbCrLf
                End If
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ex As Exception

        End Try
    End Sub
    Private Function GetSecurityRef(ByVal ref) As String
        Dim x As String = ""

        Try
            strSQL = "select tb_id from alldealsall where dealreference='" & Trim(ref) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = drSQL.Item(0).ToString
            Loop

            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            Return x

        Catch ex As Exception

        End Try
    End Function
    Private Function NotSecuredOrSecurity(ByVal dealref As String) As Boolean
        Dim x As Boolean = False

        Try
            strSQLx = "select securityrequired,othercharacteristics from alldealsall where dealreference='" & Trim(dealref) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                If Trim(drSQLx.Item(0).ToString) = "N" And Trim(drSQLx.Item(1).ToString) = "Basic Loan" Or Trim(drSQLx.Item(1).ToString) = "Basic Deposit" Then
                    x = False
                Else
                    x = True
                End If
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            Return x

        Catch ex As Exception

        End Try
    End Function
    Private Sub Verify(ByRef dealref As String)
        'check if the dealer is not about to verify their own deals
        If Trim(lblDealCapturer.Text).Equals(Trim(Session("username"))) Then
            MsgBox("You cannot verify deals you captured.", MsgBoxStyle.Exclamation, "Verify Deals")
            Exit Sub
        End If

        'check if the dealer is not about to verify their own deals - Actioned
        If Trim(ActionedBy(Trim(dealref))).Equals(Trim(Session("username"))) Then
            MsgBox("You cannot verify deals you Actioned.", MsgBoxStyle.Exclamation, "Verify Deals")
            Exit Sub
        End If

        'If MsgBox("Confirm you want to accept deal verification for this deal: " & Trim(dealref), "Verify Deal", MsgBoxStyle.OkCancel) = MsgBoxResult.Yes Then
        '    'If MessageBox.Show("Confirm you want to accept deal verification for this deal: " & Trim(dealref), "Verify Deal", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
        '    Exit Sub
        'End If




        If getVerificationStatus(Trim(dealref)) = "P" Then

            Try

                If CheckVerificationStatus(Trim(dealref)) = True Then
                    MsgBox("Someone else has already verified the deal", MsgBoxStyle.Exclamation, "Verification")
                    Exit Sub
                End If


                'if front office is implemented check deal status should be N after this authorisation and change the status to P

                If Session("dealtable") = "Deals_live" Then

                    strSQLx = "begin tran x" & _
                              " update DEALAUTHORISATIONS set verify2='" & Trim(Session("username")) & "' where dealreference='" & Trim(dealref) & "' and seq=" & GetLastSeq(Trim(dealref), Trim(lblDealCapturer.Text)) & " " & _
                              " update deals_live set authorisationstatus='V', verifiedby='" & Trim(Session("username")) & "' where dealreference='" & Trim(dealref) & "' " & _
                              " update securitiesconfirmations set authorisationstatus='V' , verifiedby='" & Trim(Session("username")) & "'where dealreference='" & Trim(dealref) & "' " & _
                              " commit tran x"
                Else
                    strSQLx = " update deals_matured set authorisationstatus='V', FundsStatus='" & Trim(Session("username")) & "' where dealreference='" & Trim(dealref) & "' "

                End If

                cnSQLx = New SqlConnection(Session("ConnectionString"))
                cnSQLx.Open()
                cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                drSQLx = cmSQLx.ExecuteReader()

                drSQLx.Close()
                cnSQLx.Close()
                cmSQLx.Dispose()
                cnSQLx.Dispose()







            Catch ex As NullReferenceException
                MsgBox("Select a deal that you want to verify.", MsgBoxStyle.Information, "Deal Verification")

            Catch eb As Exception
                MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
            End Try

        Else
            MsgBox("Deal already verified", MsgBoxStyle.Information, "Verification")

        End If
        MsgBox("Deal verified", MsgBoxStyle.Information, "Verification")
        Call LoadFilter()

    End Sub
    Public Function GetLastSeq(ByVal ref As String, dealer As String) As Integer
        Dim x As Integer
        Dim cnSQLX1 As SqlConnection
        Dim cmSQLX1 As SqlCommand
        Dim drSQLX1 As SqlDataReader
        Dim strSQLX1 As String

        Try

            strSQLX1 = "if not exists (select * from DEALAUTHORISATIONS where dealreference='" & ref & "')" & _
                     " insert into DEALAUTHORISATIONS values('" & ref & "','','','','','" & CDate(Session("SysDate")) & "','" & dealer & "',0,'')"


            cnSQLX1 = New SqlConnection(Session("ConnectionString"))
            cnSQLX1.Open()
            cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
            drSQLX1 = cmSQLX1.ExecuteReader()

            ' Close and Clean up objects
            drSQLX1.Close()
            cnSQLX1.Close()
            cmSQLX1.Dispose()
            cnSQLX1.Dispose()

            strSQLX1 = "select max(seq) from DEALAUTHORISATIONS where dealreference='" & ref & "'"

            cnSQLX1 = New SqlConnection(Session("ConnectionString"))
            cnSQLX1.Open()
            cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
            drSQLX1 = cmSQLX1.ExecuteReader()

            Do While drSQLX1.Read
                x = drSQLX1.Item(0)
                Exit Do
            Loop

            ' Close and Clean up objects
            drSQLX1.Close()
            cnSQLX1.Close()
            cmSQLX1.Dispose()
            cnSQLX1.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "GetLastSeq")
            'Log the event *****************************************************
            'Send(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "CheckParam")
            '************************END****************************************
        End Try

        Return x

    End Function
    'Check if someone else verified the deal whilst waiting to authorise
    Public Function CheckVerificationStatus(ByVal dealref As String) As Boolean
        Dim x As Boolean

        Try
            strSQL = "select AuthorisationStatus from deals_live where dealreference='" & Trim(dealref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                If Trim(drSQL.Item(0).ToString) = "V" Or Trim(drSQL.Item(0).ToString) = "DV" Then
                    x = True
                Else
                    x = False
                End If
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As NullReferenceException

        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try

        Return x

    End Function
    Private Function getVerificationStatus(ByVal dealref As String) As String
        Dim x As String = ""
        Try
            strSQL = "select authorisationstatus from alldealsall where dealreference='" & Trim(dealref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = Trim(drSQL.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As NullReferenceException
        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try

        Return x
    End Function
    Private Function ActionedBy(ByVal dealref As String) As String
        Dim x As String = ""
        Try
            strSQL = "select dealer from ACTIONLIST where dealref='" & Trim(dealref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = Trim(drSQL.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As NullReferenceException
        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try

        Return x
    End Function
End Class
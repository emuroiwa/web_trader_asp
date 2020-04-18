Imports System.Data.SqlClient
Imports sys_ui
Public Class AddSecurityItem
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
    Private CusDownloadStart As Threading.Thread

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call LoadCustomers()
            Call loadCollateralLocation()
            Call loadCollateralTypes()
            Call GetBaseCurrency()

            'Start thread to run customer download
            CusDownloadStart = New Threading.Thread(AddressOf LoadCustomers)
            CusDownloadStart.Start()
        End If
    End Sub



    Protected Sub cmbcustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbcustomer.SelectedIndexChanged

    End Sub

    Protected Sub txtvalue_TextChanged(sender As Object, e As EventArgs) Handles txtvalue.TextChanged
        lblBankValuation.Text = (Format(CDbl(BankValuation(Trim(cmbTypeDesc.SelectedValue.ToString))), "###,###.00"))
    End Sub

    Protected Sub cmbTypeDesc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTypeDesc.SelectedIndexChanged
        On Error Resume Next

    End Sub
    Private Sub LoadCustomers()


        'listCustomers.Columns(1).TextAlign = HorizontalAlignment.Left
        Try
            strSQL = "select customer_Number,fullName from customer where frontoffice='Y' and len(customer_number)<=9 order by fullname"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            'listCustomers.Invoke(CType(AddressOf listCustomersClear, MethodInvoker))
            'lstnumbers.Invoke(CType(AddressOf lstnumbersclear, MethodInvoker))

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    'txtCustomerNameLoan.Items.Add(Trim(dbinfo.drSQL.Item(1).ToString) + " " + Trim(dbinfo.drSQL.Item(0).ToString))
                    cmbcustomer.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
                Loop
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            '' CusDownloadStart.Suspend()

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Function BankValuation(ByVal collartID As String) As Decimal
        Dim BnkValue As Decimal

        Try
            strSQL = "select CollateralBankValuation from COLL_COLLATERAL_TYPES where collateralID='" & collartID & "' "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                BnkValue = (CDbl(drSQL.Item("CollateralBankValuation").ToString) / 100) * CDbl(txtValue.Text)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ec As Exception
            'MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try

        Return BnkValue

    End Function

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        cmbcustomer.Text = ""
        lblCollateralReference.Text = ""
        btnSave.Enabled = True
        btnNew.Enabled = False
        dtExpiry.Text = CDate(Session("SysDate"))
        txtUserReference.Text = ""
        txtvalue.Text = ""
        lblBankValuation.Text = ""
        txtAdditionalInfo.Text = ""
    End Sub
    Private Sub loadCollateralTypes()
        Try
            strSQL = "select * from COLL_COLLATERAL_TYPES"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            cmbTypeDesc.Items.Clear()

            Do While drSQL.Read
                'cmbType.Items.Add(Trim(drSQL.Item(0).ToString))
                cmbTypeDesc.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub

    Private Sub loadCollateralLocation()
        Try
            strSQL = "select * from COLL_COLLATERAL_LOCATION"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            cmbLocationDesc.Items.Clear()

            Do While drSQL.Read
                'cmbLocation.Items.Add(Trim(drSQL.Item(0).ToString))
                cmbLocationDesc.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            txtcustnumber.Text= cmbcustomer.SelectedValue.ToString
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub
    Private Sub GetBaseCurrency()
        Try
            strSQL = "select currencycode from CURRENCIES where isfxbasecurrency='Yes'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                lblCurrency.Text = (Trim(drSQL.Item(0).ToString))

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub
    Private Function GetCollateralReference() As String
        Dim DealRefLength As String = ""
        Dim strLen1, strLen2 As Integer
        Dim num As Integer

        Try
            strSQL = "select refs from COLLATERAL_REFS "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    num = Int(drSQL.Item("refs").ToString)
                Loop
            Else
                num = 0
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


            Dim mnth As String = DatePart(DateInterval.Month, CDate(Session("SysDate")))
            If Len(mnth) = 1 Then
                mnth = "0" & mnth
            End If

            DealRefLength = "000"
            strLen1 = Len(DealRefLength)
            num = Int(num) + 1
            strLen2 = Len(num.ToString)
            If strLen1 > strLen2 Then
                DealRefLength = Mid(DealRefLength, 1, strLen1 - strLen2)
                DealRefLength = "COLL" & DatePart(DateInterval.Year, CDate(Session("SysDate"))) & mnth & DealRefLength & num
            Else
                DealRefLength = "COLL" & DatePart(DateInterval.Year, CDate(Session("SysDate"))) & mnth & num
            End If




            strSQL = "update COLLATERAL_REFS set refs=refs+1"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try

        Return DealRefLength
    End Function

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("index.aspx")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Trim(cmbcustomer.SelectedValue.ToString) = "" Then
            MsgBox("Select a customer for whom the collateral is being created", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If Trim(cmbTypeDesc.Text) = "" Then
            MsgBox("Select the collateral type", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If Trim(cmbLocationDesc.Text) = "" Then
            MsgBox("Select the collateral location", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If CDate(dtExpiry.Text) < CDate(Session("SysDate")) Then
            MsgBox("Expiry date of collateral cannot be less than today", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If Trim(txtUserReference.Text) = "" Then
            MsgBox("Enter a reference for the collateral item", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If Trim(txtDesc.Text) = "" Then
            MsgBox("Enter a Description for the collateral item", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If CDbl(txtvalue.Text) < 0 Then
            MsgBox("Collateral Value cannot be less than 0", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If Trim(cmbAssignment.Text) = "" Then
            MsgBox("Select Collateral assignment option", MsgBoxStyle.Exclamation)
            Exit Sub
        End If



        Try
            Dim CollartReference As String = GetCollateralReference() ' Get reference for collateralItem
            lblBankValuation.Text = (Format(CDbl(BankValuation(Trim(cmbTypeDesc.SelectedValue.ToString))), "###,###.00"))

            'Get Reference
            lblCollateralReference.Text = CollartReference
        Catch ex As Exception

        End Try


        MsgBox("" + cmbcustomer.SelectedValue.ToString)

        'Save the record
        Try
            strSQL = "insert into COLLATERAL_ITEMS values('" & Trim(cmbcustomer.SelectedValue.ToString) & "','" & Trim(cmbTypeDesc.SelectedValue.ToString) & "','" & Trim(cmbLocationDesc.SelectedValue.ToString) & "','" & _
            Trim(lblCollateralReference.Text) & "','" & Trim(txtDesc.Text) & "','" & Trim(cmbAssignment.Text) & "','" & CDate(dtExpiry.Text) & "','" & _
            Trim(txtUserReference.Text) & "','" & _
            Trim(lblCurrency.Text) & "','N'," & CDbl(txtvalue.Text) & "," & CDbl(lblBankValuation.Text) & ",'" & Trim(txtAdditionalInfo.Text) & "','" & _
            Trim(Session("username")) & "','','" & CDate(Session("SysDate")) & "','1900 -01 -01 ','N','N','N','1900-01-01')"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            MsgBox("Collateral Item added. Reference number is - " & Trim(lblCollateralReference.Text), MsgBoxStyle.Information, "Collateral")


            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


            'Log the event *****************************************************loggedUserLog formated for log
            object_userlog.SendDataToLog(Session("username") & "COLT01" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Collateral Item Added -  Reference : " & Trim(lblCollateralReference.Text) & _
            " Customer : -" & Trim(cmbcustomer.SelectedValue.ToString & " Bank Valuation : - " & CDbl(lblBankValuation.Text)) _
             , Session("serverName"), Session("client"))
            '************************END****************************************


            btnSave.Enabled = False

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try


    End Sub

End Class
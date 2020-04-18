Imports System.Data.SqlClient

Public Class MaintainSecurityTypes
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call loadCollateralTypes()
    End Sub
    Protected Sub GrdTypes_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call

        If e.CommandName = "Edit" Then
            Dim s As String = String.Empty
            txtID.Text = e.Item.Cells(1).Value.ToString()
            txtDescription.Text = e.Item.Cells(2).Value.ToString()
            txtBankValuation.Text = e.Item.Cells(3).Value.ToString()
            txtloan.text = e.Item.Cells(4).Value.ToString()
            txtdeposit.Text = e.Item.Cells(5).Value.ToString()
            Call EditType(txtID.Text, txtDescription.Text, txtBankValuation.Text)

        End If
        If e.CommandName = "Delete" Then
            Dim s As String = String.Empty
            txtID.Text = e.Item.Cells(1).Value.ToString()
            txtDescription.Text = e.Item.Cells(2).Value.ToString()
            txtBankValuation.Text = e.Item.Cells(3).Value.ToString()
            txtloan.Text = e.Item.Cells(4).Value.ToString()
            txtdeposit.Text = e.Item.Cells(5).Value.ToString()
            Call DeleteType(txtID.Text, txtDescription.Text, txtBankValuation.Text)
        End If

    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        txtID.Enabled = True
        txtID.Text = ""
        txtDescription.Text = ""
    End Sub
    Private Sub loadCollateralTypes()
        Try

            Dim Deposit, Loan As String

            strSQL = "select * from COLL_COLLATERAL_TYPES"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GrdTypes.DataSource = drSQL
            GrdTypes.DataBind()
            'lstCollateralTypes.Items.Clear()

            Do While drSQL.Read

                'Dim itmx As New ListViewItem(Trim(drSQL.Item(0).ToString))
                'itmx.SubItems.Add(Trim(drSQL.Item(1).ToString))
                'itmx.SubItems.Add(Trim(drSQL.Item(2).ToString))

                If Trim(drSQL.Item("SecureLoan").ToString) = "Y" Then
                    Loan = "Y"
                Else
                    Loan = "N"
                End If

                If Trim(drSQL.Item("SecureDeposit").ToString) = "Y" Then
                    Deposit = "Y"
                Else
                    Deposit = "N"
                End If

                'itmx.SubItems.Add(Loan)
                'itmx.SubItems.Add(Deposit)


                'lstCollateralTypes.Items.Add(itmx)

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
    Private Sub EditType(ByVal TypeID As String, ByVal Typedesc As String, ByVal value As String)
        Try

            txtID.Enabled = False
            txtDescription.Text = Trim(Typedesc)
            txtID.Text = Trim(TypeID)
            txtBankValuation.Text = value

            If Trim(txtloan.Text) = "Y" Then
                CheckBoxLoans.Checked = True
            Else
                CheckBoxLoans.Checked = False
            End If

            If Trim(txtdeposit.Text) = "Y" Then
                CheckBoxDeposit.Checked = True
            Else
                CheckBoxDeposit.Checked = False
            End If

        Catch ex As Exception

        End Try
    End Sub
    Private Sub DeleteType(ByVal TypeID As String, ByVal Typedesc As String, ByVal value As String)
        'Call confirm()
        Try

            strSQL = " delete COLL_COLLATERAL_TYPES where collateralID='" & Trim(TypeID) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            MsgBox("Collataral type deleted", MsgBoxStyle.Information)

            txtID.Enabled = True
            txtID.Text = ""
            txtDescription.Text = ""
            txtBankValuation.Text = ""

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'Log the event *****************************************************loggedUserLog formated for log
            object_userlog.SendDataToLog(Session("username") & "COLT01" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Collateral type deleted - ID : " & _
            Trim(TypeID) & " Collateral Description : " & Trim(Trim(Typedesc)) _
             , Session("serverName"), Session("client"))
            '************************END****************************************


            Call loadCollateralTypes()

        Catch ex As SqlException
            ' MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
        'End If
    End Sub

    Protected Sub btnSave0_Click(sender As Object, e As EventArgs) Handles btnSave0.Click
        Dim Loans, Deposits As String
        Try
            If txtID.Text = "" Then
                MsgBox("Please enter a unique ID for the collateral type upto 30 characters", MsgBoxStyle.Information)
                Exit Sub
            End If

            If txtDescription.Text = "" Then
                MsgBox("Please enter a description for the collateral type.", MsgBoxStyle.Information)
                Exit Sub
            End If


            If txtBankValuation.Text = "" Then
                MsgBox("Please the bank valuation rate for the collateral type.", MsgBoxStyle.Information)
                Exit Sub

            Else
                If CDbl(txtBankValuation.Text) > 100 Then
                    MsgBox("The bank valuation rate for the collateral is greater than 100%.", MsgBoxStyle.Information)
                    'Exit Sub
                End If
            End If

            If CheckBoxDeposit.Checked = True Then
                Deposits = "Y"
            Else
                Deposits = "N"
            End If

            If CheckBoxLoans.Checked = True Then
                Loans = "Y"
            Else
                Loans = "N"
            End If

            strSQL = " begin tran X" & _
                     " if exists (select * from COLL_COLLATERAL_TYPES where collateralID='" & Trim(txtID.Text) & "') " & _
                     " update COLL_COLLATERAL_TYPES set CollateralDescription='" & Trim(txtDescription.Text) & "',CollateralBankValuation=" & CDbl(txtBankValuation.Text) & _
                     " where" & _
                     " collateralID='" & Trim(txtID.Text) & "'" & _
                     " else" & _
                     " insert COLL_COLLATERAL_TYPES values('" & Trim(txtID.Text) & "','" & Trim(txtDescription.Text) & "','" & CDbl(txtBankValuation.Text) & "','" & Loans _
                     & "','" & Deposits & "')" & _
                     " commit tran X"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            MsgBox("Collataral type saved", MsgBoxStyle.Information)


            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'Log the event *****************************************************loggedUserLog formated for log
            object_userlog.SendDataToLog(Session("username") & "COLT01" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Collateral Type Added -  ID : " & Trim(txtID.Text) & _
            " Collateral Description : -" & Trim(txtDescription.Text & " Bank Valuation : - " & CDbl(txtBankValuation.Text)) _
             , Session("serverName"), Session("client"))
            '************************END****************************************

            txtID.Enabled = True
            txtID.Text = ""
            txtDescription.Text = ""
            txtBankValuation.Text = ""


            Call loadCollateralTypes()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub
    'Private Sub confirm()


    '    Dim CSM As ClientScriptManager = Page.ClientScript
    '    If Not ReturnValue() Then
    '        Dim strconfirm As String = "<script>if(!window.confirm(' Are u sure u want to delete\nPress Cancel to Stop')){window.location.reload()'}</script>"
    '        CSM.RegisterClientScriptBlock(Me.[GetType](), "Confirm", strconfirm, False)
    '    End If
    'End Sub
    'Private Function ReturnValue() As Boolean
    '    Return False
    'End Function
End Class
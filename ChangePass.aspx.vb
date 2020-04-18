Imports System.Data.SqlClient
Imports sys_ui


Public Class ChangePass
    Inherits System.Web.UI.Page
    Public pass As New clientlogin.Hashing
    Private oldPass As String
    'ernest
    Public frmconnect As New Login  'instance of the logon form
    'Private dbinfo As New dbparm.dbinfo
    Private object_userlog As New usrlog.usrlog
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class
    'Private connectionString As String = Session("ConnectionString")
    'Private cnSQL As SqlConnection = dbinfo.cnSQL
    'Private strSQL As String = dbinfo.strSQL
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Public ComplexityCheck As New cmplx.cmplex
    'Private serverName As String
    'Private dataBaseName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub changepass_Click(sender As Object, e As EventArgs) Handles changepass.Click


        'check if old password is correct
        If checkPass() = True Then

            'check if password complexity checking is a requirement
            If RequireComplexity() = True Then
                'Check if password meets password complexity
                If ComplexityCheck.ChkPassComplex(Trim(txtnew.Text)) = False Then
                    txtnew.Text = ""
                    txtold.Text = ""
                    txtconfirm.Text = ""
                    Exit Sub
                End If
            End If

            'Check if the new passwords match
            If Len(txtnew.Text) <> Len(txtconfirm.Text) Then
                'MsgBox("New passwords do not match.", MsgBoxStyle.Critical, "Password")
                lblError.Text = alert(" New passwords do not match", " Password")
                txtnew.Text = ""
                txtold.Text = ""
                txtconfirm.Text = ""
                Exit Sub
            Else
                'check the password history for matching passwords
                If checkPassHistory() = True Then
                    'MsgBox("New password matches one of the passwords previously used", MsgBoxStyle.Critical, "Password")
                    lblError.Text = alert(" New password matches one of the passwords previously used", " Password")
                    txtnew.Text = ""
                    txtold.Text = ""
                    txtconfirm.Text = ""
                    Exit Sub
                Else
                    'save the old password to the password history
                    Call SaveOldPass()
                    'change the user's password
                    Call SaveNewPass()

                End If
            End If
        Else
            'MsgBox(" Old password incorrect.", MsgBoxStyle.Critical, "Password")
            lblError.Text = alert(" Old password incorrect", " Password")
            Exit Sub
        End If

    End Sub

    'save new password
    Private Sub SaveNewPass()

        Session("passContents") = txtnew.Text
        'clayton
        frmconnect.encryptpassword() ' this module call will encrypt the password  using the original key before comparing

        Try
            strSQL = "begin tran x" & _
                    " update logins set user_password = '" & Session("passContents") & "',dateset = '" & Session("SysDate") & "',requirepasschange='No' where user_id = '" & Session("username") & "'" & _
                    " update SYSALTUSR set sysPass='" & Session("passContents") & "' where sysUserID = '" & Session("username") & "'" & _
                    " commit tran x"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'MsgBox("Password changed, you are required to re-logon to the system.", MsgBoxStyle.Information, "Password")
            lblsuccess.Text = alert(" Password changed, you are required to re-logon to the system.", " Password")
            clientlogin_vars.passwordChanged = True 'Variable that determines if a user has change the password for compulsory change requirement

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "PWD001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " changed own password", Session("serverName"), Session("client"))
            '************************END****************************************
            Response.AddHeader("REFRESH", "1;URL=login.aspx")
            'Response.Redirect("Login.aspx", True)

            ' Shell(strCurrentDirectory & "\treysys.exe", AppWinStyle.NormalFocus, False)


        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            alert(ex.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub
    'Save old password
    Private Sub SaveOldPass()
        Try
            strSQL = "insert  pwdhistory values('" & Session("username") & "','" & oldPass & "','" & Session("SysDate") & "')"
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
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            alert(ex.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub
    Private Function checkPassHistory() As Boolean
        Dim pwdHis As New Collection
        Session("passContents") = txtnew.Text
        'clayton
        frmconnect.encryptpassword() ' this module call will encrypt the password  using the original key before comparing

        Try

            strSQL = "select user_pwd from pwdhistory where user_ID = '" & Session("username") & "' order by user_changedate asc"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                pwdHis.Add(drSQL.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            Dim match As Boolean
            If pwdHis.Count > 0 Then
                Dim x, y As Integer
                x = pwdHis.Count
                For y = 1 To x

                    If Trim(pwdHis.Item(y).ToString).Equals(Session("passContents")) Then
                        match = True
                        Exit For
                    End If
                Next

                'delete the oldest password
                If x = 12 Then
                    strSQL = "delete pwdhistory where user_id='" & Session("username") & "' and user_pwd = '" & pwdHis.Item(1).ToString & "'"
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader()

                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()

                End If
            End If


            Return match

        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            alert(ex.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function

    'checking the old password if its correct
    Private Function checkPass() As Boolean

        Session("passContents") = Trim(txtold.Text)
        Try

            strSQL = "select user_password from Logins where user_ID = '" & Session("username") & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                oldPass = drSQL.Item(0).ToString
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            'encrypt the password before comparing
            'clayton
            frmconnect.encryptpassword() ' this module call will encrypt the password  using the original key before comparing

            '-------------------------------------------------------end of encription
            'Compare the result

            If Session("passContents").Equals(Trim(oldPass)) = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical, "checkPass")
            alert(ex.Message, "CheckPass")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " checkPass", Session("serverName"), Session("client"))
            '************************END****************************************

        End Try

    End Function

    'checking the old password if its correct
    Private Function RequireComplexity() As Boolean
        Dim Complex As String
        Try

            strSQL = "select [value] from systemparameters where parameter = 'cmplx'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                Complex = UCase(drSQL.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            If Trim(Complex) = "Y" Then
                Return True
            Else
                Return False
            End If

        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical, "RequireComplexity")
            alert(ex.Message, "RequireComplexity")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " RequireComplexity", Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

    End Function



End Class
Imports System.Data.SqlClient
Imports sys_ui



Public Class Resetpass
    Inherits System.Web.UI.Page
    Private Resetusername As String
    Private user_id As String
    Public frmconnect As New Login  'instance of the logon form
    'ernest
    'Private dbinfo As New dbparm.dbinfo
    Private object_userlog As New usrlog.usrlog
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class
    Public pass As New clientlogin.Hashing
    'Private connectionString As String = Session("ConnectionString")
    'Private cnSQL As SqlConnection = cnSQL
    'Private strSQL As String = dbinfo.strSQL
    Public ComplexityCheck As New cmplx.cmplex

    'Private serverName As String
    'Private dataBaseName As String
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then


            Call GetSystemDate()

            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then

                'For i = 1 To Request.QueryString("user_id").Count
                '    Response.Write(Request.QueryString("user_id")(i) & "<BR>")
                'Next


                user_id = Request.QueryString("user_id")
                Dim linkUser As String = ConvertHexToString(user_id, System.Text.Encoding.Unicode)
                'Request.ServerVariables("QUERY_STRING") = LoggedUser
                txtresetUser.Text = linkUser
                txtresetUser.Enabled = False
                'MsgBox("username" + txtresetUser.Text)
            Else
                'MsgBox("no username found")
                lblError.Text = alert(" No username found", " Username")
                'Response.Redirect("forgotpass.aspx")
                Response.AddHeader("REFRESH", "1;URL=forgotpass.aspx")
            End If
        End If
    End Sub


    Protected Sub btnresetpass_Click(sender As Object, e As EventArgs) Handles btnresetpass.Click
        'If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then
        'Request.ServerVariables("QUERY_STRING") = txtresetUser.Text
        'Request.ServerVariables("URL") & "?" & Request.Querystring = txtresetUser.Text

        Session("loggedUserLog") = txtresetUser.Text

        Session("SysDate") = GetSystemDate()
        If txtnewpass.Text = "" Or txtconfirmpass.Text = "" Then
            lblError.Text = alert(" PLEASE ENTER YOUR NEW PASSWORD", "Enter Password")
            '' MsgBox(" PLEASE ENTER YOUR USERNAME AND PASSWORD")
            Exit Sub
        End If
        If checkName() = True Then

            'check if password complexity checking is a requirement
            If RequireComplexity() = True Then
                'Check if password meets password complexity
                If ComplexityCheck.ChkPassComplex(Trim(txtnewpass.Text)) = False Then
                    txtnewpass.Text = ""
                    'txtresetUser.Text = ""
                    txtconfirmpass.Text = ""
                    Exit Sub
                End If
            End If

            'Check if the new passwords match
            If Len(txtnewpass.Text) <> Len(txtconfirmpass.Text) Then
                'MsgBox("New passwords do not match.", MsgBoxStyle.Critical, "Password")
                lblError.Text = alert(" New passwords do not match", " Password")
                txtnewpass.Text = ""
                'txtresetUser.Text = ""
                txtconfirmpass.Text = ""
                Exit Sub
            Else
                'check the password history for matching passwords
                If checkPassHistory() = True Then
                    'MsgBox("New password matches one of the passwords previously used", MsgBoxStyle.Critical, "Password")
                    lblError.Text = alert(" New password matches one of the passwords previously used", " Password")
                    txtnewpass.Text = ""
                    'txtresetUser.Text = ""
                    txtconfirmpass.Text = ""
                    Exit Sub
                Else
                    'save the old password to the password history
                    'Call SaveOldPass()
                    'change the user's password
                    Call SaveNewPass()


                End If
            End If
        Else
            'MsgBox(" Username not recognised .", MsgBoxStyle.Critical, "Password")
            lblError.Text = alert(" Username not recognised", " Username")
            Exit Sub
        End If


    End Sub

    Private Sub SaveNewPass()

        Session("passContents") = txtnewpass.Text
        ''clayton
        frmconnect.encryptpassword() ' this module call will encrypt the password  using the original key before comparing

        Try
            strSQL = "begin tran x" & _
                    " update logins set user_password = '" & Session("passContents") & "',dateset = '" & Session("SysDate") & "',requirepasschange='No' where user_id = '" & Session("loggedUserLog") & "'" & _
                    " update SYSALTUSR set sysPass='" & Session("passContents") & "' where sysUserID = '" & Session("loggedUserLog") & "'" & _
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

            lblsuccess.Text = alert(" Password Reset, You can now login.", " Password")
            'MsgBox("Password Reset,You can now login.", MsgBoxStyle.Information, "Password")
            clientlogin_vars.passwordChanged = True 'Variable that determines if a user has change the password for compulsory change requirement

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "PWD001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " changed own password", Session("serverName"), Session("client"))
            '************************END****************************************
            Response.AddHeader("REFRESH", "1;URL=login.aspx")
            'Response.Redirect("Login.aspx")

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
    'Private Sub SaveOldPass()
    '    Try
    '        strSQL = "insert  pwdhistory values('" & LoggedUser & "','" & oldPass & "','" & SysDate & "')"
    '        cnSQL = New SqlConnection(Session("ConnectionString"))
    '        cnSQL.Open()
    '        cmSQL = New SqlCommand(strSQL, cnSQL)
    '        drSQL = cmSQL.ExecuteReader()

    '        ' Close and Clean up objects
    '        drSQL.Close()
    '        cnSQL.Close()
    '        cmSQL.Dispose()
    '        cnSQL.Dispose()


    '    Catch ex As SqlException
    '        MsgBox(ex.Message, MsgBoxStyle.Critical)

    '        'Log the event *****************************************************
    '        SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message)
    '        '************************END****************************************

    '    End Try
    'End Sub
    Private Function checkPassHistory() As Boolean
        Dim pwdHis As New Collection
        Session("passContents") = txtnewpass.Text
        'clayton
        frmconnect.encryptpassword() ' this module call will encrypt the password  using the original key before comparing

        Try

            strSQL = "select user_pwd from pwdhistory where user_ID = '" & Session("loggedUserLog") & "' order by user_changedate asc"
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
                    strSQL = "delete pwdhistory where user_id='" & Session("loggedUserLog") & "' and user_pwd = '" & pwdHis.Item(1).ToString & "'"
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
    Private Function checkName() As Boolean

        clientlogin_vars.UsernameCheck = Trim(txtresetUser.Text)
        Try

            strSQL = "select user_id from Logins where user_ID = '" & Session("loggedUserLog") & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                Resetusername = drSQL.Item(0).ToString
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
            'encrypt the password before comparing

            frmconnect.encryptpassword() ' this module call will encrypt the password  using the original key before comparing

            '-------------------------------------------------------end of encription
            'Compare the result

            If clientlogin_vars.UsernameCheck.Equals(Trim(Resetusername)) = True Then
                Return True
            Else
                Return False
            End If

        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical, "checkPass")
            alert(ex.Message, "CheckName")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " Check username", Session("serverName"), Session("client"))
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

    Private Function GetSystemDate() As String
        Dim Sys As String
        Try
            'validate username first
            strSQL = "select [value] from systemparameters where parameter='sysdt'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                Sys = drSQL.Item(0).ToString
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            If Trim(Sys) = "-" Then
                'alert("System date not set", "System Date")
                MsgBox("System date not set", MsgBoxStyle.Critical, "System Date")
                'Dim deft As New DefaultParamScrn
                'deft.grpSysDate.Visible = True
                'deft.grpBaseCurrency.Visible = False
                'deft.ShowDialog()

                Return False

            End If

            Return Sys
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            alert(ex.Message, "Error")
            'Dim mysettings As New frmSettings
            'mysettings.ShowDialog()

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function
    'Public Function ConvertStringToHex(input As [String], encoding As System.Text.Encoding) As String
    '    Dim stringBytes As [Byte]() = encoding.GetBytes(input)
    '    Dim sbBytes As New StringBuilder(stringBytes.Length * 2)
    '    For Each b As Byte In stringBytes
    '        sbBytes.AppendFormat("{0:X2}", b)
    '    Next
    '    Return sbBytes.ToString()
    'End Function
    Public Function ConvertHexToString(hexInput As [String], encoding As System.Text.Encoding) As String
        Dim numberChars As Integer = hexInput.Length
        Dim bytes As Byte() = New Byte(numberChars / 2 - 1) {}
        For i As Integer = 0 To numberChars - 1 Step 2
            bytes(i / 2) = Convert.ToByte(hexInput.Substring(i, 2), 16)
        Next
        Return encoding.GetString(bytes)
    End Function


   


End Class
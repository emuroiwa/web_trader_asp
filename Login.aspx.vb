Imports System.Data.SqlClient
Imports sys_ui
Imports System.Security.Cryptography
Imports System.Net.Sockets
Imports System.IO
Imports System.Net



Public Class Login

    Inherits System.Web.UI.Page
    Public passwordChanged As Boolean = False
    Private LogThread As Threading.Thread
    'Public client As TcpClient
    'Public pass As New clientlogin.Hashing
    'Private pass As New clientlogin.Declarations  'instance of the Hashing class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class
    'Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    'Private hashing_object As New clientlogin.Hashing  'instance of the userlogins class
    Private object_userlog As New usrlog.usrlog
    ''Private encryption_object As New clientlogin.Encryptions  'instance of the userlogins class
    Private encry As clientlogin.Encryptions
    Private strSourcePath As String
    Private strRijndaelSaltIVFile As String
    Private strTripleDESSaltIVFile As String
    Private strCurrentKeyFile As String
    Private intCurrentGradientShift As Integer = 10
    ''Public LoggedUser As String = username.Text
    'Public logonCount As Integer

    Public ServerNameCookie As HttpCookie
    Public DatabaseNameCookie As HttpCookie
    Public DatabasePasswordCookie As HttpCookie
    Private serverName As String
    Private dataBaseName As String
    'Private ConnectionString As String = Session("ConnectionString")
    'Public client As TcpClient 'instance of the TCP Client
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader

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
                lblError.Text = alert("System date not set", "System Date")
                '' MsgBox("System date not set", MsgBoxStyle.Critical, "System Date")
                'Dim deft As New DefaultParamScrn
                'deft.grpSysDate.Visible = True
                'deft.grpBaseCurrency.Visible = False
                'deft.ShowDialog()

                Return False

            End If

            Return Sys
        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            lblError.Text = alert(ex.Message, "Error")
            'Dim mysettings As New frmSettings
            'mysettings.ShowDialog()

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function
    Private Sub SetDBDetails()
        'Dim servername1 As String
        'Dim databasename1 As String
        Session("Isdealer") = "True"
        Session("dataBaseName") = Request.Cookies("ckDatabaseName").Value
        Session("serverName") = Request.Cookies("ckServerName").Value
        Session("ConnectionString") = "Network Library=DBMSSOCN;" & _
                      "Data Source= " & Session("serverName") & ",1433;" & _
                      "Initial Catalog=" & Session("dataBaseName") & ";" & _
                      "User ID=uniuser;" & _
                      "Password=passwordA@@@pass;Connect Timeout=30"
        'databasename1 = Session("dataBaseName")
        'servername1 = Session("serverName")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then


            If Not Request.Cookies("ckServerName") Is Nothing Then

                ''txtServerName.Text = " "
                If Not Request.Cookies("ckServerName").ToString() = "" Then
                    ServerNameCookie = Request.Cookies("ckServerName")
                    txtServerName.Text = Server.HtmlEncode(ServerNameCookie.Value).ToString()
                    'MsgBox(Server.HtmlEncode(svrcokie.Value).ToString())
                End If
                If Not Request.Cookies("ckDatabaseName") Is Nothing Then
                    DatabaseNameCookie = Request.Cookies("ckDatabaseName")
                    txtDatabaseName.Text = Server.HtmlEncode(DatabaseNameCookie.Value).ToString()
                End If
                If Not Request.Cookies("ckDatabasePassword") Is Nothing Then
                    DatabasePasswordCookie = Request.Cookies("ckDatabasePassword")
                    txtDatabaseName.Text = Server.HtmlEncode(DatabasePasswordCookie.Value).ToString()
                End If
                'dbname.Text = "fbc_bank"
                'svrname.Text = "clayton-pc"
                If checkDateFormat() <> "MM/dd/yyyy" Then
                    lblError.Text = alert("Incorrect date format set on PC. Set the short date format to MM/dd/yyyy", "Check System Time")
                    ''  MsgBox("Incorrect date format set on PC. Set the short date format to MM/dd/yyyy")

                End If

                Call SetDBDetails()

                'Get IP address for system logging on
                Call GetIPAddress()

                ''username.Focus()
                'STEP : 0
                'determine where the server database are located
                If ServerDetails() = True Then
                    Call GetCompanyInfo()
                    Call GetSystemDate()
                    Call LogOnCountSetting()
                    'Get IP address for system logging on
                    'Call GetIPAddress()


                    'Check for values necessary to start system
                    Call CheckBaseCurrency()

                    Session("SysDate") = GetSystemDate()

                    lblDate.Text = Format(Session("SysDate"), "Long Date")

                    Session("ServerLog") = "TEST"

                    LogThread = New Threading.Thread(AddressOf SendToLog)
                    LogThread.Start()
                    'Get the current system date
                    Try
                        client = New TcpClient(Session("serverName"), Session("PORT_NUM"))


                    Catch Ex As Exception
                        '' MsgBox("Server is not active.  Please start server and try again.",  MsgBoxStyle.Exclamation)
                        lblError.Text = alert("Server is not active.  Please start server and try again.", "Server Down")
                        btnLogin.Enabled = False
                    End Try

                End If
            Else
                lblError.Text = alert("Server is not active.  Please start server and try again.", "Server Down")

                ''MsgBox("Server is not active.  Please start server and try again.", _   MsgBoxStyle.Exclamation)
                btnLogin.Enabled = False

            End If
        End If
        'Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub

    Public Sub SendToLog()
        On Error Resume Next

        Dim writer As New IO.StreamWriter(client.GetStream)
        client.Connect(Session("serverName"), Session("PORT_NUM"))
        writer.Write("LOGEVENT|" & dataBaseName & "!" & Session("ServerLog") & vbCr)
        writer.Flush()

        'Threading.Thread.Sleep(10000)
    End Sub

    Private Function CompPasswords() As Boolean
        Dim pass1 As String = ""
        Dim pass2 As String = ""

        Try
            'validate username first
            strSQL = "select user_password from logins where user_ID = '" & Trim(txtUsername.Text) & "' "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                pass1 = Trim(drSQL.Item(0).ToString)
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'validate username first
            strSQL = "select sysPass from SYSALTUSR where sysUserID = '" & Trim(txtUsername.Text) & "' "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                pass2 = Trim(drSQL.Item(0).ToString)
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception

        End Try

        Return pass1.Equals(pass2)

    End Function

    'Checks if the user must change the password before logging on
    Private Function CheckPassChange() As String
        Dim result As String
        Try
            strSQL = "select requirepasschange required from logins where user_id = '" & Session("username") & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    result = drSQL.Item(0).ToString
                Loop
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            Return result
            'catch any error
        Catch e As Exception
            lblError.Text = alert(e.Message, "Error")

            ''  MsgBox(e.Message, MsgBoxStyle.Critical, "Error")

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & e.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function

    Private Sub GetCompanyInfo()

        Try
            'validate username first
            strSQL = "select companyname from instinfo"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                lblCompName.Text = Trim(drSQL.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            '' MsgBox(ex.Message, MsgBoxStyle.Critical)
            lblError.Text = alert(ex.Message, "Error")

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub


    'Check account status blocked or locked checkAccountStatus
    Private Function checkAccountStatus() As String
        Dim result As String
        Try
            strSQL = "select user_statusABL from logins where user_id = '" & Trim(txtUsername.Text) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    result = drSQL.Item(0).ToString
                Loop
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            If result = "B" Then Return "Blocked"
            If result = "L" Then Return "Locked"

            'catch any error
        Catch e As Exception
            '' MsgBox(e.Message, MsgBoxStyle.Critical, "Error")
            alert(e.Message, "Error")

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & e.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function

    'Check Password expiry against epiery periods
    Private Sub PasswordValidity()

        Try
            strSQL = "begin tran PassValidCheck " & _
                     " DECLARE @result char(50) " & _
                     " declare @date2 datetime " & _
                     " declare @diff int " & _
                     " declare @date3 datetime " & _
                     " declare @ValidityPeriod int " & _
                     " select @date2 = dateset from logins where user_id= '" & Session("username") & "' " & _
                     " select @date3 = [value]  from SYSTEMPARAMETERS where [parameter]= 'sysdt' " & _
                     " select @ValidityPeriod = daystoexpirey from passwords " & _
                     " select @diff=datediff(""dd"",@date2,@date3) " & _
                     " if (@ValidityPeriod - @diff) = 7 " & _
                     " Begin " & _
                     " set @result = 'Seven days left before password expires.' " & _
                     " select @result " & _
                     " End " & _
                     " if (@ValidityPeriod - @diff) = 6 " & _
                     " Begin " & _
                     " set @result = '6 days left before password expires.' " & _
                     " select @result " & _
                     " End " & _
                     " if (@ValidityPeriod - @diff) = 5 " & _
                     " Begin " & _
                     " set @result = '5 days left before password expires.' " & _
                     " select @result " & _
                     " End " & _
                     " if (@ValidityPeriod - @diff) = 4 " & _
                     " Begin " & _
                     " set @result = '4 days left before password expires.' " & _
                     " select @result " & _
                     " End " & _
                     " if (@ValidityPeriod - @diff)= 3 " & _
                     " Begin " & _
                     " set @result = '3 days left before password expires.' " & _
                     " select @result " & _
                     " End " & _
                     " if (@ValidityPeriod - @diff) = 2 " & _
                     " Begin " & _
                     " set @result = '2 days left before password expires.' " & _
                     " select @result " & _
                     " End " & _
                     " if (@ValidityPeriod - @diff) = 1 " & _
                     " Begin " & _
                     " set @result = '1 days left before password expires.' " & _
                     " select @result " & _
                     " End " & _
                     " if (@ValidityPeriod - @diff) <= 0 " & _
                     " Begin " & _
                     " set @result = 'Your password has expired.' " & _
                     " select @result " & _
                     " End " & _
                     " Commit tran PassValidCheck "

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                Dim SQLMessage As String
                SQLMessage = drSQL.Item(0).ToString
                If Trim(SQLMessage) = "Your password has expired." Then

                    'Log the event *****************************************************
                    object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR199" & Format(Now, "dd/MM/yyyy hh:mm:ss") & txtUsername.Text & "   :Error :  Your password has expired." & " from ID address : ernest", Session("serverName"), Session("client"))
                    ''************************END****************************************

                    ''alert()

                    MsgBox(Trim(SQLMessage) & " Click OK to change your password.", MsgBoxStyle.Critical, "Password")
                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()
                    'Load the password change form
                    ''Dim pas As New ChangePass
                    Response.Redirect("ChangePass.aspx", True)
                    '' password.Text = pas.txtnew.Text

                    'Response.Redirect("Login.aspx", True)
                    'Exit Sub
                ElseIf SQLMessage <> "" Then
                    MsgBox(SQLMessage, MsgBoxStyle.Information, "Password")
                End If
            Loop


            'catch any error
        Catch e As SqlException
            'MsgBox(e.Message, MsgBoxStyle.Critical, "Error")
            alert(e.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & e.Message, Session("serverName"), Session("client"))
            ''************************END****************************************

        End Try
    End Sub
    'Save the date the user last logged on as now
    Private Sub ResetCount()
        Try
            strSQL = "update logins set user_failedAttempts=0,user_lastLogin='" & Now & "' where user_id = '" & Session("username") & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'catch any error
        Catch e As Exception
            'MsgBox(e.Message, MsgBoxStyle.Critical, "Error")
            alert(e.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & e.Message, Session("serverName"), Session("client"))
            ''************************END****************************************

        End Try
    End Sub

    '    encrypts the user password using the MD5 hashing algorithm
    Public Sub encryptpassword()
        Dim passwordHashMD5 As String
        Dim passwordHashSha1 As String
        Dim passwordHashSha256 As String
        Dim passwordHashSha384 As String
        Dim passwordHashSha512 As String

        passwordHashMD5 = _
         clientlogin.Hashing.ComputeHash(Session("passContents"), "MD5", Nothing)
        Session("passContents") = passwordHashMD5

    End Sub
    Private Sub PreviousFailedCheck()
        'Check Previous failed attempts field and update as neccessary

        Try
            strSQL = "Select user_failedAttempts from logins where user_id = '" & Session("username") & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                Session("logonCount") = Int(drSQL.Item(0))
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'catch any error
        Catch e As Exception
            'MsgBox(e.Message, MsgBoxStyle.Critical, "Error")
            alert(e.Message, "Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & e.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub
    Private Sub LogOnCountSetting()
        Try
            'validate username first
            strSQL = "select lockPolicy from Passwords"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                Session("logonCountMax") = drSQL.Item(0).ToString
            Loop

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


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Session("username") = txtUsername.Text
        '' Response.Write("sdgfsd")

        Session("username") = txtUsername.Text
        Session("loggedPass") = txtPassword.Text
        Session("passContents") = txtPassword.Text  ' Capture the userpassword
        Session("loggedUserLog") = Session("username")
        Call SetDBDetails()
        Call LogOnCountSetting()
        'Call initialiseMain_Module()


        If txtUsername.Text = "" Or txtPassword.Text = "" Then
            lblError.Text = alert(" PLEASE ENTER YOUR USERNAME AND PASSWORD", "Enter Password")
            '' MsgBox(" PLEASE ENTER YOUR USERNAME AND PASSWORD")
            Exit Sub
        End If

        'Ensure that Session("loggedUserLog") is 15 characters long **************************
        Session("loggedUserLog") = Session("username")
        Dim x, y, z As Integer
        Dim blank As String = " "
        x = Len(Session("loggedUserLog"))
        If x < 15 Then
            y = 15 - x
            'append y characters to loggeduserlog contents
            For z = 1 To y
                Session("loggedUserLog") = Session("loggedUserLog") & blank
            Next
        End If
        '**************************************END

        'STEP : 1
        If CompPasswords() = False Then
            '' MsgBox("Password problem, contact system administrator", MsgBoxStyle.Critical)
            lblError.Text = alert("Password problem, contact system administrator", "Password Error")
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR199" & Format(Now, "dd/MM/yyyy hh:mm:ss") & txtUsername.Text & "   :Error :  Password problem, contact system administrator" & " from IP address : ernest ", Session("serverName"), Session("client"))
            '************************END****************************************
            Exit Sub
        End If

        'Check Password expiry against epiery periods
        Call PasswordValidity()

        'STEP : 2
        'Check Previous failed attempts field and update as neccessary
        Call PreviousFailedCheck()

        'STEP : 3
        'Check account status blocked or locked checkAccountStatus
        If checkAccountStatus() <> "" Then
            lblError.Text = alert("User cannot log on. Account is " & checkAccountStatus(), "Cannot Login")
            '' MsgBox("User cannot log on. Account is " & checkAccountStatus(), MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR199" & Format(Now, "dd/MM/yyyy hh:mm:ss") & txtUsername.Text & "   :Error : " & "User cannot log on. Account is " & checkAccountStatus() & " from IP address : ernest", Session("serverName"), Session("client"))
            '************************END****************************************

            txtUsername.Text = ""
            txtPassword.Text = ""
            ''username.Focus()
            Exit Sub
        End If


        'call the function to validate the user

        If clientlogin_object.validate_Login(txtUsername.Text) = True Then
            Dim PassChanged As Boolean = True
            '    'Save the date the user last logged on as now
            '    'reset the failed logon count if logoncount is not 0

            '    'Show user last activity status
            '    'Dim frmact As New frmSysStatus
            '    'frmact.ShowDialog()
            'STEP : 4
            Call ResetCount()
            'Check if password change is required
            If CheckPassChange() = "Yes" Then


                PassChanged = False
                'lblError.Text = alert("Your password has expired. Click ok to change your password.", "Change Password")
                lblError.Text = alert("Your password has expired. Click ok to change your password.", "Login Error")

                'Log the event *****************************************************
                object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR199" & Format(Now, "dd/MM/yyyy hh:mm:ss") & txtUsername.Text & "   :Error : " & "Your password has expired. Click ok to change your password.", Session("serverName"), Session("client"))
                '************************END****************************************
                'Response.AddHeader("REFRESH", "1;URL=login.aspx")
                Response.Redirect("ChangePass.aspx", True)
                'Dim passChange As New ChangePass
                ''passChange.ShowDialog()
                PassChanged = passwordChanged
            End If

            If PassChanged = False Then  'The user has refused to change the password
                ' Response.Redirect("index.aspx", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "openWindow();", True)
                'Else
                '    username.Text = ""
                '    password.Text = ""
                '    username.Focus()
            End If


            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "LOG001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " User logged onto the system. ", Session("serverName"), Session("client"))
            '************************END****************************************
            'Log the event *****************************************************
            object_userlog.SendToLog(Session("loggedUserLog") & "LOG001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & txtUsername.Text & "  Logged on successfully from IP address : Clayton-pc")
            '************************END****************************************
            object_userlog.SendData("CONNECT|" & Session("username"), Session("serverName"), Session("client"))
            '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "openWindow();", True)
            Response.Redirect("index.aspx", True)
            'If Login(username.Text, password.Text) = False Then
            '    MsgBox(" WRONG  USERNAME OR PASSWORD")
        Else
            'Else
            'MsgBox(" Login failed")
            'txtUsername.Text = ""
            'password.Text = ""
            'username.Focus()

        End If



    End Sub
    Protected Sub save_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim svrcookie As New HttpCookie("ckserverName")
        svrcookie.Value = txtServerName.Text
        svrcookie.Expires = DateTime.Now.AddDays(365)
        Response.Cookies.Add(svrcookie)

        Dim DbNamecookie As New HttpCookie("ckDatabaseName")
        DbNamecookie.Value = txtDatabaseName.Text
        DbNamecookie.Expires = DateTime.Now.AddDays(365)
        Response.Cookies.Add(DbNamecookie)

        Dim DbPasswordcookie As New HttpCookie("ckDatabasePassword")
        DbPasswordcookie.Value = txtDatabaseName.Text
        DbPasswordcookie.Expires = DateTime.Now.AddDays(10)
        Response.Cookies.Add(DbPasswordcookie)

        'My.Settings.pass = svrpass.Text

        'My.Settings.Save()
        'My.Settings.Reload()
        lblsuccess.Text = success("Settings saved.", "System Settings")
        'MsgBox("Settings saved. Please refresh your browser", MsgBoxStyle.Information)

        Response.AddHeader("REFRESH", "1;URL=login.aspx")
        'Response.Redirect("login.aspx")
    End Sub
    Public Function ServerDetails() As Boolean

        If Server.HtmlEncode(DatabaseNameCookie.Value).ToString() = "" Then
            '' MsgBox("Configure Database and Server location.", MsgBoxStyle.Information, "Settings")
            lblError.Text = alert("Configure Database and Server location.", "Create Settings")
            btnLogin.Enabled = False
            Return False
        Else
            btnLogin.Enabled = True
            'cmdConfig.Visible = False

            'Initialise dB path and location variables
            '' Call DbLocation()
            'Get comports details
            Call getcomPorts()
            ' get the logon failure count setting
            Call LogOnCountSetting()
            Return True
        End If

    End Function

    'Public Sub DbLocation()
    '    Try

    '        '    'Reinitialize connection string
    '        'If Not Request.Cookies("svrname") Is Nothing Then
    '        '    Dim svrcokie As HttpCookie = Request.Cookies("svrname")
    '        '    serverName = Server.HtmlEncode(svrcokie.Value)
    '        'End If
    '        'If Not Request.Cookies("dbname") Is Nothing Then
    '        '    Dim dbcokie As HttpCookie = Request.Cookies("dbname")
    '        '    dataBaseName = Server.HtmlEncode(dbcokie.Value)
    '        'End If

    '        serverName = txtServerName.Text
    '        dataBaseName = txtDatabaseName.Text

    '        LoadDatabase.ConnectionString = _
    '        "Network Library=DBMSSOCN;" & _
    '         "Data Source= " & serverName & ",1433;" & _
    '         "Initial Catalog=" & dataBaseName & ";" & _
    '         "User ID=uniuser;" & _
    '         "Password=passwordA@@@pass;Connect Timeout=30"

    '        LoadDatabase.SQL_CONNECTION_STRING = _
    '        "Network Library=DBMSSOCN;" & _
    '         "Data Source= " & serverName & ",1433;" & _
    '         "Initial Catalog=" & dataBaseName & ";" & _
    '         "User ID=uniuser;" & _
    '         "Password=passwordA@@@pass;Connect Timeout=30"
    '    Catch E As Exception
    '        lblError.Text = alert(E.Message, "Error")
    '        ''MsgBox(E.Message)
    '    End Try
    'End Sub
    'Gets the communication ports the server is listening on
    Private Sub getcomPorts()

        Try
            strSQL = "Select * from sysport"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                Session("PORT_NUM") = Int(drSQL.Item(1))
                Session("PORT_NUMLM") = Int(drSQL.Item(0))
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'catch any error
        Catch e As Exception
            lblError.Text = alert(e.Message, "Get ComPorts")
            ''  MsgBox(e.Message, MsgBoxStyle.Critical, "Get ComPorts")
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & e.Message, serverName, Session("client"))
        End Try
    End Sub

    'Public Sub Sys()
    '    ServerCheckThread = New Threading.Thread(AddressOf CheckServer)
    '    ServerCheckThread.Start()

    'End Sub
    'End Module
    Private Function checkDateFormat() As String
        Dim X As New Globalization.CultureInfo("en-ZW", True)
        'MsgBox(X.DateTimeFormat.ShortDatePattern)
        ' user defined
        Return X.DateTimeFormat.ShortDatePattern

    End Function
    Private Sub GetIPAddress()

    End Sub
    'Shared Function ReadFileAsString(ByVal path As String) As String
    '    Dim fs As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
    '    Dim abyt(CInt(fs.Length - 1)) As Byte

    '    fs.Read(abyt, 0, abyt.Length)
    '    fs.Close()

    '    Return UTF8.GetString(abyt)
    'End Function
    Private Sub CheckBaseCurrency()
        'Dim x As Integer = 0
        'Try
        '    'validate username first
        '    strSQL = "select * from currencies where isbasecurrency='yes'"
        '    cnSQL = New SqlConnection(Session("ConnectionString"))
        '    cnSQL.Open()
        '    cmSQL = New SqlCommand(strSQL, cnSQL)
        '    drSQL = cmSQL.ExecuteReader()

        '    If drSQL.HasRows = True Then
        '        Do While drSQL.Read
        '            x = x + 1
        '        Loop

        '    Else
        '        MsgBox("System base currency not defined", MsgBoxStyle.Critical)

        '        Dim deft As New DefaultParamScrn
        '        deft.grpSysDate.Visible = False
        '        deft.grpBaseCurrency.Visible = True
        '        deft.ShowDialog()


        '    End If

        '    If x > 1 Then
        '        MsgBox("More than one System base currencies are defined.", MsgBoxStyle.Critical)

        '    End If
        '    ' Close and Clean up objects
        '    drSQL.Close()
        '    cnSQL.Close()
        '    cmSQL.Dispose()
        '    cnSQL.Dispose()

        'Catch ex As SqlException
        '    MsgBox(ex.Message, MsgBoxStyle.Critical)

        '    'Log the event *****************************************************
        '    SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & " CheckBaseCurrency")
        '    '************************END****************************************

        'End Try

    End Sub

    'Private Sub initialiseMain_Module()
    '    Dim sys_start As New clientlogin.sys_start
    '    sys_start.StartApplication(client, Session("serverName"), Session("dataBaseName"), Session("logonCountMax"), Session("PORT_NUM"), Session("loggedUserLog"),Session("username"), pass.passContents, Session("ConnectionString"))
    'End Sub

End Class
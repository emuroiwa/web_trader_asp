Imports System.Data.SqlClient
Imports System.Net.Sockets
Imports System.IO

''Imports WEBTDS.


Public Class Main
    Inherits System.Web.UI.MasterPage
    Public serverstatus As Boolean
    Private clients As TcpClient
    Private LogThread As Threading.Thread
    'Private dbinfo As New dbparm.dbinfo
    Private object_userlog As New usrlog.usrlog
    Private clientlogin_vars As clientlogin.Declarations  'instance of the userlogins class
    Public ServerCheckThread As Threading.Thread
    'Private connectionString As String = Session("ConnectionString")
    'Private cnSQL As SqlConnection = cnSQL
    'Private strSQL As String = strSQL
    'Private connectionString As String = ConnectionString
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    'Private serverName As String
    'Private dataBaseName As String

    Public Sub CheckServer()
        'check if server has been started



        Do
            Try
                client = New TcpClient(Session("serverName"), Session("PORT_NUMLM"))
                ' Session("clientLM") = New TcpClient(Session("serverName"), Session("PORT_NUMLM"))
                 Catch Ex As Exception
                serverstatus = False
                Session.Remove("username")
                'Session("username") = ""

                ServerCheckThread.Abort()
            End Try


        Loop


    End Sub
    Public Sub Sys()
        ServerCheckThread = New Threading.Thread(AddressOf CheckServer)
        ServerCheckThread.Start()

    End Sub
    Private Sub GetSystemDate()

        Try
            'validate username first
            strSQL = "select [value] from systemparameters where parameter='sysdt'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                lblDate.Text = Format(Trim(drSQL.Item(0).ToString), "Long Date")
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
    Public Function GetUrlFolder()
        Dim segments As String() = Request.Url.Segments
        Return segments(1)
    End Function
    Public Function GetUrlFilename()
        Dim page As String = Trim(Path.GetFileName(Request.PhysicalPath))

        Return page
    End Function
    Public Function ActiveLink(ByVal pagename As String)
        Dim li As String

        If pagename = GetUrlFilename() Then
            li = "<li class='active'>"
        Else
            li = "<li>"
        End If
        Return li
    End Function


    Private Sub NavigationItems(ByVal modId As String)

        Dim cnSQLp As SqlConnection
        Dim cmSQLp As SqlCommand
        Dim drSQLp As SqlDataReader
        Dim strSQLp As String

        'Populate the outlook bar
        Try
            'strSQLp = "select * from sysmenuopts join sysmodules on sysmenuopts.modid=sysmodules.moduleid" & _
            '          "join user_functions on user_functions.function_id=sysmenuopts where" & _
            '          " sysmodules.moduledescription='" & modId & "' "

            strSQLp = "Select * from user_functions join sysmenuopts" & _
                      " on user_functions.function_id=sysmenuopts.optid join sysmodules on sysmenuopts.modid=sysmodules.moduleid" & _
                      " where user_id='" & Session("username") & "' and sysmodules.moduledescription='" & modId & "' Order by cast(sysmenuopts.funcid as int) asc"
            cnSQLp = New SqlConnection(Session("ConnectionString"))
            cnSQLp.Open()
            cmSQLp = New SqlCommand(strSQLp, cnSQLp)
            drSQLp = cmSQLp.ExecuteReader()

            ' lstsubMenu.Items.Clear()
            ''lstoptions.Items.Clear()
            'lblTreeView.Text += "<li"
            'lblTreeView.Text += "class=active treeview"
            'lblTreeView.Text += ">"
            Do While drSQLp.Read

                Select Case modId

                    Case "Money Market"


                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString

                        If GetUrlFolder() = "MoneyMarket/" Then
                            lblMoneyMarket.Text += ActiveLink(pagelink) & "<a href='" & pagelink & "' ><i class='fa fa-circle-o'></i>" & myitm & "</a></li>"
                        ElseIf GetUrlFolder() <> "MoneyMarket/" Then
                            lblMoneyMarket.Text += ActiveLink(pagelink) & "<a href='../MoneyMarket/" & pagelink & "'><i class='fa fa-circle-o'></i>" & myitm & "</a></li>"
                        Else
                            lblMoneyMarket.Text += ActiveLink(pagelink) & "<a href='MoneyMarket/" & pagelink & "'><i class='fa fa-circle-o'></i>" & myitm & "</a></li>"
                        End If

                    Case "Foreign Market"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblForeignMarket.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li>"
                    Case "Securities"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblSecurities.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li>"
                    Case "Derivatives"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblDerivatives.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li> "
                    Case "Portfolio Management"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblPortfolioManagement.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li> "
                    Case "System Tools"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblSystemTools.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li> "

                    Case "Reports"
                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        If GetUrlFolder() = "ReportPages/" Then
                            lblReports.Text += " <li><a href='" & pagelink & "'><i class='fa fa-circle-o'></i>" & myitm & "</a></li>"
                        ElseIf GetUrlFolder() <> "ReportPages/" Then
                            lblReports.Text += " <li><a href='../ReportPages/" & pagelink & "'><i class='fa fa-circle-o'></i>" & myitm & "</a></li>"
                        Else
                            lblReports.Text += " <li><a href='ReportPages/" & pagelink & "'><i class='fa fa-circle-o'></i>" & myitm & "</a></li>"
                        End If
                    Case "Administration"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblAdministration.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li> "
                    Case "Miscellaneous"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblMiscellaneous.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li> "
                    Case "Static Data"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblStaticData.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li> "
                    Case "User Management"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblUserManagement.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li> "
                    Case "Fixed Income"

                        Dim myitm As String = drSQLp.Item("OptDescription").ToString
                        Dim pagelink As String = drSQLp.Item("Pagelink").ToString
                        lblFixedIncome.Text += "<li><a href=" & pagelink & "><i class='fa fa-circle-o'></i>" & myitm & "</a></li> "
                End Select
            Loop
            ' Close and Clean up objects
            drSQLp.Close()
            cnSQLp.Close()
            cmSQLp.Dispose()
            cnSQLp.Dispose()

            'catch any error
        Catch e As Exception
            MsgBox(e.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub
    Public Sub GetSessionTimoutStatus()
        Dim cnSQLxw As SqlConnection
        Dim cmSQLxw As SqlCommand
        Dim drSQLxw As SqlDataReader
        Dim strSQLxw As String

        Try
            strSQLxw = "select timeout from PASSWORDS"
            cnSQLxw = New SqlConnection(Session("ConnectionString"))
            cnSQLxw.Open()
            cmSQLxw = New SqlCommand(strSQLxw, cnSQLxw)
            drSQLxw = cmSQLxw.ExecuteReader()

            Do While drSQLxw.Read
                lblSessionTime.Text = drSQLxw.Item(0)
            Loop

            ' Close and Clean up objects
            drSQLxw.Close()
            cnSQLxw.Close()
            cmSQLxw.Dispose()
            cnSQLxw.Dispose()

            ''  myEVTHandler.IdleTime = New TimeSpan(0, timeRemaining, 0)


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Update IP Address")

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

    End Sub


    'Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
    '    '   MsgBox(Request.Cookies("ckServerName").Value)
    '    'Session("username") = Request.Cookies("TDS_USERNAME").Value
    '    Session("ConnectionString") = "Network Library=DBMSSOCN;" & _
    '                  "Data Source= " & Request.Cookies("ckServerName").Value & ",1433;" & _
    '                  "Initial Catalog=" & Request.Cookies("ckDatabaseName").Value & ";" & _
    '                  "User ID=uniuser;" & _
    '                  "Password=passwordA@@@pass;Connect Timeout=30"


    '    Session("password") = Request.Cookies("TDS_PASSWORD").Value
    '    Session("SysDate") = Request.Cookies("TDS_SYSDATE").Value
    '    ' MsgBox(Session("ConnectionString"))
    'End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ernest
        '  Response.Write("<script>  alert('Deal saved Successfully. Deal reference is ') </script>")
        'object_userlog.Msg("")
        If Not IsPostBack Then
            'Call checkUserStatus()
            'If ConRefused = True Then
            '    Call RefuseconRecieved()
            'End If
            Call Sys()

            'If serverstatus = False Then
            '    alert("Server is not active.  Please start server and try again.", "Server Down")
            'End If
            'Session("username") Is Nothing Or

            If Session("username") = "" Then
                alert("Server has been shut down.  Please start server and try again.", "Server Down")
                If GetUrlFolder() <> "index.aspx" Then
                    Response.Redirect("../logout.aspx")
                Else
                    Response.AddHeader("REFRESH", "1;URL=logout.aspx")
                End If

            End If

            
            Call GetSessionTimoutStatus()
            Call NavigationItems("Money Market")
            Call NavigationItems("Foreign Market")
            Call NavigationItems("Securities")
            Call NavigationItems("Derivatives")
            Call NavigationItems("Portfolio Management")
            Call NavigationItems("Fixed Income")
            Call NavigationItems("System Tools")
            Call NavigationItems("Reports")
            Call NavigationItems("Administration")
            Call NavigationItems("Miscellaneous")
            Call NavigationItems("Static Data")
            Call NavigationItems("User Management")

            '' Session("username") = "haya"
            '' Call GetSMTP() 'Get SMTP address
            'GetSystemDate()

            GetFullName(Session("username"))
            lblDate.Text = Format(Session("SysDate"), "Long Date")
            lblUserFullName.Text = Session("UserFullName")
            lblDatabaseName.Text = Session("dataBaseName")
            lblServerName.Text = Session("serverName")
        End If
        'Session("ServerLog") = "TEST"

        'LogThread = New Threading.Thread(AddressOf SendToLog)
        'LogThread.Start()
        'Try
        '    clients = New TcpClient(Session("serverName"), Session("PORT_NUM"))
        '    object_userlog.SendData("CONNECT|" & Session("username"), Session("serverName"), clients)
        '    'Log the event *****************************************************
        '    object_userlog.SendDataToLog(Session("loggedUserLog") & "LOG001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & Session("username") & "  Logged on successfully from IP address : Clayton-pc", Session("serverName"), clients)
        '    '************************END****************************************
        '    object_userlog.SendData("CONNECT|" & Session("username"), Session("serverName"), clients)


        'Catch Ex As Exception
        '    MsgBox("Server is not active.  Please start server and try again.", MsgBoxStyle.Exclamation)

        'End Try

    End Sub

    'Public Sub SendToLog()
    '    On Error Resume Next

    '    Dim writer As New IO.StreamWriter(clients.GetStream)
    '    clients.Connect(Session("serverName"), Session("PORT_NUM"))
    '    writer.Write("LOGEVENT|" & Session("dataBaseName") & "!" & Session("ServerLog") & vbCr)
    '    writer.Flush()

    '    Threading.Thread.Sleep(100)
    'End Sub

    Public Sub alert(ByVal errorstring As String, ByVal alertname As String)
        lblError.Text = "<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>  <h4><i class='icon fa fa-ban'></i> " + alertname + "!</h4>" & errorstring & "</div>"
    End Sub
    Public Sub GetFullName(ByVal username As String)

        Try
            'validate username first
            strSQL = "SELECT user_Fullname FROM [dbo].[USERS] where user_ID='" & username & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read

                Session("UserFullName") = drSQL.Item(0).ToString
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

End Class
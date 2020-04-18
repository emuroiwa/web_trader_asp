

Public Class logout1
    Inherits System.Web.UI.Page
    Private object_userlog As New usrlog.usrlog
    'Private dbinfo As New dbparm.dbinfo
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        object_userlog.SendDataToLog(Session("loggedUserLog") & "LOG001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & CType(Session.Item("username"), String) & "  Logged OUT successfully from IP address : ernest", Session("serverName"), Session("client"))
        object_userlog.SendData("FORCEDISCONNECT|" & Session("username"), Session("serverName"), Session("client"))
        Session.Abandon()
        Session.Clear()
        Session.RemoveAll()
        Session.Abandon()
        'Response.Redirect("login.aspx")
    End Sub

End Class
Public Class starting
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'MsgBox(Request.Cookies("ckServerName").Value)
        'Session("username") = Request.Cookies("TDS_USERNAME").Value
        'Session("ConnectionString") = "Network Library=DBMSSOCN;" & _
        '              "Data Source= " & Request.Cookies("ckServerName").Value & ",1433;" & _
        '              "Initial Catalog=" & Request.Cookies("ckDatabaseName").Value & ";" & _
        '              "User ID=uniuser;" & _
        '              "Password=passwordA@@@pass;Connect Timeout=30"


        'Session("password") = Request.Cookies("TDS_PASSWORD").Value
        'Session("SysDate") = Request.Cookies("TDS_SYSDATE").Value
        'MsgBox(Session("ConnectionString"))
        'Response.Redirect("index.aspx")
    End Sub

End Class
Public Class lockscreen
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Abandon()
    End Sub

    Protected Sub btnReLogin_Click(sender As Object, e As EventArgs) Handles btnReLogin.Click
        ''dd
    End Sub
End Class
Public Class lockscreen
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblUserFullName.Text = UserFullName
        Session.Abandon()
    End Sub

    Protected Sub btnReLogin_Click(sender As Object, e As EventArgs) Handles btnReLogin.Click

        If txtPassword.Text = "" Then
            lblError.Text = alert(" PLEASE ENTER YOUR PASSWORD", "Enter Password")
            '' MsgBox(" PLEASE ENTER YOUR USERNAME AND PASSWORD")
            Exit Sub
        End If
        If txtPassword.Text = passContents Then
            lblError.Text = alert(" PLEASE ENTER YOUR USERNAME AND PASSWORD", "Enter Password")
            '' MsgBox(" PLEASE ENTER YOUR USERNAME AND PASSWORD")
            Exit Sub
        End If
    End Sub
End Class
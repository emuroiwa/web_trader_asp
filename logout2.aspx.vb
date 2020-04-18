Public Class logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Abandon()
        '' Me.Close()
        ClientScript.RegisterStartupScript(GetType(Page), "closePage", "<script type='text/JavaScript'>self.close();</script>")
    End Sub

End Class
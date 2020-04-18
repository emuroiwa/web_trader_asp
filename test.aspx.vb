Public Class test
    Inherits System.Web.UI.Page
    Private LogData As New usrlog.usrlog

    Private Sub test_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not IsPostBack Then
            Dim sm As ScriptManager = ScriptManager.GetCurrent(Page)
            If Not sm.IsInAsyncPostBack Then
                Dim css As String = String.Format("<link rel=""stylesheet"" href=""{0}"" type=""text/css"" />", ResolveUrl("haya.css"))
                ScriptManager.RegisterClientScriptBlock(Me, GetType(Button), "Button1", css, False)
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MsgBox1.Show("Confirm Delete", _
                 "Please confirm if you wish to delete this record (Just a demo, nothing is to be deleted really).", Nothing, _
                 New EO.Web.MsgBoxButton("Yes", Nothing, "Delete"), _
                 New EO.Web.MsgBoxButton("Cancel"))
        End If
    End Sub
    Protected Sub MsgBox1_ButtonClick(sender As Object, e As System.Web.UI.WebControls.CommandEventArgs)
        'Use the command name to determine which
        'button was clicked
        If e.CommandName = "Delete" Then
            'Proceed to delete....
            Label1.Text = "Record has been deleted (Just a demo, nothing was really deleted)."
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LogData.Msg("Printed Deal slips for deal ref:", True, "test.aspx", "mmmm", "error")

        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alertScript", "alert('Alter From asp.net page');", True)
    End Sub
End Class
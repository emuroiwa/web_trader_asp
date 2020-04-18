Public Class tables
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' MsgBox(Calendar1.VisibleDate)
        '' MsgBox(Calendar1.SelectedDate.Date)
    End Sub

    Protected Sub Calendar1_SelectionChanged(sender As Object, e As EventArgs) Handles Calendar1.SelectionChanged
        MsgBox(Calendar1.SelectedDate.Date)
    End Sub
End Class
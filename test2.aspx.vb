Public Class test2
    Inherits System.Web.UI.Page
    Private LogData As New usrlog.usrlog


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Protected Sub cbCars_Execute(sender As Object, e As EO.Web.CallbackEventArgs)
        LogData.Msg("Printed Deal slips for deal ref:", True, "test.aspx", "mmmm", "error")

    End Sub



End Class
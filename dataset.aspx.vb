Public Class dataset
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As New DataTable()

        dt.Columns.Add(New DataColumn("SecRef", GetType(String)))
     

       
        Session("loadGrid") = Session("loadGrid") & "|" & TextBox1.Text
        Dim arr() As String = Split(Session("loadGrid"), "|")
        'arr.ElementAt(1).Remove(1)

        Dim arrlen As Integer = arr.Length
        Dim x As Integer

        For x = 1 To arr.Length - 1
            Dim arr2() As String = Split(arr(x), ",")
            Dim dr As DataRow = dt.NewRow()
            dr("SecRef") = arr2(0)
           
            dt.Rows.Add(dr)
        Next

        Me.Grid1.Visible = True

        Grid1.DataSource = dt

        Grid1.DataBind()
    End Sub

End Class
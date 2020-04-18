Imports System.Data.SqlClient

Public Class eo
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim QueryString As String

       
        QueryString = "select dealreference,dealamount,dateentered,startdate,maturitydate,currency,expired from dealsdashboard "


        Try

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(QueryString, cnSQL)
            GridDashBoard.DataSource = cmSQL.ExecuteReader
            GridDashBoard.DataBind()


            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Public ReadOnly Property DeletedItems As DataGridItem()
        Get
            MsgBox(GridDashBoard.ClientSideOnItemSelected.ToString())
        End Get
    End Property
End Class
Imports System.Data.SqlClient

Public Class eoo
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            strSQL = "select * from deals_live join customer on " & _
                                     " deals_live.CustomerNumber=customer.Customer_Number"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GrdDealsMM.DataSource = drSQL
            GrdDealsMM.DataBind()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
         
            MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try
    End Sub

End Class
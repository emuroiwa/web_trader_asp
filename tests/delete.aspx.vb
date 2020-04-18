'Imports System.Data.SqlClient

'Public Class delete
'    Inherits System.Web.UI.Page
'    Private cnSQL As SqlConnection
'    Private strSQL As String
'    Private cmSQL As SqlCommand
'    Private drSQL As SqlDataReader
'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

'        If Not IsPostBack Then
'            BindData()
'        End If
'    End Sub
'    Private Sub BindData()
'        Dim ds As New DataSet()

'        'validate username first
'        strSQL = "SELECT Amount,Customer FROM [dbo].[SECURITYTEMP]"
'        cnSQL = New SqlConnection(Session("ConnectionString"))
'        cnSQL.Open()
'        cmSQL = New SqlCommand(strSQL, cnSQL)
'        drSQL = cmSQL.ExecuteReader()
'        Do While drSQL.Read
'            cmSQL.Fill(ds)
'        Loop


'        ' Close and Clean up objects
'        drSQL.Close()
'        cnSQL.Close()
'        cmSQL.Dispose()
'        cnSQL.Dispose()
'                    sqlAdp.Fill(ds)

'        grdResults.DataSource = ds
'        grdResults.DataBind()

'    End Sub
'    Protected Sub lbDelete_Click(sender As Object, e As EventArgs)
'        Dim lnkbtn As LinkButton = TryCast(sender, LinkButton)
'        Dim grdRow As GridViewRow = TryCast(lnkbtn.NamingContainer, GridViewRow)
'        Dim Cityid As String = grdResults.DataKeys(grdRow.RowIndex).Value.ToString()
'        Using sqlConn As New SqlConnection(Session("ConnectionString"))
'            Using sqlCmd As New SqlCommand()
'                If sqlConn.State = ConnectionState.Closed Then
'                    sqlConn.Open()
'                End If
'                sqlCmd.Connection = sqlConn
'                sqlCmd.CommandType = CommandType.StoredProcedure
'                sqlCmd.Parameters.AddWithValue("@Id", Cityid)
'                sqlCmd.CommandText = "delete FROM [dbo].[SECURITYTEMP]"
'                sqlCmd.ExecuteNonQuery()
'                sqlConn.Close()

'            End Using
'        End Using
'        BindData()
'    End Sub
'End Class
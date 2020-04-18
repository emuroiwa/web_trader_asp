Imports System.Data.SqlClient

Public Class SecIDEnquiry
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private object_userlog As New usrlog.usrlog

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Protected Sub btnFind_Click(sender As Object, e As EventArgs) Handles btnFind.Click


        Try
            strSQL = "select tb_id  from securitypurchase where dealreference='" & Trim(txtSecDealref.Text) & "' "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            If drSQL.HasRows = True Then 'assume the deal is in the matured deals table 
                'The ref was found in the live deals table

                Do While drSQL.Read
                    lblDealSecID.Text = "The Security ID = " & Trim(drSQL.Item(0).ToString) & vbCrLf
                Loop

                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()

            Else
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()

                lblDealSecID.Text = "The deal reference you entered does not exist on the database"
            End If


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("../index.aspx")
    End Sub
End Class
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.Net.WebRequest
Imports sys_ui

Public Class forgotpass
    Inherits System.Web.UI.Page
    'Private dbinfo As New dbparm.dbinfo
    Private object_userlog As New usrlog.usrlog
    'Private connectionString As String = connectionString
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class
    Private serverName As String
    Private dataBaseName As String
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("username") = txtUser.Text
    End Sub

    Protected Sub btnEmaillink_Click(sender As Object, e As EventArgs) Handles btnEmaillink.Click
        Dim mail As New MailMessage()
        Dim smtps As New SmtpClient

        Dim emailAdress As String

        Dim linkUser As String = ConvertStringToHex(Session("username"), System.Text.Encoding.Unicode)


        Try
            strSQL = "select [address] from logins where user_ID = '" & Session("username") & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()


            If drSQL.HasRows = True Then




                smtps.Credentials = New Net.NetworkCredential("claeemt@gmail.com", "00772359133cifo")
                smtps.Port = 587
                smtps.Host = "smtp.gmail.com"
                smtps.EnableSsl = True
                smtps.EnableSsl = True
                While drSQL.Read()

                    emailAdress = drSQL.Item(0).ToString

                    mail.To.Add(emailAdress)


                    mail.From = New MailAddress("claeemt@gmail.com")
                    mail.Subject = "Password Reset"

                    'Response.Redirect("frmMain.aspx?dealcode=BSPL", True)
                    mail.Body = "Please use the Reset link to Reset your password:http://localhost:54144/ResetPass.aspx?user_Id=" & linkUser & ""
                    smtps.Send(mail)
                    'MsgBox("Reset link send")
                    lblsuccess.Text = success("Reset link send, Please check your registered mail and attempt reset", " Reset Link")
                    Response.AddHeader("REFRESH", "1;URL=login.aspx")
                End While
                '<a href=
                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()

            Else

                'MsgBox("No email adress found for user")
                lblError.Text = alert("No email address found for user", " Email")

            End If




        Catch ex As SqlException
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            alert(ex.Message, "Error")
        Catch ex As Exception

        End Try

    End Sub
    Public Function ConvertStringToHex(input As [String], encoding As System.Text.Encoding) As String
        Dim stringBytes As [Byte]() = encoding.GetBytes(input)
        Dim sbBytes As New StringBuilder(stringBytes.Length * 2)
        For Each b As Byte In stringBytes
            sbBytes.AppendFormat("{0:X2}", b)
        Next
        Return sbBytes.ToString()
    End Function
End Class
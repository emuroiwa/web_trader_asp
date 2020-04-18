Imports System.Data.SqlClient
Imports sys_ui
Public Class Decline
    Inherits System.Web.UI.Page
    Private cnSQLx As SqlConnection
    Private strSQLx As String
    Private cmSQLx As SqlCommand
    Private drSQLx As SqlDataReader
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private object_userlog As New usrlog.usrlog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then
                lbldealref.Text = Request.QueryString("dealref")

                'check if the dealer is not about to verify their own deals
                If Trim(lbldealer.Text).Equals(Trim(Session("username"))) Then
                    MsgBox("You cannot verify deals you captured.", MsgBoxStyle.Exclamation, "Verify Deals")
                    Exit Sub

                Else

                    Dim dealCancelled As Boolean

                    'Check if deal is cancelled and revove cancelled status
                    strSQLx = "Select DealCancelled from  deals_live where DealCancelled ='Y' and dealreference = '" & Trim(lbldealref.Text) & "'"
                    cnSQLx = New SqlConnection(Session("ConnectionString"))
                    cnSQLx.Open()
                    cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                    drSQLx = cmSQLx.ExecuteReader()
                    If drSQLx.HasRows = True Then
                        dealCancelled = True
                    End If
                    ' Close and Clean up objects
                    drSQLx.Close()
                    cnSQLx.Close()
                    cmSQLx.Dispose()
                    cnSQLx.Dispose()



                    If dealCancelled = True Then
                        lbldealcancelled.Text = "Yes"
                    Else
                        lbldealcancelled.Text = "No"
                    End If

                    Call LoadDealInfo(lbldealref.Text)

                End If
            Else
                lblError.Text = alert("Select a deal that you want to Decline verification.", "Incomplete informaton")

                btnDecline.Enabled = False
            End If
        End If
    End Sub

    Public Function LoadDealInfo(ByVal ref As String) As String

        Try
            strSQLx = "select * from deals_live where dealreference = '" & ref & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader

            If drSQLx.HasRows = True Then
                Do While drSQLx.Read
                    lbldealer.Text = drSQLx.Item("dealcapturer").ToString

                Loop

            Else
                lblError.Text = alert("Cant find deal details", "No Details in Database")

            End If

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()




        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function

    Protected Sub btnExit0_Click(sender As Object, e As EventArgs) Handles btnExit0.Click
        Response.Redirect("mmdealblotter.aspx")
    End Sub

    Protected Sub btnDecline_Click(sender As Object, e As EventArgs) Handles btnDecline.Click
        If CheckVerificationStatus(Trim(lbldealref.Text)) = True Then
            MsgBox("Someone else has already verified the deal", MsgBoxStyle.Exclamation, "Verification")
            Exit Sub
        End If

        Try
            Dim singleQuote, Character As String
            singleQuote = "'"
            Character = """"

            txtComment.Text = Replace(Trim(txtComment.Text), singleQuote, Character)

            If lblDealCancelled.Text = "Yes" Then
                If MsgBox("This will revoke the cancelled status of the deal. Do you want to proceed?", "Revoke Cancelled Status", MsgBoxStyle.OkCancel) = MsgBoxResult.Yes Then

                    'Check if deal is cancelled and revove cancelled status
                    strSQLx = "begin tran x" & _
                   " update deals_live set DealCancelled ='N', authorisationstatus='" & GetStataus(Trim(lbldealref.Text)) & "' where dealreference = '" & Trim(lbldealref.Text) & "'" & _
                     " insert DealStatusDecline values('" & Trim(lbldealref.Text) & "','" & Session("username") & "','DA','" & Trim(txtComment.Text) & "','" & Now & "')" & _
                      " commit tran x"

                    'Log the event *****************************************************
                    object_userlog.SendDataToLog(Session("username") & "DECLIN" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " Revoked deal cancelled status ", Session("serverName"), Session("client"))
                    '************************END****************************************

                End If

            Else
                strSQLx = "begin tran x" & _
                " update deals_live set authorisationstatus='DV' where dealreference='" & Trim(lbldealref.Text) & "' " & _
                " insert DealStatusDecline values('" & Trim(lbldealref.Text) & "','" & Session("username") & "','DV','" & Trim(txtComment.Text) & "','" & Now & "')" & _
                 " commit tran x"
            End If

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            MsgBox("Deal verification declined", MsgBoxStyle.Information, "Verification")



        Catch ex As NullReferenceException

        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try
    End Sub
    'Check if someone else verified the deal whilst waiting to authorise
    Public Function CheckVerificationStatus(ByVal dealref As String) As Boolean
        Dim x As Boolean

        Try
            strSQL = "select AuthorisationStatus from deals_live where dealreference='" & Trim(dealref) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQLx.Read
                If Trim(drSQL.Item(0).ToString) = "V" Or Trim(drSQL.Item(0).ToString) = "DV" Then
                    x = True
                Else
                    x = False
                End If
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As NullReferenceException

        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try

        Return x


    End Function
  
    Private Function GetStataus(ByVal ref As String) As String
        Dim res As String

        Try

            strSQLx = "select authorise1,authorise2 from DEALAUTHORISATIONS where dealreference='" & ref & "'"


            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQL.Read
                If Trim(drSQLx.Item(0).ToString) = "" Or Trim(drSQLx.Item(1).ToString) = "" Then
                    res = "P"
                Else
                    res = "A"
                End If
            Loop

            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ex As NullReferenceException

        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try

        Return res

    End Function
End Class
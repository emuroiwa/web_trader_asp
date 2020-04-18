Imports System.Data.SqlClient
Imports sys_ui
Public Class changeInstructions
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private cnSQLx As SqlConnection
    Private strSQLx As String
    Private cmSQLx As SqlCommand
    Private drSQLx As SqlDataReader
    Private object_userlog As New usrlog.usrlog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then
                txtDealref.Text = Request.QueryString("dealref")

                Dim Dealstructure As String = getDealOtherCharacteristics(Trim(txtDealref.Text))
                Dim dealref As String = Trim(txtDealref.Text)
                LoadInstructions(Dealstructure)

                txtOtherStart.Text = getDealInstr(Trim(txtDealref.Text), "Instruction")
                txtOtherMaturity.Text = getDealInstr(Trim(txtDealref.Text), "InstructionMat")

            Else
                lblError.Text = alert("Select a deal that you want to change tax.", "Incomplete informaton")
                btnUpdate.Enabled = False

            End If
        End If
    End Sub

    Protected Sub cmbInstructionStart_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbInstructionStart.SelectedIndexChanged
        txtOtherStart.Text = cmbInstructionStart.Text
    End Sub

    Protected Sub cmbInstructionMaturity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbInstructionMaturity.SelectedIndexChanged
        txtOtherMaturity.Text = cmbInstructionMaturity.Text
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If txtOtherStart.Text = "" Then
            MsgBox("Deal Start instruction not entered", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If txtOtherMaturity.Text = "" Then
            MsgBox("Deal maturity instruction not entered", MsgBoxStyle.Critical)
            Exit Sub
        End If

        Dim cnSQL As SqlConnection
        Dim cmSQL As SqlCommand
        Dim drSQL As SqlDataReader
        Dim strSQL As String

        txtOtherMaturity.Text = Replace(Trim(txtOtherMaturity.Text), "'", """")
        txtOtherMaturity.Text = Replace(Trim(txtOtherMaturity.Text), "’", """")
        txtOtherMaturity.Text = Replace(Trim(txtOtherMaturity.Text), "&", "and")

        txtOtherStart.Text = Replace(Trim(txtOtherStart.Text), "'", """")
        txtOtherStart.Text = Replace(Trim(txtOtherStart.Text), "’", """")
        txtOtherStart.Text = Replace(Trim(txtOtherStart.Text), "&", "and")


        Try

            strSQL = "Begin tran X" & _
                " update deals_live set InstructionMat='" & Trim(txtOtherMaturity.Text) & "',Instruction='" & Trim(txtOtherStart.Text) & "' where dealreference='" & Trim(txtDealref.Text) & "'" & _
                " update securitiesconfirmations set InstructionMat='" & Trim(txtOtherMaturity.Text) & "',Instruction='" & Trim(txtOtherStart.Text) & "' where dealreference='" & Trim(txtDealref.Text) & "'" & _
                " commit tran X"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            MsgBox("Instructions updated", MsgBoxStyle.Information)

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()



        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("mmdealblotter.aspx")
    End Sub
    Private Function getDealOtherCharacteristics(ref As String) As String
        Dim x As String = ""
        Dim cnSQL As SqlConnection
        Dim cmSQL As SqlCommand
        Dim drSQL As SqlDataReader
        Dim strSQL As String

        Try

            strSQL = "select OtherCharacteristics from deals_live where dealreference='" & ref & "' "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = Trim(drSQL.Item(0).ToString)

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()



        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Return x
    End Function
    Public Sub LoadInstructions(AppType As String)

        Select Case AppType

            Case "Basic Deposit" 'Load Instructions Deposit Deal
                Try
                    'validate username first
                    strSQL = "Select * from instrucparam join dealinstr on instrucparam.instid= dealinstr.instid where appid='MNKDEP'"
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader()

                    If drSQL.HasRows = True Then
                        Do While drSQL.Read
                            If drSQL.Item("purpose").ToString.Equals("M") Then
                                cmbInstructionMaturity.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                            ElseIf drSQL.Item("purpose").ToString.Equals("I") Then
                                cmbInstructionStart.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                            End If
                        Loop


                    End If

                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()

                Catch ex As SqlException
                    MsgBox(ex.Message, MsgBoxStyle.Critical)
                End Try

            Case "Basic Loan" 'Load Instructions for a placement deal

                Try
                    'validate username first
                    strSQL = "Select * from instrucparam join dealinstr on instrucparam.instid= dealinstr.instid where appid='MNKPLAC'"
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader()

                    If drSQL.HasRows = True Then
                        Do While drSQL.Read
                            If drSQL.Item("purpose").ToString.Equals("I") Then
                                cmbInstructionStart.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                            ElseIf drSQL.Item("purpose").ToString.Equals("M") Then
                                cmbInstructionMaturity.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                            End If
                        Loop
                    End If

                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()

                Catch ex As SqlException
                    MsgBox(ex.Message, MsgBoxStyle.Critical)
                End Try

            Case "Discount Purchase" 'Load Instructions for a security purchase deal

                Try
                    'validate username first
                    strSQL = "Select * from instrucparam join dealinstr on instrucparam.instid= dealinstr.instid where appid='MNKSECPUR'"
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader()

                    If drSQL.HasRows = True Then
                        Do While drSQL.Read
                            If drSQL.Item("purpose").ToString.Equals("M") Then
                                cmbInstructionMaturity.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                            ElseIf drSQL.Item("purpose").ToString.Equals("I") Then
                                cmbInstructionStart.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                            End If
                        Loop


                    End If

                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()

                Catch ex As SqlException
                    MsgBox(ex.Message, MsgBoxStyle.Critical)

                End Try


            Case "Discount Sale"

                Try
                    'Load Instructions

                    'validate username first
                    strSQL = "Select * from instrucparam join dealinstr on instrucparam.instid= dealinstr.instid where appid='MNKSECSAL'"
                    cnSQL = New SqlConnection(Session("ConnectionString"))
                    cnSQL.Open()
                    cmSQL = New SqlCommand(strSQL, cnSQL)
                    drSQL = cmSQL.ExecuteReader()

                    If drSQL.HasRows = True Then
                        Do While drSQL.Read
                            If drSQL.Item("purpose").ToString.Equals("M") Then
                                cmbInstructionMaturity.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                            ElseIf drSQL.Item("purpose").ToString.Equals("I") Then
                                cmbInstructionStart.Items.Add(Trim(drSQL.Item("instdesc").ToString))
                            End If
                        Loop

                    End If



                    ' Close and Clean up objects
                    drSQL.Close()
                    cnSQL.Close()
                    cmSQL.Dispose()
                    cnSQL.Dispose()

                Catch ex As Exception

                End Try
        End Select



      
    End Sub
    Private Function getDealInstr(ref As String, instrType As String) As String
        Dim x As String = ""
        Dim cnSQL As SqlConnection
        Dim cmSQL As SqlCommand
        Dim drSQL As SqlDataReader
        Dim strSQL As String

        Try

            strSQL = "select " & instrType & " from deals_live where dealreference='" & ref & "' "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                x = Trim(drSQL.Item(0).ToString)

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()



        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Return x
    End Function

End Class
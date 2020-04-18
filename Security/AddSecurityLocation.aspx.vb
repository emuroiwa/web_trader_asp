Imports System.Data.SqlClient

Public Class AddSecurityLocation
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
        Call loadCollateralLocation()
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        txtID.Enabled = True
        txtID.Text = ""
        txtDescription.Text = ""
        txtAddress.Text = ""
    End Sub
    Protected Sub GrdLocation_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call

        If e.CommandName = "Edit" Then
            Dim s As String = String.Empty
            txtID.Text = e.Item.Cells(1).Value.ToString()
            txtDescription.Text = e.Item.Cells(2).Value.ToString()
            Call EditLocation(txtID.Text, txtDescription.Text)

        End If
        If e.CommandName = "Delete" Then
            Dim s As String = String.Empty
            txtID.Text = e.Item.Cells(1).Value.ToString()

            Call DeleteLocation(txtID.Text, txtDescription.Text)
        End If

    End Sub

    Protected Sub btnExit0_Click(sender As Object, e As EventArgs) Handles btnExit0.Click
        Response.Redirect("index.aspx")
    End Sub
    Private Sub loadCollateralLocation()
        Try
            strSQL = "select * from COLL_COLLATERAL_LOCATION"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GrdLocation.DataSource = drSQL
            GrdLocation.DataBind()


            Do While drSQL.Read
                'Dim itmx As New ListViewItem(Trim(drSQL.Item(0).ToString))
                'itmx.SubItems.Add(Trim(drSQL.Item(1).ToString))

                'lstLocation.Items.Add(itmx)

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub
    Private Function GetLocationAddress(ByVal id As Integer) As String
        Dim res As String = ""
        Try
            strSQL = "select LocationAddress from COLL_COLLATERAL_LOCATION where LocationID=" & id & ""

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            'lstLocation.Items.Clear()

            Do While drSQL.Read
                res = Trim(drSQL.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

        Return res
    End Function
    Private Sub EditLocation(ByVal LocID As String, ByVal locdesc As String)
        Try

            txtID.Enabled = False
            txtDescription.Text = Trim(locdesc)
            txtID.Text = Trim(LocID)
            txtAddress.Text = GetLocationAddress(Int(txtID.Text))

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DeleteLocation(ByVal LocID As String, ByVal locdesc As String)

        'If MessageBox.Show("Confirm you want to delete the collateral Location", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
        Try

            strSQL = " delete COLL_COLLATERAL_LOCATION where LocationID='" & Int(LocID) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            MsgBox("Collataral location deleted", MsgBoxStyle.Information)

            txtID.Enabled = True
            txtID.Text = ""
            txtDescription.Text = ""
            txtAddress.Text = ""


            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'Log the event *****************************************************loggedUserLog formated for log
            object_userlog.SendDataToLog(Session("username") & "COLT01" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Collateral location deleted - ID : " & _
            Trim(LocID) & " Location Description : " & Trim(Trim(locdesc)) _
             , Session("serverName"), Session("client"))
            '************************END****************************************


            Call loadCollateralLocation()

        Catch ex As SqlException
            ' MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
        'End If
    End Sub

    Protected Sub btnSave0_Click(sender As Object, e As EventArgs) Handles btnSave0.Click
        Try
            If txtID.Text = "" Then
                MsgBox("Please enter a unique ID for the collateral location upto 30 characters", MsgBoxStyle.Information)
                Exit Sub
            End If

            If txtDescription.Text = "" Then
                MsgBox("Please enter a description for the collateral location.", MsgBoxStyle.Information)
                Exit Sub
            End If

            If txtAddress.Text = "" Then
                MsgBox("Please enter an address for the collateral location.", MsgBoxStyle.Information)
                Exit Sub
            End If

            strSQL = " begin tran X" & _
                     " if exists (select * from COLL_COLLATERAL_LOCATION where LocationID='" & Trim(txtID.Text) & "') " & _
                     " update COLL_COLLATERAL_LOCATION set locationDescription='" & Trim(txtDescription.Text) & "',LocationAddress='" & Trim(txtAddress.Text) & "' where" & _
                     " locationID='" & Trim(txtID.Text) & "'" & _
                     " else" & _
                     " insert COLL_COLLATERAL_LOCATION values('" & Trim(txtID.Text) & "','" & Trim(txtDescription.Text) & "','" & Trim(txtAddress.Text) & "')" & _
                     " commit tran X"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            MsgBox("Collataral Location Saved", MsgBoxStyle.Information)

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'Log the event *****************************************************loggedUserLog formated for log
            object_userlog.SendDataToLog(Session("username") & "COLT01" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Collateral location Added -  ID : " & Trim(txtID.Text) & _
            " Collateral Location Description :-  " & Trim(txtDescription.Text) & " Collateral Location Address : -" & Trim(txtAddress.Text), Session("serverName"), Session("client"))
            '************************END****************************************

            txtID.Enabled = True
            txtID.Text = ""
            txtDescription.Text = ""
            txtAddress.Text = ""

            Call loadCollateralLocation()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub
End Class
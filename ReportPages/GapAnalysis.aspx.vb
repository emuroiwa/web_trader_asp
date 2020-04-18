Imports System.Data.SqlClient

Public Class GapAnalysis
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
 
   
   
  
    Public REPORTNAME As String
    Private curr As String
    Private object_userlog As New usrlog.usrlog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetCurrency()
        End If
    End Sub
    Private Sub GetCurrency()
        Try

            strSQL = "select currencycode from astval"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader



            Do While drSQL.Read
                cmbCurrency.Items.Add(Trim(drSQL.Item(0)))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
    End Sub
    Private Sub MAT1toX(ByVal grpID As String)


        Dim Total As Decimal = 0
        Dim Sum_1to7 As Decimal = 0
        Dim Sum_8to14 As Decimal = 0
        Dim Sum_15to21 As Decimal = 0
        Dim Sum_22to30 As Decimal = 0
        Dim Sum_31to60 As Decimal = 0
        Dim Sum_61to90 As Decimal = 0
        Dim Sum_Over90 As Decimal = 0
        Dim RowDescription As String
        Dim itemType As String


        Try

            'Create Itrem row with zeros on amounts
            strSQL = "select [description],[type] from gapanalysisgrp where groupid='" & grpID & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                RowDescription = drSQL.Item(0).ToString
                itemType = drSQL.Item(1).ToString
                CreateGapRow(Total, 0, drSQL.Item(0), drSQL.Item(1))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            '***************************************************************************************

            Dim cnSQLx As SqlConnection
            Dim strSQLx As String
            Dim cmSQLx As SqlCommand
            Dim drSQLx As SqlDataReader

            'Get items in period 1 - 7 ------------------------------------------------------------------
            strSQLx = "select sum(maturityamount),[description],[type] from deals_live join gapdealcodes" & _
                     " on deals_live.dealtype=gapdealcodes.dealcode " & _
                     " join gapanalysisgrp on gapdealcodes.groupid=gapanalysisgrp.groupid" & _
                     " where daystomaturity<=7 and gapanalysisgrp.groupid='" & grpID & "' and" & _
                     " currency='" & cmbCurrency.Text & "' group by gapanalysisgrp.description, gapanalysisgrp.type"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader

            If drSQLx.HasRows = True Then
                Do While drSQLx.Read
                    Total = Total + drSQLx.Item(0) ' total for the line item across
                    Sum_1to7 = drSQLx.Item(0)
                Loop

                ' Close and Clean up objects
                drSQLx.Close()
                cnSQLx.Close()
                cmSQLx.Dispose()
                cnSQLx.Dispose()
            End If


            Dim cnSQLa As SqlConnection
            Dim strSQLa As String
            Dim cmSQLa As SqlCommand
            Dim drSQLa As SqlDataReader


            'get items in period 8 - 14------------------------------------------------------------------
            strSQLa = "select sum(maturityamount)from deals_live join gapdealcodes" & _
                               " on deals_live.dealtype=gapdealcodes.dealcode " & _
                               " join gapanalysisgrp on gapdealcodes.groupid=gapanalysisgrp.groupid" & _
                               " where daystomaturity>=8 and daystomaturity<=14 and gapanalysisgrp.groupid='" & grpID & "'" & _
                               " and  currency='" & cmbCurrency.Text & "' group by gapanalysisgrp.description, gapanalysisgrp.type"

            cnSQLa = New SqlConnection(Session("ConnectionString"))
            cnSQLa.Open()
            cmSQLa = New SqlCommand(strSQLa, cnSQLa)
            drSQLa = cmSQLa.ExecuteReader

            If drSQLa.HasRows = True Then
                Do While drSQLa.Read

                    Total = Total + drSQLa.Item(0) ' total for the line item across
                    Sum_8to14 = drSQLa.Item(0)
                Loop
            End If

            ' Close and Clean up objects
            drSQLa.Close()
            cnSQLa.Close()
            cmSQLa.Dispose()
            cnSQLa.Dispose()

            Dim cnSQLy As SqlConnection
            Dim strSQLy As String
            Dim cmSQLy As SqlCommand
            Dim drSQLy As SqlDataReader

            'get items in period 15 - 21------------------------------------------------------------------
            strSQLy = "select sum(maturityamount)from deals_live join gapdealcodes" & _
                               " on deals_live.dealtype=gapdealcodes.dealcode " & _
                               " join gapanalysisgrp on gapdealcodes.groupid=gapanalysisgrp.groupid" & _
                               " where daystomaturity>=15 and daystomaturity<=21 and gapanalysisgrp.groupid='" & grpID & "'" & _
                               " and  currency='" & cmbCurrency.Text & "' group by gapanalysisgrp.description, gapanalysisgrp.type"

            cnSQLy = New SqlConnection(Session("ConnectionString"))
            cnSQLy.Open()
            cmSQLy = New SqlCommand(strSQLy, cnSQLy)
            drSQLy = cmSQLy.ExecuteReader

            If drSQLy.HasRows = True Then
                Do While drSQLy.Read

                    Total = Total + drSQLy.Item(0) ' total for the line item across
                    Sum_15to21 = drSQLy.Item(0)
                Loop
            End If

            ' Close and Clean up objects
            drSQLy.Close()
            cnSQLy.Close()
            cmSQLy.Dispose()
            cnSQLy.Dispose()



            Dim cnSQLz As SqlConnection
            Dim strSQLz As String
            Dim cmSQLz As SqlCommand
            Dim drSQLz As SqlDataReader

            'get items in period 22 - 30------------------------------------------------------------------
            strSQLz = "select sum(maturityamount)from deals_live join gapdealcodes" & _
                               " on deals_live.dealtype=gapdealcodes.dealcode " & _
                               " join gapanalysisgrp on gapdealcodes.groupid=gapanalysisgrp.groupid" & _
                               " where daystomaturity>=22 and daystomaturity<=30 and gapanalysisgrp.groupid='" & grpID & "'" & _
                               " and  currency='" & cmbCurrency.Text & "' group by gapanalysisgrp.description, gapanalysisgrp.type"

            cnSQLz = New SqlConnection(Session("ConnectionString"))
            cnSQLz.Open()
            cmSQLz = New SqlCommand(strSQLz, cnSQLz)
            drSQLz = cmSQLz.ExecuteReader

            If drSQLz.HasRows = True Then
                Do While drSQLz.Read

                    Total = Total + drSQLz.Item(0) ' total for the line item across
                    Sum_22to30 = drSQLz.Item(0)
                Loop
           
            End If

            ' Close and Clean up objects
            drSQLz.Close()
            cnSQLz.Close()
            cmSQLz.Dispose()
            cnSQLz.Dispose()



            Dim cnSQLb As SqlConnection
            Dim strSQLb As String
            Dim cmSQLb As SqlCommand
            Dim drSQLb As SqlDataReader

            'get items in period 31 - 60------------------------------------------------------------------
            strSQLb = "select sum(maturityamount)from deals_live join gapdealcodes" & _
                               " on deals_live.dealtype=gapdealcodes.dealcode " & _
                               " join gapanalysisgrp on gapdealcodes.groupid=gapanalysisgrp.groupid" & _
                               " where daystomaturity>=31 and daystomaturity<=60 and gapanalysisgrp.groupid='" & grpID & "'" & _
                               " and currency='" & cmbCurrency.Text & "' group by gapanalysisgrp.description, gapanalysisgrp.type"

            cnSQLb = New SqlConnection(Session("ConnectionString"))
            cnSQLb.Open()
            cmSQLb = New SqlCommand(strSQLb, cnSQLb)
            drSQLb = cmSQLb.ExecuteReader

            If drSQLb.HasRows = True Then
                Do While drSQLb.Read

                    Total = Total + drSQLb.Item(0) ' total for the line item across
                    Sum_31to60 = drSQLb.Item(0)
                Loop
           
            End If

            ' Close and Clean up objects
            drSQLb.Close()
            cnSQLb.Close()
            cmSQLb.Dispose()
            cnSQLb.Dispose()



            Dim cnSQLc As SqlConnection
            Dim strSQLc As String
            Dim cmSQLc As SqlCommand
            Dim drSQLc As SqlDataReader

            'get items in period 61 - 90------------------------------------------------------------------
            strSQLc = "select sum(maturityamount)from deals_live join gapdealcodes" & _
                               " on deals_live.dealtype=gapdealcodes.dealcode " & _
                               " join gapanalysisgrp on gapdealcodes.groupid=gapanalysisgrp.groupid" & _
                               " where daystomaturity>=61 and daystomaturity<=90 and gapanalysisgrp.groupid='" & grpID & "'" & _
                               " and currency='" & cmbCurrency.Text & "' group by gapanalysisgrp.description, gapanalysisgrp.type"

            cnSQLc = New SqlConnection(Session("ConnectionString"))
            cnSQLc.Open()
            cmSQLc = New SqlCommand(strSQLc, cnSQLc)
            drSQLc = cmSQLc.ExecuteReader

            If drSQLc.HasRows = True Then
                Do While drSQLc.Read
                    Total = Total + drSQLc.Item(0) ' total for the line item across
                    Sum_61to90 = drSQLc.Item(0)
                Loop
           
            End If

            ' Close and Clean up objects
            drSQLc.Close()
            cnSQLc.Close()
            cmSQLc.Dispose()
            cnSQLc.Dispose()



            Dim cnSQLv As SqlConnection
            Dim strSQLv As String
            Dim cmSQLv As SqlCommand
            Dim drSQLv As SqlDataReader
            'get items in period over 90------------------------------------------------------------------
            strSQLv = "select sum(maturityamount)from deals_live join gapdealcodes" & _
                               " on deals_live.dealtype=gapdealcodes.dealcode " & _
                               " join gapanalysisgrp on gapdealcodes.groupid=gapanalysisgrp.groupid" & _
                               " where daystomaturity>90 and gapanalysisgrp.groupid='" & grpID & "'" & _
                               "  and currency='" & cmbCurrency.Text & "' group by gapanalysisgrp.description, gapanalysisgrp.type"

            cnSQLv = New SqlConnection(Session("ConnectionString"))
            cnSQLv.Open()
            cmSQLv = New SqlCommand(strSQLv, cnSQLv)
            drSQLv = cmSQLv.ExecuteReader

            If drSQLv.HasRows = True Then
                Do While drSQLv.Read
                    Total = Total + drSQLv.Item(0) ' total for the line item across
                    Sum_Over90 = drSQLv.Item(0)
                Loop
           
            End If

            ' Close and Clean up objects
            drSQLv.Close()
            cnSQLv.Close()
            cmSQLv.Dispose()
            cnSQLv.Dispose()


            'Update entire row with values
            UpdateGapRow(Total, itemType, RowDescription, Sum_1to7, Sum_8to14, Sum_15to21, Sum_22to30, Sum_31to60, Sum_61to90, Sum_Over90)

        Catch ex As SqlException
                      'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
    End Sub

    Private Sub ResetTable()

        strSQL = "delete gapwork where userid='" & Trim(Session("username")) & "'"

        cnSQL = New SqlConnection(Session("ConnectionString"))
        cnSQL.Open()
        cmSQL = New SqlCommand(strSQL, cnSQL)
        drSQL = cmSQL.ExecuteReader

        ' Close and Clean up objects
        drSQL.Close()
        cnSQL.Close()
        cmSQL.Dispose()
        cnSQL.Dispose()

    End Sub

    Public Sub GetGapGrid()



        strSQL = "SELECT * FROM GAPWORK where userId='" & Trim(Session("username")) & "' and currency='" & Trim(cmbCurrency.SelectedValue.ToString) & "'"

        cnSQL = New SqlConnection(Session("ConnectionString"))
        cnSQL.Open()
        cmSQL = New SqlCommand(strSQL, cnSQL)
        drSQL = cmSQL.ExecuteReader()
        GridGap.DataSource = drSQL
        GridGap.DataBind()

        ' Close and Clean up objects
        drSQL.Close()
        cnSQL.Close()
        cmSQL.Dispose()
        cnSQL.Dispose()
    End Sub

    Private Sub CreateGapRow(ByVal Total As Decimal, ByVal period1to7 As Decimal, ByVal ItemDescription As String, ByVal ItemType As String)


        Dim cnSQLp As SqlConnection
        Dim strSQLp As String
        Dim cmSQLp As SqlCommand
        Dim drSQLp As SqlDataReader

        'delete existing records for this user
        Try

            Dim period8to14 As Decimal = 0
            Dim period15to21 As Decimal = 0
            Dim period22to30 As Decimal = 0
            Dim period31to60 As Decimal = 0
            Dim period61to90 As Decimal = 0
            Dim periodOver90 As Decimal = 0




            'Save the new data
            strSQLp = "insert into gapwork values ('" & ItemType & "','" & ItemDescription & "','" & period1to7 & "','" & period8to14 & "','" & period15to21 & "','" & _
                                                                 period22to30 & "','" & period31to60 & "','" & period61to90 & "','" & periodOver90 & "','" & Total & "','" & Trim(Session("username")) & "','" & cmbCurrency.Text & "')"

            cnSQLp = New SqlConnection(Session("ConnectionString"))
            cnSQLp.Open()
            cmSQLp = New SqlCommand(strSQLp, cnSQLp)
            drSQLp = cmSQLp.ExecuteReader

            ' Close and Clean up objects
            drSQLp.Close()
            cnSQLp.Close()
            cmSQLp.Dispose()
            cnSQLp.Dispose()



        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
    End Sub


    Private Sub UpdateGapRow(ByVal Total As Decimal, Itemtype As String, ByVal RowDescription As String, ByVal Sum_1to7 As Decimal, ByVal Sum_8to14 As Decimal, _
                             ByVal Sum_15to21 As Decimal, ByVal Sum_22to30 As Decimal, ByVal Sum_31to60 As Decimal, ByVal Sum_61to90 As Decimal, ByVal Sum_Over90 As Decimal)


        Try


            'Save the new data
            strSQL = " update gapwork set period1to7=" & Sum_1to7 & ",period8to14 =" & Sum_8to14 & ", period15to21=" & Sum_15to21 & ", period22to30=" & Sum_22to30 & _
                     ", period31to60=" & Sum_31to60 & ", period61to90=" & Sum_61to90 & ", periodover90=" & Sum_Over90 & ", total=" & Total & _
                     " where itemtype='" & Itemtype & "' and itemdescription='" & RowDescription & "' and userid='" & Session("username") & "' and currency='" & _
                       cmbCurrency.Text & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()



        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************

        End Try
    End Sub

    Protected Sub btnGap_Click(sender As Object, e As EventArgs) Handles btnGap.Click
        Dim cnSQLo As SqlConnection
        Dim cmSQLo As SqlCommand
        Dim drSQLo As SqlDataReader
        Dim strSQLo As String

        ResetTable()
        Try
            strSQLo = "select groupid from gapanalysisgrp"

            cnSQLo = New SqlConnection(Session("ConnectionString"))
            cnSQLo.Open()
            cmSQLo = New SqlCommand(strSQLo, cnSQLo)
            drSQLo = cmSQLo.ExecuteReader


            Do While drSQLo.Read
                Call MAT1toX(Trim(drSQLo.Item(0).ToString))
            Loop


            ' Close and Clean up objects
            drSQLo.Close()
            cnSQLo.Close()
            cmSQLo.Dispose()
            cnSQLo.Dispose()

            GetGapGrid()
            btnPrint.Enabled = True

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "DoUnclassified", "error")
            '************************END****************************************
        End Try
    End Sub

    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Response.Redirect("ReportViewer.aspx?report=Gap1")

    End Sub
End Class
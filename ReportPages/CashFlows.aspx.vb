Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing

Public Class CashFlows


    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private prodlist As String = ""
    Private curr As String
    Private object_userlog As New usrlog.usrlog
    ' Private prodlist As String
    Private SubtotalDealAmount As Decimal
    Private SubtotalMaturityAmount As Decimal
    Private SubtotalNetinterest As Decimal

    Private grandTotalDealAmount As Decimal
    Private grandTotalMaturityAmount As Decimal
    Private grandTotalNetinterest As Decimal

    Private SubtotalDealAmount1 As Decimal
    Private SubtotalMaturityAmount1 As Decimal
    Private SubtotalNetinterest1 As Decimal

    Private grandTotalDealAmount1 As Decimal
    Private grandTotalMaturityAmount1 As Decimal
    Private grandTotalNetinterest1 As Decimal

    Private NetPos As Decimal
    Private listItemsXTotal As Integer
    Private listItemsXTotal1 As Integer

    Private printFont As Font
    Private streamToPrint As StreamReader


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetCurrency()
            loadDays()
            GetDealCodes()
            selectiondate.Text = CDate(Session("SysDate"))
        End If
    End Sub
    Private Sub GetDealCodes()
        Dim cnSQLX As SqlConnection
        Dim strSQLX As String
        Dim cmSQLX As SqlCommand
        Dim drSQLX As SqlDataReader
        Try
            strSQLX = ("select Deal_code from dealtypes where [application] in('MM','SS')")
            cnSQLX = New SqlConnection(Session("ConnectionString"))
            cnSQLX.Open()
            cmSQLX = New SqlCommand(strSQLX, cnSQLX)
            drSQLX = cmSQLX.ExecuteReader()

            GridDeals.DataSource = drSQLX
            GridDeals.DataBind()

            ' Close and Clean up objects
            drSQLX.Close()
            cnSQLX.Close()
            cmSQLX.Dispose()
            cnSQLX.Dispose()

        Catch ec As Exception
            '   Response.Write("<script>  alert('" & ec.Message & " . Error.') </script>")
            'Log the event *****************************************************
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")
            '************************END****************************************
        End Try

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


        Catch xr As Exception
            'Log the event *****************************************************
            object_userlog.Msg(xr.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & xr.Message, "error")
            '************************END****************************************
        End Try
    End Sub
    Private Sub loadDays()
        Try

            strSQL = "select * from CshFlwPar"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader



            Do While drSQL.Read
                cmbDays.Items.Add(Trim(drSQL.Item(0)))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch xr As Exception

            'Log the event *****************************************************
            object_userlog.Msg(xr.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & xr.Message, "error")
            '************************END****************************************
        End Try
    End Sub
    Private Sub getdeals()

        For Each item As EO.Web.GridItem In GridDeals.CheckedItems
            prodlist = prodlist & ",'" & item.Cells(1).Value & "'"

        Next
        prodlist = Trim(Mid(prodlist, 2, Len(prodlist)))
    End Sub
    Protected Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        getdeals()
        '  MsgBox(prodlist)
        Dim x As Integer

        Dim itmCount As Integer = 0

        SubtotalDealAmount = 0
        SubtotalMaturityAmount = 0
        SubtotalNetinterest = 0
        SubtotalDealAmount1 = 0
        SubtotalMaturityAmount1 = 0
        SubtotalNetinterest1 = 0

        grandTotalDealAmount = 0
        grandTotalMaturityAmount = 0
        grandTotalNetinterest = 0
        grandTotalDealAmount1 = 0
        grandTotalMaturityAmount1 = 0
        grandTotalNetinterest1 = 0
        listItemsXTotal = 0
        NetPos = 0

        DealAmount.Text = 0
        MaturityAmount.Text = 0
        NetInterest.Text = 0

        x = Int(cmbDays.Text)
        're-initialise subtotal variables
        SubtotalDealAmount = 0
        SubtotalMaturityAmount = 0
        SubtotalNetinterest = 0
        SubtotalDealAmount1 = 0
        SubtotalMaturityAmount1 = 0
        SubtotalNetinterest1 = 0
        Call DeleteRecs()
        Call GetInflows(1, x, prodlist)
        Call GetOutflows(1, x, prodlist)

        'insert the grand totals





        'Get the net Positions
        DealAmount.Text = Format(grandTotalDealAmount - grandTotalDealAmount1, "###,###,###.00")
        MaturityAmount.Text = Format(grandTotalMaturityAmount - grandTotalMaturityAmount1, "###,###,###.00")
        NetInterest.Text = Format(grandTotalNetinterest - grandTotalNetinterest1, "###,###,###.00")
        Call PrepareReportSummaries()
    End Sub
      Private Function GetInflows(ByVal nummin As Integer, ByVal nummax As Integer, ByVal prodlist As String)
        Dim daysToMaturity As String = ""
        Dim daysToMaturityNew As String = ""
        Dim x As Boolean = False

        Try
            strSQL = "Select * from alldealsall join dealtypes on alldealsall.dealtype=dealtypes.deal_code " & _
                   " where daystomaturity  between '" & nummin & "' and '" & nummax & "' and dealbasictype='L'" & _
                   " and alldealsall.othercharacteristics<>'Discount Sale' and deal_code in(" & prodlist & ")" & _
                   " and alldealsall.currency='" & Trim(cmbCurrency.Text) & "' and dealcancelled<>'Y'" & _
                   " UNION" & _
                   " Select * from alldealsall join dealtypes on alldealsall.dealtype=dealtypes.deal_code " & _
                   " where maturitydate between '" & CDate(selectiondate.Text) & "' and '" & CDate(Session("SysDate")) & "' and dealbasictype='L'" & _
                   " and alldealsall.othercharacteristics<>'Discount Sale' and deal_code in(" & prodlist & ")" & _
                   " and alldealsall.currency='" & Trim(cmbCurrency.Text) & "' and dealcancelled<>'Y' order by daystomaturity  asc"
            'validate username first
            'strSQL = "Select * from alldealsall join dealtypes on alldealsall.dealtype=dealtypes.deal_code " & _
            '     " where daystomaturity between '" & nummin & "' and '" & nummax & "' and dealbasictype='L'" & _
            '     " and alldealsall.othercharacteristics<>'Discount Sale' and  deal_code in(" & prodlist & ")" & _
            '     " and alldealsall.currency='" & Trim(cmbCurrency.Text) & "' and dealcancelled<>'Y'" & _
            '      " UNION" & _
            '     " Select * from alldealsall join dealtypes on alldealsall.dealtype=dealtypes.deal_code " & _
            '     " where maturitydate between '" & CDate(selectiondate.Text) & "' and '" & CDate(Session("SysDate")) & "' and dealbasictype='L'" & _
            '     " and alldealsall.othercharacteristics<>'Discount Sale' and deal_code in (" & prodlist & ")" & _
            '     " and alldealsall.currency='" & Trim(cmbCurrency.Text) & "' and dealcancelled<>'Y' order by daystomaturity  asc"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Dim listItemsX As Integer = 0
            Dim dt As New DataTable()
            dt.Columns.Add(New DataColumn("dealref", GetType(String)))
            dt.Columns.Add(New DataColumn("dealamnt", GetType(String)))
            dt.Columns.Add(New DataColumn("maturityamount", GetType(String)))
            dt.Columns.Add(New DataColumn("netinterest", GetType(String)))
            dt.Columns.Add(New DataColumn("days", GetType(String)))
            dt.Columns.Add(New DataColumn("maturitydate", GetType(String)))

            Do While drSQL.Read


                If Trim(drSQL.Item("daystomaturity").ToString) <> daysToMaturity And daysToMaturity <> "" Then

                    Dim dra1 As DataRow = dt.NewRow()
                    dra1("dealref") = "<font color='red'>Totals</font>"
                    dra1("dealamnt") = "<font color='red'>" & Format(SubtotalDealAmount, "###,###,###.00") & "</font>"
                    dra1("maturityamount") = "<font color='red'>" & Format(SubtotalMaturityAmount, "###,###,###.00") & "</font>"
                    dra1("netinterest") = "<font color='red'>" & Format(SubtotalNetinterest, "###,###,###.00") & "</font>"
                    dra1("days") = ""
                    dra1("maturitydate") = ""

                    dt.Rows.Add(dra1)

                    Dim drz1 As DataRow = dt.NewRow()
                    'Insert a blank line after the totals
                    drz1("dealref") = ""
                    drz1("dealamnt") = ""
                    drz1("maturityamount") = ""
                    drz1("netinterest") = ""
                    drz1("days") = ""
                    drz1("maturitydate") = ""
                    dt.Rows.Add(drz1)
                    SubtotalDealAmount = 0
                    SubtotalMaturityAmount = 0
                    SubtotalNetinterest = 0
                    x = False
                End If


                Dim dr2 As DataRow = dt.NewRow()
                dr2("dealref") = Trim(drSQL.Item("Dealreference").ToString)
                dr2("dealamnt") = Format(CDec(drSQL.Item("dealamount").ToString), "###,###,###.00")
                dr2("maturityamount") = Format(CDec(drSQL.Item("maturityamount").ToString), "###,###,###.00")
                dr2("netinterest") = Format(CDec(drSQL.Item("netinterest").ToString), "###,###,###.00")
                dr2("days") = drSQL.Item("daystomaturity").ToString
                dr2("maturitydate") = drSQL.Item("maturitydate").ToString
                dt.Rows.Add(dr2)

                'Get totals
                SubtotalDealAmount = SubtotalDealAmount + CDec(drSQL.Item("dealamount").ToString)
                SubtotalMaturityAmount = SubtotalMaturityAmount + CDec(drSQL.Item("maturityamount").ToString)
                SubtotalNetinterest = SubtotalNetinterest + CDec(drSQL.Item("netinterest").ToString)

                grandTotalDealAmount = grandTotalDealAmount + CDec(drSQL.Item("dealamount").ToString)
                grandTotalMaturityAmount = grandTotalMaturityAmount + CDec(drSQL.Item("maturityamount").ToString)
                grandTotalNetinterest = grandTotalNetinterest + CDec(drSQL.Item("netinterest").ToString)


                listItemsX = listItemsX + 1
                listItemsXTotal = listItemsXTotal + 1

                daysToMaturity = Trim(drSQL.Item("daystomaturity").ToString)
            Loop

            Dim dr1 As DataRow = dt.NewRow()
            dr1("dealref") = "<font color='red'>Totals</font>"
            dr1("dealamnt") = "<font color='red'>" & Format(SubtotalDealAmount, "###,###,###.00") & "</font>"
            dr1("maturityamount") = "<font color='red'>" & Format(SubtotalMaturityAmount, "###,###,###.00") & "</font>"
            dr1("netinterest") = "<font color='red'>" & Format(SubtotalNetinterest, "###,###,###.00") & "</font>"
            dr1("days") = ""
            dr1("maturitydate") = ""
            dt.Rows.Add(dr1)
            Dim drc1 As DataRow = dt.NewRow()
            'Insert a blank line after the totals
            drc1("dealref") = ""
            drc1("dealamnt") = ""
            drc1("maturityamount") = ""
            drc1("netinterest") = ""
            drc1("days") = ""
            drc1("maturitydate") = ""
            dt.Rows.Add(drc1)

            Dim dre1 As DataRow = dt.NewRow()

            dre1("dealref") = "<font color='red'>GRAND TOTAL</font>"
            dre1("dealamnt") = "<font color='red'>" & Format(grandTotalDealAmount, "###,###,###.00") & "</font>"
            dre1("maturityamount") = "<font color='red'>" & Format(grandTotalMaturityAmount, "###,###,###.00") & "</font>"
            dre1("netinterest") = "<font color='red'>" & Format(grandTotalNetinterest, "###,###,###.00") & "</font>"
            dre1("days") = ""
            dre1("maturitydate") = ""
            dt.Rows.Add(dre1)
            Me.GridIn.Visible = True
            GridIn.DataSource = dt
            GridIn.DataBind()
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            '  SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message)
            '************************END****************************************
        End Try
    End Function

    Private Function GetOutflows(ByVal nummin As Integer, ByVal nummax As Integer, ByVal prodlist As String)
        Dim daysToMaturity As String = ""
        Dim daysToMaturityNew As String = ""
        Dim x As Boolean = False

        Try

            strSQL = "Select * from alldealsall join dealtypes on alldealsall.dealtype=dealtypes.deal_code " & _
                 " where daystomaturity between '" & nummin & "' and '" & nummax & "' and dealbasictype='D'" & _
                 " and alldealsall.othercharacteristics<>'Discount Sale' and  deal_code in(" & prodlist & ")" & _
                 " and alldealsall.currency='" & Trim(cmbCurrency.Text) & "' and dealcancelled<>'Y'" & _
                  " UNION" & _
                 " Select * from alldealsall join dealtypes on alldealsall.dealtype=dealtypes.deal_code " & _
                 " where maturitydate between '" & CDate(selectiondate.Text) & "' and '" & CDate(Session("SysDate")) & "' and dealbasictype='D'" & _
                 " and alldealsall.othercharacteristics<>'Discount Sale' and deal_code in (" & prodlist & ")" & _
                 " and alldealsall.currency='" & Trim(cmbCurrency.Text) & "' and dealcancelled<>'Y' order by daystomaturity  asc"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Dim listItemsX As Integer = 0
            Dim dt1 As New DataTable()
            dt1.Columns.Add(New DataColumn("dealref", GetType(String)))
            dt1.Columns.Add(New DataColumn("dealamnt", GetType(String)))
            dt1.Columns.Add(New DataColumn("maturityamount", GetType(String)))
            dt1.Columns.Add(New DataColumn("netinterest", GetType(String)))
            dt1.Columns.Add(New DataColumn("days", GetType(String)))
            dt1.Columns.Add(New DataColumn("maturitydate", GetType(String)))
            Do While drSQL.Read
                Dim drx1 As DataRow = dt1.NewRow()

               If Trim(drSQL.Item("daystomaturity").ToString) <> daysToMaturity And daysToMaturity <> "" Then

                    drx1("dealref") = "<font color='red'>Totals</font>"
                    drx1("dealamnt") = "<font color='red'>" & Format(SubtotalDealAmount, "###,###,###.00") & "</font>"
                    drx1("maturityamount") = "<font color='red'>" & Format(SubtotalMaturityAmount, "###,###,###.00") & "</font>"
                    drx1("netinterest") = "<font color='red'>" & Format(SubtotalNetinterest, "###,###,###.00") & "</font>"
                    drx1("days") = ""
                    drx1("maturitydate") = ""
                    dt1.Rows.Add(drx1)

                    Dim drx3 As DataRow = dt1.NewRow()
                    'Insert a blank line after the totals
                    drx3("dealref") = ""
                    drx3("dealamnt") = ""
                    drx3("maturityamount") = ""
                    drx3("netinterest") = ""
                    drx3("days") = ""
                    drx3("maturitydate") = ""
                    dt1.Rows.Add(drx3)

                    SubtotalDealAmount1 = 0
                    SubtotalMaturityAmount1 = 0
                    SubtotalNetinterest1 = 0
                    x = False
                End If

                Dim drx2 As DataRow = dt1.NewRow()
                drx2("dealref") = Trim(drSQL.Item("Dealreference").ToString)
                drx2("dealamnt") = Format(CDec(drSQL.Item("dealamount").ToString), "###,###,###.00")
                drx2("maturityamount") = Format(CDec(drSQL.Item("maturityamount").ToString), "###,###,###.00")
                drx2("netinterest") = Format(CDec(drSQL.Item("netinterest").ToString), "###,###,###.00")
                drx2("days") = drSQL.Item("daystomaturity").ToString
                drx2("maturitydate") = drSQL.Item("maturitydate").ToString
                dt1.Rows.Add(drx2)

                'Get totals
                SubtotalDealAmount1 = SubtotalDealAmount1 + CDec(drSQL.Item("dealamount").ToString)
                SubtotalMaturityAmount1 = SubtotalMaturityAmount1 + (CDec(drSQL.Item("maturityamount").ToString) + CDec(drSQL.Item("taxamount").ToString)) 'less tax
                SubtotalNetinterest1 = SubtotalNetinterest1 + CDec(drSQL.Item("netinterest").ToString)

                grandTotalDealAmount1 = grandTotalDealAmount1 + CDec(drSQL.Item("dealamount").ToString)
                grandTotalMaturityAmount1 = grandTotalMaturityAmount1 + CDec(drSQL.Item("maturityamount").ToString)
                grandTotalNetinterest1 = grandTotalNetinterest1 + CDec(drSQL.Item("netinterest").ToString)

               
                listItemsX = listItemsX + 1
                listItemsXTotal1 = listItemsXTotal1 + 1

                daysToMaturity = drSQL.Item("daystomaturity").ToString
            Loop

            Dim dr1 As DataRow = dt1.NewRow()
            dr1("dealref") = "<font color='red'>Totals</font>"
            dr1("dealamnt") = "<font color='red'>" & Format(SubtotalDealAmount, "###,###,###.00") & "</font>"
            dr1("maturityamount") = "<font color='red'>" & Format(SubtotalMaturityAmount, "###,###,###.00") & "</font>"
            dr1("netinterest") = "<font color='red'>" & Format(SubtotalNetinterest, "###,###,###.00") & "</font>"
            dr1("days") = ""
            dr1("maturitydate") = ""
            dt1.Rows.Add(dr1)
            Dim drc1 As DataRow = dt1.NewRow()
            'Insert a blank line after the totals
            drc1("dealref") = ""
            drc1("dealamnt") = ""
            drc1("maturityamount") = ""
            drc1("netinterest") = ""
            drc1("days") = ""
            drc1("maturitydate") = ""
            dt1.Rows.Add(drc1)

            Dim dre1 As DataRow = dt1.NewRow()

            dre1("dealref") = "<font color='red'>GRAND TOTAL</font>"
            dre1("dealamnt") = "<font color='red'>" & Format(grandTotalDealAmount1, "###,###,###.00") & "</font>"
            dre1("maturityamount") = "<font color='red'>" & Format(grandTotalMaturityAmount1, "###,###,###.00") & "</font>"
            dre1("netinterest") = "<font color='red'>" & Format(grandTotalNetinterest1, "###,###,###.00") & "</font>"
            dre1("days") = ""
            dre1("maturitydate") = ""
            dt1.Rows.Add(dre1)

            Me.GridOut.Visible = True
            GridOut.DataSource = dt1
            GridOut.DataBind()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            ' SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message)
            '************************END****************************************
        End Try
    End Function
    'Entry point to class functions
    Public Sub SaveCashFlow(ByVal dealRef As String, ByVal dealamount As Decimal, ByVal maturityAmount As Decimal _
                                , ByVal netinterest As Decimal, ByVal DDY As Integer, ByVal status As String, matDate As String)
        Dim cnSQL As SqlConnection
        Dim cmSQL As SqlCommand
        Dim drSQL As SqlDataReader
        Dim strSQL As String

        Try


            'insert new records
            strSQL = "insert into cashflowwork values('" & Trim(Session("username")) & "','" & Trim(dealRef) & "','" & dealamount & "','" & maturityAmount & "','" & netinterest & "','" & DDY & "','" & Trim(status) & "','" & CDate(matDate) & "')"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Sub
    Public Sub DeleteRecs()

        Try
            'Delete all previous work
            strSQL = " begin tran x" & _
                    " delete cashflowwork where username = '" & Trim(Session("username")) & "'" & _
                    " delete CASHFLOWSUMMARIES where LoggedUser='" & Trim(Session("username")) & "'" & _
                    " commit tran x"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************        End Try
        End Try
    End Sub

    Private Sub PrepareReportSummaries()

        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        Try
            strSQLx = "select distinct(maturitydate), daystomaturity from CASHFLOWWORK where username='" & Session("username") & "'order by daystomaturity asc "
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                Dim liabilities As Decimal = 0
                Dim assets As Decimal = 0
                'Get totals Assets  
                assets = TotalForDate(CDate(drSQLx.Item(0)), "asset", ">0")
                'Get totals Liabilities
                liabilities = TotalForDate(CDate(drSQLx.Item(0)), "liability", "<0")

                'Save the record
                SaveAllSammarries(Int(drSQLx.Item(1)), drSQLx.Item(0), assets, liabilities)
            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ex As Exception
      
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Sub


    Private Sub SaveAllSammarries(daystomaturity As Integer, maturitydate As String, assets As Decimal, liabilities As Decimal)
        Dim cnSQLz As SqlConnection
        Dim cmSQLz As SqlCommand
        Dim drSQLz As SqlDataReader
        Dim strSQLz As String

        Try
            strSQLz = "insert into cashflowSummaries values('" & daystomaturity & "','" & maturitydate & "', " & assets & "," & liabilities & ",'" & Session("username") & "')"
            cnSQLz = New SqlConnection(Session("ConnectionString"))
            cnSQLz.Open()
            cmSQLz = New SqlCommand(strSQLz, cnSQLz)
            drSQLz = cmSQLz.ExecuteReader()

            ' Close and Clean up objects
            drSQLz.Close()
            cnSQLz.Close()
            cmSQLz.Dispose()
            cnSQLz.Dispose()

        Catch ex As Exception
       'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try
    End Sub
    Private Function TotalForDate(ttDate As String, dealtype As String, valtype As String) As Decimal
        Dim cnSQLy As SqlConnection
        Dim cmSQLy As SqlCommand
        Dim drSQLy As SqlDataReader
        Dim strSQLy As String

        Dim x As Decimal = 0

        Try
            strSQLy = "select sum(maturityamount) from CASHFLOWWORK where maturitydate='" & CDate(ttDate) & "' and maturityamount " & valtype & " and username='" & Session("username") & "'"
            cnSQLy = New SqlConnection(Session("ConnectionString"))
            cnSQLy.Open()
            cmSQLy = New SqlCommand(strSQLy, cnSQLy)
            drSQLy = cmSQLy.ExecuteReader()

            Do While drSQLy.Read
                If IsDBNull(drSQLy.Item(0)) = False Then
                    x = CDec(drSQLy.Item(0).ToString)
                Else
                    x = 0
                End If

            Loop

            ' Close and Clean up objects
            drSQLy.Close()
            cnSQLy.Close()
            cmSQLy.Dispose()
            cnSQLy.Dispose()

        Catch ex As Exception
         'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************
        End Try

        Return x

    End Function
End Class
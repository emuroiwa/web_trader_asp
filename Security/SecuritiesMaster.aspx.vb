Imports System.Data.SqlClient
Imports System.Drawing

Public Class SecuritiesMaster
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
    Private ref As String
    Private lowerAmt As Decimal
    Private upperAmt As Decimal
    Private LowerRem As Decimal
    Private upperRem As Decimal
    Private PurDealRef As String
    Private limitsch As New usrlmt.usrlmt

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Call loadPortfolios()
            Call loadCurrencies()
            Call selectBaseOnList(getBaseCurrency)
        End If
        Call loadRevaluations(Trim(cmbTBBAS.SelectedValue.ToString)) 'Get the revaluations done
    End Sub

    Protected Sub cmbcurrency_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbcurrency.SelectedIndexChanged
        Call LoadSecurityPurchase(Trim(cmbcurrency.Text)) 'Get all security purchases
    End Sub
    'Get all security purchases
    Private Sub LoadSecurityPurchase(ByVal curr As String)
        Try
            'strSQL = "select  tb_id ,matured,dealreference from securityPurchase"
            strSQL = "select securitypurchase.tb_id,matured,securitypurchase.dealreference from deals_live join" & _
                    " securityPurchase on securitypurchase.tb_id=deals_live.TB_ID and deals_live.dealreference=" & _
                    " securitypurchase.dealreference join dealtypes on dealtypes.deal_code=deals_live.dealtype " & _
                    " where dealtypes.othercharacteristics='Trading' and" & _
                    "  dealtypes.currency='" & Trim(cmbcurrency.Text) & "'"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            'lstPuchasedTBS.Items.Clear() 'Clear list items

            Do While drSQL.Read
                'Dim itml As New ListViewItem(Trim(drSQL.Item(0).ToString))
                'itml.SubItems.Add(Trim(drSQL.Item(1).ToString))
                'itml.SubItems.Add(Trim(drSQL.Item(2).ToString))
                'cmbTBBAS.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(2).ToString), Trim(drSQL.Item(1).ToString)))
                cmbTBBAS.Items.Add(Trim(drSQL.Item(0).ToString))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            strSQL = "select securitypurchase.tb_id,matured,securitypurchase.dealreference from deals_matured join" & _
                    " securityPurchase on securitypurchase.tb_id=deals_matured.TB_ID and deals_matured.dealreference=" & _
                    " securitypurchase.dealreference join dealtypes on dealtypes.deal_code=deals_matured.dealtype " & _
                    " where dealtypes.othercharacteristics='Trading' and" & _
                    "  dealtypes.currency='" & Trim(cmbcurrency.Text) & "'"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                'Dim itml As New ListViewItem(Trim(drSQL.Item(0).ToString))
                'cmbTBBAS.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + " " + Trim(drSQL.Item(2).ToString), Trim(drSQL.Item(2).ToString)))
                cmbTBBAS.Items.Add(Trim(drSQL.Item(0).ToString))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            ''Sort the list
            'lstPuchasedTBS.Columns(0).ListView.Sorting = SortOrder.Descending
            'lstPuchasedTBS.Sort()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub
    Private Sub loadCurrencies()
        Try
            strSQL = "select currencycode from astval "

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            'cmbCurrency.Items.Clear() 'Clear list items

            Do While drSQL.Read
                cmbcurrency.Items.Add(Trim(drSQL.Item(0).ToString))
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

    Private Function getBaseCurrency() As String
        Dim x As String

        Try
            strSQL = "select currencycode from currencies where isbasecurrency='Y'"

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

            Return x

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Function


    Private Sub loadPortfolios()
        Try
            strSQL = "select  portfolioname,portfolioid from portfoliostructure where securities='yes'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            'lstPuchasedTBS.Items.Clear() 'Clear list items
            Do While drSQL.Read
                'cmbType.Items.Add(Trim(drSQL.Item(0).ToString))
                'ComboBox1.Items.Add(Trim(drSQL.Item(1).ToString))
                'cmbType.Items.Add(New ListItem(Trim(drSQL.Item(0).ToString) + " " + Trim(drSQL.Item(1).ToString), Trim(drSQL.Item(0).ToString)))
                cmbType.Items.Add(Trim(drSQL.Item(0).ToString))
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
    Private Sub selectBaseOnList(ByVal x As String)
        Dim item As Integer = 0
        For item = 0 To cmbcurrency.Items.Count - 1
            cmbcurrency.SelectedIndex = item
            If cmbcurrency.Text = Trim(x) Then
                Exit For
            End If
        Next
    End Sub
    'get all sales made from purchase
    Private Function LoadMiniSales(ByVal Dref As String) As String

        Try


            strSQL = "select  * from deals_live  join customer on deals_live.customernumber " & _
                     " = customer.customer_number join selldetail on deals_live.dealreference=" & _
                     " selldetail.refsell where selldetail.tbid = '" & cmbTBBAS.SelectedValue.ToString & "' and selldetail.refpurchase= '" & Dref & "'"
            'and refpurchase= '" & lstPuchasedTBS.FocusedItem.SubItems(2).Text & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GrdSale.DataSource = drSQL
            GrdSale.DataBind()

            Do While drSQL.Read


                'If Trim(drSQL.Item("purchaseref").ToString) = "" Then
                '    listItems.SubItems.Add("Bulk Sell")
                'Else
                '    listItems.SubItems.Add("Single Sell")
                'End If

                'lstSales.Items.Add(listItems)

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            strSQL = "select  * from deals_matured  join customer on deals_matured.customernumber " & _
                     " = customer.customer_number join selldetail on deals_matured.dealreference=" & _
                     " selldetail.refsell where selldetail.tbid = '" & cmbTBBAS.SelectedValue.ToString & "' and selldetail.refpurchase= '" & Dref & "'"
            'and refpurchase= '" & lstPuchasedTBS.FocusedItem.SubItems(2).Text & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            ' lstSales.Items.Clear() 'Clear list items
            Do While drSQL.Read

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

    End Function

    Protected Sub cmbTBBAS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTBBAS.SelectedIndexChanged
        lblcost.Text = ""
        lblGain.Text = ""
        lblvalue.Text = ""
        lblinter.Visible = False
        Cvalue.Visible = False
        Pvalue.Visible = False
        Gain.Visible = False
        ImaLossIcon.Visible = False
        imgGainIcon.Visible = False
        Try
            Dim table As String

            'txtPurNet.Text = 0 'ensure this field is equal to zero
            Call getTBS()
            If Trim(txtmatured.Text) = "Y" Then

                table = "Deals_Matured"
            Else
                table = "Deals_Live"
            End If

            'Label9.Text = 0
            Call ActualGetPurchase(Trim(cmbTBBAS.SelectedValue.ToString), table, txtTbID.Text) 'Get the original purchaased deal
            Call LoadMiniSales(txtTbID.Text) 'Get the sales made from the deal
            ref = txtTbID.Text
            Call loadRevaluations(Trim(cmbTBBAS.SelectedValue.ToString)) 'Get the revaluations done

        Catch ex As NullReferenceException

        End Try
    End Sub
    Private Sub getTBS()
        Try
            'strSQL = "select  tb_id ,matured,dealreference from securityPurchase"
            strSQL = "select securitypurchase.tb_id,matured,securitypurchase.dealreference from deals_live join" & _
                    " securityPurchase on securitypurchase.tb_id=deals_live.TB_ID and deals_live.dealreference=" & _
                    " securitypurchase.dealreference join dealtypes on dealtypes.deal_code=deals_live.dealtype " & _
                    " where dealtypes.othercharacteristics='Trading' and" & _
                    "  dealtypes.currency='" & Trim(cmbcurrency.Text) & "' and securitypurchase.TB_ID= '" & Trim(cmbTBBAS.SelectedValue.ToString) & "'"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            'lstPuchasedTBS.Items.Clear() 'Clear list items

            Do While drSQL.Read
                txtTbID.Text = Trim(drSQL.Item(2).ToString)
                txtmatured.Text = Trim(drSQL.Item(1).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            strSQL = "select securitypurchase.tb_id,matured,securitypurchase.dealreference from deals_matured join" & _
                    " securityPurchase on securitypurchase.tb_id=deals_matured.TB_ID and deals_matured.dealreference=" & _
                    " securitypurchase.dealreference join dealtypes on dealtypes.deal_code=deals_matured.dealtype " & _
                    " where dealtypes.othercharacteristics='Trading' and" & _
                    "  dealtypes.currency='" & Trim(cmbcurrency.Text) & "'and securitypurchase.TB_ID= '" & Trim(cmbTBBAS.SelectedValue.ToString) & "'"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                txtTbID.Text = Trim(drSQL.Item(2).ToString)
                txtmatured.Text = Trim(drSQL.Item(1).ToString)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            ''Sort the list
            'lstPuchasedTBS.Columns(0).ListView.Sorting = SortOrder.Descending
            'lstPuchasedTBS.Sort()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub
    Private Function loadRevaluations(ByVal TBID As String) As String

        'txtPurNet.Text = 0
        Try
            strSQL = "select  * from  selldetail where tbid = '" & TBID & "' and refpurchase = '" & ref & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()




            Do While drSQL.Read
                'Dim listItems As New ListViewItem(Trim(drSQL.Item("refsell").ToString))
                lblcost.Text = Trim(drSQL.Item("cost").ToString)
                lblvalue.Text = Trim(drSQL.Item("presentvalue").ToString)
                lblGain.Text = Trim(drSQL.Item("profit").ToString)
                lblinter.Visible = True
                Cvalue.Visible = True
                Pvalue.Visible = True
                Gain.Visible = True
                If CDec(Trim(lblGain.Text) < 0) Then
                    lblGain.ForeColor = Color.Red
                    ImaLossIcon.Visible = True

                Else
                    lblGain.ForeColor = Color.Green
                    imgGainIcon.Visible = True
                End If

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myfunction", "bar();", True)
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
    End Function


    Private Function ActualGetPurchase(ByVal TBID As String, ByVal Table As String, ByVal dealref As String) As String

        Try
            strSQL = "select securityPurchase.dealamount as PurchaseAmt,* from  " & Table & "  join customer on " & Table & ".customernumber " & _
                     " = customer.customer_number join securityPurchase on " & Table & ".dealreference=" & _
                     " securityPurchase.dealreference where " & Table & ".tb_id = '" & TBID & "' and  " & Table & ".dealreference = '" & dealref & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GrdMaster.DataSource = drSQL
            GrdMaster.DataBind()

            'listSecurityPurchased.Items.Clear() 'Clear list items
            Do While drSQL.Read


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
    End Function

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim x As String
        Try
            If txtAmtLower.Text = "" Then
                MsgBox("Please enter a lower bound value for the amount.", MsgBoxStyle.Information)
                Exit Sub
            End If
            If txtAmtUpper.Text = "" Then
                MsgBox("Please enter a upper bound value for the amount.", MsgBoxStyle.Information)
                Exit Sub
            End If
            If txtRemUpper.Text = "" Then
                MsgBox("Please enter a lower bound value for the days remaining.", MsgBoxStyle.Information)
                Exit Sub
            End If
            If txtRemLower.Text = "" Then
                MsgBox("Please enter a upper bound value for the days remaining.", MsgBoxStyle.Information)
                Exit Sub
            End If

            lowerAmt = CDec(txtAmtLower.Text)
            upperAmt = CDec(txtAmtUpper.Text)
            LowerRem = Int(txtRemLower.Text)
            upperRem = Int(txtRemUpper.Text)


            strSQL = "select * from securityPurchase join deals_live " & _
                     " on securityPurchase.tb_id=deals_live.tb_id join portfoliostructure on portfoliostructure.portfolioid=deals_live.portfolioid" & _
                     " where deals_live.dealamount between '" & lowerAmt & "' and '" & upperAmt & "' and daystomaturity between '" & LowerRem & "' and '" & upperRem & "'" & _
                     " and deals_live.portfolioid='" & Trim(txtPortfolio.Text) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()


            Do While drSQL.Read
                cmbTBBAS.Items.Add(Trim(drSQL.Item(1).ToString))

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
        End Try
    End Sub
    Private Sub getPortifolio()
        Try
            strSQL = "select  portfolioname,portfolioid from portfoliostructure where securities='yes' and portfolioname='" & Trim(cmbType.SelectedValue.ToString) & "'"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            'lstPuchasedTBS.Items.Clear() 'Clear list items

            Do While drSQL.Read

                txtPortfolio.Text = Trim(drSQL.Item(1).ToString)
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

    Protected Sub cmbType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType.SelectedIndexChanged
        Call getPortifolio()
    End Sub
    Protected Sub GrdSale_ItemCommand(sender As Object, e As Global.EO.Web.GridCommandEventArgs)
        'Check whether it is from our client side
        'JavaScript call

        If e.CommandName = "select" Then
            Dim s As String = String.Empty
            Session("saleref") = e.Item.Cells(1).Value.ToString()
            Session("saleStartDate") = e.Item.Cells(4).Value.ToString()
            Session("saleMatDate") = e.Item.Cells(5).Value.ToString()
            Session("saleDealAmt") = e.Item.Cells(3).Value.ToString()
            Session("saleMatAmt") = e.Item.Cells(11).Value.ToString()
            Session("saleYield") = e.Item.Cells(6).Value.ToString()
            Session("saleDiscount") = e.Item.Cells(7).Value.ToString()
            Session("saleTenor") = e.Item.Cells(8).Value.ToString()
            Session("saleAcrued") = e.Item.Cells(9).Value.ToString()
            Session("Purref") = Trim(txtTbID.Text)
            Session("salecurrency") = Trim(cmbcurrency.SelectedValue.ToString)
            Session("saletb") = Trim(cmbTBBAS.SelectedValue.ToString)


        End If

    End Sub
   
   

  
End Class
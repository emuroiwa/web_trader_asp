Imports System.Data.SqlClient

Public Class newdeal
    Inherits System.Web.UI.Page

    Public MyPortfolioCollectionID As New Integer
    Public MyPortfolioCollection As New Collection 'users portfolio collection
    Public DealCode As String
    'ernest
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private object_userlog As New usrlog.usrlog
    Private object_mmdeal As New mmDeal.DeaNumbers
    Private object_calcfxn As New mmDeal.CalculationFunctions
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class

    '' Public LoggedUser As String = "kelvin"

    Public Sub myPortfolios()
        Try
            'validate username first
            strSQL = "Select portfoliostructure.portfolioid,portfolioname from userportfolios join portfoliostructure " & _
            "  on userportfolios.portfolioid=portfoliostructure.portfolioid where userID = '" & CType(Session.Item("username"), String) & "' order by PortfolioName asc"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                MyPortfolioCollectionID = drSQL.Item(0).ToString
                lstPortfolio.Items.Add(drSQL.Item(1).ToString)
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

            'MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

        'If the portfolios collection is empty then disable the new deal menu
        If MyPortfolioCollectionID = 0 Then
            ''  mdiCli.ToolBar1.Buttons.Item(1).Enabled = True
            object_userlog.Msg("You are not assigned any portfolios. Dealing will be disabled." & vbNewLine & _
                    "Contact your system administrator for assistance.", True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "You are not assigned any portfolios. Dealing will be disabled." & vbNewLine & _
                    "Contact your system administrator for assistance.", "error")

            ' MsgBox("You are not assigned any portfolios. Dealing will be disabled." & vbNewLine & _
            '"Contact your system administrator for assistance.", MsgBoxStyle.Information, "Portfolios")
        End If

    End Sub
    Private Function GetDealCode(ByVal name As String)
        Dim aa As String
        Try
            strSQL = "SELECT Deal_Code FROM [dbo].[DEALTYPES] WHERE DealTypeDescription='" + name + "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                aa = drSQL.Item(0).ToString
            Loop
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

            ' MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Return aa
    End Function
    Private Function GetID(ByVal name As String)
        Dim aa As Integer
        Try
            strSQL = "Select portfoliostructure.portfolioid from userportfolios join portfoliostructure " & _
        "  on userportfolios.portfolioid=portfoliostructure.portfolioid where  portfolioname='" + name + "' and  userID = '" & CType(Session.Item("username"), String) & "' order by PortfolioName asc"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                aa = drSQL.Item(0).ToString
                ''lstPortfolio.Items.Add(drSQL.Item(1).ToString)
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

            ' MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Return aa
    End Function
    Private Sub getCurrencies()
        Try
            'strSQL = "Select currencycode from currencies where isbasecurrency='Yes'"
            strSQL = "Select currencycode from astval"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                cmbCurrency.Items.Add(Trim(drSQL.Item(0).ToString))
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            '   MsgBox(ex.Message, MsgBoxStyle.Critical)

                        object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")


        End Try
    End Sub
    Private Sub GetProduct(ByVal currentportfolio As String)
        '' Dim x As Integer
        Dim ID As String
        '' Dim itmx As ListViewItem

        Try

            ID = GetID(lstPortfolio.SelectedItem.Text.ToString())

            If RadioButtonList1.SelectedValue = "rdPlacement" Then
                strSQL = "Select dealtype,dealtypeDescription from portfolioinformation join dealtypes  on dealtypes.deal_code=portfolioinformation.dealtype" & _
          "   where portfolioinformation.portfolioid = '" & ID & "' and dealtypes.dealbasictype = 'L' and  dealtypes.discount <> 'Y' and currency='" & Trim(cmbCurrency.Text) & "'"

            End If

            If RadioButtonList1.SelectedValue = "rdDeposit" Then

                strSQL = "Select dealtype,dealtypedescription from portfolioinformation join dealtypes  on dealtypes.deal_code=portfolioinformation.dealtype" & _
                "   where portfolioinformation.portfolioid = '" & ID & "' and dealtypes.dealbasictype = 'D' and  dealtypes.discount <> 'Y' and currency='" & Trim(cmbCurrency.Text) & "'"
            End If

            If RadioButtonList1.SelectedValue = "rdSecurity" Then

                strSQL = "Select dealtype,dealtypedescription from portfolioinformation join dealtypes  on dealtypes.deal_code=portfolioinformation.dealtype" & _
                "   where portfolioinformation.portfolioid = '" & ID & "' and dealtypes.dealbasictype = 'L' and  dealtypes.discount = 'Y' and currency='" & Trim(cmbCurrency.Text) & "'"
            End If


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            lstProducts.Items.Clear()
            ''lstCodes.Items.Clear()
            ''linkProductCode.Text = ""
            If drSQL.HasRows = True Then
                Do While drSQL.Read

                    lstProducts.Items.Add(drSQL.Item(1).ToString)
                Loop
            Else
                lstProducts.Items.Add("No Money Market Products Avaliable")

            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
                     object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")


        Catch er As Exception

        End Try
    End Sub
    Public Sub lstPortfolio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstPortfolio.SelectedIndexChanged, lstProducts.SelectedIndexChanged
        '' lstProducts.Items.Add(lstPortfolio.SelectedItem.Text.ToString())
        '' MsgBox(GetID(lstPortfolio.SelectedItem.Text.ToString()))
        ''MsgBox(lstPortfolio.SelectedItem.Text.ToString())
        lblPortifolioID.Text = GetID(lstPortfolio.SelectedItem.Text.ToString())
        lblPortifolio.Text = lstPortfolio.SelectedItem.Text.ToString()


        Call GetProduct(lstPortfolio.SelectedItem.Text.ToString())
    End Sub
    Public Sub lstProducts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstProducts.SelectedIndexChanged
        DealCode = GetDealCode(lstProducts.SelectedItem.Text.ToString())
        lblDealCode.Text = Trim(DealCode)
        lblCurrency.Text = cmbCurrency.SelectedValue.ToString()
        lblProduct.Text = lstProducts.SelectedItem.Text.ToString()
        PopulateFields()
        Call PopulateFields()
    End Sub
    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        lstPortfolio.Items.Clear()
        myPortfolios()
    End Sub
    Private Sub PopulateFields()
        Try
            strSQL = ("select * from dealtypes where deal_code = '" & DealCode & "'")

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader
            Dim htmlTable As New StringBuilder()
            htmlTable.Append("<center><table border='0' cellpadding=4 cellspacing=0 width='50%'>")

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    htmlTable.Append("<tr>")
                    htmlTable.Append("<td> Deal Code </td> <td>" & drSQL.Item("Deal_Code").ToString() & "</td>")
                    htmlTable.Append("<td> Currency </td> <td> " & drSQL.Item("currency").ToString() & "</td>")
                    htmlTable.Append("</tr>")
                    htmlTable.Append("<tr>")
                    htmlTable.Append("<td> Application </td> <td> " & drSQL.Item("Application").ToString() & "</td>")
                    htmlTable.Append("<td> Deal Type </td> <td>" & drSQL.Item("DealBasicType").ToString() & "</td>")
                    htmlTable.Append("</tr>")
                    htmlTable.Append("<tr>")
                    htmlTable.Append("<td> Application </td> <td>" & drSQL.Item("Application").ToString() & "</td>")
                    htmlTable.Append("<td> Deal Type </td> <td>" & drSQL.Item("DealBasicType").ToString() & "</td>")
                    htmlTable.Append("</tr>")
                    htmlTable.Append("<tr>")
                    htmlTable.Append("<td> Deal Type Description </td> <td>" & drSQL.Item("DealTypeDescription").ToString() & "</td>")
                    htmlTable.Append("<td> Discount </td> <td>" & drSQL.Item("Discount").ToString() & "</td>")
                    htmlTable.Append("</tr>")
                    htmlTable.Append("<tr>")
                    htmlTable.Append("<td> Taxable </td> <td>" & drSQL.Item("Taxable").ToString() & "</td>")
                    htmlTable.Append("<td> Summary Of Deal </td> <td>" & drSQL.Item("SummaryOfDeal").ToString() & "</td>")
                    htmlTable.Append("</tr>")
                    htmlTable.Append("<tr>")
                    htmlTable.Append("<td> IntComponent </td> <td> " & drSQL.Item("IntComponent").ToString() & "</td>")
                    htmlTable.Append("<td> Security Type </td> <td>" & drSQL.Item("securitytype").ToString() & "</td>")
                    htmlTable.Append("</tr>")
                    htmlTable.Append("<tr>")
                    htmlTable.Append("<td> FCC Portfolio </td> <td>" & drSQL.Item("FCCPortfolio").ToString() & "</td>")
                    htmlTable.Append("<td> Deal Code Mapped </td> <td>" & drSQL.Item("deal_code_mapped").ToString() & "</td>")
                    htmlTable.Append("</tr>")
                    htmlTable.Append("<tr>")
                    htmlTable.Append("<td> Other Characteristics </td> <td>" & drSQL.Item("OtherCharacteristics").ToString() & "</td>")
                    htmlTable.Append("</tr>")
                    ''txtLoanDepositM.Text = drSQL.Item("deal_code_mapped").ToString

                Loop
                htmlTable.Append("</table></center>")
                lblDealInfo.Text = "<div class='alert alert-success alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>  <h4>	<i class='icon fa fa-check'></i>" + DealCode + "!</h4><strong>" & htmlTable.ToString() & "  </strong>  </div>"
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'Get the limit set for the deal code
            strSQL = ("select * from  productlimits  where dealcode = '" & DealCode & "'")
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            If drSQL.HasRows = True Then
                Do While drSQL.Read
                    If IsDBNull(drSQL.Item("limit")) = False Then
                        ''6txtLimit.Text = Format(CDec(drSQL.Item("Limit").ToString), "###,###,###.00")
                    Else
                        ''txtLimit.Text = ""
                    End If
                Loop
            Else
                '' txtLimit.Text = ""
            End If

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            '   MsgBox(ec.Message, MsgBoxStyle.Information)
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''MsgBox(CType(Session.Item("username"), String)) 'checking postback
        If Not IsPostBack Then
            Call getCurrencies()
            If chkDash.Checked = True Then
                lblDash.Text = "1"
                '' MsgBox("fkgvdh")

            Else
                lblDash.Text = "0"
            End If

        End If

    End Sub
    Public Sub btnnew_Click(sender As Object, e As EventArgs) Handles btnnew.Click
        If chkDash.Checked = True Then
            lblDash.Text = True
        Else
            lblDash.Text = False
        End If
        If chkMatured.Checked = False Then

            If RadioButtonList1.SelectedValue = "rdPlacement" Then
                Dim page As String = "Loan"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "mmRedirect('" & page & "');", True)
            End If
            If RadioButtonList1.SelectedValue = "rdDeposit" Then
                Dim page As String = "Deposit"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "mmRedirect('" & page & "');", True)
            End If
            If RadioButtonList1.SelectedValue = "rdSecurity" Then
                Dim page As String = "SecurityPurchase"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "mmRedirect('" & page & "');", True)
            End If
        Else

            If RadioButtonList1.SelectedValue = "rdPlacement" Then
                Dim page As String = "MaturedLoan"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "mmRedirect('" & page & "');", True)
            End If
            If RadioButtonList1.SelectedValue = "rdDeposit" Then
                Dim page As String = "MaturedDeposit"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "mmRedirect('" & page & "');", True)
            End If
            If RadioButtonList1.SelectedValue = "rdSecurity" Then
                Dim page As String = "MaturedSecurity"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "mmRedirect('" & page & "');", True)
            End If
        End If

    End Sub
    Protected Sub btncancel_Click(sender As Object, e As EventArgs) Handles btncancel.Click
        Response.Redirect("newdeal.aspx")
    End Sub
End Class
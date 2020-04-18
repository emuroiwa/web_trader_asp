Imports System.Data.SqlClient

Public Class NewSell
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private object_userlog As New usrlog.usrlog

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call getCurrencies()
            Call LoadPortfolio()
        End If

    End Sub

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
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Sub
    Public Sub LoadPortfolio()
        Try
            'validate username first
            strSQL = "Select distinct portfoliostructure.portfolioid,portfolioname from userportfolios join portfoliostructure " & _
            "  on userportfolios.portfolioid=portfoliostructure.portfolioid where userID = '" & CType(Session.Item("username"), String) & "' order by PortfolioName asc"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                'cmbPort.Items.Add(New ListItem(Trim(drSQL.Item(1).ToString) + "  " + Trim(drSQL.Item(0).ToString), Trim(drSQL.Item(0).ToString)))
                cmbPort.Items.Add(drSQL.Item(1).ToString)
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

       


    End Sub
    Private Function GetID(ByVal Port As String)
        Dim id As Integer
        Try
            strSQL = "Select portfoliostructure.portfolioid from userportfolios join portfoliostructure " & _
        "  on userportfolios.portfolioid=portfoliostructure.portfolioid where  portfolioname='" + Port + "'  order by PortfolioName asc"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                lblportid.Text = drSQL.Item("portfolioid").ToString
                ''lstPortfolio.Items.Add(drSQL.Item(1).ToString)
                'lblportid = GetID(Trim(cmbPort.SelectedValue.ToString))
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Return id
    End Function

    Protected Sub cmbPort_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPort.SelectedIndexChanged
        Call GetID(Trim(cmbPort.SelectedValue.ToString))

    End Sub

    Protected Sub rdSellType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rdSellType.SelectedIndexChanged
        If rdSellType.SelectedValue = "rdSingle" Then
            cmbPort.Visible = False
            lblport.Visible = False
        Else
            cmbPort.Visible = True
            lblport.Visible = True
        End If
    End Sub

    Protected Sub btnnew_Click(sender As Object, e As EventArgs) Handles btnnew.Click
        lblCurrency.Text = cmbCurrency.SelectedValue.ToString()
        lblMatAmount.Text = Trim(txtamont.Text)
        lbldays.Text = Trim(txtdays.Text)
        lblport.Text = Trim(cmbPort.SelectedValue.ToString)
        Session("Portname") = Trim(lblport.Text)
        If rdOption.SelectedValue = "rdDiscount" Then
            lblselloption.Text = "Discount"
        Else
            lblselloption.Text = "Yield"
        End If

        If rdSellType.SelectedValue = "rdSingle" Then
            Dim page As String = "SingleSell"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "mmRedirect('" & page & "');", True)
        End If
        If rdSellType.SelectedValue = "rdMultiple" Then
            Dim page As String = "Multisell"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myFunction", "mmRedirect('" & page & "');", True)
        End If

    End Sub
End Class
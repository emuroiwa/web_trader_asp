Imports System.Data.SqlClient

Public Class SwapRequired
    Inherits System.Web.UI.Page
    Public SwappingSecurity As Boolean
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
 

    Private object_userlog As New usrlog.usrlog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If SwappingSecurity = True Then
            Call LoadExpiredSecurityDeals()
        Else
            Call LoadUnsecuredDeals()
        End If
    End Sub
    Private Sub LoadUnsecuredDeals()
        Try
            'loans
            strSQL = "select dealreference from deals_live join dealtypes on deals_live.dealtype= dealtypes.deal_code where" & _
                     " dealreference not in(select distinct(dealreference) from counterpartySecurity join COLLATERAL_ITEMS on" & _
                     " counterpartySecurity.tb_id=COLLATERAL_ITEMS.collateralreference join COLL_COLLATERAL_TYPES on " & _
                     " COLLATERAL_ITEMS.collateraltype=COLL_COLLATERAL_TYPES.collateralID ) and dealbasictype='L'"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()



            Do While drSQL.Read
                cmbLoanDeal.Items.Add(Trim(drSQL.Item("dealreference").ToString))

            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'Deposits


            strSQL = "select dealreference from deals_live join dealtypes on deals_live.dealtype= dealtypes.deal_code where" & _
                       " dealreference not in(select distinct(dealreferencedeal) from attachedsecurities join COLLATERAL_ITEMS on" & _
                       " attachedsecurities.tb_id=COLLATERAL_ITEMS.collateralreference join COLL_COLLATERAL_TYPES on " & _
                       " COLLATERAL_ITEMS.collateraltype=COLL_COLLATERAL_TYPES.collateralID ) and dealbasictype='D'"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()



            Do While drSQL.Read
                cmbDepositDeal.Items.Add(Trim(drSQL.Item("dealreference").ToString))

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


    Private Sub LoadExpiredSecurityDeals()
        Try
            'loans
            strSQL = "select distinct(dealreference) from counterpartySecurity join COLLATERAL_ITEMS on counterpartySecurity.tb_id=COLLATERAL_ITEMS.collateralreference" & _
                " join COLL_COLLATERAL_TYPES on COLLATERAL_ITEMS.collateraltype=COLL_COLLATERAL_TYPES.collateralID " & _
                "where status in ('E')"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

          

            Do While drSQL.Read
              cmbLoanDeal.Items.Add(Trim(drSQL.Item("dealreference").ToString))

            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'Deposits


            strSQL = "select distinct(dealreferenceDeal) from attachedsecurities join COLLATERAL_ITEMS on attachedsecurities.tb_id=COLLATERAL_ITEMS.collateralreference" & _
                         " join COLL_COLLATERAL_TYPES on COLLATERAL_ITEMS.collateraltype=COLL_COLLATERAL_TYPES.collateralID " & _
                         "where  matured in ('E')"


            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()



            Do While drSQL.Read
                cmbDepositDeal.Items.Add(Trim(drSQL.Item("dealreference").ToString))

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

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("../index.aspx")
    End Sub

    Protected Sub cmbLoanDeal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLoanDeal.SelectedIndexChanged
        Response.Redirect("SecSwap.aspx?dealref=" & Trim(cmbLoanDeal.SelectedValue.ToString))
    End Sub

    Protected Sub cmbDepositDeal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDepositDeal.SelectedIndexChanged
        Response.Redirect("SecSwap.aspx?dealref=" & Trim(cmbDepositDeal.SelectedValue.ToString))
    End Sub
End Class
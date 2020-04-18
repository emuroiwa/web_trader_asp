Imports System.Data.SqlClient

Public Class MaturitiesMaintanance
    Inherits System.Web.UI.Page
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class
    Private globalvars_mmdeal As New GlobalVars.mmDeal   'instance of the userlogins class
    Private object_savedash As New mmDeal.SaveDashBoardDeal   'instance of the userlogins class
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    'ernest
    'Private dbinfo As New dbparm.dbinfo
    Private object_userlog As New usrlog.usrlog
    Private object_mmdeal As New mmDeal.DeaNumbers
    Private object_calcfxn As New mmDeal.CalculationFunctions
    Private object_dealinstr As New mmDeal.DealInstructions
    Private mmdeal_object As New mmDeal.DealMaturityCheck
   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CalendarMaturities.VisibleDate = CDate(Session("Sysdate"))
        Call loadDeals(Session("Sysdate"))
        
    End Sub

    Private Sub loadDeals(ByVal aa As String)
        'lstDealsDetails.VirtualMode = False
        'Dim img As Integer
        Try
            strSQL = "select * from deals_live  join customer on " & _
                     " deals_live.CustomerNumber=customer.Customer_Number where authorisationstatus = 'V' "


            If CDate(aa) > CDate(Session("SysDate")) Then
                strSQL = "select * from deals_live join customer on deals_live.customernumber=customer.customer_number" & _
                                 "  where maturitydate='" & CDate(aa) & "' "
                'img = 1
            Else
                strSQL = "select * from deals_matured join customer on deals_matured.customernumber=customer.customer_number" & _
                     "  where maturitydate='" & CDate(aa) & "' "
                ' img = 0
            End If
          
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GrdDealsMaturity.DataSource = drSQL
            GrdDealsMaturity.DataBind()


            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()
        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")
            '************************END****************************************

            'MsgBox(ex.Message, MsgBoxStyle.Critical)

        End Try
    End Sub

    Protected Sub CalendarMaturities_SelectionChanged(sender As Object, e As EventArgs) Handles CalendarMaturities.SelectionChanged
        Call loadDeals(CalendarMaturities.SelectedDate)
    End Sub
End Class
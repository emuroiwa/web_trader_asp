Imports System.Data.SqlClient
Imports sys_ui
Imports mmDeal


Public Class index
    Inherits System.Web.UI.Page
    Dim mmdeal_object As New mmDeal.DealMaturityCheck

    Private object_userlog As New usrlog.usrlog

    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    Private clientlogin_object As New clientlogin.UserLogins  'instance of the userlogins class
    Private alerts As New csvptt.csvptt
    'Private serverName As String
    'Private dataBaseName As String

    Protected Sub btnMe_Click(sender As Object, e As EventArgs) Handles btnMe.Click
        lblDealUsers.Text = "--> " + Session("UserFullName")
        Call GetNotifications(1)
        Call GetDetails(1)
        Call GetDashBoard(1)
        ''hiding login label
        lblLogin.Visible = True
        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "HideLabel();", True)
    End Sub

    Protected Sub btnAll_Click(sender As Object, e As EventArgs) Handles btnAll.Click
        lblDealUsers.Text = "--> All Deals"
        Call GetNotifications(2)
        Call GetDetails(2)
        Call GetDashBoard(2)
        ''hiding login label
        lblLogin.Visible = True
        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "HideLabel();", True)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
       
        If Not IsPostBack Then
            lblDealUsers.Text = "--> " + Session("UserFullName")
            Call GetNotifications(1)
            Call GetDetails(1)
            Call GetDashBoard(1)
            Call CheckDealSecurityExpiry()
        End If
        ''hiding login label
        lblLogin.Visible = True
        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "HideLabel();", True)
    End Sub
    Private Sub frmSysStatus_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim cnSQLy2 As SqlConnection
        Dim cmSQLy2 As SqlCommand
        Dim drSQLy2 As SqlDataReader
        Dim strSQLy2 As String
        Dim LastLogin As String
        ''Dim NumFailed As String


        Try
            strSQLy2 = "select user_lastlogin,user_failedAttempts from logins where user_id='" & CType(Session.Item("username"), String) & "'"
            cnSQLy2 = New SqlConnection(Session("ConnectionString"))
            cnSQLy2.Open()
            cmSQLy2 = New SqlCommand(strSQLy2, cnSQLy2)
            drSQLy2 = cmSQLy2.ExecuteReader

            Do While drSQLy2.Read
                If IsDBNull(drSQLy2.Item(0)) = True Then
                    LastLogin = "Never"
                Else
                    LastLogin = drSQLy2.Item(0).ToString
                End If

                lblLogin.Text = success("Last Login Date " & LastLogin & "<br>" & "Number Of Failed Attempts " & drSQLy2.Item(1).ToString, "Login History")
            Loop

            ' Close and Clean up objects
            drSQLy2.Close()
            cnSQLy2.Close()
            cmSQLy2.Dispose()
            cnSQLy2.Dispose()

        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

            '  MsgBox(ex.Message, MsgBoxStyle.Critical)
            'object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
        End Try
    End Sub

    Private Sub CheckDealSecurityExpiry()
        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        Try

            'COUNTERPARTYSECURITY
            strSQLx = "select * from COUNTERPARTYSECURITY join COLLATERAL_ITEMS on COUNTERPARTYSECURITY.tb_id=COLLATERAL_ITEMS.collateralReference" & _
                       " where status='E'"


            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()
            GridSecurity.DataSource = drSQLx
            GridSecurity.DataBind()


            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()
        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            ' slog.LogEvent(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, serverName)
            '************************END****************************************

        End Try
        Try
            Dim cnSQLc As SqlConnection
            Dim cmSQLc As SqlCommand
            Dim drSQLc As SqlDataReader
            Dim strSQLc As String

            'ATTACHEDSECURITIES
            strSQLc = "select * from ATTACHEDSECURITIES join COLLATERAL_ITEMS on ATTACHEDSECURITIES.tb_id=COLLATERAL_ITEMS.collateralReference" & _
                       " where Matured='E'"



            cnSQLc = New SqlConnection(Session("ConnectionString"))
            cnSQLc.Open()
            cmSQLc = New SqlCommand(strSQLc, cnSQLc)
            drSQLc = cmSQLc.ExecuteReader()
            GridSecurity.DataSource = drSQLc
            GridSecurity.DataBind()


            ' Close and Clean up objects
            drSQLc.Close()
            cnSQLc.Close()
            cmSQLc.Dispose()
            cnSQLc.Dispose()
        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            ' slog.LogEvent(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, serverName)
            '************************END****************************************

        End Try
        Try
            Dim cnSQLz As SqlConnection
            Dim cmSQLz As SqlCommand
            Dim drSQLz As SqlDataReader
            Dim strSQLz As String
            'Security Purchases
            strSQLz = "select * from ATTACHEDSECURITIES join securitypurchase on ATTACHEDSECURITIES.tb_id=SECURITYPURCHASE.dealreference  where ATTACHEDSECURITIES.Matured='E'"

            cnSQLz = New SqlConnection(Session("ConnectionString"))
            cnSQLz.Open()
            cmSQLz = New SqlCommand(strSQLz, cnSQLz)
            drSQLz = cmSQLz.ExecuteReader()
            GridSecurity.DataSource = drSQLz
            GridSecurity.DataBind()


            ' Close and Clean up objects
            drSQLz.Close()
            cnSQLz.Close()
            cmSQLz.Dispose()
            cnSQLz.Dispose()


        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            ' slog.LogEvent(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, serverName)
            '************************END****************************************

        End Try
    End Sub
   
    Private Sub GetNotifications(ByVal rectype As Integer)
        Try

            If rectype = 1 Then
                strSQL = "select * from dealnotifications join deals_live on dealnotifications.dealreference=deals_live.dealreference" & _
                         " where dealcapturer='" & Session("username") & "' and daystomaturity<=2"
            Else
                strSQL = "select * from dealnotifications join deals_live on dealnotifications.dealreference=deals_live.dealreference" & _
                         " where daystomaturity<=2"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GridNoti.DataSource = drSQL
            GridNoti.DataBind()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "index.aspx", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try

    End Sub
    Private Sub GetDashBoard(ByVal rectype As Integer)
        Try

            If rectype = 1 Then

                strSQL = "select dealreference,dealamount,dateentered,startdate,maturitydate,currency,expired from dealsdashboard where dealcapturer='" & Session("username") & "'"
            Else
                strSQL = "select dealreference,dealamount,dateentered,startdate,maturitydate,currency,expired from dealsdashboard "
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            GridDashboard.DataSource = drSQL
            GridDashboard.DataBind()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As Exception

            object_userlog.Msg(ex.Message, True, "index.aspx", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")


        End Try

    End Sub
    Private Sub GetDetails(ByVal recs As Integer)

        'if rec =1 then its for the logged user

        Try
            'Get Live Deals - MM
            If recs = 1 Then
                strSQL = "select count(dealreference) from deals_live where dealcapturer='" & CType(Session.Item("username"), String) & "'"
            Else
                strSQL = "select count(dealreference) from deals_live "
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblLiveMM.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "index.aspx", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

        End Try

        Try
            'Get matured deals MM
            If recs = 1 Then
                strSQL = "select count(dealreference) from deals_matured where dealcapturer='" & CType(Session.Item("username"), String) & "'"
            Else
                strSQL = "select count(dealreference) from deals_matured "
            End If
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblMaturedMM.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "index.aspx", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")
        End Try

        Try
            'Get cancelled deals MM
            If recs = 1 Then
                strSQL = "select count(dealreference) from deals_cancelled where dealcapturer='" & CType(Session.Item("username"), String) & "'"
            Else
                strSQL = "select count(dealreference) from deals_cancelled"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblCancelledMM.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "index.aspx", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")
        End Try

        Try
            'Get deal notifications - MM
            If recs = 1 Then
                strSQL = "select count(dealnotifications.dealreference) from dealnotifications join deals_live on dealnotifications.dealreference=deals_live.dealreference where dealcapturer='" & CType(Session.Item("username"), String) & "'"
            Else
                strSQL = "select count(dealnotifications.dealreference) from dealnotifications join deals_live on dealnotifications.dealreference=deals_live.dealreference "
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblDealNot.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

            'MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try


        Try
            'Get Spot Deals - FX
            If recs = 1 Then
                strSQL = "select count(dealref) from fxswitches where dealer='" & CType(Session.Item("username"), String) & "' and transactiondescription='Fx Spot Deal' and settlementdate='" & BusinessDays() & "'"
            Else
                strSQL = "select count(dealref) from fxswitches where  transactiondescription='Fx Spot Deal' and settlementdate='" & BusinessDays() & "'"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblSpot.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")
        End Try

        Try
            'Get Swap Deals - FX
            If recs = 1 Then
                strSQL = "select count(dealref) from fxswitches where dealer='" & CType(Session.Item("username"), String) & "' and transactiondescription='Fx Swap Deal' and settlementdate='" & BusinessDays() & "'"
            Else
                strSQL = "select count(dealref) from fxswitches where  transactiondescription='Fx Swap Deal' and settlementdate='" & BusinessDays() & "'"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblSwap1.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")
            'MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try

        Try
            'Get Forward Deals - FX
            If recs = 1 Then
                strSQL = "select count(dealref) from fxswitches where dealer='" & CType(Session.Item("username"), String) & "' and transactiondescription='Fx forward Deal' and settlementdate='" & BusinessDays() & "'"
            Else
                strSQL = "select count(dealref) from fxswitches where transactiondescription='Fx forward Deal' and settlementdate='" & BusinessDays() & "'"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblFEC.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

            'MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try

        Try
            'Get Interest rate Swaps
            If recs = 1 Then
                strSQL = "select count(dealreference) from fxIntSwpNet join fxSwitches on" & _
                        " fxIntSwpNet.dealreference=fxSwitches.dealref where fxSwitches.dealer='" & CType(Session.Item("username"), String) & "' and fxIntSwpNet.ConfirmationPrinted='N'"
            Else
                strSQL = "select count(dealreference) from fxIntSwpNet join fxSwitches on" & _
                        " fxIntSwpNet.dealreference=fxSwitches.dealref where fxIntSwpNet.ConfirmationPrinted='N'"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblSwap.Text = Trim(drSQL.Item(0)) & " Deals"
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

            'MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try

        Try
            'Get Currency Swaps
            If recs = 1 Then
                strSQL = "select count(dealreference) from fxCurSwpNet join fxSwitches on" & _
                        " fxCurSwpNet.dealreference=fxSwitches.dealref where fxSwitches.dealer='" & CType(Session.Item("username"), String) & "' and FxCurSwpNet.ConfirmationPrinted='N'"
            Else
                strSQL = "select count(dealreference) from fxCurSwpNet join fxSwitches on" & _
                        " fxCurSwpNet.dealreference=fxSwitches.dealref where FxCurSwpNet.ConfirmationPrinted='N'"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblCurrencySwaps.Text = Trim(drSQL.Item(0)) & " Deals"
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

            'MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try



        Try
            'Get Live Placements and deposits - FX
            If recs = 1 Then
                strSQL = "select count(dealreference) from fxdeals_live where dealcapturer='" & CType(Session.Item("username"), String) & "'"
            Else
                strSQL = "select count(dealreference) from fxdeals_live "
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblLiveFX.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

            'MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try

        Try
            'Get matured Placements and deposits - FX
            If recs = 1 Then
                strSQL = "select count(dealreference) from fxdeals_matured where dealcapturer='" & CType(Session.Item("username"), String) & "'"
            Else
                strSQL = "select count(dealreference) from fxdeals_matured "
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblMaturedFX.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

            'MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try

        Try
            'Get cancelled Placements and deposits - FX
            If recs = 1 Then
                strSQL = "select count(dealreference) from fxdeals_cancelled where dealcapturer='" & CType(Session.Item("username"), String) & "'"
            Else
                strSQL = "select count(dealreference) from fxdeals_cancelled"
            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                lblCancelledFX.Text = drSQL.Item(0)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ec As Exception
            object_userlog.Msg(ec.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ec.Message, "error")

            'MsgBox(ec.Message, MsgBoxStyle.Information)
        End Try

    End Sub
    Private Function BusinessDays() As Date
        Dim CheckValue As Boolean
        Dim busDate As Date

        busDate = DateAdd(DateInterval.Day, 1, CDate(Session("SysDate")))

        'Check if busDate date is not a holiday or a non-business day 
        'If it is a holiday or a non-business day search for the next good business day and set
        'it as the next business date

        CheckValue = False

        Do Until CheckValue = True
            If (mmdeal_object.NonBusinessDay(busDate) = False) Then
                CheckValue = True
            Else
                busDate = DateAdd(DateInterval.Day, 1, busDate)
            End If
        Loop

        Return busDate

    End Function

  
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'object_userlog.Msg("test", True, "moneymarket/newdeal.aspx", "uysdhsdh")

    End Sub
End Class
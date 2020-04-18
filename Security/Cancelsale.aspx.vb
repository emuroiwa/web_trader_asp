Imports System.Data.SqlClient

Public Class Cancelsale
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private limitsch As New usrlmt.usrlmt
    Private object_userlog As New usrlog.usrlog

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then


            Call getDetails()
        End If
    End Sub
    Private Sub getDetails()
        lblsaleref.Text = Session("saleref")
        lblStart.Text = Session("saleStartDate")
        lblMaturityDate.Text = Session("saleMatDate")
        lblAmount.Text = Session("saleDealAmt")
        lblMaturityAmount.Text = Session("saleMatAmt")
        lblYieldRate.Text = Session("saleYield")
        lbldiscount.Text = Session("saleDiscount")
        lblTenor.Text = Session("saleTenor")
        lblAccrued.Text = Session("saleAcrued")
        lblPurRef.Text = Session("Purref")
        lblSalecurr.text = Session("salecurrency")

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim DealCapturer, StatusCancelled, DealType, portfolioid, customernum, DealTypepur As String
        Dim Costamt As Decimal

        'Check for the dealing function
        'Important for distinguishing scenario pullers and actual dealers
        'If CanCancelSecuritySale = False Then
        '    MsgBox("You have not been granted the option to cancel this sale", MsgBoxStyle.Critical, "Deal")
        '    Exit Sub
        'End If

        Try

            strSQL = "select dealcancelled from deals_live where dealreference= '" & Trim(lblsaleref.Text) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                StatusCancelled = Trim(drSQL.Item("DealCancelled").ToString)
            Loop

            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            If Trim(StatusCancelled) = "Y" Then
                MsgBox(" This deal has already been cancelled.", MsgBoxStyle.OkOnly, "Warning")
                Exit Sub
            End If

        Catch ed As Exception
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ed.Message & " Check deal cancelled status", Session("serverName"), Session("client"))
            '************************END****************************************

            Exit Sub
        End Try


        ''When a deal is cancelled set the authorised status to P and the deal cancelled status to Y
        'txtReasonCancel = InputBox("Enter the reason for cancelling the sale.", "Cancel Sale", , , )

        If txtReasonCancel.Text = "" Then
            MsgBox("Please enter the reason for cancelling this deal.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If





        Dim DealEntryType As String

        Try
            strSQL = "select cost from selldetail where refsell='" & Trim(lblsaleref.Text) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                Costamt = CDec(drSQL.Item(0))
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

        'Determine Deal code Portfolio
        Try
            'validate username first
            strSQL = "select entrytype from deals_live " & _
                     " where dealreference ='" & Trim(lblPurRef.Text) & "' "
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()
            Do While drSQL.Read
                DealEntryType = UCase(Trim(drSQL.Item(0).ToString))
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

        Try
            'Record deal cancelling information
            'update deals table and set the deals' cancelled status on the deal to be cancelled
            'update deals table and add back the sale amount to the purchase
            'delete the revaluations made for the sale
            If Trim(DealEntryType) <> "D" Then
                strSQL = " Begin tran T" & _
                        " insert  cancelDeals values('" & lblsaleref.Text & "','" & txtReasonCancel.Text & "','" & Session("username") & "')" & _
                        " update deals_live set DealCancelled = 'Y', AuthorisationStatus='V',datecancelled='" & CDate(Session("SysDate")) & "' where dealreference = '" & Trim(lblsaleref.Text) & "'" & _
                        " update deals_live set dealamount = dealamount+ (select cost from selldetail where refsell='" & Trim(lblsaleref.Text) & "'), " & _
                        " netinterest=netinterest+('" & CDec(lblMaturityAmount.Text) & "' -(select cost from selldetail where refsell='" & Trim(lblsaleref.Text) & "')), " & _
                        " grossinterest=grossinterest+('" & CDec(lblMaturityAmount.Text) & "' -(select cost from selldetail where refsell='" & Trim(lblsaleref.Text) & "')), " & _
                        " intaccruedtodate=intaccruedtodate+'" & CDec(lblAccrued.Text) & "'+(select presentvalue-cost from selldetail where refsell='" & Trim(lblsaleref.Text) & "')" & _
                        " ,maturityamount=maturityamount+'" & CDec(lblMaturityAmount.Text) & "' where dealreference = '" & Trim(lblPurRef.Text) & "'" & _
                        " delete selldetail where refsell = '" & Trim(lblsaleref.Text) & "'" & _
                        " update securitiesconfirmations set DealCancelled = 'Y', AuthorisationStatus='P',datecancelled='" & CDate(Session("SysDate")) & "' where dealreference = '" & lblsaleref.Text & "'" & _
                        " Commit tran T"
            Else

                strSQL = " Begin tran T" & _
                        " insert  cancelDeals values('" & lblsaleref.Text & "','" & txtReasonCancel.Text & "','" & Session("username") & "')" & _
                        " update deals_live set DealCancelled = 'Y', AuthorisationStatus='V',datecancelled='" & CDate(Session("SysDate")) & "' where dealreference = '" & lblsaleref.Text & "'" & _
                        " update deals_live set dealamount = dealamount+ (select cost from selldetail where refsell='" & Trim(lblsaleref.Text) & "'), " & _
                        " netinterest=netinterest+('" & CDec(lblMaturityAmount.Text) & "' -(select cost from selldetail where refsell='" & Trim(lblsaleref.Text) & "')), " & _
                        " grossinterest=grossinterest+('" & CDec(lblMaturityAmount.Text) & "' -(select cost from selldetail where refsell='" & Trim(lblsaleref.Text) & "')), " & _
                        " intaccruedtodate=intaccruedtodate+'" & CDec(lblAccrued.Text) & "'+(select presentvalue-cost from selldetail where refsell='" & Trim(lblsaleref.Text) & "')" & _
                        " ,maturityamount=maturityamount+'" & CDec(lblMaturityAmount.Text) & "' where dealreference = '" & Trim(lblPurRef.Text) & "'" & _
                        " delete selldetail where refsell = '" & Trim(lblsaleref.Text) & "'" & _
                        " update securitiesconfirmations set DealCancelled = 'Y', AuthorisationStatus='V',datecancelled='" & CDate(Session("SysDate")) & "' where dealreference = '" & lblsaleref.Text & "'" & _
                        " Commit tran T"

            End If

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            'Update deal status

            recDaysEffectUpdate2(Trim(lblsaleref.Text))


            MsgBox("Deal cancelled.", MsgBoxStyle.Information)

            'print deal slip

            'Call PrintSlips(Trim(Session("username")), Trim(lblsaleref.Text))

            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("username") & "DTC001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & "Deal Ref : " & lblsaleref.Text & " Reason : " & txtReasonCancel, Session("serverName"), Session("client"))
            '************************END****************************************



        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************

            object_userlog.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

        Dim p As String
        'Retrieve the deal capturers name and the deal code
        Try
            strSQL = "select dealcapturer,dealtype,portfolioid,customernumber,dealbasictype from deals_live" & _
            " join dealtypes on deals_live.dealtype=dealtypes.deal_code where dealreference = '" & Trim(lblsaleref.Text) & "'"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                DealCapturer = Trim(drSQL.Item(0).ToString)
                DealType = Trim(drSQL.Item(1).ToString)
                portfolioid = Trim(drSQL.Item(2).ToString)
                customernum = Trim(drSQL.Item(3).ToString)
                If Trim(drSQL.Item(3).ToString) = "D" Then
                    p = "-"
                Else
                    p = "+"
                End If
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

        Try
            strSQL = "select dealtype from deals_live" & _
            " join dealtypes on deals_live.dealtype=dealtypes.deal_code where dealreference = (select purchaseref from deals_live where dealreference='" & Trim(lblsaleref.Text) & "')"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                DealTypepur = Trim(drSQL.Item(0).ToString)
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

        limitsch.ConnectionString = Session("ConnectionString")

        'Update all limit positions*******************************************
        limitsch.clients = client
        'update dealer limit
        Call limitsch.UpdateDealerLimitCancel(DealCapturer, Trim(lblsaleref.Text), DealType, CDec(lblAmount.Text))
        'Update product Limit sale
        Call limitsch.UpdateProductPosCancel(DealType, CDec(lblAmount.Text))
        'update portfolio limit
        Call limitsch.UpdatePortPosCancel(portfolioid, CDec(lblAmount.Text), p, Trim(lblSalecurr.Text))
        'update counterparty limit
        Call limitsch.UpdateCounterpartyLimitCancel(customernum, DealType, CDec(lblAmount.Text), Trim(lblSalecurr.Text))
        'Update product Limit purchase
        Call limitsch.UpdateProductPos(DealTypepur, Costamt)
        Call limitsch.UpdateAssetHoldingPos("-", CDec(lblAmount.Text), Trim(lblSalecurr.Text)) 'update asset position and limits - portfolio / grouped products
        '***********End********************************************************

        'Call LoadMiniSales(Session("saletb")) 'Get the sales made from the deal
        'Call loadRevaluations(Session("saletb")) 'Get the revaluations done


    End Sub
    'dertermine the number of amendments
    'Retrieve the last amendment so that we can set the actual days the amendment has been in effect
    Private Sub recDaysEffectUpdate2(ByVal dealNumber)
        Try

            Dim y As Integer

            strSQL = "select * from earlymaturity where dealreference = '" & dealNumber & "' order by recnumber desc"
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                y = Int(drSQL.Item("recnumber"))
                Exit Do
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

            strSQL = "update earlymaturity set DaysOnChangeActual=DaysOnChangeTemp,changed='Deal Cancelled' where dealreference = '" & dealNumber & "' and recnumber=" & y & ""
            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader

            Do While drSQL.Read
                y = Int(drSQL.Item("recnumber"))
                Exit Do
            Loop
            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("SecuritiesMaster.aspx")
    End Sub
End Class
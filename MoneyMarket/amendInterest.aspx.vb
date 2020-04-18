
Imports sys_ui
Imports System.Data.SqlClient

Public Class amendInterest
    Inherits System.Web.UI.Page
    Private usrdet As New usrlog.usrlog
    Private PrintPages As Integer
    Private DealType As String
    'Revaluations variables
    Private RevalNetInt As Decimal
    Private RevalIntRate As Decimal
    Private RevalAccrued As Decimal
    Private roll As String = ""
    Private TranxLimitVal As String() 'Stores status of Transaction limits
    Private ccy As String
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private cnSQLx As SqlConnection
    Private strSQLx As String
    Private cmSQLx As SqlCommand
    Private drSQLx As SqlDataReader
    Private object_userlog As New usrlog.usrlog
    Private dealOPs As New mmDeal.MMdealOperations
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then
                txtdealref.Text = Request.QueryString("dealref")
                'Check deal status
                If dealOPs.RetrieveDealStatus(Trim(txtdealref.Text)) = True Then
                    lblError.Text = alert("Deal Cancelled.", "Deal Status")
                    Exit Sub
                End If

                If dealOPs.GetDealStatusAuth(Trim(txtdealref.Text)) = "A" Then
                    If dealOPs.CheckSystemChangeParameter() = "N" Then
                       
                        lblError.Text = alert("Maintence of deals after an authorisation cycle is disabled." & _
                               " Cancel this deal to effect a new change.", "Interest Rate Amendment")
                        Exit Sub
                    End If
                End If

                Dim DealType As String = ""

                Try

                    strSQLx = "select othercharacteristics from deals_live where dealreference= '" & Trim(txtdealref.Text) & "'"
                    cnSQLx = New SqlConnection(Session("ConnectionString"))
                    cnSQLx.Open()
                    cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                    drSQLx = cmSQLx.ExecuteReader()

                    Do While drSQLx.Read
                        DealType = Trim(drSQLx.Item("othercharacteristics").ToString)
                    Loop

                    drSQLx.Close()
                    cnSQLx.Close()
                    cmSQLx.Dispose()
                    cnSQLx.Dispose()

                    If Trim(DealType) = "Discount Purchase" Then
                        If checkSales(Trim(txtdealref.Text)) = True Then

                            lblError.Text = alert("This deal cannot be modified because this Security has sales made from it.", "Warning")
                            Exit Sub
                        End If
                    End If

                    If Trim(DealType) = "Discount sale" Then

                        lblError.Text = alert("This deal cannot be ammended.", "Warning")
                        Exit Sub
                    End If

                    Call LoadDealInfo(txtdealref.Text)

                Catch ex As NullReferenceException

                    lblError.Text = alert("Select a deal that you want to change its interest rate.", "Interest Rate")
                Catch eb As Exception
                    alert(eb.Message, "Error")

                End Try
            Else
                lblError.Text = alert("Select a deal that you want to change tax.", "Incomplete informaton")
                btnValidate.Enabled = False
                btnSave.Enabled = False
            End If
        End If
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtNewInt.Enabled = True
        btnSave.Enabled = False
        txtreason.Enabled = True
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Redirect("mmdealblotter.aspx")
    End Sub

    Protected Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        Dim dealAmnt As Decimal
        Dim TaxationRate As Integer
        Dim Tenor As Integer
        Dim DaysToMaturity As Integer
        Dim intDaysBasis As Integer

        Try

            strSQLx = "select dealAmount,TaxRate,interestrate,othercharacteristics," & _
                     " Tenor,daystomaturity,intdaysbasis,rolloverdeal,currency from deals_live where dealreference= '" & Trim(txtdealref.Text) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            If drSQLx.HasRows = False Then 'Deal reference does not exit
                MsgBox("The deal reference entered does not exist. Input a correct reference and try again ")
               
                Exit Sub

            End If

            If txtNewInt.Text = "" Then  'Prompt the user to enter the new interest rate if new int text box is empty
                MsgBox("Please enter the new interest rate", MsgBoxStyle.OkOnly, "Rate Amendment")
                Exit Sub
            End If

            If Trim(txtreason.Text) = "" Then
                MsgBox("Please enter a valid reason for this operation", MsgBoxStyle.Exclamation, "Deal Tax Change")
                Exit Sub
            End If

            Do While drSQLx.Read
                txtOldInt.Text = drSQLx.Item(2).ToString
                dealAmnt = CDec(drSQLx.Item(0).ToString)
                TaxationRate = CInt(drSQLx.Item(1).ToString)
                Tenor = CInt(drSQLx.Item(4).ToString)
                DaysToMaturity = CInt(drSQLx.Item(5).ToString)
                intDaysBasis = CInt(drSQLx.Item(6).ToString)
                DealType = Trim(drSQLx.Item(3).ToString)
                roll = drSQLx.Item("rolloverdeal").ToString
                ccy = Trim(drSQLx.Item("currency").ToString)
            Loop


            If Trim(roll) = "P" Then
                MsgBox("Amendment not allowed because the deal has pending rollover instructions.", MsgBoxStyle.OkOnly, "Warning")

                drSQLx.Close()
                cnSQLx.Close()
                cmSQLx.Dispose()
                cnSQLx.Dispose()
                Exit Sub
            End If



            'Check Trnasaction Limits if implemented this function will return 1 to indicate that 
            'limits have been exceeded and 2 to indicate that transaction is within the limit
            '0 means Limit checking not implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount
            Dim limitsch As New usrlmt.usrlmt
            limitsch.clients = Session("client")
            limitsch.ConnectionString = Session("ConnectionString")

            TranxLimitVal = limitsch.PeriodLimitValidation(Int(Tenor), getDealType(GetDealCode(Trim(txtdealref.Text))), Trim(Session("username")), _
                            CDbl(dealAmnt), Trim(ccy), GetDealCode(Trim(txtdealref.Text)))

            If TranxLimitVal(0) = "1" Then
                MsgBox("Transaction Limit exceeded" & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       TranxLimitVal(1) & vbCrLf & _
                      "****************************************************" & vbCrLf & _
                       "Limit Amount             :   " & Format(CDbl(TranxLimitVal(2)), "###,###.00") & vbCrLf & _
                       "Transaction Amount :   " & Format(dealAmnt, "###,###.00"), MsgBoxStyle.Information, "Limits")
            End If


            'Select code to execute based on the type of deal

            Select Case DealType

                Case "Basic Deposit"
                    Computations(dealAmnt, CDec(txtNewInt.Text), TaxationRate, Tenor, DaysToMaturity, intDaysBasis)
                    btnSave.Enabled = True
                    'REPORTNAME = "Deal Slip Deposit.rpt"

                Case "Basic Loan"
                    Computations(dealAmnt, CDec(txtNewInt.Text), TaxationRate, Tenor, DaysToMaturity, intDaysBasis)
                    btnSave.Enabled = True
                    'REPORTNAME = "Deal Slip Loan.rpt"

                Case "Discount Sale"
                    MsgBox("Amendment not allowed.", MsgBoxStyle.OkOnly, "Interest Amendment")
                    Exit Select

                Case "Discount Purchase"
                    If checkSales(Trim(txtdealref.Text)) = True Then
                        MsgBox("Amendment not allowed because this Security has sales", MsgBoxStyle.OkOnly, "Rate Change")

                        drSQLx.Close()
                        cnSQLx.Close()
                        cmSQLx.Dispose()
                        cnSQLx.Dispose()
                        Exit Select
                    Else
                        MsgBox("Amendment not allowed .", MsgBoxStyle.OkOnly, "Rate Change")
                    End If

            End Select

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            txtNewInt.Enabled = False
            btnSave.Enabled = True
            txtreason.Enabled = False

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub
    Private Function checkSales(ByVal dealreference As String) As Boolean
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Dim response As Boolean
        Try
            'Save details
            strSQL1 = "Select count(dealreference) from deals_live where purchaseref='" & dealreference & "'"
            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()

            Do While drSQL1.Read
                If Int(drSQL1.Item(0).ToString) = 0 Then
                    response = False
                Else
                    response = True
                End If
            Loop


            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()

            Return response
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function
    Private Sub Computations(ByVal dealAmnt As Decimal, ByVal Intrate As Decimal, ByVal TaxationRate As Integer, ByVal Tenor As Integer, ByVal DaysToMaturity As Integer, ByVal intDaysBasis As Integer)

        Dim NetIntAmnt As Decimal
        Dim GrossIntAmnt As Decimal
        Dim TaxAmount As Decimal
        Dim MaturityAmount As Decimal
        Dim accruedint As Decimal

        GrossIntAmnt = (dealAmnt * Intrate * Tenor) / (intDaysBasis * 100)
        TaxAmount = GrossIntAmnt * TaxationRate / 100
        NetIntAmnt = GrossIntAmnt - TaxAmount
        MaturityAmount = dealAmnt + NetIntAmnt

        accruedint = dealAmnt * Intrate * (Tenor - DaysToMaturity) / (intDaysBasis * 100)
        accruedint = accruedint * (1 - TaxationRate / 100)

        txtDealAmt.Text = Format(CDec(dealAmnt), "###,###,###.00").ToString()
        txtGrossInt.Text = Format(CDec(GrossIntAmnt), "###,###,###.00").ToString()
        txtNetInt.Text = Format((NetIntAmnt), "###,###,###.00").ToString()
        txtTax.Text = Format(CDec(TaxAmount), "###,###,###.00").ToString()
        txtMaturity.Text = Format(CDec(MaturityAmount), "###,###,###.00").ToString()
        txtAccrued.Text = Format(CDec(accruedint), "###,###,###.00").ToString()

    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            'Save details
            Dim OrgTenor As String
            Dim OrgInterestrate As String
            Dim OrgNetInterest As String
            Dim OrgTaxAmount As String
            Dim OrgMaturityDate As String
            Dim OrgMaturityAmount As String
            Dim OrgGross As String
            Dim recn As Integer

            'Get Record number from historical modifications
            Try
                'retrieve the current info before updating
                strSQLx = "select count(dealreference) from earlymaturity where dealreference='" & Trim(txtdealref.Text) & "'"
                cnSQLx = New SqlConnection(Session("ConnectionString"))
                cnSQLx.Open()
                cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                drSQLx = cmSQLx.ExecuteReader()
                If drSQLx.HasRows = True Then
                    Do While drSQLx.Read
                        recn = Int(drSQLx.Item(0)) + 1
                    Loop
                Else
                    recn = 1
                End If

                ' Close and Clean up objects
                drSQLx.Close()
                cnSQLx.Close()
                cmSQLx.Dispose()
                cnSQLx.Dispose()

            Catch eb As Exception
                MsgBox(eb.Message, MsgBoxStyle.Critical)
                Exit Sub
            End Try


            strSQLx = "select maturitydate,tenor,maturityamount,interestrate,netinterest,taxamount,grossinterest from deals_live where dealreference='" & Trim(txtdealref.Text) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                OrgTenor = drSQLx.Item(1).ToString
                OrgInterestrate = drSQLx.Item(3).ToString
                OrgNetInterest = drSQLx.Item(4).ToString
                OrgTaxAmount = drSQLx.Item(5).ToString
                OrgMaturityDate = drSQLx.Item(0).ToString
                OrgMaturityAmount = drSQLx.Item(2).ToString
                OrgGross = drSQLx.Item(6).ToString
            Loop
            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


            txtreason.Text = Replace(Trim(txtreason.Text), "'", "")


            strSQLx = " begin tran O " & _
                     " Update deals_live set netinterest=" & CDec(txtNetInt.Text) & ",maturityamount=" & CDec(txtMaturity.Text) & "," & _
                     " intaccruedTodate=" & CDec(txtAccrued.Text) & ", Grossinterest=" & CDec(txtGrossInt.Text) & ",TaxAmount=" & CDec(txtTax.Text) & "," & _
                     " Datelastmaintained='" & CDate(Session("SysDate")) & "', useridlastupdate='" & Trim(Session("username")) & "'," & _
                     " interestrate='" & txtNewInt.Text & "',authorisationstatus='P' where dealreference='" & Trim(txtdealref.Text) & "'" & _
                      " Update securitiesConfirmations set netinterest=" & CDec(txtNetInt.Text) & ",maturityamount=" & CDec(txtMaturity.Text) & "," & _
                     " intaccruedTodate=" & CDec(txtAccrued.Text) & ",Grossinterest=" & CDec(txtGrossInt.Text) & ",TaxAmount=" & CDec(txtTax.Text) & "," & _
                     " Datelastmaintained='" & CDate(Session("SysDate")) & "', useridlastupdate='" & Trim(Session("username")) & "'," & _
                     " interestrate='" & txtNewInt.Text & "',authorisationstatus='" & RequireFrontAuthoriser() & "' where dealreference='" & Trim(txtdealref.Text) & "'" & _
                     "  insert into EarlyMaturity (oldMaturityDate,oldtenor,oldMaturityAmount,oldinterestrate,oldNetInterest," & _
                     " dealReference,userID,oldtaxAmount,oldgrossinterest,reason,daterevised,changed,recnumber," & _
                     " olddealamount,newdealamount,newMaturityDate,newTenor,newMaturityamount,NewNetInterest,NewGrossInterest," & _
                     " NewTaxAmount,newInterestrate,interestvaluedate)" & _
                     " values ('" & CDate(OrgMaturityDate) & "','" & Int(OrgTenor) & " ', '" & _
                       CDec(OrgMaturityAmount) & "', '" & OrgInterestrate & "','" & CDec(OrgNetInterest) & "','" & _
                        Trim(txtdealref.Text) & "','" & Trim(Session("username")) & "','" & CDec(OrgTaxAmount) & "','" & CDec(OrgGross) & "','" & Trim(txtreason.Text) & "','" & _
                        Session("SysDate") & "','Interest Rate','" & recn & "','" & _
                        CDec(txtDealAmt.Text) & "','" & CDec(txtDealAmt.Text) & "','" & CDate(OrgMaturityDate) & "','" & OrgTenor & "','" & CDec(txtMaturity.Text) & "', '" & _
                        CDec(txtNetInt.Text) & "','" & CDec(txtGrossInt.Text) & "', '" & CDec(txtTax.Text) & "','" & CDec(txtNewInt.Text) & "','" & CDate(Session("SysDate")) & "')" & _
                    "   commit tran O"





            '"  insert into EarlyMaturity (oldMaturityDate,oldtenor,oldMaturityAmount,oldinterestrate,oldNetInterest,dealReference,userID,oldtaxAmount,oldgrossinterest,changed,recnumber,reason)" & _
            '                     " values ('" & CDate(OrgMaturityDate) & "','" & Int(OrgTenor) & "'," & CDec(OrgMaturityAmount) & ", '" & OrgInterestrate & "'," & CDec(OrgNetInterest) & "," & _
            '                     " '" & Trim(txtDealRef.Text) & "' ,'" & Trim(Session("username")) & "','" & CDec(OrgTaxAmount) & "','" & CDec(OrgGross) & "','Interest Rate','" & recn & "','" & Trim(txtreason.Text) & "')" & _

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


            ' 1 to indicate that limits have been exceeded and 
            ' 2 to indicate that transaction is within the limit
            ' 0 means Limit checking not implemented 

            'msg(0) = is the limit status value
            'msg(1) = Decription of limit
            'msg(2) = Limit Amount
            'msg(3) = Limit period
            If TranxLimitVal(0) <> "0" Then
                Dim limitsch As New usrlmt.usrlmt
                limitsch.clients = Session("client")
                limitsch.ConnectionString = Session("ConnectionString")
                limitsch.SaveTranxLimitsDetails(Trim(Session("username")), Int(TranxLimitVal(3)), Trim(TranxLimitVal(1)), Trim(txtdealref.Text) _
                , CDbl(txtDealAmt.Text), CDbl(TranxLimitVal(2)), CDate(Session("SysDate")), Int(TranxLimitVal(0)), Trim(ccy))
            End If


            MsgBox("Interest rate amended sucessfully.", MsgBoxStyle.Information, "Interest Amendment")
            btnSave.Enabled = False

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            Exit Sub
        End Try

        'pass the deal ref parameter and the report name
        'Dim reportview As New csvptt.Reportviewer
        'reportview.strCurrentD = strCurrentDirectory
        'reportview.db = dataBaseName
        'reportview.ReportN = REPORTNAME
        'reportview.dealRefParam = Trim(txtdealref.Text)
        'reportview.PrintPages = PrintPages
        'reportview.Show()


        'Save revaluation if the deal is a security sale

        If DealType = "Security sale" Then
            Try
                'Save details
                strSQLx = "Update revaluations set netinterest='" & RevalNetInt & "',interestrate='" & RevalIntRate & "'," & _
                         " intaccruedtodate='" & RevalAccrued & "' where dealreference='" & Trim(txtdealref.Text) & "'"
                cnSQLx.Open()
                cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                drSQLx = cmSQLx.ExecuteReader()

                ' Close and Clean up objects
                drSQLx.Close()
                cnSQLx.Close()
                cmSQLx.Dispose()
                cnSQLx.Dispose()

                btnSave.Enabled = False

            Catch ex As SqlException
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try

        End If

        'Send notification Email-------------------------------------
        ' usrdet.SendData("GENEMAILMATUCHANGED|A003|" & Trim(txtDealRef.Text), serverName, clients)
        '------------------------------------------------------------

    End Sub
    Public Function RequireFrontAuthoriser() As String
        Dim res As String = ""

        Dim cnSQLX1 As SqlConnection
        Dim cmSQLX1 As SqlCommand
        Dim drSQLX1 As SqlDataReader
        Dim strSQLX1 As String

        Try

            strSQLX1 = "Select [value] from SYSTEMPARAMETERS where [parameter]='frontauth'"
            cnSQLX1 = New SqlConnection(Session("ConnectionString"))
            cnSQLX1.Open()
            cmSQLX1 = New SqlCommand(strSQLX1, cnSQLX1)
            drSQLX1 = cmSQLX1.ExecuteReader()

            Do While drSQLX1.Read
                If Trim(drSQLX1.Item(0).ToString) = "Y" Then
                    res = "N"
                Else
                    res = "P"
                End If
            Loop

            ' Close and Clean up objects
            drSQLX1.Close()
            cnSQLX1.Close()
            cmSQLX1.Dispose()
            cnSQLX1.Dispose()

        Catch ex As Exception

        End Try

        Return res

    End Function
    Public Function GetDealCode(ByVal ref As String) As String
        Dim cnSQL1 As SqlConnection
        Dim cmSQL1 As SqlCommand
        Dim drSQL1 As SqlDataReader
        Dim strSQL1 As String
        Dim code As String = ""
        Try
            'Save details
            strSQL1 = "Select dealtype from AllDealsAll where dealreference='" & Trim(ref) & "'"
            cnSQL1 = New SqlConnection(Session("ConnectionString"))
            cnSQL1.Open()
            cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
            drSQL1 = cmSQL1.ExecuteReader()

            Do While drSQL1.Read
                code = drSQL1.Item(0).ToString
            Loop


            ' Close and Clean up objects
            drSQL1.Close()
            cnSQL1.Close()
            cmSQL1.Dispose()
            cnSQL1.Dispose()



        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

        Return code

    End Function

    'Get deal structure type loan or deposit
    Public Function getDealType(ByVal dealcode As String) As String
        Dim dealType As String = ""
        Dim cnSQL4 As SqlConnection
        Dim cmSQL4 As SqlCommand
        Dim drSQL4 As SqlDataReader
        Dim strSQL4 As String

        Try
            strSQL4 = "select dealbasictype from dealtypes where deal_code = '" & dealcode & "' "
            cnSQL4 = New SqlConnection(Session("ConnectionString"))
            cnSQL4.Open()
            cmSQL4 = New SqlCommand(strSQL4, cnSQL4)
            drSQL4 = cmSQL4.ExecuteReader()

            Do While drSQL4.Read
                If drSQL4.Item(0).ToString = "L" Then
                    dealType = "LoanLimit"
                Else
                    dealType = "depositLimit"
                End If
            Loop
            ' Close and Clean up objects
            drSQL4.Close()
            cnSQL4.Close()
            cmSQL4.Dispose()
            cnSQL4.Dispose()


        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            'LogClass.SendDataToLog(loggedUserLog & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, serverName, clients)
            '************************END****************************************
        End Try

        Return dealType

    End Function
    Public Function LoadDealInfo(ByVal ref As String) As String

        Try
            strSQLx = "select * from deals_live where dealreference = '" & ref & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader

            If drSQLx.HasRows = True Then
                Do While drSQLx.Read

                    'txtDealAmnt.Text = Format(drSQLx.Item("dealamount"), "###,###,###.00")

                    'txtMaturityAmnt.Text = Format(drSQLx.Item("Maturityamount"), "###,###,###.00")
                    txtNetInt.Text = Format(drSQLx.Item("netinterest"), "###,###,###.00")
                    txtGrossInt.Text = Format(drSQLx.Item("grossinterest"), "###,###,###.00")
                    txtOldInt.Text = drSQLx.Item("interestrate").ToString

                    'txtTaxRate.Text = drSQLx.Item("taxrate").ToString
                    'txtTaxAmnt.Text = Format(drSQLx.Item("taxamount"), "###,###,###.00")

                    'txtacruedTax.Text = Format(drSQLx.Item("intaccruedtodate"), "###,###,###.00")




                Loop

            Else
                MsgBox("Cant find deal details", MsgBoxStyle.Critical, "No Details in Database")

            End If

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()




        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function
End Class
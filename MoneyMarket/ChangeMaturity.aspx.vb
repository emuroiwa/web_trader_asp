Imports System.Data.SqlClient
Imports sys_ui
Public Class ChangeMaturity
    Inherits System.Web.UI.Page
    Private usrdet As New usrlog.usrlog
    Private SavDls As New csvptt.csvptt
    Private roll As String
    Private InterestBasisValue As Integer 'Stores the current value for use as the interest dayas basis
    Public AmendType As String = ""
    Private dealCapturer As String = ""
    Private dealcode As String = ""
    Private PortfolioID As String = ""
    Private currency As String = ""
    Private customernumber As String = ""
    Private Apptype As String = ""
    Private CurrentDealAmount As String
    Private crStatus As String 'Stores counterparty limit status 1=exceeded 2= ok
    Private cnSQLx As SqlConnection
    Private cmSQLx As SqlCommand
    Private drSQLx As SqlDataReader
    Private strSQLx As String
    Private AccumulateGross As Decimal = 0
    Private cnSQL As SqlConnection
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader
    Private strSQL As String
    Private globalvars_mmdeal As New GlobalVars.mmDeal   'instance of the userlogins class
    'Private sysAccounting As New accupd.accupdx
    Private object_userlog As New usrlog.usrlog
    Private calc As New mmDeal.CalculationFunctions
    Private updates As New mmDeal.LimitsMatureDeals
    Private clientlogin_vars As New clientlogin.Declarations  'instance of the userlogins class
    '*************************REFERALS***********************************************
    'Private ref As New Referal 'instance of the referals from
    Private RefMessageDealsize As String 'Referal Message Dealsize
    Private RefMessageDaily As String 'Referal Message Daily Limit
    Private referalRequired As Boolean 'Is referal required
    Private refAmount As Decimal 'amount being referred
    Private RefDealType As String 'Deal basic type
    Private dealsizeBroken As Boolean 'Infor on limit status
    Private dailyBroken As Boolean 'Infor on limit status
    Private PortfolioBroken As Boolean 'Infor on limit status
    Private ProductBroken As Boolean 'Infor on limit status
    Private CounterPartyBroken As Boolean 'Infor on limit status
    Private Limitauthoriser As String 'the authoriser
    Private RefMessagePortfolio As String ' Referal Message Portfolio
    Private RefMessageProduct As String 'Referal Message Product
    Private RefMessageCounterparty As String
    Private refIDRecieve As Integer
    Dim checkMaturity As New mmDeal.DealMaturityCheck
    Dim NonBusiness As New mmDeal.DealMaturityCheck
    Dim Holidays As New mmDeal.DealMaturityCheck
    'Referal Message Counterparty
    Private CustName As String
    Private dealOPs As New mmDeal.MMdealOperations
    Private TranxLimitVal As String() 'Stores status of Transaction limits
    Private ccy As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.ServerVariables("QUERY_STRING") Is Nothing Then
                lblRef.Text = Request.QueryString("dealref")
                'Check deal status
                If dealOPs.RetrieveDealStatus(Trim(lblRef.Text)) = True Then
                    'MsgBox("Deal Cancelled", MsgBoxStyle.Exclamation, "Deal Status")
                    lblError.Text = alert("Deal Cancelled", "Deal Status")
                    btnValidate.Enabled = False
                    btnSave.Enabled = False
                    btnReset.Enabled = False
                    Exit Sub
                End If

                If dealOPs.GetDealStatusAuth(Trim(lblRef.Text)) = "A" Then
                    If dealOPs.CheckSystemChangeParameter() = "N" Then
                        'MsgBox("Maintence of deals after an authorisation cycle is disabled." & _
                        '       " Cancel this deal to effect a new change.", MsgBoxStyle.Information, "Maturity Date Amendment")
                        lblError.Text = alert("Maintence of deals after an authorisation cycle is disabled..Cancel this deal to effect a new change..", "Maturity Date Amendment")
                        btnValidate.Enabled = False
                        btnSave.Enabled = False
                        btnReset.Enabled = False
                        Exit Sub
                    End If
                End If


                Dim DealType As String = ""

                Try

                    strSQLx = "select othercharacteristics from deals_live where dealreference= '" & Trim(lblRef.Text) & "'"
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

                    If Trim(DealType) = "Discount Purchase" Or Trim(DealType) = "Discount Sale" Then
                        ' If checkSales(Trim(lstDeals.FocusedItem.Text)) = True Then
                        ' MsgBox(" This deal cannot be modified because this Security has sales made from it.", MsgBoxStyle.OkOnly, "Warning")

                        lblError.Text = alert("This type of amendment not allowed on securities..", "Warning")
                        btnValidate.Enabled = False
                        btnSave.Enabled = False
                        btnReset.Enabled = False
                        Exit Sub
                        'End If
                    End If

                    If Trim(DealType) = "Discount sale" Then
                        'MsgBox(" This deal cannot be ammended.", MsgBoxStyle.OkOnly, "Warning")
                        lblError.Text = alert("This deal cannot be ammended.", "Warning")
                        btnValidate.Enabled = False
                        btnSave.Enabled = False
                        btnReset.Enabled = False
                        Exit Sub
                    End If
                    AmendType = "Change Maturity"
                    Call LoadDealInfo(lblRef.Text)
                Catch ex As NullReferenceException
                    lblError.Text = alert("Select a deal that you want to change its maturity date.", "Deal Tax")
                    btnValidate.Enabled = False
                    btnSave.Enabled = False
                    btnReset.Enabled = False

                Catch eb As Exception
                    alert(eb.Message, "Error")
                End Try
            Else
                lblError.Text = alert("Select a deal that you want to change tax.", "Incomplete informaton")
                btnValidate.Enabled = False
                btnSave.Enabled = False
                btnReset.Enabled = False
            End If
        End If
        AmendType = "Change Maturity"
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        'Response.Redirect("mmdealblotter.aspx")
    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        'Enable controls
        btnSave.Enabled = False
        txtRate.Enabled = True
        dtMaturity.Enabled = True
        btnValidate.Enabled = True
        LoadDealInfo(Trim(lblRef.Text))
        If AmendType = "Change Maturity" Then
            txtPrincipalChange.Enabled = False
        Else
            txtPrincipalChange.Enabled = True
        End If
        AccumulateGross = 0
    End Sub
    Public Function LoadDealInfo(ByVal ref As String) As String

        Try
            strSQLx = "select * from deals_live where dealreference = '" & ref & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader

            If drSQLx.HasRows = True Then
                Do While drSQLx.Read
                    lblRef.Text = drSQLx.Item("dealreference").ToString
                    lblStart.Text = Format(drSQLx.Item("startdate"), "Short Date")
                    lblMaturityDate.Text = Format(drSQLx.Item("Maturitydate"), "Short Date")
                    LblAmount.Text = Format(drSQLx.Item("dealamount"), "###,###,###.00")
                    CurrentDealAmount = Format(drSQLx.Item("dealamount"), "###,###,###.00")
                    lblMaturityAmount.Text = Format(drSQLx.Item("Maturityamount"), "###,###,###.00")
                    lblNet.Text = Format(drSQLx.Item("netinterest"), "###,###,###.00")
                    lblGross.Text = Format(drSQLx.Item("grossinterest"), "###,###,###.00")
                    lblRate.Text = drSQLx.Item("interestrate").ToString
                    txtRate.Text = drSQLx.Item("interestrate").ToString
                    lblTenor.Text = drSQLx.Item("tenor").ToString
                    txtTenor.Text = drSQLx.Item("tenor").ToString
                    lblTaxRate.Text = drSQLx.Item("taxrate").ToString
                    lblTaxAmount.Text = Format(drSQLx.Item("taxamount"), "###,###,###.00")
                    lblCaptureType.Text = drSQLx.Item("othercharacteristics").ToString
                    txtid.Text = drSQLx.Item("tb_id").ToString
                    txtTBID.Text = Trim(drSQLx.Item("othercharacteristics").ToString)
                    txtPrincipalChange.Text = Format(drSQLx.Item("dealamount"), "###,###,###.00")
                    InterestValueDate.Text = Format(CDate(Session("SysDate")), "Long date")
                    dtMaturity.Text = CDate(drSQLx.Item("Maturitydate"))
                    txtAccrrued.Text = Format(drSQLx.Item("intaccruedtodate"), "###,###,###.00")
                    txtDaystomaturity.Text = drSQLx.Item("daystomaturity").ToString
                    dealCapturer = drSQLx.Item("dealcapturer").ToString
                    dealcode = Trim(drSQLx.Item("dealtype").ToString)
                    PortfolioID = Trim(drSQLx.Item("PortfolioID").ToString)
                    customernumber = Trim(drSQLx.Item("customernumber").ToString)
                    currency = Trim(drSQLx.Item("currency").ToString)
                    txtTenor.Text = drSQLx.Item("tenor").ToString

                    If AmendType = "Change Maturity" Then
                        InterestValueDate.Text = CDate(drSQLx.Item("startdate"))
                    End If

                Loop

            Else
                MsgBox("Cant find deal details", MsgBoxStyle.Critical, "No Details in Database")

            End If

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


            txtTenor.Text = Math.Abs(DateDiff(DateInterval.Day, CDate(lblMaturityDate.Text), CDate(lblStart.Text)))

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function

    Protected Sub dtMaturity_TextChanged(sender As Object, e As EventArgs) Handles dtMaturity.TextChanged
        If CDate(dtMaturity.Text) < CDate(Session("SysDate")) Then
            MsgBox("Maturity date cannot be a past date", MsgBoxStyle.Information)
            dtMaturity.Text = CDate(Session("SysDate"))

            Exit Sub
        End If

        txtTenor.Text = Math.Abs(DateDiff(DateInterval.Day, CDate(dtMaturity.Text), CDate(lblStart.Text)))
    End Sub

    Protected Sub txtTenor_TextChanged(sender As Object, e As EventArgs) Handles txtTenor.TextChanged
        Dim x As Long
        Try

            dtMaturity.Text = DateAdd(DateInterval.Day, CDec(txtTenor.Text), CDate(InterestValueDate.Text))
            If x < 0 Then
                'lblError.Text = alert("Maturity date cannot be less than start date", "Maturity Date")
                dtMaturity.Text = CDate(Session("SysDate"))
                dtMaturity.Focus()
                Exit Sub
            End If

        Catch ex As Exception
            'Log the event *****************************************************
            object_userlog.SendDataToLog(Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub

    Protected Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        Dim rate As Decimal = 0
        'referalResponse = 3
        'ResponseReason = 0
        btnSave.Enabled = False
        'Validate the entries on the form before proceeding
        '*********************************************************************

        If Trim(roll) = "P" Then
            MsgBox(" This deal cannot be modified because it has pending rollover instructions.", MsgBoxStyle.OkOnly, "Warning")
            Exit Sub
        End If

        If checkMaturity.NonBusinessDay(dtMaturity.Text) = True Then
            MsgBox("The selected maturity date is a not a business day.", MsgBoxStyle.Critical, "Non-Business Day")
            Exit Sub
        End If

        If checkMaturity.Holidays(dtMaturity.Text, getbasecurrency) = True Then
            MsgBox("The selected maturity date is a not a business day.", "Non-Business Day")
            Exit Sub
        End If

        If txtRate.Text = "" Then
            MsgBox("Enter the interest rate", MsgBoxStyle.Information)
            Exit Sub
        End If


        If lblTaxRate.Text <> "" Then rate = CDec(lblTaxRate.Text)

        Select Case AmendType


            Case "Change Maturity"

                If txtReason.Text = "" Then
                    MsgBox("Enter the reason for maturity date change.", MsgBoxStyle.Exclamation, "Reason")
                    Exit Sub
                End If


                If CDate(dtMaturity.Text) <= CDate(lblStart.Text) Then
                    MsgBox("The maturity date must be greater than or equal to the deal start date.", MsgBoxStyle.Critical, "Maturity Date")
                    Exit Sub
                End If

                'check system value parameter if institutation supports this amendment
                If checkSysParam() <> True Then
                    If CDate(dtMaturity.Text) < CDate(Session("SysDate")) Then
                        MsgBox("The maturity date must be greater than or equal to current business date.", MsgBoxStyle.Critical, "Maturity Date")
                        Exit Sub
                    End If
                Else
                    MsgBox("This will change the maturity date", MsgBoxStyle.Information, "Maturity Date")
                End If

                If checkSysParam() <> True Then
                    If Format(CDate(dtMaturity.Text), "Short Date") < CDate(lblStart.Text) Then
                        MsgBox("Maturity date cannot be less than  start date of the deal.", MsgBoxStyle.Critical, "Maturity Date")
                        Exit Sub
                    End If
                End If
                '***********************End**********************************************


                'Determine number of days each change was applicable

                Call DetermineDays(Trim(lblRef.Text))

                'Calculate values for all changes and sum them up

                Call CalcChangesForDeal(Trim(lblRef.Text), rate)


                'Enable controls
                btnSave.Enabled = True
                txtRate.Enabled = False
                dtMaturity.Enabled = False
                btnValidate.Enabled = False


           

        End Select
    End Sub
    Private Function getSysParam(ByVal parm As String) As String

        Dim x As String = ""
        Dim cnSQLdd As SqlConnection
        Dim cmSQLdd As SqlCommand
        Dim drSQLdd As SqlDataReader
        Dim strSQLdd As String

        Try
            strSQLdd = "select * from SYSTEMPARAMETERS where [parameter]='" & parm & "'"
            cnSQLdd = New SqlConnection(Session("ConnectionString"))
            cnSQLdd.Open()
            cmSQLdd = New SqlCommand(strSQLdd, cnSQLdd)
            drSQLdd = cmSQLdd.ExecuteReader

            Do While drSQLdd.Read
                x = Trim(drSQLdd.Item(1))
            Loop

            ' Close and Clean up objects
            drSQLdd.Close()
            cnSQLdd.Close()
            cmSQLdd.Dispose()
            cnSQLdd.Dispose()


        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)

        End Try

        Return x

    End Function
    'Get deal structure type loan or deposit
    Public Function getDealTypeStructureMature(ByVal dealcode As String) As String
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
                    Session("DealStructure") = "Loan"
                Else
                    Session("DealStructure") = "Deposit"
                End If
            Loop
            ' Close and Clean up objects
            drSQL4.Close()
            cnSQL4.Close()
            cmSQL4.Dispose()
            cnSQL4.Dispose()

            Return Session("DealStructure")

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

    End Function
    Private Function CounterpartyLimitViolation(dealamt As Double, dealtype As String) As String
        Dim x As String = ""
        Dim myLimit As Decimal = CDbl(GetFieldVal(Trim(lblCustomerNumber.Text), "limit", dealtype))
        Dim mycumulativeTotal As Decimal = CDbl(GetFieldVal(Trim(lblCustomerNumber.Text), "cumulativetotal", dealtype))

        MsgBox("Counterparty Limit exceeded" & vbCrLf & _
                  "_____________________________________________________" & vbCrLf & _
                  "Excess : " & Format(myLimit - CDbl(dealamt) + mycumulativeTotal, "###,###.00") & vbCrLf & _
                  "Limit Amount :   " & Format(myLimit, "###,###.00") & vbCrLf & _
                  "_____________________________________________________" & vbCrLf & _
                  "Cumulative Total :   " & Format(mycumulativeTotal, "###,###.00") & vbCrLf & _
                  "Transaction Amount :   " & Format(dealamt, "###,###.00"), MsgBoxStyle.Information, "Limits")


        If getSysParam("tranlmt") = "warn" Then
            MsgBox("Counterparty Limit violation will be recorded", MsgBoxStyle.Exclamation, "Counterparty Limit exceeded")
            x = "warn"
        ElseIf getSysParam("tranlmt") = "stop" Then
            MsgBox("Counterparty Limit exceeded, you cannot proceed with the transaction", MsgBoxStyle.Critical, "Counterparty Limit exceeded")
            x = "stop"
        End If

        Return x
    End Function

    'Returns the requested field as specified for the returnval parameter
    Private Function GetFieldVal(ByVal fldkey As String, ByVal returnVal As String, dealtype As String) As Decimal
        Dim x As Decimal = 0

        Try
            strSQL = "select " & returnVal & " from COUNTERLIMITS where customernumber='" & Trim(fldkey) & "' and dealtype='" & dealtype & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                If drSQL.IsDBNull(0) = False Then
                    x = (Trim(drSQL.Item(0).ToString))
                End If
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()


        Catch ex As NullReferenceException

        Catch eb As Exception
            MsgBox(eb.Message, MsgBoxStyle.Information, "Error")
        End Try

        Return x

    End Function
    Private Sub DetermineDays(ByVal ref As String)
        Dim RecTotal As Integer = 0
        Dim x, y As Integer
        'dertermine the number of amendments
        'Try
        On Error Resume Next
        strSQLx = "select * from earlymaturity where dealreference = '" & ref & "' order by recnumber desc"
        cnSQLx = New SqlConnection(Session("ConnectionString"))
        cnSQLx.Open()
        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
        drSQLx = cmSQLx.ExecuteReader

        Do While drSQLx.Read
            x = Math.Abs(DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(drSQLx.Item("interestvaluedate"))))
            y = Int(drSQLx.Item("recnumber"))
            Exit Do
        Loop
        ' Close and Clean up objects
        drSQLx.Close()
        cnSQLx.Close()
        cmSQLx.Dispose()
        cnSQLx.Dispose()



        strSQLx = "update earlymaturity set daysonchangetemp=" & x & " where dealreference='" & ref & "' and recnumber=" & y & ""
        cnSQLx = New SqlConnection(Session("ConnectionString"))
        cnSQLx.Open()
        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
        drSQLx = cmSQLx.ExecuteReader

        ' Close and Clean up objects
        drSQLx.Close()
        cnSQLx.Close()
        cmSQLx.Dispose()
        cnSQLx.Dispose()


        'Catch ex As SqlException
        '    MsgBox(ex.Message, MsgBoxStyle.Critical)
        'End Try

    End Sub
    Private Sub CalcChangesForDeal(ByVal dealref As String, ByVal taxrate As Decimal)

        Dim DaysinPeriod As Integer
        Dim dealValue As Decimal
        Dim dealRate As Decimal
        AccumulateGross = 0

        On Error Resume Next
        strSQLx = "select DaysOnChangetemp,NewDealAmount,NewInterestRate from earlymaturity where dealreference = '" & dealref & "'"
        cnSQLx = New SqlConnection(Session("ConnectionString"))
        cnSQLx.Open()
        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
        drSQLx = cmSQLx.ExecuteReader

        Do While drSQLx.Read
            DaysinPeriod = Int(drSQLx.Item("daysonchangetemp"))
            dealValue = CDec(drSQLx.Item("NewDealAmount"))
            dealRate = CDec(drSQLx.Item("NewInterestRate"))

            CalcEnginePrincipal(DaysinPeriod, dealRate, dealValue)

        Loop

        Select Case AmendType


            Case "Change Maturity"
                'Get Value for new changes with new principal
                CalcEnginePrincipal(Math.Abs(DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(dtMaturity.Text))), CDec(txtRate.Text), CDec(txtPrincipalChange.Text))
            Case Else
                'Get Value for new changes with new principal
                CalcEnginePrincipal(CDec(txtDaystomaturity.Text), CDec(txtRate.Text), CDec(txtPrincipalChange.Text))

        End Select


        'calculate net interest,marurity value and tax amount
        calc.taxAmount = AccumulateGross * (taxrate / 100)
        calc.netInt = (AccumulateGross - calc.taxAmount)
        calc.maturityAmount = calc.netInt + CDec(txtPrincipalChange.Text)

        lblNet.Text = Format(calc.netInt, "###,###,###.00")
        lblGross.Text = Format(AccumulateGross, "###,###,###.00")
        lblTaxAmount.Text = Format(calc.taxAmount, "###,###,###.00")
        lblMaturityAmount.Text = Format(CDec(calc.maturityAmount), "###,###,###.00")
        LblAmount.Text = Format(CDec(txtPrincipalChange.Text), "###,###,###.00")
        lblRate.Text = txtRate.Text
        txtDiscountRate.Text = calc.DiscountRateDerived


        ' Close and Clean up objects
        drSQLx.Close()
        cnSQLx.Close()
        cmSQLx.Dispose()
        cnSQLx.Dispose()




    End Sub
    'Calculations for non-discounted deals
    Public Function CalcEnginePrincipal(ByVal trn As Decimal, ByVal rate As Decimal, ByVal dealAm As Decimal) As Boolean
        AccumulateGross = AccumulateGross + (trn * rate * dealAm) / (InterestBasisValue * 100)
        'taxAmount = grossInt * (taxRate / 100)
        'netInt = (grossInt - taxAmount)
        'maturityAmount = netInt + dealAm
        'DiscountRateDerived = (grossInt * (InterestBasisValue * 100)) / (trn * (dealAm + grossInt))

        'intAccruedToDate = Decimal.Round(CDec(netInt), 2)
        'netInt = Decimal.Round(CDec(netInt), 2)
        'grossInt = Decimal.Round(CDec(grossInt), 2)
        'taxAmount = Decimal.Round(CDec(taxAmount), 2)
        'maturityAmount = Decimal.Round(CDec(maturityAmount), 2)
        'DiscountRateDerived = Decimal.Round(CDec(DiscountRateDerived), 9)
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
    Public Function getBaseCurrency() As String
        Dim x As String
        Try
            'validate username first
            strSQLx = "select currencycode from currencies where isbasecurrency='yes'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                x = Trim(drSQLx.Item(0))
            Loop


            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            Return x

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Function
    Private Function checkSysParam() As Boolean
        Dim x As Boolean
        Try
            strSQLx = "select [value] from systemparameters where [parameter] = 'matbkval'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader
            If drSQLx.HasRows = True Then
                Do While drSQLx.Read
                    If Trim(drSQLx.Item(0).ToString) = "Y" Then
                        x = True
                    Else
                        x = False
                    End If
                Loop
            Else
                x = False
            End If

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            Return x

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)

            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
    End Function

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim daystoMat As String = 0
        'Check for the dealing function
        'Important for distinguishing scenario pullers and actual dealers
        If Session("IsDealer") = False Then
            MsgBox("You have not been granted the dealing option", MsgBoxStyle.Critical, "Deal")
            Exit Sub
        End If

        If txtAccrrued.Text = "" Then txtAccrrued.Text = "0"

        'SavDls.clients = Session("client")
        SavDls.strCurrentD = clientlogin_vars.strCurrentDirectory
        'SavDls.databasename = dataBaseName



        txtReason.Text = Replace(Trim(txtReason.Text), "'", "")
        txtReason.Text = Replace(Trim(txtReason.Text), "&", "and")


        Select Case AmendType


            Case "Change Maturity"

                daystoMat = DateDiff(DateInterval.Day, CDate(Session("SysDate")), CDate(dtMaturity.Text))
                If daystoMat < 0 Then
                    daystoMat = 0
                End If

                SavDls.saveLoanDepositUpdate(CDate(dtMaturity.Text), CDec(lblMaturityAmount.Text), CDec(txtPrincipalChange.Text), CDec(txtRate.Text) _
                , CDec(lblNet.Text), Int(txtTenor.Text), CDec(lblTaxAmount.Text), Trim(lblRef.Text), _
                daystoMat, Trim(lblCaptureType.Text), CDec(lblGross.Text), CDec(txtAccrrued.Text), _
                CDec(txtDiscountRate.Text), Trim(Session("username")), Session("SysDate"), Session("serverName"), Session("dataBaseName"), globalvars_mmdeal.PrintPages, Trim(txtReason.Text), "Maturity Date", _
                 Trim(dealCapturer), dealcode, PortfolioID, currency, customernumber, dealApptype(dealcode), CDate(InterestValueDate.Text))



                'Check if the deal is set to mature right away
                If daystoMat = 0 Then
                    'release security for deals that have matured so that it becomes available
                    'for sale or to secure other deals
                    Try
                        strSQLx = "select dealreference from deals_live" & _
                        " where daystomaturity=0 and securityrequired ='Y'"

                        cnSQLx = New SqlConnection(Session("ConnectionString"))
                        cnSQLx.Open()
                        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
                        drSQLx = cmSQLx.ExecuteReader

                        Do While drSQLx.Read
                            'Update securities table if records found matching criteria
                            UpdateSecurities(drSQLx.Item(0))
                        Loop

                        ' Close and Clean up objects
                        drSQLx.Close()
                        cnSQLx.Close()
                        cmSQLx.Dispose()
                        cnSQLx.Dispose()

                    Catch ec As Exception
                        MsgBox(ec.Message, MsgBoxStyle.Information)
                        'Log the event *****************************************************
                        usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ec.Message, Session("serverName"), Session("client"))
                        '************************END****************************************
                    End Try
                End If

                Call MaturedDeals()




            Case "Principal Increase"


                SavDls.saveLoanDepositUpdate(CDate(lblMaturityDate.Text), CDec(lblMaturityAmount.Text), CDec(txtPrincipalChange.Text), CDec(txtRate.Text) _
                , CDec(lblNet.Text), Int(lblTenor.Text), CDec(lblTaxAmount.Text), Trim(lblRef.Text), _
                Trim(txtDaystomaturity.Text), Trim(lblCaptureType.Text), CDec(lblGross.Text), CDec(txtAccrrued.Text), _
                CDec(txtDiscountRate.Text), Trim(Session("username")), Session("SysDate"), Session("serverName"), Session("dataBaseName"), globalvars_mmdeal.PrintPages, Trim(txtReason.Text), "Principal Increase", _
                Trim(dealCapturer), dealcode, PortfolioID, currency, customernumber, dealApptype(dealcode), CDate(InterestValueDate.Text))

                'Send notification Email-------------------------------------
                'usrdet.SendData("GENEMAILMATUCHANGED|A003|" & Trim(lblRef.Text), serverName, Session("client"))
                '------------------------------------------------------------
                ' 1 to indicate that limits have been exceeded and 
                ' 2 to indicate that transaction is within the limit
                ' 0 means Limit checking not implemented 

                'msg(0) = is the limit status value
                'msg(1) = Decription of limit
                'msg(2) = Limit Amount
                'msg(3) = Limit period
                Dim limitsch As New usrlmt.usrlmt
                limitsch.clients = Session("client")
                limitsch.ConnectionString = Session("ConnectionString")

                'If TranxLimitVal(0) <> "0" Then
                '    limitsch.SaveTranxLimitsDetails(Trim(Session("username")), Int(TranxLimitVal(3)), Trim(TranxLimitVal(1)), Trim(lblRef.Text) _
                '    , CDbl(LblAmount.Text), CDbl(TranxLimitVal(2)), CDate(Session("SysDate")), Int(TranxLimitVal(0)), Trim(lblCurrency.Text))
                'End If

                'Save counterparty limit information detail here
                limitsch.SaveCounterpartyLimitsDetails(Trim(Session("username")), Trim(lblRef.Text), CDec(txtPrincipalChange.Text), GetFieldVal(Trim(lblCustomerNumber.Text), "limit", "Loan") _
                  , CDate(Session("SysDate")), crStatus, Trim(lblCurrency.Text), "L", Trim(lblCustomerNumber.Text), Int(TranxLimitVal(3)), Trim(TranxLimitVal(1)), CDbl(TranxLimitVal(2)), _
                  Int(TranxLimitVal(0)), "")



            Case "Principal Decrease"

                SavDls.saveLoanDepositUpdate(CDate(lblMaturityDate.Text), CDec(lblMaturityAmount.Text), CDec(txtPrincipalChange.Text), CDec(txtRate.Text) _
                , CDec(lblNet.Text), Int(lblTenor.Text), CDec(lblTaxAmount.Text), Trim(lblRef.Text), _
                 Trim(txtDaystomaturity.Text), Trim(lblCaptureType.Text), CDec(lblGross.Text), CDec(txtAccrrued.Text), _
                 CDec(txtDiscountRate.Text), Trim(Session("username")), Session("SysDate"), Session("serverName"), Session("dataBaseName"), globalvars_mmdeal.PrintPages, Trim(txtReason.Text), "Principal Decrease", _
                  Trim(dealCapturer), dealcode, PortfolioID, currency, customernumber, dealApptype(dealcode), CDate(InterestValueDate.Text))



                Dim limitsch As New usrlmt.usrlmt
                limitsch.clients = Session("client")
                limitsch.ConnectionString = Session("ConnectionString")

              

                'Save counterparty limit information detail here
                limitsch.SaveCounterpartyLimitsDetails(Trim(Session("username")), Trim(lblRef.Text), CDec(txtPrincipalChange.Text), GetFieldVal(Trim(lblCustomerNumber.Text), "limit", "Loan") _
                  , CDate(Session("SysDate")), crStatus, Trim(lblCurrency.Text), "L", Trim(lblCustomerNumber.Text), Int(TranxLimitVal(3)), Trim(TranxLimitVal(1)), CDbl(TranxLimitVal(2)), _
                  Int(TranxLimitVal(0)), "")



        End Select
    End Sub
    Private Function dealApptype(ByVal dealcode As String) As String
        Dim x As String
        Dim cnSQL3 As New SqlConnection
        Dim cmSQL3 As New SqlCommand
        Dim strSQL3 As String
        Dim drSQL3 As SqlDataReader

        Try
            strSQL3 = "select dealbasictype from dealtypes where deal_code='" & Trim(dealcode) & "'"
            cnSQL3 = New SqlConnection(Session("ConnectionString"))
            cnSQL3.Open()
            cmSQL3 = New SqlCommand(strSQL3, cnSQL3)
            drSQL3 = cmSQL3.ExecuteReader

            Do While drSQL3.Read
                If Trim(drSQL3.Item("dealbasictype").ToString) = "L" Then
                    x = "-"
                Else
                    x = "+"
                End If

            Loop
            ' Close and Clean up objects
            drSQL3.Close()
            cnSQL3.Close()
            cmSQL3.Dispose()
            cnSQL3.Dispose()

            Return x

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ec.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try

    End Function
    'check for deals that have matured and move them to the deals matured table
    Private Function MaturedDeals()
        'Check for TB purchases that could be maturing
        'if exists check the revaluations table for sales made relating to the purchase
        'if exists take the interest accrued and add it to the matuaring purchase
        'change the status of the shaddow deal to matured to prevent further accruals

        '*************************Start**********
        'step 1
        'Check for TB purchases that could be maturing
        Try
            strSQLx = " select * from  deals_live where daystomaturity=0 " & _
         " and othercharacteristics='Discount Purchase' "
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader
            Do While drSQLx.Read
                'STEP 2
                'if exists check the revaluations table for sales made relating to the purchase
                'if exists take the interest accrued and add it to the matuaring purchase
                'change the status of the shaddow deal to matured to prevent further accruals
                Dim cnSQL1 As SqlConnection
                Dim cmSQL1 As SqlCommand
                Dim drSQL1 As SqlDataReader
                Dim strSQL1 As String

                strSQL1 = "begin tran UpdateMaturing " & _
                          " declare @RecordsReturned int " & _
                          " select @RecordsReturned=count(tb_id) from revaluations where TB_ID='" & Trim(drSQLx.Item("TB_ID").ToString()) & "' and  refsource='" & Trim(drSQLx.Item("dealreference").ToString()) & "' and matured='N'" & _
                          " if  @RecordsReturned > 0 " & _
                          " Begin " & _
                          " update deals_live set intaccruedtodate = intaccruedtodate+" & _
                          " (select sum(intaccruedtodate) from revaluations where tb_id='" & Trim(drSQLx.Item("TB_ID").ToString()) & "' and   refsource='" & Trim(drSQLx.Item("dealreference").ToString()) & "'and matured='N') where dealreference='" & Trim(drSQLx.Item("dealreference")) & "'" & _
                          "  update revaluations set matured = 'Y' where tb_id = '" & Trim(drSQLx.Item("TB_ID").ToString()) & "' and  refsource='" & Trim(drSQLx.Item("dealreference").ToString()) & "' " & _
                          "  update securitypurchase set matured = 'Y' where tb_id = '" & Trim(drSQLx.Item("TB_ID").ToString()) & "' " & _
                          " End " & _
                          " else " & _
                          "  update revaluations set matured = 'Y' where tb_id = '" & Trim(drSQLx.Item("TB_ID").ToString()) & "' and  refsource='" & Trim(drSQLx.Item("dealreference").ToString()) & "'" & _
                          "  update securitypurchase set matured = 'Y' where tb_id = '" & Trim(drSQLx.Item("TB_ID").ToString()) & "' " & _
                          " commit tran UpdateMaturing"
                cnSQL1 = New SqlConnection(Session("ConnectionString"))
                cnSQL1.Open()
                cmSQL1 = New SqlCommand(strSQL1, cnSQL1)
                drSQL1 = cmSQL1.ExecuteReader

                ' Close and Clean up objects
                drSQL1.Close()
                cnSQL1.Close()
                cmSQL1.Dispose()
                cnSQL1.Dispose()

            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            'Log this activity
        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ec.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try
        '*******************************End*****************

        'STEP 3
        '************************************************
        'Get all sales that are maturing today
        'Get the revaluation and add the accrued todate back to the purchase
        'Get the sale's dealamount and add it back to the purchase
        '************************************************
        'Get all sales that are maturing today
        Try
            'get sale
            strSQLx = "select dealreference,purchaseref,dealamount,daystomaturity from deals_live" & _
            " where daystomaturity=0 and othercharacteristics ='Discount Sale'"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader

            'if there are records in the record set look for the purchase
            'that made the sale and then compare the maturity dates using the days 
            'to maturity field
            Do While drSQLx.Read
                Dim cnSQL9 As SqlConnection
                Dim cmSQL9 As SqlCommand
                Dim drSQL9 As SqlDataReader
                Dim strSQL9 As String

                'Get Purchase
                strSQL9 = "select * from deals_live where dealreference='" & Trim(drSQLx.Item("purchaseref")) & "'"

                cnSQL9 = New SqlConnection(Session("ConnectionString"))
                cnSQL9.Open()
                cmSQL9 = New SqlCommand(strSQL9, cnSQL9)
                drSQL9 = cmSQL9.ExecuteReader
                'Compare days to maturity field to determine if its a buy back
                Do While drSQL9.Read
                    If Int(drSQL9.Item("daystomaturity")) > 0 Then 'Its a buy back
                        'Buy back operations
                        Call BuyBackOpps(Trim(drSQLx.Item("dealreference")), Trim(drSQL9.Item("dealreference")), CDec(drSQLx.Item("dealamount")))
                    End If
                Loop

                ' Close and Clean up objects
                drSQL9.Close()
                cnSQL9.Close()
                cmSQL9.Dispose()
                cnSQL9.Dispose()

            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ec.Message, Session("serverName"), Session("client"))
            '************************END****************************************

        End Try

        'STEP 4
        'Move matured deals to the matured deals table
        'First update all limits tables
        Dim cnSQL8 As SqlConnection
        Dim cmSQL8 As SqlCommand
        Dim drSQL8 As SqlDataReader
        Dim strSQL8 As String

        Try
            strSQL8 = "begin tran Deal " & _
                     " select * from  deals_live where daystomaturity=0 " & _
                     " Commit tran Deal "

            cnSQL8 = New SqlConnection(Session("ConnectionString"))
            cnSQL8.Open()
            cmSQL8 = New SqlCommand(strSQL8, cnSQL8)
            drSQL8 = cmSQL8.ExecuteReader

            Do While drSQL8.Read
                Dim userId, Dealref, dealcode, portfolioID, cusmum As String
                Dim dealamount, MaturityAmount As Decimal

                userId = Trim(drSQL8.Item("dealcapturer").ToString)
                Dealref = Trim(drSQL8.Item("dealreference").ToString)
                dealcode = Trim(drSQL8.Item("dealtype").ToString)
                dealamount = CDec(drSQL8.Item("dealamount").ToString)
                MaturityAmount = CDec(drSQL8.Item("maturityamount").ToString)
                portfolioID = Trim(drSQL8.Item("portfolioID").ToString)
                cusmum = Trim(drSQL8.Item("customernumber").ToString)

                'Update positions
                updates.UpdateProductPosMature(dealcode, dealamount, getDealTypeStructureMature(dealcode))
                updates.UpdateCounterpartyLimitMature(cusmum, dealcode, dealamount)
                If drSQL8.IsDBNull(38) = True Then
                    updates.UpdatePortPosMature(portfolioID, MaturityAmount, getDealTypeStructureMature(dealcode))
                End If
            Loop

            ' Close and Clean up objects
            drSQL8.Close()
            cnSQL8.Close()
            cmSQL8.Dispose()
            cnSQL8.Dispose()

        Catch ex As Exception
            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ex.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try


        Dim x As Integer
        Try
            strSQLx = "begin tran Deal " & _
                     " insert into deals_matured " & _
                     " select * from  deals_live where daystomaturity=0 " & _
                    " delete deals_live where daystomaturity=0 " & _
                     " Commit tran Deal "

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader

            x = drSQLx.RecordsAffected()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            'Log this activity

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ec.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Function
    'Update purchase and revaluation
    Private Function BuyBackOpps(ByVal dealrefSale As String, ByVal dealrefPurchase As String, ByVal dealAmt As Decimal)
        Dim cnSQL19 As SqlConnection
        Dim cmSQL19 As SqlCommand
        Dim drSQL19 As SqlDataReader
        Dim strSQL19 As String

        Dim DealPortfolio As String

        'Determine Deal code Portfolio
        Try
            'validate username first
            strSQL19 = "select portfolioname from deals_live join portfoliostructure on portfoliostructure.portfolioid=deals_live.portfolioid  " & _
                     " where dealreference ='" & dealrefPurchase & "' "
            cnSQL19 = New SqlConnection(Session("ConnectionString"))
            cnSQL19.Open()
            cmSQL19 = New SqlCommand(strSQL19, cnSQL19)
            drSQL19 = cmSQL19.ExecuteReader()
            Do While drSQL19.Read
                DealPortfolio = UCase(Trim(drSQL19.Item(0).ToString))
            Loop
            ' Close and Clean up objects
            drSQL19.Close()
            cnSQL19.Close()
            cmSQL19.Dispose()
            cnSQL19.Dispose()

        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

        Dim IntAccruedReval As Decimal
        Try
            If DealPortfolio <> "BANKERS ACCEPTANCE" Then
                strSQL19 = "begin tran O " & _
                           " update deals_live set dealamount=dealamount+" & dealAmt & ",maturityamount = maturityamount+ '" & dealAmt & "',intaccruedtodate=" & _
                           " intaccruedtodate+(select intaccruedtodate from " & _
                           " revaluations where dealreference = '" & dealrefSale & "')where dealreference= " & _
                           " '" & dealrefPurchase & "'" & _
                           " update revaluations set matured = 'Y' where dealreference = '" & dealrefSale & "'" & _
                           " Commit tran O"
            Else

                strSQL19 = " Begin tran T" & _
                         " update deals_live set maturityamount = maturityamount+ '" & dealAmt & "', " & _
                         " dealamount=dealamount+" & dealAmt & "," & _
                         " intaccruedtodate=intaccruedtodate+(select intaccruedtodate from " & _
                         " revaluations where dealreference = '" & dealrefSale & "')where dealreference= " & _
                         " '" & dealrefPurchase & "'" & _
                         " update revaluations set matured = 'Y' where dealreference = '" & dealrefSale & "'" & _
                         " Commit tran T"
            End If

            cnSQL19 = New SqlConnection(Session("ConnectionString"))
            cnSQL19.Open()
            cmSQL19 = New SqlCommand(strSQL19, cnSQL19)
            drSQL19 = cmSQL19.ExecuteReader

            ' Close and Clean up objects
            drSQL19.Close()
            cnSQL19.Close()
            cmSQL19.Dispose()
            cnSQL19.Dispose()

        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ec.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Function
    'this function release security from matured deals so that it becomes 
    'available for sale 
    Private Sub UpdateSecurities(ByVal ref As String)

        Dim cnSQL3 As New SqlConnection
        Dim cmSQL3 As New SqlCommand
        Dim strSQL3 As String
        Dim drSQL3 As SqlDataReader

        Try
            strSQL3 = "update attachedsecurities" & _
                     " set matured='Y' where Dealreference='" & ref & "'"
            cnSQL3 = New SqlConnection(Session("ConnectionString"))
            cnSQL3.Open()
            cmSQL3 = New SqlCommand(strSQL3, cnSQL3)
            drSQL3 = cmSQL3.ExecuteReader

            ' Close and Clean up objects
            drSQL3.Close()
            cnSQL3.Close()
            cmSQL3.Dispose()
            cnSQL3.Dispose()


        Catch ec As Exception
            MsgBox(ec.Message, MsgBoxStyle.Information)
            'Log the event *****************************************************
            usrdet.SendDataToLog(Session("username") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & " " & ec.Message, Session("serverName"), Session("client"))
            '************************END****************************************
        End Try
    End Sub

End Class
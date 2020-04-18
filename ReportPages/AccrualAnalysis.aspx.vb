Imports System.Data.SqlClient

Public Class AccrualAnalysis
    Inherits System.Web.UI.Page
    Private cnSQL As SqlConnection
    Private strSQL As String
    Private cmSQL As SqlCommand
    Private drSQL As SqlDataReader

    Private formType As Integer = 1
    Private selectedcustomers As String
    Private curr As String
    Private object_userlog As New usrlog.usrlog
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetCurrency()
            ' LoadCustomers()
            DateSystem.Text = Session("SysDate")
        End If
    End Sub
    'Private Sub LoadCustomers()

    '    Try
    '        strSQL = "select customer_Number,fullName from customer where frontoffice='Y' and len(customer_number)<=9 order by fullname"
    '        cnSQL = New SqlConnection(Session("ConnectionString"))
    '        cnSQL.Open()
    '        cmSQL = New SqlCommand(strSQL, cnSQL)
    '        drSQL = cmSQL.ExecuteReader


    '        GridCustomer.DataSource = drSQL
    '        GridCustomer.DataBind()

    '        ' Close and Clean up objects
    '        drSQL.Close()
    '        cnSQL.Close()
    '        cmSQL.Dispose()
    '        cnSQL.Dispose()
    '        'ernest
    '        '' CusDownloadStart.Suspend()

    '    Catch ec As Exception
    '        '  lblError.Text = alert(ec.Message, "Error")
    '    End Try
    'End Sub
    'Private Sub getdeals()


    '    For Each item As EO.Web.GridItem In GridCustomer.CheckedItems
    '        selectedcustomers = selectedcustomers & ",'" & item.Cells(1).Value & "'"

    '    Next
    '    selectedcustomers = Trim(Mid(selectedcustomers, 2, Len(selectedcustomers)))
    'End Sub

    'Loads details for live deals for Assets
    Private Sub LoadLiveDealsAssets(val As Integer, ccy As String)

        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        Dim cnSQLy As SqlConnection
        Dim cmSQLy As SqlCommand
        Dim drSQLy As SqlDataReader
        Dim strSQLy As String

        Dim AssetGroups As String
        Dim GroupID As Integer

        On Error Resume Next

        'Determine the groups in the results first so that we will know the group description/Product Description
        strSQLy = "select groupid from ACCRUALANALYSISGRP where " & _
                 " type='Asset'"

        cnSQLy = New SqlConnection(Session("ConnectionString"))
        cnSQLy.Open()
        cmSQLy = New SqlCommand(strSQLy, cnSQLy)
        drSQLy = cmSQLy.ExecuteReader()

        Do While drSQLy.Read

            GroupID = drSQLy.Item(0)

            'Determine the groups in the results first so that we will know the group description/Product Description
            strSQLx = "select description,parentgroupid from ACCRUALANALYSISGRP where " & _
                     " type='Asset' and parentgroupid<>groupid and parentgroupid=" & GroupID & ""

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            AssetGroups = ""
            Do While drSQLx.Read

                AssetGroups = AssetGroups & Trim(drSQLx.Item(0).ToString) & " "

            Loop


            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()


            'Determine the groups in the results first so that we will know the group description/Product Description
            strSQLx = "select description,parentgroupid from ACCRUALANALYSISGRP where " & _
                     " type='Asset' and parentgroupid<>groupid and parentgroupid=" & GroupID & ""

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read

                'construct grouping structure
                Dim ExtractGroupFormat As String() = Split(Trim(drSQLx.Item(0).ToString), "-")
                Dim upper, lower As Integer

                'In the comparisons below make sure a higher comparison will include the lower groups in the ranges
                'Selected if the lower ranges are not selected by the user.
                Select Case ExtractGroupFormat(1)

                    Case "Up to 90Days"
                        upper = 90
                        lower = 1

                    Case "Up to 180Days"
                        If AssetGroups.Contains("Up to 90Days") Then
                            upper = 180
                            lower = 91
                        Else
                            upper = 180
                            lower = 1
                        End If


                    Case "Up to 365Days"
                        If AssetGroups.Contains("Up to 180Days") Then
                            upper = 365
                            lower = 181
                            Exit Select
                        End If
                        If AssetGroups.Contains("Up to 180Days") = False And _
                           AssetGroups.Contains("Up to 90Days") = True Then
                            upper = 365
                            lower = 91
                            Exit Select
                        End If
                        If AssetGroups.Contains("Up to 90Days") = False And _
                           AssetGroups.Contains("Up to 180Days") = False Then
                            upper = 365
                            lower = 1
                            Exit Select
                        End If

                    Case "Up to 2Years"
                        If AssetGroups.Contains("Up to 365Days") Then
                            upper = 730
                            lower = 366
                            Exit Select
                        End If
                        If AssetGroups.Contains("Up to 365Days") = False And _
                           AssetGroups.Contains("Up to 180Days") = True Then
                            upper = 730
                            lower = 181
                            Exit Select
                        End If

                        If AssetGroups.Contains("Up to 180Days") = False And _
                           AssetGroups.Contains("Up to 90Days") = True And _
                           AssetGroups.Contains("Up to 365Days") = False Then

                            upper = 730
                            lower = 91
                            Exit Select

                        End If

                        If AssetGroups.Contains("Up to 90Days") = False And _
                         AssetGroups.Contains("Up to 180Days") = False And _
                         AssetGroups.Contains("Up to 365Days") = False Then
                            upper = 730
                            lower = 1
                            Exit Select
                        End If



                    Case "Up to 3Years"
                        If AssetGroups.Contains("Up to 2Years") Then

                            upper = 1095
                            lower = 731
                            Exit Select

                        End If

                        If AssetGroups.Contains("Up to 2Years") = False And _
                           AssetGroups.Contains("Up to 365Days") = True Then
                            upper = 1095
                            lower = 366
                            Exit Select
                        End If

                        If AssetGroups.Contains("Up to 2Years") = False And _
                           AssetGroups.Contains("Up to 365Days") = False And _
                           AssetGroups.Contains("Up to 180Days") = True Then
                            upper = 1095
                            lower = 181
                            Exit Select
                        End If

                        If AssetGroups.Contains("Up to 2Years") = False And _
                           AssetGroups.Contains("Up to 365Days") = False And _
                           AssetGroups.Contains("Up to 180Days") = False And _
                           AssetGroups.Contains("Up to 90Days") = True Then
                            upper = 1095
                            lower = 91
                            Exit Select
                        End If

                        If AssetGroups.Contains("Up to 2Years") = False And _
                           AssetGroups.Contains("Up to 365Days") = False And _
                           AssetGroups.Contains("Up to 180Days") = False And _
                           AssetGroups.Contains("Up to 90Days") = False Then
                            upper = 730
                            lower = 1
                            Exit Select
                        End If


                    Case "Up to 5Years"
                        If AssetGroups.Contains("Up to 3Years") Then
                            upper = 1825
                            lower = 1096
                            Exit Select
                        End If
                        If AssetGroups.Contains("Up to 3Years") = False And _
                            AssetGroups.Contains("Up to 2Years") = True Then
                            upper = 1825
                            lower = 731
                            Exit Select
                        End If
                        If AssetGroups.Contains("Up to 3Years") = False And _
                            AssetGroups.Contains("Up to 2Years") = False And _
                            AssetGroups.Contains("Up to 365Days") = True Then
                            upper = 1825
                            lower = 366
                            Exit Select
                        End If
                        If AssetGroups.Contains("Up to 3Years") = False And _
                            AssetGroups.Contains("Up to 2Years") = False And _
                            AssetGroups.Contains("Up to 365Days") = False And _
                            AssetGroups.Contains("Up to 180Days") = True Then
                            upper = 1825
                            lower = 181
                            Exit Select
                        End If
                        If AssetGroups.Contains("Up to 3Years") = False And _
                            AssetGroups.Contains("Up to 2Years") = False And _
                            AssetGroups.Contains("Up to 365Days") = False And _
                            AssetGroups.Contains("Up to 180Days") = False And _
                            AssetGroups.Contains("Up to 90Days") = True Then
                            upper = 1825
                            lower = 91
                            Exit Select
                        End If
                        If AssetGroups.Contains("Up to 3Years") = False And _
                            AssetGroups.Contains("Up to 2Years") = False And _
                            AssetGroups.Contains("Up to 365Days") = False And _
                            AssetGroups.Contains("Up to 180Days") = False And _
                            AssetGroups.Contains("Up to 90Days") = False Then
                            upper = 1825
                            lower = 1
                            Exit Select
                        End If

                    Case "Above 5Years"
                        If AssetGroups.Contains("Up to 5Years") Then
                            upper = 9999999
                            lower = 1826
                            Exit Select
                        End If


                End Select

                If ccy = "" Then


                    If val = 0 Then
                        strSQL = "select * from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                                 " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                                 " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                                 " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                                 " ACCRUALANALYSISGRP.type='Asset' and parentgroupid=" & drSQLx.Item(1) & ") and " & _
                                 " alldeals.tenor between " & lower & " and " & upper & "  and startdate<='" & _
                                 CDate(DateSystem.Text) & "' and maturitydate>'" & CDate(DateSystem.Text) & "' and customernumber in(" & SelectedCustomers & ")"
                    Else
                        strSQL = "select * from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                               " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                               " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                               " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                               " ACCRUALANALYSISGRP.type='Asset' and parentgroupid=" & drSQLx.Item(1) & ") and " & _
                               " alldeals.tenor between " & lower & " and " & upper & "  and startdate<='" & _
                               CDate(DateSystem.Text) & "' and maturitydate>'" & CDate(DateSystem.Text) & "' "
                    End If

                Else

                    If val = 0 Then
                        strSQL = "select * from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                                 " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                                 " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                                 " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                                 " ACCRUALANALYSISGRP.type='Asset' and parentgroupid=" & drSQLx.Item(1) & ") and " & _
                                 " alldeals.tenor between " & lower & " and " & upper & "  and startdate<='" & _
                                 CDate(DateSystem.Text) & "' and maturitydate>'" & CDate(DateSystem.Text) & "' and customernumber in(" & SelectedCustomers & ") and alldeals.currency = '" & ccy & "'"
                    Else
                        strSQL = "select * from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                               " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                               " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                               " ACCRUALANALYSISGRP.parentgroupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                               " ACCRUALANALYSISGRP.type='Asset' and parentgroupid=" & drSQLx.Item(1) & ") and " & _
                               " alldeals.tenor between " & lower & " and " & upper & "  and startdate<='" & _
                               CDate(DateSystem.Text) & "' and maturitydate>'" & CDate(DateSystem.Text) & "' and alldeals.currency='" & ccy & "' "
                    End If

                End If


                cnSQL = New SqlConnection(Session("ConnectionString"))
                cnSQL.Open()
                cmSQL = New SqlCommand(strSQL, cnSQL)
                drSQL = cmSQL.ExecuteReader()

                Do While drSQL.Read

                    'Variables required in computations for other fields
                    Dim discountRate As Decimal = 0
                    Dim YieldRate As Decimal = 0
                    Dim NominalAmount As Decimal = 0
                    Dim tenor As Integer = 0
                    Dim intdaysbasis As Integer = 0
                    Dim discountAmount As Decimal = 0
                    Dim AmtInvested As Decimal = 0
                    Dim DaysRun As Integer = 0
                    Dim DaysToRun As Integer = 0
                    Dim InterestAccrued As Decimal = 0
                    Dim bookValue As Decimal = 0
                    Dim YTMBookvalue As Decimal = 0
                    Dim MKTValue As Decimal = 0
                    Dim dailyaccrual As Decimal = 0
                    Dim WeekEndAccrual As Decimal = 0
                    '***************************************************

               

                    intdaysbasis = drSQL.Item("intdaysbasis")

                    'Nominal amount
                    'if the deal is discounted the norminal amount is the maturity value
                    'if the deal is on Yield baisis the norminal amount is the deal amount value
                    If Trim(drSQL.Item("entrytype")).Equals("D") = True Then

                        NominalAmount = drSQL.Item("maturityamount")
                    Else

                        NominalAmount = drSQL.Item("dealamount")
                    End If

                   

                    'If discount rate is not in the database then calculate it
                    If Trim(drSQL.Item("discountrate")).Equals("0") = True Then
                        Dim disc As Decimal
                        disc = (drSQL.Item("grossinterest") / drSQL.Item("maturityamount")) * (((drSQL.Item("intdaysbasis") * 100)) / drSQL.Item("tenor"))

                        discountRate = Decimal.Round(disc, 9)
                    Else

                        discountRate = drSQL.Item("discountrate")
                    End If


                    YieldRate = CDec(drSQL.Item("interestrate"))


                    tenor = drSQL.Item("tenor")
                    'Discount Amount
                    '(Nominal amount*Discount Rate *Days)/36500
                    If Trim(drSQL.Item("entrytype")).Equals("D") = True Then
                        discountAmount = (NominalAmount * discountRate * tenor) / (intdaysbasis * 100)
                    Else
                        discountAmount = CDec(drSQL.Item("grossinterest"))
                    End If


                    'Amount Invested
                    'Nominal amount – discount amount
                    'AmtInvested = NominalAmount - discountAmount
                    AmtInvested = drSQL.Item("dealamount")


                    'Days run
                    'System date –Deal start date
                    DaysRun = DateDiff(DateInterval.Day, drSQL.Item("startdate"), CDate(Session("SysDate")))


                    'Days to run
                    'Days – Days run
                    DaysToRun = tenor - DaysRun


                    'Interest Accrued
                    '(Nominal amount * Discount Rate *days run)/36500
                    InterestAccrued = Math.Round((NominalAmount * discountRate * DaysRun) / (intdaysbasis * 100), 2)


                    'Book Value
                    'Interest accrued + amount invested 
                    bookValue = InterestAccrued + AmtInvested


                    'YTM  Book value
                    '(Discount rate/100)/((1-(discount rate *DTR/100))/365)
                    YTMBookvalue = Decimal.Round((discountRate / 100) / ((1 - (discountRate * DaysToRun / 100) / intdaysbasis)), 9)


                    'daily accrual
                    '(Nominal amount *discount rate )/36500
                    If Trim(drSQL.Item("discountrate")).Equals("0") = True Then
                        dailyaccrual = (NominalAmount * YieldRate) / (intdaysbasis * 100)
                    Else
                        dailyaccrual = Math.Round((NominalAmount * discountRate) / (intdaysbasis * 100), 2)
                    End If



                    'Weekend Accrual
                    '(Nominal amount *discount rate*3)/36500
                    If Trim(drSQL.Item("discountrate")).Equals("0") = True Then
                        WeekEndAccrual = Math.Round((NominalAmount * YieldRate * 3) / (intdaysbasis * 100), 2)
                    Else
                        WeekEndAccrual = Math.Round((NominalAmount * discountRate * 3) / (intdaysbasis * 100), 2)
                    End If

                    

                    'Save this Line to the database

                    SaveAccrualDetailsAssets(Trim(drSQL.Item("dealtypedescription").ToString), Trim(ExtractGroupFormat(1)), Trim(drSQL.Item("dealtype")) _
                    , Trim(drSQL.Item("currency")), Trim(drSQL.Item("dealreference")), Trim(drSQL.Item("fullname")), NominalAmount, drSQL.Item("startdate") _
                    , drSQL.Item("maturitydate"), discountRate, YieldRate, tenor, discountAmount, AmtInvested, DaysRun, DaysToRun, InterestAccrued, bookValue _
                    , YTMBookvalue, dailyaccrual, WeekEndAccrual, Trim(Session("username")))

                Loop

                ' Close and Clean up objects
                drSQL.Close()
                cnSQL.Close()
                cmSQL.Dispose()
                cnSQL.Dispose()
            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Loop


        ' Close and Clean up objects
        drSQLy.Close()
        cnSQLy.Close()
        cmSQLy.Dispose()
        cnSQLy.Dispose()

    End Sub


    'Save Assets details to the database
    Private Sub SaveAccrualDetailsAssets(ByVal ProductDescription As String, ByVal Classification As String, ByVal DealCode As String, ByVal Currency As String, _
    ByVal DealReference As String, ByVal CustomerName As String, ByVal NominalAmount As Decimal, ByVal StartDate As String, ByVal MaturityDate As String, ByVal DiscountRate As String, _
    ByVal YieldRate As String, ByVal Tenor As Integer, ByVal DiscountAmount As Decimal, ByVal AmountInvested As Decimal, ByVal DR As Integer, ByVal DTR As Integer, ByVal InterestAccrued As Decimal, _
    ByVal BookValue As Decimal, ByVal YTMBV As String, ByVal DailyAccrual As Decimal, ByVal WeekendAccrual As Decimal, ByVal usr As String)

        Dim cnSQLy As SqlConnection
        Dim cmSQLy As SqlCommand
        Dim drSQLy As SqlDataReader
        Dim strSQLy As String

        Try

            'Determine the groups in the results first so that we will know the group description/Product Description
            strSQLy = "insert ACCRUALSDETAILWORKASSETS values('" & ProductDescription & "','" & Classification & "','" & _
                  DealCode & "', '" & Currency & "','" & DealReference & "','" & CustomerName & "'," & NominalAmount & ",'" & _
                  StartDate & "','" & MaturityDate & "','" & DiscountRate & "','" & YieldRate & "'," & Tenor & "," & _
                  DiscountAmount & "," & AmountInvested & ", " & DR & "," & DTR & "," & InterestAccrued & "," & BookValue & ",'" & _
                  YTMBV & "', " & DailyAccrual & ", " & WeekendAccrual & ", '" & usr & "','" & CDate(DateSystem.Text) & "')"

            cnSQLy = New SqlConnection(Session("ConnectionString"))
            cnSQLy.Open()
            cmSQLy = New SqlCommand(strSQLy, cnSQLy)
            drSQLy = cmSQLy.ExecuteReader()

            ' Close and Clean up objects
            drSQLy.Close()
            cnSQLy.Close()
            cmSQLy.Dispose()
            cnSQLy.Dispose()

        Catch ex As Exception

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "SaveAccrualDetailsAssets", "error")

        End Try

    End Sub


    'Loads details for live deals for liabilities
    Private Sub LoadLiveDealsLiabilities(val As Integer, ccy As String)


        On Error Resume Next
        If ccy = "" Then

            If val = 0 Then


                strSQL = "select * from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                         " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                         " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                         " ACCRUALANALYSISGRP.groupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                         " ACCRUALANALYSISGRP.type='Liability') and startdate<='" & CDate(DateSystem.Text) & "' and maturitydate>'" & _
                         CDate(DateSystem.Text) & "' and customernumber in(" & selectedcustomers & ")"
            Else
                strSQL = "select * from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                       " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                       " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                       " ACCRUALANALYSISGRP.groupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                       " ACCRUALANALYSISGRP.type='Liability') and startdate<='" & CDate(DateSystem.Text) & "' and maturitydate>'" & _
                       CDate(DateSystem.Text) & "' "
            End If
        Else

            If val = 0 Then


                strSQL = "select * from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                         " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                         " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                         " ACCRUALANALYSISGRP.groupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                         " ACCRUALANALYSISGRP.type='Liability') and startdate<='" & CDate(DateSystem.Text) & "' and maturitydate>'" & _
                         CDate(DateSystem.Text) & "' and customernumber in(" & selectedcustomers & ") and alldeals.currency = '" & ccy & "'"
            Else
                strSQL = "select * from alldeals join customer on alldeals.customernumber=customer.customer_number" & _
                       " join dealtypes on alldeals.dealtype=dealtypes.deal_code where alldeals.dealtype" & _
                       " in(select dealcode from ACCRUALANALYSISDEALCODES join ACCRUALANALYSISGRP on" & _
                       " ACCRUALANALYSISGRP.groupid=ACCRUALANALYSISDEALCODES.groupid where " & _
                       " ACCRUALANALYSISGRP.type='Liability') and startdate<='" & CDate(DateSystem.Text) & "' and maturitydate>'" & _
                       CDate(DateSystem.Text) & "' and alldeals.currency = '" & ccy & "' "
            End If

        End If

        cnSQL = New SqlConnection(Session("ConnectionString"))
        cnSQL.Open()
        cmSQL = New SqlCommand(strSQL, cnSQL)
        drSQL = cmSQL.ExecuteReader()




        Do While drSQL.Read



            Dim disc As Decimal = 0
            'Compute discount rate for products without a discount rate
            If Trim(drSQL.Item("discountrate")) = "0" Then
                '(Interest amount /Maturity amount)*(daysbasis*100/tenor)

                disc = (drSQL.Item("grossinterest") / drSQL.Item("maturityamount")) * (((drSQL.Item("intdaysbasis") * 100)) / drSQL.Item("tenor"))
                disc = Decimal.Round(disc, 9)
            Else
                disc = drSQL.Item("discountrate") 'needs computation for deals with no discount rate
            End If


            Dim intAccruedToDate As Decimal = 0
            intAccruedToDate = drSQL.Item("intaccruedtodate") / (1 - (drSQL.Item("taxrate") / 100))



            'daily accrual
            Dim dailyaccrual As Decimal = 0

            dailyaccrual = (drSQL.Item("dealamount") * drSQL.Item("interestrate")) / (drSQL.Item("intdaysbasis") * 100)

            SaveAccrualDetailsLiabilities(Trim(drSQL.Item("dealtypedescription")), Trim(drSQL.Item("dealreference")), Trim(drSQL.Item("fullname")), Format(drSQL.Item("startdate"), "dd-MMM-yyyy"), Format(drSQL.Item("maturitydate"), "dd-MMM-yyyy"), drSQL.Item("dealamount"), Decimal.Round(disc, 9), drSQL.Item("interestrate"), drSQL.Item("tenor"), drSQL.Item("grossinterest"), DateDiff(DateInterval.Day, drSQL.Item("startdate"), CDate(Session("SysDate"))), intAccruedToDate, DateDiff(DateInterval.Day, drSQL.Item("startdate"), CDate(Session("SysDate"))), drSQL.Item("maturityamount"), dailyaccrual, drSQL.Item("currency"), drSQL.Item("dealtype"))
        Loop

        ' Close and Clean up objects
        drSQL.Close()
        cnSQL.Close()
        cmSQL.Dispose()
        cnSQL.Dispose()

        'Catch ex As SqlException
        '    object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "LoadLiveDeals", "error")
        '    '   MsgBox(ex.Message, MsgBoxStyle.Critical, "LoadLiveDeals")
        'End Try

    End Sub

    'Save Accrual Details for liablities into work table
    Private Sub SaveAccrualDetailsLiabilities(ByVal dealDescription As String, ByVal dealReference As String, ByVal CustomerName As String, ByVal startDate As String, maturitydate As String, ByVal AmountInvested As Decimal, ByVal discRate As String, ByVal YTM As String, ByVal tenor As Integer, ByVal interestAmount As Decimal, ByVal DR As Integer, ByVal interestAccrued As Decimal, ByVal DTR As Integer, ByVal maturityValue As Decimal, ByVal DailyAccrual As Decimal, ByVal Curr As String, ByVal dealCode As String)
        Try

            Dim cnSQLa As SqlConnection
            Dim cmSQLa As SqlCommand
            Dim drSQLa As SqlDataReader
            Dim strSQLa As String

            strSQLa = "insert into AccrualsDetailWorkliabilities values('" & dealDescription & "','" & dealReference & "','" & CustomerName & "'" & _
                     ",'" & startDate & "','" & maturitydate & "','" & AmountInvested & "','" & discRate & "','" & YTM & "','" & tenor & "','" & interestAmount & "'" & _
                     "," & DR & " ,'" & interestAccrued & "'," & DTR & ",'" & maturityValue & "','" & DailyAccrual & "','" & Curr & "','" & Trim(Session("username")) & "','" & dealCode & "','" & CDate(DateSystem.Text) & "')"

            cnSQLa = New SqlConnection(Session("ConnectionString"))
            cnSQLa.Open()
            cmSQLa = New SqlCommand(strSQLa, cnSQLa)
            drSQLa = cmSQLa.ExecuteReader()


            ' Close and Clean up objects
            drSQLa.Close()
            cnSQLa.Close()
            cmSQLa.Dispose()
            cnSQLa.Dispose()



        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "SaveAccrualDetailsLiabilities", "error")

        End Try

    End Sub
    'Calculate subtotals for liabilities
    Private Sub LiabilitiesSubtotals()

        Dim dealcode As String = ""
        Dim Currency As String = ""
        Dim SubTotalamountInvested As Decimal = 0
        Dim AvgDiscountRate As Decimal = 0
        Dim AvgYTM As Decimal = 0
        Dim AvgDays As Integer = 0
        Dim subTotalInterestAmount As Decimal = 0
        Dim AvgDR As Decimal = 0
        Dim subTotalInterestAccrued As Decimal = 0
        Dim AvgDTR As Integer = 0
        Dim SubTotalMaturityValue As Decimal = 0
        Dim SubTotalDailyAccrual As Decimal = 0
        Dim IntDaysBasisForCurrency As Integer = 0

        On Error Resume Next


        strSQL = "select dealcode ,sum(amountInvested),sum(interestamount)," & _
                 " sum(interestaccrued),sum(maturityvalue),sum(dailyAccrual),currency" & _
                 " from accrualsdetailworkliabilities where usr='" & Trim(Session("username")) & "' group by dealcode,currency"

        cnSQL = New SqlConnection(Session("ConnectionString"))
        cnSQL.Open()
        cmSQL = New SqlCommand(strSQL, cnSQL)
        drSQL = cmSQL.ExecuteReader()



        Do While drSQL.Read

            dealcode = Trim(drSQL.Item(0))
            Currency = Trim(drSQL.Item(6))
            IntDaysBasisForCurrency = GetDaysBasis(Currency)
            'Sub totals are represented in product groups - group by product only here
            'I am summing up sub total fields here first and initialise the variables
            SubTotalamountInvested = drSQL.Item(1)
            subTotalInterestAmount = drSQL.Item(2)
            subTotalInterestAccrued = drSQL.Item(3)
            SubTotalMaturityValue = drSQL.Item(4)
            SubTotalDailyAccrual = drSQL.Item(5)

            'Calculate the Average Discount Rate
            '(Sum of daily accrual/Sum of maturity value)*36500
            AvgDiscountRate = Math.Round((SubTotalDailyAccrual / SubTotalMaturityValue) * (IntDaysBasisForCurrency * 100), 9)

            'Calculate the Average YTM
            '(Sum of daily accrual/sum of amount invested) *36500
            AvgYTM = Math.Round((SubTotalDailyAccrual / SubTotalamountInvested) * (IntDaysBasisForCurrency * 100), 9)

            'Calculate the average for days
            '(sum of interest amount/sum of amount invested)*(36500/ Average  Yield to Maturity)
            AvgDays = Math.Round((subTotalInterestAmount / SubTotalamountInvested) * ((IntDaysBasisForCurrency * 100) / AvgYTM), 0)

            'Calculate average Days Run (DR)
            '(Sum of Interest accrued/Sum of amount invested)*(36500/Average Yield to maturity)
            AvgDR = Math.Round((subTotalInterestAccrued / SubTotalamountInvested) * ((IntDaysBasisForCurrency * 100) / AvgYTM), 0)

            'Calculate average Days remaining to run DTR
            'Average days – Average DR
            AvgDTR = AvgDays - AvgDR

            
            'Save this record to the database table for summaries
            SaveAccrualSummaryLiabilities(dealcode, Currency, SubTotalamountInvested, AvgDiscountRate, AvgYTM _
            , AvgDays, subTotalInterestAmount, AvgDR, subTotalInterestAccrued, AvgDTR, SubTotalMaturityValue, _
              SubTotalDailyAccrual)



            'reinitialise these variables incase an error resemed
            dealcode = ""
            Currency = ""
            SubTotalamountInvested = 0
            AvgDiscountRate = 0
            AvgYTM = 0
            AvgDays = 0
            subTotalInterestAmount = 0
            AvgDR = 0
            subTotalInterestAccrued = 0
            AvgDTR = 0
            SubTotalMaturityValue = 0
            SubTotalDailyAccrual = 0
            IntDaysBasisForCurrency = 0
        Loop
        ' GetSummaryLiabilities()
        ' Close and Clean up objects
        drSQL.Close()
        cnSQL.Close()
        cmSQL.Dispose()
        cnSQL.Dispose()


        'Catch ex As SqlException
        '    MsgBox(ex.Message, MsgBoxStyle.Critical, "LiabilitiesSubtotals")
        'End Try


    End Sub
  
    Public Sub GetSubTotalAssets()

        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader


        strSQLx = "SELECT * FROM ACCRUALSUMMARYWORKASSETS where Usr='" & Trim(Session("username")) & "' and rundate='" & CDate(DateSystem.Text) & "'"

        cnSQLx = New SqlConnection(Session("ConnectionString"))
        cnSQLx.Open()
        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
        drSQLx = cmSQLx.ExecuteReader()
        GridSubTotalAssets.DataSource = drSQLx
        GridSubTotalAssets.DataBind()

        ' Close and Clean up objects
        drSQLx.Close()
        cnSQLx.Close()
        cmSQLx.Dispose()
        cnSQLx.Dispose()
    End Sub
    'Save the subtotal lines for assets
    Private Sub SaveSubTotalAssets(ByVal ProductDescription As String, ByVal Classification As String, ByVal DealCode As String, ByVal Currency As String, _
    ByVal Nominalamount As Decimal, ByVal AvgDiscountRate As String, ByVal AvgYield As String, ByVal AvgDays As Integer, ByVal Discount As Decimal, _
    ByVal AmountInvested As Decimal, ByVal DaysRun As Integer, ByVal DTR As Integer, ByVal InterestAccrued As Decimal, ByVal Bookvalue As Decimal, _
    ByVal DailyAccrual As Decimal, ByVal WeekendAccrual As Decimal)

        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        Try
            strSQLx = "insert ACCRUALSUMMARYWORKASSETS values ('" & Trim(ProductDescription) & "','" & Trim(Classification) & "','" & _
            DealCode & "', '" & Currency & "', " & Nominalamount & ",'" & AvgDiscountRate & "','" & AvgYield & "'," & AvgDays & "," & _
            Discount & ", " & AmountInvested & ", " & DaysRun & "," & DTR & "," & InterestAccrued & "," & Bookvalue & "," & DailyAccrual & ", " & _
            WeekendAccrual & ",'" & Trim(Session("username")) & "','" & CDate(DateSystem.Text) & "')"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()



        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "SaveSubTotalAssets", "error")


        End Try

    End Sub
    'Summarises Subtotals of Liabilities at top level Liability group
    Private Sub LiabilitiesSummarySubTotals()
        MsgBox("dd")
        Dim Daily As Decimal = 0
        Dim Volume As Decimal = 0
        Dim Averagediscount As String = ""
        Dim AverageYTM As String = ""
        Dim AverageDTR As Integer = 0
        Dim AverageDecayedyield As Decimal = 0
        Dim Mix As Decimal = 0
        Dim MaturityValue As Decimal = 0
        Dim interestAmount As Decimal = 0
        Dim InterestAccruals As Decimal = 0
        Dim IntDaysBasisForCurrency As Integer = 0
        Dim exchangeRate As Double = 0

        On Error Resume Next

        strSQL = "select sum(subtotaldailyaccrual),sum(subtotalamountinvested),(sum(subtotaldailyaccrual)/(sum(subtotalmaturityvalue))*(daysbasis*100))," & _
                " avg(cast((avgytm) as decimal)),avg(avgdtr),sum(subtotalmaturityvalue),sum(subtotalInterestAmount),sum(subtotalInterestAccrued)," & _
                " ACCRUALANALYSISGRP.description,currency,exchange,reciprocal from ACCRUALSUMMARYWORKLIABILITIES join" & _
                " ACCRUALANALYSISDEALCODES on ACCRUALSUMMARYWORKLIABILITIES.dealcode=ACCRUALANALYSISDEALCODES.dealcode" & _
                " join ACCRUALANALYSISGRP on ACCRUALANALYSISDEALCODES.groupid=ACCRUALANALYSISGRP.groupid" & _
                " join currencies on ACCRUALSUMMARYWORKLIABILITIES.currency=currencies.currencycode" & _
                " join exchange_rates on ACCRUALSUMMARYWORKLIABILITIES.currency=exchange_rates.currencycode " & _
                " where ACCRUALANALYSISGRP.type='Liability'  and usr='" & Trim(Session("username")) & "' group by ACCRUALANALYSISGRP.groupid,ACCRUALANALYSISGRP.description," & _
                " currency,daysbasis,exchange,reciprocal"

        cnSQL = New SqlConnection(Session("ConnectionString"))
        cnSQL.Open()
        cmSQL = New SqlCommand(strSQL, cnSQL)
        drSQL = cmSQL.ExecuteReader()

        Do While drSQL.Read

            IntDaysBasisForCurrency = GetDaysBasis(drSQL.Item("Currency"))
            Daily = drSQL.Item(0)
            Volume = drSQL.Item(1)
            Averagediscount = drSQL.Item(2).ToString
            AverageYTM = drSQL.Item(3).ToString
            AverageDTR = drSQL.Item(4)
            MaturityValue = drSQL.Item(5)
            interestAmount = drSQL.Item(6)
            InterestAccruals = drSQL.Item(7)
            exchangeRate = drSQL.Item("exchange")

            If drSQL.Item("reciprocal").ToString = "Y" Then
                exchangeRate = 1 / exchangeRate
            End If

            'Average Decayed yield=Average discount rate/((1-(Average DTR*Average Discount rate)/36500))
            AverageDecayedyield = Averagediscount / ((1 - (AverageDTR * Averagediscount) / (IntDaysBasisForCurrency * 100)))
            'Volume / Grand Total Volume
            Mix = 0


            
            'Save the subtotals to the database
            saveLiabilitiesSummarySubTotals(Trim(drSQL.Item("description").ToString), _
            Trim(drSQL.Item("currency").ToString), Daily, Volume, Averagediscount, _
            AverageYTM, AverageDTR, AverageDecayedyield, MaturityValue, interestAmount, InterestAccruals, exchangeRate)



            'reinitialise these variables incase an error resemed
            Daily = 0
            Volume = 0
            Averagediscount = ""
            AverageYTM = ""
            AverageDTR = 0
            AverageDecayedyield = 0
            Mix = 0
            IntDaysBasisForCurrency = 0
        Loop

        ' Close and Clean up objects
        drSQL.Close()
        cnSQL.Close()
        cmSQL.Dispose()
        cnSQL.Dispose()

        'GetLiabilitiesSummarySubTotals()

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        On Error GoTo err

        Dim cnSQLp As SqlConnection
        Dim cmSQLp As SqlCommand
        Dim drSQLp As SqlDataReader
        Dim strSQLp As String


        'Look Major total lines for assets that are not avalable and fill them up to make 
        '0 total fields available


        'This will look for groups that had values for some currencies for the logged user

        'strSQL = "select  accrualanalysisgrp.description from accrualanalysisgrp join ACCRUALSLIABILITIESSUMMARYSUBTOTALS on " & _
        '         " ACCRUALSLIABILITIESSUMMARYSUBTOTALS.description = accrualanalysisgrp.description" & _
        '         " where accrualanalysisgrp.description in(select [description] from" & _
        '         " ACCRUALSLIABILITIESSUMMARYSUBTOTALS)and parentgroupid=groupid and type='Liability'and " & _
        '         " ACCRUALSLIABILITIESSUMMARYSUBTOTALS.usr='" & Trim(loggeduser) & "'"
        'cnSQL = New SqlConnection(ConnectionString)
        'cnSQL.Open()
        'cmSQL = New SqlCommand(strSQL, cnSQL)
        'drSQL = cmSQL.ExecuteReader()

        'Do While drSQL.Read

        '    'take each group returned above and look for the missing currencies and update the database
        '    strSQLp = "select currencycode from currencies where currencycode not in(select  " & _
        '              " currency from accrualanalysisgrp join ACCRUALSLIABILITIESSUMMARYSUBTOTALS on  " & _
        '              " ACCRUALSLIABILITIESSUMMARYSUBTOTALS.description = accrualanalysisgrp.description" & _
        '              " where accrualanalysisgrp.description in(select [description] from " & _
        '              " ACCRUALSLIABILITIESSUMMARYSUBTOTALS)and parentgroupid=groupid and type='Liability'and " & _
        '              " accrualanalysisgrp.description='" & Trim(drSQL.Item(0)) & "' and " & _
        '              " ACCRUALSLIABILITIESSUMMARYSUBTOTALS.usr='" & Trim(loggeduser) & "')"

        '    cnSQLp = New SqlConnection(ConnectionString)
        '    cnSQLp.Open()
        '    cmSQLp = New SqlCommand(strSQLp, cnSQLp)
        '    drSQLp = cmSQLp.ExecuteReader()

        '    Do While drSQLp.Read
        '        'Save each line for each currency with zeroes through out
        '        saveLiabilitiesSummarySubTotals(Trim(drSQL.Item(0)), Trim(drSQLp.Item(0)), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        '    Loop

        '    ' Close and Clean up objects
        '    drSQLp.Close()
        '    cnSQLp.Close()
        '    cmSQLp.Dispose()
        '    cnSQLp.Dispose()

        'Loop

        '' Close and Clean up objects
        'drSQL.Close()
        'cnSQL.Close()
        'cmSQL.Dispose()
        'cnSQL.Dispose()



        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        'This will look fo groups that did not have values for any currency
        'Get all currencies defined in the system since the report is multi-currency
        'We need to make sure that each group appears for every currency


        'This will return all asset groups that do not have sub total lines at least for any currency
        'strSQL = "select accrualanalysisgrp.description from accrualanalysisgrp where accrualanalysisgrp.description " & _
        '          " not in(select [description] from ACCRUALSLIABILITIESSUMMARYSUBTOTALS where usr='" & Trim(loggeduser) & "') and parentgroupid=groupid and type='Liability'"

        'cnSQL = New SqlConnection(ConnectionString)
        'cnSQL.Open()
        'cmSQL = New SqlCommand(strSQL, cnSQL)
        'drSQL = cmSQL.ExecuteReader()

        'Do While drSQL.Read

        '    strSQLp = "Select currencycode from currencies"
        '    cnSQLp = New SqlConnection(ConnectionString)
        '    cnSQLp.Open()
        '    cmSQLp = New SqlCommand(strSQLp, cnSQLp)
        '    drSQLp = cmSQLp.ExecuteReader()

        '    Do While drSQLp.Read
        '        'Save each line for each currency with zeroes through out
        '        saveLiabilitiesSummarySubTotals(Trim(drSQL.Item(0)), Trim(drSQLp.Item(0)), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        '    Loop

        '    ' Close and Clean up objects
        '    drSQLp.Close()
        '    cnSQLp.Close()
        '    cmSQLp.Dispose()
        '    cnSQLp.Dispose()


        'Loop

        '' Close and Clean up objects
        'drSQL.Close()
        'cnSQL.Close()
        'cmSQL.Dispose()
        'cnSQL.Dispose()

        Exit Sub


err:

        object_userlog.Msg(Err.Description, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & Err.Description & "LiabilitySummarySubTotal", "error")

       
    End Sub

    'Subtotals for assets
    Private Sub AssetsSubTotals()
        Dim ProductDescription As String = ""
        Dim Classification As String = ""
        Dim DealCode As String = ""
        Dim Currency As String = ""
        Dim Nominalamount As Decimal = 0
        Dim AvgDiscountRate As String = "0"
        Dim AvgYield As String = "0"
        Dim AvgDays As Integer = 0
        Dim Discount As Decimal = 0
        Dim AmountInvested As Decimal = 0
        Dim DaysRun As Integer = 0
        Dim DTR As Integer = 0
        Dim InterestAccrued As Decimal = 0
        Dim Bookvalue As Decimal = 0
        Dim DailyAccrual As Decimal = 0
        Dim WeekendAccrual As Decimal = 0
        Dim IntDaysBasisForCurrency As Integer = 0

        On Error Resume Next

        strSQL = "select currency,Classification,dealcode,ProductDescription,sum(Nominalamount),sum(DailyAccrual)," & _
            " sum(AmountInvested),sum(DiscountAmount),sum(InterestAccrued),sum(Bookvalue),sum(DailyAccrual),sum(WeekendAccrual)" & _
            " from ACCRUALSDETAILWORKASSETS where usr='" & Trim(Session("username")) & "' group by dealcode,Classification,ProductDescription,currency"

        cnSQL = New SqlConnection(Session("ConnectionString"))
        cnSQL.Open()
        cmSQL = New SqlCommand(strSQL, cnSQL)
        drSQL = cmSQL.ExecuteReader()

        Do While drSQL.Read


            ProductDescription = Trim(drSQL.Item(3).ToString)
            Classification = Trim(drSQL.Item(1).ToString)
            DealCode = Trim(drSQL.Item(2).ToString)
            Currency = Trim(drSQL.Item(0).ToString)
            IntDaysBasisForCurrency = GetDaysBasis(Currency)

            Nominalamount = drSQL.Item(4)
            DailyAccrual = drSQL.Item(5)
            AmountInvested = drSQL.Item(6)
            Discount = drSQL.Item(7)
            InterestAccrued = drSQL.Item(8)
            Bookvalue = drSQL.Item(9)
            DailyAccrual = drSQL.Item(10)
            WeekendAccrual = drSQL.Item(11)

            'Average Discount rate =(Sum of daily accrual/sum of nominal amount)*365
            AvgDiscountRate = Math.Round((DailyAccrual / Nominalamount) * (IntDaysBasisForCurrency * 100), 9)

            'Average Yield =(Sum of daily accrual/sum of amount invested)*365
            AvgYield = Math.Round((DailyAccrual / AmountInvested) * (IntDaysBasisForCurrency * 100), 9)

            'Average days =(Sum of Discount amount/sum of amount invested)*(365/Average yield)
            AvgDays = (Discount / AmountInvested) * (IntDaysBasisForCurrency / AvgYield)

            'Days Run =(Sum of Interest accrued/sum of Amount invested)* (365/Average yield)
            DaysRun = (InterestAccrued / AmountInvested) * (IntDaysBasisForCurrency / AvgYield)

            'DTR= ((Sum of discount-  sum of interest accrued)/sum of amount invested)*(365/Average yield)
            DTR = ((Discount - InterestAccrued) / AmountInvested) * ((IntDaysBasisForCurrency * 100) / AvgYield)



            'Save the subtotal lines for assets
            SaveSubTotalAssets(ProductDescription, Classification, DealCode, Currency, Nominalamount, AvgDiscountRate _
            , AvgYield, AvgDays, Discount, AmountInvested, DaysRun, DTR, InterestAccrued, Bookvalue, DailyAccrual _
            , WeekendAccrual)


            'reinitialise these variables incase an error resemed
            ProductDescription = ""
            Classification = ""
            DealCode = ""
            Currency = ""
            Nominalamount = 0
            AvgDiscountRate = "0"
            AvgYield = "0"
            AvgDays = 0
            Discount = 0
            AmountInvested = 0
            DaysRun = 0
            DTR = 0
            InterestAccrued = 0
            Bookvalue = 0
            DailyAccrual = 0
            WeekendAccrual = 0
            IntDaysBasisForCurrency = 0

        Loop

        ' Close and Clean up objects
        drSQL.Close()
        cnSQL.Close()
        cmSQL.Dispose()
        cnSQL.Dispose()

        'Catch ex As SqlException
        '    MsgBox(ex.Message, MsgBoxStyle.Critical, "AssetsSubTotals")
        'End Try

    End Sub
    'Summarises Subtotals of assets at top level asset group
    Private Sub AssetsSummarySubTotals()

        Dim Daily As Decimal = 0
        Dim Volume As Decimal = 0
        Dim Averagediscount As String = ""
        Dim AverageYTM As String = ""
        Dim AverageDTR As Integer = 0
        Dim AverageDecayedyield As Decimal = 0
        Dim Mix As Decimal = 0
        Dim MaturityValue As Decimal = 0
        Dim interestAmount As Decimal = 0
        Dim Discount As Decimal = 0
        Dim IntAccrued As Decimal = 0
        Dim IntDaysBasisForCurrency As Integer = 0
        Dim exchangerate As Decimal = 0

        On Error Resume Next

        strSQL = "select sum(dailyaccrual),sum(nominalamount),(sum(dailyaccrual)/sum(nominalamount))*(daysbasis*100)," & _
                 " avg(cast((avgyield) as decimal)),avg(dtr),sum(Nominalamount),sum(AmountInvested),sum(Discount),sum(InterestAccrued)," & _
                 " ACCRUALANALYSISGRP.description,currency,exchange,reciprocal from ACCRUALSUMMARYWORKASSETS join" & _
                 " ACCRUALANALYSISDEALCODES on ACCRUALSUMMARYWORKASSETS.dealcode=ACCRUALANALYSISDEALCODES.dealcode" & _
                 " join ACCRUALANALYSISGRP on ACCRUALANALYSISDEALCODES.groupid=ACCRUALANALYSISGRP.groupid" & _
                 " join currencies on ACCRUALSUMMARYWORKASSETS.currency=currencies.currencycode" & _
                 " join exchange_rates on ACCRUALSUMMARYWORKASSETS.currency=exchange_rates.currencycode" & _
                 " where ACCRUALANALYSISGRP.type='Asset' and usr='" & Trim(Session("username")) & "' group by ACCRUALANALYSISGRP.groupid,ACCRUALANALYSISGRP.description," & _
                 " currency,daysbasis,exchange,reciprocal"

        cnSQL = New SqlConnection(Session("ConnectionString"))
        cnSQL.Open()
        cmSQL = New SqlCommand(strSQL, cnSQL)
        drSQL = cmSQL.ExecuteReader()

        Do While drSQL.Read

            Daily = drSQL.Item(0)
            Volume = drSQL.Item(1)
            Averagediscount = drSQL.Item(2).ToString
            AverageYTM = drSQL.Item(3).ToString
            AverageDTR = drSQL.Item(4)
            MaturityValue = drSQL.Item(5)
            interestAmount = drSQL.Item(6)
            Discount = drSQL.Item(7)
            IntAccrued = drSQL.Item(8)
            IntDaysBasisForCurrency = GetDaysBasis(drSQL.Item("currency"))
            exchangerate = drSQL.Item("exchange")

            If drSQL.Item("reciprocal").ToString = "Y" Then
                exchangerate = 1 / exchangerate
            End If

            'Average Decayed yield=Average discount rate/((1-(Average DTR*Average Discount rate)/36500))
            AverageDecayedyield = Averagediscount / ((1 - (AverageDTR * Averagediscount) / (IntDaysBasisForCurrency * 100)))
            'Volume / Grand Total Volume
            Mix = 0

  

            'save the summary of assets subtotals
            saveSummarySubtotalsAssets(Trim(drSQL.Item("description").ToString), _
            Trim(drSQL.Item("currency").ToString), Daily, Volume, Averagediscount, _
            AverageYTM, AverageDTR, AverageDecayedyield, MaturityValue, interestAmount, Discount, IntAccrued, exchangerate)

            'reinitialise these variables incase an error resemed
            Daily = 0
            Volume = 0
            Averagediscount = ""
            AverageYTM = ""
            AverageDTR = 0
            AverageDecayedyield = 0
            Mix = 0
            MaturityValue = 0
            IntDaysBasisForCurrency = 0
        Loop
        ' Call GetSummarySubtotalsAssets()
        ' Close and Clean up objects
        drSQL.Close()
        cnSQL.Close()
        cmSQL.Dispose()
        cnSQL.Dispose()

        'GetSubTotalAssets()

        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        On Error GoTo err

        Dim cnSQLp As SqlConnection
        Dim cmSQLp As SqlCommand
        Dim drSQLp As SqlDataReader
        Dim strSQLp As String


        'Look Major total lines for assets that are not avalable and fill them up to make 
        '0 total fields available


        'This will look for groups that had values for some currencies for the logged user

        'strSQL = "select  accrualanalysisgrp.description from accrualanalysisgrp join ACCRUALSASSETSSUMMARYSUBTOTALS on " & _
        '         " ACCRUALSASSETSSUMMARYSUBTOTALS.description = accrualanalysisgrp.description" & _
        '         " where accrualanalysisgrp.description in(select [description] from" & _
        '         " ACCRUALSASSETSSUMMARYSUBTOTALS)and parentgroupid=groupid and type='Asset'and " & _
        '         " ACCRUALSASSETSSUMMARYSUBTOTALS.usr='" & Trim(loggeduser) & "'"
        'cnSQL = New SqlConnection(ConnectionString)
        'cnSQL.Open()
        'cmSQL = New SqlCommand(strSQL, cnSQL)
        'drSQL = cmSQL.ExecuteReader()

        'Do While drSQL.Read

        '    'take each group returned above and look for the missing currencies and update the database
        '    strSQLp = "select currencycode from currencies where currencycode not in(select  " & _
        '              " currency from accrualanalysisgrp join ACCRUALSASSETSSUMMARYSUBTOTALS on  " & _
        '              " ACCRUALSASSETSSUMMARYSUBTOTALS.description = accrualanalysisgrp.description" & _
        '              " where accrualanalysisgrp.description in(select [description] from " & _
        '              " ACCRUALSASSETSSUMMARYSUBTOTALS)and parentgroupid=groupid and type='Asset'and " & _
        '              " accrualanalysisgrp.description='" & Trim(drSQL.Item(0)) & "' and " & _
        '              " ACCRUALSASSETSSUMMARYSUBTOTALS.usr='" & Trim(loggeduser) & "')"

        '    cnSQLp = New SqlConnection(ConnectionString)
        '    cnSQLp.Open()
        '    cmSQLp = New SqlCommand(strSQLp, cnSQLp)
        '    drSQLp = cmSQLp.ExecuteReader()

        '    Do While drSQLp.Read
        '        'Save each line for each currency with zeroes through out
        '        saveSummarySubtotalsAssets(Trim(drSQL.Item(0)), Trim(drSQLp.Item(0)), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        '    Loop

        '    ' Close and Clean up objects
        '    drSQLp.Close()
        '    cnSQLp.Close()
        '    cmSQLp.Dispose()
        '    cnSQLp.Dispose()

        'Loop

        '' Close and Clean up objects
        'drSQL.Close()
        'cnSQL.Close()
        'cmSQL.Dispose()
        'cnSQL.Dispose()



        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        'This will look fo groups that did not have values for any currency
        'Get all currencies defined in the system since the report is multi-currency
        'We need to make sure that each group appears for every currency


        'This will return all asset groups that do not have sub total lines at least for any currency
        'strSQL = "select accrualanalysisgrp.description from accrualanalysisgrp where accrualanalysisgrp.description " & _
        '          " not in(select [description] from ACCRUALSASSETSSUMMARYSUBTOTALS where usr='" & Trim(loggeduser) & "') and parentgroupid=groupid and type='Asset'"

        'cnSQL = New SqlConnection(ConnectionString)
        'cnSQL.Open()
        'cmSQL = New SqlCommand(strSQL, cnSQL)
        'drSQL = cmSQL.ExecuteReader()

        'Do While drSQL.Read

        '    strSQLp = "Select currencycode from currencies"
        '    cnSQLp = New SqlConnection(ConnectionString)
        '    cnSQLp.Open()
        '    cmSQLp = New SqlCommand(strSQLp, cnSQLp)
        '    drSQLp = cmSQLp.ExecuteReader()

        '    Do While drSQLp.Read
        '        'Save each line for each currency with zeroes through out
        '        saveSummarySubtotalsAssets(Trim(drSQL.Item(0)), Trim(drSQLp.Item(0)), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        '    Loop

        '    ' Close and Clean up objects
        '    drSQLp.Close()
        '    cnSQLp.Close()
        '    cmSQLp.Dispose()
        '    cnSQLp.Dispose()


        'Loop

        '' Close and Clean up objects
        'drSQL.Close()
        'cnSQL.Close()
        'cmSQL.Dispose()
        'cnSQL.Dispose()

        Exit Sub


err:

        object_userlog.Msg(Err.Description, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & Err.Description & "AssetSummarySubTotal", "error")




    End Sub
    'Saving in database******************************************************************************************************************************
    'Save the subtotals to the database
    Private Sub saveLiabilitiesSummarySubTotals(ByVal description As String, ByVal currency As String, ByVal Daily As Decimal, _
    ByVal Volume As Decimal, ByVal Averagediscount As String, ByVal AverageYTM As String, ByVal AverageDTR As Integer, _
    ByVal AverageDecayedyield As Decimal, ByVal maturityvalue As Decimal, ByVal interestAmount As Decimal, _
    ByVal InterestAccruals As Decimal, ByVal exchangeRate As Decimal)


        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        If Averagediscount = "" Then
            Averagediscount = 0
        End If
        Try
            strSQLx = "insert ACCRUALSLIABILITIESSUMMARYSUBTOTALS values('" & description & "','" & currency & "', " & Daily & "," & _
            Volume & ", " & Math.Round(CDec(Averagediscount), 9) & ", " & Math.Round(CDec(AverageYTM), 9) & ", " & AverageDTR & ", " & _
            AverageDecayedyield & "," & maturityvalue & "," & interestAmount & "," & InterestAccruals & ",'" & Trim(Session("username")) & "'," & exchangeRate & ",'" & currency & "','" & CDate(DateSystem.Text) & "')"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()


            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ex As Exception

            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try


    End Sub

    Private Sub SaveAccrualSummaryLiabilities(ByVal dealcode As String, ByVal Currency As String, ByVal SubTotalamountInvested As Decimal, _
   ByVal AvgDiscountRate As Decimal, ByVal AvgYTM As Decimal, ByVal AvgDays As Integer, ByVal subTotalInterestAmount As Decimal, _
   ByVal AvgDR As Decimal, ByVal subTotalInterestAccrued As Decimal, ByVal AvgDTR As Integer, ByVal SubTotalMaturityValue As Decimal, _
   ByVal SubTotalDailyAccrual As Decimal)

        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        Try

            strSQLx = "insert ACCRUALSUMMARYWORKLIABILITIES values('" & dealcode & "', '" & Currency & "', " & SubTotalamountInvested & ",'" & _
            AvgDiscountRate & "', '" & AvgYTM & "', " & AvgDays & ", " & subTotalInterestAmount & ", " & AvgDR & "," & _
            subTotalInterestAccrued & ", " & AvgDTR & ", " & SubTotalMaturityValue & ", " & SubTotalDailyAccrual & ",'" & Trim(Session("username")) & "','" & CDate(DateSystem.Text) & "')"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()



        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try

    End Sub
    'save the summary of assets subtotals
    Private Sub saveSummarySubtotalsAssets(ByVal description As String, ByVal currency As String, ByVal Daily As Decimal, _
    ByVal Volume As Decimal, ByVal Averagediscount As String, ByVal AverageYTM As String, ByVal AverageDTR As Integer, _
    ByVal AverageDecayedyield As Decimal, ByVal maturityvalue As Decimal, ByVal interestAmount As Decimal, _
    ByVal discount As Decimal, ByVal IntAccrued As Decimal, ByVal exchangerate As Decimal)


        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        If Averagediscount = "" Then
            Averagediscount = 0
        End If

        Try
            strSQLx = "insert ACCRUALSASSETSSUMMARYSUBTOTALS values('" & description & "','" & currency & "', " & Daily & "," & _
            Volume & ", " & Math.Round(CDec(Averagediscount), 9) & ", " & Math.Round(CDec(AverageYTM), 9) & ", " & AverageDTR & ", " & _
            AverageDecayedyield & "," & maturityvalue & "," & interestAmount & "," & discount & "," & IntAccrued & ",'" & Trim(Session("username")) & "'," & exchangerate & ",'" & currency & "','" & CDate(DateSystem.Text) & "')"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()


            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ex As Exception
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try
    End Sub
    'end Saving in database******************************************************************************************************************************

  
    Public Sub GetSummarySubtotalsAssets()

        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        '   ACCRUALSASSETSSUMMARYSUBTOTALS()
        strSQLx = "SELECT * FROM ACCRUALSASSETSSUMMARYSUBTOTALS where Usr='" & Trim(Session("username")) & "' and rundate='" & CDate(DateSystem.Text) & "'"

        cnSQLx = New SqlConnection(Session("ConnectionString"))
        cnSQLx.Open()
        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
        drSQLx = cmSQLx.ExecuteReader()
        GridSummarySubtotalsAssets.DataSource = drSQLx
        GridSummarySubtotalsAssets.DataBind()

        ' Close and Clean up objects
        drSQLx.Close()
        cnSQLx.Close()
        cmSQLx.Dispose()
        cnSQLx.Dispose()
    End Sub
    Public Sub GetLiabilitiesSummarySubTotals()

        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader

        strSQLx = "SELECT * FROM ACCRUALSUMMARYWORKLIABILITIES where Usr='" & Trim(Session("username")) & "' and rundate='" & CDate(DateSystem.Text) & "'"

        cnSQLx = New SqlConnection(Session("ConnectionString"))
        cnSQLx.Open()
        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
        drSQLx = cmSQLx.ExecuteReader()
        GridLiabilitiesSummary.DataSource = drSQLx
        GridLiabilitiesSummary.DataBind()

        ' Close and Clean up objects
        drSQLx.Close()
        cnSQLx.Close()
        cmSQLx.Dispose()
        cnSQLx.Dispose()
    End Sub
    Public Sub GetSummaryLiabilities()

        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader

        strSQLx = "SELECT * FROM ACCRUALSLIABILITIESSUMMARYSUBTOTALS where Usr='" & Trim(Session("username")) & "' and rundate='" & CDate(DateSystem.Text) & "'"

        cnSQLx = New SqlConnection(Session("ConnectionString"))
        cnSQLx.Open()
        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
        drSQLx = cmSQLx.ExecuteReader()
        GridSubTotals.DataSource = drSQLx
        GridSubTotals.DataBind()

        ' Close and Clean up objects
        drSQLx.Close()
        cnSQLx.Close()
        cmSQLx.Dispose()
        cnSQLx.Dispose()
    End Sub
    Public Sub GetLoadLiveDealsLiabilities()

        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader

        strSQLx = "SELECT * FROM AccrualsDetailWorkliabilities where Usr='" & Trim(Session("username")) & "' and rundate='" & CDate(DateSystem.Text) & "'"

        cnSQLx = New SqlConnection(Session("ConnectionString"))
        cnSQLx.Open()
        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
        drSQLx = cmSQLx.ExecuteReader()
        GridAccrualsDetailWorkliabilities.DataSource = drSQLx
        GridAccrualsDetailWorkliabilities.DataBind()

        ' Close and Clean up objects
        drSQLx.Close()
        cnSQLx.Close()
        cmSQLx.Dispose()
        cnSQLx.Dispose()
    End Sub
    Public Sub GetLoadLiveDealsAssets()

        Dim cnSQLx As SqlConnection
        Dim strSQLx As String
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader

        strSQLx = "SELECT * FROM AccrualsDetailWorkAssets where Usr='" & Trim(Session("username")) & "' and rundate='" & CDate(DateSystem.Text) & "'"

        cnSQLx = New SqlConnection(Session("ConnectionString"))
        cnSQLx.Open()
        cmSQLx = New SqlCommand(strSQLx, cnSQLx)
        drSQLx = cmSQLx.ExecuteReader()
        GridAccrualsDetailWorkAssets.DataSource = drSQLx
        GridAccrualsDetailWorkAssets.DataBind()

        ' Close and Clean up objects
        drSQLx.Close()
        cnSQLx.Close()
        cmSQLx.Dispose()
        cnSQLx.Dispose()
    End Sub
    Private Function GetDaysBasis(ByVal curr As String) As String

        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String
        Dim x As String
        Try

            strSQLx = "select daysbasis from currencies where currencycode='" & Trim(curr) & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                x = Trim(drSQLx.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

            If x = "" Then x = 0

            Return x

        Catch ex As Exception

        End Try
    End Function
    Private Sub ClearWorkTables()
        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        Try

            strSQLx = "begin tran X " & _
                      " Delete AccrualsDetailWorkliabilities where usr='" & Trim(Session("username")) & "'" & _
                      " Delete ACCRUALSUMMARYWORKLIABILITIES where usr='" & Trim(Session("username")) & "'" & _
                      " Delete ACCRUALSDETAILWORKASSETS where usr='" & Trim(Session("username")) & "'" & _
                      " delete ACCRUALSUMMARYWORKASSETS where usr='" & Trim(Session("username")) & "'" & _
                      " delete ACCRUALSDETAILWORKASSETS where usr='" & Trim(Session("username")) & "' " & _
                      "  delete ACCRUALSASSETSSUMMARYSUBTOTALS where usr='" & Trim(Session("username")) & "'" & _
                      " Delete ACCRUALSLIABILITIESSUMMARYSUBTOTALS where usr='" & Trim(Session("username")) & "'" & _
                      " delete ACCRUALSASSETSSUMMARYSUBTOTALS where usr='" & Trim(Session("username")) & "'" & _
                      " delete ACCRUALSANALYSISNET where usr='" & Trim(Session("username")) & "'" & _
                      " commit tran X"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "ClearWorkTables", "error")


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
            MsgBox(xr.Message, MsgBoxStyle.Critical)
            'Log the event *****************************************************
            object_userlog.Msg(xr.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & xr.Message & "ClearWorkTables", "error")
            '************************END****************************************
        End Try
    End Sub
    Private Function getProductDescription(ByVal code As String) As String
        Dim x As String = ""
        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        Try
            strSQLx = "select dealtypedescription from dealtypes where deal_code='" & code & "'"
            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            Do While drSQLx.Read
                x = Trim(drSQLx.Item(0).ToString)
            Loop

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()



        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message, "error")

        End Try

        Return x

    End Function
    Private Sub clearNetTotals()
        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String

        Try

            strSQLx = "begin tran X " & _
                     " delete ACCRUALSANALYSISNET where usr='" & Trim(Session("username")) & "'" & _
                      " commit tran X"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()

        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "ClearNetTables", "error")

        End Try
    End Sub
    Private Sub ConvertSummaryToBase()
        Dim cnSQLx As SqlConnection
        Dim cmSQLx As SqlCommand
        Dim drSQLx As SqlDataReader
        Dim strSQLx As String


        Try
            'ACCRUALSLIABILITIESSUMMARYSUBTOTALS,ACCRUALSASSETSSUMMARYSUBTOTALS
            strSQLx = " begin tran Z " & _
                      " update ACCRUALSLIABILITIESSUMMARYSUBTOTALS set dailysubtotal=dailysubtotal/exchangeRate,VolumeSubtotal=VolumeSubtotal/exchangeRate where usr='" & Trim(Session("username")) & "'" & _
                      " update ACCRUALSASSETSSUMMARYSUBTOTALS set dailysubtotal=dailysubtotal/exchangeRate,VolumeSubtotal=VolumeSubtotal/exchangeRate,amountinvested=amountinvested/exchangeRate where usr='" & Trim(Session("username")) & "'" & _
                      " update ACCRUALSLIABILITIESSUMMARYSUBTOTALS set currency='USD' where usr='" & Trim(Session("username")) & "'" & _
                      " update ACCRUALSASSETSSUMMARYSUBTOTALS set currency='USD' where usr='" & Trim(Session("username")) & "'" & _
                      " commit tran Z"

            cnSQLx = New SqlConnection(Session("ConnectionString"))
            cnSQLx.Open()
            cmSQLx = New SqlCommand(strSQLx, cnSQLx)
            drSQLx = cmSQLx.ExecuteReader()

            ' Close and Clean up objects
            drSQLx.Close()
            cnSQLx.Close()
            cmSQLx.Dispose()
            cnSQLx.Dispose()



        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "ConvertSummaryToBase", "error")


        End Try
    End Sub
 

    Protected Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        Call ClearWorkTables()

        'Liabilities Procedure**********************************
        If cmbCurrency.Text = "Consolidated" Then
            Call LoadLiveDealsLiabilities(formType, "")
        Else
            Call LoadLiveDealsLiabilities(formType, Trim(cmbCurrency.Text))
        End If


        Call LiabilitiesSubtotals()
        Call LiabilitiesSummarySubTotals()
        '_______________________________________________________

        'Assets Procedure***************************************
        If Trim(cmbCurrency.Text) = "Consolidated" Then
            Call LoadLiveDealsAssets(formType, "")
        Else
            Call LoadLiveDealsAssets(formType, Trim(cmbCurrency.Text))
        End If

        Call AssetsSubTotals()
        Call AssetsSummarySubTotals()
        '_______________________________________________________
        Call GetLoadLiveDealsLiabilities()
        Call GetLoadLiveDealsAssets()
        Call GetLiabilitiesSummarySubTotals()
         Call GetSubTotalAssets()
        Call GetSummaryLiabilities()
        Call GetSummarySubtotalsAssets()
        btnPrint.Enabled = True

    End Sub
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim mydate As String
        Dim currency As String
        Dim status As String
        If cmbCurrency.Text = "" Then
            MsgBox("select the currency to print", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        ''Consolidation of data at EOD will distort the saved data at this time so prevent this report from running
        'If RadioSystem.Checked = True And Trim(cmbCurrency.Text) = "Consolidated" Then
        '    MsgBox("Option not available at this time", MsgBoxStyle.Information)
        '    Exit Sub
        'End If


        'Calculate the Net Totals
        Call clearNetTotals()

        If Trim(cmbCurrency.Text) = "Consolidated" Then
            Call ConvertSummaryToBase()
            btnPrint.Enabled = False

            Call NetTotals("USD")
        Else

            Call NetTotals(Trim(cmbCurrency.Text))
        End If

        ' reportview.ReportName = "Accruals Analysis.rpt"
        ' mydate = Format(DateSystem.Text, "yyyy-MM-dd
        Dim dt As Date = DateSystem.Text
        mydate = Format(dt, "yyyy-MM-dd")

        If Trim(cmbCurrency.Text) = "Consolidated" Then
            currency = "USD"
            status = "Consolidated"
        Else
            currency = Trim(cmbCurrency.Text)
            status = ""
        End If

        Response.Redirect("ReportViewer.aspx?report=Accruals Analysis&date=" & mydate & "&currency=" & currency)




    End Sub
    Private Sub NetTotals(ByVal curr As String)
        Dim DailySubTotalLiabilities As Decimal = 0
        Dim VolumeSubTotalLiabilities As Decimal = 0
        Dim VolumeSubTotalAssets As Decimal = 0
        Dim DailySubTotalAssets As Decimal = 0
        Dim MaturityValueLiabilities As Decimal = 0
        Dim AverageDiscountLiability As Decimal = 0
        Dim AverageDiscountAssets As Decimal = 0
        Dim NominalSubTotalAssets As Decimal = 0
        Dim InterestAmountLiabilities As Decimal = 0
        Dim AverageYTMLiabilities As Decimal = 0
        Dim AverageDTRLiabilities As Integer = 0
        Dim AverageDTRAssets As Integer
        Dim AverageYTMAssets As Decimal = 0
        Dim AmountInvestedAssets As Decimal = 0
        Dim DiscountAmountAssets As Decimal = 0
        Dim InterestAccruedAssets As Decimal = 0
        Dim InterestAccruals As Decimal = 0
        Dim AverageDecayAssets As Decimal = 0
        Dim AverageDecayLiabilities As Decimal = 0

        Dim NetDaily As Decimal = 0
        Dim NetVolume As Decimal = 0
        Dim NetAveragedDiscount As Decimal = 0
        Dim NetAverageDTR As Integer = 0
        Dim NetAverageDecayedYield As Decimal = 0

        Dim IntDaysBasisForCurrency As Integer = 0

        'Liability section
        Try
            strSQL = "select sum(dailysubtotal),sum(VolumeSubtotal),sum(MaturityValue),sum(interestamount),sum(interestaccruals) from ACCRUALSLIABILITIESSUMMARYSUBTOTALS join" & _
                     " ACCRUALANALYSISGRP on ACCRUALANALYSISGRP.description=ACCRUALSLIABILITIESSUMMARYSUBTOTALS.description" & _
                     " where ACCRUALANALYSISGRP.type='Liability' and ACCRUALSLIABILITIESSUMMARYSUBTOTALS.usr='" & Trim(Session("username")) & "'" & _
                     " and ACCRUALSLIABILITIESSUMMARYSUBTOTALS.currency='" & Trim(curr) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                If drSQL.Item(0) Is DBNull.Value Then
                    DailySubTotalLiabilities = 0
                Else
                    DailySubTotalLiabilities = drSQL.Item(0)
                End If

                If drSQL.Item(1) Is DBNull.Value Then
                    VolumeSubTotalLiabilities = 0
                Else
                    VolumeSubTotalLiabilities = drSQL.Item(1)
                End If

                If drSQL.Item(2) Is DBNull.Value Then
                    MaturityValueLiabilities = 0
                Else
                    MaturityValueLiabilities = drSQL.Item(2)
                End If

                If drSQL.Item(3) Is DBNull.Value Then
                    InterestAmountLiabilities = 0
                Else
                    InterestAmountLiabilities = drSQL.Item(3)
                End If

                If drSQL.Item(4) Is DBNull.Value Then
                    InterestAccruals = 0
                Else
                    InterestAccruals = drSQL.Item(4)
                End If

                IntDaysBasisForCurrency = GetDaysBasis(curr)
            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "NetTotals Assets section", "error")

            Exit Sub
        End Try

        'Calculate AverageDiscountLiability for computation of the net figure
        If DailySubTotalLiabilities = 0 Or MaturityValueLiabilities = 0 Then
            AverageDiscountLiability = 0
        Else
            AverageDiscountLiability = (DailySubTotalLiabilities / MaturityValueLiabilities) * (IntDaysBasisForCurrency * 100)
        End If

        'Calculate averageYTM for liabilities for computation of the averageDTR figure
        If DailySubTotalLiabilities = 0 Or VolumeSubTotalLiabilities = 0 Then
            AverageYTMLiabilities = 0
        Else
            AverageYTMLiabilities = (DailySubTotalLiabilities / VolumeSubTotalLiabilities) * IntDaysBasisForCurrency
        End If


        'Calculate averageDTR for liabilities for computation of the net figure
        If InterestAmountLiabilities = 0 Or VolumeSubTotalLiabilities = 0 Or AverageYTMLiabilities = 0 Or DailySubTotalLiabilities = 0 Or VolumeSubTotalLiabilities = 0 Or InterestAccruals = 0 Or IntDaysBasisForCurrency = 0 Then
            AverageDTRLiabilities = 0
        Else
            AverageDTRLiabilities = ((InterestAmountLiabilities / VolumeSubTotalLiabilities) * (IntDaysBasisForCurrency / AverageYTMLiabilities)) - ((InterestAccruals / VolumeSubTotalLiabilities) * (IntDaysBasisForCurrency / AverageYTMLiabilities))
        End If

        'Calculate average decay yield for liabilities for computation of the net figure
        If (1 - (AverageDTRLiabilities * AverageDiscountLiability)) <> 0 Then
            If IntDaysBasisForCurrency <> 0 Then
                AverageDecayLiabilities = AverageDiscountLiability / ((1 - (AverageDTRLiabilities * AverageDiscountLiability) / (IntDaysBasisForCurrency * 100)))
            Else
                AverageDecayLiabilities = 0
            End If
        Else
            AverageDecayLiabilities = 0
        End If

        'Asset section
        Try
            strSQL = "select sum(dailysubtotal),sum(VolumeSubtotal),sum(nominalamount),sum(amountInvested),sum(discount),sum(interestaccrued) from" & _
                     " ACCRUALSASSETSSUMMARYSUBTOTALS join" & _
                     " ACCRUALANALYSISGRP on ACCRUALANALYSISGRP.description=ACCRUALSASSETSSUMMARYSUBTOTALS.description" & _
                     " where ACCRUALANALYSISGRP.type='Asset'and ACCRUALSASSETSSUMMARYSUBTOTALS.usr='" & Trim(Session("username")) & "'" & _
                     " and ACCRUALSASSETSSUMMARYSUBTOTALS.currency='" & Trim(curr) & "'"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            Do While drSQL.Read
                If drSQL.Item(0) Is DBNull.Value Then
                    DailySubTotalAssets = 0
                Else
                    DailySubTotalAssets = drSQL.Item(0)
                End If
                If drSQL.Item(1) Is DBNull.Value Then
                    VolumeSubTotalAssets = 0
                Else
                    VolumeSubTotalAssets = drSQL.Item(1)
                End If
                If drSQL.Item(2) Is DBNull.Value Then
                    NominalSubTotalAssets = 0
                Else
                    NominalSubTotalAssets = drSQL.Item(2)
                End If
                If drSQL.Item(3) Is DBNull.Value Then
                    AmountInvestedAssets = 0
                Else
                    AmountInvestedAssets = drSQL.Item(3)
                End If

                If drSQL.Item(4) Is DBNull.Value Then
                    DiscountAmountAssets = 0
                Else
                    DiscountAmountAssets = drSQL.Item(4)
                End If

                If drSQL.Item(5) Is DBNull.Value Then
                    InterestAccruedAssets = 0
                Else
                    InterestAccruedAssets = drSQL.Item(5)
                End If

                IntDaysBasisForCurrency = GetDaysBasis(curr)

            Loop

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "NetTotals Assets section", "error")

            '   MsgBox(ex.Message, MsgBoxStyle.Critical, "NetTotals Assets section")
        End Try


        'Calculate AverageDiscountLiability for computation of the net figure
        If DailySubTotalAssets = 0 Or VolumeSubTotalAssets = 0 Then
            AverageDiscountAssets = 0
        Else
            AverageDiscountAssets = (DailySubTotalAssets / VolumeSubTotalAssets) * (IntDaysBasisForCurrency * 100)
        End If

        'Calculate averageYTM for Assets for computation of the averageDTR figure
        If AmountInvestedAssets = 0 Or DailySubTotalAssets = 0 Then
            AverageYTMAssets = 0
        Else
            AverageYTMAssets = (DailySubTotalAssets / AmountInvestedAssets) * IntDaysBasisForCurrency
        End If

        'Calculate averageDTR for Assets for computation of the net figure
        If (DiscountAmountAssets - InterestAccruedAssets) = 0 Or AmountInvestedAssets = 0 Or AverageYTMAssets = 0 Or IntDaysBasisForCurrency = 0 Then
            AverageDTRAssets = 0
        Else
            AverageDTRAssets = ((DiscountAmountAssets - InterestAccruedAssets) / AmountInvestedAssets) * (IntDaysBasisForCurrency / AverageYTMAssets)
        End If

        'Calculate average decay yield for assets for computation of the net figure
        If (1 - (AverageDTRAssets * AverageDiscountAssets)) <> 0 Then
            If IntDaysBasisForCurrency <> 0 Then
                AverageDecayAssets = AverageDiscountAssets / ((1 - (AverageDTRAssets * AverageDiscountAssets) / (IntDaysBasisForCurrency * 100)))
            Else
                AverageDecayAssets = 0
            End If
        Else
            AverageDecayAssets = 0
        End If
        '**********************************************************************


        NetDaily = (DailySubTotalLiabilities * -1) + DailySubTotalAssets
        NetVolume = VolumeSubTotalAssets - VolumeSubTotalLiabilities
        NetAveragedDiscount = AverageDiscountAssets - AverageDiscountLiability
        NetAverageDTR = AverageDTRLiabilities - AverageDTRAssets


        NetAverageDecayedYield = AverageDecayAssets - AverageDecayLiabilities


        'Save the Net summary
        Try
            strSQL = "insert ACCRUALSANALYSISNET values(" & NetDaily & "," & NetVolume & "," & NetAveragedDiscount & "," & NetAverageDTR & "," & _
                                                         NetAverageDecayedYield & " ,'" & curr & "','" & Trim(Session("username")) & "','" & CDate(DateSystem.Text) & "')"

            cnSQL = New SqlConnection(Session("ConnectionString"))
            cnSQL.Open()
            cmSQL = New SqlCommand(strSQL, cnSQL)
            drSQL = cmSQL.ExecuteReader()

            ' Close and Clean up objects
            drSQL.Close()
            cnSQL.Close()
            cmSQL.Dispose()
            cnSQL.Dispose()

        Catch ex As SqlException
            object_userlog.Msg(ex.Message, True, "#", Session("loggedUserLog") & "ERR001" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ex.Message & "ave the Net summary", "error")

        End Try


    End Sub
End Class
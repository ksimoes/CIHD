Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class FinInd

    Public Function FormatCurrency(val As Object) As String
        Dim dec As Decimal
        If Decimal.TryParse(val.ToString().Replace("$", "").Replace(",", ""), dec) Then
            Return dec.ToString("C2") ' $1,234.56
        End If
        Return "N/A"
    End Function





    Public strCMSnum As String
    Dim foundFinHosp As HospitalContext
    Public Async Function ShowFinancialDataApi(cmsNum As String) As Task

        Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?keyword=" & Uri.EscapeDataString(cmsNum) & "&size=1000"
        Dim decDollarAmount As Decimal
        Dim provider As Object = Nothing
        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)
                provider = data.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = cmsNum)
                If provider Is Nothing Then provider = data(0)
            End If
        End Using




        ' Find the exact match for Provider CCN if possible




        'Fiscal Year End Date
        lblPedFinIndResult.Text = If(provider("Fiscal Year End Date") IsNot Nothing, CDate(provider("Fiscal Year End Date")).ToString("MM/dd/yyyy"), "N/A")

        'Total Current Assets
        lblTotCurrentAssetsResult.Text = FormatCurrency((provider("Total Current Assets")))
        lblTotCurrentAssetsResult2.Text = lblTotCurrentAssetsResult.Text

        'Total Assets
        lblTotAssetsResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Assets")))
        lblTotAssetsResult2.Text = lblTotAssetsResult.Text
        lblTotAssetsResult3.Text = lblTotAssetsResult.Text
        lblTotAssetsResult4.Text = lblTotAssetsResult.Text
        lblTotAssetsResult5.Text = lblTotAssetsResult.Text

        'Total Operating Revenue
        lblTotOperatingRevFinIndResult.Text = FormatCurrency(Search.CleanMeUp(provider("Net Patient Revenue")))
        lblTotOperatingRevenueFinInd2.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult3.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult4.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult5.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult6.Text = lblTotOperatingRevFinIndResult.Text

        'Total Inventory
        lblInventoryResult.Text = FormatCurrency(Search.CleanMeUp(provider("Inventory")))
        lblInventoryResult2.Text = lblInventoryResult.Text

        'Accounts Recievable
        lblAccountsRecievableResult.Text = FormatCurrency(Search.CleanMeUp(provider("Accounts Receivable")))
        lblAccountsRecievableResult2.Text = lblAccountsRecievableResult.Text

        'Other Expenses
        lblOtherExpenseResultFindInd.Text = FormatCurrency(Search.CleanMeUp(provider("Total Other Expenses")))

        'Total Long Term Liabilities
        lblTotLongTermLiabilitiesResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Long Term Liabilities")))

        'Lease Cost
        lblLeaseCostResult.Text = FormatCurrency(Search.CleanMeUp(provider("Leasehold Improvements")))

        'Notes Receivable
        lblNotesReceivableRes.Text = FormatCurrency(Search.CleanMeUp(provider("Notes Receivable")))

        'Total Liabilities
        lblTotLiabilitiesResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Liabilities")))
        lblTotLiabilitiesResult2.Text = lblTotLiabilitiesResult.Text
        lblTotLiabilitiesResult3.Text = lblTotLiabilitiesResult.Text

        'Cash on Hand
        lblCashonHandResult.Text = FormatCurrency((provider("Cash on Hand and in Banks")))
        lblCashonHandResult2.Text = lblCashonHandResult.Text

        'Total Current Liabilities
        lblTotCurrentLiabilitiesResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Current Liabilities")))
        lblTotCurrentLiabilitesResult2.Text = lblTotCurrentLiabilitiesResult.Text
        lblTotCurrentLiabilitesResult3.Text = lblTotCurrentLiabilitiesResult.Text


        'Total Operating Expense
        lblTotOperatingExpenseFinIndResult.Text = FormatCurrency(Search.CleanMeUp(provider("Less Total Operating Expense")))
        lblTotOperatingExpenseFinInd2.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd3.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd4.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd5.Text = lblTotOperatingExpenseFinIndResult.Text

        'Net Income
        lblNetIncomeResult.Text = FormatCurrency(Search.CleanMeUp(provider("Net Income")))
        lblNetIncomeFinIndResult.Text = lblNetIncomeResult.Text
        lblNetIncomeResult2.Text = lblNetIncomeResult.Text
        ' lblInterestExpenseFinIndResult.Text = Search.CleanMeUp(provider("Interest Expense"))







        lblInterestExpenseFinIndResult.Text = "Result"
        lblDepAmortExpenseResult.Text = "Result"


        'EBITDAR (earnings before interest, taxes, depreciation, amortization, and rent)
        lblEbitResult.Text = FormatCurrency(CDec(Search.CleanMeUp(lblNetIncomeFinIndResult.Text, True)) + CDec(Search.CleanMeUp(lblInterestExpenseFinIndResult.Text, True)) + CDec(Search.CleanMeUp(lblDepAmortExpenseResult.Text, True)) + CDec(Search.CleanMeUp(lblLeaseCostResult.Text, True))).ToString
        'Operating Margin
        lblOperatingMarginFinIndResult.Text = ((CDec(Search.CleanMeUp(lblTotOperatingRevFinIndResult.Text, True)) - CDec(Search.CleanMeUp(lblTotOperatingExpenseFinIndResult.Text, True))) / ((CDec(Search.CleanMeUp(lblTotOperatingRevFinIndResult.Text, True))) * 100) * 100).ToString("N2") & "%"
        '''Excess Margin (need non operating revenue)
        '(need non operating rev) lblExcessMarginResult.Text = (CDec(Search.cleanMeUp(lblTotOperatingRevFinIndResult.Text, True)) - CDec(Search.cleanMeUp(lblTotOperatingExpenseFinIndResult.Text, True)) - CDec(Search.cleanMeUp(lblOtherExpenseResultFindInd.Text, True))) / CDec(Search.cleanMeUp(lblTotOperatingRevFinIndResult.Text, True)) * 100)))

        'ROE (Return on Equity)
        lblROEResult.Text = FormatCurrency((CDec(Search.CleanMeUp(lblNetIncomeFinIndResult.Text, True)) / ((CDec(Search.CleanMeUp(lblTotAssetsResult.Text, True)) - (CDec(Search.CleanMeUp(lblTotLiabilitiesResult.Text, True)))))) * 100).ToString("N2") & "%"
        'ROA (Return on Assets)
        lblRoaResult.Text = FormatCurrency((CDec(Search.CleanMeUp(lblNetIncomeFinIndResult.Text, True)) / CDec(Search.CleanMeUp(lblTotAssetsResult.Text, True))) * 100).ToString("N2") & "%"
        'Current Ratio
        lblCurrentRatioResult.Text = FormatCurrency((CDec(Search.CleanMeUp(lblTotCurrentAssetsResult.Text, True)) / CDec(Search.CleanMeUp(lblTotCurrentLiabilitiesResult.Text, True)))).ToString("N2")
        ''Quick Ratio
        lblQuickRatioResult.Text = FormatCurrency((CDec(Search.CleanMeUp(lblTotCurrentAssetsResult.Text, True)) - CDec(Search.CleanMeUp(lblInventoryResult.Text, True))) / CDec(Search.CleanMeUp(lblTotCurrentLiabilitiesResult.Text, True))).ToString("N2")
        'Long Term Debt to Net Assets Ratio
        lblLtdtnaResult.Text = FormatCurrency(((CDec(Search.CleanMeUp(lblTotLongTermLiabilitiesResult.Text, True)) / (CDec(Search.CleanMeUp(lblTotAssetsResult.Text, True) - CDec(Search.CleanMeUp(lblTotLiabilitiesResult.Text)))).ToString("N2")))).ToString("N2")
        'Total Debt to Net Assets Ratio
        lblTdtna.Text = FormatCurrency(((CDec(Search.CleanMeUp(lblTotLiabilitiesResult.Text, True)) / (CDec(Search.CleanMeUp(lblTotAssetsResult.Text, True) - CDec(Search.CleanMeUp(lblTotLiabilitiesResult.Text))).ToString("N2"))))).ToString("N")

        'Depreciation Expense
        lblDepreciationExpenseResultFinInd.Text = FormatCurrency(Search.CleanMeUp(provider("Depreciation Cost")))
        lblDepreciationExpenseResultFinInd2.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd3.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd4.Text = lblDepreciationExpenseResultFinInd.Text

        'Salary Expense
        lblSalaryExpenseResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Salaries (adjusted)")))
        'comtract labor
        lblContractLaborResult.Text = FormatCurrency(If(provider("Contract Labor:Direct Patient Care") IsNot Nothing, CDec(provider("Contract Labor:Direct Patient Care")).ToString("N"), "N/A"))
        'Allowances for Uncollectible Notes and Accounts Receivable
        lblAllowforUncollectRes.Text = FormatCurrency(Search.CleanMeUp(provider("Less: Allowances for Uncollectible Notes and Accounts Receivable")))
        lblAllowforUncollectRes2.Text = lblAllowforUncollectRes.Text

        'Market Securities
        lblMarketSecuritiesResult.Text = FormatCurrency(Search.CleanMeUp(provider("Temporary Investments")))
        lblMarketSecuritiesResult2.Text = lblMarketSecuritiesResult.Text

        'investments
        lblInvestmentsResultFinInd.Text = FormatCurrency(Search.CleanMeUp(provider("Investments")))

        'Depreciation and Amortization Expense
        lblDepAmortExpenseResult.Text = (Search.CleanMeUp(provider("Depreciation Cost")))
        'Depreciciation Expense
        lblDepreciationExpenseResultFinInd.Text = FormatCurrency(Search.CleanMeUp(provider("Depreciation Cost")))
        lblDepreciationExpenseResultFinInd2.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd3.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd4.Text = lblDepreciationExpenseResultFinInd.Text

        'Days Cash on Hand
        lblDaysCOH.Text = FormatCurrency((CDec(lblCashonHandResult.Text) + CDec(Search.CleanMeUp(lblMarketSecuritiesResult.Text, True))) / ((CDec(Search.CleanMeUp(lblTotOperatingExpenseFinIndResult.Text, True) - CDec(Search.CleanMeUp(lblDepreciationExpenseResultFinInd.Text) / 365)))).ToString("N2")).ToString("N2")






    End Function










    Private Sub finind_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = New Size(900, 1500) ' or whatever you want
        Me.MaximumSize = New Size(0, 0) ' unlimited

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Hide()
        Profile.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Hide()
        Financial.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        Hide()
        Quality.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        Hide()
        Inpatient.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)
        Hide()
        Outpatient.Show()
    End Sub

    Private Sub Label113_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        Hide()
        Search.Show()
    End Sub

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub btnProfileFinInd_Click(sender As Object, e As EventArgs) Handles btnProfileFinInd.Click
        Hide()
        Profile.Show()
    End Sub

    Private Sub btnDepartmentsFinInd_Click(sender As Object, e As EventArgs) Handles btnDepartmentsFinInd.Click
        Hide()
        Departments.Show()
    End Sub

    Private Sub btnFinancialFinInd_Click(sender As Object, e As EventArgs) Handles btnFinancialFinInd.Click
        Hide()
        Financial.Show()
    End Sub

    Private Sub btnQualityFinInd_Click(sender As Object, e As EventArgs) Handles btnQualityFinInd.Click
        Hide()
        Quality.Show()
    End Sub

    Private Sub btnInpatientFinInd_Click(sender As Object, e As EventArgs) Handles btnInpatientFinInd.Click
        Hide()
        Inpatient.Show()
    End Sub

    Private Sub btnOutpatientFinInd_Click(sender As Object, e As EventArgs) Handles btnOutpatientFinInd.Click
        Hide()
        Outpatient.Show()
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
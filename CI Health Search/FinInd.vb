Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class FinInd
    Public strCMSnum As String
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


        lblPedFinIndResult.Text = If(provider("Fiscal Year End Date") IsNot Nothing, CDate(provider("Fiscal Year End Date")).ToString("MM/dd/yyyy"), "N/A")
        lblTotCurrentAssetsResult.Text = Search.CleanMeUp(provider("Total Current Assets"))
        lblTotCurrentAssetsResult2.Text = lblTotCurrentAssetsResult.Text
        lblTotAssetsResult.Text = Search.CleanMeUp(provider("Total Assets"))
        lblTotAssetsResult2.Text = lblTotAssetsResult.Text
        lblTotAssetsResult3.Text = lblTotAssetsResult.Text
        lblTotAssetsResult4.Text = lblTotAssetsResult.Text
        lblTotAssetsResult5.Text = lblTotAssetsResult.Text

        lblTotOperatingRevFinIndResult.Text = Search.CleanMeUp(provider("Net Patient Revenue"))
        lblTotOperatingRevenueFinInd2.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult3.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult4.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult5.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult6.Text = lblTotOperatingRevFinIndResult.Text

        lblInventoryResult.Text = Search.CleanMeUp(provider("Inventory"))
        lblInventoryResult2.Text = lblInventoryResult.Text

        lblAccountsRecievableResult.Text = Search.CleanMeUp(provider("Accounts Receivable"))
        lblAccountsRecievableResult2.Text = lblAccountsRecievableResult.Text

        lblOtherExpenseResultFindInd.Text = Search.CleanMeUp(provider("Total Other Expenses"))

        lblTotLongTermLiabilitiesResult.Text = Search.CleanMeUp(provider("Total Long Term Liabilities"))

        lblLeaseCostResult.Text = Search.CleanMeUp(provider("Leasehold Improvements"))

        lblNotesReceivableRes.Text = Search.CleanMeUp(provider("Notes Receivable"))



        lblTotOperatingExpenseFinIndResult.Text = Search.CleanMeUp(provider("Less Total Operating Expense"))
        lblTotOperatingExpenseFinInd2.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd3.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd4.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd5.Text = lblTotOperatingExpenseFinIndResult.Text


        lblOtherExpenseResultFindInd.Text = Search.CleanMeUp(provider("Total Other Expenses"))

        lblNetIncomeResult.Text = Search.CleanMeUp(provider("Net Income"))

        lblNetIncomeFinIndResult.Text = lblNetIncomeResult.Text
        ' lblInterestExpenseFinIndResult.Text = Search.CleanMeUp(provider("Interest Expense"))
        lblInterestExpenseFinIndResult.Text = "Result"
        lblDepAmortExpenseResult.Text = "Result"
        lblLeaseCostResult.Text = Search.CleanMeUp(provider("Leasehold Improvements"))
        lblEbitResult.Text = (CDec(Search.CleanMeUp(lblNetIncomeFinIndResult.Text, True)) + CDec(Search.CleanMeUp(lblInterestExpenseFinIndResult.Text, True)) + CDec(Search.CleanMeUp(lblDepAmortExpenseResult.Text, True)) + CDec(Search.CleanMeUp(lblLeaseCostResult.Text, True))).ToString

        lblNetIncomeResult2.Text = lblNetIncomeResult.Text
        lblDepreciationExpenseResultFinInd.Text = Search.CleanMeUp(provider("Depreciation Cost"))
        lblDepreciationExpenseResultFinInd2.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd3.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd4.Text = lblDepreciationExpenseResultFinInd.Text

        lblSalaryExpenseResult.Text = Search.CleanMeUp(provider("Total Salaries (adjusted)"))

        lblContractLaborResult.Text = If(provider("Contract Labor:Direct Patient Care") IsNot Nothing, CDec(provider("Contract Labor:Direct Patient Care")).ToString("N"), "N/A")

        lblTotLiabilitiesResult.Text = Search.CleanMeUp(provider("Total Liabilities"))
        lblTotLiabilitiesResult2.Text = lblTotLiabilitiesResult.Text
        lblTotLiabilitiesResult3.Text = lblTotLiabilitiesResult.Text

        lblCashonHandResult.Text = Search.CleanMeUp(provider("Cash on Hand and in Banks"))
        lblCashonHandResult2.Text = lblCashonHandResult.Text

        lblTotCurrentLiabilitiesResult.Text = Search.CleanMeUp(provider("Total Current Liabilities"))
        lblTotCurrentLiabilitesResult2.Text = lblTotCurrentLiabilitiesResult.Text
        lblTotCurrentLiabilitesResult3.Text = lblTotCurrentLiabilitiesResult.Text






        ' lblMarketSecuritiesResult.Text = Search.CleanMeUp(provider("Temporary Investments"))
        ' lblMarketSecuritiesResult2.Text = lblMarketSecuritiesResult.Text

        ' lblInvestmentsResultFinInd.Text = Search.CleanMeUp(provider("Investments"))

        'lblLeaseCostResult.Text = Search.CleanMeUp(provider("Leasehold Improvements"))
        '
        'lblNotesReceivableRes.Text = Search.CleanMeUp(provider("Notes Receivable"))

        lblAllowforUncollectRes.Text = Search.CleanMeUp(provider("Less: Allowances for Uncollectible Notes and Accounts Receivable"))
        lblAllowforUncollectRes2.Text = lblAllowforUncollectRes.Text

        lblTotLongTermLiabilitiesResult.Text = Search.CleanMeUp(provider("Total Long Term Liabilities"))



        'lblCashonHandResult.Text = If(provider("Cash on Hand and in Banks") IsNot Nothing, CDec(provider("Cash on Hand and in Banks")).ToString("N"), "N/A")
        'lblCashonHandResult2.Text = lblCashonHandResult.Text

        'lblInventoryResult.Text = Search.CleanMeUp(provider("Inventory"))
        'lblInventoryResult2.Text = lblInventoryResult.Text

        'lblAccountsRecievableResult.Text = If(provider("Accounts Receivable") IsNot Nothing, CDec(provider("Accounts Receivable")).ToString("N"), "N/A")
        'lblAccountsRecievableResult2.Text = lblAccountsRecievableResult.Text

        'lblOtherExpenseResultFindInd.Text = If(provider("Total Other Expenses") IsNot Nothing, CDec(provider("Total Other Expenses")).ToString("N"), "N/A")

        'lblTotLongTermLiabilitiesResult.Text = Search.CleanMeUp(provider("Total Long Term Liabilities"))

        'lblLeaseCostResult.Text = Search.CleanMeUp(provider("Leasehold Improvements"))

        'lblNotesReceivableRes.Text = Search.CleanMeUp(provider("Notes Receivable"))









    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileFinInd.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentsFinInd.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialFinInd.Click
        Me.Hide()
        Financial.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityFinInd.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientFinInd.Click
        Me.Hide()
        Inpatient.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientFinInd.Click
        Me.Hide()
        Outpatient.Show()
    End Sub

    Private Sub Label113_Click(sender As Object, e As EventArgs) Handles lblCashonHandResult2.Click

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
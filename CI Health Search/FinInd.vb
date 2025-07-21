Imports Newtonsoft.Json.Linq

Public Class FinInd
    Public strCMSnum As String

    ' Entry point for showing financial indicator data
    Public Async Function ShowFinancialDataApi(cmsNum As String) As Task
        lblstatus.Text = "Loading financial indicators..."
        lblstatus.Visible = True

        strCMSnum = cmsNum
        Try
            Dim apiUrl As String = ApiHelper.ApiUrls("MainProfileApi") & "?keyword=" & Uri.EscapeDataString(cmsNum) & "&size=1000"
            Dim data As JArray = Await ApiHelper.GetApiDataAsync(apiUrl)

            If data.Count = 0 Then
                SetAllFinIndLabels("No result")
                lblstatus.Text = ""
                lblstatus.Visible = False
                Exit Function
            End If

            Dim provider = data.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = cmsNum)
            If provider Is Nothing Then provider = data(0)

            lblPedFinIndResult.Text = AppHelpers.SafeGetDate(provider, "Fiscal Year End Date")
            lblTotCurrentAssetsResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Current Assets"))
            lblTotAssetsResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Assets"))
            lblTotOperatingRevFinIndResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Net Patient Revenue"))
            lblInventoryResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Inventory"))
            lblAccountsRecievableResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Accounts Receivable"))
            lblOtherExpenseResultFindInd.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Other Expenses"))
            lblTotLongTermLiabilitiesResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Long Term Liabilities"))
            lblLeaseCostResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Leasehold Improvements"))
            lblNotesReceivableRes.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Notes Receivable"))
            lblTotLiabilitiesResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Liabilities"))
            lblCashonHandResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Cash on Hand and in Banks"))
            lblTotCurrentLiabilitiesResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Current Liabilities"))
            lblTotOperatingExpenseFinIndResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Less Total Operating Expense"))
            lblNetIncomeResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Net Income"))
            lblDepreciationExpenseResultFinInd.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Depreciation Cost"))
            lblSalaryExpenseResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Salaries (adjusted)"))
            lblContractLaborResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Contract Labor:Direct Patient Care"))
            lblAllowforUncollectRes.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Less: Allowances for Uncollectible Notes and Accounts Receivable"))
            lblMarketSecuritiesResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Temporary Investments"))
            lblInvestmentsResultFinInd.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Investments"))
            lblDepAmortExpenseResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Depreciation Cost"))

            ' Example: Set calculated fields to "N/A" or perform calculations as needed
            lblInterestExpenseFinIndResult.Text = "N/A"
            lblEbitResult.Text = "N/A"
            lblOperatingMarginFinIndResult.Text = "N/A"

            lblstatus.Text = ""
        Catch ex As Exception
            SetAllFinIndLabels("No result")
            lblstatus.Text = "Error loading financial indicators. Please try again."
        End Try

        lblstatus.Visible = False
    End Function

    ' Helper: Set all FinInd labels to a value
    Private Sub SetAllFinIndLabels(val As String)
        lblPedFinIndResult.Text = val
        lblTotCurrentAssetsResult.Text = val
        lblTotAssetsResult.Text = val
        lblTotOperatingRevFinIndResult.Text = val
        lblInventoryResult.Text = val
        lblAccountsRecievableResult.Text = val
        lblOtherExpenseResultFindInd.Text = val
        lblTotLongTermLiabilitiesResult.Text = val
        lblLeaseCostResult.Text = val
        lblNotesReceivableRes.Text = val
        lblTotLiabilitiesResult.Text = val
        lblCashonHandResult.Text = val
        lblTotCurrentLiabilitiesResult.Text = val
        lblTotOperatingExpenseFinIndResult.Text = val
        lblNetIncomeResult.Text = val
        lblDepreciationExpenseResultFinInd.Text = val
        lblSalaryExpenseResult.Text = val
        lblContractLaborResult.Text = val
        lblAllowforUncollectRes.Text = val
        lblMarketSecuritiesResult.Text = val
        lblInvestmentsResultFinInd.Text = val
        lblDepAmortExpenseResult.Text = val
        lblInterestExpenseFinIndResult.Text = val
        lblEbitResult.Text = val
        lblOperatingMarginFinIndResult.Text = val
    End Sub

    ' Navigation buttons
    Private Sub btnProfileFinInd_Click(sender As Object, e As EventArgs) Handles btnProfileFinInd.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub btnDepartmentsFinInd_Click(sender As Object, e As EventArgs) Handles btnDepartmentsFinInd.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub btnFinancialFinInd_Click(sender As Object, e As EventArgs) Handles btnFinancialFinInd.Click
        Me.Hide()
        Financial.Show()
    End Sub

    Private Sub btnQualityFinInd_Click(sender As Object, e As EventArgs) Handles btnQualityFinInd.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub btnInpatientFinInd_Click(sender As Object, e As EventArgs) Handles btnInpatientFinInd.Click
        Me.Hide()
        Inpatient.Show()
        Inpatient.LoadPatientOriginDataAsync(Results.SelectedHospital)
        Inpatient.LoadCeoDataAsync(Results.SelectedHospital)
    End Sub

    Private Sub btnOutpatientFinInd_Click(sender As Object, e As EventArgs) Handles btnOutpatientFinInd.Click
        Me.Hide()
        Outpatient.Show()
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
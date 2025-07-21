Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class FinInd

    Public Function FormatCurrency(val As Object) As String
        Dim dec As Decimal
        If Decimal.TryParse(val.ToString().Replace("$", "").Replace(",", ""), dec) Then
            Return dec.ToString("N0") ' 1,234 (no decimals)
        End If
        Return "N/A"
    End Function





    Public strCMSnum As String
    Dim foundFinHosp As HospitalContext
    Public Async Function ShowFinancialDataApi(cmsNum As String) As Task
        ' Step 1: Populate from Results.SelectedHospital
        Dim hosp = Results.SelectedHospital

        lblPedFinIndResult.Text = If(hosp.TotalDays > 0, hosp.TotalDays.ToString(), "N/A")
        lblTotCurrentAssetsResult.Text = If(hosp.TotalCurrentAssets > 0, hosp.TotalCurrentAssets.ToString("N0"), "N/A")
        lblTotCurrentAssetsResult2.Text = lblTotCurrentAssetsResult.Text
        lblTotCurrentAssets10.Text = lblTotCurrentAssetsResult.Text

        lblTotAssetsResult.Text = If(hosp.TotalAssets > 0, hosp.TotalAssets.ToString("N0"), "N/A")
        lblTotAssetsResult2.Text = lblTotAssetsResult.Text
        lblTotAssetsResult3.Text = lblTotAssetsResult.Text
        lblTotAssetsResult4.Text = lblTotAssetsResult.Text
        lblTotAssetsResult5.Text = lblTotAssetsResult.Text

        lblTotOperatingRevFinIndResult.Text = If(hosp.NetPatientRev > 0, hosp.NetPatientRev.ToString("N0"), "N/A")
        lblTotOperatingRevenueFinInd2.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult3.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult4.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult5.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult6.Text = lblTotOperatingRevFinIndResult.Text

        lblInventoryResult.Text = If(hosp.Inventory > 0, hosp.Inventory.ToString("N0"), "N/A")
        lblInventoryResult2.Text = lblInventoryResult.Text

        lblAccountsRecievableResult.Text = "N/A" ' Add to HospitalContext if needed
        lblAccountsRecievableResult2.Text = lblAccountsRecievableResult.Text

        lblOtherExpenseResultFindInd.Text = "N/A" ' Add to HospitalContext if needed

        lblTotLongTermLiabilitiesResult.Text = If(hosp.TotalLongTermLiabilities > 0, hosp.TotalLongTermLiabilities.ToString("N0"), "N/A")
        lblLeaseCostResult.Text = If(hosp.LeaseCost > 0, hosp.LeaseCost.ToString("N0"), "N/A")
        lblNotesReceivableRes.Text = If(hosp.NotesReceivable > 0, hosp.NotesReceivable.ToString("N0"), "N/A")

        lblTotLiabilitiesResult.Text = If(hosp.TotalLiabilities > 0, hosp.TotalLiabilities.ToString("N0"), "N/A")
        lblTotLiabilitiesResult2.Text = lblTotLiabilitiesResult.Text
        lblTotLiabilitiesResult3.Text = lblTotLiabilitiesResult.Text

        lblCashonHandResult.Text = "N/A" ' Add to HospitalContext if needed
        lblCashonHandResult2.Text = lblCashonHandResult.Text

        lblTotCurrentLiabilitiesResult.Text = If(hosp.TotalCurrentLiabilities > 0, hosp.TotalCurrentLiabilities.ToString("N0"), "N/A")
        lblTotCurrentLiabilitesResult2.Text = lblTotCurrentLiabilitiesResult.Text
        lblTotCurrentLiabilitesResult3.Text = lblTotCurrentLiabilitiesResult.Text
        lblTotCurrentLiabilities10.Text = lblTotCurrentLiabilitiesResult.Text

        lblTotOperatingExpenseFinIndResult.Text = If(hosp.TotalOperatingExpense > 0, hosp.TotalOperatingExpense.ToString("N0"), "N/A")
        lblTotOperatingExpenseFinInd2.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd3.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd4.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd5.Text = lblTotOperatingExpenseFinIndResult.Text

        lblNetIncomeResult.Text = If(hosp.NetIncome > 0, hosp.NetIncome.ToString("N0"), "N/A")
        lblNetIncomeFinIndResult.Text = lblNetIncomeResult.Text
        lblNetIncomeResult2.Text = lblNetIncomeResult.Text

        lblInterestExpenseFinIndResult.Text = "N/A" ' Add to HospitalContext if needed
        lblDepAmortExpenseResult.Text = "N/A" ' Add to HospitalContext if needed

        lblEbitResult.Text = "N/A" ' You can calculate if all components are present in HospitalContext
        lblOperatingMarginFinIndResult.Text = "N/A" ' You can calculate if all components are present

        lblDepreciationExpenseResultFinInd.Text = If(hosp.DepreciationCost > 0, hosp.DepreciationCost.ToString("N0"), "N/A")
        lblDepreciationExpenseResultFinInd2.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd3.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd4.Text = lblDepreciationExpenseResultFinInd.Text

        lblSalaryExpenseResult.Text = "N/A" ' Add to HospitalContext if needed
        lblContractLaborResult.Text = "N/A" ' Add to HospitalContext if needed

        lblAllowforUncollectRes.Text = "N/A" ' Add to HospitalContext if needed
        lblAllowforUncollectRes2.Text = lblAllowforUncollectRes.Text

        lblMarketSecuritiesResult.Text = If(hosp.MarketSecurities > 0, hosp.MarketSecurities.ToString("N0"), "N/A")
        lblMarketSecuritiesResult2.Text = lblMarketSecuritiesResult.Text

        lblInvestmentsResultFinInd.Text = If(hosp.Investments > 0, hosp.Investments.ToString("N0"), "N/A")

        lblDepAmortExpenseResult.Text = "N/A" ' Add to HospitalContext if needed

        ' Step 2: Supplement with API for the most up-to-date info
        Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?keyword=" & Uri.EscapeDataString(cmsNum) & "&size=1000"
        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)
                If data.Count = 0 Then
                    MessageBox.Show("No data returned from API.")
                    Exit Function
                End If
                Dim provider = data.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = cmsNum)
                If provider Is Nothing Then provider = data(0)

                ' Now update labels with API data (as in your original code)
                lblPedFinIndResult.Text = If(provider("Fiscal Year End Date") IsNot Nothing, CDate(provider("Fiscal Year End Date")).ToString("MM/dd/yyyy"), lblPedFinIndResult.Text)
                lblTotCurrentAssetsResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Total Current Assets")))
                lblTotCurrentAssetsResult2.Text = lblTotCurrentAssetsResult.Text
                lblTotCurrentAssets10.Text = lblTotCurrentAssetsResult.Text

                lblTotAssetsResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Total Assets")))
                lblTotAssetsResult2.Text = lblTotAssetsResult.Text
                lblTotAssetsResult3.Text = lblTotAssetsResult.Text
                lblTotAssetsResult4.Text = lblTotAssetsResult.Text
                lblTotAssetsResult5.Text = lblTotAssetsResult.Text

                lblTotOperatingRevFinIndResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Net Patient Revenue")))
                lblTotOperatingRevenueFinInd2.Text = lblTotOperatingRevFinIndResult.Text
                lblTotOperatingRevResult3.Text = lblTotOperatingRevFinIndResult.Text
                lblTotOperatingRevResult4.Text = lblTotOperatingRevFinIndResult.Text
                lblTotOperatingRevResult5.Text = lblTotOperatingRevFinIndResult.Text
                lblTotOperatingRevResult6.Text = lblTotOperatingRevFinIndResult.Text

                lblInventoryResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Inventory")))
                lblInventoryResult2.Text = lblInventoryResult.Text

                lblAccountsRecievableResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Accounts Receivable")))
                lblAccountsRecievableResult2.Text = lblAccountsRecievableResult.Text

                lblOtherExpenseResultFindInd.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Total Other Expenses")))

                lblTotLongTermLiabilitiesResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Total Long Term Liabilities")))
                lblLeaseCostResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Leasehold Improvements")))
                lblNotesReceivableRes.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Notes Receivable")))

                lblTotLiabilitiesResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Total Liabilities")))
                lblTotLiabilitiesResult2.Text = lblTotLiabilitiesResult.Text
                lblTotLiabilitiesResult3.Text = lblTotLiabilitiesResult.Text

                lblCashonHandResult.Text = Me.FormatCurrency(provider("Cash on Hand and in Banks"))
                lblCashonHandResult2.Text = lblCashonHandResult.Text

                lblTotCurrentLiabilitiesResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Total Current Liabilities")))
                lblTotCurrentLiabilitesResult2.Text = lblTotCurrentLiabilitiesResult.Text
                lblTotCurrentLiabilitesResult3.Text = lblTotCurrentLiabilitiesResult.Text
                lblTotCurrentLiabilities10.Text = lblTotCurrentLiabilitiesResult.Text

                lblTotOperatingExpenseFinIndResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Less Total Operating Expense")))
                lblTotOperatingExpenseFinInd2.Text = lblTotOperatingExpenseFinIndResult.Text
                lblTotOperatingExpenseFinInd3.Text = lblTotOperatingExpenseFinIndResult.Text
                lblTotOperatingExpenseFinInd4.Text = lblTotOperatingExpenseFinIndResult.Text
                lblTotOperatingExpenseFinInd5.Text = lblTotOperatingExpenseFinIndResult.Text

                lblNetIncomeResult.Text = Me.FormatCurrency(Search.CleanMeUp(provider("Net Income")))
                lblNetIncomeFinIndResult.Text = lblNetIncomeResult.Text
                lblNetIncomeResult2.Text = lblNetIncomeResult.Text

                ' ...continue for all other fields as in your original API code...
            End If
        End Using
    End Function





    '







    Private Sub finind_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Me.Size = New Size(900, 1500) ' or whatever you want
        '  Me.MaximumSize = New Size(0, 0) ' unlimited


        ' Create a new BoldGroupBox
        Dim boldGb As New BoldGroupBox With {
            .Text = ".",
            .Location = New Point(4, 108),    ' Use the same location as GroupBox3
            .Size = New Size(626, 153),       ' Use the same size as GroupBox3
            .BackColor = Color.LightGray
        }

        ' Add the relevant controls to the new BoldGroupBox
        boldGb.Controls.Add(Label4)   ' "EBITDAR - (Earnings before Interest, Taxes, Depreciation, Amortization, and Rent)"
        boldGb.Controls.Add(Label5)   ' "Net Income (Before Taxes)"
        boldGb.Controls.Add(Label6)   ' "Interest Expense"
        boldGb.Controls.Add(Label7)   ' "Depreciation and Amortization Expense"
        boldGb.Controls.Add(Label8)   ' "Lease Cost"
        boldGb.Controls.Add(lblEbitResult)
        boldGb.Controls.Add(lblNetIncomeFinIndResult)
        boldGb.Controls.Add(lblInterestExpenseFinIndResult)
        boldGb.Controls.Add(lblDepAmortExpenseResult)
        boldGb.Controls.Add(lblLeaseCostResult)
        ' Add other labels as needed

        ' Add the new BoldGroupBox to the form
        Panel1.Controls.Add(boldGb)


        ' Test Financial Summary Grid


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
        Inpatient.LoadPatientOriginDataAsync(Results.SelectedHospital)
        Inpatient.LoadCeoDataAsync(Results.SelectedHospital)
    End Sub

    Private Sub btnOutpatientFinInd_Click(sender As Object, e As EventArgs) Handles btnOutpatientFinInd.Click
        Hide()
        Outpatient.Show()
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub

    Private Sub GroupBox5_Enter(sender As Object, e As EventArgs) Handles GroupBox5.Enter

    End Sub

    Private Sub dgvfinancialsummary_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub
End Class
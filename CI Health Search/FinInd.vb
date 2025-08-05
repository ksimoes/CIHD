Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class FinInd

    Private currentHospital As HospitalContext

    Private Function GetBestFacilityName(ctx As HospitalContext) As String
        If ctx Is Nothing Then Return "N/A"
        If ctx.LastDataRow IsNot Nothing Then
            Dim row = ctx.LastDataRow
            If row.Table.Columns.Contains("FAC_NAME") Then Return row("FAC_NAME").ToString()
            If row.Table.Columns.Contains("PRVDR_NAME") Then Return row("PRVDR_NAME").ToString()
            If row.Table.Columns.Contains("ORGANIZATION NAME") Then Return row("ORGANIZATION NAME").ToString()
            If row.Table.Columns.Contains("organization_name") Then Return row("organization_name").ToString()
            If row.Table.Columns.Contains("Facility Name") Then Return row("Facility Name").ToString()
            If row.Table.Columns.Contains("Hospital Name") Then Return row("Hospital Name").ToString()
        End If
        If Not String.IsNullOrWhiteSpace(ctx.Name) Then Return ctx.Name
        Return "N/A"
    End Function

    ' New constructor to accept a HospitalContext
    Public Sub New(hosp As HospitalContext)
        InitializeComponent()
        currentHospital = hosp
    End Sub

    ' Default constructor for designer compatibility
    Public Sub New()
        InitializeComponent()
    End Sub

    ' Format currency with dollar sign and commas, fallback to N/A
    Public Shared Function FormatCurrency(val As Object) As String
        Dim dec As Decimal
        If Decimal.TryParse(val?.ToString().Replace("$", "").Replace(",", ""), dec) Then
            Return dec.ToString("C0")
        End If
        If val IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(val.ToString()) Then
            Return "$" & val.ToString()
        End If
        Return "N/A"
    End Function

    Public strCMSnum As String
    Dim foundFinHosp As HospitalContext

    Public Async Function ShowFinancialDataApi(cmsNum As String) As Task
        ' Step 1: Populate from Results.SelectedHospital
        Dim hosp = currentHospital
        lblHN.Text = GetBestFacilityName(hosp)

        lblPedFinIndResult.Text = If(hosp.TotalDays > 0, hosp.TotalDays.ToString(), "N/A")
        lblTotCurrentAssetsResult.Text = FormatCurrency(hosp.TotalCurrentAssets)
        lblTotCurrentAssetsResult2.Text = lblTotCurrentAssetsResult.Text
        lblTotCurrentAssets10.Text = lblTotCurrentAssetsResult.Text

        lblTotAssetsResult.Text = FormatCurrency(hosp.TotalAssets)
        lblTotAssetsResult2.Text = lblTotAssetsResult.Text
        lblTotAssetsResult3.Text = lblTotAssetsResult.Text
        lblTotAssetsResult4.Text = lblTotAssetsResult.Text
        lblTotAssetsResult5.Text = lblTotAssetsResult.Text

        lblTotOperatingRevFinIndResult.Text = FormatCurrency(hosp.NetPatientRev)
        lblTotOperatingRevenueFinInd2.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult3.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult4.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult5.Text = lblTotOperatingRevFinIndResult.Text
        lblTotOperatingRevResult6.Text = lblTotOperatingRevFinIndResult.Text

        lblInventoryResult.Text = FormatCurrency(hosp.Inventory)
        lblInventoryResult2.Text = lblInventoryResult.Text

        lblAccountsRecievableResult.Text = "N/A"
        lblAccountsRecievableResult2.Text = lblAccountsRecievableResult.Text

        lblOtherExpenseResultFindInd.Text = "N/A"

        lblTotLongTermLiabilitiesResult.Text = FormatCurrency(hosp.TotalLongTermLiabilities)
        lblLeaseCostResult.Text = FormatCurrency(hosp.LeaseCost)
        lblNotesReceivableRes.Text = FormatCurrency(hosp.NotesReceivable)

        lblTotLiabilitiesResult.Text = FormatCurrency(hosp.TotalLiabilities)
        lblTotLiabilitiesResult2.Text = lblTotLiabilitiesResult.Text
        lblTotLiabilitiesResult3.Text = lblTotLiabilitiesResult.Text

        lblCashonHandResult.Text = "N/A"
        lblCashonHandResult2.Text = lblCashonHandResult.Text

        lblTotCurrentLiabilitiesResult.Text = FormatCurrency(hosp.TotalCurrentLiabilities)
        lblTotCurrentLiabilitesResult2.Text = lblTotCurrentLiabilitiesResult.Text
        lblTotCurrentLiabilitesResult3.Text = lblTotCurrentLiabilitiesResult.Text
        lblTotCurrentLiabilities10.Text = lblTotCurrentLiabilitiesResult.Text

        lblTotOperatingExpenseFinIndResult.Text = FormatCurrency(hosp.TotalOperatingExpense)
        lblTotOperatingExpenseFinInd2.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd3.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd4.Text = lblTotOperatingExpenseFinIndResult.Text
        lblTotOperatingExpenseFinInd5.Text = lblTotOperatingExpenseFinIndResult.Text

        lblNetIncomeResult.Text = FormatCurrency(hosp.NetIncome)
        lblNetIncomeFinIndResult.Text = lblNetIncomeResult.Text
        lblNetIncomeResult2.Text = lblNetIncomeResult.Text

        lblInterestExpenseFinIndResult.Text = "N/A"
        lblDepAmortExpenseResult.Text = "N/A"

        lblEbitResult.Text = "N/A"
        lblOperatingMarginFinIndResult.Text = "N/A"

        lblDepreciationExpenseResultFinInd.Text = FormatCurrency(hosp.DepreciationCost)
        lblDepreciationExpenseResultFinInd2.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd3.Text = lblDepreciationExpenseResultFinInd.Text
        lblDepreciationExpenseResultFinInd4.Text = lblDepreciationExpenseResultFinInd.Text

        lblSalaryExpenseResult.Text = "N/A"
        lblContractLaborResult.Text = "N/A"

        lblAllowforUncollectRes.Text = "N/A"
        lblAllowforUncollectRes2.Text = lblAllowforUncollectRes.Text

        lblMarketSecuritiesResult.Text = FormatCurrency(hosp.MarketSecurities)
        lblMarketSecuritiesResult2.Text = lblMarketSecuritiesResult.Text

        lblInvestmentsResultFinInd.Text = FormatCurrency(hosp.Investments)

        lblDepAmortExpenseResult.Text = "N/A"

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

                lblPedFinIndResult.Text = If(provider("Fiscal Year End Date") IsNot Nothing, CDate(provider("Fiscal Year End Date")).ToString("MM/dd/yyyy"), lblPedFinIndResult.Text)
                lblTotCurrentAssetsResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Current Assets")))
                lblTotCurrentAssetsResult2.Text = lblTotCurrentAssetsResult.Text
                lblTotCurrentAssets10.Text = lblTotCurrentAssetsResult.Text

                lblTotAssetsResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Assets")))
                lblTotAssetsResult2.Text = lblTotAssetsResult.Text
                lblTotAssetsResult3.Text = lblTotAssetsResult.Text
                lblTotAssetsResult4.Text = lblTotAssetsResult.Text
                lblTotAssetsResult5.Text = lblTotAssetsResult.Text

                lblTotOperatingRevFinIndResult.Text = FormatCurrency(Search.CleanMeUp(provider("Net Patient Revenue")))
                lblTotOperatingRevenueFinInd2.Text = lblTotOperatingRevFinIndResult.Text
                lblTotOperatingRevResult3.Text = lblTotOperatingRevFinIndResult.Text
                lblTotOperatingRevResult4.Text = lblTotOperatingRevFinIndResult.Text
                lblTotOperatingRevResult5.Text = lblTotOperatingRevFinIndResult.Text
                lblTotOperatingRevResult6.Text = lblTotOperatingRevFinIndResult.Text

                lblInventoryResult.Text = FormatCurrency(Search.CleanMeUp(provider("Inventory")))
                lblInventoryResult2.Text = lblInventoryResult.Text

                lblAccountsRecievableResult.Text = FormatCurrency(Search.CleanMeUp(provider("Accounts Receivable")))
                lblAccountsRecievableResult2.Text = lblAccountsRecievableResult.Text

                lblOtherExpenseResultFindInd.Text = FormatCurrency(Search.CleanMeUp(provider("Total Other Expenses")))

                lblTotLongTermLiabilitiesResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Long Term Liabilities")))
                lblLeaseCostResult.Text = FormatCurrency(Search.CleanMeUp(provider("Leasehold Improvements")))
                lblNotesReceivableRes.Text = FormatCurrency(Search.CleanMeUp(provider("Notes Receivable")))

                lblTotLiabilitiesResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Liabilities")))
                lblTotLiabilitiesResult2.Text = lblTotLiabilitiesResult.Text
                lblTotLiabilitiesResult3.Text = lblTotLiabilitiesResult.Text

                lblCashonHandResult.Text = FormatCurrency(provider("Cash on Hand and in Banks"))
                lblCashonHandResult2.Text = lblCashonHandResult.Text

                lblTotCurrentLiabilitiesResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Current Liabilities")))
                lblTotCurrentLiabilitesResult2.Text = lblTotCurrentLiabilitiesResult.Text
                lblTotCurrentLiabilitesResult3.Text = lblTotCurrentLiabilitiesResult.Text
                lblTotCurrentLiabilities10.Text = lblTotCurrentLiabilitiesResult.Text

                lblTotOperatingExpenseFinIndResult.Text = FormatCurrency(Search.CleanMeUp(provider("Less Total Operating Expense")))
                lblTotOperatingExpenseFinInd2.Text = lblTotOperatingExpenseFinIndResult.Text
                lblTotOperatingExpenseFinInd3.Text = lblTotOperatingExpenseFinIndResult.Text
                lblTotOperatingExpenseFinInd4.Text = lblTotOperatingExpenseFinIndResult.Text
                lblTotOperatingExpenseFinInd5.Text = lblTotOperatingExpenseFinIndResult.Text

                lblNetIncomeResult.Text = FormatCurrency(Search.CleanMeUp(provider("Net Income")))
                lblNetIncomeFinIndResult.Text = lblNetIncomeResult.Text
                lblNetIncomeResult2.Text = lblNetIncomeResult.Text

                ' ...continue for all other fields as in your original API code...
            End If
        End Using
    End Function

    Private Async Sub FinInd_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If currentHospital IsNot Nothing Then
            lblHN.Text = GetBestFacilityName(currentHospital)
            Await ShowFinancialDataApi(currentHospital.CMSNum)
        Else
            lblHN.Text = "No hospital context"
        End If

        ' Create a new BoldGroupBox
        Dim boldGb As New BoldGroupBox With {
            .Text = ".",
            .Location = New Point(4, 108),
            .Size = New Size(626, 153),
            .BackColor = Color.LightGray
        }

        boldGb.Controls.Add(Label4)
        boldGb.Controls.Add(Label5)
        boldGb.Controls.Add(Label6)
        boldGb.Controls.Add(Label7)
        boldGb.Controls.Add(Label8)
        boldGb.Controls.Add(lblEbitResult)
        boldGb.Controls.Add(lblNetIncomeFinIndResult)
        boldGb.Controls.Add(lblInterestExpenseFinIndResult)
        boldGb.Controls.Add(lblDepAmortExpenseResult)
        boldGb.Controls.Add(lblLeaseCostResult)
        Panel1.Controls.Add(boldGb)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Hide()
        Dim profileForm As New Profile(currentHospital)
        profileForm.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Hide()
        Dim departmentsForm As New Departments(currentHospital)
        departmentsForm.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Hide()
        Dim financialForm As New Financial(currentHospital)
        financialForm.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        Hide()
        Dim qualityForm As New Quality(currentHospital)
        qualityForm.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        Hide()
        Dim inpatientForm As New Inpatient(currentHospital)
        inpatientForm.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)
        Hide()
        Dim outpatientForm As New Outpatient(currentHospital)
        outpatientForm.Show()
    End Sub

    Private Sub btnProfileFinInd_Click(sender As Object, e As EventArgs) Handles btnProfileFinInd.Click
        Hide()
        Dim profileForm As New Profile(currentHospital)
        profileForm.Show()
    End Sub

    Private Sub btnDepartmentsFinInd_Click(sender As Object, e As EventArgs) Handles btnDepartmentsFinInd.Click
        Hide()
        Dim departmentsForm As New Departments(currentHospital)
        departmentsForm.Show()
    End Sub

    Private Sub btnFinancialFinInd_Click(sender As Object, e As EventArgs) Handles btnFinancialFinInd.Click
        Hide()
        Dim finForm As New Financial(currentHospital)
        finForm.Show()
    End Sub

    Private Sub btnQualityFinInd_Click(sender As Object, e As EventArgs) Handles btnQualityFinInd.Click
        Hide()
        Dim qualityForm As New Quality(currentHospital)
        qualityForm.Show()
    End Sub

    Private Sub btnInpatientFinInd_Click(sender As Object, e As EventArgs) Handles btnInpatientFinInd.Click
        Hide()
        Dim inpatientForm As New Inpatient(currentHospital)
        inpatientForm.Show()
    End Sub

    Private Sub btnOutpatientFinInd_Click(sender As Object, e As EventArgs) Handles btnOutpatientFinInd.Click
        Hide()
        Dim outpatientForm As New Outpatient(currentHospital)
        outpatientForm.Show()
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
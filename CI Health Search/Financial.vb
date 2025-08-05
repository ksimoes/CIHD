Imports System.Data.SqlClient
Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class Financial
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    Public strCMSnum As String

    ' Store the context for this instance
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

    ' Helper to format currency values
    Public Shared Function FormatCurrency(rawValue As Object) As String
        Dim moneyVal As Decimal
        If Decimal.TryParse(rawValue?.ToString(), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, moneyVal) Then
            Return moneyVal.ToString("C0")
        ElseIf rawValue IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(rawValue.ToString()) Then
            Return "$" & rawValue.ToString()
        Else
            Return "N/A"
        End If
    End Function

    ' Call this method to load data for the
    Public Async Function ShowFinancialData(hospitalId As Integer, Optional ByVal state As String = "") As Task
        Dim hosp = currentHospital
        lblHN.Text = GetBestFacilityName(hosp)

        lblPedResult.Text = If(hosp.TotalDays > 0, hosp.TotalDays.ToString(), "N/A")
        lblCurAssetResult.Text = FormatCurrency(hosp.TotalCurrentAssets)
        lblFixAssetsResult.Text = FormatCurrency(hosp.TotalAssets)
        lblTotAssetsResult.Text = FormatCurrency(hosp.TotalAssets)
        lblNetPatRevResult.Text = FormatCurrency(hosp.NetPatientRev)
        lblTotPatRevResult.Text = FormatCurrency(hosp.TotalPatientRev)
        lblOutPatResult.Text = "N/A"
        lblInpRevResult.Text = "N/A"
        lblTotOperatingExpenseResult.Text = FormatCurrency(hosp.TotalOperatingExpense)
        lblNetIncomeResult.Text = FormatCurrency(hosp.NetIncome)
        lblDepreciationExpenseResult.Text = FormatCurrency(hosp.DepreciationCost)
        lblCcResult.Text = FormatCurrency(hosp.CharityCost)
        lblUncompResult.Text = FormatCurrency(hosp.UncompensatedCost)
        lblTotUcResult.Text = FormatCurrency(hosp.UncompensatedCost)
        lblCurLiabilitiesRes.Text = FormatCurrency(hosp.TotalCurrentLiabilities)
        lblLtResult.Text = FormatCurrency(hosp.TotalLongTermLiabilities)
        lblTlResult.Text = FormatCurrency(hosp.TotalLiabilities)
        ' ...add more as needed...

        ' Step 2: Supplement with DB for TN/TX
        If state = "TN" Or state = "TX" Then
            Dim queryFinance As String = ""
            Dim queryCharity As String = ""
            If state = "TN" Then
                queryFinance = "SELECT * FROM tn.Financials WHERE LicenseNum = @LicenseNum"
            ElseIf state = "TX" Then
                queryFinance = "SELECT * FROM tx.Finance WHERE id = @id"
                queryCharity = "SELECT * FROM tx.Charity WHERE id = @id"
            End If

            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(queryFinance, conn)
                    If state = "TN" Then
                        cmd.Parameters.AddWithValue("@LicenseNum", hospitalId)
                    ElseIf state = "TX" Then
                        cmd.Parameters.AddWithValue("@id", hospitalId)
                    End If
                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            ' Overwrite with DB values if available
                            lblInpRevResult.Text = FormatCurrency(If(Not IsDBNull(reader("Total Gross Inpatient Revenue")), reader("Total Gross Inpatient Revenue"), lblInpRevResult.Text))
                            lblOutPatResult.Text = FormatCurrency(If(Not IsDBNull(reader("Total Gross Outpatient Revenue")), reader("Total Gross Outpatient Revenue"), lblOutPatResult.Text))
                            ' ...add more as needed...
                        End If
                    End Using
                End Using
            End Using

            If state = "TX" Then
                Using conn2 As New SqlConnection(connectionString)
                    Using cmd2 As New SqlCommand(queryCharity, conn2)
                        cmd2.Parameters.AddWithValue("@id", hospitalId)
                        conn2.Open()
                        Using reader2 = cmd2.ExecuteReader()
                            If reader2.Read() Then
                                lblTotUcResult.Text = FormatCurrency(If(Not IsDBNull(reader2("Total Uncompensated Care")), reader2("Total Uncompensated Care"), lblTotUcResult.Text))
                                lblUncompResult.Text = FormatCurrency(If(Not IsDBNull(reader2("Bad Debt Charges")), reader2("Bad Debt Charges"), lblUncompResult.Text))
                                lblCcResult.Text = FormatCurrency(If(Not IsDBNull(reader2("Charity Charges")), reader2("Charity Charges"), lblCcResult.Text))
                                lblucpctResult.Text = If(Not IsDBNull(reader2("Uncompensated Care as pcnt of GPR")), reader2("Uncompensated Care as pcnt of GPR").ToString(), "N/A")
                            End If
                        End Using
                    End Using
                End Using
            End If
            Return
        End If

        ' Step 3: Supplement with API for all other states
        Await ShowFinancialDataApi(hosp.CMSNum)
    End Function

    ' API-based financial data for non-TN/TX
    Public Async Function ShowFinancialDataApi(cmsNum As String) As Task
        strCMSnum = cmsNum
        Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?keyword=" & Uri.EscapeDataString(cmsNum) & "&size=1000"
        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)
                If data.Count > 0 Then
                    ' Find the exact match for Provider CCN if possible
                    Dim provider = data.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = cmsNum)
                    If provider Is Nothing Then provider = data(0)

                    lblPedResult.Text = If(provider("Fiscal Year End Date") IsNot Nothing, CDate(provider("Fiscal Year End Date")).ToString("MM/dd/yyyy"), "N/A")
                    lblCurAssetResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Current Assets")))
                    lblFixAssetsResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Fixed Assets")))
                    lblOtherAssetsResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Other Assets")))
                    lblTotAssetsResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Assets")))
                    lblNetPatRevResult.Text = FormatCurrency(Search.CleanMeUp(provider("Net Patient Revenue")))
                    lblTotPatRevResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Patient Revenue")))
                    lblOutPatResult.Text = FormatCurrency(Search.CleanMeUp(provider("Outpatient Revenue")))
                    lblInpRevResult.Text = FormatCurrency(Search.CleanMeUp(provider("Inpatient Revenue")))
                    lblTotOperatingExpenseResult.Text = FormatCurrency(Search.CleanMeUp(provider("Less Total Operating Expense")))
                    lblContractAllowanceResult.Text = FormatCurrency(Search.CleanMeUp(provider("Less Contractual Allowance and Discounts on Patients' Account")))
                    lblTotOtherIncomeResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Other Income")))
                    lblTotOtherExpensesResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Other Expenses")))
                    lblNetIncomeResult.Text = FormatCurrency(Search.CleanMeUp(provider("Net Income")))
                    lblDepreciationExpenseResult.Text = FormatCurrency(Search.CleanMeUp(provider("Depreciation Cost")))
                    lblCcResult.Text = FormatCurrency(Search.CleanMeUp(provider("Cost of Charity Care")))
                    lblUncompResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Bad Debt Expense")))
                    lblTotUcResult.Text = FormatCurrency(Search.CleanMeUp(provider("Cost of Uncompensated Care")))

                    lblCurLiabilitiesRes.Text = FormatCurrency(Search.CleanMeUp(provider("Total Current Liabilities")))
                    lblLtResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Long Term Liabilities")))
                    lblTlResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Liabilities")))
                    lblTotFbResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Fund Balances")))
                    lblTotLandFbResult.Text = FormatCurrency(Search.CleanMeUp(provider("Total Liabilities and Fund Balances")))

                    'lblucpctResult.Text = If(provider("Uncompensated Care as % of GPR") IsNot Nothing, CDec(provider("Uncompensated Care as % of GPR")).ToString("P"), "N/A")
                Else
                    SetAllFinancialLabels("No result")
                End If
            Else
                SetAllFinancialLabels("API error")
            End If
        End Using
        Using HospGenClient As New HttpClient()
            Dim response As HttpResponseMessage = Await HospGenClient.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)
            End If
        End Using
    End Function

    Private Sub SetAllFinancialLabels(val As String)
        lblPedResult.Text = val
        lblCurAssetResult.Text = val
        lblFixAssetsResult.Text = val
        lblOtherAssetsResult.Text = val
        lblTotAssetsResult.Text = val
        lblCurLiabilitiesRes.Text = val
        lblLtResult.Text = val
        lblTlResult.Text = val
        lblTotFbResult.Text = val
        lblTotLandFbResult.Text = val
        lblInpRevResult.Text = val
        lblOutPatResult.Text = val
        lblTotPatRevResult.Text = val
        lblNetPatRevResult.Text = val
    End Sub

    ' Automatically load data when the form loads
    Private Async Sub Financial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If currentHospital IsNot Nothing Then
            lblHN.Text = GetBestFacilityName(currentHospital)
            Dim state = currentHospital.State
            If state = "TN" Or state = "TX" Then
                Await ShowFinancialData(currentHospital.HospitalId, state)
            Else
                Await ShowFinancialDataApi(currentHospital.CMSNum)
            End If
        Else
            lblHN.Text = "No hospital context"
        End If
    End Sub

    Private Async Sub Financial_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If currentHospital IsNot Nothing Then
            lblHN.Text = GetBestFacilityName(currentHospital)
            Dim state = currentHospital.State
            If state = "TN" Or state = "TX" Then
                Await ShowFinancialData(currentHospital.HospitalId, state)
            Else
                Await ShowFinancialDataApi(currentHospital.CMSNum)
            End If
        Else
            lblHN.Text = "No hospital context"
        End If
    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles lblTotLandFbResult.Click

    End Sub

    Private Sub btnProfileFinancial_Click(sender As Object, e As EventArgs) Handles btnProfileFinancial.Click
        Me.Hide()
        Dim cmsNum As String = currentHospital.CMSNum
        Dim fullContext As HospitalContext = Results.GetHospitalContextByCMSNum(cmsNum)
        If fullContext IsNot Nothing Then
            Dim profileForm As New Profile(fullContext)
            profileForm.Show()
        Else
            MessageBox.Show("Could not reload full hospital context.")
        End If
    End Sub

    Private Sub btnFInIndFinancial_Click(sender As Object, e As EventArgs) Handles btnFInIndFinancial.Click
        Me.Hide()
        Dim finIndForm As New FinInd(currentHospital)
        finIndForm.Show()
    End Sub

    Private Sub btnQualityFinancial_Click(sender As Object, e As EventArgs) Handles btnQualityFinancial.Click
        Me.Hide()
        Dim qualityForm As New Quality(currentHospital)
        qualityForm.Show()
    End Sub

    Private Sub btnInpatientFinancial_Click(sender As Object, e As EventArgs) Handles btnInpatientFinancial.Click
        Me.Hide()
        Dim inpatientForm As New Inpatient(currentHospital)
        inpatientForm.Show()
    End Sub

    Private Sub btnDepartmentsFinancial_Click(sender As Object, e As EventArgs) Handles btnDepartmentsFinancial.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub

    Private Sub btnOutpatientFinancial_Click(sender As Object, e As EventArgs) Handles btnOutpatientFinancial.Click

    End Sub

    Private Sub lblNumMonthsPeriodResult_Click(sender As Object, e As EventArgs) Handles lblNumMonthsPeriodResult.Click

    End Sub
End Class
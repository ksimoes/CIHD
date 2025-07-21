Imports System.Data.SqlClient
Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class Financial
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    Public strCMSnum As String
    ' Call this method to load data for the selected hospital
    Public Async Function ShowFinancialData(hospitalId As Integer, state As String) As Task
        ' Step 1: Populate from Results.SelectedHospital
        Dim hosp = Results.SelectedHospital

        lblPedResult.Text = If(hosp.TotalDays > 0, hosp.TotalDays.ToString(), "N/A")
        lblCurAssetResult.Text = If(hosp.TotalCurrentAssets > 0, hosp.TotalCurrentAssets.ToString("N0"), "N/A")
        lblFixAssetsResult.Text = If(hosp.TotalAssets > 0, hosp.TotalAssets.ToString("N0"), "N/A")
        lblTotAssetsResult.Text = If(hosp.TotalAssets > 0, hosp.TotalAssets.ToString("N0"), "N/A")
        lblNetPatRevResult.Text = If(hosp.NetPatientRev > 0, hosp.NetPatientRev.ToString("N0"), "N/A")
        lblTotPatRevResult.Text = If(hosp.TotalPatientRev > 0, hosp.TotalPatientRev.ToString("N0"), "N/A")
        lblOutPatResult.Text = "N/A"
        lblInpRevResult.Text = "N/A"
        lblTotOperatingExpenseResult.Text = If(hosp.TotalOperatingExpense > 0, hosp.TotalOperatingExpense.ToString("N0"), "N/A")
        lblNetIncomeResult.Text = If(hosp.NetIncome > 0, hosp.NetIncome.ToString("N0"), "N/A")
        lblDepreciationExpenseResult.Text = If(hosp.DepreciationCost > 0, hosp.DepreciationCost.ToString("N0"), "N/A")
        lblCcResult.Text = If(hosp.CharityCost > 0, hosp.CharityCost.ToString("N0"), "N/A")
        lblUncompResult.Text = If(hosp.UncompensatedCost > 0, hosp.UncompensatedCost.ToString("N0"), "N/A")
        lblTotUcResult.Text = If(hosp.UncompensatedCost > 0, hosp.UncompensatedCost.ToString("N0"), "N/A")
        lblCurLiabilitiesRes.Text = If(hosp.TotalCurrentLiabilities > 0, hosp.TotalCurrentLiabilities.ToString("N0"), "N/A")
        lblLtResult.Text = If(hosp.TotalLongTermLiabilities > 0, hosp.TotalLongTermLiabilities.ToString("N0"), "N/A")
        lblTlResult.Text = If(hosp.TotalLiabilities > 0, hosp.TotalLiabilities.ToString("N0"), "N/A")
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
                            lblInpRevResult.Text = If(Not IsDBNull(reader("Total Gross Inpatient Revenue")), reader("Total Gross Inpatient Revenue").ToString(), lblInpRevResult.Text)
                            lblOutPatResult.Text = If(Not IsDBNull(reader("Total Gross Outpatient Revenue")), reader("Total Gross Outpatient Revenue").ToString(), lblOutPatResult.Text)
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
                                lblTotUcResult.Text = If(Not IsDBNull(reader2("Total Uncompensated Care")), reader2("Total Uncompensated Care").ToString(), lblTotUcResult.Text)
                                lblUncompResult.Text = If(Not IsDBNull(reader2("Bad Debt Charges")), reader2("Bad Debt Charges").ToString(), lblUncompResult.Text)
                                lblCcResult.Text = If(Not IsDBNull(reader2("Charity Charges")), reader2("Charity Charges").ToString(), lblCcResult.Text)
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
        Dim decDollarAmount As Decimal
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
                    lblCurAssetResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Total Current Assets")))
                    lblFixAssetsResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Total Fixed Assets")))
                    lblOtherAssetsResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Total Other Assets")))
                    lblTotAssetsResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Total Assets")))
                    lblNetPatRevResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Net Patient Revenue")))
                    lblTotPatRevResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Total Patient Revenue")))
                    lblOutPatResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Outpatient Revenue")))
                    lblInpRevResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Inpatient Revenue")))
                    lblTotOperatingExpenseResult.Text = FinInd.FormatCurrency(If(provider("Less Total Operating Expense") IsNot Nothing, CDec(provider("Less Total Operating Expense")).ToString("N"), "N/A"))
                    lblContractAllowanceResult.Text = FinInd.FormatCurrency(If(provider("Less Contractual Allowance and Discounts on Patients' Account") IsNot Nothing, CDec(provider("Less Contractual Allowance and Discounts on Patients' Account")).ToString("N"), "N/A"))
                    lblTotOtherIncomeResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Total Other Income")))
                    lblTotOtherExpensesResult.Text = FinInd.FormatCurrency(If(provider("Total Other Expenses") IsNot Nothing, provider("Total Other Expenses").ToString(), "N/A"))
                    lblNetIncomeResult.Text = FinInd.FormatCurrency(If(provider("Net Income") IsNot Nothing, CDec(provider("Net Income")).ToString("N"), "N/A"))
                    lblDepreciationExpenseResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Depreciation Cost")))
                    lblCcResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Cost of Charity Care")))
                    lblUncompResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Total Bad Debt Expense")))
                    lblTotUcResult.Text = FinInd.FormatCurrency(Search.CleanMeUp(provider("Cost of Uncompensated Care")))




                    lblCurLiabilitiesRes.Text = FinInd.FormatCurrency(If(provider("Total Current Liabilities") IsNot Nothing, provider("Total Current Liabilities").ToString(), "N/A"))
                    lblLtResult.Text = FinInd.FormatCurrency(If(provider("Total Long Term Liabilities") IsNot Nothing, CDec(provider("Total Long Term Liabilities")).ToString("N"), "N/A"))
                    lblTlResult.Text = FinInd.FormatCurrency(If(provider("Total Liabilities") IsNot Nothing, CDec(provider("Total Liabilities")).ToString("N"), "N/A"))
                    lblTotFbResult.Text = FinInd.FormatCurrency(If(provider("Total Fund Balances") IsNot Nothing, CDec(provider("Total Fund Balances")).ToString("N"), "N/A"))
                    lblTotLandFbResult.Text = FinInd.FormatCurrency(If(provider("Total Liabilities and Fund Balances") IsNot Nothing, CDec(provider("Total Liabilities and Fund Balances")).ToString(), "N/A"))

                    lblInpRevResult.Text = FinInd.FormatCurrency(If(provider("Inpatient Revenue") IsNot Nothing, CDec(provider("Inpatient Revenue")).ToString("N"), "N/A"))
                    'lblOutPatResult.Text = If(provider("Outpatient Revenue") IsNot Nothing, CDec(provider("Outpatient Revenue")).ToString("N"), "N/A")
                    'lblTotPatRevResult.Text = If(provider("Total Patient Revenue") IsNot Nothing, CDec(provider("Total Patient Revenue")).ToString("N"), "N/A")
                    '''lblCcResult.Text = If(provider("Cost of Charity Care") IsNot Nothing, CDec(provider("Cost of Charity Care")).ToString("N"), "N/A")
                    '''lblUncompResult.Text = If(provider("Total Bad Debt Expense") IsNot Nothing, CDec(provider("Total Bad Debt Expense")).ToString("N"), "N/A")
                    '''lblTotUcResult.Text = If(provider("Cost of Uncompensated Care") IsNot Nothing, CDec(provider("Cost of Uncompensated Care")).ToString("N"), "N/A")
                    'lblucpctResult.Text = If(provider("Uncompensated Care as % of GPR") IsNot Nothing, CDec(provider("Uncompensated Care as % of GPR")).ToString("P"), "N/A")


                    'lblNetPatRevResult.Text = If(provider("Net Patient Revenue") IsNot Nothing, CDec(provider("Net Patient Revenue")).ToString("N"), "N/A")
                    'lblCurLiabilitiesRes.Text = If(provider("Total Current Liabilities") IsNot Nothing, CDec(provider("Total Current Liabilities")).ToString("N"), "N/A")
                    'lblLtResult.Text = If(provider("Total Long Term Liabilities") IsNot Nothing, CDec(provider("Total Long Term Liabilities")).ToString("N"), "N/A")
                    'lblTlResult.Text = If(provider("Total Liabilities") IsNot Nothing, CDec(provider("Total Liabilities")).ToString("N"), "N/A")
                    'lblTotFbResult.Text = If(provider("Total Fund Balances") IsNot Nothing, CDec(provider("Total Fund Balances")).ToString("N"), "N/A")
                    'lblTotLandFbResult.Text = If(provider("Total Liabilities and Fund Balances") IsNot Nothing, CDec(provider("Total Liabilities and Fund Balances")).ToString("N"), "N/A")


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
        If Results.SelectedHospital IsNot Nothing Then
            Dim state = Results.SelectedHospital.State
            If state = "TN" Or state = "TX" Then
                Await ShowFinancialData(Results.SelectedHospital.HospitalId, state)
            Else
                Await ShowFinancialDataApi(Results.SelectedHospital.CMSNum)
            End If
        End If
    End Sub



    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles lblTotLandFbResult.Click

    End Sub

    Private Sub btnProfileFinancial_Click(sender As Object, e As EventArgs) Handles btnProfileFinancial.Click
        Me.Hide()
        Profile.ShowProfile(Results.SelectedHospital)
        Profile.Show()
    End Sub



    Private Sub btnFInIndFinancial_Click(sender As Object, e As EventArgs) Handles btnFInIndFinancial.Click
        Me.Hide()
        FinInd.Show()
        FinInd.ShowFinancialDataApi(strCMSnum)
    End Sub

    Private Sub btnQualityFinancial_Click(sender As Object, e As EventArgs) Handles btnQualityFinancial.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub btnInpatientFinancial_Click(sender As Object, e As EventArgs) Handles btnInpatientFinancial.Click
        Me.Hide()
        Inpatient.Show()
        Inpatient.LoadPatientOriginDataAsync(Results.SelectedHospital)
        Inpatient.LoadCeoDataAsync(Results.SelectedHospital)
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
End Class
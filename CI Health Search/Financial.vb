Imports System.Data.SqlClient
Imports Newtonsoft.Json.Linq

Public Class Financial
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    Public strCMSnum As String

    ' Entry point for showing financial data
    Public Async Function ShowFinancialData(hospitalId As Integer, state As String) As Task
        lblStatus.Text = "Loading financial data..."
        lblStatus.Visible = True

        Try
            If state = "TN" Or state = "TX" Then
                Await ShowSqlFinancialDataAsync(hospitalId, state)
            Else
                Await ShowFinancialDataApi(Results.SelectedHospital.CMSNum)
            End If
            lblStatus.Text = ""
        Catch ex As Exception
            SetAllFinancialLabels("No result")
            lblStatus.Text = "Error loading financial data. Please try again."
        End Try

        lblStatus.Visible = False
    End Function

    ' SQL financial data loading
    Private Async Function ShowSqlFinancialDataAsync(hospitalId As Integer, state As String) As Task
        Dim queryFinance As String = ""
        Dim queryCharity As String = ""

        Select Case state
            Case "TN"
                queryFinance = "SELECT * FROM tn.Financials WHERE LicenseNum = @LicenseNum"
            Case "TX"
                queryFinance = "SELECT * FROM tx.Finance WHERE id = @id"
                queryCharity = "SELECT * FROM tx.Charity WHERE id = @id"
        End Select

        Try
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
                            lblInpRevResult.Text = AppHelpers.SafeGet(reader, "Total Gross Inpatient Revenue")
                            lblOutPatResult.Text = AppHelpers.SafeGet(reader, "Total Gross Outpatient Revenue")
                        Else
                            SetAllFinancialLabels("No result")
                        End If
                    End Using
                End Using
            End Using

            If state = "TX" AndAlso Not String.IsNullOrEmpty(queryCharity) Then
                Using conn2 As New SqlConnection(connectionString)
                    Using cmd2 As New SqlCommand(queryCharity, conn2)
                        cmd2.Parameters.AddWithValue("@id", hospitalId)
                        conn2.Open()
                        Using reader2 = cmd2.ExecuteReader()
                            If reader2.Read() Then
                                lblTotUcResult.Text = AppHelpers.SafeGet(reader2, "Total Uncompensated Care")
                                lblUncompResult.Text = AppHelpers.SafeGet(reader2, "Bad Debt Charges")
                                lblCcResult.Text = AppHelpers.SafeGet(reader2, "Charity Charges")
                                lblucpctResult.Text = AppHelpers.SafeGet(reader2, "Uncompensated Care as pcnt of GPR")
                            Else
                                lblTotUcResult.Text = "No result"
                                lblUncompResult.Text = "No result"
                                lblCcResult.Text = "No result"
                                lblucpctResult.Text = "No result"
                            End If
                        End Using
                    End Using
                End Using
            End If
        Catch
            SetAllFinancialLabels("No result")
        End Try

        If state = "TX" Or state = "TN" Then
            Await ShowFinancialDataApi(Results.SelectedHospital.CMSNum)
        End If
    End Function

    ' API-based financial data for non-TN/TX
    Public Async Function ShowFinancialDataApi(cmsNum As String) As Task
        strCMSnum = cmsNum
        Try
            Dim apiUrl As String = ApiHelper.ApiUrls("MainProfileApi") & "?keyword=" & Uri.EscapeDataString(cmsNum) & "&size=1000"
            Dim data As JArray = Await ApiHelper.GetApiDataAsync(apiUrl)

            If data.Count > 0 Then
                Dim provider = data.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = cmsNum)
                If provider Is Nothing Then provider = data(0)

                lblPedResult.Text = AppHelpers.SafeGetDate(provider, "Fiscal Year End Date")
                lblCurAssetResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Current Assets"))
                lblFixAssetsResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Fixed Assets"))
                lblOtherAssetsResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Other Assets"))
                lblTotAssetsResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Assets"))
                lblNetPatRevResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Net Patient Revenue"))
                lblTotPatRevResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Patient Revenue"))
                lblOutPatResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Outpatient Revenue"))
                lblInpRevResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Inpatient Revenue"))
                lblTotOperatingExpenseResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Less Total Operating Expense"))
                lblContractAllowanceResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Less Contractual Allowance and Discounts on Patients' Account"))
                lblTotOtherIncomeResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Other Income"))
                lblTotOtherExpensesResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Other Expenses"))
                lblNetIncomeResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Net Income"))
                lblDepreciationExpenseResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Depreciation Cost"))
                lblCcResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Cost of Charity Care"))
                lblUncompResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Bad Debt Expense"))
                lblTotUcResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Cost of Uncompensated Care"))
                lblCurLiabilitiesRes.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Current Liabilities"))
                lblLtResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Long Term Liabilities"))
                lblTlResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Liabilities"))
                lblTotFbResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Fund Balances"))
                lblTotLandFbResult.Text = AppHelpers.FormatCurrency(AppHelpers.SafeGet(provider, "Total Liabilities and Fund Balances"))
            Else
                SetAllFinancialLabels("No result")
            End If
        Catch
            SetAllFinancialLabels("No result")
        End Try
    End Function

    ' Helper: Set all financial labels to a value
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
        lblTotOperatingExpenseResult.Text = val
        lblContractAllowanceResult.Text = val
        lblTotOtherIncomeResult.Text = val
        lblTotOtherExpensesResult.Text = val
        lblNetIncomeResult.Text = val
        lblDepreciationExpenseResult.Text = val
        lblCcResult.Text = val
        lblUncompResult.Text = val
        lblTotUcResult.Text = val
        lblucpctResult.Text = val
    End Sub

    ' Automatically load data when the form loads
    Private Async Sub Financial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Results.SelectedHospital IsNot Nothing Then
            Dim state = Results.SelectedHospital.State
            Await ShowFinancialData(Results.SelectedHospital.HospitalId, state)
        End If
    End Sub

    ' Navigation buttons
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
        Me.Hide()
        Outpatient.Show()
    End Sub
End Class
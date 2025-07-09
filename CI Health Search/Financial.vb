Imports System.Data.SqlClient
Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class Financial
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    Public strCMSnum As String
    ' Call this method to load data for the selected hospital
    Public Async Function ShowFinancialData(hospitalId As Integer, state As String) As Task
        Dim queryFinance As String = ""
        Dim queryCharity As String = ""

        ' Choose the correct table based on state
        Select Case state
            Case "TN"
                queryFinance = "SELECT * FROM tn.Financials WHERE LicenseNum = @LicenseNum"
            Case "TX"
                queryFinance = "SELECT * FROM tx.Finance WHERE id = @id"
                queryCharity = "SELECT * FROM tx.Charity WHERE id = @id"
            Case Else
                ' For all other states, use API
                Await ShowFinancialDataApi(Results.SelectedHospital.CMSNum)
                Exit Function
        End Select

        ' --- Query 1: Finance Table ---
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
                        lblInpRevResult.Text = If(Not IsDBNull(reader("Total Gross Inpatient Revenue")), reader("Total Gross Inpatient Revenue").ToString(), "N/A")
                        lblOutPatResult.Text = If(Not IsDBNull(reader("Total Gross Outpatient Revenue")), reader("Total Gross Outpatient Revenue").ToString(), "N/A")
                        ' Add more fields as needed for TN/TX
                    Else
                        lblPedResult.Text = "No result"
                        lblNumMonthsPeriodResult.Text = "No result"
                    End If
                End Using
                conn.Close()
            End Using
        End Using

        '--- Query 2 Charity Table(TX only) - --
        If state = "TX" Then
            Using conn2 As New SqlConnection(connectionString)
                Using cmd2 As New SqlCommand(queryCharity, conn2)
                    cmd2.Parameters.AddWithValue("@id", hospitalId)
                    conn2.Open()
                    Using reader2 = cmd2.ExecuteReader()
                        If reader2.Read() Then
                            lblTotUcResult.Text = If(Not IsDBNull(reader2("Total Uncompensated Care")), reader2("Total Uncompensated Care").ToString(), "N/A")
                            lblUncompResult.Text = If(Not IsDBNull(reader2("Bad Debt Charges")), reader2("Bad Debt Charges").ToString(), "N/A")
                            lblCcResult.Text = If(Not IsDBNull(reader2("Charity Charges")), reader2("Charity Charges").ToString(), "N/A")
                            lblucpctResult.Text = If(Not IsDBNull(reader2("Uncompensated Care as pcnt of GPR")), reader2("Uncompensated Care as pcnt of GPR").ToString(), "N/A")
                        Else
                            lblTotUcResult.Text = "No result"
                            lblUncompResult.Text = "No result"
                            lblCcResult.Text = "No result"
                            lblucpctResult.Text = "No result"
                        End If
                    End Using
                End Using
            End Using
            ShowFinancialDataApi(hospitalId)
        End If
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
                    lblCurAssetResult.Text = Search.CleanMeUp(provider("Total Current Assets"))
                    lblFixAssetsResult.Text = Search.CleanMeUp(provider("Total Fixed Assets"))
                    lblOtherAssetsResult.Text = Search.CleanMeUp(provider("Total Other Assets"))
                    lblTotAssetsResult.Text = Search.CleanMeUp(provider("Total Assets"))
                    lblNetPatRevResult.Text = Search.CleanMeUp(provider("Net Patient Revenue"))
                    lblTotPatRevResult.Text = Search.CleanMeUp(provider("Total Patient Revenue"))
                    lblOutPatResult.Text = Search.CleanMeUp(provider("Outpatient Revenue"))
                    lblInpRevResult.Text = Search.CleanMeUp(provider("Inpatient Revenue"))
                    lblTotOperatingExpenseResult.Text = If(provider("Less Total Operating Expense") IsNot Nothing, CDec(provider("Less Total Operating Expense")).ToString("N"), "N/A")
                    lblContractAllowanceResult.Text = If(provider("Less Contractual Allowance and Discounts on Patients' Account") IsNot Nothing, CDec(provider("Less Contractual Allowance and Discounts on Patients' Account")).ToString("N"), "N/A")
                    lblTotOtherIncomeResult.Text = Search.CleanMeUp(provider("Total Other Income"))
                    lblTotOtherExpensesResult.Text = If(provider("Total Other Expenses") IsNot Nothing, provider("Total Other Expenses").ToString(), "N/A")
                    lblNetIncomeResult.Text = If(provider("Net Income") IsNot Nothing, CDec(provider("Net Income")).ToString("N"), "N/A")
                    lblDepreciationExpenseResult.Text = Search.CleanMeUp(provider("Depreciation Cost"))
                    lblCcResult.Text = Search.CleanMeUp(provider("Cost of Charity Care"))
                    lblUncompResult.Text = Search.CleanMeUp(provider("Total Bad Debt Expense"))
                    lblTotUcResult.Text = Search.CleanMeUp(provider("Cost of Uncompensated Care"))




                    lblCurLiabilitiesRes.Text = If(provider("Total Current Liabilities") IsNot Nothing, provider("Total Current Liabilities").ToString(), "N/A")
                    lblLtResult.Text = If(provider("Total Long Term Liabilities") IsNot Nothing, CDec(provider("Total Long Term Liabilities")).ToString("N"), "N/A")
                    lblTlResult.Text = If(provider("Total Liabilities") IsNot Nothing, CDec(provider("Total Liabilities")).ToString("N"), "N/A")
                    lblTotFbResult.Text = If(provider("Total Fund Balances") IsNot Nothing, CDec(provider("Total Fund Balances")).ToString("N"), "N/A")
                    lblTotLandFbResult.Text = If(provider("Total Liabilities and Fund Balances") IsNot Nothing, CDec(provider("Total Liabilities and Fund Balances")).ToString(), "N/A")

                    lblInpRevResult.Text = If(provider("Inpatient Revenue") IsNot Nothing, CDec(provider("Inpatient Revenue")).ToString("N"), "N/A")
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
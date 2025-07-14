Imports System.Net.Http
Imports Newtonsoft.Json.Linq
Imports System.Data

Public Class Inpatient
    ' NOTE MEDICARE_PROV_NUM = CCN Number
    Dim strURLPatientOrigin As String = "https://data.cms.gov/data-api/v1/dataset/8708ca8b-8636-44ed-8303-724cbfaf78ad/data"
    Dim strURLPatientOrigin2019 As String = "https://data.cms.gov/data-api/v1/dataset/2713ba99-c59e-4b25-9a3d-3661d35988da/data"
    Dim strURLPatientOrigin2023 As String = "https://data.cms.gov/data-api/v1/dataset/7f749f00-bfa9-4377-9a98-90c15cacc2f3/data"
    Dim strUrlCeoApi As String = "https://data.cms.gov/data-api/v1/dataset/029c119f-f79c-49be-9100-344d31d10344/data"
    Dim strUrlNewApi As String = "https://data.cms.gov/data-api/v1/dataset/690ddc6c-2767-4618-b277-420ffb2bf27c/data"

    ' Call this method to load data into the DataGridView
    Public Async Function LoadPatientOriginDataAsync(myHospital As HospitalContext) As Task
        Dim dtCurrPatientZipsByHosp As DataTable = Await GetTablefromAPI(strURLPatientOrigin + "?filter[MEDICARE_PROV_NUM]=" + myHospital.CMSNum)
        Dim dtPrevYearPatientZipsByHosp As DataTable = Await GetTablefromAPI(strURLPatientOrigin2023 + "?filter[MEDICARE_PROV_NUM]=" + myHospital.CMSNum)
        Dim dtMasterCurrent As New DataTable()
        dtMasterCurrent.Columns.Add("CMS Number")
        dtMasterCurrent.Columns.Add("ZIP")
        dtMasterCurrent.Columns.Add("Discharges")
        dtMasterCurrent.Columns.Add("Days of Care")
        dtMasterCurrent.Columns.Add("Charges")
        dtMasterCurrent.Columns.Add("Discharge Change")
        dtMasterCurrent.Columns.Add("Market Share")
        dtMasterCurrent.Columns.Add("Market Share 5 Years Prior")

        ' Sort by TOTAL_CASES descending and take top 10
        Dim topRows = dtCurrPatientZipsByHosp.AsEnumerable() _
        .OrderByDescending(Function(r) If(IsNumeric(r("TOTAL_CASES")), Convert.ToInt32(r("TOTAL_CASES")), 0)) _
        .Take(10).ToList()

        ' Prepare all the tasks for parallel execution
        Dim zipCodes = topRows.Select(Function(r) r("ZIP_CD_OF_RESIDENCE").ToString()).ToList()
        Dim zipTasks = zipCodes.Select(Function(zip) GetTablefromAPI(strURLPatientOrigin + "?filter[ZIP_CD_OF_RESIDENCE]=" + zip)).ToArray()
        Dim zipTasks5Yr = zipCodes.Select(Function(zip) GetTablefromAPI(strURLPatientOrigin2019 + "?filter[ZIP_CD_OF_RESIDENCE]=" + zip)).ToArray()
        Dim allZipResults = Await Task.WhenAll(zipTasks)
        Dim allZipResults5Yr = Await Task.WhenAll(zipTasks5Yr)

        For i As Integer = 0 To topRows.Count - 1
            Dim datarow = topRows(i)
            Dim zipCode = datarow("ZIP_CD_OF_RESIDENCE").ToString()
            Dim hospitalDischarges As Integer = If(IsNumeric(datarow("TOTAL_CASES")), Convert.ToInt32(datarow("TOTAL_CASES")), 0)
            Dim dtAllHospitalsForZip = allZipResults(i)
            Dim totalDischargesFromZip As Integer = dtAllHospitalsForZip.AsEnumerable().Sum(Function(r) If(IsNumeric(r("TOTAL_CASES")), Convert.ToInt32(r("TOTAL_CASES")), 0))

            ' Market Share (current)
            Dim marketShare As String = "0.00"
            If totalDischargesFromZip > 0 Then
                marketShare = ((hospitalDischarges / totalDischargesFromZip) * 100).ToString("N2")
            End If

            ' Discharge Change (compare to prior year)
            Dim prevYearRow = dtPrevYearPatientZipsByHosp.AsEnumerable().FirstOrDefault(Function(r) r("ZIP_CD_OF_RESIDENCE").ToString() = zipCode)
            Dim prevYearDischarges As Integer = If(prevYearRow IsNot Nothing AndAlso IsNumeric(prevYearRow("TOTAL_CASES")), Convert.ToInt32(prevYearRow("TOTAL_CASES")), 0)
            Dim dischargeChange As String = "N/A"
            If prevYearDischarges > 0 Then
                dischargeChange = (((hospitalDischarges - prevYearDischarges) / prevYearDischarges) * 100).ToString("N2")
            End If

            ' Market Share 5 Years Prior
            Dim dtAllHospitalsForZip5Yr = allZipResults5Yr(i)
            Dim totalDischargesFromZip5Yr As Integer = dtAllHospitalsForZip5Yr.AsEnumerable().Sum(Function(r) If(IsNumeric(r("TOTAL_CASES")), Convert.ToInt32(r("TOTAL_CASES")), 0))
            ' Find this hospital's discharges for this ZIP 5 years ago
            Dim hosp5YrRow = dtAllHospitalsForZip5Yr.AsEnumerable().FirstOrDefault(Function(r) r("MEDICARE_PROV_NUM").ToString() = myHospital.CMSNum)
            Dim hospitalDischarges5Yr As Integer = If(hosp5YrRow IsNot Nothing AndAlso IsNumeric(hosp5YrRow("TOTAL_CASES")), Convert.ToInt32(hosp5YrRow("TOTAL_CASES")), 0)
            Dim marketShare5Yr As String = "0.00"
            If totalDischargesFromZip5Yr > 0 Then
                marketShare5Yr = ((hospitalDischarges5Yr / totalDischargesFromZip5Yr) * 100).ToString("N2")
            End If

            dtMasterCurrent.Rows.Add(
            myHospital.CMSNum,
            zipCode,
            datarow("TOTAL_CASES").ToString(),
            datarow("TOTAL_DAYS_OF_CARE").ToString(),
            datarow("TOTAL_CHARGES").ToString(),
            dischargeChange,
            marketShare,
            marketShare5Yr
        )
        Next

        dgvPatientOrigin.DataSource = dtMasterCurrent
    End Function

    Public Async Function LoadCeoDataAsync(myHospital As HospitalContext) As Task
        Dim dtCeo As DataTable = Await GetTablefromAPI(strUrlCeoApi + "?filter[Organization Name]=" + myHospital.Name)
        Dim dtMasterCeo As New DataTable()
        dtMasterCeo.Columns.Add("Organization Name")
        dtMasterCeo.Columns.Add("First Name - Owner")
        dtMasterCeo.Columns.Add("Payment")
        dtMasterCeo.Columns.Add("Cost")
        dtMasterCeo.Columns.Add("CMI")

        For Each row As DataRow In dtCeo.Rows
            dtMasterCeo.Rows.Add(
                        myHospital.CMSNum,
               row("Organization Name").ToString(),
               row("First Name - Owner").ToString())


        Next







        dgvCeo.DataSource = dtMasterCeo
    End Function

    Public Async Function LoadNewApiDataAsync(apiUrl As String) As Task
        Dim rawDt As DataTable = Await GetTablefromAPI(apiUrl)
        Dim dtCustom As New DataTable()


        ' Customize columns
        dtCustom.Columns.Add("Provider Name")
        dtCustom.Columns.Add("DRG Description")
        dtCustom.Columns.Add("Avg Charge")
        dtCustom.Columns.Add("Avg Cost")
        dtCustom.Columns.Add("Avg Payment")

        For Each row As DataRow In rawDt.Rows
            dtCustom.Rows.Add(
            row("Rndrng_Prvdr_Org_Name").ToString(),
            row("DRG_Desc").ToString(),
            row("Avg_Submtd_Cvrd_chrg").ToString(),
            row("Avg_Tot_Pymt_Amt").ToString())
            row("Avg_Mdcr_Pymt_Amt").ToString()

        Next

        dgvNewApiTable.DataSource = dtCustom
        FormatNewApiTable()
    End Function

    Private Sub FormatNewApiTable()
        With dgvNewApiTable
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .RowHeadersVisible = False
        End With
    End Sub
    Public Async Function GetTablefromAPI(strAPI As String) As Task(Of DataTable)
        Dim dt As New DataTable()
        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync(strAPI)
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)

                If data.Count > 0 Then
                    ' Create columns based on the first record
                    For Each prop In CType(data(0), JObject).Properties()
                        dt.Columns.Add(prop.Name)
                    Next

                    ' Add rows
                    For Each item As JObject In data
                        Dim row As DataRow = dt.NewRow()
                        For Each prop In item.Properties()
                            row(prop.Name) = prop.Value.ToString()
                        Next
                        dt.Rows.Add(row)
                    Next
                End If
            End If
            Return dt
        End Using
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileInpatient.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentsInpatient.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialInpatient.Click
        Me.Hide()
        Financial.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndInpatient.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityInpatient.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientInpatient.Click
        Me.Hide()
        Outpatient.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub

    Private Sub Label14_Click(sender As Object, e As EventArgs) Handles Label14.Click
    End Sub

    Private Sub dgvPatientOrigin_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPatientOrigin.CellContentClick
    End Sub

    Private Async Sub Inpatient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadPatientOriginDataAsync(Results.SelectedHospital)
        Await LoadCeoDataAsync(Results.SelectedHospital)
        Dim apiUrl As String = strUrlNewApi & "?filter[Rndrng_Prvdr_CCN]=" & Results.SelectedHospital.CMSNum
        Await LoadNewApiDataAsync(apiUrl) ' <-- Use the filtered URL here!
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvNewApiTable.CellContentClick

    End Sub
End Class
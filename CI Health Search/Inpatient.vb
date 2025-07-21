Imports Newtonsoft.Json.Linq
Imports System.Data

Public Class Inpatient
    ' Load patient origin data into the DataGridView
    Public Async Function LoadPatientOriginDataAsync(myHospital As HospitalContext) As Task
        lblstatus.Text = "Loading patient origin data..."
        lblstatus.Visible = True

        Try
            Dim urlCurrent = ApiHelper.ApiUrls("PatientOrigin") & "?filter[MEDICARE_PROV_NUM]=" & myHospital.CMSNum
            Dim urlPrevYear = ApiHelper.ApiUrls("PatientOrigin2023") & "?filter[MEDICARE_PROV_NUM]=" & myHospital.CMSNum
            Dim dtCurrPatientZipsByHosp As DataTable = Await ApiHelper.GetTableFromApiAsync(urlCurrent)
            Dim dtPrevYearPatientZipsByHosp As DataTable = Await ApiHelper.GetTableFromApiAsync(urlPrevYear)

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
            Dim topRows = dtCurrPatientZipsByHosp.AsEnumerable().
                OrderByDescending(Function(r) If(IsNumeric(r("TOTAL_CASES")), Convert.ToInt32(r("TOTAL_CASES")), 0)).
                Take(10).ToList()

            ' Prepare all the tasks for parallel execution
            Dim zipCodes = topRows.Select(Function(r) r("ZIP_CD_OF_RESIDENCE").ToString()).ToList()
            Dim zipTasks = zipCodes.Select(Function(zip) ApiHelper.GetTableFromApiAsync(ApiHelper.ApiUrls("PatientOrigin") & "?filter[ZIP_CD_OF_RESIDENCE]=" & zip)).ToArray()
            Dim zipTasks5Yr = zipCodes.Select(Function(zip) ApiHelper.GetTableFromApiAsync(ApiHelper.ApiUrls("PatientOrigin2019") & "?filter[ZIP_CD_OF_RESIDENCE]=" & zip)).ToArray()
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
            lblstatus.Text = ""
        Catch ex As Exception
            lblstatus.Text = "Error loading patient origin data. Please try again."
        End Try

        lblstatus.Visible = False
    End Function

    ' CEO Data
    Public Async Function LoadCeoDataAsync(myHospital As HospitalContext) As Task
        lblstatus.Text = "Loading CEO data..."
        lblstatus.Visible = True

        Try
            Dim url = ApiHelper.ApiUrls("CeoApi") & "?filter[Organization Name]=" & myHospital.Name
            Dim dtCeo As DataTable = Await ApiHelper.GetTableFromApiAsync(url)
            Dim dtMasterCeo As New DataTable()
            dtMasterCeo.Columns.Add("CMS Number")
            dtMasterCeo.Columns.Add("Organization Name")
            dtMasterCeo.Columns.Add("First Name - Owner")

            For Each row As DataRow In dtCeo.Rows
                dtMasterCeo.Rows.Add(
                    myHospital.CMSNum,
                    AppHelpers.SafeGet(row, "Organization Name"),
                    AppHelpers.SafeGet(row, "First Name - Owner")
                )
            Next

            dgvCeo.DataSource = dtMasterCeo
            lblstatus.Text = ""
        Catch ex As Exception
            lblstatus.Text = "Error loading CEO data. Please try again."
        End Try

        lblstatus.Visible = False
    End Function

    ' New API Data
    Public Async Function LoadNewApiDataAsync(apiUrl As String) As Task
        lblstatus.Text = "Loading DRG data..."
        lblstatus.Visible = True

        Try
            Dim rawDt As DataTable = Await ApiHelper.GetTableFromApiAsync(apiUrl)
            Dim dtCustom As New DataTable()
            dtCustom.Columns.Add("Provider Name")
            dtCustom.Columns.Add("DRG Description")
            dtCustom.Columns.Add("Avg Charge")
            dtCustom.Columns.Add("Avg Cost")
            dtCustom.Columns.Add("Avg Payment")

            For Each row As DataRow In rawDt.Rows
                dtCustom.Rows.Add(
                    AppHelpers.SafeGet(row, "Rndrng_Prvdr_Org_Name"),
                    AppHelpers.SafeGet(row, "DRG_Desc"),
                    AppHelpers.SafeGet(row, "Avg_Submtd_Cvrd_chrg"),
                    AppHelpers.SafeGet(row, "Avg_Tot_Pymt_Amt"),
                    AppHelpers.SafeGet(row, "Avg_Mdcr_Pymt_Amt")
                )
            Next

            dgvNewApiTable.DataSource = dtCustom
            FormatNewApiTable()
            lblstatus.Text = ""
        Catch ex As Exception
            lblstatus.Text = "Error loading DRG data. Please try again."
        End Try

        lblstatus.Visible = False
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

    ' Navigation and form load
    Private Async Sub Inpatient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadPatientOriginDataAsync(Results.SelectedHospital)
        Await LoadCeoDataAsync(Results.SelectedHospital)
        Dim apiUrl As String = ApiHelper.ApiUrls("NewApi") & "?filter[Rndrng_Prvdr_CCN]=" & Results.SelectedHospital.CMSNum
        Await LoadNewApiDataAsync(apiUrl)
    End Sub

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
End Class
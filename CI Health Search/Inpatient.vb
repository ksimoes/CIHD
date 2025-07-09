Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class Inpatient
    ' NOTE MEDICARE_PROV_NUM = CCN Number
    Dim strURLPatientOrigin As String = "https://data.cms.gov/data-api/v1/dataset/8708ca8b-8636-44ed-8303-724cbfaf78ad/data"
    Dim strURLPatientOrigin2019 As String = "https://data.cms.gov/data-api/v1/dataset/2713ba99-c59e-4b25-9a3d-3661d35988da/data"
    Dim strURLPatientOrigin2023 As String = "https://data.cms.gov/data-api/v1/dataset/7f749f00-bfa9-4377-9a98-90c15cacc2f3/data"


    ' Call this method to load data into the DataGridView
    Public Async Function LoadPatientOriginDataAsync(myHospital As HospitalContext) As Task
        Dim dtCurrPatientZipsByHosp, dtPrevPatientYear, dtFiveYrPatient, dtCurrRegional, dtMasterCurrent, dtProfileZips, dtPrevRegional, dtFIVERegional As New DataTable()
        Dim curHospDischarge, curRegionDischarge As Integer
        Dim disChargeChange As Decimal

        Dim curDatarow As DataRow
        dtMasterCurrent.Columns.Add("ZIP")
        dtMasterCurrent.Columns.Add("Discharges")
        dtMasterCurrent.Columns.Add("Days of Care")
        dtMasterCurrent.Columns.Add("Charges")
        dtMasterCurrent.Columns.Add("Discharge Change") 'This year over last year - 1*100
        dtMasterCurrent.Columns.Add("Market Share")
        dtMasterCurrent.Columns.Add("Market Share 5 Years Prior")
        'Pulls back zip codes of all patients that were discharged from the hospital in the current year
        dtCurrPatientZipsByHosp = Await GetTablefromAPI(strURLPatientOrigin + "?filter[MEDICARE_PROV_NUM]=" + myHospital.CMSNum)
        dtPrevPatientYear = Await GetTablefromAPI(strURLPatientOrigin2023 + "?filter[MEDICARE_PROV_NUM]=" + myHospital.CMSNum)
        dtFiveYrPatient = Await GetTablefromAPI(strURLPatientOrigin2019 + "?filter[MEDICARE_PROV_NUM]=" + myHospital.CMSNum)

        For Each datarow In dtCurrPatientZipsByHosp.Rows 'Loop through each row in the current year patient zip data
            dtCurrRegional = Await GetTablefromAPI(strURLPatientOrigin + "?filter[ZIP_CD_OF_RESIDENCE]=" + datarow("ZIP_CD_OF_RESIDENCE").ToString())
            For Each zipDataRow In dtCurrRegional.Rows
                If IsNumeric(zipDataRow("ZIP_CD_OF_RESIDENCE")) Then
                    curRegionDischarge += Convert.ToInt32(zipDataRow("TOTAL_CASES"))
                End If

                'DISCHARGE CHANGE CALCULATION
                disChargeChange = dtCurrPatientZipsByHosp.Rows(0)("TOTAL_CASES") / dtPrevPatientYear.Rows(0)("TOTAL_CASES")  'Calculate the percentage change in discharges
                If disChargeChange = 0 Then
                    disChargeChange = 0
                Else
                    disChargeChange = (disChargeChange - 1) * 100 'Calculate the percentage change
                End If

                'GET DAYS OF CARE AND CHARGES

                'Get Market Share

                'get market share for 5 years prior

            Next
            dtMasterCurrent.Rows.Add("", "", "", "", disChargeChange, "", "") 'Add a new row to the master table with the current data)
        Next


        dgvPatientOrigin.DataSource = dtMasterCurrent
        'The percentage increase Or decrease in Medicare discharges Is calculated by comparison with the hospital's discharges during the prior year.
        'The Market share Is calculated by comparison with all other hospitals having Medicare discharges from the ZIP code (i.e. the hospital's discharges from a ZIP code as a percentage of total discharges to all hospitals from the ZIP code).
        'The Market share 5-years prior Is calculated In the same fashion As market share, but Using Service Area File data from five years prior. For example, 2014 data would be used for this field when 2019 data Is the most current period reported.


        ' Bind to DataGridView
        dgvPatientOrigin.DataSource = dtCurrPatientZipsByHosp
    End Function

    Public Async Function GetTablefromAPI(strAPI As String) As Task(Of DataTable)
        Dim dt As New DataTable()
        Using client As New HttpClient()
            'Pulls bacc data based on CCN/CMS 
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
    End Sub

End Class
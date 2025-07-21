Imports Newtonsoft.Json.Linq
Imports System.Data.SqlClient
Imports System.Net.Http

Public Class Profile
    Private ReadOnly FacilityTypeMap As New Dictionary(Of String, String) From {
        {"STH", "Short-term"},
        {"FQHC", "Federally Qualified Health Center"},
        {"ADH", "Alcohol/Drug Hospitals"},
        {"MAF", "Medical Assistance Facilitie"},
        {"CAH", "Critical Access Hospital"},
        {"CCMHC", "Continuation of Community Mental Health Center"},
        {"HOS", "Hospice"},
        {"RNMHC", "Religious Non-medical Health Care Institution"},
        {"LTCH", "Long-Term Care Hospital"},
        {"HBRDF", "Hospital-based Renal Dialysis Facility"},
        {"IDRF", "Independent Renal Dialysis Facility"},
        {"ISPRDF", "Independent Special Purpose Renal Dialysis Facility"},
        {"FTH", "Formerly Tuberculosis Hospital"},
        {"RH", "Rehabilitation Hospital"},
        {"HHA", "Home Health Agency"},
        {"CCORF", "Continuation of Comprehensive Outpatient Rehabilitation Facility"},
        {"CH", "Children’s Hospital"},
        {"RHC", "Continuation of Rural Health Clinic"},
        {"HBSRDF", "Hospital-based Special Purpose Renal Dialysis Facility"},
        {"PH", "Psychiatric Hospital"},
        {"CORF", "Comprehensive Outpatient Rehabilitation Facility"},
        {"CMHC", "Community Mental Health Center"},
        {"SNF", "Skilled Nursing Facility"},
        {"OPTS", "Outpatient Physical Therapy Services"},
        {"NR", "Numbers Reserved"},
        {"CHHA", "Continuation of Home Health Agency"},
        {"TC", "Transplant Center"},
        {"RFU", "Reserved for Future Use"}
    }
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    Public strCMSnum As String

    Public Async Sub ShowProfile(foundHospital As HospitalContext)
        Dim useSql As Boolean = (foundHospital.State = "TN" Or foundHospital.State = "TX")
        Results.SelectedHospital = foundHospital
        strCMSnum = foundHospital.CMSNum

        If useSql Then
            Dim queryProfile As String = If(foundHospital.State = "TN",
                "SELECT * FROM tn.AdminCon WHERE LicenseNum = @LicenseNum",
                "SELECT * FROM tx.Utilization WHERE id = @id")
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(queryProfile, conn)
                    If foundHospital.State = "TN" Then
                        cmd.Parameters.AddWithValue("@LicenseNum", foundHospital.HospitalId)
                    ElseIf foundHospital.State = "TX" Then
                        cmd.Parameters.AddWithValue("@id", foundHospital.HospitalId)
                    End If
                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            lblNameAddressResult.Text = SafeGet(reader, "Facility Name")
                            lblPhoneNumResult.Text = SafeGet(reader, "Phone")
                            lblCeoPresResult.Text = SafeGet(reader, "Admin")
                            lblCountyFipsResult.Text = SafeGet(reader, "County")
                            lblTotalPatientDaysResult.Text = SafeGet(reader, "Inpatient Days")
                        Else
                            lblNameAddressResult.Text = "No result"
                            lblPhoneNumResult.Text = "No result"
                            lblCeoPresResult.Text = "No result"
                            lblCountyFipsResult.Text = "No result"
                            lblTotalPatientDaysResult.Text = "No result"
                        End If
                        Await ShowApiProfileAsync(foundHospital)
                    End Using
                End Using
            End Using
        Else
            Await ShowApiProfileAsync(foundHospital)
        End If

        ' Fetch and display NPI from NPPES API (by NPI if available, else by name/state)
        Dim npiData As JObject = Await FetchNpiDataAsync(foundHospital.NPI, foundHospital.Name, foundHospital.State)
        If npiData IsNot Nothing Then
            lblNpiResult.Text = npiData("number")?.ToString() ' "NPI not found"'
            foundHospital.NPI = npiData("number")?.ToString()
            ' Optionally display more NPPES fields here
            ' Example:
            ' lblNpiOrgName.Text = npiData("basic")?("organization_name")?.ToString()
            ' lblNpiAddress.Text = npiData("addresses")?(0)?("address_1")?.ToString()
        Else
            lblNpiResult.Text = "NPI not found"
        End If
    End Sub

    ' Helper function to safely get column value by name
    Private Function SafeGet(reader As SqlDataReader, columnName As String) As String
        Try
            Dim ordinal = reader.GetOrdinal(columnName)
            If Not reader.IsDBNull(ordinal) Then
                Return reader.GetValue(ordinal).ToString()
            End If
        Catch ex As IndexOutOfRangeException
            ' Column does not exist
        End Try
        Return "N/A"
    End Function

    Public Async Function ShowApiProfileAsync(foundHosp As HospitalContext) As Task
        Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?"
        Dim mainURL As String = "https://data.cms.gov/data-api/v1/dataset/8143cbc7-484f-438b-9dfa-2e81d5d6a1ed/data?"
        Dim filters As New List(Of String)
        If Not String.IsNullOrEmpty(foundHosp.CMSNum) Then filters.Add("keyword=" & Uri.EscapeDataString(foundHosp.CMSNum))
        If Not String.IsNullOrEmpty(foundHosp.NPI) Then filters.Add("keyword=" & Uri.EscapeDataString(foundHosp.NPI))
        apiUrl &= String.Join("&", filters)
        filters.Add("size=1000")
        strCMSnum = foundHosp.CMSNum
        Dim myArray As JArray = GetAPIArray(apiUrl)

        If myArray.Count > 0 Then
            Dim provider = myArray.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = foundHosp.CMSNum)
            If provider Is Nothing Then provider = myArray(0)

            Dim facilityAcronym As String = If(provider("CCN Facility Type") IsNot Nothing, provider("CCN Facility Type").ToString(), "N/A")
            lblFacilityResult.Text = If(FacilityTypeMap.ContainsKey(facilityAcronym), FacilityTypeMap(facilityAcronym), facilityAcronym)

            lblCountyFipsResult.Text = If(provider("County Name") IsNot Nothing, provider("County Name").ToString(), "N/A")
            If lblCountyFipsResult.Text.Equals("N/A") Then lblCountyFipsResult.Text = If(provider("County") IsNot Nothing, provider("County").ToString(), "N/A")

            lblCmsCertNumProfileResult.Text = foundHosp.CMSNum
            lblNameAddressResult.Text = foundHosp.Name
            lbladdy.Text = foundHosp.Address
            lblCbsaResult.Text = foundHosp.CBSAnum
            lblGeneralMedSurgBedsResult.Text = foundHosp.NumOfBeds.ToString()
            lblTotalEmployeesResult.Text = foundHosp.NumOfEmployees
            lblTotalDischargesResult.Text = foundHosp.TotalDischarges.ToString()
            lblCmsUrbRurDesigResult.Text = foundHosp.RuralOUrban
            If lblCmsUrbRurDesigResult.Text = "R" Then
                lblRuralReferralResult.Text = "Y"
            ElseIf lblCmsUrbRurDesigResult.Text = "U" Then
                lblRuralReferralResult.Text = "N"
            Else
                lblRuralReferralResult.Text = ""
            End If
            lblZipCodeResult.Text = foundHosp.Zip

            lblTotalPatientRevenueResult.Text = If(provider("Total Patient Revenue") IsNot Nothing, provider("Total Patient Revenue").ToString(), "N/A")
            lblTypeControlResult.Text = If(provider("Type of Control") IsNot Nothing, provider("Type of Control").ToString(), "N/A")
            lblZipCodeResult.Text = If(provider("Zip Code") IsNot Nothing, provider("Zip Code").ToString(), "N/A")
            lblTotalPatientDaysResult.Text = If(provider("Hospital Total Days (V + XVIII + XIX + Unknown) For Adults & Peds ") IsNot Nothing, provider("Hospital Total Days (V + XVIII + XIX + Unknown) For Adults & Peds ").ToString(), "N/A")
        Else
            lblCmsCertNumProfileResult.Text = "No result"
            lblNameAddressResult.Text = "No result"
            lbladdy.Text = "No result"
            lblCountyFipsResult.Text = "No result"
            lblCbsaResult.Text = "No result"
            lblGeneralMedSurgBedsResult.Text = "No result"
            lblTotalEmployeesResult.Text = "No result"
            lblTotalDischargesResult.Text = "No result"
            lblCmsUrbRurDesigResult.Text = "No result"
            lblZipCodeResult.Text = "No result"
        End If

        Dim dataobject As JArray = GetAPIArray(mainURL & "filter[PRVDR_NUM]=" & foundHosp.CMSNum & "&offset=0&size=1")
        If dataobject.Count > 0 Then
            Dim provider = dataobject.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = foundHosp.CMSNum)
            If provider Is Nothing Then provider = dataobject(0)
            lblPhoneNumResult.Text = If(provider("PHNE_NUM") IsNot Nothing, provider("PHNE_NUM").ToString(), "N/A")
            lblCbsaResult.Text = If(provider("CBSA_CD") IsNot Nothing, provider("CBSA_CD").ToString(), "N/A")
            lblMedicareCertifiedBedsResult.Text = Search.CleanMeUp(provider("MDCR_SNF_BED_CNT"))
        End If
    End Function

    ' Fetch NPI from NPPES API (by NPI if available, else by name/state)
    Public Async Function FetchNpiDataAsync(npi As String, hospitalName As String, state As String) As Task(Of JObject)
        Dim apiUrl As String
        If Not String.IsNullOrWhiteSpace(npi) Then
            apiUrl = $"https://npiregistry.cms.hhs.gov/api/?version=2.1&number={Uri.EscapeDataString(npi)}"
        Else
            apiUrl = $"https://npiregistry.cms.hhs.gov/api/?version=2.1&enumeration_type=NPI-2&organization_name={Uri.EscapeDataString(hospitalName)}&state={state}&limit=1"
        End If
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                If obj("results") IsNot Nothing AndAlso obj("results").HasValues Then
                    Return obj("results")(0)
                End If
            End If
        End Using
        Return Nothing
    End Function

    Public Function GetAPIArray(strAPIurl As String) As JArray
        Dim client As New HttpClient()
        Dim response As HttpResponseMessage = client.GetAsync(strAPIurl).Result
        If response.IsSuccessStatusCode Then
            Dim jsonString As String = response.Content.ReadAsStringAsync().Result
            Return JArray.Parse(jsonString)
        Else
            Throw New Exception("API call failed with status: " & response.StatusCode.ToString())
        End If
    End Function

    ' Navigation buttons
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentProfile.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialProfile.Click
        Me.Hide()
        Financial.Show()
        Financial.ShowFinancialData(Results.SelectedHospital.HospitalId, Results.SelectedHospital.State)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndProfile.Click
        Me.Hide()
        FinInd.Show()
        FinInd.ShowFinancialDataApi(lblCmsCertNumProfileResult.Text)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityProfile.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientProfile.Click
        Me.Hide()
        Inpatient.Show()
        Inpatient.LoadPatientOriginDataAsync(Results.SelectedHospital)
        Inpatient.LoadCeoDataAsync(Results.SelectedHospital)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientProfile.Click
        Me.Hide()
        Outpatient.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub

    ' Empty event handlers for designer compatibility
    Private Sub Profile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub lblPhoneNum_Click(sender As Object, e As EventArgs) Handles lblPhoneNum.Click
    End Sub

    Private Sub lblLatLongResult_Click(sender As Object, e As EventArgs) Handles lblLatLongResult.Click
    End Sub

    Private Sub gbUniversityAff_Enter(sender As Object, e As EventArgs)
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
    End Sub

    Private Sub btnpoo_Click(sender As Object, e As EventArgs)
        Hide()
        yk.Show()
    End Sub
End Class
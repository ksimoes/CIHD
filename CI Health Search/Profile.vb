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

    ' Helper to get hospital name from any context or DataRow
    Private Function GetBestFacilityName(ctx As HospitalContext) As String
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

    Private Function GetBestAddress(ctx As HospitalContext) As String
        If ctx.LastDataRow IsNot Nothing Then
            Dim row = ctx.LastDataRow
            If row.Table.Columns.Contains("ST_ADR") Then Return row("ST_ADR").ToString()
            If row.Table.Columns.Contains("ADDR_LN_1_TXT") Then Return row("ADDR_LN_1_TXT").ToString()
            If row.Table.Columns.Contains("ADDRESS LINE 1") Then Return row("ADDRESS LINE 1").ToString()
            If row.Table.Columns.Contains("address_line_1") Then Return row("address_line_1").ToString()
            If row.Table.Columns.Contains("Address") Then Return row("Address").ToString()
            If row.Table.Columns.Contains("Facility Address") Then Return row("Facility Address").ToString()
        End If
        If Not String.IsNullOrWhiteSpace(ctx.Address) Then Return ctx.Address
        Return "N/A"
    End Function

    Private Function GetBestCmsNum(ctx As HospitalContext) As String
        If ctx.LastDataRow IsNot Nothing Then
            Dim row = ctx.LastDataRow
            If row.Table.Columns.Contains("PRVDR_NUM") Then Return row("PRVDR_NUM").ToString()
            If row.Table.Columns.Contains("Provider CCN") Then Return row("Provider CCN").ToString()
            If row.Table.Columns.Contains("CMSNum") Then Return row("CMSNum").ToString()
        End If
        If Not String.IsNullOrWhiteSpace(ctx.CMSNum) Then Return ctx.CMSNum
        Return "N/A"
    End Function

    Private Function GetHospitalNameFromContext(ctx As HospitalContext) As String
        If Not String.IsNullOrWhiteSpace(ctx.Name) Then
            Return ctx.Name
        End If
        Return "N/A"
    End Function

    Public Async Sub ShowProfile(foundHospital As HospitalContext)
        ' Show the basic info from the context first
        lblNameAddressResult.Text = GetBestFacilityName(foundHospital)
        lbladdy.Text = GetBestAddress(foundHospital)
        lblZipCodeResult.Text = If(Not String.IsNullOrWhiteSpace(foundHospital.Zip), foundHospital.Zip, "N/A")
        lblCountyFipsResult.Text = If(Not String.IsNullOrWhiteSpace(foundHospital.County), foundHospital.County, "N/A")
        lblCmsCertNumProfileResult.Text = GetBestCmsNum(foundHospital)
        lblNpiResult.Text = If(Not String.IsNullOrWhiteSpace(foundHospital.NPI), foundHospital.NPI, "N/A")
        lblGeneralMedSurgBedsResult.Text = If(foundHospital.NumOfBeds > 0, foundHospital.NumOfBeds.ToString(), "N/A")
        lblTotalEmployeesResult.Text = If(foundHospital.NumOfEmployees > 0, foundHospital.NumOfEmployees.ToString(), "N/A")
        lblTotalDischargesResult.Text = If(foundHospital.TotalDischarges > 0, foundHospital.TotalDischarges.ToString(), "N/A")
        lblTotalPatientRevenueResult.Text = If(foundHospital.TotalPatientRev > 0, foundHospital.TotalPatientRev.ToString("N0"), "N/A")
        lblTypeControlResult.Text = If(Not String.IsNullOrWhiteSpace(foundHospital.FacilityType), foundHospital.FacilityType, "N/A")
        lblCmsUrbRurDesigResult.Text = If(Not String.IsNullOrWhiteSpace(foundHospital.RuralOUrban), foundHospital.RuralOUrban, "N/A")
        lblCbsaResult.Text = If(Not String.IsNullOrWhiteSpace(foundHospital.CBSAnum), foundHospital.CBSAnum, "N/A")
        lblPhoneNumResult.Text = If(Not String.IsNullOrWhiteSpace(foundHospital.Phone), foundHospital.Phone, "N/A")

        ' If TN/TX, supplement with SQL data (but do NOT overwrite name/address)
        Dim useSql As Boolean = (foundHospital.State = "TN" Or foundHospital.State = "TX")
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
                            lblPhoneNumResult.Text = SafeGet(reader, "Phone")
                            lblCeoPresResult.Text = SafeGet(reader, "Admin")
                            lblCountyFipsResult.Text = SafeGet(reader, "County")
                            lblTotalPatientDaysResult.Text = SafeGet(reader, "Inpatient Days")
                        End If
                    End Using
                End Using
            End Using
        End If

        ' Always supplement with API data for the most up-to-date info (but do NOT overwrite name/address)
        Await ShowApiProfileAsync(foundHospital)

        ' Fetch and display NPI from NPPES API (by NPI if available, else by name/state)
        Dim npiData As JObject = Await FetchNpiDataAsync(foundHospital.NPI, GetHospitalNameFromContext(foundHospital), foundHospital.State)
        If npiData IsNot Nothing Then
            lblNpiResult.Text = npiData("number")?.ToString()
            foundHospital.NPI = npiData("number")?.ToString()
        ElseIf String.IsNullOrWhiteSpace(lblNpiResult.Text) OrElse lblNpiResult.Text = "N/A" Then
            lblNpiResult.Text = "NPI not found"
        End If

        ' Show affiliated providers for any hospital using its CCN
        Await LoadAffiliatedProvidersAsync(GetBestCmsNum(foundHospital))
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

    Private Async Function LoadAffiliatedProvidersAsync(ccn As String) As Task
        Dim apiUrl As String = "https://data.cms.gov/provider-data/api/1/datastore/query/27ea-46a8/0"
        Dim postBody As String = "{
        ""conditions"": [
            {
                ""property"": ""facility_affiliations_certification_number"",
                ""value"": """ & ccn & """,
                ""operator"": ""=""
            }
        ],
        ""limit"": 1000
    }"

        Try
            Using client As New HttpClient()
                Dim content = New StringContent(postBody, System.Text.Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(apiUrl, content)
                If response.IsSuccessStatusCode Then
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim obj = JObject.Parse(json)
                    If obj("results") IsNot Nothing AndAlso obj("results").HasValues Then
                        Dim data = obj("results")
                        Dim dt As New DataTable()
                        For Each col In data(0).ToObject(Of JObject)().Properties()
                            dt.Columns.Add(col.Name)
                        Next
                        For Each item In data
                            Dim row = dt.NewRow()
                            For Each col In dt.Columns
                                row(col.ToString()) = item(col.ToString())
                            Next
                            dt.Rows.Add(row)
                        Next

                        ' --- Fetch procedure_category for each NPI ---
                        Dim npiList As New List(Of String)
                        For Each row As DataRow In dt.Rows
                            Dim npi As String = row("npi").ToString()
                            If Not String.IsNullOrWhiteSpace(npi) Then npiList.Add(npi)
                        Next

                        Dim npiToProcedureCategory As New Dictionary(Of String, String)
                        If npiList.Count > 0 Then
                            Dim npiConditions As New List(Of String)
                            For Each npi In npiList.Distinct()
                                npiConditions.Add("{""property"":""npi"",""value"":""" & npi & """,""operator"":""=""}")
                            Next
                            Dim procPostBody As String = "{
                            ""conditions"": [
                                {""or"": [" & String.Join(",", npiConditions) & "]}
                            ],
                            ""limit"": 1000
                        }"

                            Dim procApiUrl As String = "https://data.cms.gov/provider-data/api/1/datastore/query/n0yb-util/0"
                            Using procClient As New HttpClient()
                                Dim procContent = New StringContent(procPostBody, System.Text.Encoding.UTF8, "application/json")
                                Dim procResponse = Await procClient.PostAsync(procApiUrl, procContent)
                                If procResponse.IsSuccessStatusCode Then
                                    Dim procJson = Await procResponse.Content.ReadAsStringAsync()
                                    Dim procObj = JObject.Parse(procJson)
                                    If procObj("results") IsNot Nothing AndAlso procObj("results").HasValues Then
                                        For Each item In procObj("results")
                                            Dim npiVal As String = item("npi")?.ToString()
                                            Dim procCat As String = item("procedure_category")?.ToString()
                                            If Not String.IsNullOrWhiteSpace(npiVal) AndAlso Not npiToProcedureCategory.ContainsKey(npiVal) Then
                                                npiToProcedureCategory(npiVal) = procCat
                                            End If
                                        Next
                                    End If
                                End If
                            End Using
                        End If

                        If Not dt.Columns.Contains("procedure_category") Then
                            dt.Columns.Add("procedure_category")
                        End If

                        For Each row As DataRow In dt.Rows
                            Dim npi As String = row("npi").ToString()
                            If npiToProcedureCategory.ContainsKey(npi) Then
                                row("procedure_category") = npiToProcedureCategory(npi)
                            Else
                                row("procedure_category") = ""
                            End If
                        Next

                        ' --- Fetch taxonomy description for each NPI (primary only) ---
                        If Not dt.Columns.Contains("Description") Then
                            dt.Columns.Add("Description")
                        End If

                        For Each row As DataRow In dt.Rows
                            Dim npi As String = row("npi").ToString()
                            Dim taxonomyDescription As String = ""
                            If Not String.IsNullOrWhiteSpace(npi) Then
                                Dim npiApiUrl As String = $"https://npiregistry.cms.hhs.gov/api/?number={npi}&version=2.1"
                                Using npiClient As New HttpClient()
                                    Dim npiResponse = Await npiClient.GetAsync(npiApiUrl)
                                    If npiResponse.IsSuccessStatusCode Then
                                        Dim npiJson = Await npiResponse.Content.ReadAsStringAsync()
                                        Dim npiObj = JObject.Parse(npiJson)
                                        If npiObj("results") IsNot Nothing AndAlso npiObj("results").HasValues Then
                                            Dim result = npiObj("results")(0)
                                            If result("taxonomies") IsNot Nothing AndAlso result("taxonomies").HasValues Then
                                                For Each taxonomy In result("taxonomies")
                                                    If taxonomy("primary") IsNot Nothing AndAlso taxonomy("primary").ToString().ToLower() = "true" Then
                                                        taxonomyDescription = taxonomy("desc")?.ToString()
                                                        Exit For
                                                    End If
                                                Next
                                                If String.IsNullOrWhiteSpace(taxonomyDescription) Then
                                                    taxonomyDescription = result("taxonomies")(0)("desc")?.ToString()
                                                End If
                                            End If
                                        End If
                                    End If
                                End Using
                            End If
                            row("Description") = If(String.IsNullOrWhiteSpace(taxonomyDescription), "N/A", taxonomyDescription)
                        Next

                        ' --- Bind to DataGridView and set column visibility ---
                        dgvProviders.DataSource = dt

                        ' Set custom column headers
                        If dgvProviders.Columns.Contains("provider_first_name") Then
                            dgvProviders.Columns("provider_first_name").HeaderText = "First Name"
                        End If
                        If dgvProviders.Columns.Contains("provider_last_name") Then
                            dgvProviders.Columns("provider_last_name").HeaderText = "Last Name"
                        End If
                        If dgvProviders.Columns.Contains("Description") Then
                            dgvProviders.Columns("Description").HeaderText = "Specialty"
                        End If

                        ' Set column visibility
                        For Each col As DataGridViewColumn In dgvProviders.Columns
                            col.Visible = (col.Name = "npi" OrElse
                                       col.Name = "provider_first_name" OrElse
                                       col.Name = "provider_last_name" OrElse
                                       col.Name = "facility_affiliations_certification_number" OrElse
                                       col.Name = "procedure_category" OrElse
                                       col.Name = "Description")
                        Next

                        ' Move "Specialty" column right after "First Name" and "Last Name"
                        Dim colOrder As New List(Of String) From {
                        "provider_first_name",
                        "provider_last_name",
                        "Description", ' Specialty
                        "npi",
                        "facility_affiliations_certification_number",
                        "procedure_category"
                    }
                        Dim displayIndex As Integer = 0
                        For Each colName In colOrder
                            If dgvProviders.Columns.Contains(colName) Then
                                dgvProviders.Columns(colName).DisplayIndex = displayIndex
                                displayIndex += 1
                            End If
                        Next

                        ' --- Make DataGridView more visually appealing ---
                        dgvProviders.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
                        dgvProviders.DefaultCellStyle.BackColor = Color.White
                        dgvProviders.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue
                        dgvProviders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                        dgvProviders.EnableHeadersVisualStyles = False
                        dgvProviders.GridColor = Color.LightSteelBlue
                        dgvProviders.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue
                        dgvProviders.DefaultCellStyle.SelectionForeColor = Color.Black

                    Else
                        dgvProviders.DataSource = Nothing
                        dgvProviders.Columns.Clear()
                        dgvProviders.Rows.Clear()
                        dgvProviders.Refresh()
                    End If
                Else
                    dgvProviders.DataSource = Nothing
                    dgvProviders.Columns.Clear()
                    dgvProviders.Rows.Clear()
                    dgvProviders.Refresh()
                End If
            End Using
        Catch ex As Exception
            dgvProviders.DataSource = Nothing
            dgvProviders.Columns.Clear()
            dgvProviders.Rows.Clear()
            dgvProviders.Refresh()
            MessageBox.Show("Provider API error: " & ex.Message)
        End Try
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

    Private Async Sub ShowProviderDetailsPopup(npi As String)
        Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/9552739e-3d05-4c1b-8eff-ecabf391e2e5/data?filter[Prscrbr_NPI]={Uri.EscapeDataString(npi)}&size=100"
        Dim dt As New DataTable()
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim data = JArray.Parse(json)
                If data.Count > 0 Then
                    For Each col In data(0).ToObject(Of JObject)().Properties()
                        dt.Columns.Add(col.Name)
                    Next
                    For Each item In data
                        Dim row = dt.NewRow()
                        For Each col In dt.Columns
                            row(col.ToString()) = item(col.ToString())
                        Next
                        dt.Rows.Add(row)
                    Next
                End If
            End If
        End Using

        ' Show the popup form
        Dim detailsForm As New ProviderDetailsForm()
        detailsForm.dgvDetails.DataSource = dt
        detailsForm.Text = $"Provider Details for NPI: {npi}"
        detailsForm.ShowDialog()
    End Sub

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

    Private Sub lblTotalDischargesResult_Click(sender As Object, e As EventArgs) Handles lblTotalDischargesResult.Click
    End Sub

    Private Sub dgvProviders_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProviders.CellDoubleClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Dim npi As String = ""
            If dgvProviders.Columns(e.ColumnIndex).Name = "npi" OrElse
               dgvProviders.Columns(e.ColumnIndex).Name = "provider_first_name" OrElse
               dgvProviders.Columns(e.ColumnIndex).Name = "provider_last_name" Then

                npi = dgvProviders.Rows(e.RowIndex).Cells("npi").Value?.ToString()
                If Not String.IsNullOrWhiteSpace(npi) Then
                    ShowProviderDetailsPopup(npi)
                End If
            End If
        End If
    End Sub
End Class
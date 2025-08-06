Imports System.Data.SqlClient
Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class Search
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    Public Shared SearchHospital As New HospitalContext()

    Private ReadOnly FacilityTypeMap As New Dictionary(Of String, String) From {
        {"Childrens", "CH"},
        {"Critical Access", "CAH"},
        {"Long Term", "LTCH"},
        {"Psychiatric", "PH"},
        {"Rehabilitation", "RH"},
        {"Rural Emergency Hospital", "RHC"},
        {"Short Term Acute Care", "STH"},
        {"Federally Qualified Health Centers", "FQHC"},
        {"Rural Health Clinic", "RHC"}
    }

    Public Function useSQL(ByRef selectedState As String) As Boolean
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand("Select UseSql from dbo.States Where StateCode = '" + selectedState + "'", conn)
                conn.Open()
                Return cmd.ExecuteScalar()
            End Using
        End Using
    End Function

    Private Sub SetupHospProfile()
        SearchHospital.CMSNum = If(Not String.IsNullOrWhiteSpace(txtCmsCertNumDemoAll.Text), txtCmsCertNumDemoAll.Text.Trim(), TextBox39.Text.Trim())
        SearchHospital.Zip = If(Not String.IsNullOrWhiteSpace(txtZipCodeDemoAll.Text), txtZipCodeDemoAll.Text.Trim(), TextBox40.Text.Trim())
        SearchHospital.State = If(lbStateAll.SelectedItem IsNot Nothing, lbStateAll.SelectedItem.ToString().Trim(), "")
        SearchHospital.Name = VerifiySearch(txtHospitalNameAll)
        SearchHospital.City = VerifiySearch(txtCityAll)
        SearchHospital.Phone = VerifiySearch(txtAreaCodeAll)
        SearchHospital.NPI = VerifiySearch(txtNpiAll)
    End Sub

    Public Function VerifiySearch(txtSearched As TextBox) As String
        If (txtSearched.Text IsNot Nothing) Or txtSearched.Text.Length > 0 Then
            Return txtSearched.Text.Trim()
        Else
            Return Nothing
        End If
    End Function

    Public Function CleanMeUp(strMydata As Object, Optional ByRef isNumber As Boolean = False) As String
        If strMydata Is Nothing Then
            If isNumber Then
                Return "0"
            Else
                Return "N/A"
            End If
        End If

        Dim strVal As String = strMydata.ToString().Trim()
        If String.IsNullOrEmpty(strVal) OrElse strVal = "Result" OrElse strVal = "N/A" Then
            If isNumber Then
                Return "0"
            Else
                Return "N/A"
            End If
        End If

        If isNumber Then
            Dim dec As Decimal
            If Decimal.TryParse(strVal.Replace("$", "").Replace(",", ""), dec) Then
                Return dec.ToString("N")
            Else
                Return "0"
            End If
        Else
            Return strVal
        End If
    End Function

    Private Function NormalizeName(name As String) As String
        If String.IsNullOrEmpty(name) Then Return ""
        Dim cleaned = name.ToLower().Trim()
        cleaned = cleaned.Replace(".", "").Replace(",", "").Replace("-", " ")
        Dim commonWords = New String() {"hospital", "center", "medical", "the", "of", "and"}
        For Each word In commonWords
            cleaned = cleaned.Replace(word, "")
        Next
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, "\s+", " ")
        Return cleaned.Trim()
    End Function

    Private Function NormalizeAddress(addr As String) As String
        If String.IsNullOrWhiteSpace(addr) Then Return ""
        Dim cleaned = addr.ToLower().Trim()
        cleaned = cleaned.Replace("us highway", "hwy").Replace("us hwy", "hwy").Replace("highway", "hwy")
        cleaned = cleaned.Replace("street", "st").Replace("avenue", "ave").Replace("road", "rd")
        cleaned = cleaned.Replace("drive", "dr").Replace("boulevard", "blvd").Replace("lane", "ln")
        cleaned = cleaned.Replace("east", "e").Replace("west", "w").Replace("north", "n").Replace("south", "s")
        cleaned = cleaned.Replace(".", "").Replace(",", "")
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, "\s+", " ")
        Return cleaned
    End Function

    Private Function IsFuzzyMatch(npiName As String, facilityName As String) As Boolean
        Dim normNpi = NormalizeName(npiName)
        Dim normFacility = NormalizeName(facilityName)
        Return normFacility.Contains(normNpi) OrElse normNpi.Contains(normFacility)
    End Function

    Private Function IsFuzzyAddressMatch(addr1 As String, addr2 As String) As Boolean
        Dim norm1 = NormalizeAddress(addr1)
        Dim norm2 = NormalizeAddress(addr2)
        If String.IsNullOrEmpty(norm1) OrElse String.IsNullOrEmpty(norm2) Then Return False

        Dim tokens1 = norm1.Split(" "c).Where(Function(t) t.Length > 1).ToArray()
        Dim tokens2 = norm2.Split(" "c).Where(Function(t) t.Length > 1).ToArray()
        Dim overlap = tokens1.Intersect(tokens2).Count()
        Return overlap >= 3 OrElse norm1.Contains(norm2) OrElse norm2.Contains(norm1)
    End Function

    Private Async Function GetNpiRegistryInfoAsync(npi As String) As Task(Of JObject)
        Dim apiUrl As String = $"https://npiregistry.cms.hhs.gov/api/?number={Uri.EscapeDataString(npi)}&version=2.1"
        Try
            Using client As New HttpClient()
                client.Timeout = TimeSpan.FromSeconds(15)
                Dim response = Await client.GetAsync(apiUrl)
                If response.IsSuccessStatusCode Then
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim obj = JObject.Parse(json)
                    If obj("results") IsNot Nothing AndAlso obj("results").HasValues Then
                        Return obj("results")(0)
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("NPI Registry API error: " & ex.Message)
        End Try
        Return Nothing
    End Function

    Private Async Function SearchFacilityNpiOnlyAsync(npi As String) As Task(Of DataTable)
        Dim apiUrl As String = $"https://data.cms.gov/provider-data/api/1/datastore/query/4jcv-atw7/0?filters=%7B%22npi%22%3A%22{Uri.EscapeDataString(npi)}%22%7D&size=100"
        Try
            Using client As New HttpClient()
                client.Timeout = TimeSpan.FromSeconds(15)
                Dim response = Await client.GetAsync(apiUrl)
                If response.IsSuccessStatusCode Then
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim obj = JObject.Parse(json)
                    Dim data = obj("data")
                    If data IsNot Nothing AndAlso data.HasValues Then
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
                        Return dt
                    End If
                Else
                    MessageBox.Show("Facility NPI API error: " & response.StatusCode.ToString())
                End If
            End Using
        Catch ex As TaskCanceledException
            MessageBox.Show("Facility NPI API request timed out.")
        Catch ex As Exception
            MessageBox.Show("Facility NPI API error: " & ex.Message)
        End Try
        Return Nothing
    End Function

    Private Async Function GetNpiResultsAsync(apiUrl As String) As Task(Of DataTable)
        Try
            Using client As New HttpClient()
                client.Timeout = TimeSpan.FromSeconds(15)
                Dim response = Await client.GetAsync(apiUrl)
                If response.IsSuccessStatusCode Then
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim data = JArray.Parse(json)
                    If data.Count > 0 Then
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
                        Return dt
                    End If
                Else
                    MessageBox.Show("API error: " & response.StatusCode.ToString())
                End If
            End Using
        Catch ex As TaskCanceledException
            MessageBox.Show("API request timed out.")
        Catch ex As Exception
            MessageBox.Show("API error: " & ex.Message)
        End Try
        Return Nothing
    End Function

    Private Async Function GetFqhcsAsync() As Task(Of DataTable)
        Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8ba0f9b4-9493-4aa0-9f82-44ea9468d1b5/data?size=1000"
        Try
            Using client As New HttpClient()
                client.Timeout = TimeSpan.FromSeconds(15)
                Dim response = Await client.GetAsync(apiUrl)
                If response.IsSuccessStatusCode Then
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim data = JArray.Parse(json)
                    If data.Count > 0 Then
                        Dim dt As New DataTable()
                        For Each col In data(0).ToObject(Of JObject)().Properties()
                            dt.Columns.Add(col.Name)
                        Next
                        For Each item In data
                            If item("PRVDR_CTGRY_CD") IsNot Nothing AndAlso item("PRVDR_CTGRY_CD").ToString() = "21" Then
                                Dim row = dt.NewRow()
                                For Each col In dt.Columns
                                    row(col.ToString()) = item(col.ToString())
                                Next
                                dt.Rows.Add(row)
                            End If
                        Next
                        Return dt
                    End If
                Else
                    MessageBox.Show("FQHC API error: " & response.StatusCode.ToString())
                End If
            End Using
        Catch ex As TaskCanceledException
            MessageBox.Show("FQHC API request timed out.")
        Catch ex As Exception
            MessageBox.Show("FQHC API error: " & ex.Message)
        End Try
        Return Nothing
    End Function

    Private Async Sub btnSearchHCPCSCode_Click(sender As Object, e As EventArgs) Handles btnSearchHCPCSCode.Click
        Dim code As String = tbCode.Text.Trim().ToUpper()
        If String.IsNullOrWhiteSpace(code) Then
            MessageBox.Show("Please enter an HCPCS code to search.")
            Return
        End If

        lblstatus.Text = "Searching by HCPCS code..."
        lblstatus.Visible = True

        Try
            Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/92396110-2aed-4d63-a6a2-5d6207d46a29/data?filter[HCPCS_Cd]={Uri.EscapeDataString(code)}&size=1000"
            Dim dt As New DataTable()
            Using client As New HttpClient()
                client.Timeout = TimeSpan.FromSeconds(15)
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
                Else
                    MessageBox.Show("HCPCS API error: " & response.StatusCode.ToString())
                End If
            End Using

            If dt.Rows.Count = 0 Then
                MessageBox.Show("No results found for this HCPCS code.")
                lblstatus.Text = ""
                lblstatus.Visible = False
                Return
            End If

            Dim popup As New HCPCSCodeResultsForm(dt, code)
            popup.ShowDialog()
        Catch ex As TaskCanceledException
            MessageBox.Show("HCPCS API request timed out.")
        Catch ex As Exception
            MessageBox.Show("Error searching by HCPCS code: " & ex.Message)
        Finally
            lblstatus.Text = ""
            lblstatus.Visible = False
        End Try
    End Sub

    Private Async Function SearchByApiAsync(selectedState As String, filterSummary As String) As Task
        Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?"
        Dim filters As New List(Of String)

        If Not String.IsNullOrEmpty(selectedState) Then filters.Add("filter[State Code]=" & Uri.EscapeDataString(selectedState))
        If Not String.IsNullOrEmpty(txtCityAll.Text) Then filters.Add("filter[City]=" & Uri.EscapeDataString(txtCityAll.Text.Trim()))
        Dim cmsNum As String = If(Not String.IsNullOrWhiteSpace(txtCmsCertNumDemoAll.Text), txtCmsCertNumDemoAll.Text.Trim(), TextBox39.Text.Trim())
        If Not String.IsNullOrEmpty(cmsNum) Then
            filters.Add("filter[Provider CCN]=" & Uri.EscapeDataString(cmsNum))
        End If
        If Not String.IsNullOrWhiteSpace(txtcountygeoall.Text) Then
            filters.Add("filter[County]=" & Uri.EscapeDataString(txtcountygeoall.Text.Trim()))
        End If
        If lbTypeFacilityCharAll.SelectedItem IsNot Nothing Then
            Dim selectedDisplay = lbTypeFacilityCharAll.SelectedItem.ToString()
            If FacilityTypeMap.ContainsKey(selectedDisplay) Then
                Dim acronym = FacilityTypeMap(selectedDisplay)
                filters.Add("filter[CCN Facility Type]=" & Uri.EscapeDataString(acronym))
            End If
        End If
        If Not String.IsNullOrEmpty(txtNpiAll.Text) Then filters.Add("filter[NPI]=" & Uri.EscapeDataString(txtNpiAll.Text.Trim()))
        If Not String.IsNullOrEmpty(txtHospitalNameAll.Text) Then filters.Add("filter[Hospital Name]=" & Uri.EscapeDataString(txtHospitalNameAll.Text.Trim()))
        If cbRUAll.SelectedItem IsNot Nothing AndAlso Not String.IsNullOrEmpty(cbRUAll.SelectedItem.ToString()) Then
            filters.Add("filter[Rural Versus Urban]=" & Uri.EscapeDataString(cbRUAll.SelectedItem.ToString()))
        End If

        Dim zipCode As String = If(Not String.IsNullOrWhiteSpace(txtZipCodeDemoAll.Text), txtZipCodeDemoAll.Text.Trim(), TextBox40.Text.Trim())
        If Not String.IsNullOrEmpty(zipCode) Then
            filters.Add("filter[Zip Code]=" & Uri.EscapeDataString(zipCode))
        End If

        filters.Add("size=1000")
        apiUrl &= String.Join("&", filters)

        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)
                If data.Count > 0 Then
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

                    If (Not String.IsNullOrEmpty(txtMinTotPatRevAll.Text) OrElse Not String.IsNullOrEmpty(txtMaxTotPatRevAll.Text)) AndAlso dt.Columns.Contains("Total Patient Revenue") Then
                        Dim minVal As Decimal = 0
                        Dim maxVal As Decimal = Decimal.MaxValue
                        If Not String.IsNullOrEmpty(txtMinTotPatRevAll.Text) Then Decimal.TryParse(txtMinTotPatRevAll.Text, minVal)
                        If Not String.IsNullOrEmpty(txtMaxTotPatRevAll.Text) Then Decimal.TryParse(txtMaxTotPatRevAll.Text, maxVal)
                        Dim filteredRows = dt.AsEnumerable().Where(
                            Function(r)
                                Dim val As Decimal = 0
                                Dim strVal = r.Field(Of String)("Total Patient Revenue")
                                If String.IsNullOrWhiteSpace(strVal) OrElse Not Decimal.TryParse(strVal.Replace("$", "").Replace(",", ""), val) Then
                                    Return False
                                End If
                                Return val >= minVal AndAlso val <= maxVal
                            End Function
                        ).ToArray()
                        dt = If(filteredRows.Length > 0, filteredRows.CopyToDataTable(), dt.Clone())
                    End If

                    If (Not String.IsNullOrEmpty(txtMinTotalBedsAll.Text) OrElse Not String.IsNullOrEmpty(txtMaxTotalBedsAll.Text)) AndAlso dt.Columns.Contains("Number of Beds") Then
                        Dim minBeds As Decimal = 0
                        Dim maxBeds As Decimal = Decimal.MaxValue
                        If Not String.IsNullOrEmpty(txtMinTotalBedsAll.Text) Then Decimal.TryParse(txtMinTotalBedsAll.Text, minBeds)
                        If Not String.IsNullOrEmpty(txtMaxTotalBedsAll.Text) Then Decimal.TryParse(txtMaxTotalBedsAll.Text, maxBeds)
                        Dim filteredRows = dt.AsEnumerable().Where(
                            Function(r)
                                Dim val As Decimal = 0
                                Dim strVal = r.Field(Of String)("Number of Beds")
                                If String.IsNullOrWhiteSpace(strVal) OrElse Not Decimal.TryParse(strVal.Replace("$", "").Replace(",", ""), val) Then
                                    Return False
                                End If
                                Return val >= minBeds AndAlso val <= maxBeds
                            End Function
                        ).ToArray()
                        dt = If(filteredRows.Length > 0, filteredRows.CopyToDataTable(), dt.Clone())
                    End If

                    If dt.Rows.Count = 0 Then
                        MessageBox.Show("No results found for your search.")
                        Return
                    End If

                    Results.SetResults(dt, filterSummary)
                    Results.SelectedState = selectedState
                    Hide()
                    Results.Show()
                Else
                    MessageBox.Show("No results found for your search.")
                End If
            Else
                MessageBox.Show("API error: " & response.StatusCode.ToString())
            End If
        End Using
    End Function

    Private Async Sub btnSearchAll_Click(sender As Object, e As EventArgs) Handles btnSearchAll.Click
        lblstatus.Text = "Searching..."
        lblstatus.Visible = True
        Dim filterSummary As String = BuildFilterSummary()

        Try
            Dim filters As New List(Of String)
            Dim parameters As New List(Of SqlParameter)
            Dim selectedState As String = ""
            If lbStateAll.SelectedItem IsNot Nothing Then
                selectedState = lbStateAll.SelectedItem.ToString().Trim()
            End If

            If Not String.IsNullOrWhiteSpace(txtCmsCertNumDemoAll.Text) Then
                Dim cmsNum As String = txtCmsCertNumDemoAll.Text.Trim()
                Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?filter[Provider CCN]={Uri.EscapeDataString(cmsNum)}&size=1000"
                Dim dt As DataTable = Await GetNpiResultsAsync(apiUrl)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    Results.SetResults(dt, filterSummary)
                    Results.SelectedState = selectedState
                    Hide()
                    Results.Show()
                    lblstatus.Text = ""
                    lblstatus.Visible = False
                    Return
                Else
                    MessageBox.Show("No results found for this CMS number.")
                    lblstatus.Text = ""
                    lblstatus.Visible = False
                    Return
                End If
            End If

            If Not String.IsNullOrWhiteSpace(txtNpiAll.Text) Then
                Dim npi As String = txtNpiAll.Text.Trim()
                Dim facilityUrl As String = $"https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?filter[NPI]={Uri.EscapeDataString(npi)}&size=1000"
                Dim dtFacility As DataTable = Await GetNpiResultsAsync(facilityUrl)
                Dim dtFacilityNpiOnly As DataTable = Await SearchFacilityNpiOnlyAsync(npi)
                Dim providerUrl As String = $"https://data.cms.gov/data-api/v1/dataset/4bcae866-3411-439a-b762-90a6187c194b/data?filter[npi]={Uri.EscapeDataString(npi)}&size=1000"
                Dim dtProvider As DataTable = Await GetNpiResultsAsync(providerUrl)

                Dim npiInfo = Await GetNpiRegistryInfoAsync(npi)
                If npiInfo IsNot Nothing Then
                    Dim npiName = npiInfo("basic")?("organization_name")?.ToString()
                    Dim npiCity = npiInfo("addresses")?(0)?("city")?.ToString()
                    Dim npiState = npiInfo("addresses")?(0)?("state")?.ToString()
                    Dim npiAddress = npiInfo("addresses")?(0)?("address_1")?.ToString()

                    If Not String.IsNullOrEmpty(npiName) AndAlso Not String.IsNullOrEmpty(npiCity) AndAlso Not String.IsNullOrEmpty(npiState) Then
                        Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?filter[State Code]={Uri.EscapeDataString(npiState)}&filter[City]={Uri.EscapeDataString(npiCity)}&size=1000"
                        Dim dtFacilities As DataTable = Await GetNpiResultsAsync(apiUrl)

                        If dtFacilities IsNot Nothing AndAlso dtFacilities.Rows.Count > 0 Then
                            Dim dtMatches As DataTable = dtFacilities.Clone()
                            For Each row As DataRow In dtFacilities.Rows
                                Dim facilityName = ""
                                If dtFacilities.Columns.Contains("Facility Name") Then
                                    facilityName = row("Facility Name").ToString()
                                ElseIf dtFacilities.Columns.Contains("Hospital Name") Then
                                    facilityName = row("Hospital Name").ToString()
                                End If

                                Dim facilityAddress As String = ""
                                If dtFacilities.Columns.Contains("Street Address") Then
                                    facilityAddress = row("Street Address").ToString()
                                End If

                                If (Not String.IsNullOrEmpty(facilityName) AndAlso IsFuzzyMatch(npiName, facilityName)) OrElse
                                   (Not String.IsNullOrEmpty(facilityAddress) AndAlso IsFuzzyAddressMatch(npiAddress, facilityAddress)) Then
                                    dtMatches.ImportRow(row)
                                End If
                            Next
                            If dtMatches.Rows.Count > 0 Then
                                Results.SetResults(dtMatches, filterSummary)
                                Results.SelectedState = npiState
                                Hide()
                                Results.Show()
                                lblstatus.Text = ""
                                lblstatus.Visible = False
                                Return
                            End If
                        End If
                    End If
                End If

                MessageBox.Show("No results found for your NPI search.")
                lblstatus.Text = ""
                lblstatus.Visible = False
                Return
            End If

            If lbTypeFacilityCharAll.SelectedItem IsNot Nothing AndAlso lbTypeFacilityCharAll.SelectedItem.ToString() = "Federally Qualified Health Centers" Then
                Dim fqhcApiUrl As String = "https://data.cms.gov/data-api/v1/dataset/4bcae866-3411-439a-b762-90a6187c194b/data?"
                Dim fqhcFilters As New List(Of String)

                If lbStateAll.SelectedItem IsNot Nothing Then
                    fqhcFilters.Add("filter[STATE]=" & Uri.EscapeDataString(lbStateAll.SelectedItem.ToString().Trim()))
                End If
                If Not String.IsNullOrWhiteSpace(txtCityAll.Text) Then
                    fqhcFilters.Add("filter[CITY]=" & Uri.EscapeDataString(txtCityAll.Text.Trim()))
                End If
                If Not String.IsNullOrWhiteSpace(txtZipCodeDemoAll.Text) Then
                    fqhcFilters.Add("filter[ZIP]=" & Uri.EscapeDataString(txtZipCodeDemoAll.Text.Trim()))
                End If
                If Not String.IsNullOrWhiteSpace(txtNpiAll.Text) Then
                    fqhcFilters.Add("filter[NPI]=" & Uri.EscapeDataString(txtNpiAll.Text.Trim()))
                End If

                fqhcFilters.Add("size=10000")
                fqhcApiUrl &= String.Join("&", fqhcFilters)

                Dim fqhcDt As DataTable = Await GetNpiResultsAsync(fqhcApiUrl)
                If fqhcDt IsNot Nothing AndAlso fqhcDt.Rows.Count > 0 Then
                    Results.SetResults(fqhcDt, filterSummary)
                    Results.SelectedState = If(lbStateAll.SelectedItem IsNot Nothing, lbStateAll.SelectedItem.ToString().Trim(), "")
                    Hide()
                    Results.Show()
                    lblstatus.Text = ""
                    lblstatus.Visible = False
                    Return
                Else
                    MessageBox.Show("No FQHCs found.")
                    lblstatus.Text = ""
                    lblstatus.Visible = False
                    Return
                End If
            End If

            If selectedState = "TN" OrElse selectedState = "TX" Then
                If Not String.IsNullOrEmpty(selectedState) Then
                    filters.Add("State = @State")
                    parameters.Add(New SqlParameter("@State", selectedState))
                End If
                If Not String.IsNullOrEmpty(txtCityAll.Text) Then
                    filters.Add("City = @City")
                    parameters.Add(New SqlParameter("@City", txtCityAll.Text.Trim()))
                End If

                Dim query As String
                If selectedState = "TN" Then
                    query = "SELECT LicenseNum, [Facility Name], State, City, County FROM tn.AdminCon WHERE 1=1"
                ElseIf selectedState = "TX" Then
                    query = "SELECT id AS LicenseNum, [Facility Name], State, City, County FROM tx.Utilization WHERE 1=1"
                Else
                    query = ""
                End If

                If filters.Count > 0 Then
                    query &= " AND " & String.Join(" AND ", filters)
                End If

                Dim dt As New DataTable()
                Using conn As New SqlConnection(connectionString)
                    Using cmd As New SqlCommand(query, conn)
                        cmd.Parameters.AddRange(parameters.ToArray())
                        Dim da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using

                Results.SetResults(dt, filterSummary)
                Results.SelectedState = selectedState
                Hide()
                Results.Show()
                Await SearchByApiAsync(selectedState, filterSummary)
            Else
                Await SearchByApiAsync(selectedState, filterSummary)
            End If

            lblstatus.Text = ""
        Catch ex As Exception
            lblstatus.Text = "Error during search. Please try again." & vbCrLf & ex.Message
        End Try

        lblstatus.Visible = False
    End Sub

    Private Async Sub SearchDemographicsTab(
    lbState As ListBox,
    txtCity As TextBox,
    txtCmsCertNum As TextBox,
    txtNpi As TextBox,
    txtHospitalName As TextBox,
    txtAreaCode As TextBox,
    txtZipCode As TextBox
)
        lblstatus.Text = "Searching..."
        lblstatus.Visible = True
        Try
            Dim selectedState As String = ""
            If lbState.SelectedItem IsNot Nothing Then
                selectedState = lbState.SelectedItem.ToString().Trim()
            End If

            Dim filters As New List(Of String)
            If Not String.IsNullOrWhiteSpace(txtCity.Text) Then filters.Add("filter[City]=" & Uri.EscapeDataString(txtCity.Text.Trim()))
            If Not String.IsNullOrWhiteSpace(txtCmsCertNum.Text) Then filters.Add("filter[Provider CCN]=" & Uri.EscapeDataString(txtCmsCertNum.Text.Trim()))
            If Not String.IsNullOrWhiteSpace(txtNpi.Text) Then filters.Add("filter[NPI]=" & Uri.EscapeDataString(txtNpi.Text.Trim()))
            If Not String.IsNullOrWhiteSpace(txtHospitalName.Text) Then filters.Add("filter[Hospital Name]=" & Uri.EscapeDataString(txtHospitalName.Text.Trim()))
            If Not String.IsNullOrWhiteSpace(txtAreaCode.Text) Then filters.Add("filter[Phone]=" & Uri.EscapeDataString(txtAreaCode.Text.Trim()))
            If Not String.IsNullOrWhiteSpace(txtZipCode.Text) Then filters.Add("filter[Zip Code]=" & Uri.EscapeDataString(txtZipCode.Text.Trim()))
            If Not String.IsNullOrEmpty(selectedState) Then filters.Add("filter[State Code]=" & Uri.EscapeDataString(selectedState))
            filters.Add("size=1000")

            Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?" & String.Join("&", filters)

            Using client As New HttpClient()
                Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
                If response.IsSuccessStatusCode Then
                    Dim json As String = Await response.Content.ReadAsStringAsync()
                    Dim data As JArray = JArray.Parse(json)
                    If data.Count > 0 Then
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
                        Results.SetResults(dt, "Demographics Search")
                        Results.SelectedState = selectedState
                        Hide()
                        Results.Show()
                    Else
                        MessageBox.Show("No results found for your search.")
                    End If
                Else
                    MessageBox.Show("API error: " & response.StatusCode.ToString())
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error during search: " & ex.Message)
        Finally
            lblstatus.Text = ""
            lblstatus.Visible = False
        End Try
    End Sub

    Private Async Sub SearchUtilizationRanges(
    txtMinRoutineBeds As TextBox,
    txtMaxRoutineBeds As TextBox,
    txtMinSpecialBeds As TextBox,
    txtMaxSpecialBeds As TextBox,
    txtMinAnnualDis As TextBox,
    txtMaxAnnualDis As TextBox,
    txtMinTotPatRev As TextBox,
    txtMaxTotPatRev As TextBox,
    txtMinTotalBeds As TextBox,
    txtMaxTotalBeds As TextBox
)
        lblstatus.Text = "Searching..."
        lblstatus.Visible = True
        Try
            Dim filters As New List(Of String)
            filters.Add("size=1000")
            Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?" & String.Join("&", filters)

            Using client As New HttpClient()
                Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
                If response.IsSuccessStatusCode Then
                    Dim json As String = Await response.Content.ReadAsStringAsync()
                    Dim data As JArray = JArray.Parse(json)
                    If data.Count > 0 Then
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

                        ' Routine Beds
                        If (Not String.IsNullOrEmpty(txtMinRoutineBeds.Text) OrElse Not String.IsNullOrEmpty(txtMaxRoutineBeds.Text)) AndAlso dt.Columns.Contains("Routine Beds") Then
                            Dim minVal As Decimal = 0
                            Dim maxVal As Decimal = Decimal.MaxValue
                            If Not String.IsNullOrEmpty(txtMinRoutineBeds.Text) Then Decimal.TryParse(txtMinRoutineBeds.Text, minVal)
                            If Not String.IsNullOrEmpty(txtMaxRoutineBeds.Text) Then Decimal.TryParse(txtMaxRoutineBeds.Text, maxVal)
                            Dim filteredRows = dt.AsEnumerable().Where(
                            Function(r)
                                Dim val As Decimal = 0
                                Dim strVal = r.Field(Of String)("Routine Beds")
                                If String.IsNullOrWhiteSpace(strVal) OrElse Not Decimal.TryParse(strVal.Replace("$", "").Replace(",", ""), val) Then
                                    Return False
                                End If
                                Return val >= minVal AndAlso val <= maxVal
                            End Function
                        ).ToArray()
                            dt = If(filteredRows.Length > 0, filteredRows.CopyToDataTable(), dt.Clone())
                        End If

                        ' Special Care Beds
                        If (Not String.IsNullOrEmpty(txtMinSpecialBeds.Text) OrElse Not String.IsNullOrEmpty(txtMaxSpecialBeds.Text)) AndAlso dt.Columns.Contains("Special Care Beds") Then
                            Dim minVal As Decimal = 0
                            Dim maxVal As Decimal = Decimal.MaxValue
                            If Not String.IsNullOrEmpty(txtMinSpecialBeds.Text) Then Decimal.TryParse(txtMinSpecialBeds.Text, minVal)
                            If Not String.IsNullOrEmpty(txtMaxSpecialBeds.Text) Then Decimal.TryParse(txtMaxSpecialBeds.Text, maxVal)
                            Dim filteredRows = dt.AsEnumerable().Where(
                            Function(r)
                                Dim val As Decimal = 0
                                Dim strVal = r.Field(Of String)("Special Care Beds")
                                If String.IsNullOrWhiteSpace(strVal) OrElse Not Decimal.TryParse(strVal.Replace("$", "").Replace(",", ""), val) Then
                                    Return False
                                End If
                                Return val >= minVal AndAlso val <= maxVal
                            End Function
                        ).ToArray()
                            dt = If(filteredRows.Length > 0, filteredRows.CopyToDataTable(), dt.Clone())
                        End If

                        ' Annual Discharges
                        If (Not String.IsNullOrEmpty(txtMinAnnualDis.Text) OrElse Not String.IsNullOrEmpty(txtMaxAnnualDis.Text)) AndAlso dt.Columns.Contains("Annual Discharges") Then
                            Dim minVal As Decimal = 0
                            Dim maxVal As Decimal = Decimal.MaxValue
                            If Not String.IsNullOrEmpty(txtMinAnnualDis.Text) Then Decimal.TryParse(txtMinAnnualDis.Text, minVal)
                            If Not String.IsNullOrEmpty(txtMaxAnnualDis.Text) Then Decimal.TryParse(txtMaxAnnualDis.Text, maxVal)
                            Dim filteredRows = dt.AsEnumerable().Where(
                            Function(r)
                                Dim val As Decimal = 0
                                Dim strVal = r.Field(Of String)("Annual Discharges")
                                If String.IsNullOrWhiteSpace(strVal) OrElse Not Decimal.TryParse(strVal.Replace("$", "").Replace(",", ""), val) Then
                                    Return False
                                End If
                                Return val >= minVal AndAlso val <= maxVal
                            End Function
                        ).ToArray()
                            dt = If(filteredRows.Length > 0, filteredRows.CopyToDataTable(), dt.Clone())
                        End If

                        ' Total Patient Revenue
                        If (Not String.IsNullOrEmpty(txtMinTotPatRev.Text) OrElse Not String.IsNullOrEmpty(txtMaxTotPatRev.Text)) AndAlso dt.Columns.Contains("Total Patient Revenue") Then
                            Dim minVal As Decimal = 0
                            Dim maxVal As Decimal = Decimal.MaxValue
                            If Not String.IsNullOrEmpty(txtMinTotPatRev.Text) Then Decimal.TryParse(txtMinTotPatRev.Text, minVal)
                            If Not String.IsNullOrEmpty(txtMaxTotPatRev.Text) Then Decimal.TryParse(txtMaxTotPatRev.Text, maxVal)
                            Dim filteredRows = dt.AsEnumerable().Where(
                            Function(r)
                                Dim val As Decimal = 0
                                Dim strVal = r.Field(Of String)("Total Patient Revenue")
                                If String.IsNullOrWhiteSpace(strVal) OrElse Not Decimal.TryParse(strVal.Replace("$", "").Replace(",", ""), val) Then
                                    Return False
                                End If
                                Return val >= minVal AndAlso val <= maxVal
                            End Function
                        ).ToArray()
                            dt = If(filteredRows.Length > 0, filteredRows.CopyToDataTable(), dt.Clone())
                        End If

                        ' Total Beds
                        If (Not String.IsNullOrEmpty(txtMinTotalBeds.Text) OrElse Not String.IsNullOrEmpty(txtMaxTotalBeds.Text)) AndAlso dt.Columns.Contains("Number of Beds") Then
                            Dim minBeds As Decimal = 0
                            Dim maxBeds As Decimal = Decimal.MaxValue
                            If Not String.IsNullOrEmpty(txtMinTotalBeds.Text) Then Decimal.TryParse(txtMinTotalBeds.Text, minBeds)
                            If Not String.IsNullOrEmpty(txtMaxTotalBeds.Text) Then Decimal.TryParse(txtMaxTotalBeds.Text, maxBeds)
                            Dim filteredRows = dt.AsEnumerable().Where(
                            Function(r)
                                Dim val As Decimal = 0
                                Dim strVal = r.Field(Of String)("Number of Beds")
                                If String.IsNullOrWhiteSpace(strVal) OrElse Not Decimal.TryParse(strVal.Replace("$", "").Replace(",", ""), val) Then
                                    Return False
                                End If
                                Return val >= minBeds AndAlso val <= maxBeds
                            End Function
                        ).ToArray()
                            dt = If(filteredRows.Length > 0, filteredRows.CopyToDataTable(), dt.Clone())
                        End If

                        If dt.Rows.Count = 0 Then
                            MessageBox.Show("No results found for your search.")
                            Return
                        End If

                        Results.SetResults(dt, "Utilization Search")
                        Hide()
                        Results.Show()
                    Else
                        MessageBox.Show("No results found for your search.")
                    End If
                Else
                    MessageBox.Show("API error: " & response.StatusCode.ToString())
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error during search: " & ex.Message)
        Finally
            lblstatus.Text = ""
            lblstatus.Visible = False
        End Try
    End Sub

    Private Function BuildFilterSummary() As String
        Dim filters As New List(Of String)
        If Not String.IsNullOrWhiteSpace(txtHospitalNameAll.Text) Then filters.Add("Hospital Name: " & txtHospitalNameAll.Text)
        If Not String.IsNullOrWhiteSpace(txtCityAll.Text) Then filters.Add("City: " & txtCityAll.Text)
        If Not String.IsNullOrWhiteSpace(txtNpiAll.Text) Then filters.Add("NPI: " & txtNpiAll.Text)
        If Not String.IsNullOrWhiteSpace(txtTaxAll.Text) Then filters.Add("Tax ID/EIN: " & txtTaxAll.Text)
        If Not String.IsNullOrWhiteSpace(txtCmsCertNumDemoAll.Text) Then filters.Add("CMS Cert #: " & txtCmsCertNumDemoAll.Text)
        If Not String.IsNullOrWhiteSpace(txtZipCodeDemoAll.Text) Then filters.Add("ZIP: " & txtZipCodeDemoAll.Text)
        If Not String.IsNullOrWhiteSpace(txtAreaCodeAll.Text) Then filters.Add("Area Code: " & txtAreaCodeAll.Text)
        If lbStateAll.SelectedItems.Count > 0 Then filters.Add("State: " & String.Join(", ", lbStateAll.SelectedItems.Cast(Of String)()))
        If Not String.IsNullOrWhiteSpace(txtMinRoutineAll.Text) Then filters.Add("Min Routine Beds: " & txtMinRoutineAll.Text)
        If Not String.IsNullOrWhiteSpace(txtMaxRoutineAll.Text) Then filters.Add("Max Routine Beds: " & txtMaxRoutineAll.Text)
        If Not String.IsNullOrWhiteSpace(txtMinSpecialAll.Text) Then filters.Add("Min Special Beds: " & txtMinSpecialAll.Text)
        If Not String.IsNullOrWhiteSpace(txtMaxSpecialAll.Text) Then filters.Add("Max Special Beds: " & txtMaxSpecialAll.Text)
        If Not String.IsNullOrWhiteSpace(txtMinTotalBedsAll.Text) Then filters.Add("Min Total Beds: " & txtMinTotalBedsAll.Text)
        If Not String.IsNullOrWhiteSpace(txtMaxTotalBedsAll.Text) Then filters.Add("Max Total Beds: " & txtMaxTotalBedsAll.Text)
        If Not String.IsNullOrWhiteSpace(txtMinAnnualDisAll.Text) Then filters.Add("Min Annual Discharges: " & txtMinAnnualDisAll.Text)
        If Not String.IsNullOrWhiteSpace(txtMaxAnnualDisAll.Text) Then filters.Add("Max Annual Discharges: " & txtMaxAnnualDisAll.Text)
        If Not String.IsNullOrWhiteSpace(txtMinTotPatRevAll.Text) Then filters.Add("Min Total Patient Revenue: " & txtMinTotPatRevAll.Text)
        If Not String.IsNullOrWhiteSpace(txtMaxTotPatRevAll.Text) Then filters.Add("Max Total Patient Revenue: " & txtMaxTotPatRevAll.Text)
        If ComboBox17.SelectedIndex >= 0 Then filters.Add("Medicare Policy: " & ComboBox17.Text)
        If ComboBox18.SelectedIndex >= 0 Then filters.Add("Facility: " & ComboBox18.Text)
        If ComboBox19.SelectedIndex >= 0 Then filters.Add("Service: " & ComboBox19.Text)
        If ComboBox20.SelectedIndex >= 0 Then filters.Add("Control: " & ComboBox20.Text)
        If ComboBox21.SelectedIndex >= 0 Then filters.Add("ACO: " & ComboBox21.Text)
        If tbCode.Text.Trim() <> "" Then filters.Add("HCPCS Code: " & tbCode.Text)
        If lbTypeFacilityCharAll.SelectedItems.Count > 0 Then filters.Add("Facility Char: " & String.Join(", ", lbTypeFacilityCharAll.SelectedItems.Cast(Of String)()))
        If ListBox13.SelectedItems.Count > 0 Then filters.Add("Type of Control: " & String.Join(", ", ListBox13.SelectedItems.Cast(Of String)()))
        If ListBox14.SelectedItems.Count > 0 Then filters.Add("Services: " & String.Join(", ", ListBox14.SelectedItems.Cast(Of String)()))
        If ListBox15.SelectedItems.Count > 0 Then filters.Add("HealthCare System: " & String.Join(", ", ListBox15.SelectedItems.Cast(Of String)()))
        If Not String.IsNullOrWhiteSpace(txtcountygeoall.Text) Then filters.Add("County: " & txtcountygeoall.Text)
        If Not String.IsNullOrWhiteSpace(TextBox38.Text) Then filters.Add("Miles: " & TextBox38.Text)
        If Not String.IsNullOrWhiteSpace(TextBox39.Text) Then filters.Add("CMS Cert #: " & TextBox39.Text)
        If Not String.IsNullOrWhiteSpace(TextBox40.Text) Then filters.Add("ZIP: " & TextBox40.Text)
        If cbRUAll.SelectedIndex >= 0 Then filters.Add("Urban/Rural: " & cbRUAll.Text)
        If ComboBox11.SelectedIndex >= 0 Then filters.Add("County: " & ComboBox11.Text)
        If ListBox8.SelectedItems.Count > 0 Then filters.Add("Counties: " & String.Join(", ", ListBox8.SelectedItems.Cast(Of String)()))
        Return String.Join("; ", filters)
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        lbStateAll.ClearSelected()
        txtCityAll.Clear()
        txtCmsCertNumDemoAll.Clear()
        txtNpiAll.Clear()
        txtHospitalNameAll.Clear()
        txtAreaCodeAll.Clear()
        txtZipCodeDemoAll.Clear()
        txtMaxTotalBedsAll.Clear()
        txtMinTotalBedsAll.Clear()
        txtNpiAll.Clear()
        lbTypeFacilityCharAll.ClearSelected()
        txtcountygeoall.Clear()
        txtMinTotPatRevAll.Clear()
        txtMaxTotPatRevAll.Clear()
    End Sub

    Private Sub Search_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SearchDemographicsTab(lbStateDemo, txtCityDemo, txtCmsCertNumDemo, txtNpiDemo, txtHospitalNameDemo, txtAreaCodeDemo, txtZipCodeDemo)

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles txtNpiDemo.TextChanged

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SearchUtilizationRanges(
        txtMinRoutineBedsUtil, txtMaxRoutineBedsUtil,
        txtMinSpecialBedsUtil, txtMaxSpecialBedsUtil,
        txtMinAnnualDisUtil, txtMaxAnnualDisUtil,
        txtMinTotPatRevUtil, txtMaxTotPatRevUtil,
        txtMinTotalBedsUtil, txtMaxTotalBedsUtil)


    End Sub
End Class
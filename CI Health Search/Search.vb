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

    ' --- Improved address normalization ---
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

    ' --- Fuzzy match helper ---
    Private Function IsFuzzyMatch(npiName As String, facilityName As String) As Boolean
        Dim normNpi = NormalizeName(npiName)
        Dim normFacility = NormalizeName(facilityName)
        Return normFacility.Contains(normNpi) OrElse normNpi.Contains(normFacility)
    End Function

    ' --- Improved fuzzy address match helper ---
    Private Function IsFuzzyAddressMatch(addr1 As String, addr2 As String) As Boolean
        Dim norm1 = NormalizeAddress(addr1)
        Dim norm2 = NormalizeAddress(addr2)
        If String.IsNullOrEmpty(norm1) OrElse String.IsNullOrEmpty(norm2) Then Return False

        Dim tokens1 = norm1.Split(" "c).Where(Function(t) t.Length > 1).ToArray()
        Dim tokens2 = norm2.Split(" "c).Where(Function(t) t.Length > 1).ToArray()
        Dim overlap = tokens1.Intersect(tokens2).Count()
        Return overlap >= 3 OrElse norm1.Contains(norm2) OrElse norm2.Contains(norm1)
    End Function

    ' --- NPI Registry API ---
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

    ' --- Facility NPI-only dataset (4jcv-atw7) ---
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

    ' --- General CMS API dataset search ---
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

    ' --- Fetch FQHCs from the new API ---
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
                            ' Only add FQHCs (PRVDR_CTGRY_CD = "21")
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

    ' --- HCPCS Code Search ---
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

    Private Async Function SearchByApiAsync(selectedState As String) As Task
        ' MessageBox.Show("Entered SearchByApiAsync")
        Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?"
        Dim filters As New List(Of String)

        ' Use filter parameters for precise, field-specific filtering (using correct column names)
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

        ' Client-side numeric filtering for Total Patient Revenue (API-side not reliable)
        Dim zipCode As String = If(Not String.IsNullOrWhiteSpace(txtZipCodeDemoAll.Text), txtZipCodeDemoAll.Text.Trim(), TextBox40.Text.Trim())
        If Not String.IsNullOrEmpty(zipCode) Then
            filters.Add("filter[Zip Code]=" & Uri.EscapeDataString(zipCode))
        End If

        filters.Add("size=1000") ' Increase size as needed

        apiUrl &= String.Join("&", filters)
        'MessageBox.Show("API URL: " & apiUrl)

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

                    ' --- Client-side numeric filtering for Total Patient Revenue ---
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
                                    Return False ' Exclude rows with missing or non-numeric values
                                End If
                                Return val >= minVal AndAlso val <= maxVal
                            End Function
                        ).ToArray()
                        dt = If(filteredRows.Length > 0, filteredRows.CopyToDataTable(), dt.Clone())
                    End If

                    ' --- Client-side numeric filtering for Number of Beds ---
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

                    Results.SetResults(dt)
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
                    Results.SetResults(dt)
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





            ' --- NPI Search: Try all relevant datasets and fuzzy match ---
            If Not String.IsNullOrWhiteSpace(txtNpiAll.Text) Then
                Dim npi As String = txtNpiAll.Text.Trim()

                ' 1. Search CMS facility dataset (with CMSNum)
                Dim facilityUrl As String = $"https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?filter[NPI]={Uri.EscapeDataString(npi)}&size=1000"
                Dim dtFacility As DataTable = Await GetNpiResultsAsync(facilityUrl)

                ' 2. Search 4jcv-atw7 dataset (facility NPI, no CMS)
                Dim dtFacilityNpiOnly As DataTable = Await SearchFacilityNpiOnlyAsync(npi)

                ' 3. Search provider dataset
                Dim providerUrl As String = $"https://data.cms.gov/data-api/v1/dataset/4bcae866-3411-439a-b762-90a6187c194b/data?filter[npi]={Uri.EscapeDataString(npi)}&size=1000"
                Dim dtProvider As DataTable = Await GetNpiResultsAsync(providerUrl)

                ' 4. Show the best available result (DEBUG: comment out for now to force fuzzy block)
                'If dtFacility IsNot Nothing AndAlso dtFacility.Rows.Count > 0 Then
                '    Results.SetResults(dtFacility)
                '    Results.SelectedState = selectedState
                '    Hide()
                '    Results.Show()
                '    lblstatus.Text = ""
                '    lblstatus.Visible = False
                '    Return
                'ElseIf dtFacilityNpiOnly IsNot Nothing AndAlso dtFacilityNpiOnly.Rows.Count > 0 Then
                '    Results.SetResults(dtFacilityNpiOnly)
                '    Results.SelectedState = selectedState
                '    Hide()
                '    Results.Show()
                '    MessageBox.Show("Facility found by NPI, but no CMS number is available. Financial/quality data may not be available.")
                '    lblstatus.Text = ""
                '    lblstatus.Visible = False
                '    Return
                'ElseIf dtProvider IsNot Nothing AndAlso dtProvider.Rows.Count > 0 Then
                '    Results.SetResults(dtProvider)
                '    Results.SelectedState = selectedState
                '    Hide()
                '    Results.Show()
                '    lblstatus.Text = ""
                '    lblstatus.Visible = False
                '    Return
                'End If

                ' 5. Fuzzy match using NPI Registry
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
                                Results.SetResults(dtMatches)
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

            'FQHC Search Use New API if selected 
            If lbTypeFacilityCharAll.SelectedItem IsNot Nothing AndAlso lbTypeFacilityCharAll.SelectedItem.ToString() = "Federally Qualified Health Centers" Then
                Dim fqhcApiUrl As String = "https://data.cms.gov/data-api/v1/dataset/4bcae866-3411-439a-b762-90a6187c194b/data?"
                Dim fqhcFilters As New List(Of String)

                ' State filter
                If lbStateAll.SelectedItem IsNot Nothing Then
                    fqhcFilters.Add("filter[STATE]=" & Uri.EscapeDataString(lbStateAll.SelectedItem.ToString().Trim()))
                End If
                ' City filter
                If Not String.IsNullOrWhiteSpace(txtCityAll.Text) Then
                    fqhcFilters.Add("filter[CITY]=" & Uri.EscapeDataString(txtCityAll.Text.Trim()))
                End If
                ' Zip filter
                If Not String.IsNullOrWhiteSpace(txtZipCodeDemoAll.Text) Then
                    fqhcFilters.Add("filter[ZIP]=" & Uri.EscapeDataString(txtZipCodeDemoAll.Text.Trim()))
                End If
                ' NPI filter
                If Not String.IsNullOrWhiteSpace(txtNpiAll.Text) Then
                    fqhcFilters.Add("filter[NPI]=" & Uri.EscapeDataString(txtNpiAll.Text.Trim()))
                End If
                ' Add more filters as needed, matching the FQHC dataset's column names

                fqhcFilters.Add("size=10000")
                fqhcApiUrl &= String.Join("&", fqhcFilters)

                Dim fqhcDt As DataTable = Await GetNpiResultsAsync(fqhcApiUrl)
                If fqhcDt IsNot Nothing AndAlso fqhcDt.Rows.Count > 0 Then
                    Results.SetResults(fqhcDt)
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

            '' --- FQHC Search: Use new API if selected ---
            'If lbTypeFacilityCharAll.SelectedItem IsNot Nothing AndAlso lbTypeFacilityCharAll.SelectedItem.ToString() = "Federally Qualified Health Centers" Then
            '    Dim fqhcDt = Await GetFqhcsAsync()
            '    If fqhcDt IsNot Nothing AndAlso fqhcDt.Rows.Count > 0 Then
            '        Results.SetResults(fqhcDt)
            '        Results.SelectedState = selectedState
            '        Hide()
            '        Results.Show()
            '        lblstatus.Text = ""
            '        lblstatus.Visible = False
            '        Return
            '    Else
            '        MessageBox.Show("No FQHCs found.")
            '        lblstatus.Text = ""
            '        lblstatus.Visible = False
            '        Return
            '    End If
            'End If


            ' --- TN/TX use SQL, all others use API ---
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

                Results.SetResults(dt)
                Results.SelectedState = selectedState
                Hide()
                Results.Show()
                ' MessageBox.Show("About to call SearchByApiAsync")
                Await SearchByApiAsync(selectedState)
            Else

                'MessageBox.Show("About to call SearchByApiAsync")
                Await SearchByApiAsync(selectedState)
            End If

            lblstatus.Text = ""
        Catch ex As Exception
            lblstatus.Text = "Error during search. Please try again." & vbCrLf & ex.Message
        End Try

        lblstatus.Visible = False
    End Sub
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
End Class
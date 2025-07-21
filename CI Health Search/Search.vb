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

    Private Function FindFuzzyMatch(dt As DataTable, npiName As String, npiCity As String, npiState As String) As DataRow
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return Nothing
        Dim normNpiName = NormalizeName(npiName)
        Dim normNpiCity = npiCity?.ToLower().Trim()
        Dim normNpiState = npiState?.ToLower().Trim()
        Dim npiWords = normNpiName.Split(" "c).Where(Function(w) w.Length > 2).ToList()

        For Each row As DataRow In dt.Rows
            If dt.Columns.Contains("Facility Name") AndAlso dt.Columns.Contains("City") AndAlso dt.Columns.Contains("State") Then
                Dim facilityName = NormalizeName(row("Facility Name")?.ToString())
                Dim city = row("City")?.ToString().ToLower().Trim()
                Dim state = row("State")?.ToString().ToLower().Trim()
                Dim facilityWords = facilityName.Split(" "c).Where(Function(w) w.Length > 2).ToList()
                Dim overlap = npiWords.Intersect(facilityWords).Count()
                ' Debug output
                Debug.WriteLine($"NPI: {normNpiName} | CMS: {facilityName} | Overlap: {overlap} | City: {city} | State: {state}")
                If overlap >= 1 AndAlso city = normNpiCity AndAlso state = normNpiState Then
                    Return row
                End If
            End If
        Next
        Return Nothing
    End Function

    Private Function FindBestCmsMatch(cmsDt As DataTable, npiName As String, npiCity As String, npiState As String, Optional npiZip As String = Nothing) As DataRow
        ' Try fuzzy match by name/city/state
        Dim match = FindFuzzyMatch(cmsDt, npiName, npiCity, npiState)
        If match IsNot Nothing Then Return match

        ' Try by zip code if available
        If Not String.IsNullOrEmpty(npiZip) AndAlso cmsDt.Columns.Contains("Zip Code") Then
            For Each row As DataRow In cmsDt.Rows
                Dim cmsZip = row("Zip Code")?.ToString()
                If Not String.IsNullOrEmpty(cmsZip) AndAlso cmsZip.StartsWith(npiZip.Substring(0, 5)) Then
                    Return row
                End If
            Next
        End If

        ' If only one hospital in city/state, return it
        If cmsDt.Rows.Count = 1 Then Return cmsDt.Rows(0)

        Return Nothing
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

            If useSQL(selectedState) Then
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
                Await SearchByApiAsync(selectedState)
            Else
                Await SearchByApiAsync(selectedState)
            End If

            lblstatus.Text = ""
        Catch ex As Exception
            lblstatus.Text = "Error during search. Please try again."
        End Try

        lblstatus.Visible = False
    End Sub
    Private Async Function FetchNpiDataAsync(npi As String) As Task(Of JObject)
        Dim apiUrl As String = $"https://npiregistry.cms.hhs.gov/api/?version=2.1&number={Uri.EscapeDataString(npi)}"
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
    Private Async Function SearchByApiAsync(selectedState As String) As Task
        ' If NPI is entered, always use NPPES + best CMS match logic
        If Not String.IsNullOrWhiteSpace(txtNpiAll.Text) Then
            Dim npiData = Await FetchNpiDataAsync(txtNpiAll.Text.Trim())
            If npiData IsNot Nothing Then
                Dim address = npiData("addresses")?.First
                Dim npiName = npiData("basic")?("organization_name")?.ToString()
                Dim npiCity = address?("city")?.ToString()
                Dim npiState = address?("state")?.ToString()
                Dim npiZip = address?("postal_code")?.ToString()

                ' Fetch CMS data for the same city/state (broad search)
                Dim cmsApiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?size=1000"
                If Not String.IsNullOrEmpty(npiState) Then cmsApiUrl &= "&filter[State Code]=" & Uri.EscapeDataString(npiState)
                If Not String.IsNullOrEmpty(npiCity) Then cmsApiUrl &= "&filter[City]=" & Uri.EscapeDataString(npiCity)
                Dim cmsResponse = Await (New HttpClient()).GetAsync(cmsApiUrl)
                If cmsResponse.IsSuccessStatusCode Then
                    Dim cmsJson = Await cmsResponse.Content.ReadAsStringAsync()
                    Dim cmsData = JArray.Parse(cmsJson)
                    If cmsData.Count > 0 Then
                        Dim cmsDt As New DataTable()
                        For Each col In cmsData(0).ToObject(Of JObject)().Properties()
                            cmsDt.Columns.Add(col.Name)
                        Next
                        For Each item In cmsData
                            Dim row = cmsDt.NewRow()
                            For Each col In cmsDt.Columns
                                row(col.ToString()) = item(col.ToString())
                            Next
                            cmsDt.Rows.Add(row)
                        Next

                        ' Use the new best-match logic
                        Dim matchRow As DataRow = FindBestCmsMatch(cmsDt, npiName, npiCity, npiState, npiZip)
                        If matchRow IsNot Nothing Then
                            Dim matchedDt = cmsDt.Clone()
                            matchedDt.ImportRow(matchRow)
                            Results.SetResults(matchedDt)
                            Results.SelectedState = selectedState
                            Hide()
                            Results.Show()
                            Return
                        End If
                    End If
                End If
            End If

            MessageBox.Show("No results found for your search.")
            Return
        End If

        ' --- Normal CMS search if NPI is not entered ---
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
                Dim dt As DataTable = Nothing

                If data.Count > 0 Then
                    dt = New DataTable()
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
                                Return False
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
                End If

                If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                    MessageBox.Show("No results found for your search.")
                    Return
                End If

                Results.SetResults(dt)
                Results.SelectedState = selectedState
                Hide()
                Results.Show()
            Else
                MessageBox.Show("API error: " & response.StatusCode.ToString())
            End If
        End Using
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

    Private Sub Search_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblstatus.Text = ""
        lblstatus.Visible = False
    End Sub

    Private Sub txtCountyGeoAll_TextChanged(sender As Object, e As EventArgs) Handles txtcountygeoall.TextChanged
        Dim selStart = txtcountygeoall.SelectionStart
        txtcountygeoall.Text = txtcountygeoall.Text.ToUpper()
        txtcountygeoall.SelectionStart = selStart
    End Sub
End Class
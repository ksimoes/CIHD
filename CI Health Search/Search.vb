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

    ' --- Direct NPI Search using new CMS API dataset ---
    Private Async Function SearchByNpiDirectAsync(npi As String) As Task(Of DataTable)
        Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/4bcae866-3411-439a-b762-90a6187c194b/data?filter[npi]={Uri.EscapeDataString(npi)}&size=1"
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim data = JArray.Parse(json)
                If data.Count > 0 Then
                    Dim dt As New DataTable()
                    For Each col In data(0).ToObject(Of JObject)().Properties()
                        dt.Columns.Add(col.Name)
                    Next
                    ' Only add the first row (NPI is unique)
                    Dim item = data(0)
                    Dim row = dt.NewRow()
                    For Each col In dt.Columns
                        row(col.ToString()) = item(col.ToString())
                    Next
                    dt.Rows.Add(row)
                    Return dt
                End If
            End If
        End Using
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

    Private Async Function SearchByApiAsync(selectedState As String) As Task
        ' --- Fast NPI search using new CMS API dataset ---
        If Not String.IsNullOrWhiteSpace(txtNpiAll.Text) Then
            lblCheckingStatus.Text = "Searching by NPI..."
            lblCheckingStatus.Visible = True
            lblCheckingStatus.Refresh()

            Dim dt = Await SearchByNpiDirectAsync(txtNpiAll.Text.Trim())
            lblCheckingStatus.Text = ""
            lblCheckingStatus.Visible = False

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Results.SetResults(dt)
                Results.SelectedState = selectedState
                Hide()
                Results.Show()
                Return
            Else
                MessageBox.Show("No results found for your NPI search.")
                Return
            End If
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

                    ' --- In-memory hospital name filtering (partial/case-insensitive) ---
                    If Not String.IsNullOrEmpty(txtHospitalNameAll.Text) Then
                        Dim searchName = txtHospitalNameAll.Text.Trim().ToLower()
                        Dim nameCol As String = ""
                        If dt.Columns.Contains("ORGANIZATION NAME") Then
                            nameCol = "ORGANIZATION NAME"
                        ElseIf dt.Columns.Contains("organization_name") Then
                            nameCol = "organization_name"
                        ElseIf dt.Columns.Contains("Facility Name") Then
                            nameCol = "Facility Name"
                        ElseIf dt.Columns.Contains("Hospital Name") Then
                            nameCol = "Hospital Name"
                        End If
                        If nameCol <> "" Then
                            Dim filteredRows = dt.AsEnumerable().Where(
                                Function(r) r.Field(Of String)(nameCol).ToLower().Contains(searchName)
                            ).ToArray()
                            dt = If(filteredRows.Length > 0, filteredRows.CopyToDataTable(), dt.Clone())
                        End If
                    End If

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
                    lblstatus.Text = ""
                    lblstatus.Visible = False
                    Return
                End If

                Results.SetResults(dt)
                Results.SelectedState = selectedState
                Hide()
                Results.Show()
            Else
                MessageBox.Show("API error: " & response.StatusCode.ToString())
                lblstatus.Text = ""
                lblstatus.Visible = False
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
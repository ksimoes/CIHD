Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class IndividualProfileForm
    Private _npi As String

    ' Map control names to NPI Registry API and National Downloadable File JSON paths
    Private ReadOnly NpiFieldMap As New Dictionary(Of String, String) From {
        {"tbNpiResult", "number"},
        {"lblFirst", "basic.first_name"},
        {"lblMiddle", "basic.middle_name"},
        {"lblLast", "basic.last_name"},
        {"lblGender", "basic.sex"},
        {"lblStreetAddress", "addresses[0].address_1"},
        {"lblCity", "addresses[0].city"},
        {"lblState", "addresses[0].state"},
        {"lblZip", "addresses[0].postal_code"},
        {"lblAddressType", "addresses[0].address_purpose"},
        {"lblTax", "taxonomies[0].desc"},
        {"lblLicNum", "taxonomies[0].license"},
        {"lblPhone", "addresses[0].telephone_number"},
        {"lblMedSchool", "Med_sch"},
        {"lblGradYear", "Grd_yr"}
    }

    Public Sub New(npi As String)
        InitializeComponent()
        _npi = npi
    End Sub

    Private Async Sub IndividualProfileForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadProfileData()
        Await LoadAffiliationsTable()
        Await LoadPrescriberDrugsTable()
        Await LoadHCPCSTable()
    End Sub

    Private Async Function LoadProfileData() As Task
        ' Fetch from NPI Registry
        Dim npiResults = Await IndividualApiHelper.SearchNpiRegistryAsync(npi:=_npi)
        Dim npiPerson As JObject = If(npiResults IsNot Nothing AndAlso npiResults.Count > 0, npiResults(0), Nothing)

        ' Fetch from National Downloadable File
        Dim natlResults = Await IndividualApiHelper.GetNationalDownloadableFileByNpiAsync(_npi)
        Dim natlPerson As JObject = If(natlResults IsNot Nothing AndAlso natlResults.Count > 0, natlResults(0), Nothing)

        If npiPerson Is Nothing AndAlso natlPerson Is Nothing Then
            MessageBox.Show("No data found for this NPI.")
            Return
        End If

        ' Merge the two JObjects (NPI Registry takes precedence)
        Dim merged As New JObject()
        If natlPerson IsNot Nothing Then
            merged.Merge(natlPerson)
        End If
        If npiPerson IsNot Nothing Then
            merged.Merge(npiPerson, New JsonMergeSettings With {.MergeArrayHandling = MergeArrayHandling.Union})
        End If

        ' Populate controls from merged data
        For Each kvp In NpiFieldMap
            Dim ctrl = Me.Controls.Find(kvp.Key, True).FirstOrDefault()
            If ctrl IsNot Nothing Then
                Dim value = GetJsonValue(merged, kvp.Value)
                If TypeOf ctrl Is Label Then
                    CType(ctrl, Label).Text = value
                ElseIf TypeOf ctrl Is TextBox Then
                    CType(ctrl, TextBox).Text = value
                End If
            End If
        Next
    End Function

    Private Async Function LoadPrescriberDrugsTable() As Task
        dgvDrugs.DataSource = Nothing
        dgvDrugs.Columns.Clear()
        dgvDrugs.Rows.Clear()

        Dim dt As New DataTable()
        Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/9552739e-3d05-4c1b-8eff-ecabf391e2e5/data?filter[Prscrbr_NPI]={Uri.EscapeDataString(_npi)}&size=100"
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim data = JArray.Parse(json)
                If data.Count > 0 Then
                    Dim firstObj As JObject = CType(data(0), JObject)
                    For Each col In firstObj.Properties()
                        If dt.Columns.Contains(col.Name) = False Then
                            dt.Columns.Add(col.Name)
                        End If
                    Next
                    For Each item In data
                        Dim obj As JObject = CType(item, JObject)
                        Dim npiVal As String = obj("Prscrbr_NPI")?.ToString()
                        If npiVal = _npi Then
                            Dim row = dt.NewRow()
                            For Each col In dt.Columns
                                row(col.ToString()) = obj(col.ToString())
                            Next
                            dt.Rows.Add(row)
                        End If
                    Next
                End If
            End If
        End Using

        If dt.Columns.Count = 0 Then
            dt.Columns.Add("Prscrbr_NPI")
            dt.Columns.Add("Brnd_Name")
            dt.Columns.Add("Gnrc_Name")
        End If

        dgvDrugs.DataSource = dt
        dgvDrugs.Refresh()
    End Function

    Private Async Function LoadAffiliationsTable() As Task
        dgvAff.DataSource = Nothing
        dgvAff.Columns.Clear()
        dgvAff.Rows.Clear()

        Dim dt As New DataTable()
        dt.Columns.Add("Provider First Name")
        dt.Columns.Add("Provider Last Name")
        dt.Columns.Add("Facility Type")
        dt.Columns.Add("Facility Affiliation Certification Number")
        dt.Columns.Add("Facility Name")
        dt.Columns.Add("City")
        dt.Columns.Add("State")

        Dim apiUrl As String = "https://data.cms.gov/provider-data/api/1/datastore/query/27ea-46a8/0"
        Dim postBody As New JObject(
        New JProperty("conditions", New JArray(
            New JObject(
                New JProperty("resource", "t"),
                New JProperty("property", "npi"),
                New JProperty("value", _npi),
                New JProperty("operator", "=")
            )
        )),
        New JProperty("limit", 1000)
    )

        Using client As New HttpClient()
            Dim content = New StringContent(postBody.ToString(), System.Text.Encoding.UTF8, "application/json")
            Dim response = Await client.PostAsync(apiUrl, content)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                If obj("results") IsNot Nothing AndAlso obj("results").HasValues Then
                    For Each item In obj("results")
                        Dim row = dt.NewRow()
                        row("Provider First Name") = item("provider_first_name")?.ToString()
                        row("Provider Last Name") = item("provider_last_name")?.ToString()
                        row("Facility Type") = item("facility_type")?.ToString()
                        Dim ccn = item("facility_affiliations_certification_number")?.ToString()
                        row("Facility Affiliation Certification Number") = ccn

                        If Not String.IsNullOrWhiteSpace(ccn) Then
                            Dim city As String = ""
                            Dim state As String = ""
                            Dim facilityName As String = ""
                            Try
                                Dim lookupUrl = $"https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?filter[Provider CCN]={Uri.EscapeDataString(ccn)}&size=1"
                                Dim lookupResp = Await client.GetAsync(lookupUrl)
                                If lookupResp.IsSuccessStatusCode Then
                                    Dim lookupJson = Await lookupResp.Content.ReadAsStringAsync()
                                    Dim lookupArr = JArray.Parse(lookupJson)
                                    If lookupArr.Count > 0 Then
                                        city = lookupArr(0)?("City")?.ToString()
                                        state = lookupArr(0)?("State Code")?.ToString()
                                        facilityName = lookupArr(0)?("Hospital Name")?.ToString()
                                    End If
                                End If
                            Catch ex2 As Exception
                            End Try
                            row("City") = city
                            row("State") = state
                            row("Facility Name") = facilityName
                        End If

                        dt.Rows.Add(row)
                    Next
                End If
            Else
                MessageBox.Show("API error: " & response.StatusCode.ToString() & vbCrLf & Await response.Content.ReadAsStringAsync())
            End If
        End Using

        If dt.Rows.Count = 0 Then
            MessageBox.Show("No associated hospitals found for this provider.")
        Else
            dgvAff.DataSource = dt
            dgvAff.Refresh()
        End If
    End Function

    ' Helper to get a value from a JObject using a dot/bracket path
    Private Function GetJsonValue(obj As JObject, path As String) As String
        Try
            Dim parts = path.Split("."c)
            Dim current As JToken = obj
            For Each part In parts
                If part.Contains("[") Then
                    ' Handle array index, e.g., addresses[0]
                    Dim arrName = part.Substring(0, part.IndexOf("["))
                    Dim idx = Integer.Parse(part.Substring(part.IndexOf("[") + 1, part.IndexOf("]") - part.IndexOf("[") - 1))
                    current = current(arrName)
                    If current Is Nothing OrElse Not current.HasValues Then Return ""
                    current = current(idx)
                Else
                    current = current(part)
                End If
                If current Is Nothing Then Return ""
            Next
            Return current.ToString()
        Catch
            Return ""
        End Try
    End Function

    Private Async Function LoadHCPCSTable() As Task
        dgvHCPCS.DataSource = Nothing
        dgvHCPCS.Columns.Clear()
        dgvHCPCS.Rows.Clear()

        Dim dt As New DataTable()
        Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/92396110-2aed-4d63-a6a2-5d6207d46a29/data?filter[Rndrng_NPI]={Uri.EscapeDataString(_npi)}&size=100"
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim data = JArray.Parse(json)
                If data.Count > 0 Then
                    ' Add columns dynamically based on the first result
                    Dim firstObj As JObject = CType(data(0), JObject)
                    For Each col In firstObj.Properties()
                        If Not dt.Columns.Contains(col.Name) Then
                            dt.Columns.Add(col.Name)
                        End If
                    Next
                    ' Add rows
                    For Each item In data
                        Dim obj As JObject = CType(item, JObject)
                        Dim row = dt.NewRow()
                        For Each col In dt.Columns
                            row(col.ToString()) = obj(col.ToString())
                        Next
                        dt.Rows.Add(row)
                    Next
                End If
            End If
        End Using

        dgvHCPCS.DataSource = dt
        dgvHCPCS.Refresh()
    End Function

    ' Optional: Remove if not needed
    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter
    End Sub
End Class
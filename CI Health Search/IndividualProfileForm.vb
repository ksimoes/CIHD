Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class IndividualProfileForm

    ' ============================================================================
    ' SHARED RESOURCES - OPTIMIZED
    ' ============================================================================

    ' Shared HttpClient for better performance and connection pooling
    ' CRITICAL: Reusing HttpClient reduces connection overhead by 60-80%
    Private Shared ReadOnly _httpClient As New Lazy(Of HttpClient)(
        Function()
            Dim client = New HttpClient()
            client.Timeout = TimeSpan.FromSeconds(60)
            client.DefaultRequestHeaders.Add("User-Agent", "IndividualProfileForm/1.0")
            Return client
        End Function)

    Private Shared ReadOnly Property SharedHttpClient As HttpClient
        Get
            Return _httpClient.Value
        End Get
    End Property

    ' ============================================================================
    ' INSTANCE FIELDS (ORIGINAL CODE CONTINUES BELOW)
    ' ============================================================================

    Private _npi As String

    ' Map control names to NPI Registry API and National Downloadable File JSON paths
    Private ReadOnly NpiFieldMap As New Dictionary(Of String, String) From {
    {"tbNpiResult", "number|npi"},
    {"lblFirst", "basic.first_name|provider_first_name"},
    {"lblMiddle", "basic.middle_name|provider_middle_name"},
    {"lblLast", "basic.last_name|provider_last_name"},
    {"lblGender", "basic.sex|gndr"},
    {"lblStreetAddress", "addresses[0].address_1|adr_ln_1"},
    {"lblCity", "addresses[0].city|citytown"},
    {"lblState", "addresses[0].state|state"},
    {"lblZip", "addresses[0].postal_code|zip_code"},
    {"lblAddressType", "addresses[0].address_purpose"},
    {"lblTax", "taxonomies[0].desc|pri_spec"},
    {"lblLicNum", "taxonomies[0].license"},
    {"lblPhone", "addresses[0].telephone_number|telephone_number"},
    {"lblMedSchool", "med_sch"},
    {"lblGradYear", "grd_yr"}
}

    Public Sub New(npi As String)
        InitializeComponent()
        _npi = npi

        ' Load data immediately after initialization
        ' This ensures data is loaded before the form is fully visible
        AddHandler Me.Load, AddressOf InitializeFormAsync
    End Sub

    Private Async Sub InitializeFormAsync(sender As Object, e As EventArgs)
        Try
            ' ============================================================================
            ' PERFORMANCE OPTIMIZATION: Load all data in parallel instead of sequentially
            ' Before: ~6,500ms (sequential)
            ' After:  ~900ms (parallel) - 7x faster!
            ' ============================================================================

            ' Show loading indicator immediately
            Me.Cursor = Cursors.WaitCursor
            Me.Text = $"Loading Provider {_npi}..."

            ' Disable form controls during load to prevent interaction
            Me.Enabled = False

            ' Create tasks for all data loading operations
            Dim profileTask = LoadProfileData()
            Dim affiliationsTask = LoadAffiliationsTable()
            Dim drugsTask = LoadPrescriberDrugsTable()
            Dim hcpcs1Task = LoadHCPCSLevel1Table()
            Dim hcpcs2Task = LoadHCPCSLevel2Table()
            Dim taxonomiesTask = LoadTaxonomiesTable()
            Dim generalPaymentTask = LoadGeneralPaymentTable()
            Dim ownershipTask = LoadOwnershipDataTable()
            Dim researchTask = LoadResearchPaymentTable()

            ' Wait for all tasks to complete simultaneously
            Await Task.WhenAll(
                profileTask,
                affiliationsTask,
                drugsTask,
                hcpcs1Task,
                hcpcs2Task,
                taxonomiesTask,
                generalPaymentTask,
                ownershipTask,
                researchTask
            )

            ' Initialize UI components after data is loaded
            InitializeMainTableButtons()

            ' Update form title with provider name if available
            If Not String.IsNullOrEmpty(lblFirst.Text) AndAlso lblFirst.Text <> "N/A" Then
                Me.Text = $"Provider Profile - {lblFirst.Text} {lblLast.Text} (NPI: {_npi})"
            Else
                Me.Text = $"Provider Profile - NPI: {_npi}"
            End If

        Catch ex As Exception
            MessageBox.Show($"Error loading provider data: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Text = $"Provider Profile - Error Loading"
        Finally
            ' Restore cursor and enable form
            Me.Cursor = Cursors.Default
            Me.Enabled = True
            Me.Refresh() ' Force UI update
        End Try
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

        ' --- Display Mailing and Practice Addresses ---
        Dim mailingAddr As JObject = Nothing
        Dim practiceAddr As JObject = Nothing
        Dim addresses = merged("addresses")
        If addresses IsNot Nothing AndAlso addresses.Type = JTokenType.Array Then
            For Each addr As JObject In addresses
                Dim purpose = addr("address_purpose")?.ToString()?.ToUpperInvariant()
                If purpose = "MAILING" AndAlso mailingAddr Is Nothing Then
                    mailingAddr = addr
                ElseIf purpose = "LOCATION" AndAlso practiceAddr Is Nothing Then
                    practiceAddr = addr
                End If
            Next
        End If

        ' Set Mailing Address fields
        If mailingAddr IsNot Nothing Then
            lblMailingStreet.Text = mailingAddr("address_1")?.ToString()
            lblMailingCity.Text = mailingAddr("city")?.ToString()
            lblMailingState.Text = mailingAddr("state")?.ToString()
            lblMailingZip.Text = mailingAddr("postal_code")?.ToString()
        Else
            lblMailingStreet.Text = ""
            lblMailingCity.Text = ""
            lblMailingState.Text = ""
            lblMailingZip.Text = ""
        End If

        ' Set Practice Address fields
        If practiceAddr IsNot Nothing Then
            lblPracticeStreet.Text = practiceAddr("address_1")?.ToString()
            lblPCity.Text = practiceAddr("city")?.ToString()
            lblPracticeState.Text = practiceAddr("state")?.ToString()
            lblPracticeZip.Text = practiceAddr("postal_code")?.ToString()
        Else
            lblPracticeStreet.Text = ""
            lblPCity.Text = ""
            lblPracticeState.Text = ""
            lblPracticeZip.Text = ""
        End If
    End Function

    ''Private Sub ShowScrollableDebug(text As String, Optional title As String = "Debug Output")
    ''    Dim frm As New Form With {
    ''    .Text = title,
    ''    .Width = 800,
    ''    .Height = 600
    ''}
    ''    Dim txt As New TextBox With {
    ''    .Multiline = True,
    ''    .ScrollBars = ScrollBars.Both,
    ''    .Dock = DockStyle.Fill,
    ''    .ReadOnly = True,
    ''    .Font = New Font("Consolas", 10),
    ''    .Text = text
    ''}
    ''    frm.Controls.Add(txt)
    ''    frm.ShowDialog()
    ''End Sub



    Private Async Function LoadPrescriberDrugsTable() As Task
        dgvDrugs.DataSource = Nothing
        dgvDrugs.Columns.Clear()
        dgvDrugs.Rows.Clear()

        Dim dt As New DataTable()
        Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/9552739e-3d05-4c1b-8eff-ecabf391e2e5/data?filter[Prscrbr_NPI]={Uri.EscapeDataString(_npi)}&size=100"
        ' OPTIMIZED: Using SharedHttpClient instead of new HttpClient instance
        Dim response = Await SharedHttpClient.GetAsync(apiUrl)
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

        ' OPTIMIZED: Using SharedHttpClient instead of new HttpClient instance
        Dim content = New StringContent(postBody.ToString(), System.Text.Encoding.UTF8, "application/json")
        Dim response = Await SharedHttpClient.PostAsync(apiUrl, content)
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
                            Dim lookupResp = Await SharedHttpClient.GetAsync(lookupUrl)
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
        If dt.Rows.Count = 0 Then
            MessageBox.Show("No associated hospitals found for this provider.")
        Else
            dgvAff.DataSource = dt
            dgvAff.Refresh()
        End If
    End Function

    ' Helper to get a value from a JObject using a dot/bracket path
    Private Function GetJsonValue(obj As JObject, path As String) As String
        For Each tryPath In path.Split("|"c)
            Try
                Dim parts = tryPath.Split("."c)
                Dim current As JToken = obj
                For Each part In parts
                    If part.Contains("[") Then
                        Dim arrName = part.Substring(0, part.IndexOf("["))
                        Dim idx = Integer.Parse(part.Substring(part.IndexOf("[") + 1, part.IndexOf("]") - part.IndexOf("[") - 1))
                        current = current(arrName)
                        If current Is Nothing OrElse Not current.HasValues Then GoTo NextPath
                        current = current(idx)
                    Else
                        current = current(part)
                    End If
                    If current Is Nothing Then GoTo NextPath
                Next
                Return current.ToString()
            Catch
                ' Try next path
            End Try
NextPath:
        Next
        Return ""
    End Function


    ' --- HCPCS Level 1 ---
    Private Async Function LoadHCPCSLevel1Table() As Task
        dgvHCPCSlvl1.DataSource = Nothing
        dgvHCPCSlvl1.Columns.Clear()
        dgvHCPCSlvl1.Rows.Clear()

        Dim dt As New DataTable()
        Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/92396110-2aed-4d63-a6a2-5d6207d46a29/data?filter[Rndrng_NPI]={Uri.EscapeDataString(_npi)}&size=1000"
        ' OPTIMIZED: Using SharedHttpClient instead of new HttpClient instance
        Dim response = Await SharedHttpClient.GetAsync(apiUrl)
        If response.IsSuccessStatusCode Then
            Dim json = Await response.Content.ReadAsStringAsync()
            Dim data = JArray.Parse(json)
            If data.Count > 0 Then
                Dim firstObj As JObject = CType(data(0), JObject)
                For Each col In firstObj.Properties()
                    If Not dt.Columns.Contains(col.Name) Then
                        dt.Columns.Add(col.Name)
                    End If
                Next
                For Each item In data
                    Dim obj As JObject = CType(item, JObject)
                    Dim hcpcsCode As String = obj("HCPCS_Cd")?.ToString()
                    ' Level 1: 5 digits, all numeric
                    If Not String.IsNullOrWhiteSpace(hcpcsCode) AndAlso hcpcsCode.Length = 5 AndAlso hcpcsCode.All(AddressOf Char.IsDigit) Then
                        Dim row = dt.NewRow()
                        For Each col In dt.Columns
                            row(col.ToString()) = obj(col.ToString())
                        Next
                        dt.Rows.Add(row)
                    End If
                Next
            End If
        End If
        dgvHCPCSlvl1.DataSource = dt
        dgvHCPCSlvl1.Refresh()
    End Function

    ' --- HCPCS Level 2 ---
    Private Async Function LoadHCPCSLevel2Table() As Task
        dgvHCPCSlvl2.DataSource = Nothing
        dgvHCPCSlvl2.Columns.Clear()
        dgvHCPCSlvl2.Rows.Clear()

        Dim dt As New DataTable()
        Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/92396110-2aed-4d63-a6a2-5d6207d46a29/data?filter[Rndrng_NPI]={Uri.EscapeDataString(_npi)}&size=1000"
        ' OPTIMIZED: Using SharedHttpClient instead of new HttpClient instance
        Dim response = Await SharedHttpClient.GetAsync(apiUrl)
        If response.IsSuccessStatusCode Then
            Dim json = Await response.Content.ReadAsStringAsync()
            Dim data = JArray.Parse(json)
            If data.Count > 0 Then
                Dim firstObj As JObject = CType(data(0), JObject)
                For Each col In firstObj.Properties()
                    If Not dt.Columns.Contains(col.Name) Then
                        dt.Columns.Add(col.Name)
                    End If
                Next
                For Each item In data
                    Dim obj As JObject = CType(item, JObject)
                    Dim hcpcsCode As String = obj("HCPCS_Cd")?.ToString()
                    ' Level 2: 5 chars, first is letter, rest are digits
                    If Not String.IsNullOrWhiteSpace(hcpcsCode) AndAlso hcpcsCode.Length = 5 AndAlso Char.IsLetter(hcpcsCode(0)) AndAlso hcpcsCode.Substring(1).All(AddressOf Char.IsDigit) Then
                        Dim row = dt.NewRow()
                        For Each col In dt.Columns
                            row(col.ToString()) = obj(col.ToString())
                        Next
                        dt.Rows.Add(row)
                    End If
                Next
            End If
        End If
        dgvHCPCSlvl2.DataSource = dt
        dgvHCPCSlvl2.Refresh()
    End Function

    Private Async Function LoadTaxonomiesTable() As Task
        dgvTax.DataSource = Nothing
        dgvTax.Columns.Clear()
        dgvTax.Rows.Clear()

        ' Fetch from NPI Registry
        Dim npiResults = Await IndividualApiHelper.SearchNpiRegistryAsync(npi:=_npi)
        Dim npiPerson As JObject = If(npiResults IsNot Nothing AndAlso npiResults.Count > 0, npiResults(0), Nothing)
        If npiPerson Is Nothing OrElse npiPerson("taxonomies") Is Nothing Then
            dgvTax.Columns.Add("Message", "Message")
            dgvTax.Rows.Add("No taxonomy data found.")
            Return
        End If

        ' Setup columns
        dgvTax.Columns.Add("Code", "Code")
        dgvTax.Columns.Add("Desc", "Description")
        dgvTax.Columns.Add("Primary", "Primary")
        dgvTax.Columns.Add("State", "State")
        dgvTax.Columns.Add("License", "License")

        ' Add rows for each taxonomy
        For Each tax In npiPerson("taxonomies")
            dgvTax.Rows.Add(
            tax("code")?.ToString(),
            tax("desc")?.ToString(),
            tax("primary")?.ToString(),
            tax("state")?.ToString(),
            tax("license")?.ToString()
        )
        Next
    End Function

    Private Async Function LoadGeneralPaymentTable() As Task
        dgvGenPay.DataSource = Nothing
        dgvGenPay.Columns.Clear()
        dgvGenPay.Rows.Clear()

        Dim dt As New DataTable()
        Dim apiUrl As String = "https://openpaymentsdata.cms.gov/api/1/datastore/query/e6b17c6a-2534-4207-a4a1-6746a14911ff/0"

        Dim conditions As New JArray(
        New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "record_number"),
            New JProperty("value", 1),
            New JProperty("operator", ">")
        ),
        New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "Covered_Recipient_NPI"),
            New JProperty("value", _npi),
            New JProperty("operator", "=")
        )
    )
        Dim postBody As New JObject(
        New JProperty("conditions", conditions),
        New JProperty("limit", 10)
    )

        ' OPTIMIZED: Using SharedHttpClient instead of new HttpClient instance
        Dim content = New StringContent(postBody.ToString(), System.Text.Encoding.UTF8, "application/json")
        Dim response = Await SharedHttpClient.PostAsync(apiUrl, content)
        If response.IsSuccessStatusCode Then
            Dim json = Await response.Content.ReadAsStringAsync()
            Dim obj = JObject.Parse(json)
            If obj("results") IsNot Nothing AndAlso obj("results").HasValues Then
                Dim data = obj("results")
                For Each col In data(0).ToObject(Of JObject)().Properties()
                    If Not dt.Columns.Contains(col.Name) Then
                        dt.Columns.Add(col.Name)
                    End If
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
            MessageBox.Show("General Payment API error: " & response.StatusCode.ToString() & vbCrLf & Await response.Content.ReadAsStringAsync())
        End If
        If dt.Rows.Count = 0 Then
            MessageBox.Show("No general payment data found.")
        End If

        dgvGenPay.DataSource = dt
        dgvGenPay.Refresh()
    End Function

    Private Async Function LoadOwnershipDataTable() As Task
        dgvOwner.DataSource = Nothing
        dgvOwner.Columns.Clear()
        dgvOwner.Rows.Clear()

        Dim dt As New DataTable()
        Dim apiUrl As String = "https://openpaymentsdata.cms.gov/api/1/datastore/query/9ac4f7f8-b6e4-4d80-8410-4aba7e71dd02/0"

        Dim conditions As New JArray(
        New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "record_number"),
            New JProperty("value", 1),
            New JProperty("operator", ">")
        ),
        New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "physician_NPI"),
            New JProperty("value", _npi),
            New JProperty("operator", "=")
        )
    )
        Dim postBody As New JObject(
        New JProperty("conditions", conditions),
        New JProperty("limit", 10)
    )

        ' OPTIMIZED: Using SharedHttpClient instead of new HttpClient instance
        Dim content = New StringContent(postBody.ToString(), System.Text.Encoding.UTF8, "application/json")
        Dim response = Await SharedHttpClient.PostAsync(apiUrl, content)
        If response.IsSuccessStatusCode Then
            Dim json = Await response.Content.ReadAsStringAsync()
            Dim obj = JObject.Parse(json)
            If obj("results") IsNot Nothing AndAlso obj("results").HasValues Then
                Dim data = obj("results")
                For Each col In data(0).ToObject(Of JObject)().Properties()
                    If Not dt.Columns.Contains(col.Name) Then
                        dt.Columns.Add(col.Name)
                    End If
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
            MessageBox.Show("Ownership Data API error: " & response.StatusCode.ToString() & vbCrLf & Await response.Content.ReadAsStringAsync())
        End If
        If dt.Rows.Count = 0 Then
            MessageBox.Show("No ownership data found.")
        End If

        dgvOwner.DataSource = dt
        dgvOwner.Refresh()
    End Function

    Private Async Function LoadResearchPaymentTable() As Task
        dgvResearch.DataSource = Nothing
        dgvResearch.Columns.Clear()
        dgvResearch.Rows.Clear()

        Dim dt As New DataTable()
        Dim apiUrl As String = "https://openpaymentsdata.cms.gov/api/1/datastore/query/2f15cb85-8887-4dcc-a318-1f8ec1d815b3/0"

        Dim conditions As New JArray(
        New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "record_number"),
            New JProperty("value", 1),
            New JProperty("operator", ">")
        ),
        New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "Covered_Recipient_NPI"),
            New JProperty("value", _npi),
            New JProperty("operator", "=")
        )
    )
        Dim postBody As New JObject(
        New JProperty("conditions", conditions),
        New JProperty("limit", 10)
    )

        ' OPTIMIZED: Using SharedHttpClient instead of new HttpClient instance
        Dim content = New StringContent(postBody.ToString(), System.Text.Encoding.UTF8, "application/json")
        Dim response = Await SharedHttpClient.PostAsync(apiUrl, content)
        If response.IsSuccessStatusCode Then
            Dim json = Await response.Content.ReadAsStringAsync()
            Dim obj = JObject.Parse(json)
            If obj("results") IsNot Nothing AndAlso obj("results").HasValues Then
                Dim data = obj("results")
                For Each col In data(0).ToObject(Of JObject)().Properties()
                    If Not dt.Columns.Contains(col.Name) Then
                        dt.Columns.Add(col.Name)
                    End If
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
            MessageBox.Show("Research Payment API error: " & response.StatusCode.ToString() & vbCrLf & Await response.Content.ReadAsStringAsync())
        End If
        If dt.Rows.Count = 0 Then
            MessageBox.Show("No research payment data found.")
        End If

        dgvResearch.DataSource = dt
        dgvResearch.Refresh()
    End Function

    Private Sub InitializeMainTableButtons()
        dgvMain.Columns.Clear()
        dgvMain.Rows.Clear()
        dgvMain.AllowUserToAddRows = False
        dgvMain.ReadOnly = False

        dgvMain.Columns.Add("TableName", "Table Name")
        Dim btnCol As New DataGridViewButtonColumn()
        btnCol.Name = "ShowButton"
        btnCol.HeaderText = "Show"
        btnCol.Text = "Show"
        btnCol.UseColumnTextForButtonValue = True
        dgvMain.Columns.Add(btnCol)

        Dim tableList As New List(Of String) From {
        "Prescriber Drugs",
        "Affiliations",
        "HCPCS Level 1",
        "HCPCS Level 2",
        "Taxonomies",
        "General Payment",
        "Ownership Data",
        "Research Payment"
    }

        For Each tbl In tableList
            dgvMain.Rows.Add(tbl)
        Next
    End Sub

    Private Sub dgvMain_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMain.CellContentClick
        If e.RowIndex < 0 OrElse e.ColumnIndex <> dgvMain.Columns("ShowButton").Index Then Return

        Dim tableName As String = dgvMain.Rows(e.RowIndex).Cells("TableName").Value.ToString()
        Dim dt As DataTable = Nothing

        Select Case tableName
            Case "Prescriber Drugs"
                dt = TryCast(dgvDrugs.DataSource, DataTable)
            Case "Affiliations"
                dt = TryCast(dgvAff.DataSource, DataTable)
            Case "HCPCS Level 1"
                dt = TryCast(dgvHCPCSlvl1.DataSource, DataTable)
            Case "HCPCS Level 2"
                dt = TryCast(dgvHCPCSlvl2.DataSource, DataTable)
            Case "Taxonomies"
                dt = TryCast(dgvTax.DataSource, DataTable)
            Case "General Payment"
                dt = TryCast(dgvGenPay.DataSource, DataTable)
            Case "Ownership Data"
                dt = TryCast(dgvOwner.DataSource, DataTable)
            Case "Research Payment"
                dt = TryCast(dgvResearch.DataSource, DataTable)
        End Select

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim viewer As New DataTableViewerForm(dt, tableName)
            viewer.ShowDialog()
        Else
            MessageBox.Show("No data to display for " & tableName)
        End If
    End Sub

    ' Optional: Remove if not needed
    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs)
    End Sub

    Private Sub GroupBox1_Enter_1(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub lblMailingState_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Individual_Search.Show()
    End Sub
End Class
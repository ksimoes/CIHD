Imports System.Windows.Forms
Imports Newtonsoft.Json.Linq
Imports System.Net.Http

Public Class ProviderDetailsForm
    Private WithEvents btnPrescriberDrugs As New Button With {.Text = "Prescriber Drugs", .Width = 150}
    Private WithEvents btnProviderProfile As New Button With {.Text = "Provider Profile", .Width = 150}
    Private WithEvents btnHCPCSLevel1 As New Button With {.Text = "HCPCS Level I", .Width = 150}
    Private WithEvents btnHCPCSLevel2 As New Button With {.Text = "HCPCS Level II", .Width = 150}
    Private WithEvents btnAssociatedHospitals As New Button With {.Text = "Associated Hospitals", .Width = 150}
    Private buttonPanel As New FlowLayoutPanel()

    Private currentNpi As String
    Private lblLoading As New Label With {.Text = "Loading...", .Dock = DockStyle.Top, .ForeColor = Color.Red, .Font = New Font("Segoe UI", 12, FontStyle.Bold), .Visible = False}

    ' Default constructor for designer compatibility
    Public Sub New()
        Me.New("")
    End Sub

    ' Main constructor
    Public Sub New(npi As String)
        InitializeComponent()
        AddHandler dgvDetails.CellContentClick, AddressOf dgvDetails_CellContentClick
        Me.Text = $"Provider Details for NPI: {npi}"
        Me.Size = New Size(1000, 600)
        Me.StartPosition = FormStartPosition.CenterParent
        currentNpi = npi

        buttonPanel.Dock = DockStyle.Top
        buttonPanel.Height = 50
        buttonPanel.FlowDirection = FlowDirection.LeftToRight
        buttonPanel.Controls.Add(btnProviderProfile)
        buttonPanel.Controls.Add(btnPrescriberDrugs)

        buttonPanel.Controls.Add(btnHCPCSLevel1)
        buttonPanel.Controls.Add(btnHCPCSLevel2)
        buttonPanel.Controls.Add(btnAssociatedHospitals)
        Me.Controls.Add(buttonPanel)

        Me.Controls.Add(lblLoading)
        lblLoading.BringToFront()


        AddHandler btnProviderProfile.Click, AddressOf btnProviderProfile_Click
        AddHandler btnPrescriberDrugs.Click, AddressOf btnPrescriberDrugs_Click
        AddHandler btnHCPCSLevel1.Click, AddressOf btnHCPCSLevel1_Click
        AddHandler btnHCPCSLevel2.Click, AddressOf btnHCPCSLevel2_Click
        AddHandler btnAssociatedHospitals.Click, AddressOf btnAssociatedHospitals_Click
        btnPrescriberDrugs.PerformClick()
    End Sub

    Private Async Sub btnPrescriberDrugs_Click(sender As Object, e As EventArgs)
        lblHCPCSDescription.Visible = False
        linkMoreInfo.Visible = False
        Await LoadPrescriberDrugsTable()
    End Sub

    Private Async Sub btnProviderProfile_Click(sender As Object, e As EventArgs)
        lblHCPCSDescription.Visible = False
        linkMoreInfo.Visible = False
        Await LoadProviderProfileTable()
    End Sub

    Private Const Level1Desc As String = "HCPCS Code Level 1 - Physician and other qualified healthcare professional services, codes for procedures like surgeries, office visits, and diagnostic tests."
    Private Const Level2Desc As String = "HCPCS Code Level 2 - Non-physician services, supplies, and durable medical equipment,codes for procedures like surgeries, office visits, and diagnostic tests."

    Private Async Sub btnHCPCSLevel1_Click(sender As Object, e As EventArgs)
        lblHCPCSDescription.Text = Level1Desc
        lblHCPCSDescription.Visible = True
        linkMoreInfo.Visible = True
        Await LoadHCPCSTable(level:=1)
    End Sub

    Private Async Sub btnHCPCSLevel2_Click(sender As Object, e As EventArgs)
        lblHCPCSDescription.Text = Level2Desc
        lblHCPCSDescription.Visible = True
        linkMoreInfo.Visible = True
        Await LoadHCPCSTable(level:=2)
    End Sub

    Private Sub ClearGrid()
        dgvDetails.DataSource = Nothing
        dgvDetails.Columns.Clear()
        dgvDetails.Rows.Clear()
        dgvDetails.Refresh()
    End Sub

    Private Sub ApplyCustomColors()
        With dgvDetails
            .AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow   ' Alternating rows
            .DefaultCellStyle.BackColor = Color.White                        ' Main rows
            .ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateBlue   ' Header background
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White           ' Header text
            .DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue        ' Selected row
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .EnableHeadersVisualStyles = False
        End With
    End Sub
    Private Sub linkMoreInfo_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles linkMoreInfo.LinkClicked
        Try
            Dim psi As New ProcessStartInfo With {
            .FileName = "https://www.cms.gov/medicare/coding-billing/healthcare-common-procedure-system/alpha-numeric",
            .UseShellExecute = True
        }
            Process.Start(psi)
        Catch ex As Exception
            MessageBox.Show("Unable to open link: " & ex.Message)
        End Try
    End Sub

    Private Async Function LoadPrescriberDrugsTable() As Task
        lblLoading.Visible = True
        Try
            ClearGrid()
            Dim dt As New DataTable()
            Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/9552739e-3d05-4c1b-8eff-ecabf391e2e5/data?filter[Prscrbr_NPI]={Uri.EscapeDataString(currentNpi)}&size=100"
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
                            If npiVal = currentNpi Then
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

            dgvDetails.DataSource = dt
            SetFriendlyColumnHeaders()
            ApplyCustomColors()
            dgvDetails.Refresh()
        Catch ex As Exception
            MessageBox.Show("Error loading Prescriber Drugs: " & ex.Message)
        Finally
            lblLoading.Visible = False
        End Try
    End Function

    Private Async Function LoadProviderProfileTable() As Task
        lblLoading.Visible = True
        Try
            ClearGrid()
            Dim apiUrl As String = $"https://npiregistry.cms.hhs.gov/api/?number={Uri.EscapeDataString(currentNpi)}&version=2.1"
            Dim dt As New DataTable()
            Using client As New HttpClient()
                Dim response = Await client.GetAsync(apiUrl)
                If response.IsSuccessStatusCode Then
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim obj = JObject.Parse(json)
                    If obj("results") IsNot Nothing AndAlso obj("results").HasValues Then
                        Dim result = obj("results")(0)
                        ' Only add the most useful columns
                        dt.Columns.Add("NPI")
                        dt.Columns.Add("Enumeration Type")
                        dt.Columns.Add("First Name")
                        dt.Columns.Add("Last Name")
                        dt.Columns.Add("Credential")
                        dt.Columns.Add("Gender")
                        dt.Columns.Add("Enumeration Date")
                        dt.Columns.Add("Last Updated")
                        dt.Columns.Add("Status")
                        dt.Columns.Add("Primary Taxonomy")
                        dt.Columns.Add("Primary Taxonomy Desc")
                        dt.Columns.Add("Primary Taxonomy State")
                        dt.Columns.Add("Primary Taxonomy License")

                        Dim row = dt.NewRow()
                        row("NPI") = result("number")?.ToString()
                        row("Enumeration Type") = result("enumeration_type")?.ToString()
                        row("First Name") = result("basic")?("first_name")?.ToString()
                        row("Last Name") = result("basic")?("last_name")?.ToString()
                        row("Credential") = result("basic")?("credential")?.ToString()
                        row("Gender") = result("basic")?("gender")?.ToString()
                        row("Enumeration Date") = result("basic")?("enumeration_date")?.ToString()
                        row("Last Updated") = result("basic")?("last_updated")?.ToString()
                        row("Status") = result("basic")?("status")?.ToString()

                        ' Taxonomy (primary)
                        Dim primaryTaxonomy = result("taxonomies")?.FirstOrDefault(Function(t) t("primary")?.ToString().ToLower() = "true")
                        If primaryTaxonomy Is Nothing AndAlso result("taxonomies") IsNot Nothing AndAlso result("taxonomies").HasValues Then
                            primaryTaxonomy = result("taxonomies")(0)
                        End If
                        row("Primary Taxonomy") = primaryTaxonomy?("code")?.ToString()
                        row("Primary Taxonomy Desc") = primaryTaxonomy?("desc")?.ToString()
                        row("Primary Taxonomy State") = primaryTaxonomy?("state")?.ToString()
                        row("Primary Taxonomy License") = primaryTaxonomy?("license")?.ToString()

                        dt.Rows.Add(row)
                    Else
                        dt.Columns.Add("NPI")
                        dt.Columns.Add("Status")
                        Dim row = dt.NewRow()
                        row("NPI") = currentNpi
                        row("Status") = "Not found"
                        dt.Rows.Add(row)
                    End If
                End If
            End Using
            dgvDetails.DataSource = dt
            ApplyCustomColors()
            dgvDetails.Refresh()
        Catch ex As Exception
            MessageBox.Show("Error loading Provider Profile: " & ex.Message)
        Finally
            lblLoading.Visible = False
        End Try
    End Function

    Private Async Function LoadHCPCSTable(level As Integer) As Task
        lblLoading.Visible = True
        Try
            ClearGrid()
            Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/92396110-2aed-4d63-a6a2-5d6207d46a29/data?filter[Rndrng_NPI]={Uri.EscapeDataString(currentNpi)}&size=1000"
            Dim dt As New DataTable()
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
                            Dim hcpcsCode As String = obj("HCPCS_Cd")?.ToString()
                            If Not String.IsNullOrWhiteSpace(hcpcsCode) Then
                                If level = 1 AndAlso hcpcsCode.Length = 5 AndAlso hcpcsCode.All(AddressOf Char.IsDigit) Then
                                    Dim row = dt.NewRow()
                                    For Each col In dt.Columns
                                        row(col.ToString()) = obj(col.ToString())
                                    Next
                                    dt.Rows.Add(row)
                                ElseIf level = 2 AndAlso hcpcsCode.Length = 5 AndAlso Char.IsLetter(hcpcsCode(0)) AndAlso hcpcsCode.Substring(1).All(AddressOf Char.IsDigit) Then
                                    Dim row = dt.NewRow()
                                    For Each col In dt.Columns
                                        row(col.ToString()) = obj(col.ToString())
                                    Next
                                    dt.Rows.Add(row)
                                End If
                            End If
                        Next
                    Else
                        dt.Columns.Add("Rndrng_NPI")
                        dt.Columns.Add("HCPCS_Cd")
                        dt.Columns.Add("HCPCS_Desc")
                    End If
                End If
            End Using
            dgvDetails.DataSource = dt
            SetHCPCSColumnHeaders()
            ApplyCustomColors()
            dgvDetails.Refresh()
        Catch ex As Exception
            MessageBox.Show("Error loading HCPCS table: " & ex.Message)
        Finally
            lblLoading.Visible = False
        End Try
    End Function
    Private Async Sub btnAssociatedHospitals_Click(sender As Object, e As EventArgs)
        lblHCPCSDescription.Visible = False
        linkMoreInfo.Visible = False
        lblLoading.Visible = True
        Try
            ClearGrid()
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
                        New JProperty("value", currentNpi),
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

                            ' Lookup City and State by CCN
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
                                    ' Ignore lookup errors, leave city/state blank
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
                dgvDetails.DataSource = dt

                ' Make CCN column a link
                If dgvDetails.Columns.Contains("Facility Affiliation Certification Number") Then
                    Dim idx = dgvDetails.Columns("Facility Affiliation Certification Number").Index
                    Dim linkCol As New DataGridViewLinkColumn()
                    linkCol.Name = "Facility Affiliation Certification Number"
                    linkCol.HeaderText = "Facility Affiliation Certification Number"
                    linkCol.DataPropertyName = "Facility Affiliation Certification Number"
                    linkCol.LinkColor = Color.Blue
                    linkCol.ActiveLinkColor = Color.Red
                    linkCol.VisitedLinkColor = Color.Purple
                    linkCol.TrackVisitedState = False
                    dgvDetails.Columns.RemoveAt(idx)
                    dgvDetails.Columns.Insert(idx, linkCol)
                End If

                dgvDetails.Refresh()
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading associated hospitals: " & ex.Message)
        Finally
            lblLoading.Visible = False
        End Try
    End Sub

    ' Handle CCN link click
    Private Async Sub dgvDetails_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso dgvDetails.Columns(e.ColumnIndex).Name = "Facility Affiliation Certification Number" Then
            Dim ccn As String = dgvDetails.Rows(e.RowIndex).Cells("Facility Affiliation Certification Number").Value?.ToString()
            If Not String.IsNullOrWhiteSpace(ccn) Then
                ' Fetch hospital data for this CCN from the API
                Dim apiUrl As String = $"https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?filter[Provider CCN]={Uri.EscapeDataString(ccn)}&size=1"
                Using client As New HttpClient()
                    Dim response = Await client.GetAsync(apiUrl)
                    If response.IsSuccessStatusCode Then
                        Dim json = Await response.Content.ReadAsStringAsync()
                        Dim arr = JArray.Parse(json)
                        If arr.Count > 0 Then
                            Dim row = arr(0)
                            Dim ctx As New HospitalContext()
                            ctx.CMSNum = row("Provider CCN")?.ToString()
                            ctx.Name = row("Hospital Name")?.ToString()
                            ctx.Address = row("Address")?.ToString()
                            ctx.City = row("City")?.ToString()
                            ctx.State = row("State Code")?.ToString()
                            ctx.Zip = row("ZIP Code")?.ToString()
                            ctx.County = row("County Name")?.ToString()
                            ctx.Phone = row("Phone Number")?.ToString()
                            ctx.FacilityType = row("Type of Facility")?.ToString()
                            ctx.RuralOUrban = row("Rural Urban Designation")?.ToString()
                            ctx.NumOfBeds = If(Integer.TryParse(row("Number of Beds")?.ToString(), 0), Integer.Parse(row("Number of Beds")?.ToString()), 0)
                            ctx.LastDataRow = Nothing

                            ' Open a new Profile window for this hospital context
                            Dim profileForm As New Profile(ctx)
                            profileForm.StartPosition = FormStartPosition.Manual
                            profileForm.Location = New Point(Me.Location.X + 30, Me.Location.Y + 30)
                            profileForm.Show()
                        Else
                            MessageBox.Show("No data found for this CCN.")
                        End If
                    Else
                        MessageBox.Show("API error: " & response.StatusCode.ToString())
                    End If
                End Using
            End If
        End If
    End Sub


    Public Sub SetFriendlyColumnHeaders()
        Dim headerMap As New Dictionary(Of String, String) From {
            {"Prscrbr_NPI", "NPI"},
            {"Prscrbr_Last_Org_Name", "Last Name"},
            {"Prscrbr_First_Name", "First Name"},
            {"Prscrbr_Cred", "Credentials"},
            {"Prscrbr_State_Abrvtn", "State"},
            {"Prscrbr_City", "City"},
            {"Prscrbr_Zip", "ZIP"},
            {"Prscrbr_Type", "Provider Type"},
            {"Prscrbr_Type_Src", "Prescriber Type"},
            {"Brnd_Name", "Brand Name"},
            {"Gnrc_Name", "Generic Name"},
            {"Tot_Clms", "Total Claims"},
            {"Tot_30day_Fills", "Total 30-Day Fills"},
            {"Tot_Day_Suply", "Total Day Supply"},
            {"Tot_Drug_Cst", "Total Drug Cost"}
        }
        For Each col As DataGridViewColumn In dgvDetails.Columns
            If headerMap.ContainsKey(col.Name) Then
                col.HeaderText = headerMap(col.Name)
            End If
        Next
    End Sub

    Public Sub SetProviderProfileColumnHeaders()
        Dim headerMap As New Dictionary(Of String, String) From {
            {"number", "NPI"},
            {"enumeration_type", "Enumeration Type"},
            {"basic_first_name", "First Name"},
            {"basic_last_name", "Last Name"},
            {"basic_middle_name", "Middle Name"},
            {"basic_credential", "Credential"},
            {"basic_gender", "Gender"},
            {"basic_enumeration_date", "Enumeration Date"},
            {"basic_last_updated", "Last Updated"},
            {"basic_status", "Status"}
        }
        For Each col As DataGridViewColumn In dgvDetails.Columns
            If headerMap.ContainsKey(col.Name) Then
                col.HeaderText = headerMap(col.Name)
            End If
        Next
    End Sub

    Public Sub SetHCPCSColumnHeaders()
        Dim headerMap As New Dictionary(Of String, String) From {
            {"Rndrng_NPI", "NPI"},
            {"Rndrng_Prov_Nm", "Provider Name"},
            {"Rndrng_Prov_Type", "Provider Type"},
            {"Rndrng_Prov_Cred", "Provider Credentials"},
            {"Rndrng_Prov_Gndr", "Provider Gender"},
            {"Rndrng_Prov_Addr", "Provider Address"},
            {"Rndrng_Prov_City", "Provider City"},
            {"Rndrng_Prov_State", "Provider State"},
            {"Rndrng_Prov_Zip", "Provider ZIP"},
            {"HCPCS_Cd", "HCPCS Code"},
            {"HCPCS_Desc", "HCPCS Description"},
            {"HCPCS_Actv_Ind", "Active Indicator"},
            {"HCPCS_Eff_Yr", "Effective Year"}
        }
        For Each col As DataGridViewColumn In dgvDetails.Columns
            If headerMap.ContainsKey(col.Name) Then
                col.HeaderText = headerMap(col.Name)
            End If
        Next
    End Sub

    Private Sub ProviderDetailsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
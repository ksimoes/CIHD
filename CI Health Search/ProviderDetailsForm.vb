Imports System.Windows.Forms
Imports Newtonsoft.Json.Linq
Imports System.Net.Http

Public Class ProviderDetailsForm
    Private WithEvents btnPrescriberDrugs As New Button With {.Text = "Prescriber Drugs", .Width = 150}
    Private WithEvents btnProviderProfile As New Button With {.Text = "Provider Profile", .Width = 150}
    Private WithEvents btnHCPCSLevel1 As New Button With {.Text = "HCPCS Level I", .Width = 150}
    Private WithEvents btnHCPCSLevel2 As New Button With {.Text = "HCPCS Level II", .Width = 150}
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
        Me.Text = $"Provider Details for NPI: {npi}"
        Me.Size = New Size(1000, 600)
        Me.StartPosition = FormStartPosition.CenterParent
        currentNpi = npi

        buttonPanel.Dock = DockStyle.Top
        buttonPanel.Height = 50
        buttonPanel.FlowDirection = FlowDirection.LeftToRight
        buttonPanel.Controls.Add(btnPrescriberDrugs)
        buttonPanel.Controls.Add(btnProviderProfile)
        buttonPanel.Controls.Add(btnHCPCSLevel1)
        buttonPanel.Controls.Add(btnHCPCSLevel2)
        Me.Controls.Add(buttonPanel)

        Me.Controls.Add(lblLoading)
        lblLoading.BringToFront()

        AddHandler btnPrescriberDrugs.Click, AddressOf btnPrescriberDrugs_Click
        AddHandler btnProviderProfile.Click, AddressOf btnProviderProfile_Click
        AddHandler btnHCPCSLevel1.Click, AddressOf btnHCPCSLevel1_Click
        AddHandler btnHCPCSLevel2.Click, AddressOf btnHCPCSLevel2_Click

        btnPrescriberDrugs.PerformClick()
    End Sub

    Private Async Sub btnPrescriberDrugs_Click(sender As Object, e As EventArgs)
        Await LoadPrescriberDrugsTable()
    End Sub

    Private Async Sub btnProviderProfile_Click(sender As Object, e As EventArgs)
        Await LoadProviderProfileTable()
    End Sub

    Private Async Sub btnHCPCSLevel1_Click(sender As Object, e As EventArgs)
        Await LoadHCPCSTable(level:=1)
    End Sub

    Private Async Sub btnHCPCSLevel2_Click(sender As Object, e As EventArgs)
        Await LoadHCPCSTable(level:=2)
    End Sub

    Private Sub ClearGrid()
        dgvDetails.DataSource = Nothing
        dgvDetails.Columns.Clear()
        dgvDetails.Rows.Clear()
        dgvDetails.Refresh()
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
                        Dim resultObj As JObject = CType(result, JObject)
                        For Each prop In resultObj.Properties()
                            If dt.Columns.Contains(prop.Name) = False AndAlso prop.Name <> "basic" Then
                                dt.Columns.Add(prop.Name)
                            End If
                        Next
                        If resultObj("basic") IsNot Nothing Then
                            Dim basicObj As JObject = CType(resultObj("basic"), JObject)
                            For Each prop In basicObj.Properties()
                                If dt.Columns.Contains("basic_" & prop.Name) = False Then
                                    dt.Columns.Add("basic_" & prop.Name)
                                End If
                            Next
                        End If
                        Dim row = dt.NewRow()
                        For Each col As DataColumn In dt.Columns
                            If col.ColumnName.StartsWith("basic_") Then
                                Dim basicName = col.ColumnName.Substring(6)
                                row(col.ColumnName) = resultObj("basic")?(basicName)?.ToString()
                            Else
                                row(col.ColumnName) = resultObj(col.ColumnName)?.ToString()
                            End If
                        Next
                        dt.Rows.Add(row)
                    Else
                        dt.Columns.Add("number")
                        dt.Columns.Add("enumeration_type")
                    End If
                End If
            End Using
            dgvDetails.DataSource = dt
            SetProviderProfileColumnHeaders()
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
            dgvDetails.Refresh()
        Catch ex As Exception
            MessageBox.Show("Error loading HCPCS table: " & ex.Message)
        Finally
            lblLoading.Visible = False
        End Try
    End Function

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
End Class
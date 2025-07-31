Imports System.Runtime.CompilerServices

Public Class Results

    Public Property SelectedState As String
    Private resultsTable As DataTable
    Public strCMSnum As String
    Public Shared SelectedHospital As New HospitalContext()

    ' Call this from Search form to set and display results
    Public Sub SetResults(dt As DataTable)
        resultsTable = dt

        ' Setup DataGridView columns only once
        If dgvResults.Columns.Count = 0 Then
            dgvResults.Columns.Add(New DataGridViewCheckBoxColumn() With {.Name = "Select", .HeaderText = "", .Width = 30})
            dgvResults.Columns.Add("HospitalName", "Hospital Name")
            dgvResults.Columns.Add("City", "City")
            dgvResults.Columns.Add("State", "State")
            dgvResults.Columns.Add("Zip", "Zip")
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvResults.MultiSelect = False
        End If
        dgvResults.Rows.Clear()

        ' Determine which column to use for display
        Dim displayCol As String = ""
        If dt.Columns.Contains("FAC_NAME") Then
            displayCol = "FAC_NAME"
        ElseIf dt.Columns.Contains("ORGANIZATION NAME") Then
            displayCol = "ORGANIZATION NAME"
        ElseIf dt.Columns.Contains("facility_name") Then
            displayCol = "facility_name"
        ElseIf dt.Columns.Contains("provider_name") Then
            displayCol = "provider_name"
        ElseIf dt.Columns.Contains("Rndrng_Prvdr_Org_Name") Then
            displayCol = "Rndrng_Prvdr_Org_Name"
        ElseIf dt.Columns.Contains("Hospital Name") Then
            displayCol = "Hospital Name"
        ElseIf dt.Columns.Contains("Facility Name") Then
            displayCol = "Facility Name"
        ElseIf dt.Columns.Count > 0 Then
            displayCol = dt.Columns(0).ColumnName ' fallback
        End If

        For Each row As DataRow In dt.Rows
            Dim name As String = row(displayCol).ToString()
            Dim city As String = If(dt.Columns.Contains("City"), row("City").ToString(), "")
            Dim state As String = ""
            If dt.Columns.Contains("State") Then
                state = row("State").ToString()
            ElseIf dt.Columns.Contains("STATE") Then
                state = row("STATE").ToString()
            ElseIf dt.Columns.Contains("State Code") Then
                state = row("State Code").ToString()
            End If
            Dim zip As String = If(dt.Columns.Contains("Zip Code"), row("Zip Code").ToString(),
                   If(dt.Columns.Contains("ZIP"), row("ZIP").ToString(), ""))

            dgvResults.Rows.Add(False, name, city, state, zip)
        Next
    End Sub

    ' Ensure only one checkbox is checked at a time (single selection)
    Private Sub dgvResults_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvResults.CellValueChanged
        If e.ColumnIndex = dgvResults.Columns("Select").Index AndAlso CBool(dgvResults.Rows(e.RowIndex).Cells("Select").Value) Then
            For i As Integer = 0 To dgvResults.Rows.Count - 1
                If i <> e.RowIndex Then
                    dgvResults.Rows(i).Cells("Select").Value = False
                End If
            Next
        End If
    End Sub

    ' Profile button click
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim selectedRow As DataGridViewRow = dgvResults.Rows.Cast(Of DataGridViewRow)().
            FirstOrDefault(Function(r) CBool(r.Cells("Select").Value))
        If selectedRow Is Nothing Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If

        Dim name As String = selectedRow.Cells("HospitalName").Value.ToString()
        Dim city As String = selectedRow.Cells("City").Value.ToString()
        Dim state As String = selectedRow.Cells("State").Value.ToString()
        Dim zip As String = selectedRow.Cells("Zip").Value.ToString()

        Dim displayCol As String = ""
        If resultsTable.Columns.Contains("FAC_NAME") Then
            displayCol = "FAC_NAME"
        ElseIf resultsTable.Columns.Contains("ORGANIZATION NAME") Then
            displayCol = "ORGANIZATION NAME"
        ElseIf resultsTable.Columns.Contains("facility_name") Then
            displayCol = "facility_name"
        ElseIf resultsTable.Columns.Contains("provider_name") Then
            displayCol = "provider_name"
        ElseIf resultsTable.Columns.Contains("Rndrng_Prvdr_Org_Name") Then
            displayCol = "Rndrng_Prvdr_Org_Name"
        ElseIf resultsTable.Columns.Contains("Hospital Name") Then
            displayCol = "Hospital Name"
        ElseIf resultsTable.Columns.Contains("Facility Name") Then
            displayCol = "Facility Name"
        ElseIf resultsTable.Columns.Count > 0 Then
            displayCol = resultsTable.Columns(0).ColumnName ' fallback
        End If

        Dim selectedDataRow As DataRow = resultsTable.AsEnumerable().FirstOrDefault(
            Function(r) r.Field(Of String)(displayCol) = name AndAlso
                        (Not resultsTable.Columns.Contains("City") OrElse r.Field(Of String)("City") = city) AndAlso
                        (Not resultsTable.Columns.Contains("State") OrElse r.Field(Of String)("State") = state OrElse
                         resultsTable.Columns.Contains("STATE") AndAlso r.Field(Of String)("STATE") = state OrElse
                         resultsTable.Columns.Contains("State Code") AndAlso r.Field(Of String)("State Code") = state) AndAlso
                        (Not resultsTable.Columns.Contains("Zip Code") OrElse r.Field(Of String)("Zip Code") = zip OrElse
                         resultsTable.Columns.Contains("ZIP") AndAlso r.Field(Of String)("ZIP") = zip)
        )

        If selectedDataRow IsNot Nothing Then
            Dim hosp As New HospitalContext()
            ' Core Identifiers
            hosp.HospitalId = If(resultsTable.Columns.Contains("LicenseNum"), SafeInt(selectedDataRow("LicenseNum")), 0)
            hosp.CMSNum = SafeStr(selectedDataRow, "Provider CCN", "CMSNum", "PRVDR_NUM", "CCN", "ccn")
            hosp.NPI = SafeStr(selectedDataRow, "NPI", "npi")
            hosp.Name = SafeStr(selectedDataRow, "FAC_NAME", "PRVDR_NM", "PRVDR_NAME", "ORGANIZATION NAME", "organization_name", "Facility Name", "Hospital Name", "provider_name")
            ' Location & Contact
            hosp.Address = SafeStr(selectedDataRow, "ADDR_LN_1_TXT", "ADDRESS LINE 1", "address_line_1", "Address", "Facility Address", "Street Address", "STREET", "STREET1", "STREET_ADDRESS")
            hosp.City = SafeStr(selectedDataRow, "CITY_NM", "City")
            hosp.Zip = SafeStr(selectedDataRow, "ZIP_CD", "Zip Code", "ZIP")
            hosp.County = SafeStr(selectedDataRow, "County Name", "County")
            hosp.Phone = SafeStr(selectedDataRow, "Phone", "Telephone Number")
            hosp.Website = SafeStr(selectedDataRow, "Website")
            hosp.State = SafeStr(selectedDataRow, state)
            ' Classification
            hosp.CBSAnum = SafeStr(selectedDataRow, "CBSA", "CBSA Code")
            hosp.FacilityType = SafeStr(selectedDataRow, "Facility Type")
            hosp.RuralOUrban = SafeStr(selectedDataRow, "Rural Versus Urban")
            ' Capacity & Staffing
            hosp.NumOfBeds = SafeInt(selectedDataRow, "Number of Beds", "General Med/Surg Beds")
            hosp.NumOfEmployees = SafeInt(selectedDataRow, "Total Employees")
            hosp.TotalDays = SafeInt(selectedDataRow, "Total Days", "Inpatient Days")
            hosp.TotalDischarges = SafeInt(selectedDataRow, "Total Discharges")
            ' Financials
            hosp.TotalPatientRev = SafeDec(selectedDataRow, "Total Patient Revenue")
            hosp.NetPatientRev = SafeDec(selectedDataRow, "Net Patient Revenue")
            hosp.CharityCost = SafeDec(selectedDataRow, "Cost of Charity Care")
            hosp.UncompensatedCost = SafeDec(selectedDataRow, "Cost of Uncompensated Care")
            ' Additional/Expandable fields
            hosp.TotalCurrentAssets = SafeDec(selectedDataRow, "Total Current Assets")
            hosp.TotalAssets = SafeDec(selectedDataRow, "Total Assets")
            hosp.NetIncome = SafeDec(selectedDataRow, "Net Income")
            hosp.TotalOperatingRevenue = SafeDec(selectedDataRow, "Net Patient Revenue")
            hosp.TotalOperatingExpense = SafeDec(selectedDataRow, "Less Total Operating Expense")
            hosp.TotalLiabilities = SafeDec(selectedDataRow, "Total Liabilities")
            hosp.TotalCurrentLiabilities = SafeDec(selectedDataRow, "Total Current Liabilities")
            hosp.TotalLongTermLiabilities = SafeDec(selectedDataRow, "Total Long Term Liabilities")
            hosp.DepreciationCost = SafeDec(selectedDataRow, "Depreciation Cost")
            hosp.LeaseCost = SafeDec(selectedDataRow, "Leasehold Improvements")
            hosp.Inventory = SafeDec(selectedDataRow, "Inventory")
            hosp.NotesReceivable = SafeDec(selectedDataRow, "Notes Receivable")
            hosp.MarketSecurities = SafeDec(selectedDataRow, "Temporary Investments")
            hosp.Investments = SafeDec(selectedDataRow, "Investments")
            Results.SelectedHospital = hosp
            hosp.LastDataRow = selectedDataRow
        Else
            MessageBox.Show("Could not find the selected hospital in the results.")
            Return
        End If

        Profile.ShowProfile(SelectedHospital)
        Hide()
        Profile.Show()
    End Sub

    ' Repeat the above selection logic for other navigation buttons as needed
    ' Example for Button3_Click (Financial):
    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim selectedRow As DataGridViewRow = dgvResults.Rows.Cast(Of DataGridViewRow)().
            FirstOrDefault(Function(r) CBool(r.Cells("Select").Value))
        If selectedRow Is Nothing Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If

        Dim name As String = selectedRow.Cells("HospitalName").Value.ToString()
        Dim city As String = selectedRow.Cells("City").Value.ToString()
        Dim state As String = selectedRow.Cells("State").Value.ToString()
        Dim zip As String = selectedRow.Cells("Zip").Value.ToString()

        Dim displayCol As String = ""
        If resultsTable.Columns.Contains("FAC_NAME") Then
            displayCol = "FAC_NAME"
        ElseIf resultsTable.Columns.Contains("ORGANIZATION NAME") Then
            displayCol = "ORGANIZATION NAME"
        ElseIf resultsTable.Columns.Contains("facility_name") Then
            displayCol = "facility_name"
        ElseIf resultsTable.Columns.Contains("provider_name") Then
            displayCol = "provider_name"
        ElseIf resultsTable.Columns.Contains("Rndrng_Prvdr_Org_Name") Then
            displayCol = "Rndrng_Prvdr_Org_Name"
        ElseIf resultsTable.Columns.Contains("Hospital Name") Then
            displayCol = "Hospital Name"
        ElseIf resultsTable.Columns.Contains("Facility Name") Then
            displayCol = "Facility Name"
        ElseIf resultsTable.Columns.Count > 0 Then
            displayCol = resultsTable.Columns(0).ColumnName ' fallback
        End If

        Dim selectedDataRow As DataRow = resultsTable.AsEnumerable().FirstOrDefault(
            Function(r) r.Field(Of String)(displayCol) = name AndAlso
                        (Not resultsTable.Columns.Contains("City") OrElse r.Field(Of String)("City") = city) AndAlso
                        (Not resultsTable.Columns.Contains("State") OrElse r.Field(Of String)("State") = state OrElse
                         resultsTable.Columns.Contains("STATE") AndAlso r.Field(Of String)("STATE") = state OrElse
                         resultsTable.Columns.Contains("State Code") AndAlso r.Field(Of String)("State Code") = state) AndAlso
                        (Not resultsTable.Columns.Contains("Zip Code") OrElse r.Field(Of String)("Zip Code") = zip OrElse
                         resultsTable.Columns.Contains("ZIP") AndAlso r.Field(Of String)("ZIP") = zip)
        )

        If selectedDataRow IsNot Nothing Then
            Dim hosp As New HospitalContext()
            ' (populate hosp as above)
            hosp.HospitalId = If(resultsTable.Columns.Contains("LicenseNum"), SafeInt(selectedDataRow("LicenseNum")), 0)
            hosp.CMSNum = SafeStr(selectedDataRow, "Provider CCN", "CMSNum", "PRVDR_NUM", "CCN", "ccn")
            hosp.NPI = SafeStr(selectedDataRow, "NPI", "npi")
            hosp.Name = SafeStr(selectedDataRow, "FAC_NAME", "PRVDR_NM", "PRVDR_NAME", "ORGANIZATION NAME", "organization_name", "Facility Name", "Hospital Name", "provider_name")
            hosp.Address = SafeStr(selectedDataRow, "ADDR_LN_1_TXT", "ADDRESS LINE 1", "address_line_1", "Address", "Facility Address", "Street Address", "STREET", "STREET1", "STREET_ADDRESS")
            hosp.City = SafeStr(selectedDataRow, "CITY_NM", "City")
            hosp.Zip = SafeStr(selectedDataRow, "ZIP_CD", "Zip Code", "ZIP")
            hosp.County = SafeStr(selectedDataRow, "County Name", "County")
            hosp.Phone = SafeStr(selectedDataRow, "Phone", "Telephone Number")
            hosp.Website = SafeStr(selectedDataRow, "Website")
            hosp.CBSAnum = SafeStr(selectedDataRow, "CBSA", "CBSA Code")
            hosp.FacilityType = SafeStr(selectedDataRow, "Facility Type")
            hosp.RuralOUrban = SafeStr(selectedDataRow, "Rural Versus Urban")
            hosp.NumOfBeds = SafeInt(selectedDataRow, "Number of Beds", "General Med/Surg Beds")
            hosp.NumOfEmployees = SafeInt(selectedDataRow, "Total Employees")
            hosp.TotalDays = SafeInt(selectedDataRow, "Total Days", "Inpatient Days")
            hosp.TotalDischarges = SafeInt(selectedDataRow, "Total Discharges")
            hosp.TotalPatientRev = SafeDec(selectedDataRow, "Total Patient Revenue")
            hosp.NetPatientRev = SafeDec(selectedDataRow, "Net Patient Revenue")
            hosp.CharityCost = SafeDec(selectedDataRow, "Cost of Charity Care")
            hosp.UncompensatedCost = SafeDec(selectedDataRow, "Cost of Uncompensated Care")
            hosp.TotalCurrentAssets = SafeDec(selectedDataRow, "Total Current Assets")
            hosp.TotalAssets = SafeDec(selectedDataRow, "Total Assets")
            hosp.NetIncome = SafeDec(selectedDataRow, "Net Income")
            hosp.TotalOperatingRevenue = SafeDec(selectedDataRow, "Net Patient Revenue")
            hosp.TotalOperatingExpense = SafeDec(selectedDataRow, "Less Total Operating Expense")
            hosp.TotalLiabilities = SafeDec(selectedDataRow, "Total Liabilities")
            hosp.TotalCurrentLiabilities = SafeDec(selectedDataRow, "Total Current Liabilities")
            hosp.TotalLongTermLiabilities = SafeDec(selectedDataRow, "Total Long Term Liabilities")
            hosp.DepreciationCost = SafeDec(selectedDataRow, "Depreciation Cost")
            hosp.LeaseCost = SafeDec(selectedDataRow, "Leasehold Improvements")
            hosp.Inventory = SafeDec(selectedDataRow, "Inventory")
            hosp.NotesReceivable = SafeDec(selectedDataRow, "Notes Receivable")
            hosp.MarketSecurities = SafeDec(selectedDataRow, "Temporary Investments")
            hosp.Investments = SafeDec(selectedDataRow, "Investments")
            Results.SelectedHospital = hosp
            hosp.LastDataRow = selectedDataRow
        Else
            MessageBox.Show("Could not find the selected hospital in the results.")
            Return
        End If

        Hide()
        Financial.Show()
        Await Financial.ShowFinancialData(Results.SelectedHospital.HospitalId, Results.SelectedHospital.State)
    End Sub

    ' Repeat similar logic for other navigation buttons as needed...

    ' Helper functions
    Private Function SafeStr(row As DataRow, ParamArray names() As String) As String
        For Each n In names
            If row.Table.Columns.Contains(n) AndAlso Not IsDBNull(row(n)) Then
                Return row(n).ToString().Trim()
            End If
        Next
        Return ""
    End Function

    Private Function SafeInt(row As DataRow, ParamArray names() As String) As Integer
        For Each n In names
            If row.Table.Columns.Contains(n) AndAlso Not IsDBNull(row(n)) Then
                Dim val As Integer
                If Integer.TryParse(row(n).ToString(), val) Then Return val
            End If
        Next
        Return 0
    End Function

    Private Function SafeDec(row As DataRow, ParamArray names() As String) As Decimal
        For Each n In names
            If row.Table.Columns.Contains(n) AndAlso Not IsDBNull(row(n)) Then
                Dim val As Decimal
                If Decimal.TryParse(row(n).ToString().Replace("$", "").Replace(",", ""), val) Then Return val
            End If
        Next
        Return 0D
    End Function

    Private Sub Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvResults.AllowUserToAddRows = False
        dgvResults.AllowUserToDeleteRows = False
        dgvResults.ReadOnly = False
        dgvResults.Columns("Select").ReadOnly = False
        For Each col As DataGridViewColumn In dgvResults.Columns
            If col.Name <> "Select" Then col.ReadOnly = True
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Me.Hide()
        Outpatient.Show()
    End Sub

    ' Add similar selection logic to Button6_Click (Inpatient) if needed

End Class
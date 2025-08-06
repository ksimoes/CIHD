Imports System.Runtime.CompilerServices

Public Class Results

    Public Property SelectedState As String
    Public Shared resultsTable As DataTable
    Public strCMSnum As String
    Public Shared SelectedHospital As New HospitalContext()

    ' Call this from Search form to set and display results
    Public Sub SetResults(dt As DataTable, Optional filterSummary As String = "")
        resultsTable = dt

        ' Setup DataGridView columns only once
        If dgvResults.Columns.Count = 0 Then
            dgvResults.Columns.Add(New DataGridViewCheckBoxColumn() With {.Name = "Select", .HeaderText = "", .Width = 30})
            dgvResults.Columns.Add("HospitalName", "Hospital Name")
            dgvResults.Columns.Add("City", "City")
            dgvResults.Columns.Add("State", "State")
            dgvResults.Columns.Add("Zip", "Zip")
            dgvResults.Columns.Add("BedCount", "Bed Count")
            dgvResults.Columns.Add("CMSCCN", "CMS/CCN")
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

        ' Find bed count and CMS/CCN columns
        Dim bedCountCol As String = ""
        If dt.Columns.Contains("Number of Beds") Then
            bedCountCol = "Number of Beds"
        ElseIf dt.Columns.Contains("General Med/Surg Beds") Then
            bedCountCol = "General Med/Surg Beds"
        End If

        Dim cmsCol As String = ""
        If dt.Columns.Contains("Provider CCN") Then
            cmsCol = "Provider CCN"
        ElseIf dt.Columns.Contains("CMSNum") Then
            cmsCol = "CMSNum"
        ElseIf dt.Columns.Contains("PRVDR_NUM") Then
            cmsCol = "PRVDR_NUM"
        ElseIf dt.Columns.Contains("CCN") Then
            cmsCol = "CCN"
        ElseIf dt.Columns.Contains("ccn") Then
            cmsCol = "ccn"
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

            Dim bedCount As String = If(bedCountCol <> "" AndAlso Not IsDBNull(row(bedCountCol)), row(bedCountCol).ToString(), "")
            Dim cmsccn As String = If(cmsCol <> "" AndAlso Not IsDBNull(row(cmsCol)), row(cmsCol).ToString(), "")

            dgvResults.Rows.Add(False, name, city, state, zip, bedCount, cmsccn)
        Next

        ' Show result count
        lblMatches.Text = $"{dt.Rows.Count} result(s) found"

        ' Show filter summary if provided
        lblFilters.Text = If(String.IsNullOrWhiteSpace(filterSummary), "", $"Filters: {filterSummary}")
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



    Private Function GetSelectedHospitalContext() As HospitalContext
        Dim selectedRow As DataGridViewRow = dgvResults.Rows.Cast(Of DataGridViewRow)().
        FirstOrDefault(Function(r) CBool(r.Cells("Select").Value))
        If selectedRow Is Nothing Then
            MessageBox.Show("Select a hospital first.")
            Return Nothing
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

        If selectedDataRow Is Nothing Then
            MessageBox.Show("Could not find the selected hospital in the results.")
            Return Nothing
        End If

        Dim hosp As New HospitalContext()
        ' (populate hosp as before)
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
        hosp.State = SafeStr(selectedDataRow, state)
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
        hosp.LastDataRow = selectedDataRow

        Results.SelectedHospital = hosp
        Return hosp
    End Function

    ' Profile button click
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim hosp = GetSelectedHospitalContext()
        If hosp Is Nothing Then Return
        Me.Hide()
        Dim profileForm As New Profile(hosp)
        profileForm.Show()
    End Sub



    ' Example for Button3_Click (Financial):
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim hosp = GetSelectedHospitalContext()
        If hosp Is Nothing Then Return
        Hide()
        Dim financialForm As New Financial(hosp)
        financialForm.Show()
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

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim hosp = GetSelectedHospitalContext()
        If hosp Is Nothing Then Return
        Me.Hide()
        Dim inpatientForm As New Inpatient(hosp)
        inpatientForm.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim hosp = GetSelectedHospitalContext()
        If hosp Is Nothing Then Return
        Me.Hide()
        Dim deptForm As New Departments(hosp)
        deptForm.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim hosp = GetSelectedHospitalContext()
        If hosp Is Nothing Then Return
        Me.Hide()
        Dim finIndForm As New FinInd(hosp)
        finIndForm.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim hosp = GetSelectedHospitalContext()
        If hosp Is Nothing Then Return
        Me.Hide()
        Dim qualityForm As New Quality(hosp)
        qualityForm.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim hosp = GetSelectedHospitalContext()
        If hosp Is Nothing Then Return
        Me.Hide()
        Dim outpatientForm As New Outpatient(hosp)
        outpatientForm.Show()
    End Sub

    ' Back to Search button click
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Me.Hide()
        Search.Show()
    End Sub

    ' Add similar selection logic to Button6_Click (Inpatient) if needed

    Public Shared Function GetHospitalContextByCMSNum(cmsNum As String) As HospitalContext
        Dim cmsCols = New String() {"Provider CCN", "CMSNum", "PRVDR_NUM", "CCN", "ccn"}
        For Each row As DataRow In resultsTable.Rows
            For Each col In cmsCols
                If row.Table.Columns.Contains(col) AndAlso row(col).ToString() = cmsNum Then
                    Dim ctx As New HospitalContext()
                    ctx.CMSNum = row(col).ToString()
                    ctx.Name = If(row.Table.Columns.Contains("FAC_NAME"), row("FAC_NAME").ToString(), "")
                    ctx.Address = If(row.Table.Columns.Contains("ST_ADR"), row("ST_ADR").ToString(), "")
                    ctx.Zip = If(row.Table.Columns.Contains("ZIP"), row("ZIP").ToString(), "")
                    ctx.County = If(row.Table.Columns.Contains("COUNTY"), row("COUNTY").ToString(), "")
                    ctx.State = If(row.Table.Columns.Contains("STATE"), row("STATE").ToString(), "")
                    ctx.LastDataRow = row
                    ' ...populate other fields as needed...
                    Return ctx
                End If
            Next
        Next
        Return Nothing
    End Function


End Class
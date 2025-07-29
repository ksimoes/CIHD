Public Class Results

    Public Property SelectedState As String
    Private resultsTable As DataTable
    Public strCMSnum As String
    ' Shared context for selected hospital
    Public Shared SelectedHospital As New HospitalContext()

    ' Call this from Search form to set and display results
    Public Sub SetResults(dt As DataTable)
        resultsTable = dt

        CheckedListBox1.Items.Clear()
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
            ' Try common state column names
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
            Dim display As String = name
            If city <> "" Or state <> "" Or zip <> "" Then
                display &= $" ({city}, {state} {zip})"
            End If
            CheckedListBox1.Items.Add(display.Trim())
        Next
        Dim items As New List(Of String)
        For Each item In CheckedListBox1.Items
            items.Add(item.ToString())
        Next

        ' Sort the list
        items.Sort()

        ' Clear the CheckedListBox and re-add sorted items
        CheckedListBox1.Items.Clear()
        For Each item In items
            CheckedListBox1.Items.Add(item)
        Next

    End Sub

    ' Profile button click
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckedListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If
        retrieveProfile()
        Profile.ShowProfile(SelectedHospital)
        Hide()
        Profile.Show()
    End Sub

    Public Function GetSelectedHospital() As HospitalContext
        retrieveProfile()
        Return SelectedHospital
    End Function

    Public Sub retrieveProfile()
        Dim selectedDisplay As String = CheckedListBox1.SelectedItem.ToString().Trim()
        Dim selectedName As String = selectedDisplay
        Dim idx = selectedDisplay.IndexOf(" (")
        If idx > 0 Then
            selectedName = selectedDisplay.Substring(0, idx)
        End If
        Dim selectedRow As DataRow = Nothing

        ' Use the same display column logic as SetResults
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

        ' Find the selected row based on the display column
        selectedRow = resultsTable.Select("[" & displayCol & "] = '" & selectedName.Replace("'", "''") & "'").FirstOrDefault()

        If selectedRow IsNot Nothing Then
            Dim hosp As New HospitalContext()

            ' Core Identifiers
            hosp.HospitalId = If(resultsTable.Columns.Contains("LicenseNum"), SafeInt(selectedRow("LicenseNum")), 0)
            hosp.CMSNum = SafeStr(selectedRow, "Provider CCN", "CMSNum", "PRVDR_NUM")
            hosp.NPI = SafeStr(selectedRow, "NPI", "npi")
            hosp.Name = SafeStr(selectedRow, "FAC_NAME", "PRVDR_NM", "PRVDR_NAME", "ORGANIZATION NAME", "organization_name", "Facility Name", "Hospital Name", "provider_name")

            ' Location & Contact
            hosp.Address = SafeStr(selectedRow, "ADDR_LN_1_TXT", "ADDRESS LINE 1", "address_line_1", "Address", "Facility Address", "Street Address", "STREET", "STREET1", "STREET_ADDRESS")
            hosp.City = SafeStr(selectedRow, "CITY_NM", "City")
            hosp.Zip = SafeStr(selectedRow, "ZIP_CD", "Zip Code", "ZIP")
            hosp.County = SafeStr(selectedRow, "County Name", "County")
            hosp.Phone = SafeStr(selectedRow, "Phone", "Telephone Number")
            hosp.Website = SafeStr(selectedRow, "Website")

            ' Classification
            hosp.CBSAnum = SafeStr(selectedRow, "CBSA", "CBSA Code")
            hosp.FacilityType = SafeStr(selectedRow, "Facility Type")
            hosp.RuralOUrban = SafeStr(selectedRow, "Rural Versus Urban")

            ' Capacity & Staffing
            hosp.NumOfBeds = SafeInt(selectedRow, "Number of Beds", "General Med/Surg Beds")
            hosp.NumOfEmployees = SafeInt(selectedRow, "Total Employees")
            hosp.TotalDays = SafeInt(selectedRow, "Total Days", "Inpatient Days")
            hosp.TotalDischarges = SafeInt(selectedRow, "Total Discharges")

            ' Financials
            hosp.TotalPatientRev = SafeDec(selectedRow, "Total Patient Revenue")
            hosp.NetPatientRev = SafeDec(selectedRow, "Net Patient Revenue")
            hosp.CharityCost = SafeDec(selectedRow, "Cost of Charity Care")
            hosp.UncompensatedCost = SafeDec(selectedRow, "Cost of Uncompensated Care")

            ' Additional/Expandable fields
            hosp.TotalCurrentAssets = SafeDec(selectedRow, "Total Current Assets")
            hosp.TotalAssets = SafeDec(selectedRow, "Total Assets")
            hosp.NetIncome = SafeDec(selectedRow, "Net Income")
            hosp.TotalOperatingRevenue = SafeDec(selectedRow, "Net Patient Revenue")
            hosp.TotalOperatingExpense = SafeDec(selectedRow, "Less Total Operating Expense")
            hosp.TotalLiabilities = SafeDec(selectedRow, "Total Liabilities")
            hosp.TotalCurrentLiabilities = SafeDec(selectedRow, "Total Current Liabilities")
            hosp.TotalLongTermLiabilities = SafeDec(selectedRow, "Total Long Term Liabilities")
            hosp.DepreciationCost = SafeDec(selectedRow, "Depreciation Cost")
            hosp.LeaseCost = SafeDec(selectedRow, "Leasehold Improvements")
            hosp.Inventory = SafeDec(selectedRow, "Inventory")
            hosp.NotesReceivable = SafeDec(selectedRow, "Notes Receivable")
            hosp.MarketSecurities = SafeDec(selectedRow, "Temporary Investments")
            hosp.Investments = SafeDec(selectedRow, "Investments")

            Results.SelectedHospital = hosp
            hosp.LastDataRow = selectedRow
        End If
    End Sub

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

    End Sub

    'Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
    '    Me.Hide()
    '    Inpatient.Show()
    'End Sub
    Private Async Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If CheckedListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If
        retrieveProfile()
        Hide()
        Inpatient.Show()
        Await Inpatient.LoadPatientOriginDataAsync(Results.SelectedHospital)
        Await Inpatient.LoadCeoDataAsync(Results.SelectedHospital)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
        Departments.Show()
    End Sub

    'Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
    '    Me.Hide()
    '    Financial.Show()
    'End Sub
    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If CheckedListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If
        retrieveProfile()
        Hide()
        Financial.Show()
        Await Financial.ShowFinancialData(Results.SelectedHospital.HospitalId, Results.SelectedHospital.State)
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
End Class
Imports System.Runtime.CompilerServices

Public Class Results

    Public Property SelectedState As String
    Public Shared resultsTable As DataTable
    Public strCMSnum As String
    Public Shared SelectedHospital As New HospitalContext()

    ' Cached column mappings to avoid repeated lookups
    Private displayColumnCache As String = ""
    Private bedCountColumnCache As String = ""
    Private cmsColumnCache As String = ""

    ' Column name mappings - centralized for easy maintenance
    Private Shared ReadOnly NameColumns As String() = {
        "FAC_NAME", "ORGANIZATION NAME", "facility_name", "provider_name",
        "Rndrng_Prvdr_Org_Name", "Hospital Name", "Facility Name"
    }

    Private Shared ReadOnly BedCountColumns As String() = {
        "Number of Beds", "General Med/Surg Beds"
    }

    Private Shared ReadOnly CMSColumns As String() = {
        "Provider CCN", "CMSNum", "PRVDR_NUM", "CCN", "ccn"
    }

    Private Shared ReadOnly StateColumns As String() = {
        "State", "STATE", "State Code"
    }

    Private Shared ReadOnly ZipColumns As String() = {
        "Zip Code", "ZIP"
    }

    ' Type of Control mapping
    Private Shared ReadOnly TypeOfControlMap As New Dictionary(Of String, String) From {
        {"1", "Voluntary Non‐Profit‐Church"},
        {"2", "Voluntary Non‐Profit‐Other"},
        {"3", "Proprietary‐Individual"},
        {"4", "Proprietary‐Corporation"},
        {"5", "Proprietary‐Partnership"},
        {"6", "Proprietary‐Other"},
        {"7", "Governmental‐Federal"},
        {"8", "Governmental‐City‐County"},
        {"9", "Governmental‐County"},
        {"10", "Governmental‐State"},
        {"11", "Governmental‐Hospital District"},
        {"12", "Governmental‐City"},
        {"13", "Governmental‐Other"}
    }

    ''' <summary>
    ''' Sets the results table and displays data in the DataGridView
    ''' </summary>
    Public Sub SetResults(dt As DataTable, Optional filterSummary As String = "")
        resultsTable = dt

        ' Initialize DataGridView columns only once
        InitializeDataGridViewColumns()
        dgvResults.Rows.Clear()

        ' Set row colors
        dgvResults.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow
        dgvResults.DefaultCellStyle.BackColor = Color.White

        ' Cache column lookups for better performance
        displayColumnCache = FindColumn(dt, NameColumns)
        bedCountColumnCache = FindColumn(dt, BedCountColumns)
        cmsColumnCache = FindColumn(dt, CMSColumns)

        ' Populate rows
        For Each row As DataRow In dt.Rows
            AddRowToGrid(row, dt)
        Next

        ' Update UI
        lblMatches.Text = $"{dt.Rows.Count} result(s) found"
        lblFilters.Text = If(String.IsNullOrWhiteSpace(filterSummary), "", $"Filters: {filterSummary}")
    End Sub

    ''' <summary>
    ''' Initialize DataGridView columns if not already set up
    ''' </summary>
    Private Sub InitializeDataGridViewColumns()
        If dgvResults.Columns.Count = 0 Then
            With dgvResults
                .Columns.Add(New DataGridViewCheckBoxColumn() With {
                    .Name = "Select", .HeaderText = "", .Width = 30
                })
                .Columns.Add("HospitalName", "Hospital Name")
                .Columns.Add("City", "City")
                .Columns.Add("State", "State")
                .Columns.Add("Zip", "Zip")
                .Columns.Add("BedCount", "Bed Count")
                .Columns.Add("CMSCCN", "CMS/CCN")
                .Columns.Add("TypeOfControl", "Type of Control")
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .MultiSelect = False
            End With
        End If
    End Sub

    ''' <summary>
    ''' Finds the first matching column name from a list of possibilities
    ''' </summary>
    Private Function FindColumn(dt As DataTable, columnNames As String()) As String
        For Each colName In columnNames
            If dt.Columns.Contains(colName) Then Return colName
        Next
        Return If(dt.Columns.Count > 0, dt.Columns(0).ColumnName, "")
    End Function

    ''' <summary>
    ''' Adds a single row to the DataGridView
    ''' </summary>
    Private Sub AddRowToGrid(row As DataRow, dt As DataTable)
        Dim name As String = If(displayColumnCache <> "", row(displayColumnCache).ToString(), "")
        Dim city As String = SafeStr(row, "City")
        Dim state As String = SafeStr(row, StateColumns)
        Dim zip As String = SafeStr(row, ZipColumns)
        Dim bedCount As String = If(bedCountColumnCache <> "" AndAlso Not IsDBNull(row(bedCountColumnCache)),
                                     row(bedCountColumnCache).ToString(), "")
        Dim cmsccn As String = If(cmsColumnCache <> "" AndAlso Not IsDBNull(row(cmsColumnCache)),
                                   row(cmsColumnCache).ToString(), "")
        Dim typeOfControlCode As String = SafeStr(row, "Type of Control")
        Dim typeOfControlDesc As String = If(TypeOfControlMap.ContainsKey(typeOfControlCode),
                                             TypeOfControlMap(typeOfControlCode), typeOfControlCode)

        dgvResults.Rows.Add(False, name, city, state, zip, bedCount, cmsccn, typeOfControlDesc)
    End Sub

    ''' <summary>
    ''' Ensure only one checkbox is checked at a time (single selection)
    ''' </summary>
    Private Sub dgvResults_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) _
        Handles dgvResults.CellValueChanged
        If e.ColumnIndex = dgvResults.Columns("Select").Index AndAlso
           CBool(dgvResults.Rows(e.RowIndex).Cells("Select").Value) Then
            For i As Integer = 0 To dgvResults.Rows.Count - 1
                If i <> e.RowIndex Then
                    dgvResults.Rows(i).Cells("Select").Value = False
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Gets the selected hospital context from the DataGridView
    ''' </summary>
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

        ' Find the display column
        Dim displayCol As String = FindColumn(resultsTable, NameColumns)

        ' Find matching row in results table
        Dim selectedDataRow As DataRow = FindMatchingDataRow(name, city, state, zip, displayCol)

        If selectedDataRow Is Nothing Then
            MessageBox.Show("Could not find the selected hospital in the results.")
            Return Nothing
        End If

        ' Build hospital context
        Dim hosp As HospitalContext = BuildHospitalContext(selectedDataRow)
        Results.SelectedHospital = hosp
        Return hosp
    End Function

    ''' <summary>
    ''' Finds a matching DataRow based on name, city, state, and zip
    ''' </summary>
    Private Function FindMatchingDataRow(name As String, city As String, state As String,
                                        zip As String, displayCol As String) As DataRow
        Return resultsTable.AsEnumerable().FirstOrDefault(
            Function(r)
                Return r.Field(Of String)(displayCol) = name AndAlso
                       MatchesColumn(r, "City", city) AndAlso
                       MatchesStateColumn(r, state) AndAlso
                       MatchesZipColumn(r, zip)
            End Function)
    End Function

    ''' <summary>
    ''' Helper to match a column value if it exists
    ''' </summary>
    Private Function MatchesColumn(row As DataRow, columnName As String, value As String) As Boolean
        Return Not resultsTable.Columns.Contains(columnName) OrElse
               row.Field(Of String)(columnName) = value
    End Function

    ''' <summary>
    ''' Helper to match state column (multiple possible names)
    ''' </summary>
    Private Function MatchesStateColumn(row As DataRow, state As String) As Boolean
        For Each colName In StateColumns
            If resultsTable.Columns.Contains(colName) Then
                Return row.Field(Of String)(colName) = state
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Helper to match zip column (multiple possible names)
    ''' </summary>
    Private Function MatchesZipColumn(row As DataRow, zip As String) As Boolean
        For Each colName In ZipColumns
            If resultsTable.Columns.Contains(colName) Then
                Return row.Field(Of String)(colName) = zip
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Builds a complete HospitalContext from a DataRow
    ''' </summary>
    Private Function BuildHospitalContext(row As DataRow) As HospitalContext
        Dim hosp As New HospitalContext()

        ' Basic information
        hosp.CMSNum = SafeStr(row, CMSColumns)
        hosp.Name = SafeStr(row, NameColumns)
        hosp.Address = SafeStr(row, "ST_ADR", "Address", "Street Address")
        hosp.City = SafeStr(row, "City", "CITY")
        hosp.State = SafeStr(row, StateColumns)
        hosp.Zip = SafeStr(row, ZipColumns)
        hosp.County = SafeStr(row, "COUNTY", "County")
        hosp.Phone = SafeStr(row, "Phone Number", "PHONE")
        hosp.Email = SafeStr(row, "Email", "EMAIL")

        ' Facility characteristics
        hosp.BedCount = SafeInt(row, BedCountColumns)
        hosp.TypeOfControl = SafeStr(row, "Type of Control")
        hosp.EmergencyServices = SafeStr(row, "Emergency Services") = "Yes"

        ' Financial data
        hosp.TotalDischarges = SafeInt(row, "Total Discharges")
        hosp.TotalPatientRev = SafeDec(row, "Total Patient Revenue")
        hosp.NetPatientRev = SafeDec(row, "Net Patient Revenue")
        hosp.CharityCost = SafeDec(row, "Cost of Charity Care")
        hosp.UncompensatedCost = SafeDec(row, "Cost of Uncompensated Care")
        hosp.TotalCurrentAssets = SafeDec(row, "Total Current Assets")
        hosp.TotalAssets = SafeDec(row, "Total Assets")
        hosp.NetIncome = SafeDec(row, "Net Income")
        hosp.TotalOperatingRevenue = SafeDec(row, "Net Patient Revenue")
        hosp.TotalOperatingExpense = SafeDec(row, "Less Total Operating Expense")
        hosp.TotalLiabilities = SafeDec(row, "Total Liabilities")
        hosp.TotalCurrentLiabilities = SafeDec(row, "Total Current Liabilities")
        hosp.TotalLongTermLiabilities = SafeDec(row, "Total Long Term Liabilities")
        hosp.DepreciationCost = SafeDec(row, "Depreciation Cost")
        hosp.LeaseCost = SafeDec(row, "Leasehold Improvements")
        hosp.Inventory = SafeDec(row, "Inventory")
        hosp.NotesReceivable = SafeDec(row, "Notes Receivable")
        hosp.MarketSecurities = SafeDec(row, "Temporary Investments")
        hosp.Investments = SafeDec(row, "Investments")
        hosp.LastDataRow = row

        Return hosp
    End Function

    ''' <summary>
    ''' Generic navigation handler for all button clicks
    ''' </summary>
    Private Sub NavigateToForm(formFactory As Func(Of HospitalContext, Form))
        Dim hosp = GetSelectedHospitalContext()
        If hosp Is Nothing Then Return
        Me.Hide()
        formFactory(hosp).Show()
    End Sub

    ' Navigation button handlers
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        NavigateToForm(Function(h) New Profile(h))
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        NavigateToForm(Function(h) New Departments(h))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        NavigateToForm(Function(h) New Financial(h))
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        NavigateToForm(Function(h) New FinInd(h))
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        NavigateToForm(Function(h) New Quality(h))
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        NavigateToForm(Function(h) New Inpatient(h))
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        NavigateToForm(Function(h) New Outpatient(h))
    End Sub

    Private Sub btnNavigation_Click(sender As Object, e As EventArgs) Handles btnNavigation.Click
        Dim hosp = GetSelectedHospitalContext()
        If hosp Is Nothing Then Return
        Dim navPopup As New NavigationPopupForm(hosp)
        navPopup.Show()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Me.Hide()
        Search.Show()
    End Sub

    ''' <summary>
    ''' Helper function to safely retrieve string values with multiple column name options
    ''' </summary>
    Private Function SafeStr(row As DataRow, ParamArray names() As String) As String
        For Each n In names
            If row.Table.Columns.Contains(n) AndAlso Not IsDBNull(row(n)) Then
                Return row(n).ToString().Trim()
            End If
        Next
        Return ""
    End Function

    ''' <summary>
    ''' Helper function to safely retrieve integer values with multiple column name options
    ''' </summary>
    Private Function SafeInt(row As DataRow, ParamArray names() As String) As Integer
        For Each n In names
            If row.Table.Columns.Contains(n) AndAlso Not IsDBNull(row(n)) Then
                Dim val As Integer
                If Integer.TryParse(row(n).ToString(), val) Then Return val
            End If
        Next
        Return 0
    End Function

    ''' <summary>
    ''' Helper function to safely retrieve decimal values with multiple column name options
    ''' </summary>
    Private Function SafeDec(row As DataRow, ParamArray names() As String) As Decimal
        For Each n In names
            If row.Table.Columns.Contains(n) AndAlso Not IsDBNull(row(n)) Then
                Dim val As Decimal
                If Decimal.TryParse(row(n).ToString().Replace("$", "").Replace(",", ""), val) Then
                    Return val
                End If
            End If
        Next
        Return 0D
    End Function

    ''' <summary>
    ''' Form load event - configure DataGridView
    ''' </summary>
    Private Sub Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With dgvResults
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = False
            If .Columns.Contains("Select") Then
                .Columns("Select").ReadOnly = False
            End If
        End With

        ' Make all columns except Select read-only
        For Each col As DataGridViewColumn In dgvResults.Columns
            If col.Name <> "Select" Then col.ReadOnly = True
        Next
    End Sub

    ''' <summary>
    ''' Retrieves hospital context by CMS number from the results table
    ''' </summary>
    Public Shared Function GetHospitalContextByCMSNum(cmsNum As String) As HospitalContext
        If resultsTable Is Nothing Then Return Nothing

        For Each row As DataRow In resultsTable.Rows
            For Each col In CMSColumns
                If row.Table.Columns.Contains(col) AndAlso row(col).ToString() = cmsNum Then
                    Dim ctx As New HospitalContext With {
                        .CMSNum = row(col).ToString(),
                        .Name = If(row.Table.Columns.Contains("FAC_NAME"), row("FAC_NAME").ToString(), ""),
                        .Address = If(row.Table.Columns.Contains("ST_ADR"), row("ST_ADR").ToString(), ""),
                        .Zip = If(row.Table.Columns.Contains("ZIP"), row("ZIP").ToString(), ""),
                        .County = If(row.Table.Columns.Contains("COUNTY"), row("COUNTY").ToString(), ""),
                        .State = If(row.Table.Columns.Contains("STATE"), row("STATE").ToString(), ""),
                        .LastDataRow = row
                    }
                    Return ctx
                End If
            Next
        Next
        Return Nothing
    End Function

End Class
' Optimized Results.vb - Enhanced Performance Version

Imports System.Runtime.CompilerServices

Public Class Results

    Public Property SelectedState As String
    Public Shared resultsTable As DataTable
    Public strCMSnum As String
    Public Shared SelectedHospital As New HospitalContext()

    ' ============================================================================
    ' CACHED COLUMN MAPPINGS
    ' ============================================================================
    Private displayColumnCache As String = ""
    Private bedCountColumnCache As String = ""
    Private cmsColumnCache As String = ""

    ' ============================================================================
    ' COLUMN NAME MAPPINGS - Centralized for easy maintenance
    ' ============================================================================
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

    ' ============================================================================
    ' PAGINATION SUPPORT
    ' ============================================================================
    Private Const PageSize As Integer = 100
    Private CurrentPage As Integer = 0
    Private TotalPages As Integer = 0
    Private PaginationEnabled As Boolean = False

    ' ============================================================================
    ' MAIN METHODS
    ' ============================================================================

    ''' <summary>
    ''' Sets the results table and displays data in the DataGridView (OPTIMIZED)
    ''' </summary>
    Public Sub SetResults(dt As DataTable, Optional filterSummary As String = "")
        resultsTable = dt

        ' Initialize DataGridView columns only once
        If dgvResults.Columns.Count = 0 Then
            InitializeDataGridViewColumns()
        End If

        ' Suspend layout for better performance during bulk operations
        dgvResults.SuspendLayout()
        Try
            ' Cache column lookups BEFORE loop - significant performance improvement
            displayColumnCache = FindColumn(dt, NameColumns)
            bedCountColumnCache = FindColumn(dt, BedCountColumns)
            cmsColumnCache = FindColumn(dt, CMSColumns)

            ' Decide whether to use pagination
            If dt.Rows.Count > PageSize Then
                EnablePagination(dt)
            Else
                DisablePagination()
                PopulateAllRows(dt)
            End If

            ' Update UI labels
            lblMatches.Text = $"{dt.Rows.Count:N0} result(s) found"
            lblFilters.Text = If(String.IsNullOrWhiteSpace(filterSummary), "", $"Filters: {filterSummary}")

        Finally
            dgvResults.ResumeLayout()
        End Try
    End Sub

    ''' <summary>
    ''' Populate all rows at once (for smaller datasets)
    ''' </summary>
    Private Sub PopulateAllRows(dt As DataTable)
        dgvResults.Rows.Clear()

        ' Batch add rows for better performance
        Dim rowsToAdd As New List(Of DataGridViewRow)(dt.Rows.Count)

        For Each row As DataRow In dt.Rows
            rowsToAdd.Add(CreateGridRow(row, dt))
        Next

        ' Single AddRange is much faster than individual Adds
        If rowsToAdd.Count > 0 Then
            dgvResults.Rows.AddRange(rowsToAdd.ToArray())
        End If

        ' Set alternating row colors
        SetRowColors()
    End Sub

    ''' <summary>
    ''' Create a single DataGridView row from a DataRow
    ''' </summary>
    Private Function CreateGridRow(row As DataRow, dt As DataTable) As DataGridViewRow
        Dim gridRow As New DataGridViewRow()
        gridRow.CreateCells(dgvResults)

        ' Checkbox
        gridRow.Cells(0).Value = False

        ' Hospital Name
        gridRow.Cells(1).Value = If(displayColumnCache <> "", SafeGetValue(row, displayColumnCache), "")

        ' City
        gridRow.Cells(2).Value = SafeStr(row, "City")

        ' State
        gridRow.Cells(3).Value = SafeStr(row, StateColumns)

        ' Zip
        gridRow.Cells(4).Value = SafeStr(row, ZipColumns)

        ' Bed Count
        gridRow.Cells(5).Value = If(bedCountColumnCache <> "" AndAlso Not IsDBNull(row(bedCountColumnCache)),
                                     row(bedCountColumnCache).ToString(), "")

        ' CMS/CCN
        gridRow.Cells(6).Value = If(cmsColumnCache <> "" AndAlso Not IsDBNull(row(cmsColumnCache)),
                                     row(cmsColumnCache).ToString(), "")

        ' Type of Control
        Dim typeOfControlCode As String = SafeStr(row, "Type of Control")
        gridRow.Cells(7).Value = If(TypeOfControlMap.ContainsKey(typeOfControlCode),
                                     TypeOfControlMap(typeOfControlCode), typeOfControlCode)

        Return gridRow
    End Function

    ''' <summary>
    ''' Helper to safely get value from DataRow
    ''' </summary>
    Private Function SafeGetValue(row As DataRow, columnName As String) As String
        If Not IsDBNull(row(columnName)) Then
            Return row(columnName).ToString()
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Set alternating row colors
    ''' </summary>
    Private Sub SetRowColors()
        For i = 0 To dgvResults.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvResults.Rows(i).DefaultCellStyle.BackColor = Color.White
            Else
                dgvResults.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
            End If
        Next
    End Sub

    ' ============================================================================
    ' PAGINATION METHODS
    ' ============================================================================

    Private Sub EnablePagination(dt As DataTable)
        PaginationEnabled = True
        TotalPages = CInt(Math.Ceiling(dt.Rows.Count / CDbl(PageSize)))
        CurrentPage = 0
        LoadPage(0)

        ' Show pagination controls (add these to your form designer)
        ' btnPrevPage.Visible = True
        ' btnNextPage.Visible = True
        ' lblPagination.Visible = True
    End Sub

    Private Sub DisablePagination()
        PaginationEnabled = False
        ' Hide pagination controls
        ' btnPrevPage.Visible = False
        ' btnNextPage.Visible = False
        ' lblPagination.Visible = False
    End Sub

    Private Sub LoadPage(pageNumber As Integer)
        If resultsTable Is Nothing Then Return

        CurrentPage = pageNumber
        Dim startIndex = pageNumber * PageSize
        Dim endIndex = Math.Min(startIndex + PageSize, resultsTable.Rows.Count)

        dgvResults.Rows.Clear()

        ' Create rows for current page
        Dim rowsToAdd As New List(Of DataGridViewRow)
        For i = startIndex To endIndex - 1
            rowsToAdd.Add(CreateGridRow(resultsTable.Rows(i), resultsTable))
        Next

        If rowsToAdd.Count > 0 Then
            dgvResults.Rows.AddRange(rowsToAdd.ToArray())
        End If

        SetRowColors()

        ' Update pagination label
        ' lblPagination.Text = $"Page {pageNumber + 1} of {TotalPages} (Showing {startIndex + 1}-{endIndex} of {resultsTable.Rows.Count})"
    End Sub

    ' Pagination button handlers (add these to your form)
    'Private Sub btnPrevPage_Click(sender As Object, e As EventArgs)
    '    If CurrentPage > 0 Then
    '        LoadPage(CurrentPage - 1)
    '    End If
    'End Sub

    'Private Sub btnNextPage_Click(sender As Object, e As EventArgs)
    '    If CurrentPage < TotalPages - 1 Then
    '        LoadPage(CurrentPage + 1)
    '    End If
    'End Sub

    ' ============================================================================
    ' DATAGRIDVIEW INITIALIZATION
    ' ============================================================================

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
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .ReadOnly = False
            End With

            ' Make all columns except Select read-only
            For Each col As DataGridViewColumn In dgvResults.Columns
                If col.Name <> "Select" Then
                    col.ReadOnly = True
                End If
            Next
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

    ' ============================================================================
    ' CHECKBOX HANDLING
    ' ============================================================================

    ''' <summary>
    ''' Ensure only one checkbox is checked at a time (single selection)
    ''' </summary>
    Private Sub dgvResults_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) _
        Handles dgvResults.CellValueChanged
        If e.RowIndex < 0 OrElse e.ColumnIndex <> dgvResults.Columns("Select").Index Then Return

        If CBool(dgvResults.Rows(e.RowIndex).Cells("Select").Value) Then
            For i As Integer = 0 To dgvResults.Rows.Count - 1
                If i <> e.RowIndex Then
                    dgvResults.Rows(i).Cells("Select").Value = False
                End If
            Next
        End If
    End Sub

    ' ============================================================================
    ' HOSPITAL SELECTION
    ' ============================================================================

    ''' <summary>
    ''' Gets the selected hospital context from the DataGridView
    ''' </summary>
    Private Function GetSelectedHospitalContext() As HospitalContext
        Dim selectedRow As DataGridViewRow = dgvResults.Rows.Cast(Of DataGridViewRow)().
            FirstOrDefault(Function(r) CBool(r.Cells("Select").Value))

        If selectedRow Is Nothing Then
            MessageBox.Show("Please select a hospital first.")
            Return Nothing
        End If

        Dim name As String = If(selectedRow.Cells("HospitalName").Value IsNot Nothing, selectedRow.Cells("HospitalName").Value.ToString(), "")
        Dim city As String = If(selectedRow.Cells("City").Value IsNot Nothing, selectedRow.Cells("City").Value.ToString(), "")
        Dim state As String = If(selectedRow.Cells("State").Value IsNot Nothing, selectedRow.Cells("State").Value.ToString(), "")
        Dim zip As String = If(selectedRow.Cells("Zip").Value IsNot Nothing, selectedRow.Cells("Zip").Value.ToString(), "")

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
                       MatchesState(r, state) AndAlso
                       MatchesZip(r, zip)
            End Function)
    End Function

    Private Function MatchesColumn(row As DataRow, columnName As String, value As String) As Boolean
        If row.Table.Columns.Contains(columnName) Then
            Return row.Field(Of String)(columnName) = value
        End If
        Return True
    End Function

    Private Function MatchesState(row As DataRow, state As String) As Boolean
        For Each colName In StateColumns
            If resultsTable.Columns.Contains(colName) Then
                Return row.Field(Of String)(colName) = state
            End If
        Next
        Return True
    End Function

    Private Function MatchesZip(row As DataRow, zip As String) As Boolean
        For Each colName In ZipColumns
            If resultsTable.Columns.Contains(colName) Then
                Return row.Field(Of String)(colName) = zip
            End If
        Next
        Return True
    End Function

    ' ============================================================================
    ' HOSPITAL CONTEXT BUILDER
    ' ============================================================================

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

    ' ============================================================================
    ' NAVIGATION METHODS
    ' ============================================================================

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

    ' ============================================================================
    ' HELPER METHODS
    ' ============================================================================

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

    ' ============================================================================
    ' FORM EVENTS
    ' ============================================================================

    ''' <summary>
    ''' Form load event - configure DataGridView
    ''' </summary>
    Private Sub Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeDataGridViewColumns()
    End Sub

    ' ============================================================================
    ' STATIC HELPER
    ' ============================================================================

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
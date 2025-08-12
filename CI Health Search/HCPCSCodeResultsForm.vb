Imports System.Data

Public Class HCPCSCodeResultsForm
    Private WithEvents cmsColumns As New ContextMenuStrip()
    Private currentDataTable As DataTable
    Private filterPanel As Panel
    Private columnFilters As New Dictionary(Of String, ComboBox)
    Private currentPage As Integer = 0
    Private pageSize As Integer = 100
    Private code As String
    Private state As String
    Private totalRowCount As Integer = 0

    ' Constructor for paged results
    Public Sub New(hcpcsCode As String, stateFilter As String)
        InitializeComponent()
        code = hcpcsCode
        state = stateFilter

        ' Create filter panel for column filters
        filterPanel = New Panel() With {
            .Height = 30,
            .Dock = DockStyle.Top
        }
        Me.Controls.Add(filterPanel)
        Me.Controls.SetChildIndex(filterPanel, 0) ' Ensure it's above the DataGridView
    End Sub

    ' Load the first page when the form loads
    Private Async Sub HCPCSCodeResultsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadPage()
    End Sub

    ' Load a single page of results
    Private Async Function LoadPage() As Task
        btnNextPage.Enabled = False
        btnPrevPage.Enabled = False
        lblStatus.Text = $"Loading page {currentPage + 1}..."
        Dim offset = currentPage * pageSize
        Dim dt = Await CType(Owner, Search).FetchHCPCSPage(code, state, pageSize, offset)
        currentDataTable = dt
        dgvCodeResults.DataSource = dt

        ' Fetch total row count only on the first page
        If currentPage = 0 Then
            Dim allRowsDt = Await CType(Owner, Search).FetchHCPCSPage(code, state, 100000, 0)
            totalRowCount = allRowsDt.Rows.Count
        End If

        Me.Text = $"HCPCS Code Results - {code}   (Total Rows: {totalRowCount})"
        lblStatus.Text = $"Page {currentPage + 1} (showing {dt.Rows.Count} results)"
        btnPrevPage.Enabled = currentPage > 0
        btnNextPage.Enabled = dt.Rows.Count = pageSize

        SetupColumnFilters()
    End Function

    ' Dynamically create ComboBoxes for each column above the DataGridView
    Private Sub SetupColumnFilters()
        filterPanel.Controls.Clear()
        columnFilters.Clear()
        If currentDataTable Is Nothing OrElse dgvCodeResults.Columns.Count = 0 Then Return

        For Each col As DataGridViewColumn In dgvCodeResults.Columns
            If Not col.Visible Then Continue For
            Dim cb As New ComboBox() With {
                .Name = "cbFilter_" & col.Name,
                .Width = col.Width,
                .Left = dgvCodeResults.GetCellDisplayRectangle(col.Index, -1, True).Left,
                .Top = 0,
                .DropDownStyle = ComboBoxStyle.DropDown, ' Editable for search
                .Tag = col.Name
            }
            cb.Items.Add("(All)")
            Dim uniqueVals = currentDataTable.AsEnumerable().
                Select(Function(r) r(col.Name)?.ToString()).
                Where(Function(v) Not String.IsNullOrEmpty(v)).
                Distinct().
                OrderBy(Function(v) v).
                ToList()
            For Each v In uniqueVals
                cb.Items.Add(v)
            Next
            cb.SelectedIndex = 0
            AddHandler cb.SelectedIndexChanged, AddressOf ColumnFilterChanged
            AddHandler cb.TextUpdate, AddressOf ComboBox_TextUpdate
            filterPanel.Controls.Add(cb)
            columnFilters(col.Name) = cb
        Next
        ' Optional: handle resizing
        AddHandler dgvCodeResults.ColumnWidthChanged, AddressOf dgvCodeResults_ColumnWidthChanged
        AddHandler dgvCodeResults.Scroll, AddressOf dgvCodeResults_Scroll
        PositionFilterCombos()
    End Sub

    ' Keep ComboBoxes aligned with columns
    Private Sub PositionFilterCombos()
        For Each col As DataGridViewColumn In dgvCodeResults.Columns
            If columnFilters.ContainsKey(col.Name) Then
                Dim cb = columnFilters(col.Name)
                Dim rect = dgvCodeResults.GetCellDisplayRectangle(col.Index, -1, True)
                cb.Left = rect.Left
                cb.Width = rect.Width
            End If
        Next
    End Sub

    Private Sub dgvCodeResults_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs)
        PositionFilterCombos()
    End Sub

    Private Sub dgvCodeResults_Scroll(sender As Object, e As ScrollEventArgs)
        PositionFilterCombos()
    End Sub

    ' Search-as-you-type for ComboBox filter
    Private Sub ComboBox_TextUpdate(sender As Object, e As EventArgs)
        Dim cb = CType(sender, ComboBox)
        Dim colName = cb.Tag.ToString()
        If currentDataTable Is Nothing OrElse String.IsNullOrEmpty(colName) Then Return

        Dim searchText = cb.Text.Trim().ToLower()
        Dim allVals = currentDataTable.AsEnumerable().
            Select(Function(r) r(colName)?.ToString()).
            Where(Function(v) Not String.IsNullOrEmpty(v)).
            Distinct().
            OrderBy(Function(v) v).
            ToList()

        Dim currentSelection = cb.Text

        cb.Items.Clear()
        cb.Items.Add("(All)")
        For Each v In allVals
            If v.ToLower().Contains(searchText) Then
                cb.Items.Add(v)
            End If
        Next

        cb.DroppedDown = True
        cb.Text = currentSelection
        cb.SelectionStart = cb.Text.Length
        cb.SelectionLength = 0
    End Sub

    ' Apply multi-column filter
    Private Sub ColumnFilterChanged(sender As Object, e As EventArgs)
        If currentDataTable Is Nothing Then Return
        Dim filterParts As New List(Of String)
        For Each kvp In columnFilters
            Dim col = kvp.Key
            Dim cb = kvp.Value
            If cb.SelectedIndex > 0 Then
                filterParts.Add($"[{col}] = '{cb.SelectedItem.ToString().Replace("'", "''")}'")
            End If
        Next
        Dim dv As New DataView(currentDataTable)
        dv.RowFilter = String.Join(" AND ", filterParts)
        dgvCodeResults.DataSource = dv
    End Sub

    ' Context menu for column visibility
    Private Sub SetupColumnContextMenu()
        cmsColumns.Items.Clear()
        Dim showAllItem As New ToolStripMenuItem("Show All Columns")
        AddHandler showAllItem.Click, Sub()
                                          For Each col As DataGridViewColumn In dgvCodeResults.Columns
                                              col.Visible = True
                                          Next
                                          SetupColumnFilters()
                                      End Sub
        Dim hideAllItem As New ToolStripMenuItem("Hide All Columns")
        AddHandler hideAllItem.Click, Sub()
                                          For Each col As DataGridViewColumn In dgvCodeResults.Columns
                                              col.Visible = False
                                          Next
                                          SetupColumnFilters()
                                      End Sub
        cmsColumns.Items.Add(showAllItem)
        cmsColumns.Items.Add(hideAllItem)
        cmsColumns.Items.Add(New ToolStripSeparator())
        For Each col As DataGridViewColumn In dgvCodeResults.Columns
            Dim item As New ToolStripMenuItem(col.HeaderText) With {
                .Checked = col.Visible,
                .CheckOnClick = True,
                .Tag = col.Name
            }
            AddHandler item.CheckedChanged, AddressOf ColumnMenuItem_CheckedChanged
            cmsColumns.Items.Add(item)
        Next
    End Sub

    Private Sub ColumnMenuItem_CheckedChanged(sender As Object, e As EventArgs)
        Dim item = CType(sender, ToolStripMenuItem)
        Dim colName = item.Tag.ToString()
        If dgvCodeResults.Columns.Contains(colName) Then
            dgvCodeResults.Columns(colName).Visible = item.Checked
            SetupColumnFilters()
        End If
    End Sub

    Private Sub dgvCodeResults_MouseUp(sender As Object, e As MouseEventArgs) Handles dgvCodeResults.MouseUp
        If e.Button = MouseButtons.Right Then
            SetupColumnContextMenu()
            cmsColumns.Show(dgvCodeResults, e.Location)
        End If
    End Sub

    Private Async Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        currentPage += 1
        Await LoadPage()
    End Sub

    Private Async Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
        If currentPage > 0 Then
            currentPage -= 1
            Await LoadPage()
        End If
    End Sub

    Private Sub ApplyCustomColors()
        With dgvCodeResults
            .AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow
            .DefaultCellStyle.BackColor = Color.White
            .ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateBlue
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .EnableHeadersVisualStyles = False
        End With
    End Sub
End Class
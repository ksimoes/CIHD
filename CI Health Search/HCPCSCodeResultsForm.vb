Imports System.Data

Public Class HCPCSCodeResultsForm
    Private WithEvents cmsColumns As New ContextMenuStrip()
    Private currentDataTable As DataTable
    Private cmbFilterColumn As ComboBox
    Private txtFilterValue As TextBox
    Private btnApplyFilter As Button
    Private btnClearFilter As Button
    Private btnSelectAll As Button
    Private btnDeselectAll As Button
    Private clbFilterValues As CheckedListBox
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

        ' --- Add filter controls dynamically ---
        cmbFilterColumn = New ComboBox() With {
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Width = 200,
            .Location = New Point(10, 570)
        }
        txtFilterValue = New TextBox() With {
            .Width = 200,
            .Location = New Point(220, 570)
        }
        btnApplyFilter = New Button() With {
            .Text = "Filter",
            .Width = 80,
            .Location = New Point(430, 568)
        }
        btnClearFilter = New Button() With {
            .Text = "Clear",
            .Width = 80,
            .Location = New Point(520, 568)
        }
        clbFilterValues = New CheckedListBox() With {
            .CheckOnClick = True,
            .Width = 200,
            .Height = 150,
            .Location = New Point(10, 600)
        }
        btnSelectAll = New Button() With {
            .Text = "Select All",
            .Width = 95,
            .Location = New Point(220, 600)
        }
        btnDeselectAll = New Button() With {
            .Text = "Deselect All",
            .Width = 95,
            .Location = New Point(320, 600)
        }

        Me.Controls.Add(cmbFilterColumn)
        Me.Controls.Add(txtFilterValue)
        Me.Controls.Add(btnApplyFilter)
        Me.Controls.Add(btnClearFilter)
        Me.Controls.Add(clbFilterValues)
        Me.Controls.Add(btnSelectAll)
        Me.Controls.Add(btnDeselectAll)

        ' Wire up events
        AddHandler btnApplyFilter.Click, AddressOf btnApplyFilter_Click
        AddHandler btnClearFilter.Click, AddressOf btnClearFilter_Click
        AddHandler cmbFilterColumn.SelectedIndexChanged, AddressOf cmbFilterColumn_SelectedIndexChanged
        AddHandler btnSelectAll.Click, AddressOf btnSelectAll_Click
        AddHandler btnDeselectAll.Click, AddressOf btnDeselectAll_Click
    End Sub

    Private Sub cmbFilterColumn_SelectedIndexChanged(sender As Object, e As EventArgs)
        clbFilterValues.Items.Clear()
        If currentDataTable Is Nothing OrElse cmbFilterColumn.SelectedItem Is Nothing Then Return
        Dim colName = cmbFilterColumn.SelectedItem.ToString()
        Dim uniqueValues = currentDataTable.AsEnumerable().
        Select(Function(r) r(colName)?.ToString()).
        Where(Function(v) Not String.IsNullOrEmpty(v)).
        Distinct().
        OrderBy(Function(v) v).
        ToList()
        For Each value In uniqueValues
            clbFilterValues.Items.Add(value, True) ' Checked by default (include all)
        Next
    End Sub

    Private Sub btnApplyFilter_Click(sender As Object, e As EventArgs)
        If currentDataTable Is Nothing OrElse cmbFilterColumn.SelectedItem Is Nothing Then Return
        Dim colName = cmbFilterColumn.SelectedItem.ToString()
        Dim selectedValues = clbFilterValues.CheckedItems.Cast(Of String)().ToList()
        Dim filterValue = txtFilterValue.Text.Trim().Replace("'", "''")

        Dim dv As New DataView(currentDataTable)

        ' If user typed a filter value, combine with checked values
        If selectedValues.Count > 0 AndAlso filterValue <> "" Then
            Dim filterParts = selectedValues.Select(Function(val) $"([{colName}] = '{val.Replace("'", "''")}' AND [{colName}] LIKE '%{filterValue}%')")
            dv.RowFilter = String.Join(" OR ", filterParts)
        ElseIf selectedValues.Count > 0 Then
            Dim filterParts = selectedValues.Select(Function(val) $"[{colName}] = '{val.Replace("'", "''")}'")
            dv.RowFilter = String.Join(" OR ", filterParts)
        ElseIf filterValue <> "" Then
            dv.RowFilter = $"[{colName}] LIKE '%{filterValue}%'"
        Else
            dgvCodeResults.DataSource = currentDataTable.Clone() ' Show empty if nothing selected
            Return
        End If

        dgvCodeResults.DataSource = dv
    End Sub

    Private Sub btnClearFilter_Click(sender As Object, e As EventArgs)
        If currentDataTable IsNot Nothing Then
            dgvCodeResults.DataSource = currentDataTable
            txtFilterValue.Clear()
            For i = 0 To clbFilterValues.Items.Count - 1
                clbFilterValues.SetItemChecked(i, True)
            Next
        End If
    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs)
        For i = 0 To clbFilterValues.Items.Count - 1
            clbFilterValues.SetItemChecked(i, True)
        Next
    End Sub

    Private Sub btnDeselectAll_Click(sender As Object, e As EventArgs)
        For i = 0 To clbFilterValues.Items.Count - 1
            clbFilterValues.SetItemChecked(i, False)
        Next
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

        ' Populate filter column ComboBox
        cmbFilterColumn.Items.Clear()
        For Each col As DataColumn In dt.Columns
            cmbFilterColumn.Items.Add(col.ColumnName)
        Next
        If cmbFilterColumn.Items.Count > 0 Then cmbFilterColumn.SelectedIndex = 0
    End Function

    ' Context menu for column visibility
    Private Sub SetupColumnContextMenu()
        cmsColumns.Items.Clear()
        Dim showAllItem As New ToolStripMenuItem("Show All Columns")
        AddHandler showAllItem.Click, Sub()
                                          For Each col As DataGridViewColumn In dgvCodeResults.Columns
                                              col.Visible = True
                                          Next
                                      End Sub
        Dim hideAllItem As New ToolStripMenuItem("Hide All Columns")
        AddHandler hideAllItem.Click, Sub()
                                          For Each col As DataGridViewColumn In dgvCodeResults.Columns
                                              col.Visible = False
                                          Next
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
Imports System.Data

Public Class HCPCSCodeResultsForm
    Private WithEvents cmsColumns As New ContextMenuStrip()
    Private currentDataTable As DataTable
    Private filterPanel As Panel
    Private columnDropdowns As New Dictionary(Of String, Button)
    Private dropdownControls As New Dictionary(Of String, MultiSelectDropdown)
    Private openDropdownHosts As New Dictionary(Of String, DropdownHost2)
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

    ' Multi-select, searchable dropdown filter panel
    Private Sub SetupColumnFilters()
        filterPanel.Controls.Clear()
        columnDropdowns.Clear()
        dropdownControls.Clear()
        openDropdownHosts.Clear()
        If currentDataTable Is Nothing OrElse dgvCodeResults.Columns.Count = 0 Then Return

        For Each col As DataGridViewColumn In dgvCodeResults.Columns
            If Not col.Visible Then Continue For
            Dim btn As New Button() With {
                .Name = "btnFilter_" & col.Name,
                .Width = col.Width,
                .Left = dgvCodeResults.GetCellDisplayRectangle(col.Index, -1, True).Left,
                .Top = 0,
                .Text = "(All)",
                .Tag = col.Name,
                .Height = 24
            }
            Dim uniqueVals = currentDataTable.AsEnumerable().
                Select(Function(r) r(col.Name)?.ToString()).
                Where(Function(v) Not String.IsNullOrEmpty(v)).
                Distinct().
                OrderBy(Function(v) v).
                ToList()
            Dim dropdown = New MultiSelectDropdown(uniqueVals)
            dropdown.Visible = False
            AddHandler dropdown.SelectionChanged, Sub()
                                                      btn.Text = If(dropdown.SelectedValues.Count = 0, "(All)", String.Join(", ", dropdown.SelectedValues.Take(2)) & If(dropdown.SelectedValues.Count > 2, " ...", ""))
                                                      ApplyMultiColumnFilter()
                                                  End Sub
            filterPanel.Controls.Add(btn)
            columnDropdowns(col.Name) = btn
            dropdownControls(col.Name) = dropdown

            AddHandler btn.Click, Sub(senderBtn, eBtn)
                                      Dim colName = col.Name
                                      ' Toggle: close if open, open if closed
                                      If openDropdownHosts.ContainsKey(colName) Then
                                          openDropdownHosts(colName).Close()
                                          openDropdownHosts.Remove(colName)
                                          Return
                                      End If
                                      ' Close any other open dropdowns
                                      For Each h In openDropdownHosts.Values.ToList()
                                          h.Close()
                                      Next
                                      openDropdownHosts.Clear()
                                      Dim host As DropdownHost2 = Nothing
                                      host = New DropdownHost2(dropdown)
                                      AddHandler host.Closed, Sub()
                                                                  If openDropdownHosts.ContainsKey(colName) Then
                                                                      openDropdownHosts.Remove(colName)
                                                                  End If
                                                              End Sub
                                      openDropdownHosts(colName) = host
                                      Dim btnScreen = btn.PointToScreen(New Point(0, btn.Height))
                                      host.Show(btnScreen)
                                  End Sub
        Next
        AddHandler dgvCodeResults.ColumnWidthChanged, AddressOf dgvCodeResults_ColumnWidthChanged
        AddHandler dgvCodeResults.Scroll, AddressOf dgvCodeResults_Scroll
        PositionFilterCombos()
    End Sub

    ' Keep filter buttons aligned with columns
    Private Sub PositionFilterCombos()
        For Each col As DataGridViewColumn In dgvCodeResults.Columns
            If columnDropdowns.ContainsKey(col.Name) Then
                Dim btn = columnDropdowns(col.Name)
                Dim rect = dgvCodeResults.GetCellDisplayRectangle(col.Index, -1, True)
                btn.Left = rect.Left
                btn.Width = rect.Width
            End If
        Next
    End Sub

    Private Sub dgvCodeResults_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs)
        PositionFilterCombos()
    End Sub

    Private Sub dgvCodeResults_Scroll(sender As Object, e As ScrollEventArgs)
        PositionFilterCombos()
    End Sub

    ' Apply multi-column filter
    Private Sub ApplyMultiColumnFilter()
        If currentDataTable Is Nothing Then Return
        Dim filterParts As New List(Of String)
        For Each kvp In dropdownControls
            Dim col = kvp.Key
            Dim dd = kvp.Value
            If dd.SelectedValues.Count > 0 Then
                Dim orParts = dd.SelectedValues.Select(Function(val) $"[{col}] = '{val.Replace("'", "''")}'")
                filterParts.Add("(" & String.Join(" OR ", orParts) & ")")
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

' --- MultiSelectDropdown and DropdownHost classes (copy from Profile.vb) ---

Public Class MultiSelectDropdown2
    Inherits Panel

    Public Event SelectionChanged()
    Private WithEvents txtSearch As New TextBox() With {.Dock = DockStyle.Top, .PlaceholderText = "Search..."}
    Private WithEvents clb As New CheckedListBox() With {.Dock = DockStyle.Fill, .CheckOnClick = True}
    Private allItems As List(Of String)

    Public Sub New(items As IEnumerable(Of String))
        Me.Height = 200
        Me.Width = 200
        Me.BorderStyle = BorderStyle.FixedSingle
        Me.Controls.Add(clb)
        Me.Controls.Add(txtSearch)
        allItems = items.Distinct().OrderBy(Function(x) x).ToList()
        clb.Items.AddRange(allItems.ToArray())
    End Sub

    Public ReadOnly Property SelectedValues As List(Of String)
        Get
            Return clb.CheckedItems.Cast(Of String)().ToList()
        End Get
    End Property

    Public Sub SetChecked(values As IEnumerable(Of String))
        For i = 0 To clb.Items.Count - 1
            clb.SetItemChecked(i, values.Contains(clb.Items(i).ToString()))
        Next
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Dim filter = txtSearch.Text.Trim().ToLower()
        clb.Items.Clear()
        For Each item In allItems
            If item.ToLower().Contains(filter) Then
                clb.Items.Add(item, False)
            End If
        Next
    End Sub

    Private Sub clb_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clb.ItemCheck
        ' Raise after the check state changes
        Me.BeginInvoke(Sub() RaiseEvent SelectionChanged())
    End Sub
End Class

Public Class DropdownHost2
    Inherits ToolStripDropDown

    Public Sub New(content As Control)
        MyBase.New()
        content.Dock = DockStyle.Fill
        Dim host = New ToolStripControlHost(content)
        host.Margin = Padding.Empty
        host.Padding = Padding.Empty
        host.AutoSize = False
        Me.Padding = Padding.Empty
        Me.Items.Add(host)
        Me.AutoClose = True
        Me.Width = content.Width
        Me.Height = content.Height
    End Sub
End Class
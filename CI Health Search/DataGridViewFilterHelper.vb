Imports System.Windows.Forms
Imports System.Data

Public Class DataGridViewFilterHelper
    Private _filterPanel As Panel
    Private columnDropdowns As New Dictionary(Of String, Button)
    Private dropdownControls As New Dictionary(Of String, MultiSelectDropdown)
    Private openDropdownHosts As New Dictionary(Of String, ToolStripDropDown)
    Private dgv As DataGridView
    Private dt As DataTable


    Public Sub New(targetDGV As DataGridView, parentForm As Form)
        dgv = targetDGV
        dt = TryCast(dgv.DataSource, DataTable)
        If dt Is Nothing Then Return

        _filterPanel = New Panel() With {
            .Height = 30,
            .Dock = DockStyle.Top
        }
        parentForm.Controls.Add(filterPanel)
        parentForm.Controls.SetChildIndex(filterPanel, 0)

        SetupColumnFilters()
        AddHandler dgv.ColumnWidthChanged, AddressOf PositionFilterCombos
        AddHandler dgv.Scroll, AddressOf PositionFilterCombos
    End Sub

    Private Sub SetupColumnFilters()
        filterPanel.Controls.Clear()
        columnDropdowns.Clear()
        dropdownControls.Clear()
        openDropdownHosts.Clear()
        If dt Is Nothing OrElse dgv.Columns.Count = 0 Then Return

        For Each col As DataGridViewColumn In dgv.Columns
            If Not col.Visible Then Continue For
            Dim btn As New Button() With {
                .Name = "btnFilter_" & col.Name,
                .Width = col.Width,
                .Left = dgv.GetCellDisplayRectangle(col.Index, -1, True).Left,
                .Top = 0,
                .Text = "(All)",
                .Tag = col.Name,
                .Height = 24
            }
            Dim uniqueVals = dt.AsEnumerable().
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
                                      If openDropdownHosts.ContainsKey(colName) Then
                                          openDropdownHosts(colName).Close()
                                          openDropdownHosts.Remove(colName)
                                          Return
                                      End If
                                      For Each h In openDropdownHosts.Values.ToList()
                                          h.Close()
                                      Next
                                      openDropdownHosts.Clear()
                                      Dim host As New DropdownHost(dropdown)
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
        PositionFilterCombos()
    End Sub





    Private Sub PositionFilterCombos(Optional sender As Object = Nothing, Optional e As EventArgs = Nothing)
        For Each col As DataGridViewColumn In dgv.Columns
            If columnDropdowns.ContainsKey(col.Name) Then
                Dim btn = columnDropdowns(col.Name)
                Dim rect = dgv.GetCellDisplayRectangle(col.Index, -1, True)
                btn.Left = rect.Left
                btn.Width = rect.Width
            End If
        Next
    End Sub



    Private Sub ApplyMultiColumnFilter()
        If dt Is Nothing Then Return
        Dim filterParts As New List(Of String)
        For Each kvp In dropdownControls
            Dim col = kvp.Key
            Dim dd = kvp.Value
            If dd.SelectedValues.Count > 0 Then
                Dim orParts = dd.SelectedValues.Select(Function(val) $"[{col}] = '{val.Replace("'", "''")}'")
                filterParts.Add("(" & String.Join(" OR ", orParts) & ")")
            End If
        Next
        Dim dv As New DataView(dt)
        dv.RowFilter = String.Join(" AND ", filterParts)
        dgv.DataSource = dv
    End Sub

    Public Sub New(targetDGV As DataGridView, parentForm As Form, Optional filterPanelWidth As Integer = -1)
        dgv = targetDGV
        dt = TryCast(dgv.DataSource, DataTable)
        If dt Is Nothing Then Return

        _filterPanel = New Panel() With {
        .Height = 30,
        .Dock = DockStyle.Top
    }
        If filterPanelWidth > 0 Then
            _filterPanel.Dock = DockStyle.None
            _filterPanel.Width = filterPanelWidth
            _filterPanel.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        End If

        parentForm.Controls.Add(_filterPanel)
        parentForm.Controls.SetChildIndex(_filterPanel, 0)

        SetupColumnFilters()
        AddHandler dgv.ColumnWidthChanged, AddressOf PositionFilterCombos
        AddHandler dgv.Scroll, AddressOf PositionFilterCombos
    End Sub

    Public ReadOnly Property FilterPanel As Panel
        Get
            Return _filterPanel

        End Get
    End Property
End Class
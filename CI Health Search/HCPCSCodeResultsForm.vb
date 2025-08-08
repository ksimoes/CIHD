Imports System.Data

Public Class HCPCSCodeResultsForm
    Private WithEvents cmsColumns As New ContextMenuStrip()
    Private currentPage As Integer = 0
    Private pageSize As Integer = 100
    Private code As String
    Private state As String

    ' Constructor for paged results
    Public Sub New(hcpcsCode As String, stateFilter As String)
        InitializeComponent()
        code = hcpcsCode
        state = stateFilter
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
        dgvCodeResults.DataSource = dt
        lblStatus.Text = $"Page {currentPage + 1} (showing {dt.Rows.Count} results)"
        btnPrevPage.Enabled = currentPage > 0
        btnNextPage.Enabled = dt.Rows.Count = pageSize
    End Function

    Private Sub SetupColumnContextMenu()
        cmsColumns.Items.Clear()

        ' Add "Show All" and "Hide All" options
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

        ' Add a menu item for each column
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

    ' Show context menu on right-click
    Private Sub dgvCodeResults_MouseUp(sender As Object, e As MouseEventArgs) Handles dgvCodeResults.MouseUp
        If e.Button = MouseButtons.Right Then
            SetupColumnContextMenu()
            cmsColumns.Show(dgvCodeResults, e.Location)
        End If
    End Sub

    ' Next page button
    Private Async Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        currentPage += 1
        Await LoadPage()
    End Sub

    ' Previous page button
    Private Async Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
        If currentPage > 0 Then
            currentPage -= 1
            Await LoadPage()
        End If
    End Sub

    ' Optional: Apply custom DataGridView colors
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

    ' (Optional) Call ApplyCustomColors in the constructor or after InitializeComponent if desired
End Class
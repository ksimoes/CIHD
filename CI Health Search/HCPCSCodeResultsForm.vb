Imports System.Data

Public Class HCPCSCodeResultsForm
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
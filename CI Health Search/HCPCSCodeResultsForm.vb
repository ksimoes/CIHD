Imports System.Data

Public Class HCPCSCodeResultsForm
    Public Sub New(dt As DataTable, code As String)
        InitializeComponent()
        Me.Text = $"Providers for HCPCS Code: {code}"
        dgvCodeResults.DataSource = dt
        'StyleDataGridView()
        ApplyCustomColors()
    End Sub

    ''Private Sub StyleDataGridView()
    'With dgvCodeResults
    '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    '.ReadOnly = True
    '.AllowUserToAddRows = False
    '.AllowUserToDeleteRows = False
    '.DefaultCellStyle.Font = New Font("Segoe UI", 10)
    '.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
    '.DefaultCellStyle.Padding = New Padding(4, 2, 4, 2)
    '.RowTemplate.Height = 28
    '.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke
    '.DefaultCellStyle.BackColor = Color.White
    '.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue
    '.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
    '.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue
    '.DefaultCellStyle.SelectionForeColor = Color.Black
    '.EnableHeadersVisualStyles = False
    '.RowHeadersVisible = False
    'End With
    'End Sub '
    Private Sub ApplyCustomColors()
        With dgvCodeResults
            .AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow   ' Alternating rows
            .DefaultCellStyle.BackColor = Color.White                        ' Main rows
            .ColumnHeadersDefaultCellStyle.BackColor = Color.DarkSlateBlue   ' Header background
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White           ' Header text
            .DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue        ' Selected row
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .EnableHeadersVisualStyles = False
        End With
    End Sub
End Class
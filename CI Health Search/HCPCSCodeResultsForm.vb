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

    Private Const Level1Desc As String = "HCPCS Level 1: Description of the HCPCS code for the specific medical service furnished by the provider. HCPCS descriptions associated with CPT codes are consumer friendly descriptions provided by the AMA. CPT Consumer Friendly Descriptors are lay synonyms for CPT descriptors that are intended to help healthcare consumers who are not medical professionals understand clinical procedures on bills and patient portals. CPT Consumer Friendly Descriptors should not be used for clinical coding or documentation. "
    Private Const Level2Desc As String = "HCPCS Level 2: All other descriptions are CMS Level II descriptions provided in long form. Due to variable length restrictions, the CMS Level II descriptions have been truncated to 256 bytes. As a result, the same HCPCS description can be associated with more than one HCPCS code. "


    ' When showing Level 1 table:
    Private Sub ShowLevel1Table()
        lblHCPCSDescription.Text = Level1Desc
        lblHCPCSDescription.Visible = True
        linkMoreInfo.Visible = True
    End Sub

    Private Sub ShowLevel2Table()
        lblHCPCSDescription.Text = Level2Desc
        lblHCPCSDescription.Visible = True
        linkMoreInfo.Visible = True
    End Sub

    Private Sub HideTables()
        lblHCPCSDescription.Visible = False
        linkMoreInfo.Visible = False
    End Sub

    Private currentLevel As Integer = 1 ' 1 for Level 1, 2 for Level 2

    ' When you load Level 1 data:


End Class
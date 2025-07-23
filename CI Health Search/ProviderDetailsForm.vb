Imports System.Windows.Forms

Public Class ProviderDetailsForm
    Public WithEvents dgvDetails As New DataGridView()

    Public Sub New()
        ' Initialize the form and DataGridView
        Me.Text = "Provider Details"
        Me.Size = New Size(900, 500)
        Me.StartPosition = FormStartPosition.CenterParent

        dgvDetails.Dock = DockStyle.Fill
        dgvDetails.ReadOnly = True
        dgvDetails.AllowUserToAddRows = False
        dgvDetails.AllowUserToDeleteRows = False
        dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender
        dgvDetails.DefaultCellStyle.BackColor = Color.White
        dgvDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSlateBlue
        dgvDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDetails.EnableHeadersVisualStyles = False
        dgvDetails.GridColor = Color.LightSteelBlue
        dgvDetails.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue
        dgvDetails.DefaultCellStyle.SelectionForeColor = Color.Black

        Me.Controls.Add(dgvDetails)
    End Sub
End Class
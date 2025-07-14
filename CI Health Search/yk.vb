Imports System.Windows.Forms
Imports System.Drawing

Public Class yk
    Private dgvInfo As DataGridView

    Private Sub yk_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Create and configure DataGridView
        dgvInfo = New DataGridView With {
            .Location = New Point(20, 20),
            .Size = New Size(600, 400),
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .AllowUserToResizeRows = False,
            .AllowUserToResizeColumns = False,
            .ReadOnly = True,
            .RowHeadersVisible = False,
            .ColumnHeadersVisible = True,
            .CellBorderStyle = DataGridViewCellBorderStyle.Single,
            .GridColor = Color.Black,
            .AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle With {.BackColor = Color.LightGray},
            .Font = New Font("Segoe UI", 10)
        }

        ' Add columns
        dgvInfo.Columns.Add("Metric", "Metric")
        dgvInfo.Columns.Add("Value", "Value")

        ' Add sample rows (replace with your real data)
        dgvInfo.Rows.Add("Total Assets", "1,234,567")
        dgvInfo.Rows.Add("Net Income", "123,456")
        dgvInfo.Rows.Add("Operating Margin", "12.5%")
        dgvInfo.Rows.Add("Current Ratio", "2.1")
        dgvInfo.Rows.Add("Quick Ratio", "1.8")
        dgvInfo.Rows.Add("Debt to Net Assets", "0.45")
        dgvInfo.Rows.Add("Days Cash on Hand", "45")

        ' Optional: Style header
        dgvInfo.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvInfo.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue
        dgvInfo.EnableHeadersVisualStyles = False

        ' Add to form
        Me.Controls.Add(dgvInfo)
    End Sub


End Class
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

    Public Sub SetFriendlyColumnHeaders()
        ' Map API column names to user-friendly names
        Dim headerMap As New Dictionary(Of String, String) From {
            {"Prscrbr_NPI", "NPI"},
            {"Prscrbr_Last_Org_Name", "Last Name"},
            {"Prscrbr_First_Name", "First Name"},
            {"Prscrbr_Cred", "Credentials"},
            {"Prscrbr_State_Abrvtn", "State"},
            {"Prscrbr_City", "City"},
            {"Prscrbr_Zip", "ZIP"},
            {"Prscrbr_Type", "Provider Type"},
            {"Prscrbr_Type_Src", "Prescriber Type"},
            {"Brnd_Name", "Brand Name"},
            {"Gnrc_Name", "Generic Name"},
            {"Tot_Clms", "Total Claims"},
            {"Tot_30day_Fills", "Total 30-Day Fills"},
            {"Tot_Day_Suply", "Total Day Supply"},
            {"Tot_Drug_Cst", "Total Drug Cost"}}

        'Add more mappings as needed


        For Each col As DataGridViewColumn In dgvDetails.Columns
            If headerMap.ContainsKey(col.Name) Then
                col.HeaderText = headerMap(col.Name)
            End If
        Next
    End Sub
End Class
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HCPCSCodeResultsForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        dgvCodeResults = New DataGridView()
        lblHCPCSDescription = New Label()
        linkMoreInfo = New LinkLabel()
        btnPrevPage = New Button()
        btnNextPage = New Button()
        lblStatus = New Label()
        CType(dgvCodeResults, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvCodeResults
        ' 
        dgvCodeResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCodeResults.Dock = DockStyle.Fill
        dgvCodeResults.Location = New Point(0, 0)
        dgvCodeResults.Name = "dgvCodeResults"
        dgvCodeResults.Size = New Size(1448, 697)
        dgvCodeResults.TabIndex = 0
        ' 
        ' lblHCPCSDescription
        ' 
        lblHCPCSDescription.AutoSize = True
        lblHCPCSDescription.Font = New Font("Segoe UI", 9.75F, FontStyle.Italic)
        lblHCPCSDescription.ForeColor = Color.DimGray
        lblHCPCSDescription.Location = New Point(12, 320)
        lblHCPCSDescription.Name = "lblHCPCSDescription"
        lblHCPCSDescription.Size = New Size(0, 17)
        lblHCPCSDescription.TabIndex = 2
        lblHCPCSDescription.Visible = False
        ' 
        ' linkMoreInfo
        ' 
        linkMoreInfo.AutoSize = True
        linkMoreInfo.Font = New Font("Segoe UI", 9.75F)
        linkMoreInfo.Location = New Point(12, 350)
        linkMoreInfo.Name = "linkMoreInfo"
        linkMoreInfo.Size = New Size(121, 17)
        linkMoreInfo.TabIndex = 3
        linkMoreInfo.TabStop = True
        linkMoreInfo.Text = "More about HCPCS"
        linkMoreInfo.Visible = False
        ' 
        ' btnPrevPage
        ' 
        btnPrevPage.Location = New Point(1197, 674)
        btnPrevPage.Name = "btnPrevPage"
        btnPrevPage.Size = New Size(121, 23)
        btnPrevPage.TabIndex = 4
        btnPrevPage.Text = "Previous"
        btnPrevPage.UseVisualStyleBackColor = True
        ' 
        ' btnNextPage
        ' 
        btnNextPage.Location = New Point(1324, 674)
        btnNextPage.Name = "btnNextPage"
        btnNextPage.Size = New Size(121, 23)
        btnNextPage.TabIndex = 5
        btnNextPage.Text = "Next"
        btnNextPage.UseVisualStyleBackColor = True
        ' 
        ' lblStatus
        ' 
        lblStatus.AutoSize = True
        lblStatus.Location = New Point(656, 678)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(39, 15)
        lblStatus.TabIndex = 6
        lblStatus.Text = "Status"
        ' 
        ' HCPCSCodeResultsForm
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1448, 697)
        Controls.Add(lblStatus)
        Controls.Add(btnNextPage)
        Controls.Add(btnPrevPage)
        Controls.Add(dgvCodeResults)
        Controls.Add(lblHCPCSDescription)
        Controls.Add(linkMoreInfo)
        Name = "HCPCSCodeResultsForm"
        Text = "HCPCS Code Results"
        CType(dgvCodeResults, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents dgvCodeResults As DataGridView
    Friend WithEvents lblHCPCSDescription As System.Windows.Forms.Label
    Friend WithEvents linkMoreInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents btnPrevPage As Button
    Friend WithEvents btnNextPage As Button
    Friend WithEvents lblStatus As Label
End Class
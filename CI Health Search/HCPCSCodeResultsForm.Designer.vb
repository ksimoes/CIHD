<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HCPCSCodeResultsForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        dgvCodeResults = New DataGridView()
        lblHCPCSDescription = New Label()
        linkMoreInfo = New LinkLabel()
        CType(dgvCodeResults, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvCodeResults
        ' 
        dgvCodeResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCodeResults.Dock = DockStyle.Fill
        dgvCodeResults.Location = New Point(0, 0)
        dgvCodeResults.Name = "dgvCodeResults"
        dgvCodeResults.Size = New Size(1341, 587)
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
        ' HCPCSCodeResultsForm
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1341, 587)
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
End Class
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HCPCSCodeResultsForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        dgvCodeResults = New DataGridView()
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
        ' HCPCSCodeResultsForm
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1341, 587)
        Controls.Add(dgvCodeResults)
        Name = "HCPCSCodeResultsForm"
        Text = "HCPCS Code Results"
        CType(dgvCodeResults, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvCodeResults As DataGridView
End Class
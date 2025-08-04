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

        Me.lblHCPCSDescription = New System.Windows.Forms.Label()
        '
        ' lblHCPCSDescription
        '
        Me.lblHCPCSDescription.AutoSize = True
        Me.lblHCPCSDescription.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point)
        Me.lblHCPCSDescription.Location = New System.Drawing.Point(12, 320) ' Adjust as needed
        Me.lblHCPCSDescription.Name = "lblHCPCSDescription"
        Me.lblHCPCSDescription.Size = New System.Drawing.Size(0, 17)
        Me.lblHCPCSDescription.TabIndex = 2
        Me.lblHCPCSDescription.Visible = False
        Me.lblHCPCSDescription.ForeColor = System.Drawing.Color.DimGray

        ' Add the label to the form's Controls collection
        Me.Controls.Add(Me.lblHCPCSDescription)

        Me.linkMoreInfo = New System.Windows.Forms.LinkLabel()
        '
        ' linkMoreInfo
        '
        Me.linkMoreInfo.AutoSize = True
        Me.linkMoreInfo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        Me.linkMoreInfo.Location = New System.Drawing.Point(12, 350) ' Adjust as needed
        Me.linkMoreInfo.Name = "linkMoreInfo"
        Me.linkMoreInfo.Size = New System.Drawing.Size(120, 17)
        Me.linkMoreInfo.TabIndex = 3
        Me.linkMoreInfo.TabStop = True
        Me.linkMoreInfo.Text = "More about HCPCS"
        Me.linkMoreInfo.Visible = False

        ' Add to Controls
        Me.Controls.Add(Me.linkMoreInfo)
    End Sub

    Friend WithEvents dgvCodeResults As DataGridView
    Friend WithEvents lblHCPCSDescription As System.Windows.Forms.Label
    Friend WithEvents linkMoreInfo As System.Windows.Forms.LinkLabel
End Class
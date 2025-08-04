<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ProviderDetailsForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        dgvDetails = New DataGridView()
        CType(dgvDetails, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvDetails
        ' 
        dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDetails.Location = New Point(40, 98)
        dgvDetails.Name = "dgvDetails"
        dgvDetails.Size = New Size(716, 322)
        dgvDetails.TabIndex = 0
        ' 
        ' ProviderDetailsForm
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1088, 633)
        Controls.Add(dgvDetails)
        Name = "ProviderDetailsForm"
        Text = "ProviderDetailsForm"
        CType(dgvDetails, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        ' ...

        ' 
        ' lblHCPCSDescription
        '
        Me.lblHCPCSDescription = New System.Windows.Forms.Label()
        Me.lblHCPCSDescription.AutoSize = True
        Me.lblHCPCSDescription.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Italic)
        Me.lblHCPCSDescription.Location = New System.Drawing.Point(20, 60) ' Adjust as needed
        Me.lblHCPCSDescription.Name = "lblHCPCSDescription"
        Me.lblHCPCSDescription.Size = New System.Drawing.Size(0, 17)
        Me.lblHCPCSDescription.TabIndex = 100
        Me.lblHCPCSDescription.Visible = False
        Me.lblHCPCSDescription.ForeColor = System.Drawing.Color.DimGray
        Me.Controls.Add(Me.lblHCPCSDescription)

        '
        ' linkMoreInfo
        '
        Me.linkMoreInfo = New System.Windows.Forms.LinkLabel()
        Me.linkMoreInfo.AutoSize = True
        Me.linkMoreInfo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.linkMoreInfo.Location = New System.Drawing.Point(20, 80) ' Adjust as needed
        Me.linkMoreInfo.Name = "linkMoreInfo"
        Me.linkMoreInfo.Size = New System.Drawing.Size(120, 17)
        Me.linkMoreInfo.TabIndex = 101
        Me.linkMoreInfo.TabStop = True
        Me.linkMoreInfo.Text = "More about HCPCS"
        Me.linkMoreInfo.Visible = False
        Me.Controls.Add(Me.linkMoreInfo)
    End Sub

    Friend WithEvents dgvDetails As DataGridView
    Friend WithEvents lblHCPCSDescription As System.Windows.Forms.Label
    Friend WithEvents linkMoreInfo As System.Windows.Forms.LinkLabel
End Class
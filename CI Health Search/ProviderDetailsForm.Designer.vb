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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProviderDetailsForm))
        dgvDetails = New DataGridView()
        lblHCPCSDescription = New Label()
        linkMoreInfo = New LinkLabel()
        picLoad = New PictureBox()
        CType(dgvDetails, ComponentModel.ISupportInitialize).BeginInit()
        CType(picLoad, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvDetails
        ' 
        dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDetails.Location = New Point(40, 98)
        dgvDetails.Name = "dgvDetails"
        dgvDetails.Size = New Size(716, 322)
        dgvDetails.TabIndex = 0
        ' 
        ' lblHCPCSDescription
        ' 
        lblHCPCSDescription.AutoSize = True
        lblHCPCSDescription.Font = New Font("Segoe UI", 9.75F, FontStyle.Italic)
        lblHCPCSDescription.ForeColor = Color.DimGray
        lblHCPCSDescription.Location = New Point(20, 60)
        lblHCPCSDescription.Name = "lblHCPCSDescription"
        lblHCPCSDescription.Size = New Size(0, 17)
        lblHCPCSDescription.TabIndex = 100
        lblHCPCSDescription.Visible = False
        ' 
        ' linkMoreInfo
        ' 
        linkMoreInfo.AutoSize = True
        linkMoreInfo.Font = New Font("Segoe UI", 9.75F)
        linkMoreInfo.Location = New Point(40, 78)
        linkMoreInfo.Name = "linkMoreInfo"
        linkMoreInfo.Size = New Size(121, 17)
        linkMoreInfo.TabIndex = 101
        linkMoreInfo.TabStop = True
        linkMoreInfo.Text = "More about HCPCS"
        linkMoreInfo.Visible = False
        ' 
        ' picLoad
        ' 
        picLoad.BackColor = SystemColors.ControlDark
        picLoad.Image = CType(resources.GetObject("picLoad.Image"), Image)
        picLoad.Location = New Point(285, 154)
        picLoad.Name = "picLoad"
        picLoad.Size = New Size(198, 197)
        picLoad.TabIndex = 102
        picLoad.TabStop = False
        picLoad.Visible = False
        ' 
        ' ProviderDetailsForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1088, 633)
        Controls.Add(picLoad)
        Controls.Add(dgvDetails)
        Controls.Add(lblHCPCSDescription)
        Controls.Add(linkMoreInfo)
        Name = "ProviderDetailsForm"
        Text = "ProviderDetailsForm"
        CType(dgvDetails, ComponentModel.ISupportInitialize).EndInit()
        CType(picLoad, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents dgvDetails As DataGridView
    Friend WithEvents lblHCPCSDescription As System.Windows.Forms.Label
    Friend WithEvents linkMoreInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents picLoad As PictureBox
End Class
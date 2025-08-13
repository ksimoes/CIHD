<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HCPCSCodeResultsForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.dgvCodeResults = New System.Windows.Forms.DataGridView()
        Me.lblHCPCSDescription = New System.Windows.Forms.Label()
        Me.linkMoreInfo = New System.Windows.Forms.LinkLabel()
        Me.btnPrevPage = New System.Windows.Forms.Button()
        Me.btnNextPage = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.txtPageNumber = New System.Windows.Forms.TextBox()
        Me.btnGoToPage = New System.Windows.Forms.Button()
        CType(Me.dgvCodeResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' dgvCodeResults
        '
        Me.dgvCodeResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCodeResults.Dock = System.Windows.Forms.DockStyle.None
        Me.dgvCodeResults.Location = New System.Drawing.Point(0, 44)
        Me.dgvCodeResults.Name = "dgvCodeResults"
        Me.dgvCodeResults.Size = New System.Drawing.Size(1448, 553)
        Me.dgvCodeResults.TabIndex = 0
        '
        ' lblHCPCSDescription
        '
        Me.lblHCPCSDescription.AutoSize = True
        Me.lblHCPCSDescription.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Italic)
        Me.lblHCPCSDescription.ForeColor = System.Drawing.Color.DimGray
        Me.lblHCPCSDescription.Location = New System.Drawing.Point(12, 320)
        Me.lblHCPCSDescription.Name = "lblHCPCSDescription"
        Me.lblHCPCSDescription.Size = New System.Drawing.Size(0, 17)
        Me.lblHCPCSDescription.TabIndex = 2
        Me.lblHCPCSDescription.Visible = False
        '
        ' linkMoreInfo
        '
        Me.linkMoreInfo.AutoSize = True
        Me.linkMoreInfo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.linkMoreInfo.Location = New System.Drawing.Point(12, 350)
        Me.linkMoreInfo.Name = "linkMoreInfo"
        Me.linkMoreInfo.Size = New System.Drawing.Size(121, 17)
        Me.linkMoreInfo.TabIndex = 3
        Me.linkMoreInfo.TabStop = True
        Me.linkMoreInfo.Text = "More about HCPCS"
        Me.linkMoreInfo.Visible = False
        '
        ' btnPrevPage
        '
        Me.btnPrevPage.Location = New System.Drawing.Point(10, 610)
        Me.btnPrevPage.Name = "btnPrevPage"
        Me.btnPrevPage.Size = New System.Drawing.Size(90, 27)
        Me.btnPrevPage.TabIndex = 4
        Me.btnPrevPage.Text = "Previous"
        Me.btnPrevPage.UseVisualStyleBackColor = True
        '
        ' btnNextPage
        '
        Me.btnNextPage.Location = New System.Drawing.Point(110, 610)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(90, 27)
        Me.btnNextPage.TabIndex = 5
        Me.btnNextPage.Text = "Next"
        Me.btnNextPage.UseVisualStyleBackColor = True
        '
        ' lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(220, 616)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(39, 15)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.Text = "Status"
        '
        ' txtPageNumber
        '
        Me.txtPageNumber.Location = New System.Drawing.Point(420, 613)
        Me.txtPageNumber.Name = "txtPageNumber"
        Me.txtPageNumber.Size = New System.Drawing.Size(50, 23)
        Me.txtPageNumber.TabIndex = 7
        '
        ' btnGoToPage
        '
        Me.btnGoToPage.Location = New System.Drawing.Point(480, 610)
        Me.btnGoToPage.Name = "btnGoToPage"
        Me.btnGoToPage.Size = New System.Drawing.Size(50, 27)
        Me.btnGoToPage.TabIndex = 8
        Me.btnGoToPage.Text = "Go"
        Me.btnGoToPage.UseVisualStyleBackColor = True
        '
        ' HCPCSCodeResultsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1448, 650)
        Me.Controls.Add(Me.btnGoToPage)
        Me.Controls.Add(Me.txtPageNumber)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnNextPage)
        Me.Controls.Add(Me.btnPrevPage)
        Me.Controls.Add(Me.dgvCodeResults)
        Me.Controls.Add(Me.lblHCPCSDescription)
        Me.Controls.Add(Me.linkMoreInfo)
        Me.Name = "HCPCSCodeResultsForm"
        Me.Text = "HCPCS Code Results"
        CType(Me.dgvCodeResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents dgvCodeResults As System.Windows.Forms.DataGridView
    Friend WithEvents lblHCPCSDescription As System.Windows.Forms.Label
    Friend WithEvents linkMoreInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents btnPrevPage As System.Windows.Forms.Button
    Friend WithEvents btnNextPage As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents txtPageNumber As System.Windows.Forms.TextBox
    Friend WithEvents btnGoToPage As System.Windows.Forms.Button
End Class
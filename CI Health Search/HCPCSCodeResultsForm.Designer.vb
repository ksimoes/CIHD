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
        txtPageNumber = New TextBox()
        btnGoToPage = New Button()
        CType(dgvCodeResults, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvCodeResults
        ' 
        dgvCodeResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvCodeResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCodeResults.Location = New Point(0, 44)
        dgvCodeResults.Name = "dgvCodeResults"
        dgvCodeResults.Size = New Size(1448, 553)
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
        btnPrevPage.Location = New Point(10, 610)
        btnPrevPage.Name = "btnPrevPage"
        btnPrevPage.Size = New Size(90, 27)
        btnPrevPage.TabIndex = 4
        btnPrevPage.Text = "Previous"
        btnPrevPage.UseVisualStyleBackColor = True
        ' 
        ' btnNextPage
        ' 
        btnNextPage.Location = New Point(110, 610)
        btnNextPage.Name = "btnNextPage"
        btnNextPage.Size = New Size(90, 27)
        btnNextPage.TabIndex = 5
        btnNextPage.Text = "Next"
        btnNextPage.UseVisualStyleBackColor = True
        ' 
        ' lblStatus
        ' 
        lblStatus.AutoSize = True
        lblStatus.Location = New Point(220, 616)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(39, 15)
        lblStatus.TabIndex = 6
        lblStatus.Text = "Status"
        ' 
        ' txtPageNumber
        ' 
        txtPageNumber.Location = New Point(420, 613)
        txtPageNumber.Name = "txtPageNumber"
        txtPageNumber.Size = New Size(50, 23)
        txtPageNumber.TabIndex = 7
        ' 
        ' btnGoToPage
        ' 
        btnGoToPage.Location = New Point(480, 610)
        btnGoToPage.Name = "btnGoToPage"
        btnGoToPage.Size = New Size(50, 27)
        btnGoToPage.TabIndex = 8
        btnGoToPage.Text = "Go"
        btnGoToPage.UseVisualStyleBackColor = True
        ' 
        ' HCPCSCodeResultsForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1448, 650)
        Controls.Add(btnGoToPage)
        Controls.Add(txtPageNumber)
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

    Friend WithEvents dgvCodeResults As System.Windows.Forms.DataGridView
    Friend WithEvents lblHCPCSDescription As System.Windows.Forms.Label
    Friend WithEvents linkMoreInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents btnPrevPage As System.Windows.Forms.Button
    Friend WithEvents btnNextPage As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents txtPageNumber As System.Windows.Forms.TextBox
    Friend WithEvents btnGoToPage As System.Windows.Forms.Button
End Class
Imports System.Security.Policy

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HCPCSCodeResultsForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.tableLayoutMain = New System.Windows.Forms.TableLayoutPanel()
        Me.dgvCodeResults = New System.Windows.Forms.DataGridView()
        Me.lblHCPCSDescription = New System.Windows.Forms.Label()
        Me.linkMoreInfo = New System.Windows.Forms.LinkLabel()
        Me.btnPrevPage = New System.Windows.Forms.Button()
        Me.btnNextPage = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        CType(Me.dgvCodeResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tableLayoutMain.SuspendLayout()
        Me.SuspendLayout()
        '
        ' tableLayoutMain
        '
        Me.tableLayoutMain.ColumnCount = 1
        Me.tableLayoutMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tableLayoutMain.RowCount = 2
        Me.tableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!)) ' Filter panel height
        Me.tableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tableLayoutMain.Location = New System.Drawing.Point(0, 0)
        Me.tableLayoutMain.Name = "tableLayoutMain"
        Me.tableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.tableLayoutMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tableLayoutMain.Size = New System.Drawing.Size(1448, 697)
        Me.tableLayoutMain.TabIndex = 100
        '
        ' dgvCodeResults
        '
        dgvCodeResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCodeResults.Dock = System.Windows.Forms.DockStyle.None
        dgvCodeResults.Location = New System.Drawing.Point(0, 44)
        dgvCodeResults.Name = "dgvCodeResults"
        dgvCodeResults.Size = New System.Drawing.Size(1448, 653)
        dgvCodeResults.TabIndex = 0
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
        Me.btnPrevPage.Location = New System.Drawing.Point(1197, 674)
        Me.btnPrevPage.Name = "btnPrevPage"
        Me.btnPrevPage.Size = New System.Drawing.Size(121, 23)
        Me.btnPrevPage.TabIndex = 4
        Me.btnPrevPage.Text = "Previous"
        Me.btnPrevPage.UseVisualStyleBackColor = True
        '
        ' btnNextPage
        '
        Me.btnNextPage.Location = New System.Drawing.Point(1324, 674)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(121, 23)
        Me.btnNextPage.TabIndex = 5
        Me.btnNextPage.Text = "Next"
        Me.btnNextPage.UseVisualStyleBackColor = True
        '
        ' lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(656, 678)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(39, 15)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.Text = "Status"
        '
        ' HCPCSCodeResultsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1448, 697)
        Controls.Add(Me.dgvCodeResults)
        Me.Controls.Add(Me.tableLayoutMain)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.btnNextPage)
        Me.Controls.Add(Me.btnPrevPage)
        Me.Controls.Add(Me.lblHCPCSDescription)
        Me.Controls.Add(Me.linkMoreInfo)
        Me.Name = "HCPCSCodeResultsForm"
        Me.Text = "HCPCS Code Results"
        Me.tableLayoutMain.ResumeLayout(False)
        CType(Me.dgvCodeResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents tableLayoutMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents dgvCodeResults As System.Windows.Forms.DataGridView
    Friend WithEvents lblHCPCSDescription As System.Windows.Forms.Label
    Friend WithEvents linkMoreInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents btnPrevPage As System.Windows.Forms.Button
    Friend WithEvents btnNextPage As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
End Class
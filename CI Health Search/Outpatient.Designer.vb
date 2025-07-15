<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Outpatient
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        btnInpatientOutpatient = New Button()
        btnQualityOutpatient = New Button()
        btnFinIndOutpatient = New Button()
        btnFinancialOutpatient = New Button()
        btnDepartmentsOutpatient = New Button()
        btnProfileOutpatient = New Button()
        Button1 = New Button()
        dgvAPC = New DataGridView()
        Panel1 = New Panel()
        Label1 = New Label()
        CType(dgvAPC, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' btnInpatientOutpatient
        ' 
        btnInpatientOutpatient.Location = New Point(1320, 9)
        btnInpatientOutpatient.Name = "btnInpatientOutpatient"
        btnInpatientOutpatient.Size = New Size(75, 26)
        btnInpatientOutpatient.TabIndex = 19
        btnInpatientOutpatient.Text = "Inp"
        btnInpatientOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnQualityOutpatient
        ' 
        btnQualityOutpatient.Location = New Point(1239, 8)
        btnQualityOutpatient.Name = "btnQualityOutpatient"
        btnQualityOutpatient.Size = New Size(75, 26)
        btnQualityOutpatient.TabIndex = 18
        btnQualityOutpatient.Text = "Quality"
        btnQualityOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnFinIndOutpatient
        ' 
        btnFinIndOutpatient.Location = New Point(1158, 8)
        btnFinIndOutpatient.Name = "btnFinIndOutpatient"
        btnFinIndOutpatient.Size = New Size(75, 27)
        btnFinIndOutpatient.TabIndex = 17
        btnFinIndOutpatient.Text = "Fin Ind"
        btnFinIndOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnFinancialOutpatient
        ' 
        btnFinancialOutpatient.Location = New Point(1077, 9)
        btnFinancialOutpatient.Name = "btnFinancialOutpatient"
        btnFinancialOutpatient.Size = New Size(75, 26)
        btnFinancialOutpatient.TabIndex = 16
        btnFinancialOutpatient.Text = "Financial"
        btnFinancialOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnDepartmentsOutpatient
        ' 
        btnDepartmentsOutpatient.Location = New Point(996, 9)
        btnDepartmentsOutpatient.Name = "btnDepartmentsOutpatient"
        btnDepartmentsOutpatient.Size = New Size(75, 26)
        btnDepartmentsOutpatient.TabIndex = 15
        btnDepartmentsOutpatient.Text = "Departments"
        btnDepartmentsOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnProfileOutpatient
        ' 
        btnProfileOutpatient.Location = New Point(915, 9)
        btnProfileOutpatient.Name = "btnProfileOutpatient"
        btnProfileOutpatient.Size = New Size(75, 26)
        btnProfileOutpatient.TabIndex = 14
        btnProfileOutpatient.Text = "Profile"
        btnProfileOutpatient.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(1263, 581)
        Button1.Name = "Button1"
        Button1.Size = New Size(144, 81)
        Button1.TabIndex = 20
        Button1.Text = "Return to Search"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' dgvAPC
        ' 
        dgvAPC.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAPC.Location = New Point(37, 41)
        dgvAPC.Name = "dgvAPC"
        dgvAPC.Size = New Size(897, 541)
        dgvAPC.TabIndex = 21
        ' 
        ' Panel1
        ' 
        Panel1.AutoScroll = True
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(dgvAPC)
        Panel1.Controls.Add(Button1)
        Panel1.Controls.Add(btnFinIndOutpatient)
        Panel1.Controls.Add(btnInpatientOutpatient)
        Panel1.Controls.Add(btnProfileOutpatient)
        Panel1.Controls.Add(btnQualityOutpatient)
        Panel1.Controls.Add(btnDepartmentsOutpatient)
        Panel1.Controls.Add(btnFinancialOutpatient)
        Panel1.Location = New Point(-1, 3)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(2072, 2000)
        Panel1.TabIndex = 22
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label1.Location = New Point(37, 20)
        Label1.Name = "Label1"
        Label1.Size = New Size(376, 15)
        Label1.TabIndex = 22
        Label1.Text = "Statistics for the Top 20 Ambulatory Payment Classifications (APCs)"
        ' 
        ' Outpatient
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1527, 677)
        Controls.Add(Panel1)
        Name = "Outpatient"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Outpatient"
        CType(dgvAPC, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub
    Friend WithEvents btnInpatientOutpatient As Button
    Friend WithEvents btnQualityOutpatient As Button
    Friend WithEvents btnFinIndOutpatient As Button
    Friend WithEvents btnFinancialOutpatient As Button
    Friend WithEvents btnDepartmentsOutpatient As Button
    Friend WithEvents btnProfileOutpatient As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents dgvAPC As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
End Class

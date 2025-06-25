<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Financial
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
        GroupBox1 = New GroupBox()
        lbligrresult = New Label()
        lblogrresult = New Label()
        lblogr = New Label()
        lbligr = New Label()
        gbIncomeStatement = New GroupBox()
        Label2 = New Label()
        gbUncompensatedCare = New GroupBox()
        btnOutpatientFinancial = New Button()
        btnInpatientFinancial = New Button()
        btnQualityFinancial = New Button()
        btnFInIndFinancial = New Button()
        btnDepartmentsFinancial = New Button()
        btnProfileFinancial = New Button()
        GroupBox1.SuspendLayout()
        gbIncomeStatement.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(lbligrresult)
        GroupBox1.Controls.Add(lblogrresult)
        GroupBox1.Controls.Add(lblogr)
        GroupBox1.Controls.Add(lbligr)
        GroupBox1.Location = New Point(12, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(646, 360)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "GroupBox1"
        ' 
        ' lbligrresult
        ' 
        lbligrresult.AutoSize = True
        lbligrresult.Location = New Point(123, 36)
        lbligrresult.Name = "lbligrresult"
        lbligrresult.Size = New Size(74, 15)
        lbligrresult.TabIndex = 3
        lbligrresult.Text = "Inp gross rev"
        ' 
        ' lblogrresult
        ' 
        lblogrresult.AutoSize = True
        lblogrresult.Location = New Point(92, 61)
        lblogrresult.Name = "lblogrresult"
        lblogrresult.Size = New Size(31, 15)
        lblogrresult.TabIndex = 2
        lblogrresult.Text = "OGR"
        ' 
        ' lblogr
        ' 
        lblogr.AutoSize = True
        lblogr.Location = New Point(24, 61)
        lblogr.Name = "lblogr"
        lblogr.Size = New Size(31, 15)
        lblogr.TabIndex = 1
        lblogr.Text = "OGR"
        ' 
        ' lbligr
        ' 
        lbligr.AutoSize = True
        lbligr.Location = New Point(24, 36)
        lbligr.Name = "lbligr"
        lbligr.Size = New Size(74, 15)
        lbligr.TabIndex = 0
        lbligr.Text = "Inp gross rev"
        ' 
        ' gbIncomeStatement
        ' 
        gbIncomeStatement.Controls.Add(Label2)
        gbIncomeStatement.Location = New Point(664, 48)
        gbIncomeStatement.Name = "gbIncomeStatement"
        gbIncomeStatement.Size = New Size(604, 567)
        gbIncomeStatement.TabIndex = 1
        gbIncomeStatement.TabStop = False
        gbIncomeStatement.Text = "Income Statement"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(121, 93)
        Label2.Name = "Label2"
        Label2.Size = New Size(376, 15)
        Label2.TabIndex = 1
        Label2.Text = "Needs to be table with static rows, but coulmns with past 4 years or so"
        ' 
        ' gbUncompensatedCare
        ' 
        gbUncompensatedCare.Location = New Point(12, 378)
        gbUncompensatedCare.Name = "gbUncompensatedCare"
        gbUncompensatedCare.Size = New Size(633, 246)
        gbUncompensatedCare.TabIndex = 2
        gbUncompensatedCare.TabStop = False
        gbUncompensatedCare.Text = "Uncompensated Care"
        ' 
        ' btnOutpatientFinancial
        ' 
        btnOutpatientFinancial.Location = New Point(1474, 1)
        btnOutpatientFinancial.Name = "btnOutpatientFinancial"
        btnOutpatientFinancial.Size = New Size(88, 29)
        btnOutpatientFinancial.TabIndex = 20
        btnOutpatientFinancial.Text = "Outp"
        btnOutpatientFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnInpatientFinancial
        ' 
        btnInpatientFinancial.Location = New Point(1380, 1)
        btnInpatientFinancial.Name = "btnInpatientFinancial"
        btnInpatientFinancial.Size = New Size(88, 29)
        btnInpatientFinancial.TabIndex = 19
        btnInpatientFinancial.Text = "Inp"
        btnInpatientFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnQualityFinancial
        ' 
        btnQualityFinancial.Location = New Point(1286, 1)
        btnQualityFinancial.Name = "btnQualityFinancial"
        btnQualityFinancial.Size = New Size(88, 29)
        btnQualityFinancial.TabIndex = 18
        btnQualityFinancial.Text = "Quality"
        btnQualityFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnFInIndFinancial
        ' 
        btnFInIndFinancial.Location = New Point(1192, 1)
        btnFInIndFinancial.Name = "btnFInIndFinancial"
        btnFInIndFinancial.Size = New Size(88, 29)
        btnFInIndFinancial.TabIndex = 17
        btnFInIndFinancial.Text = "Fin Ind"
        btnFInIndFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnDepartmentsFinancial
        ' 
        btnDepartmentsFinancial.Location = New Point(1098, 1)
        btnDepartmentsFinancial.Name = "btnDepartmentsFinancial"
        btnDepartmentsFinancial.Size = New Size(88, 29)
        btnDepartmentsFinancial.TabIndex = 15
        btnDepartmentsFinancial.Text = "Departments"
        btnDepartmentsFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnProfileFinancial
        ' 
        btnProfileFinancial.Location = New Point(1004, 1)
        btnProfileFinancial.Name = "btnProfileFinancial"
        btnProfileFinancial.Size = New Size(88, 29)
        btnProfileFinancial.TabIndex = 14
        btnProfileFinancial.Text = "Profile"
        btnProfileFinancial.UseVisualStyleBackColor = True
        ' 
        ' Financial
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1563, 662)
        Controls.Add(btnOutpatientFinancial)
        Controls.Add(btnInpatientFinancial)
        Controls.Add(btnQualityFinancial)
        Controls.Add(btnFInIndFinancial)
        Controls.Add(btnDepartmentsFinancial)
        Controls.Add(btnProfileFinancial)
        Controls.Add(gbUncompensatedCare)
        Controls.Add(gbIncomeStatement)
        Controls.Add(GroupBox1)
        Name = "Financial"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Financial"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        gbIncomeStatement.ResumeLayout(False)
        gbIncomeStatement.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lbligr As Label
    Friend WithEvents gbIncomeStatement As GroupBox
    Friend WithEvents gbUncompensatedCare As GroupBox
    Friend WithEvents btnOutpatientFinancial As Button
    Friend WithEvents btnInpatientFinancial As Button
    Friend WithEvents btnQualityFinancial As Button
    Friend WithEvents btnFInIndFinancial As Button
    Friend WithEvents btnDepartmentsFinancial As Button
    Friend WithEvents btnProfileFinancial As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents lblogr As Label
    Friend WithEvents lbligrresult As Label
    Friend WithEvents lblogrresult As Label
End Class

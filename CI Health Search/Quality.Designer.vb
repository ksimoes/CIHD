<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Quality
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
        btnOutpatientQuality = New Button()
        btnInpatientQuality = New Button()
        btnFinIndQuality = New Button()
        btnFinancialQuality = New Button()
        btnDepartmentsQuality = New Button()
        btnProfileQuality = New Button()
        Button1 = New Button()
        SuspendLayout()
        ' 
        ' btnOutpatientQuality
        ' 
        btnOutpatientQuality.Location = New Point(1481, 2)
        btnOutpatientQuality.Name = "btnOutpatientQuality"
        btnOutpatientQuality.Size = New Size(75, 28)
        btnOutpatientQuality.TabIndex = 20
        btnOutpatientQuality.Text = "Outp"
        btnOutpatientQuality.UseVisualStyleBackColor = True
        ' 
        ' btnInpatientQuality
        ' 
        btnInpatientQuality.Location = New Point(1400, 3)
        btnInpatientQuality.Name = "btnInpatientQuality"
        btnInpatientQuality.Size = New Size(75, 28)
        btnInpatientQuality.TabIndex = 19
        btnInpatientQuality.Text = "Inp"
        btnInpatientQuality.UseVisualStyleBackColor = True
        ' 
        ' btnFinIndQuality
        ' 
        btnFinIndQuality.Location = New Point(1319, 1)
        btnFinIndQuality.Name = "btnFinIndQuality"
        btnFinIndQuality.Size = New Size(75, 29)
        btnFinIndQuality.TabIndex = 17
        btnFinIndQuality.Text = "Fin Ind"
        btnFinIndQuality.UseVisualStyleBackColor = True
        ' 
        ' btnFinancialQuality
        ' 
        btnFinancialQuality.Location = New Point(1238, 2)
        btnFinancialQuality.Name = "btnFinancialQuality"
        btnFinancialQuality.Size = New Size(75, 28)
        btnFinancialQuality.TabIndex = 16
        btnFinancialQuality.Text = "Financial"
        btnFinancialQuality.UseVisualStyleBackColor = True
        ' 
        ' btnDepartmentsQuality
        ' 
        btnDepartmentsQuality.Location = New Point(1157, 2)
        btnDepartmentsQuality.Name = "btnDepartmentsQuality"
        btnDepartmentsQuality.Size = New Size(75, 28)
        btnDepartmentsQuality.TabIndex = 15
        btnDepartmentsQuality.Text = "Departments"
        btnDepartmentsQuality.UseVisualStyleBackColor = True
        ' 
        ' btnProfileQuality
        ' 
        btnProfileQuality.Location = New Point(1076, 2)
        btnProfileQuality.Name = "btnProfileQuality"
        btnProfileQuality.Size = New Size(75, 28)
        btnProfileQuality.TabIndex = 14
        btnProfileQuality.Text = "Profile"
        btnProfileQuality.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(1291, 526)
        Button1.Name = "Button1"
        Button1.Size = New Size(150, 69)
        Button1.TabIndex = 21
        Button1.Text = "Return to Search"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Quality
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1559, 686)
        Controls.Add(Button1)
        Controls.Add(btnOutpatientQuality)
        Controls.Add(btnInpatientQuality)
        Controls.Add(btnFinIndQuality)
        Controls.Add(btnFinancialQuality)
        Controls.Add(btnDepartmentsQuality)
        Controls.Add(btnProfileQuality)
        Name = "Quality"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Quality"
        ResumeLayout(False)
    End Sub

    Friend WithEvents btnOutpatientQuality As Button
    Friend WithEvents btnInpatientQuality As Button
    Friend WithEvents btnFinIndQuality As Button
    Friend WithEvents btnFinancialQuality As Button
    Friend WithEvents btnDepartmentsQuality As Button
    Friend WithEvents btnProfileQuality As Button
    Friend WithEvents Button1 As Button
End Class

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
        SuspendLayout()
        ' 
        ' btnInpatientOutpatient
        ' 
        btnInpatientOutpatient.Location = New Point(1449, 4)
        btnInpatientOutpatient.Name = "btnInpatientOutpatient"
        btnInpatientOutpatient.Size = New Size(75, 26)
        btnInpatientOutpatient.TabIndex = 19
        btnInpatientOutpatient.Text = "Inp"
        btnInpatientOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnQualityOutpatient
        ' 
        btnQualityOutpatient.Location = New Point(1368, 3)
        btnQualityOutpatient.Name = "btnQualityOutpatient"
        btnQualityOutpatient.Size = New Size(75, 26)
        btnQualityOutpatient.TabIndex = 18
        btnQualityOutpatient.Text = "Quality"
        btnQualityOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnFinIndOutpatient
        ' 
        btnFinIndOutpatient.Location = New Point(1287, 3)
        btnFinIndOutpatient.Name = "btnFinIndOutpatient"
        btnFinIndOutpatient.Size = New Size(75, 27)
        btnFinIndOutpatient.TabIndex = 17
        btnFinIndOutpatient.Text = "Fin Ind"
        btnFinIndOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnFinancialOutpatient
        ' 
        btnFinancialOutpatient.Location = New Point(1206, 4)
        btnFinancialOutpatient.Name = "btnFinancialOutpatient"
        btnFinancialOutpatient.Size = New Size(75, 26)
        btnFinancialOutpatient.TabIndex = 16
        btnFinancialOutpatient.Text = "Financial"
        btnFinancialOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnDepartmentsOutpatient
        ' 
        btnDepartmentsOutpatient.Location = New Point(1125, 4)
        btnDepartmentsOutpatient.Name = "btnDepartmentsOutpatient"
        btnDepartmentsOutpatient.Size = New Size(75, 26)
        btnDepartmentsOutpatient.TabIndex = 15
        btnDepartmentsOutpatient.Text = "Departments"
        btnDepartmentsOutpatient.UseVisualStyleBackColor = True
        ' 
        ' btnProfileOutpatient
        ' 
        btnProfileOutpatient.Location = New Point(1044, 4)
        btnProfileOutpatient.Name = "btnProfileOutpatient"
        btnProfileOutpatient.Size = New Size(75, 26)
        btnProfileOutpatient.TabIndex = 14
        btnProfileOutpatient.Text = "Profile"
        btnProfileOutpatient.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(1368, 584)
        Button1.Name = "Button1"
        Button1.Size = New Size(144, 81)
        Button1.TabIndex = 20
        Button1.Text = "Return to Search"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Outpatient
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1527, 677)
        Controls.Add(Button1)
        Controls.Add(btnInpatientOutpatient)
        Controls.Add(btnQualityOutpatient)
        Controls.Add(btnFinIndOutpatient)
        Controls.Add(btnFinancialOutpatient)
        Controls.Add(btnDepartmentsOutpatient)
        Controls.Add(btnProfileOutpatient)
        Name = "Outpatient"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Outpatient"
        ResumeLayout(False)
    End Sub
    Friend WithEvents btnInpatientOutpatient As Button
    Friend WithEvents btnQualityOutpatient As Button
    Friend WithEvents btnFinIndOutpatient As Button
    Friend WithEvents btnFinancialOutpatient As Button
    Friend WithEvents btnDepartmentsOutpatient As Button
    Friend WithEvents btnProfileOutpatient As Button
    Friend WithEvents Button1 As Button
End Class

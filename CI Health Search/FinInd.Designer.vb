<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FinInd
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
        btnOutpatientFinInd = New Button()
        btnInpatientFinInd = New Button()
        btnQualityFinInd = New Button()
        btnFinancialFinInd = New Button()
        btnDepartmentsFinInd = New Button()
        btnProfileFinInd = New Button()
        SuspendLayout()
        ' 
        ' btnOutpatientFinInd
        ' 
        btnOutpatientFinInd.Location = New Point(1454, 1)
        btnOutpatientFinInd.Name = "btnOutpatientFinInd"
        btnOutpatientFinInd.Size = New Size(95, 29)
        btnOutpatientFinInd.TabIndex = 20
        btnOutpatientFinInd.Text = "Outp"
        btnOutpatientFinInd.UseVisualStyleBackColor = True
        ' 
        ' btnInpatientFinInd
        ' 
        btnInpatientFinInd.Location = New Point(1353, 1)
        btnInpatientFinInd.Name = "btnInpatientFinInd"
        btnInpatientFinInd.Size = New Size(95, 29)
        btnInpatientFinInd.TabIndex = 19
        btnInpatientFinInd.Text = "Inp"
        btnInpatientFinInd.UseVisualStyleBackColor = True
        ' 
        ' btnQualityFinInd
        ' 
        btnQualityFinInd.Location = New Point(1252, 1)
        btnQualityFinInd.Name = "btnQualityFinInd"
        btnQualityFinInd.Size = New Size(95, 29)
        btnQualityFinInd.TabIndex = 18
        btnQualityFinInd.Text = "Quality"
        btnQualityFinInd.UseVisualStyleBackColor = True
        ' 
        ' btnFinancialFinInd
        ' 
        btnFinancialFinInd.Location = New Point(1151, 1)
        btnFinancialFinInd.Name = "btnFinancialFinInd"
        btnFinancialFinInd.Size = New Size(95, 29)
        btnFinancialFinInd.TabIndex = 16
        btnFinancialFinInd.Text = "Financial"
        btnFinancialFinInd.UseVisualStyleBackColor = True
        ' 
        ' btnDepartmentsFinInd
        ' 
        btnDepartmentsFinInd.Location = New Point(1050, 1)
        btnDepartmentsFinInd.Name = "btnDepartmentsFinInd"
        btnDepartmentsFinInd.Size = New Size(95, 29)
        btnDepartmentsFinInd.TabIndex = 15
        btnDepartmentsFinInd.Text = "Departments"
        btnDepartmentsFinInd.UseVisualStyleBackColor = True
        ' 
        ' btnProfileFinInd
        ' 
        btnProfileFinInd.Location = New Point(949, 1)
        btnProfileFinInd.Name = "btnProfileFinInd"
        btnProfileFinInd.Size = New Size(95, 29)
        btnProfileFinInd.TabIndex = 14
        btnProfileFinInd.Text = "Profile"
        btnProfileFinInd.UseVisualStyleBackColor = True
        ' 
        ' FinInd
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1553, 658)
        Controls.Add(btnOutpatientFinInd)
        Controls.Add(btnInpatientFinInd)
        Controls.Add(btnQualityFinInd)
        Controls.Add(btnFinancialFinInd)
        Controls.Add(btnDepartmentsFinInd)
        Controls.Add(btnProfileFinInd)
        Name = "FinInd"
        StartPosition = FormStartPosition.CenterScreen
        Text = "FinInd"
        ResumeLayout(False)
    End Sub

    Friend WithEvents btnOutpatientFinInd As Button
    Friend WithEvents btnInpatientFinInd As Button
    Friend WithEvents btnQualityFinInd As Button
    Friend WithEvents btnFinancialFinInd As Button
    Friend WithEvents btnDepartmentsFinInd As Button
    Friend WithEvents btnProfileFinInd As Button
End Class

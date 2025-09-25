<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NavigationPopupForm
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
        btnProfile = New Button()
        btnDepartments = New Button()
        btnFinancial = New Button()
        btnFinind = New Button()
        btnQuality = New Button()
        btnInpatient = New Button()
        btnOutpatient = New Button()
        Button8 = New Button()
        SuspendLayout()
        ' 
        ' btnProfile
        ' 
        btnProfile.BackColor = SystemColors.ButtonHighlight
        btnProfile.Location = New Point(122, 18)
        btnProfile.Name = "btnProfile"
        btnProfile.Size = New Size(164, 26)
        btnProfile.TabIndex = 8
        btnProfile.Text = "Profile"
        btnProfile.UseVisualStyleBackColor = False
        ' 
        ' btnDepartments
        ' 
        btnDepartments.Location = New Point(122, 50)
        btnDepartments.Name = "btnDepartments"
        btnDepartments.Size = New Size(164, 26)
        btnDepartments.TabIndex = 9
        btnDepartments.Text = "Departments"
        btnDepartments.UseVisualStyleBackColor = True
        ' 
        ' btnFinancial
        ' 
        btnFinancial.Location = New Point(122, 82)
        btnFinancial.Name = "btnFinancial"
        btnFinancial.Size = New Size(164, 26)
        btnFinancial.TabIndex = 10
        btnFinancial.Text = "Financial"
        btnFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnFinind
        ' 
        btnFinind.Location = New Point(122, 114)
        btnFinind.Name = "btnFinind"
        btnFinind.Size = New Size(164, 26)
        btnFinind.TabIndex = 11
        btnFinind.Text = "Financial Indicators"
        btnFinind.UseVisualStyleBackColor = True
        ' 
        ' btnQuality
        ' 
        btnQuality.Location = New Point(122, 146)
        btnQuality.Name = "btnQuality"
        btnQuality.Size = New Size(164, 26)
        btnQuality.TabIndex = 12
        btnQuality.Text = "Quality"
        btnQuality.UseVisualStyleBackColor = True
        ' 
        ' btnInpatient
        ' 
        btnInpatient.Location = New Point(122, 178)
        btnInpatient.Name = "btnInpatient"
        btnInpatient.Size = New Size(164, 26)
        btnInpatient.TabIndex = 13
        btnInpatient.Text = "Inpatient"
        btnInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnOutpatient
        ' 
        btnOutpatient.Location = New Point(122, 210)
        btnOutpatient.Name = "btnOutpatient"
        btnOutpatient.Size = New Size(164, 26)
        btnOutpatient.TabIndex = 14
        btnOutpatient.Text = "Outpatient"
        btnOutpatient.UseVisualStyleBackColor = True
        ' 
        ' Button8
        ' 
        Button8.BackColor = SystemColors.ButtonHighlight
        Button8.Location = New Point(122, 242)
        Button8.Name = "Button8"
        Button8.Size = New Size(164, 78)
        Button8.TabIndex = 19
        Button8.Text = "Back to Search"
        Button8.UseVisualStyleBackColor = False
        ' 
        ' NavigationPopupForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(427, 356)
        Controls.Add(Button8)
        Controls.Add(btnOutpatient)
        Controls.Add(btnInpatient)
        Controls.Add(btnQuality)
        Controls.Add(btnFinind)
        Controls.Add(btnFinancial)
        Controls.Add(btnDepartments)
        Controls.Add(btnProfile)
        Name = "NavigationPopupForm"
        Text = "NavigationPopupForm"
        ResumeLayout(False)
    End Sub

    Friend WithEvents btnProfile As Button
    Friend WithEvents btnDepartments As Button
    Friend WithEvents btnFinancial As Button
    Friend WithEvents btnFinind As Button
    Friend WithEvents btnQuality As Button
    Friend WithEvents btnInpatient As Button
    Friend WithEvents btnOutpatient As Button
    Friend WithEvents Button8 As Button
End Class

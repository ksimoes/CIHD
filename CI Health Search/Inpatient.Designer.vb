<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Inpatient
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
        btnOutpatientInpatient = New Button()
        btnQualityInpatient = New Button()
        btnFinIndInpatient = New Button()
        btnFinancialInpatient = New Button()
        btnDepartmentsInpatient = New Button()
        btnProfileInpatient = New Button()
        gbKeyAttending = New GroupBox()
        gbPatientOrigin = New GroupBox()
        gbTrendReport = New GroupBox()
        gbStatsforTop20BaseMsdrg = New GroupBox()
        Button1 = New Button()
        Label1 = New Label()
        Label2 = New Label()
        gbKeyAttending.SuspendLayout()
        SuspendLayout()
        ' 
        ' btnOutpatientInpatient
        ' 
        btnOutpatientInpatient.Location = New Point(1460, 11)
        btnOutpatientInpatient.Name = "btnOutpatientInpatient"
        btnOutpatientInpatient.Size = New Size(75, 24)
        btnOutpatientInpatient.TabIndex = 20
        btnOutpatientInpatient.Text = "Outp"
        btnOutpatientInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnQualityInpatient
        ' 
        btnQualityInpatient.Location = New Point(1379, 12)
        btnQualityInpatient.Name = "btnQualityInpatient"
        btnQualityInpatient.Size = New Size(75, 24)
        btnQualityInpatient.TabIndex = 18
        btnQualityInpatient.Text = "Quality"
        btnQualityInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnFinIndInpatient
        ' 
        btnFinIndInpatient.Location = New Point(1298, 10)
        btnFinIndInpatient.Name = "btnFinIndInpatient"
        btnFinIndInpatient.Size = New Size(75, 25)
        btnFinIndInpatient.TabIndex = 17
        btnFinIndInpatient.Text = "Fin Ind"
        btnFinIndInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnFinancialInpatient
        ' 
        btnFinancialInpatient.Location = New Point(1217, 11)
        btnFinancialInpatient.Name = "btnFinancialInpatient"
        btnFinancialInpatient.Size = New Size(75, 24)
        btnFinancialInpatient.TabIndex = 16
        btnFinancialInpatient.Text = "Financial"
        btnFinancialInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnDepartmentsInpatient
        ' 
        btnDepartmentsInpatient.Location = New Point(1136, 11)
        btnDepartmentsInpatient.Name = "btnDepartmentsInpatient"
        btnDepartmentsInpatient.Size = New Size(75, 24)
        btnDepartmentsInpatient.TabIndex = 15
        btnDepartmentsInpatient.Text = "Departments"
        btnDepartmentsInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnProfileInpatient
        ' 
        btnProfileInpatient.Location = New Point(1055, 11)
        btnProfileInpatient.Name = "btnProfileInpatient"
        btnProfileInpatient.Size = New Size(75, 24)
        btnProfileInpatient.TabIndex = 14
        btnProfileInpatient.Text = "Profile"
        btnProfileInpatient.UseVisualStyleBackColor = True
        ' 
        ' gbKeyAttending
        ' 
        gbKeyAttending.Controls.Add(Label2)
        gbKeyAttending.Controls.Add(Label1)
        gbKeyAttending.Location = New Point(12, 12)
        gbKeyAttending.Name = "gbKeyAttending"
        gbKeyAttending.Size = New Size(580, 146)
        gbKeyAttending.TabIndex = 21
        gbKeyAttending.TabStop = False
        gbKeyAttending.Text = "Key Attending Providers"
        ' 
        ' gbPatientOrigin
        ' 
        gbPatientOrigin.Location = New Point(12, 164)
        gbPatientOrigin.Name = "gbPatientOrigin"
        gbPatientOrigin.Size = New Size(269, 100)
        gbPatientOrigin.TabIndex = 22
        gbPatientOrigin.TabStop = False
        gbPatientOrigin.Text = "Patient Origin"
        ' 
        ' gbTrendReport
        ' 
        gbTrendReport.Location = New Point(12, 295)
        gbTrendReport.Name = "gbTrendReport"
        gbTrendReport.Size = New Size(269, 100)
        gbTrendReport.TabIndex = 22
        gbTrendReport.TabStop = False
        gbTrendReport.Text = "Trend Report"
        ' 
        ' gbStatsforTop20BaseMsdrg
        ' 
        gbStatsforTop20BaseMsdrg.Location = New Point(12, 415)
        gbStatsforTop20BaseMsdrg.Name = "gbStatsforTop20BaseMsdrg"
        gbStatsforTop20BaseMsdrg.Size = New Size(269, 100)
        gbStatsforTop20BaseMsdrg.TabIndex = 23
        gbStatsforTop20BaseMsdrg.TabStop = False
        gbStatsforTop20BaseMsdrg.Text = "Stats for Top 20 Base MS-DRGs"
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(1393, 575)
        Button1.Name = "Button1"
        Button1.Size = New Size(132, 88)
        Button1.TabIndex = 24
        Button1.Text = "Return to Search"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label1.Location = New Point(6, 30)
        Label1.Name = "Label1"
        Label1.Size = New Size(40, 15)
        Label1.TabIndex = 25
        Label1.Text = "Name"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label2.Location = New Point(111, 30)
        Label2.Name = "Label2"
        Label2.Size = New Size(37, 15)
        Label2.TabIndex = 26
        Label2.Text = "Cases"
        ' 
        ' Inpatient
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1537, 675)
        Controls.Add(Button1)
        Controls.Add(gbStatsforTop20BaseMsdrg)
        Controls.Add(gbTrendReport)
        Controls.Add(gbPatientOrigin)
        Controls.Add(gbKeyAttending)
        Controls.Add(btnOutpatientInpatient)
        Controls.Add(btnQualityInpatient)
        Controls.Add(btnFinIndInpatient)
        Controls.Add(btnFinancialInpatient)
        Controls.Add(btnDepartmentsInpatient)
        Controls.Add(btnProfileInpatient)
        Name = "Inpatient"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Inpatient"
        gbKeyAttending.ResumeLayout(False)
        gbKeyAttending.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents btnOutpatientInpatient As Button
    Friend WithEvents btnQualityInpatient As Button
    Friend WithEvents btnFinIndInpatient As Button
    Friend WithEvents btnFinancialInpatient As Button
    Friend WithEvents btnDepartmentsInpatient As Button
    Friend WithEvents btnProfileInpatient As Button
    Friend WithEvents gbKeyAttending As GroupBox
    Friend WithEvents gbPatientOrigin As GroupBox
    Friend WithEvents gbTrendReport As GroupBox
    Friend WithEvents gbStatsforTop20BaseMsdrg As GroupBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
End Class

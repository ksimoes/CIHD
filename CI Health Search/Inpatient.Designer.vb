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
        dgvCeo = New DataGridView()
        gbPatientOrigin = New GroupBox()
        dgvPatientOrigin = New DataGridView()
        gbTrendReport = New GroupBox()
        Label16 = New Label()
        Label15 = New Label()
        Label14 = New Label()
        Label13 = New Label()
        gbStatsforTop20BaseMsdrg = New GroupBox()
        Label25 = New Label()
        Label24 = New Label()
        Label23 = New Label()
        Label22 = New Label()
        Label21 = New Label()
        Label20 = New Label()
        Label19 = New Label()
        Label18 = New Label()
        Label17 = New Label()
        Button1 = New Button()
        gbKeyAttending.SuspendLayout()
        CType(dgvCeo, ComponentModel.ISupportInitialize).BeginInit()
        gbPatientOrigin.SuspendLayout()
        CType(dgvPatientOrigin, ComponentModel.ISupportInitialize).BeginInit()
        gbTrendReport.SuspendLayout()
        gbStatsforTop20BaseMsdrg.SuspendLayout()
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
        gbKeyAttending.Controls.Add(dgvCeo)
        gbKeyAttending.Location = New Point(12, 12)
        gbKeyAttending.Name = "gbKeyAttending"
        gbKeyAttending.Size = New Size(580, 146)
        gbKeyAttending.TabIndex = 21
        gbKeyAttending.TabStop = False
        gbKeyAttending.Text = "Key Attending Providers"
        ' 
        ' dgvCeo
        ' 
        dgvCeo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCeo.Location = New Point(0, 22)
        dgvCeo.Name = "dgvCeo"
        dgvCeo.Size = New Size(568, 118)
        dgvCeo.TabIndex = 32
        ' 
        ' gbPatientOrigin
        ' 
        gbPatientOrigin.Controls.Add(dgvPatientOrigin)
        gbPatientOrigin.Location = New Point(12, 164)
        gbPatientOrigin.Name = "gbPatientOrigin"
        gbPatientOrigin.Size = New Size(1150, 274)
        gbPatientOrigin.TabIndex = 22
        gbPatientOrigin.TabStop = False
        gbPatientOrigin.Text = "Patient Origin"
        ' 
        ' dgvPatientOrigin
        ' 
        dgvPatientOrigin.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvPatientOrigin.Location = New Point(0, 22)
        dgvPatientOrigin.Name = "dgvPatientOrigin"
        dgvPatientOrigin.Size = New Size(984, 204)
        dgvPatientOrigin.TabIndex = 31
        ' 
        ' gbTrendReport
        ' 
        gbTrendReport.Controls.Add(Label16)
        gbTrendReport.Controls.Add(Label15)
        gbTrendReport.Controls.Add(Label14)
        gbTrendReport.Controls.Add(Label13)
        gbTrendReport.Location = New Point(12, 444)
        gbTrendReport.Name = "gbTrendReport"
        gbTrendReport.Size = New Size(503, 195)
        gbTrendReport.TabIndex = 22
        gbTrendReport.TabStop = False
        gbTrendReport.Text = "Trend Report"
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label16.Location = New Point(406, 19)
        Label16.Name = "Label16"
        Label16.Size = New Size(51, 15)
        Label16.TabIndex = 31
        Label16.Text = "FY 2022"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label15.Location = New Point(317, 19)
        Label15.Name = "Label15"
        Label15.Size = New Size(51, 15)
        Label15.TabIndex = 30
        Label15.Text = "FY 2023"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label14.Location = New Point(224, 19)
        Label14.Name = "Label14"
        Label14.Size = New Size(51, 15)
        Label14.TabIndex = 29
        Label14.Text = "FY 2024"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label13.Location = New Point(6, 19)
        Label13.Name = "Label13"
        Label13.Size = New Size(171, 15)
        Label13.TabIndex = 28
        Label13.Text = "Inpatient Utilization Statistics"
        ' 
        ' gbStatsforTop20BaseMsdrg
        ' 
        gbStatsforTop20BaseMsdrg.Controls.Add(Label25)
        gbStatsforTop20BaseMsdrg.Controls.Add(Label24)
        gbStatsforTop20BaseMsdrg.Controls.Add(Label23)
        gbStatsforTop20BaseMsdrg.Controls.Add(Label22)
        gbStatsforTop20BaseMsdrg.Controls.Add(Label21)
        gbStatsforTop20BaseMsdrg.Controls.Add(Label20)
        gbStatsforTop20BaseMsdrg.Controls.Add(Label19)
        gbStatsforTop20BaseMsdrg.Controls.Add(Label18)
        gbStatsforTop20BaseMsdrg.Controls.Add(Label17)
        gbStatsforTop20BaseMsdrg.Location = New Point(12, 645)
        gbStatsforTop20BaseMsdrg.Name = "gbStatsforTop20BaseMsdrg"
        gbStatsforTop20BaseMsdrg.Size = New Size(1130, 322)
        gbStatsforTop20BaseMsdrg.TabIndex = 23
        gbStatsforTop20BaseMsdrg.TabStop = False
        gbStatsforTop20BaseMsdrg.Text = "Stats for Top 20 Base MS-DRGs"
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label25.Location = New Point(1024, 28)
        Label25.Name = "Label25"
        Label25.Size = New Size(61, 15)
        Label25.TabIndex = 38
        Label25.Text = "MCC Rate"
        ' 
        ' Label24
        ' 
        Label24.AutoSize = True
        Label24.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label24.Location = New Point(904, 28)
        Label24.Name = "Label24"
        Label24.Size = New Size(80, 15)
        Label24.TabIndex = 37
        Label24.Text = "CC/MCC Rate"
        ' 
        ' Label23
        ' 
        Label23.AutoSize = True
        Label23.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label23.Location = New Point(789, 28)
        Label23.Name = "Label23"
        Label23.Size = New Size(91, 15)
        Label23.TabIndex = 36
        Label23.Text = "Case Mix Index"
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label22.Location = New Point(664, 28)
        Label22.Name = "Label22"
        Label22.Size = New Size(81, 15)
        Label22.TabIndex = 35
        Label22.Text = "Average Cost"
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label21.Location = New Point(518, 28)
        Label21.Name = "Label21"
        Label21.Size = New Size(106, 15)
        Label21.TabIndex = 34
        Label21.Text = "Average Payment"
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label20.Location = New Point(433, 28)
        Label20.Name = "Label20"
        Label20.Size = New Size(37, 15)
        Label20.TabIndex = 33
        Label20.Text = "ALOS"
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label19.Location = New Point(317, 28)
        Label19.Name = "Label19"
        Label19.Size = New Size(65, 15)
        Label19.TabIndex = 32
        Label19.Text = "IPPS Cases"
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label18.Location = New Point(126, 28)
        Label18.Name = "Label18"
        Label18.Size = New Size(152, 15)
        Label18.TabIndex = 31
        Label18.Text = "Base MS-DRG Description"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label17.Location = New Point(6, 28)
        Label17.Name = "Label17"
        Label17.Size = New Size(85, 15)
        Label17.TabIndex = 30
        Label17.Text = "Base MS-DRG"
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
        ' Inpatient
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1537, 1061)
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
        CType(dgvCeo, ComponentModel.ISupportInitialize).EndInit()
        gbPatientOrigin.ResumeLayout(False)
        CType(dgvPatientOrigin, ComponentModel.ISupportInitialize).EndInit()
        gbTrendReport.ResumeLayout(False)
        gbTrendReport.PerformLayout()
        gbStatsforTop20BaseMsdrg.ResumeLayout(False)
        gbStatsforTop20BaseMsdrg.PerformLayout()
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
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents dgvPatientOrigin As DataGridView
    Friend WithEvents dgvCeo As DataGridView
End Class

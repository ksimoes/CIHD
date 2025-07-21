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
        Button1 = New Button()
        dgvNewApiTable = New DataGridView()
        lblstatus = New Label()
        gbKeyAttending.SuspendLayout()
        CType(dgvCeo, ComponentModel.ISupportInitialize).BeginInit()
        gbPatientOrigin.SuspendLayout()
        CType(dgvPatientOrigin, ComponentModel.ISupportInitialize).BeginInit()
        gbTrendReport.SuspendLayout()
        CType(dgvNewApiTable, ComponentModel.ISupportInitialize).BeginInit()
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
        ' Button1
        ' 
        Button1.Location = New Point(1393, 575)
        Button1.Name = "Button1"
        Button1.Size = New Size(132, 88)
        Button1.TabIndex = 24
        Button1.Text = "Return to Search"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' dgvNewApiTable
        ' 
        dgvNewApiTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvNewApiTable.Location = New Point(12, 685)
        dgvNewApiTable.Name = "dgvNewApiTable"
        dgvNewApiTable.Size = New Size(809, 364)
        dgvNewApiTable.TabIndex = 25
        ' 
        ' lblstatus
        ' 
        lblstatus.AutoSize = True
        lblstatus.Location = New Point(765, 68)
        lblstatus.Name = "lblstatus"
        lblstatus.Size = New Size(41, 15)
        lblstatus.TabIndex = 75
        lblstatus.Text = "Label1"
        ' 
        ' Inpatient
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1537, 1061)
        Controls.Add(lblstatus)
        Controls.Add(dgvNewApiTable)
        Controls.Add(Button1)
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
        CType(dgvNewApiTable, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
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
    Friend WithEvents Button1 As Button
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents dgvPatientOrigin As DataGridView
    Friend WithEvents dgvCeo As DataGridView
    Friend WithEvents dgvNewApiTable As DataGridView
    Friend WithEvents lblstatus As Label
End Class

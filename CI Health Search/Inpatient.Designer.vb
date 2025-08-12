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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Inpatient))
        btnOutpatientInpatient = New Button()
        btnQualityInpatient = New Button()
        btnFinIndInpatient = New Button()
        btnFinancialInpatient = New Button()
        btnDepartmentsInpatient = New Button()
        btnProfileInpatient = New Button()
        gbKeyAttending = New GroupBox()
        picLoad1 = New PictureBox()
        dgvCeo = New DataGridView()
        gbPatientOrigin = New GroupBox()
        picLoad2 = New PictureBox()
        dgvPatientOrigin = New DataGridView()
        Button1 = New Button()
        dgvNewApiTable = New DataGridView()
        lblHN = New Label()
        picLoad3 = New PictureBox()
        gbKeyAttending.SuspendLayout()
        CType(picLoad1, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvCeo, ComponentModel.ISupportInitialize).BeginInit()
        gbPatientOrigin.SuspendLayout()
        CType(picLoad2, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvPatientOrigin, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvNewApiTable, ComponentModel.ISupportInitialize).BeginInit()
        CType(picLoad3, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btnOutpatientInpatient
        ' 
        btnOutpatientInpatient.Location = New Point(1468, 50)
        btnOutpatientInpatient.Name = "btnOutpatientInpatient"
        btnOutpatientInpatient.Size = New Size(75, 24)
        btnOutpatientInpatient.TabIndex = 20
        btnOutpatientInpatient.Text = "Outp"
        btnOutpatientInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnQualityInpatient
        ' 
        btnQualityInpatient.Location = New Point(1387, 51)
        btnQualityInpatient.Name = "btnQualityInpatient"
        btnQualityInpatient.Size = New Size(75, 24)
        btnQualityInpatient.TabIndex = 18
        btnQualityInpatient.Text = "Quality"
        btnQualityInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnFinIndInpatient
        ' 
        btnFinIndInpatient.Location = New Point(1306, 49)
        btnFinIndInpatient.Name = "btnFinIndInpatient"
        btnFinIndInpatient.Size = New Size(75, 25)
        btnFinIndInpatient.TabIndex = 17
        btnFinIndInpatient.Text = "Fin Ind"
        btnFinIndInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnFinancialInpatient
        ' 
        btnFinancialInpatient.Location = New Point(1225, 50)
        btnFinancialInpatient.Name = "btnFinancialInpatient"
        btnFinancialInpatient.Size = New Size(75, 24)
        btnFinancialInpatient.TabIndex = 16
        btnFinancialInpatient.Text = "Financial"
        btnFinancialInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnDepartmentsInpatient
        ' 
        btnDepartmentsInpatient.Location = New Point(1144, 50)
        btnDepartmentsInpatient.Name = "btnDepartmentsInpatient"
        btnDepartmentsInpatient.Size = New Size(75, 24)
        btnDepartmentsInpatient.TabIndex = 15
        btnDepartmentsInpatient.Text = "Departments"
        btnDepartmentsInpatient.UseVisualStyleBackColor = True
        ' 
        ' btnProfileInpatient
        ' 
        btnProfileInpatient.Location = New Point(1063, 50)
        btnProfileInpatient.Name = "btnProfileInpatient"
        btnProfileInpatient.Size = New Size(75, 24)
        btnProfileInpatient.TabIndex = 14
        btnProfileInpatient.Text = "Profile"
        btnProfileInpatient.UseVisualStyleBackColor = True
        ' 
        ' gbKeyAttending
        ' 
        gbKeyAttending.Controls.Add(picLoad1)
        gbKeyAttending.Controls.Add(dgvCeo)
        gbKeyAttending.Location = New Point(12, 49)
        gbKeyAttending.Name = "gbKeyAttending"
        gbKeyAttending.Size = New Size(580, 146)
        gbKeyAttending.TabIndex = 21
        gbKeyAttending.TabStop = False
        gbKeyAttending.Text = "Key Attending Providers"
        ' 
        ' picLoad1
        ' 
        picLoad1.BackColor = SystemColors.ControlDark
        picLoad1.Image = CType(resources.GetObject("picLoad1.Image"), Image)
        picLoad1.Location = New Point(161, 22)
        picLoad1.Name = "picLoad1"
        picLoad1.Size = New Size(200, 118)
        picLoad1.SizeMode = PictureBoxSizeMode.CenterImage
        picLoad1.TabIndex = 103
        picLoad1.TabStop = False
        picLoad1.Visible = False
        ' 
        ' dgvCeo
        ' 
        dgvCeo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCeo.Dock = DockStyle.Fill
        dgvCeo.Location = New Point(3, 19)
        dgvCeo.Name = "dgvCeo"
        dgvCeo.Size = New Size(574, 124)
        dgvCeo.TabIndex = 32
        ' 
        ' gbPatientOrigin
        ' 
        gbPatientOrigin.Controls.Add(picLoad2)
        gbPatientOrigin.Controls.Add(dgvPatientOrigin)
        gbPatientOrigin.Location = New Point(12, 298)
        gbPatientOrigin.Name = "gbPatientOrigin"
        gbPatientOrigin.Size = New Size(1150, 274)
        gbPatientOrigin.TabIndex = 22
        gbPatientOrigin.TabStop = False
        gbPatientOrigin.Text = "Patient Origin"
        ' 
        ' picLoad2
        ' 
        picLoad2.BackColor = SystemColors.ControlDark
        picLoad2.Image = CType(resources.GetObject("picLoad2.Image"), Image)
        picLoad2.Location = New Point(357, 22)
        picLoad2.Name = "picLoad2"
        picLoad2.Size = New Size(198, 204)
        picLoad2.TabIndex = 103
        picLoad2.TabStop = False
        picLoad2.Visible = False
        ' 
        ' dgvPatientOrigin
        ' 
        dgvPatientOrigin.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvPatientOrigin.Dock = DockStyle.Fill
        dgvPatientOrigin.Location = New Point(3, 19)
        dgvPatientOrigin.Name = "dgvPatientOrigin"
        dgvPatientOrigin.Size = New Size(1144, 252)
        dgvPatientOrigin.TabIndex = 31
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(1421, 81)
        Button1.Name = "Button1"
        Button1.Size = New Size(122, 30)
        Button1.TabIndex = 24
        Button1.Text = "Return to Search"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' dgvNewApiTable
        ' 
        dgvNewApiTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvNewApiTable.Location = New Point(1, 697)
        dgvNewApiTable.Name = "dgvNewApiTable"
        dgvNewApiTable.Size = New Size(809, 364)
        dgvNewApiTable.TabIndex = 25
        ' 
        ' lblHN
        ' 
        lblHN.AutoSize = True
        lblHN.Location = New Point(12, 211)
        lblHN.Name = "lblHN"
        lblHN.Size = New Size(41, 15)
        lblHN.TabIndex = 26
        lblHN.Text = "Label1"
        ' 
        ' picLoad3
        ' 
        picLoad3.BackColor = SystemColors.ControlDark
        picLoad3.Image = CType(resources.GetObject("picLoad3.Image"), Image)
        picLoad3.Location = New Point(254, 794)
        picLoad3.Name = "picLoad3"
        picLoad3.Size = New Size(198, 197)
        picLoad3.TabIndex = 103
        picLoad3.TabStop = False
        picLoad3.Visible = False
        ' 
        ' Inpatient
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1537, 1061)
        Controls.Add(btnOutpatientInpatient)
        Controls.Add(btnQualityInpatient)
        Controls.Add(btnFinIndInpatient)
        Controls.Add(btnFinancialInpatient)
        Controls.Add(btnDepartmentsInpatient)
        Controls.Add(btnProfileInpatient)
        Controls.Add(picLoad3)
        Controls.Add(lblHN)
        Controls.Add(dgvNewApiTable)
        Controls.Add(Button1)
        Controls.Add(gbPatientOrigin)
        Controls.Add(gbKeyAttending)
        Name = "Inpatient"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Inpatient"
        gbKeyAttending.ResumeLayout(False)
        CType(picLoad1, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvCeo, ComponentModel.ISupportInitialize).EndInit()
        gbPatientOrigin.ResumeLayout(False)
        CType(picLoad2, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvPatientOrigin, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvNewApiTable, ComponentModel.ISupportInitialize).EndInit()
        CType(picLoad3, ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents Button1 As Button
    Friend WithEvents dgvPatientOrigin As DataGridView
    Friend WithEvents dgvCeo As DataGridView
    Friend WithEvents dgvNewApiTable As DataGridView
    Friend WithEvents lblHN As Label
    Friend WithEvents picLoad1 As PictureBox
    Friend WithEvents picLoad2 As PictureBox
    Friend WithEvents picLoad3 As PictureBox
End Class

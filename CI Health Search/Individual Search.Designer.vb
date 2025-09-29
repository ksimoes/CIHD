<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Individual_Search
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
        GroupBox4 = New GroupBox()
        tbDrugGeneric = New TextBox()
        tbHCPCS = New TextBox()
        tbStLic = New TextBox()
        tbStLicNum = New TextBox()
        tbDrug = New TextBox()
        Label16 = New Label()
        Label20 = New Label()
        Label15 = New Label()
        lblBrand = New Label()
        Label14 = New Label()
        Label13 = New Label()
        Label12 = New Label()
        tbProvEnroll = New TextBox()
        tbFacilityTyp = New TextBox()
        GroupBox3 = New GroupBox()
        rdoFemale = New RadioButton()
        rdoMale = New RadioButton()
        cboTaxonomy = New ComboBox()
        Label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        Label5 = New Label()
        Label8 = New Label()
        tbNpi = New TextBox()
        tbFirst = New TextBox()
        tbMedSchool = New TextBox()
        tbMiddle = New TextBox()
        tbGradYear = New TextBox()
        tbLast = New TextBox()
        tbState = New TextBox()
        Label18 = New Label()
        Label17 = New Label()
        Label11 = New Label()
        GroupBox2 = New GroupBox()
        tbAT = New TextBox()
        Label6 = New Label()
        Label7 = New Label()
        Label9 = New Label()
        Label10 = New Label()
        tbAddress = New TextBox()
        tbCity = New TextBox()
        tbZip = New TextBox()
        Button1 = New Button()
        btnOrgSearch = New Button()
        btnSearch = New Button()
        GroupBox1.SuspendLayout()
        GroupBox4.SuspendLayout()
        GroupBox3.SuspendLayout()
        GroupBox2.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(GroupBox4)
        GroupBox1.Controls.Add(GroupBox3)
        GroupBox1.Controls.Add(GroupBox2)
        GroupBox1.Controls.Add(Button1)
        GroupBox1.Controls.Add(btnOrgSearch)
        GroupBox1.Controls.Add(btnSearch)
        GroupBox1.Location = New Point(12, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(1290, 484)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "GroupBox1"
        ' 
        ' GroupBox4
        ' 
        GroupBox4.BackColor = Color.DarkOrchid
        GroupBox4.Controls.Add(tbDrugGeneric)
        GroupBox4.Controls.Add(tbHCPCS)
        GroupBox4.Controls.Add(tbStLic)
        GroupBox4.Controls.Add(tbStLicNum)
        GroupBox4.Controls.Add(tbDrug)
        GroupBox4.Controls.Add(Label16)
        GroupBox4.Controls.Add(Label20)
        GroupBox4.Controls.Add(Label15)
        GroupBox4.Controls.Add(lblBrand)
        GroupBox4.Controls.Add(Label14)
        GroupBox4.Controls.Add(Label13)
        GroupBox4.Controls.Add(Label12)
        GroupBox4.Controls.Add(tbProvEnroll)
        GroupBox4.Controls.Add(tbFacilityTyp)
        GroupBox4.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        GroupBox4.Location = New Point(852, 22)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.Size = New Size(435, 330)
        GroupBox4.TabIndex = 46
        GroupBox4.TabStop = False
        GroupBox4.Text = "Other"
        ' 
        ' tbDrugGeneric
        ' 
        tbDrugGeneric.Location = New Point(179, 220)
        tbDrugGeneric.Name = "tbDrugGeneric"
        tbDrugGeneric.Size = New Size(180, 23)
        tbDrugGeneric.TabIndex = 43
        ' 
        ' tbHCPCS
        ' 
        tbHCPCS.Location = New Point(179, 17)
        tbHCPCS.Name = "tbHCPCS"
        tbHCPCS.Size = New Size(180, 23)
        tbHCPCS.TabIndex = 22
        ' 
        ' tbStLic
        ' 
        tbStLic.Location = New Point(179, 52)
        tbStLic.Name = "tbStLic"
        tbStLic.Size = New Size(180, 23)
        tbStLic.TabIndex = 23
        ' 
        ' tbStLicNum
        ' 
        tbStLicNum.Location = New Point(179, 87)
        tbStLicNum.Name = "tbStLicNum"
        tbStLicNum.Size = New Size(180, 23)
        tbStLicNum.TabIndex = 24
        ' 
        ' tbDrug
        ' 
        tbDrug.Location = New Point(179, 191)
        tbDrug.Name = "tbDrug"
        tbDrug.Size = New Size(180, 23)
        tbDrug.TabIndex = 42
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(9, 25)
        Label16.Name = "Label16"
        Label16.Size = New Size(75, 15)
        Label16.TabIndex = 25
        Label16.Text = "HCPCS Code"
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(8, 231)
        Label20.Name = "Label20"
        Label20.Size = New Size(123, 15)
        Label20.TabIndex = 41
        Label20.Text = "Drug Name(Generic)"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(9, 60)
        Label15.Name = "Label15"
        Label15.Size = New Size(55, 15)
        Label15.TabIndex = 26
        Label15.Text = "State Lic"
        ' 
        ' lblBrand
        ' 
        lblBrand.AutoSize = True
        lblBrand.Location = New Point(8, 199)
        lblBrand.Name = "lblBrand"
        lblBrand.Size = New Size(107, 15)
        lblBrand.TabIndex = 40
        lblBrand.Text = "Drug Brand Name"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(9, 95)
        Label14.Name = "Label14"
        Label14.Size = New Size(65, 15)
        Label14.TabIndex = 27
        Label14.Text = "State Lic #"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(9, 130)
        Label13.Name = "Label13"
        Label13.Size = New Size(119, 15)
        Label13.TabIndex = 28
        Label13.Text = "Provider Enrollment"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(9, 165)
        Label12.Name = "Label12"
        Label12.Size = New Size(74, 15)
        Label12.TabIndex = 29
        Label12.Text = "Facility Type"
        ' 
        ' tbProvEnroll
        ' 
        tbProvEnroll.Location = New Point(179, 122)
        tbProvEnroll.Name = "tbProvEnroll"
        tbProvEnroll.Size = New Size(180, 23)
        tbProvEnroll.TabIndex = 32
        ' 
        ' tbFacilityTyp
        ' 
        tbFacilityTyp.Location = New Point(179, 154)
        tbFacilityTyp.Name = "tbFacilityTyp"
        tbFacilityTyp.Size = New Size(180, 23)
        tbFacilityTyp.TabIndex = 33
        ' 
        ' GroupBox3
        ' 
        GroupBox3.BackColor = Color.DarkTurquoise
        GroupBox3.Controls.Add(rdoFemale)
        GroupBox3.Controls.Add(rdoMale)
        GroupBox3.Controls.Add(cboTaxonomy)
        GroupBox3.Controls.Add(Label1)
        GroupBox3.Controls.Add(Label2)
        GroupBox3.Controls.Add(Label3)
        GroupBox3.Controls.Add(Label4)
        GroupBox3.Controls.Add(Label5)
        GroupBox3.Controls.Add(Label8)
        GroupBox3.Controls.Add(tbNpi)
        GroupBox3.Controls.Add(tbFirst)
        GroupBox3.Controls.Add(tbMedSchool)
        GroupBox3.Controls.Add(tbMiddle)
        GroupBox3.Controls.Add(tbGradYear)
        GroupBox3.Controls.Add(tbLast)
        GroupBox3.Controls.Add(tbState)
        GroupBox3.Controls.Add(Label18)
        GroupBox3.Controls.Add(Label17)
        GroupBox3.Controls.Add(Label11)
        GroupBox3.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        GroupBox3.Location = New Point(0, 22)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(396, 330)
        GroupBox3.TabIndex = 45
        GroupBox3.TabStop = False
        GroupBox3.Text = "Personal Information"
        ' 
        ' rdoFemale
        ' 
        rdoFemale.AutoSize = True
        rdoFemale.Location = New Point(200, 93)
        rdoFemale.Name = "rdoFemale"
        rdoFemale.Size = New Size(65, 19)
        rdoFemale.TabIndex = 38
        rdoFemale.TabStop = True
        rdoFemale.Text = "Female"
        rdoFemale.UseVisualStyleBackColor = True
        ' 
        ' rdoMale
        ' 
        rdoMale.AutoSize = True
        rdoMale.Location = New Point(271, 93)
        rdoMale.Name = "rdoMale"
        rdoMale.Size = New Size(52, 19)
        rdoMale.TabIndex = 37
        rdoMale.TabStop = True
        rdoMale.Text = "Male"
        rdoMale.UseVisualStyleBackColor = True
        ' 
        ' cboTaxonomy
        ' 
        cboTaxonomy.FormattingEnabled = True
        cboTaxonomy.Items.AddRange(New Object() {"Addiction Medicine", "Advanced Practice Midwife", "Allergy & Immunology", "ALLERGY IMMUNOLOGY", "Anesthesiologist Assistant", "Anesthesiology", "Audiologist", "Audiologist-Hearing Aid Fitter", "CARDIAC SURGERY", "Cardiology", "Cardiovascualar Disease (Cardiology)", "Case Manager/Care Coordinator", "CERTIFIED CLINICAL NURSE SPECIALIST", "CERTIFIED NURSE MIDWIFE", "CERTIFIED REGISTERED NURSE ASSISTANT (CRNA)", "Chiropractic", "Chiropractor", "Clinical Neuropsychologist", "Clinical Nurse Specialist", "Clinical Pharmacology", "CLINICAL PSYCHOLOGIST", "Clinical Social Worker", "Colon & Rectal Surgery", "Colorectal Surgery", "Community Health Worker", "CRITICAL CARE (INTENSIVISTS)", "Dentist", "Dermatology", "Diagnostic Radiology", "Drama Therapist", "Electrodiagnostic Medicine", "Emergency Medical Technician, Basic", "Emergency Medical Technician, Intermediate", "Emergency Medical Technician, Paramedic", "Emergency Medicine", "ENDOCRINOLOGY", "Family Medicine", "Family Practice", "Funeral Director", "Gastroenterology", "General Practice", "General Surgery", "Genetic Counselor, MS", "Geriatric Medicine", "GYNECOLOGICAL/ONCOLOGY", "Hand Surgery", "Health & Wellness Coach", "Health Educator", "HEMATOLOGY", "HEMATOLOGY/ONCOLOGY", "Homeopath", "Hospitalist", "Independent Medical Examiner", "INFECTIOUS DISEASE", "Integrative Medicine", "Internal Medicine", "Interpreter", "Interventional Pain Management", "INTERVENTIONAL RADIOLOGY", "Lactation Consultant, Non-RN", "Legal Medicine", "LICENSED CLINICAL SOCIAL WORKER", "Marriage & Family Therapist", "MAXILLOFACIAL SURGERY", "Mechanotherapist", "Medical Genetics", "Medical Genetics, Ph.D. Medical Genetics", "MEDICAL ONCOLOGY", "Midwife", "Midwife, Lay", "Military Health Care Provider", "Multi-Specialty", "Naprapath", "Naturopath", "Nephrology", "Neurological Surgery", "Neurology", "Neuromusculoskeletal Medicine & OMM", "Neuromusculoskeletal Medicine, Sports Medicine", "NEUROPSYCHIATRY", "Neurosurgery", "Nuclear Medicine", "Nurse Anesthetist, Certified Registered", "Nurse Practitioner", "Obstetrics & Gynecology", "Obstetrics Gynecology", "Obstetrics/Gynecology", "Occupational Therapist", "Occupational Therapy Assistant", "Ophthalmology", "Optometrist", "OPTOMETRY", "Oral & Maxillofacial Surgery", "Oral Surgery (dental only)", "Orthopaedic Surgery", "Orthopedic Surgery", "Osteopathic Manipulative", "Otolaryngology", "PAIN MANAGEMENT", "Pain Medicine", "Pathology", "Pediatric Medicine", "Pediatrics", "Peer Specialist", "Personal Emergency Response Attendant", "Phlebology", "Physical Medicine & Rehabilitation", "Physical Medicine and Rehabilitation", "Physical Therapist", "Physical Therapist in Private Practice", "Physician Assistant", "Plastic and Reconstructive Surgery", "Plastic Surgery", "Podiatrist", "PODIATRY", "Poetry Therapist", "Prevention Professional", "Preventive Medicine", "Psychiatry", "Psychiatry & Neurology", "Psychoanalyst", "Psychologist", "Pulmonary Disease", "RADIATION ONCOLOGY", "Radiology", "Reflexologist", "REGISTERED DIETITIAN/NUTRITION PROFESSIONAL", "RHEUMATOLOGY", "Single Specialty", "Sleep Specialist, PhD", "Social Worker", "Specialist", "Student in an Organized Health Care Education/Training Program", "Surgery", "SURGICAL ONCOLOGY", "Therapy (OMM)", "Thoracic Surgery", "Thoracic Surgery (Cardiothoracic Vascular Surgery)", "Transplant Surgery", "Urology", "VASCULAR SURGERY"})
        cboTaxonomy.Location = New Point(174, 59)
        cboTaxonomy.Name = "cboTaxonomy"
        cboTaxonomy.Size = New Size(182, 23)
        cboTaxonomy.TabIndex = 36
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(6, 31)
        Label1.Name = "Label1"
        Label1.Size = New Size(76, 15)
        Label1.TabIndex = 0
        Label1.Text = "NPI Number"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(6, 66)
        Label2.Name = "Label2"
        Label2.Size = New Size(119, 15)
        Label2.TabIndex = 1
        Label2.Text = "Taxonomy/Specialty"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(6, 130)
        Label3.Name = "Label3"
        Label3.Size = New Size(67, 15)
        Label3.TabIndex = 2
        Label3.Text = "First Name"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(6, 165)
        Label4.Name = "Label4"
        Label4.Size = New Size(81, 15)
        Label4.TabIndex = 3
        Label4.Text = "Middle Name"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(6, 200)
        Label5.Name = "Label5"
        Label5.Size = New Size(65, 15)
        Label5.TabIndex = 4
        Label5.Text = "Last Name"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(6, 229)
        Label8.Name = "Label8"
        Label8.Size = New Size(37, 15)
        Label8.TabIndex = 7
        Label8.Text = "State"
        ' 
        ' tbNpi
        ' 
        tbNpi.Location = New Point(176, 31)
        tbNpi.Name = "tbNpi"
        tbNpi.Size = New Size(180, 23)
        tbNpi.TabIndex = 11
        ' 
        ' tbFirst
        ' 
        tbFirst.Location = New Point(176, 121)
        tbFirst.Name = "tbFirst"
        tbFirst.Size = New Size(180, 23)
        tbFirst.TabIndex = 13
        ' 
        ' tbMedSchool
        ' 
        tbMedSchool.Location = New Point(176, 285)
        tbMedSchool.Name = "tbMedSchool"
        tbMedSchool.Size = New Size(180, 23)
        tbMedSchool.TabIndex = 35
        ' 
        ' tbMiddle
        ' 
        tbMiddle.Location = New Point(176, 157)
        tbMiddle.Name = "tbMiddle"
        tbMiddle.Size = New Size(180, 23)
        tbMiddle.TabIndex = 14
        ' 
        ' tbGradYear
        ' 
        tbGradYear.Location = New Point(176, 250)
        tbGradYear.Name = "tbGradYear"
        tbGradYear.Size = New Size(180, 23)
        tbGradYear.TabIndex = 34
        ' 
        ' tbLast
        ' 
        tbLast.Location = New Point(176, 192)
        tbLast.Name = "tbLast"
        tbLast.Size = New Size(180, 23)
        tbLast.TabIndex = 15
        ' 
        ' tbState
        ' 
        tbState.Location = New Point(176, 221)
        tbState.Name = "tbState"
        tbState.Size = New Size(180, 23)
        tbState.TabIndex = 18
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(6, 297)
        Label18.Name = "Label18"
        Label18.Size = New Size(90, 15)
        Label18.TabIndex = 31
        Label18.Text = "Medical School"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(6, 260)
        Label17.Name = "Label17"
        Label17.Size = New Size(96, 15)
        Label17.TabIndex = 30
        Label17.Text = "Graduation Year"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Location = New Point(6, 95)
        Label11.Name = "Label11"
        Label11.Size = New Size(49, 15)
        Label11.TabIndex = 10
        Label11.Text = "Gender"
        ' 
        ' GroupBox2
        ' 
        GroupBox2.BackColor = Color.LimeGreen
        GroupBox2.Controls.Add(tbAT)
        GroupBox2.Controls.Add(Label6)
        GroupBox2.Controls.Add(Label7)
        GroupBox2.Controls.Add(Label9)
        GroupBox2.Controls.Add(Label10)
        GroupBox2.Controls.Add(tbAddress)
        GroupBox2.Controls.Add(tbCity)
        GroupBox2.Controls.Add(tbZip)
        GroupBox2.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        GroupBox2.Location = New Point(412, 22)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(434, 330)
        GroupBox2.TabIndex = 44
        GroupBox2.TabStop = False
        GroupBox2.Text = "Address Info"
        ' 
        ' tbAT
        ' 
        tbAT.Location = New Point(176, 122)
        tbAT.Name = "tbAT"
        tbAT.Size = New Size(180, 23)
        tbAT.TabIndex = 20
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(6, 27)
        Label6.Name = "Label6"
        Label6.Size = New Size(90, 15)
        Label6.TabIndex = 5
        Label6.Text = "Street Address"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(6, 62)
        Label7.Name = "Label7"
        Label7.Size = New Size(28, 15)
        Label7.TabIndex = 6
        Label7.Text = "City"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(7, 95)
        Label9.Name = "Label9"
        Label9.Size = New Size(55, 15)
        Label9.TabIndex = 8
        Label9.Text = "Zip Code"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(6, 130)
        Label10.Name = "Label10"
        Label10.Size = New Size(80, 15)
        Label10.TabIndex = 9
        Label10.Text = "Address Type"
        ' 
        ' tbAddress
        ' 
        tbAddress.Location = New Point(176, 19)
        tbAddress.Name = "tbAddress"
        tbAddress.Size = New Size(180, 23)
        tbAddress.TabIndex = 16
        ' 
        ' tbCity
        ' 
        tbCity.Location = New Point(176, 54)
        tbCity.Name = "tbCity"
        tbCity.Size = New Size(180, 23)
        tbCity.TabIndex = 17
        ' 
        ' tbZip
        ' 
        tbZip.Location = New Point(176, 87)
        tbZip.Name = "tbZip"
        tbZip.Size = New Size(180, 23)
        tbZip.TabIndex = 19
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Button1.BackColor = Color.Red
        Button1.Location = New Point(745, 401)
        Button1.Name = "Button1"
        Button1.Size = New Size(167, 77)
        Button1.TabIndex = 39
        Button1.Text = "Clear"
        Button1.UseVisualStyleBackColor = False
        ' 
        ' btnOrgSearch
        ' 
        btnOrgSearch.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnOrgSearch.Location = New Point(554, 401)
        btnOrgSearch.Name = "btnOrgSearch"
        btnOrgSearch.Size = New Size(166, 77)
        btnOrgSearch.TabIndex = 38
        btnOrgSearch.Text = "Go to Organizations Search"
        btnOrgSearch.UseVisualStyleBackColor = True
        ' 
        ' btnSearch
        ' 
        btnSearch.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnSearch.BackColor = Color.Green
        btnSearch.Location = New Point(357, 401)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(167, 77)
        btnSearch.TabIndex = 37
        btnSearch.Text = "Search!"
        btnSearch.UseVisualStyleBackColor = False
        ' 
        ' Individual_Search
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        AutoScroll = True
        ClientSize = New Size(1314, 710)
        Controls.Add(GroupBox1)
        Name = "Individual_Search"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Individual_Search"
        GroupBox1.ResumeLayout(False)
        GroupBox4.ResumeLayout(False)
        GroupBox4.PerformLayout()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents tbStLicNum As TextBox
    Friend WithEvents tbStLic As TextBox
    Friend WithEvents tbHCPCS As TextBox
    Friend WithEvents tbAT As TextBox
    Friend WithEvents tbZip As TextBox
    Friend WithEvents tbState As TextBox
    Friend WithEvents tbCity As TextBox
    Friend WithEvents tbAddress As TextBox
    Friend WithEvents tbLast As TextBox
    Friend WithEvents tbMiddle As TextBox
    Friend WithEvents tbFirst As TextBox
    Friend WithEvents tbTS As TextBox
    Friend WithEvents tbNpi As TextBox
    Friend WithEvents tbMedSchool As TextBox
    Friend WithEvents tbGradYear As TextBox
    Friend WithEvents tbFacilityTyp As TextBox
    Friend WithEvents tbProvEnroll As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents btnOrgSearch As Button
    Friend WithEvents btnSearch As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents tbDrugGeneric As TextBox
    Friend WithEvents tbDrug As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents lblBrand As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents cboTaxonomy As ComboBox
    Friend WithEvents rdoFemale As RadioButton
    Friend WithEvents rdoMale As RadioButton
    'Friend WithEvents tbTS As ComboBox
End Class

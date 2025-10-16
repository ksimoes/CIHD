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
        groupMedSchool = New GroupBox()
        cboMedSchoolTax = New ComboBox()
        Label22 = New Label()
        tbMedSchool = New TextBox()
        Label18 = New Label()
        Label17 = New Label()
        tbGradYear = New TextBox()
        groupMeds = New GroupBox()
        tbDrugGeneric = New TextBox()
        lblBrand = New Label()
        Label20 = New Label()
        tbDrug = New TextBox()
        Label19 = New Label()
        groupBoxOther = New GroupBox()
        tbHCPCS = New TextBox()
        tbStLic = New TextBox()
        tbStLicNum = New TextBox()
        Label16 = New Label()
        Label15 = New Label()
        Label14 = New Label()
        Label13 = New Label()
        Label12 = New Label()
        tbProvEnroll = New TextBox()
        tbFacilityTyp = New TextBox()
        cboSearchType = New ComboBox()
        groupPersonal = New GroupBox()
        ckExact = New CheckBox()
        rdoFemale = New RadioButton()
        rdoMale = New RadioButton()
        cboTaxonomy = New ComboBox()
        lblNPI = New Label()
        lblTax = New Label()
        lblFirst = New Label()
        lblMiddle = New Label()
        lblLast = New Label()
        lblState = New Label()
        tbNpi = New TextBox()
        tbFirst = New TextBox()
        tbMiddle = New TextBox()
        tbLast = New TextBox()
        tbState = New TextBox()
        lblGender = New Label()
        btnSearch = New Button()
        btnOrgSearch = New Button()
        groupAddress = New GroupBox()
        Label21 = New Label()
        tbAddressState = New TextBox()
        tbAddressType = New TextBox()
        Label6 = New Label()
        Label7 = New Label()
        Label9 = New Label()
        Label10 = New Label()
        tbAddress = New TextBox()
        tbCity = New TextBox()
        tbZip = New TextBox()
        Button1 = New Button()
        GroupBox1.SuspendLayout()
        groupMedSchool.SuspendLayout()
        groupMeds.SuspendLayout()
        groupBoxOther.SuspendLayout()
        groupPersonal.SuspendLayout()
        groupAddress.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.BackgroundImageLayout = ImageLayout.Stretch
        GroupBox1.Controls.Add(groupMedSchool)
        GroupBox1.Controls.Add(groupMeds)
        GroupBox1.Controls.Add(Label19)
        GroupBox1.Controls.Add(groupBoxOther)
        GroupBox1.Controls.Add(cboSearchType)
        GroupBox1.Controls.Add(groupPersonal)
        GroupBox1.Controls.Add(btnSearch)
        GroupBox1.Controls.Add(btnOrgSearch)
        GroupBox1.Controls.Add(groupAddress)
        GroupBox1.Controls.Add(Button1)
        GroupBox1.Font = New Font("Elephant", 14.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        GroupBox1.Location = New Point(12, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(1247, 549)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "Search Invididuals"
        ' 
        ' groupMedSchool
        ' 
        groupMedSchool.BackColor = Color.LimeGreen
        groupMedSchool.Controls.Add(cboMedSchoolTax)
        groupMedSchool.Controls.Add(Label22)
        groupMedSchool.Controls.Add(tbMedSchool)
        groupMedSchool.Controls.Add(Label18)
        groupMedSchool.Controls.Add(Label17)
        groupMedSchool.Controls.Add(tbGradYear)
        groupMedSchool.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        groupMedSchool.Location = New Point(420, 289)
        groupMedSchool.Name = "groupMedSchool"
        groupMedSchool.Size = New Size(396, 119)
        groupMedSchool.TabIndex = 45
        groupMedSchool.TabStop = False
        groupMedSchool.Text = "Medical School Info"
        ' 
        ' cboMedSchoolTax
        ' 
        cboMedSchoolTax.FormattingEnabled = True
        cboMedSchoolTax.Items.AddRange(New Object() {"Addiction Medicine", "Advanced Practice Midwife", "Allergy & Immunology", "Allergy Immunology", "Anesthesiologist Assistant", "Anesthesiology", "Audiologist", "Audiologist-Hearing Aid Fitter", "Caridac Surgery", "Cardiology", "Cardiovascualar Disease (Cardiology)", "Case Manager/Care Coordinator", "Certified Clinical Nurse Specialist", "Certified Nurse Midwife", "Certified Registered Nurse Assistant (CRNA)", "Chiropractic", "Chiropractor", "Clinical Neuropsychologist", "Clinical Nurse Specialist", "Clinical Pharmacology", "Clinical Psychologist", "Clinical Social Worker", "Colon & Rectal Surgery", "Colorectal Surgery", "Community Health Worker", "Critical Care (Intensivists)", "Dentist", "Dermatology", "Diagnostic Radiology", "Drama Therapist", "Electrodiagnostic Medicine", "Emergency Medical Technician, Basic", "Emergency Medical Technician, Intermediate", "Emergency Medical Technician, Paramedic", "Emergency Medicine", "Endocrinology", "Family Medicine", "Family Practice", "Funeral Director", "Gastroenterology", "General Practice", "General Surgery", "Genetic Counselor, MS", "Geriatric Medicine", "Gynecological/Oncology", "Hand Surgery", "Health & Wellness Coach", "Health Educator", "Hematology", "Hematology/Oncology", "Homeopath", "Hospitalist", "Independent Medical Examiner", "Infectious Disease", "Integrative Medicine", "Internal Medicine", "Interpreter", "Interventional Pain Management", "Interventional Radiology", "Lactation Consultant, Non-RN", "Legal Medicine", "Licensed Clinical Social Worker", "Marriage & Family Therapist", "Maxillofacial Surgery", "Mechanotherapist", "Medical Genetics", "Medical Genetics, Ph.D. Medical Genetics", "Medical Oncology", "Midwife", "Midwife, Lay", "Military Health Care Provider", "Multi-Specialty", "Naprapath", "Naturopath", "Nephrology", "Neurological Surgery", "Neurology", "Neuromusculoskeletal Medicine & OMM", "Neuromusculoskeletal Medicine, Sports Medicine", "Neuropsychiatry", "Neurosurgery", "Nuclear Medicine", "Nurse Anesthetist, Certified Registered", "Nurse Practitioner", "Obstetrics/Gynecology", "Occupational Therapist", "Occupational Therapy Assistant", "Ophthalmology", "Optometrist", "Optometry", "Oral & Maxillofacial Surgery", "Oral Surgery (dental only)", "Orthopaedic Surgery", "Orthopedic Surgery", "Osteopathic Manipulative", "Otolaryngology", "Pain Management", "Pain Medicine", "Pathology", "Pediatric Medicine", "Pediatrics", "Peer Specialist", "Personal Emergency Response Attendant", "Phlebology", "Physical Medicine and Rehabilitation", "Physical Therapist", "Physical Therapist in Private Practice", "Physician Assistant", "Plastic and Reconstructive Surgery", "Plastic Surgery", "Podiatrist", "Podiatry", "Poetry Therapist", "Prevention Professional", "Preventive Medicine", "Psychiatry", "Psychiatry & Neurology", "Psychoanalyst", "Psychologist", "Pulmonary Disease", "Radiation Oncology", "Radiology", "Reflexologist", "Registered Dietitian/Nutrition Professional", "Rhuematology", "Single Specialty", "Sleep Specialist, PhD", "Social Worker", "Specialist", "Student in an Organized Health Care Education/Training Program", "Surgery", "Surgical Oncology", "Therapy (OMM)", "Thoracic Surgery", "Thoracic Surgery (Cardiothoracic Vascular Surgery)", "Transplant Surgery", "Urology", "Vascular Surgery"})
        cboMedSchoolTax.Location = New Point(154, 76)
        cboMedSchoolTax.Name = "cboMedSchoolTax"
        cboMedSchoolTax.Size = New Size(148, 23)
        cboMedSchoolTax.TabIndex = 38
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Location = New Point(20, 84)
        Label22.Name = "Label22"
        Label22.Size = New Size(119, 15)
        Label22.TabIndex = 37
        Label22.Text = "Taxonomy/Specialty"
        ' 
        ' tbMedSchool
        ' 
        tbMedSchool.Location = New Point(154, 18)
        tbMedSchool.Name = "tbMedSchool"
        tbMedSchool.Size = New Size(146, 23)
        tbMedSchool.TabIndex = 35
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Location = New Point(20, 26)
        Label18.Name = "Label18"
        Label18.Size = New Size(90, 15)
        Label18.TabIndex = 31
        Label18.Text = "Medical School"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Location = New Point(20, 55)
        Label17.Name = "Label17"
        Label17.Size = New Size(96, 15)
        Label17.TabIndex = 30
        Label17.Text = "Graduation Year"
        ' 
        ' tbGradYear
        ' 
        tbGradYear.Location = New Point(154, 47)
        tbGradYear.Name = "tbGradYear"
        tbGradYear.Size = New Size(40, 23)
        tbGradYear.TabIndex = 34
        ' 
        ' groupMeds
        ' 
        groupMeds.BackColor = Color.DarkOrchid
        groupMeds.Controls.Add(tbDrugGeneric)
        groupMeds.Controls.Add(lblBrand)
        groupMeds.Controls.Add(Label20)
        groupMeds.Controls.Add(tbDrug)
        groupMeds.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        groupMeds.Location = New Point(834, 289)
        groupMeds.Name = "groupMeds"
        groupMeds.Size = New Size(396, 121)
        groupMeds.TabIndex = 47
        groupMeds.TabStop = False
        groupMeds.Text = "Medication Info"
        ' 
        ' tbDrugGeneric
        ' 
        tbDrugGeneric.Location = New Point(179, 60)
        tbDrugGeneric.Name = "tbDrugGeneric"
        tbDrugGeneric.Size = New Size(143, 23)
        tbDrugGeneric.TabIndex = 43
        ' 
        ' lblBrand
        ' 
        lblBrand.AutoSize = True
        lblBrand.Location = New Point(8, 39)
        lblBrand.Name = "lblBrand"
        lblBrand.Size = New Size(107, 15)
        lblBrand.TabIndex = 40
        lblBrand.Text = "Drug Brand Name"
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Location = New Point(8, 71)
        Label20.Name = "Label20"
        Label20.Size = New Size(123, 15)
        Label20.TabIndex = 41
        Label20.Text = "Drug Name(Generic)"
        ' 
        ' tbDrug
        ' 
        tbDrug.Location = New Point(179, 31)
        tbDrug.Name = "tbDrug"
        tbDrug.Size = New Size(143, 23)
        tbDrug.TabIndex = 42
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.BackColor = Color.Transparent
        Label19.Font = New Font("Segoe UI Black", 12F, FontStyle.Bold Or FontStyle.Italic, GraphicsUnit.Point, CByte(0))
        Label19.Location = New Point(22, 33)
        Label19.Name = "Label19"
        Label19.Size = New Size(105, 21)
        Label19.TabIndex = 2
        Label19.Text = "Search Type"
        ' 
        ' groupBoxOther
        ' 
        groupBoxOther.BackColor = Color.DarkOrchid
        groupBoxOther.Controls.Add(tbHCPCS)
        groupBoxOther.Controls.Add(tbStLic)
        groupBoxOther.Controls.Add(tbStLicNum)
        groupBoxOther.Controls.Add(Label16)
        groupBoxOther.Controls.Add(Label15)
        groupBoxOther.Controls.Add(Label14)
        groupBoxOther.Controls.Add(Label13)
        groupBoxOther.Controls.Add(Label12)
        groupBoxOther.Controls.Add(tbProvEnroll)
        groupBoxOther.Controls.Add(tbFacilityTyp)
        groupBoxOther.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        groupBoxOther.Location = New Point(834, 80)
        groupBoxOther.Name = "groupBoxOther"
        groupBoxOther.Size = New Size(396, 203)
        groupBoxOther.TabIndex = 46
        groupBoxOther.TabStop = False
        groupBoxOther.Text = "Other"
        ' 
        ' tbHCPCS
        ' 
        tbHCPCS.Location = New Point(179, 26)
        tbHCPCS.Name = "tbHCPCS"
        tbHCPCS.Size = New Size(143, 23)
        tbHCPCS.TabIndex = 22
        ' 
        ' tbStLic
        ' 
        tbStLic.Location = New Point(179, 56)
        tbStLic.Name = "tbStLic"
        tbStLic.Size = New Size(143, 23)
        tbStLic.TabIndex = 23
        ' 
        ' tbStLicNum
        ' 
        tbStLicNum.Location = New Point(179, 89)
        tbStLicNum.Name = "tbStLicNum"
        tbStLicNum.Size = New Size(143, 23)
        tbStLicNum.TabIndex = 24
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Location = New Point(19, 28)
        Label16.Name = "Label16"
        Label16.Size = New Size(75, 15)
        Label16.TabIndex = 25
        Label16.Text = "HCPCS Code"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Location = New Point(19, 60)
        Label15.Name = "Label15"
        Label15.Size = New Size(55, 15)
        Label15.TabIndex = 26
        Label15.Text = "State Lic"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Location = New Point(19, 93)
        Label14.Name = "Label14"
        Label14.Size = New Size(65, 15)
        Label14.TabIndex = 27
        Label14.Text = "State Lic #"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Location = New Point(19, 130)
        Label13.Name = "Label13"
        Label13.Size = New Size(119, 15)
        Label13.TabIndex = 28
        Label13.Text = "Provider Enrollment"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Location = New Point(19, 160)
        Label12.Name = "Label12"
        Label12.Size = New Size(74, 15)
        Label12.TabIndex = 29
        Label12.Text = "Facility Type"
        ' 
        ' tbProvEnroll
        ' 
        tbProvEnroll.Location = New Point(179, 128)
        tbProvEnroll.Name = "tbProvEnroll"
        tbProvEnroll.Size = New Size(143, 23)
        tbProvEnroll.TabIndex = 32
        ' 
        ' tbFacilityTyp
        ' 
        tbFacilityTyp.Location = New Point(179, 163)
        tbFacilityTyp.Name = "tbFacilityTyp"
        tbFacilityTyp.Size = New Size(143, 23)
        tbFacilityTyp.TabIndex = 33
        ' 
        ' cboSearchType
        ' 
        cboSearchType.Font = New Font("Corbel", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cboSearchType.FormattingEnabled = True
        cboSearchType.Items.AddRange(New Object() {"Select Search Type", "NPI Registry", "NPI Downloadable Update", "Healthcare Common Procedure Code", "Drugs Prescribed", "Medical School"})
        cboSearchType.Location = New Point(133, 31)
        cboSearchType.Name = "cboSearchType"
        cboSearchType.Size = New Size(271, 23)
        cboSearchType.TabIndex = 1
        ' 
        ' groupPersonal
        ' 
        groupPersonal.BackColor = Color.DarkTurquoise
        groupPersonal.Controls.Add(ckExact)
        groupPersonal.Controls.Add(rdoFemale)
        groupPersonal.Controls.Add(rdoMale)
        groupPersonal.Controls.Add(cboTaxonomy)
        groupPersonal.Controls.Add(lblNPI)
        groupPersonal.Controls.Add(lblTax)
        groupPersonal.Controls.Add(lblFirst)
        groupPersonal.Controls.Add(lblMiddle)
        groupPersonal.Controls.Add(lblLast)
        groupPersonal.Controls.Add(lblState)
        groupPersonal.Controls.Add(tbNpi)
        groupPersonal.Controls.Add(tbFirst)
        groupPersonal.Controls.Add(tbMiddle)
        groupPersonal.Controls.Add(tbLast)
        groupPersonal.Controls.Add(tbState)
        groupPersonal.Controls.Add(lblGender)
        groupPersonal.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        groupPersonal.Location = New Point(8, 78)
        groupPersonal.Name = "groupPersonal"
        groupPersonal.Size = New Size(396, 330)
        groupPersonal.TabIndex = 45
        groupPersonal.TabStop = False
        groupPersonal.Text = "Personal Information"
        ' 
        ' ckExact
        ' 
        ckExact.AutoSize = True
        ckExact.Location = New Point(296, 305)
        ckExact.Name = "ckExact"
        ckExact.Size = New Size(94, 19)
        ckExact.TabIndex = 39
        ckExact.Text = "Exact Match"
        ckExact.UseVisualStyleBackColor = True
        ' 
        ' rdoFemale
        ' 
        rdoFemale.AutoSize = True
        rdoFemale.Location = New Point(140, 233)
        rdoFemale.Name = "rdoFemale"
        rdoFemale.Size = New Size(65, 19)
        rdoFemale.TabIndex = 38
        rdoFemale.Text = "Female"
        rdoFemale.UseVisualStyleBackColor = True
        ' 
        ' rdoMale
        ' 
        rdoMale.AutoSize = True
        rdoMale.Checked = True
        rdoMale.Location = New Point(211, 233)
        rdoMale.Name = "rdoMale"
        rdoMale.Size = New Size(52, 19)
        rdoMale.TabIndex = 37
        rdoMale.TabStop = True
        rdoMale.Text = "Male"
        rdoMale.UseVisualStyleBackColor = True
        ' 
        ' cboTaxonomy
        ' 
        cboTaxonomy.DropDownHeight = 750
        cboTaxonomy.FormattingEnabled = True
        cboTaxonomy.IntegralHeight = False
        cboTaxonomy.Items.AddRange(New Object() {"Addiction Medicine", "Advanced Practice Midwife", "Allergy & Immunology", "Allergy Immunology", "Anesthesiologist Assistant", "Anesthesiology", "Audiologist", "Audiologist-Hearing Aid Fitter", "Caridac Surgery", "Cardiology", "Cardiovascualar Disease (Cardiology)", "Case Manager/Care Coordinator", "Certified Clinical Nurse Specialist", "Certified Nurse Midwife", "Certified Registered Nurse Assistant (CRNA)", "Chiropractic", "Chiropractor", "Clinical Neuropsychologist", "Clinical Nurse Specialist", "Clinical Pharmacology", "Clinical Psychologist", "Clinical Social Worker", "Colon & Rectal Surgery", "Colorectal Surgery", "Community Health Worker", "Critical Care (Intensivists)", "Dentist", "Dermatology", "Diagnostic Radiology", "Drama Therapist", "Electrodiagnostic Medicine", "Emergency Medical Technician, Basic", "Emergency Medical Technician, Intermediate", "Emergency Medical Technician, Paramedic", "Emergency Medicine", "Endocrinology", "Family Medicine", "Family Practice", "Funeral Director", "Gastroenterology", "General Practice", "General Surgery", "Genetic Counselor, MS", "Geriatric Medicine", "Gynecological/Oncology", "Hand Surgery", "Health & Wellness Coach", "Health Educator", "Hematology", "Hematology/Oncology", "Homeopath", "Hospitalist", "Independent Medical Examiner", "Infectious Disease", "Integrative Medicine", "Internal Medicine", "Interpreter", "Interventional Pain Management", "Interventional Radiology", "Lactation Consultant, Non-RN", "Legal Medicine", "Licensed Clinical Social Worker", "Marriage & Family Therapist", "Maxillofacial Surgery", "Mechanotherapist", "Medical Genetics", "Medical Genetics, Ph.D. Medical Genetics", "Medical Oncology", "Midwife", "Midwife, Lay", "Military Health Care Provider", "Multi-Specialty", "Naprapath", "Naturopath", "Nephrology", "Neurological Surgery", "Neurology", "Neuromusculoskeletal Medicine & OMM", "Neuromusculoskeletal Medicine, Sports Medicine", "Neuropsychiatry", "Neurosurgery", "Nuclear Medicine", "Nurse Anesthetist, Certified Registered", "Nurse Practitioner", "Obstetrics/Gynecology", "Occupational Therapist", "Occupational Therapy Assistant", "Ophthalmology", "Optometrist", "Optometry", "Oral & Maxillofacial Surgery", "Oral Surgery (dental only)", "Orthopaedic Surgery", "Orthopedic Surgery", "Osteopathic Manipulative", "Otolaryngology", "Pain Management", "Pain Medicine", "Pathology", "Pediatric Medicine", "Pediatrics", "Peer Specialist", "Personal Emergency Response Attendant", "Phlebology", "Physical Medicine and Rehabilitation", "Physical Therapist", "Physical Therapist in Private Practice", "Physician Assistant", "Plastic and Reconstructive Surgery", "Plastic Surgery", "Podiatrist", "Podiatry", "Poetry Therapist", "Prevention Professional", "Preventive Medicine", "Psychiatry", "Psychiatry & Neurology", "Psychoanalyst", "Psychologist", "Pulmonary Disease", "Radiation Oncology", "Radiology", "Reflexologist", "Registered Dietitian/Nutrition Professional", "Rhuematology", "Single Specialty", "Sleep Specialist, PhD", "Social Worker", "Specialist", "Student in an Organized Health Care Education/Training Program", "Surgery", "Surgical Oncology", "Therapy (OMM)", "Thoracic Surgery", "Thoracic Surgery (Cardiothoracic Vascular Surgery)", "Transplant Surgery", "Urology", "Vascular Surgery"})
        cboTaxonomy.Location = New Point(140, 59)
        cboTaxonomy.Name = "cboTaxonomy"
        cboTaxonomy.Size = New Size(233, 23)
        cboTaxonomy.TabIndex = 36
        ' 
        ' lblNPI
        ' 
        lblNPI.AutoSize = True
        lblNPI.Location = New Point(6, 31)
        lblNPI.Name = "lblNPI"
        lblNPI.Size = New Size(76, 15)
        lblNPI.TabIndex = 0
        lblNPI.Text = "NPI Number"
        ' 
        ' lblTax
        ' 
        lblTax.AutoSize = True
        lblTax.Location = New Point(6, 66)
        lblTax.Name = "lblTax"
        lblTax.Size = New Size(119, 15)
        lblTax.TabIndex = 1
        lblTax.Text = "Taxonomy/Specialty"
        ' 
        ' lblFirst
        ' 
        lblFirst.AutoSize = True
        lblFirst.Location = New Point(4, 99)
        lblFirst.Name = "lblFirst"
        lblFirst.Size = New Size(67, 15)
        lblFirst.TabIndex = 2
        lblFirst.Text = "First Name"
        ' 
        ' lblMiddle
        ' 
        lblMiddle.AutoSize = True
        lblMiddle.Location = New Point(4, 134)
        lblMiddle.Name = "lblMiddle"
        lblMiddle.Size = New Size(81, 15)
        lblMiddle.TabIndex = 3
        lblMiddle.Text = "Middle Name"
        ' 
        ' lblLast
        ' 
        lblLast.AutoSize = True
        lblLast.Location = New Point(4, 169)
        lblLast.Name = "lblLast"
        lblLast.Size = New Size(65, 15)
        lblLast.TabIndex = 4
        lblLast.Text = "Last Name"
        ' 
        ' lblState
        ' 
        lblState.AutoSize = True
        lblState.Location = New Point(4, 198)
        lblState.Name = "lblState"
        lblState.Size = New Size(37, 15)
        lblState.TabIndex = 7
        lblState.Text = "State"
        ' 
        ' tbNpi
        ' 
        tbNpi.Location = New Point(140, 27)
        tbNpi.Name = "tbNpi"
        tbNpi.Size = New Size(89, 23)
        tbNpi.TabIndex = 11
        ' 
        ' tbFirst
        ' 
        tbFirst.Location = New Point(140, 92)
        tbFirst.Name = "tbFirst"
        tbFirst.Size = New Size(180, 23)
        tbFirst.TabIndex = 13
        ' 
        ' tbMiddle
        ' 
        tbMiddle.Location = New Point(140, 128)
        tbMiddle.Name = "tbMiddle"
        tbMiddle.Size = New Size(180, 23)
        tbMiddle.TabIndex = 14
        ' 
        ' tbLast
        ' 
        tbLast.Location = New Point(140, 163)
        tbLast.Name = "tbLast"
        tbLast.Size = New Size(180, 23)
        tbLast.TabIndex = 15
        ' 
        ' tbState
        ' 
        tbState.Location = New Point(140, 192)
        tbState.Name = "tbState"
        tbState.Size = New Size(40, 23)
        tbState.TabIndex = 18
        ' 
        ' lblGender
        ' 
        lblGender.AutoSize = True
        lblGender.Location = New Point(4, 233)
        lblGender.Name = "lblGender"
        lblGender.Size = New Size(49, 15)
        lblGender.TabIndex = 10
        lblGender.Text = "Gender"
        ' 
        ' btnSearch
        ' 
        btnSearch.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnSearch.BackColor = Color.Green
        btnSearch.Font = New Font("Times New Roman", 12F, FontStyle.Bold)
        btnSearch.Location = New Point(363, 440)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(167, 77)
        btnSearch.TabIndex = 37
        btnSearch.Text = "Search!"
        btnSearch.UseVisualStyleBackColor = False
        ' 
        ' btnOrgSearch
        ' 
        btnOrgSearch.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnOrgSearch.Font = New Font("Times New Roman", 12F, FontStyle.Bold)
        btnOrgSearch.Location = New Point(560, 440)
        btnOrgSearch.Name = "btnOrgSearch"
        btnOrgSearch.Size = New Size(166, 77)
        btnOrgSearch.TabIndex = 38
        btnOrgSearch.Text = "Go to Organizations Search"
        btnOrgSearch.UseVisualStyleBackColor = True
        ' 
        ' groupAddress
        ' 
        groupAddress.BackColor = Color.LimeGreen
        groupAddress.Controls.Add(Label21)
        groupAddress.Controls.Add(tbAddressState)
        groupAddress.Controls.Add(tbAddressType)
        groupAddress.Controls.Add(Label6)
        groupAddress.Controls.Add(Label7)
        groupAddress.Controls.Add(Label9)
        groupAddress.Controls.Add(Label10)
        groupAddress.Controls.Add(tbAddress)
        groupAddress.Controls.Add(tbCity)
        groupAddress.Controls.Add(tbZip)
        groupAddress.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        groupAddress.Location = New Point(420, 80)
        groupAddress.Name = "groupAddress"
        groupAddress.Size = New Size(396, 203)
        groupAddress.TabIndex = 44
        groupAddress.TabStop = False
        groupAddress.Text = "Address Info"
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Location = New Point(13, 159)
        Label21.Name = "Label21"
        Label21.Size = New Size(37, 15)
        Label21.TabIndex = 21
        Label21.Text = "State"
        ' 
        ' tbAddressState
        ' 
        tbAddressState.Location = New Point(154, 158)
        tbAddressState.Name = "tbAddressState"
        tbAddressState.Size = New Size(40, 23)
        tbAddressState.TabIndex = 22
        ' 
        ' tbAddressType
        ' 
        tbAddressType.Location = New Point(154, 126)
        tbAddressType.Name = "tbAddressType"
        tbAddressType.Size = New Size(182, 23)
        tbAddressType.TabIndex = 20
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(13, 24)
        Label6.Name = "Label6"
        Label6.Size = New Size(90, 15)
        Label6.TabIndex = 5
        Label6.Text = "Street Address"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(13, 60)
        Label7.Name = "Label7"
        Label7.Size = New Size(28, 15)
        Label7.TabIndex = 6
        Label7.Text = "City"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(13, 93)
        Label9.Name = "Label9"
        Label9.Size = New Size(55, 15)
        Label9.TabIndex = 8
        Label9.Text = "Zip Code"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(13, 129)
        Label10.Name = "Label10"
        Label10.Size = New Size(80, 15)
        Label10.TabIndex = 9
        Label10.Text = "Address Type"
        ' 
        ' tbAddress
        ' 
        tbAddress.Location = New Point(154, 20)
        tbAddress.Name = "tbAddress"
        tbAddress.Size = New Size(180, 23)
        tbAddress.TabIndex = 16
        ' 
        ' tbCity
        ' 
        tbCity.Location = New Point(154, 58)
        tbCity.Name = "tbCity"
        tbCity.Size = New Size(182, 23)
        tbCity.TabIndex = 17
        ' 
        ' tbZip
        ' 
        tbZip.Location = New Point(154, 91)
        tbZip.Name = "tbZip"
        tbZip.Size = New Size(182, 23)
        tbZip.TabIndex = 19
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        Button1.BackColor = Color.Red
        Button1.Font = New Font("Times New Roman", 12F, FontStyle.Bold)
        Button1.Location = New Point(751, 440)
        Button1.Name = "Button1"
        Button1.Size = New Size(167, 77)
        Button1.TabIndex = 39
        Button1.Text = "Clear"
        Button1.UseVisualStyleBackColor = False
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
        GroupBox1.PerformLayout()
        groupMedSchool.ResumeLayout(False)
        groupMedSchool.PerformLayout()
        groupMeds.ResumeLayout(False)
        groupMeds.PerformLayout()
        groupBoxOther.ResumeLayout(False)
        groupBoxOther.PerformLayout()
        groupPersonal.ResumeLayout(False)
        groupPersonal.PerformLayout()
        groupAddress.ResumeLayout(False)
        groupAddress.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblGender As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents lblState As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents lblLast As Label
    Friend WithEvents lblMiddle As Label
    Friend WithEvents lblFirst As Label
    Friend WithEvents lblTax As Label
    Friend WithEvents lblNPI As Label
    Friend WithEvents tbStLicNum As TextBox
    Friend WithEvents tbStLic As TextBox
    Friend WithEvents tbHCPCS As TextBox
    Friend WithEvents tbAddressType As TextBox
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
    Friend WithEvents groupPersonal As GroupBox
    Friend WithEvents groupAddress As GroupBox
    Friend WithEvents groupBoxOther As GroupBox
    Friend WithEvents cboTaxonomy As ComboBox
    Friend WithEvents rdoFemale As RadioButton
    Friend WithEvents rdoMale As RadioButton
    Friend WithEvents ckExact As CheckBox
    Friend WithEvents cboSearchType As ComboBox
    Friend WithEvents Label19 As Label
    Friend WithEvents groupMeds As GroupBox
    Friend WithEvents tbDrugGeneric As TextBox
    Friend WithEvents lblBrand As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents tbDrug As TextBox
    Friend WithEvents groupMedSchool As GroupBox
    Friend WithEvents Label21 As Label
    Friend WithEvents tbAddressState As TextBox
    Friend WithEvents cboMedSchoolTax As ComboBox
    Friend WithEvents Label22 As Label
    'Friend WithEvents tbTS As ComboBox
End Class

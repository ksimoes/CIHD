<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IndividualProfileForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IndividualProfileForm))
        Panel1 = New Panel()
        TabControl1 = New TabControl()
        tbFacil = New TabPage()
        dgvAff = New DataGridView()
        tbDrugs = New TabPage()
        dgvDrugs = New DataGridView()
        tbHCPCS1 = New TabPage()
        dgvHCPCSlvl1 = New DataGridView()
        tbHCPCS2 = New TabPage()
        dgvHCPCSlvl2 = New DataGridView()
        tbTaxonomy = New TabPage()
        dgvTax = New DataGridView()
        tbPayment = New TabPage()
        dgvGenPay = New DataGridView()
        tbOwnership = New TabPage()
        dgvOwner = New DataGridView()
        tbResearchPay = New TabPage()
        dgvResearch = New DataGridView()
        Button1 = New Button()
        dgvMain = New DataGridView()
        GroupBox1 = New GroupBox()
        GroupBox2 = New GroupBox()
        Label5 = New Label()
        Street = New Label()
        lblMailingZip = New Label()
        lblmailc = New Label()
        lblMailingState = New Label()
        lblMailingStreet = New Label()
        lblMailingCity = New Label()
        Label9 = New Label()
        groupPrimAdd = New GroupBox()
        S = New Label()
        Label6 = New Label()
        Label7 = New Label()
        Label8 = New Label()
        lblPCity = New Label()
        lblPracticeState = New Label()
        lblPracticeZip = New Label()
        lblPracticeStreet = New Label()
        Label17 = New Label()
        lblFacility = New Label()
        lblGradYear = New Label()
        lblg = New Label()
        lblMedSchool = New Label()
        Label16 = New Label()
        lblPhone = New Label()
        Label13 = New Label()
        lblLicNum = New Label()
        lbllic = New Label()
        lblTax = New Label()
        Label12 = New Label()
        lblGender = New Label()
        lblLast = New Label()
        lblMiddle = New Label()
        lblFirst = New Label()
        tbNpiResult = New TextBox()
        Label11 = New Label()
        Label10 = New Label()
        Label4 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Panel1.SuspendLayout()
        TabControl1.SuspendLayout()
        tbFacil.SuspendLayout()
        CType(dgvAff, ComponentModel.ISupportInitialize).BeginInit()
        tbDrugs.SuspendLayout()
        CType(dgvDrugs, ComponentModel.ISupportInitialize).BeginInit()
        tbHCPCS1.SuspendLayout()
        CType(dgvHCPCSlvl1, ComponentModel.ISupportInitialize).BeginInit()
        tbHCPCS2.SuspendLayout()
        CType(dgvHCPCSlvl2, ComponentModel.ISupportInitialize).BeginInit()
        tbTaxonomy.SuspendLayout()
        CType(dgvTax, ComponentModel.ISupportInitialize).BeginInit()
        tbPayment.SuspendLayout()
        CType(dgvGenPay, ComponentModel.ISupportInitialize).BeginInit()
        tbOwnership.SuspendLayout()
        CType(dgvOwner, ComponentModel.ISupportInitialize).BeginInit()
        tbResearchPay.SuspendLayout()
        CType(dgvResearch, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvMain, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox1.SuspendLayout()
        GroupBox2.SuspendLayout()
        groupPrimAdd.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), Image)
        Panel1.BackgroundImageLayout = ImageLayout.Stretch
        Panel1.Controls.Add(TabControl1)
        Panel1.Controls.Add(Button1)
        Panel1.Controls.Add(dgvMain)
        Panel1.Controls.Add(GroupBox1)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(977, 644)
        Panel1.TabIndex = 0
        ' 
        ' TabControl1
        ' 
        TabControl1.Controls.Add(tbFacil)
        TabControl1.Controls.Add(tbDrugs)
        TabControl1.Controls.Add(tbHCPCS1)
        TabControl1.Controls.Add(tbHCPCS2)
        TabControl1.Controls.Add(tbTaxonomy)
        TabControl1.Controls.Add(tbPayment)
        TabControl1.Controls.Add(tbOwnership)
        TabControl1.Controls.Add(tbResearchPay)
        TabControl1.Location = New Point(12, 362)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(922, 256)
        TabControl1.TabIndex = 29
        ' 
        ' tbFacil
        ' 
        tbFacil.Controls.Add(dgvAff)
        tbFacil.Location = New Point(4, 24)
        tbFacil.Name = "tbFacil"
        tbFacil.Padding = New Padding(3)
        tbFacil.Size = New Size(914, 228)
        tbFacil.TabIndex = 0
        tbFacil.Text = "Facility Affiliation"
        tbFacil.UseVisualStyleBackColor = True
        ' 
        ' dgvAff
        ' 
        dgvAff.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAff.Location = New Point(16, 12)
        dgvAff.Name = "dgvAff"
        dgvAff.Size = New Size(866, 200)
        dgvAff.TabIndex = 11
        ' 
        ' tbDrugs
        ' 
        tbDrugs.Controls.Add(dgvDrugs)
        tbDrugs.Location = New Point(4, 24)
        tbDrugs.Name = "tbDrugs"
        tbDrugs.Padding = New Padding(3)
        tbDrugs.Size = New Size(914, 228)
        tbDrugs.TabIndex = 1
        tbDrugs.Text = "Drugs"
        tbDrugs.UseVisualStyleBackColor = True
        ' 
        ' dgvDrugs
        ' 
        dgvDrugs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDrugs.Location = New Point(15, 22)
        dgvDrugs.Name = "dgvDrugs"
        dgvDrugs.Size = New Size(879, 180)
        dgvDrugs.TabIndex = 13
        ' 
        ' tbHCPCS1
        ' 
        tbHCPCS1.Controls.Add(dgvHCPCSlvl1)
        tbHCPCS1.Location = New Point(4, 24)
        tbHCPCS1.Name = "tbHCPCS1"
        tbHCPCS1.Size = New Size(914, 228)
        tbHCPCS1.TabIndex = 2
        tbHCPCS1.Text = "HCPCS LV1"
        tbHCPCS1.UseVisualStyleBackColor = True
        ' 
        ' dgvHCPCSlvl1
        ' 
        dgvHCPCSlvl1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvHCPCSlvl1.Location = New Point(23, 17)
        dgvHCPCSlvl1.Name = "dgvHCPCSlvl1"
        dgvHCPCSlvl1.Size = New Size(870, 192)
        dgvHCPCSlvl1.TabIndex = 16
        ' 
        ' tbHCPCS2
        ' 
        tbHCPCS2.Controls.Add(dgvHCPCSlvl2)
        tbHCPCS2.Location = New Point(4, 24)
        tbHCPCS2.Name = "tbHCPCS2"
        tbHCPCS2.Size = New Size(914, 228)
        tbHCPCS2.TabIndex = 3
        tbHCPCS2.Text = "HCPCS LV2"
        tbHCPCS2.UseVisualStyleBackColor = True
        ' 
        ' dgvHCPCSlvl2
        ' 
        dgvHCPCSlvl2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvHCPCSlvl2.Location = New Point(16, 16)
        dgvHCPCSlvl2.Name = "dgvHCPCSlvl2"
        dgvHCPCSlvl2.Size = New Size(874, 194)
        dgvHCPCSlvl2.TabIndex = 20
        ' 
        ' tbTaxonomy
        ' 
        tbTaxonomy.Controls.Add(dgvTax)
        tbTaxonomy.Location = New Point(4, 24)
        tbTaxonomy.Name = "tbTaxonomy"
        tbTaxonomy.Size = New Size(914, 228)
        tbTaxonomy.TabIndex = 4
        tbTaxonomy.Text = "Taxonomy(s)"
        tbTaxonomy.UseVisualStyleBackColor = True
        ' 
        ' dgvTax
        ' 
        dgvTax.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvTax.Location = New Point(16, 24)
        dgvTax.Name = "dgvTax"
        dgvTax.Size = New Size(879, 183)
        dgvTax.TabIndex = 21
        ' 
        ' tbPayment
        ' 
        tbPayment.Controls.Add(dgvGenPay)
        tbPayment.Location = New Point(4, 24)
        tbPayment.Name = "tbPayment"
        tbPayment.Size = New Size(914, 228)
        tbPayment.TabIndex = 5
        tbPayment.Text = "General Payment"
        tbPayment.UseVisualStyleBackColor = True
        ' 
        ' dgvGenPay
        ' 
        dgvGenPay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvGenPay.Location = New Point(16, 13)
        dgvGenPay.Name = "dgvGenPay"
        dgvGenPay.Size = New Size(871, 199)
        dgvGenPay.TabIndex = 23
        ' 
        ' tbOwnership
        ' 
        tbOwnership.Controls.Add(dgvOwner)
        tbOwnership.Location = New Point(4, 24)
        tbOwnership.Name = "tbOwnership"
        tbOwnership.Size = New Size(914, 228)
        tbOwnership.TabIndex = 6
        tbOwnership.Text = "Ownership"
        tbOwnership.UseVisualStyleBackColor = True
        ' 
        ' dgvOwner
        ' 
        dgvOwner.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvOwner.Location = New Point(21, 17)
        dgvOwner.Name = "dgvOwner"
        dgvOwner.Size = New Size(870, 198)
        dgvOwner.TabIndex = 25
        ' 
        ' tbResearchPay
        ' 
        tbResearchPay.Controls.Add(dgvResearch)
        tbResearchPay.Location = New Point(4, 24)
        tbResearchPay.Name = "tbResearchPay"
        tbResearchPay.Size = New Size(914, 228)
        tbResearchPay.TabIndex = 7
        tbResearchPay.Text = "Research Payment"
        tbResearchPay.UseVisualStyleBackColor = True
        ' 
        ' dgvResearch
        ' 
        dgvResearch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvResearch.Location = New Point(18, 14)
        dgvResearch.Name = "dgvResearch"
        dgvResearch.Size = New Size(870, 200)
        dgvResearch.TabIndex = 18
        ' 
        ' Button1
        ' 
        Button1.BackgroundImage = CType(resources.GetObject("Button1.BackgroundImage"), Image)
        Button1.BackgroundImageLayout = ImageLayout.Stretch
        Button1.Location = New Point(770, 279)
        Button1.Name = "Button1"
        Button1.Size = New Size(164, 88)
        Button1.TabIndex = 28
        Button1.Text = "Back to Search!"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' dgvMain
        ' 
        dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvMain.Location = New Point(674, 12)
        dgvMain.Name = "dgvMain"
        dgvMain.Size = New Size(256, 230)
        dgvMain.TabIndex = 15
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(GroupBox2)
        GroupBox1.Controls.Add(groupPrimAdd)
        GroupBox1.Controls.Add(Label17)
        GroupBox1.Controls.Add(lblFacility)
        GroupBox1.Controls.Add(lblGradYear)
        GroupBox1.Controls.Add(lblg)
        GroupBox1.Controls.Add(lblMedSchool)
        GroupBox1.Controls.Add(Label16)
        GroupBox1.Controls.Add(lblPhone)
        GroupBox1.Controls.Add(Label13)
        GroupBox1.Controls.Add(lblLicNum)
        GroupBox1.Controls.Add(lbllic)
        GroupBox1.Controls.Add(lblTax)
        GroupBox1.Controls.Add(Label12)
        GroupBox1.Controls.Add(lblGender)
        GroupBox1.Controls.Add(lblLast)
        GroupBox1.Controls.Add(lblMiddle)
        GroupBox1.Controls.Add(lblFirst)
        GroupBox1.Controls.Add(tbNpiResult)
        GroupBox1.Controls.Add(Label11)
        GroupBox1.Controls.Add(Label10)
        GroupBox1.Controls.Add(Label4)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Location = New Point(12, 12)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(650, 333)
        GroupBox1.TabIndex = 10
        GroupBox1.TabStop = False
        GroupBox1.Text = "Individual Profile"
        ' 
        ' GroupBox2
        ' 
        GroupBox2.BackColor = SystemColors.ActiveCaption
        GroupBox2.BackgroundImage = CType(resources.GetObject("GroupBox2.BackgroundImage"), Image)
        GroupBox2.BackgroundImageLayout = ImageLayout.Stretch
        GroupBox2.Controls.Add(Label5)
        GroupBox2.Controls.Add(Street)
        GroupBox2.Controls.Add(lblMailingZip)
        GroupBox2.Controls.Add(lblmailc)
        GroupBox2.Controls.Add(lblMailingState)
        GroupBox2.Controls.Add(lblMailingStreet)
        GroupBox2.Controls.Add(lblMailingCity)
        GroupBox2.Controls.Add(Label9)
        GroupBox2.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        GroupBox2.Location = New Point(357, 185)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(272, 140)
        GroupBox2.TabIndex = 42
        GroupBox2.TabStop = False
        GroupBox2.Text = "Mailing Address"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.BackColor = Color.Transparent
        Label5.Font = New Font("Segoe UI", 9F, FontStyle.Underline)
        Label5.ForeColor = SystemColors.ControlLightLight
        Label5.Location = New Point(22, 82)
        Label5.Name = "Label5"
        Label5.Size = New Size(33, 15)
        Label5.TabIndex = 36
        Label5.Text = "State"
        ' 
        ' Street
        ' 
        Street.AutoSize = True
        Street.BackColor = Color.Transparent
        Street.Font = New Font("Segoe UI", 9F, FontStyle.Underline)
        Street.ForeColor = SystemColors.ControlLightLight
        Street.Location = New Point(22, 30)
        Street.Name = "Street"
        Street.Size = New Size(37, 15)
        Street.TabIndex = 4
        Street.Text = "Street"
        ' 
        ' lblMailingZip
        ' 
        lblMailingZip.AutoSize = True
        lblMailingZip.BackColor = Color.Transparent
        lblMailingZip.ForeColor = SystemColors.ControlLightLight
        lblMailingZip.Location = New Point(104, 108)
        lblMailingZip.Name = "lblMailingZip"
        lblMailingZip.Size = New Size(10, 15)
        lblMailingZip.TabIndex = 40
        lblMailingZip.Text = "."
        ' 
        ' lblmailc
        ' 
        lblmailc.AutoSize = True
        lblmailc.BackColor = Color.Transparent
        lblmailc.Font = New Font("Segoe UI", 9F, FontStyle.Underline)
        lblmailc.ForeColor = SystemColors.ControlLightLight
        lblmailc.Location = New Point(22, 56)
        lblmailc.Name = "lblmailc"
        lblmailc.Size = New Size(28, 15)
        lblmailc.TabIndex = 8
        lblmailc.Text = "City"
        ' 
        ' lblMailingState
        ' 
        lblMailingState.AutoSize = True
        lblMailingState.BackColor = Color.Transparent
        lblMailingState.ForeColor = SystemColors.ControlLightLight
        lblMailingState.Location = New Point(104, 84)
        lblMailingState.Name = "lblMailingState"
        lblMailingState.Size = New Size(10, 15)
        lblMailingState.TabIndex = 39
        lblMailingState.Text = "."
        ' 
        ' lblMailingStreet
        ' 
        lblMailingStreet.AutoSize = True
        lblMailingStreet.BackColor = Color.Transparent
        lblMailingStreet.ForeColor = SystemColors.ControlLightLight
        lblMailingStreet.Location = New Point(104, 30)
        lblMailingStreet.Name = "lblMailingStreet"
        lblMailingStreet.Size = New Size(10, 15)
        lblMailingStreet.TabIndex = 15
        lblMailingStreet.Text = "."
        ' 
        ' lblMailingCity
        ' 
        lblMailingCity.AutoSize = True
        lblMailingCity.BackColor = Color.Transparent
        lblMailingCity.ForeColor = SystemColors.ControlLightLight
        lblMailingCity.Location = New Point(104, 56)
        lblMailingCity.Name = "lblMailingCity"
        lblMailingCity.Size = New Size(10, 15)
        lblMailingCity.TabIndex = 19
        lblMailingCity.Text = "."
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.BackColor = Color.Transparent
        Label9.Font = New Font("Segoe UI", 9F, FontStyle.Underline)
        Label9.ForeColor = SystemColors.ControlLightLight
        Label9.Location = New Point(22, 108)
        Label9.Name = "Label9"
        Label9.Size = New Size(55, 15)
        Label9.TabIndex = 37
        Label9.Text = "Zip Code"
        ' 
        ' groupPrimAdd
        ' 
        groupPrimAdd.BackgroundImage = CType(resources.GetObject("groupPrimAdd.BackgroundImage"), Image)
        groupPrimAdd.BackgroundImageLayout = ImageLayout.Stretch
        groupPrimAdd.Controls.Add(S)
        groupPrimAdd.Controls.Add(Label6)
        groupPrimAdd.Controls.Add(Label7)
        groupPrimAdd.Controls.Add(Label8)
        groupPrimAdd.Controls.Add(lblPCity)
        groupPrimAdd.Controls.Add(lblPracticeState)
        groupPrimAdd.Controls.Add(lblPracticeZip)
        groupPrimAdd.Controls.Add(lblPracticeStreet)
        groupPrimAdd.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        groupPrimAdd.Location = New Point(357, 22)
        groupPrimAdd.Name = "groupPrimAdd"
        groupPrimAdd.Size = New Size(272, 126)
        groupPrimAdd.TabIndex = 41
        groupPrimAdd.TabStop = False
        groupPrimAdd.Text = "Primary Practice Address"
        ' 
        ' S
        ' 
        S.AutoSize = True
        S.BackColor = Color.Transparent
        S.Font = New Font("Segoe UI", 9F, FontStyle.Underline)
        S.ForeColor = SystemColors.ButtonHighlight
        S.Location = New Point(22, 19)
        S.Name = "S"
        S.Size = New Size(37, 15)
        S.TabIndex = 35
        S.Text = "Street"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.BackColor = Color.Transparent
        Label6.Font = New Font("Segoe UI", 9F, FontStyle.Underline)
        Label6.ForeColor = SystemColors.ButtonHighlight
        Label6.Location = New Point(22, 45)
        Label6.Name = "Label6"
        Label6.Size = New Size(28, 15)
        Label6.TabIndex = 5
        Label6.Text = "City"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.BackColor = Color.Transparent
        Label7.Font = New Font("Segoe UI", 9F, FontStyle.Underline)
        Label7.ForeColor = SystemColors.ButtonHighlight
        Label7.Location = New Point(22, 71)
        Label7.Name = "Label7"
        Label7.Size = New Size(33, 15)
        Label7.TabIndex = 6
        Label7.Text = "State"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.BackColor = Color.Transparent
        Label8.Font = New Font("Segoe UI", 9F, FontStyle.Underline)
        Label8.ForeColor = SystemColors.ButtonHighlight
        Label8.Location = New Point(22, 97)
        Label8.Name = "Label8"
        Label8.Size = New Size(55, 15)
        Label8.TabIndex = 7
        Label8.Text = "Zip Code"
        ' 
        ' lblPCity
        ' 
        lblPCity.AutoSize = True
        lblPCity.BackColor = Color.Transparent
        lblPCity.ForeColor = SystemColors.ButtonHighlight
        lblPCity.Location = New Point(104, 45)
        lblPCity.Name = "lblPCity"
        lblPCity.Size = New Size(10, 15)
        lblPCity.TabIndex = 16
        lblPCity.Text = "."
        ' 
        ' lblPracticeState
        ' 
        lblPracticeState.AutoSize = True
        lblPracticeState.BackColor = Color.Transparent
        lblPracticeState.ForeColor = SystemColors.ButtonHighlight
        lblPracticeState.Location = New Point(104, 71)
        lblPracticeState.Name = "lblPracticeState"
        lblPracticeState.Size = New Size(10, 15)
        lblPracticeState.TabIndex = 17
        lblPracticeState.Text = "."
        ' 
        ' lblPracticeZip
        ' 
        lblPracticeZip.AutoSize = True
        lblPracticeZip.BackColor = Color.Transparent
        lblPracticeZip.ForeColor = SystemColors.ButtonHighlight
        lblPracticeZip.Location = New Point(104, 97)
        lblPracticeZip.Name = "lblPracticeZip"
        lblPracticeZip.Size = New Size(10, 15)
        lblPracticeZip.TabIndex = 18
        lblPracticeZip.Text = "."
        ' 
        ' lblPracticeStreet
        ' 
        lblPracticeStreet.AutoSize = True
        lblPracticeStreet.BackColor = Color.Transparent
        lblPracticeStreet.ForeColor = SystemColors.ButtonHighlight
        lblPracticeStreet.Location = New Point(104, 19)
        lblPracticeStreet.Name = "lblPracticeStreet"
        lblPracticeStreet.Size = New Size(10, 15)
        lblPracticeStreet.TabIndex = 34
        lblPracticeStreet.Text = "."
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label17.Location = New Point(3, 289)
        Label17.Name = "Label17"
        Label17.Size = New Size(45, 15)
        Label17.TabIndex = 32
        Label17.Text = "Facility"
        ' 
        ' lblFacility
        ' 
        lblFacility.AutoSize = True
        lblFacility.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblFacility.Location = New Point(138, 289)
        lblFacility.Name = "lblFacility"
        lblFacility.Size = New Size(10, 15)
        lblFacility.TabIndex = 31
        lblFacility.Text = "."
        ' 
        ' lblGradYear
        ' 
        lblGradYear.AutoSize = True
        lblGradYear.Location = New Point(138, 263)
        lblGradYear.Name = "lblGradYear"
        lblGradYear.Size = New Size(10, 15)
        lblGradYear.TabIndex = 30
        lblGradYear.Text = "."
        ' 
        ' lblg
        ' 
        lblg.AutoSize = True
        lblg.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblg.Location = New Point(3, 263)
        lblg.Name = "lblg"
        lblg.Size = New Size(96, 15)
        lblg.TabIndex = 29
        lblg.Text = "Graduation Year"
        ' 
        ' lblMedSchool
        ' 
        lblMedSchool.AutoSize = True
        lblMedSchool.Location = New Point(138, 237)
        lblMedSchool.Name = "lblMedSchool"
        lblMedSchool.Size = New Size(10, 15)
        lblMedSchool.TabIndex = 28
        lblMedSchool.Text = "."
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label16.Location = New Point(3, 237)
        Label16.Name = "Label16"
        Label16.Size = New Size(90, 15)
        Label16.TabIndex = 27
        Label16.Text = "Medical School"
        ' 
        ' lblPhone
        ' 
        lblPhone.AutoSize = True
        lblPhone.Location = New Point(138, 211)
        lblPhone.Name = "lblPhone"
        lblPhone.Size = New Size(10, 15)
        lblPhone.TabIndex = 26
        lblPhone.Text = "."
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label13.Location = New Point(3, 211)
        Label13.Name = "Label13"
        Label13.Size = New Size(42, 15)
        Label13.TabIndex = 25
        Label13.Text = "Phone"
        ' 
        ' lblLicNum
        ' 
        lblLicNum.AutoSize = True
        lblLicNum.Location = New Point(138, 185)
        lblLicNum.Name = "lblLicNum"
        lblLicNum.Size = New Size(10, 15)
        lblLicNum.TabIndex = 24
        lblLicNum.Text = "."
        ' 
        ' lbllic
        ' 
        lbllic.AutoSize = True
        lbllic.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lbllic.Location = New Point(3, 185)
        lbllic.Name = "lbllic"
        lbllic.Size = New Size(97, 15)
        lbllic.TabIndex = 23
        lbllic.Text = "License Number"
        ' 
        ' lblTax
        ' 
        lblTax.AutoSize = True
        lblTax.Location = New Point(138, 159)
        lblTax.Name = "lblTax"
        lblTax.Size = New Size(10, 15)
        lblTax.TabIndex = 22
        lblTax.Text = "."
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label12.Location = New Point(3, 159)
        Label12.Name = "Label12"
        Label12.Size = New Size(119, 15)
        Label12.TabIndex = 21
        Label12.Text = "Taxonomy/Specialty"
        ' 
        ' lblGender
        ' 
        lblGender.AutoSize = True
        lblGender.Location = New Point(138, 133)
        lblGender.Name = "lblGender"
        lblGender.Size = New Size(10, 15)
        lblGender.TabIndex = 20
        lblGender.Text = "."
        ' 
        ' lblLast
        ' 
        lblLast.AutoSize = True
        lblLast.Location = New Point(138, 100)
        lblLast.Name = "lblLast"
        lblLast.Size = New Size(10, 15)
        lblLast.TabIndex = 14
        lblLast.Text = "."
        ' 
        ' lblMiddle
        ' 
        lblMiddle.AutoSize = True
        lblMiddle.Location = New Point(138, 74)
        lblMiddle.Name = "lblMiddle"
        lblMiddle.Size = New Size(10, 15)
        lblMiddle.TabIndex = 13
        lblMiddle.Text = "."
        ' 
        ' lblFirst
        ' 
        lblFirst.AutoSize = True
        lblFirst.Location = New Point(138, 48)
        lblFirst.Name = "lblFirst"
        lblFirst.Size = New Size(10, 15)
        lblFirst.TabIndex = 12
        lblFirst.Text = "."
        ' 
        ' tbNpiResult
        ' 
        tbNpiResult.Location = New Point(138, 19)
        tbNpiResult.Name = "tbNpiResult"
        tbNpiResult.Size = New Size(100, 23)
        tbNpiResult.TabIndex = 11
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label11.Location = New Point(3, 22)
        Label11.Name = "Label11"
        Label11.Size = New Size(76, 15)
        Label11.TabIndex = 10
        Label11.Text = "NPI Number"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label10.Location = New Point(3, 133)
        Label10.Name = "Label10"
        Label10.Size = New Size(49, 15)
        Label10.TabIndex = 9
        Label10.Text = "Gender"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label4.Location = New Point(3, 100)
        Label4.Name = "Label4"
        Label4.Size = New Size(65, 15)
        Label4.TabIndex = 3
        Label4.Text = "Last Name"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label3.Location = New Point(3, 74)
        Label3.Name = "Label3"
        Label3.Size = New Size(81, 15)
        Label3.TabIndex = 2
        Label3.Text = "Middle Name"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label2.Location = New Point(3, 48)
        Label2.Name = "Label2"
        Label2.Size = New Size(67, 15)
        Label2.TabIndex = 1
        Label2.Text = "First Name"
        ' 
        ' IndividualProfileForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(977, 644)
        Controls.Add(Panel1)
        Name = "IndividualProfileForm"
        Text = "IndividualProfileForm"
        Panel1.ResumeLayout(False)
        TabControl1.ResumeLayout(False)
        tbFacil.ResumeLayout(False)
        CType(dgvAff, ComponentModel.ISupportInitialize).EndInit()
        tbDrugs.ResumeLayout(False)
        CType(dgvDrugs, ComponentModel.ISupportInitialize).EndInit()
        tbHCPCS1.ResumeLayout(False)
        CType(dgvHCPCSlvl1, ComponentModel.ISupportInitialize).EndInit()
        tbHCPCS2.ResumeLayout(False)
        CType(dgvHCPCSlvl2, ComponentModel.ISupportInitialize).EndInit()
        tbTaxonomy.ResumeLayout(False)
        CType(dgvTax, ComponentModel.ISupportInitialize).EndInit()
        tbPayment.ResumeLayout(False)
        CType(dgvGenPay, ComponentModel.ISupportInitialize).EndInit()
        tbOwnership.ResumeLayout(False)
        CType(dgvOwner, ComponentModel.ISupportInitialize).EndInit()
        tbResearchPay.ResumeLayout(False)
        CType(dgvResearch, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvMain, ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        groupPrimAdd.ResumeLayout(False)
        groupPrimAdd.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents dgvResearch As DataGridView
    Friend WithEvents dgvHCPCSlvl1 As DataGridView
    Friend WithEvents dgvMain As DataGridView
    Friend WithEvents dgvDrugs As DataGridView
    Friend WithEvents dgvAff As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label17 As Label
    Friend WithEvents lblFacility As Label
    Friend WithEvents lblGradYear As Label
    Friend WithEvents lblg As Label
    Friend WithEvents lblMedSchool As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents lblPhone As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents lblLicNum As Label
    Friend WithEvents lbllic As Label
    Friend WithEvents lblTax As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents lblGender As Label
    Friend WithEvents lblMailingCity As Label
    Friend WithEvents lblPracticeZip As Label
    Friend WithEvents lblPracticeState As Label
    Friend WithEvents lblPCity As Label
    Friend WithEvents lblMailingStreet As Label
    Friend WithEvents lblLast As Label
    Friend WithEvents lblMiddle As Label
    Friend WithEvents lblFirst As Label
    Friend WithEvents tbNpiResult As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents lblmailc As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Street As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents dgvHCPCSlvl2 As DataGridView
    Friend WithEvents dgvGenPay As DataGridView
    Friend WithEvents dgvTax As DataGridView
    Friend WithEvents lblPracticeStreet As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents S As Label
    Friend WithEvents lblMailingZip As Label
    Friend WithEvents lblMailingState As Label
    Friend WithEvents dgvOwner As DataGridView
    Friend WithEvents Button1 As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tbFacil As TabPage
    Friend WithEvents tbDrugs As TabPage
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents groupPrimAdd As GroupBox
    Friend WithEvents tbHCPCS1 As TabPage
    Friend WithEvents tbHCPCS2 As TabPage
    Friend WithEvents tbTaxonomy As TabPage
    Friend WithEvents tbPayment As TabPage
    Friend WithEvents tbOwnership As TabPage
    Friend WithEvents tbResearchPay As TabPage
End Class

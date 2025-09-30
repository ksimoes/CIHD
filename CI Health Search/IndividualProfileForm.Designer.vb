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
        Panel1 = New Panel()
        Button1 = New Button()
        Label24 = New Label()
        Label1 = New Label()
        dgvOwner = New DataGridView()
        Label21 = New Label()
        dgvGenPay = New DataGridView()
        Label20 = New Label()
        dgvTax = New DataGridView()
        dgvHCPCSlvl2 = New DataGridView()
        Label19 = New Label()
        dgvResearch = New DataGridView()
        Label18 = New Label()
        dgvHCPCSlvl1 = New DataGridView()
        dgvMain = New DataGridView()
        Label15 = New Label()
        dgvDrugs = New DataGridView()
        Label14 = New Label()
        dgvAff = New DataGridView()
        GroupBox1 = New GroupBox()
        lblMailingZip = New Label()
        lblMailingState = New Label()
        Label23 = New Label()
        Label9 = New Label()
        Label5 = New Label()
        S = New Label()
        lblPracticeStreet = New Label()
        Label22 = New Label()
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
        lblMailingCity = New Label()
        lblPracticeZip = New Label()
        lblPracticeState = New Label()
        lblPCity = New Label()
        lblMailingStreet = New Label()
        lblLast = New Label()
        lblMiddle = New Label()
        lblFirst = New Label()
        tbNpiResult = New TextBox()
        Label11 = New Label()
        Label10 = New Label()
        lblmailc = New Label()
        Label8 = New Label()
        Label7 = New Label()
        Label6 = New Label()
        Street = New Label()
        Label4 = New Label()
        Label3 = New Label()
        Label2 = New Label()
        Panel1.SuspendLayout()
        CType(dgvOwner, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvGenPay, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvTax, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvHCPCSlvl2, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvResearch, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvHCPCSlvl1, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvMain, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvDrugs, ComponentModel.ISupportInitialize).BeginInit()
        CType(dgvAff, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.AutoScroll = True
        Panel1.Controls.Add(Button1)
        Panel1.Controls.Add(Label24)
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(dgvOwner)
        Panel1.Controls.Add(Label21)
        Panel1.Controls.Add(dgvGenPay)
        Panel1.Controls.Add(Label20)
        Panel1.Controls.Add(dgvTax)
        Panel1.Controls.Add(dgvHCPCSlvl2)
        Panel1.Controls.Add(Label19)
        Panel1.Controls.Add(dgvResearch)
        Panel1.Controls.Add(Label18)
        Panel1.Controls.Add(dgvHCPCSlvl1)
        Panel1.Controls.Add(dgvMain)
        Panel1.Controls.Add(Label15)
        Panel1.Controls.Add(dgvDrugs)
        Panel1.Controls.Add(Label14)
        Panel1.Controls.Add(dgvAff)
        Panel1.Controls.Add(GroupBox1)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1222, 1061)
        Panel1.TabIndex = 0
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(1022, 961)
        Button1.Name = "Button1"
        Button1.Size = New Size(164, 88)
        Button1.TabIndex = 28
        Button1.Text = "Back to Search!"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Label24
        ' 
        Label24.AutoSize = True
        Label24.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label24.Location = New Point(3, 1946)
        Label24.Name = "Label24"
        Label24.Size = New Size(110, 15)
        Label24.TabIndex = 27
        Label24.Text = "Research Payment"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label1.Location = New Point(3, 1750)
        Label1.Name = "Label1"
        Label1.Size = New Size(96, 15)
        Label1.TabIndex = 26
        Label1.Text = "Ownership Data"
        ' 
        ' dgvOwner
        ' 
        dgvOwner.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvOwner.Location = New Point(3, 1768)
        dgvOwner.Name = "dgvOwner"
        dgvOwner.Size = New Size(603, 175)
        dgvOwner.TabIndex = 25
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label21.Location = New Point(3, 1545)
        Label21.Name = "Label21"
        Label21.Size = New Size(103, 15)
        Label21.TabIndex = 24
        Label21.Text = "General Payment"
        ' 
        ' dgvGenPay
        ' 
        dgvGenPay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvGenPay.Location = New Point(3, 1563)
        dgvGenPay.Name = "dgvGenPay"
        dgvGenPay.Size = New Size(603, 175)
        dgvGenPay.TabIndex = 23
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label20.Location = New Point(3, 1349)
        Label20.Name = "Label20"
        Label20.Size = New Size(77, 15)
        Label20.TabIndex = 22
        Label20.Text = "Taxonomy(s)"
        ' 
        ' dgvTax
        ' 
        dgvTax.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvTax.Location = New Point(3, 1367)
        dgvTax.Name = "dgvTax"
        dgvTax.Size = New Size(603, 175)
        dgvTax.TabIndex = 21
        ' 
        ' dgvHCPCSlvl2
        ' 
        dgvHCPCSlvl2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvHCPCSlvl2.Location = New Point(3, 1167)
        dgvHCPCSlvl2.Name = "dgvHCPCSlvl2"
        dgvHCPCSlvl2.Size = New Size(603, 175)
        dgvHCPCSlvl2.TabIndex = 20
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label19.Location = New Point(3, 1149)
        Label19.Name = "Label19"
        Label19.Size = New Size(123, 15)
        Label19.TabIndex = 19
        Label19.Text = "HCPCS Codes Level 2"
        Label19.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' dgvResearch
        ' 
        dgvResearch.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvResearch.Location = New Point(3, 1964)
        dgvResearch.Name = "dgvResearch"
        dgvResearch.Size = New Size(602, 166)
        dgvResearch.TabIndex = 18
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label18.Location = New Point(3, 973)
        Label18.Name = "Label18"
        Label18.Size = New Size(123, 15)
        Label18.TabIndex = 17
        Label18.Text = "HCPCS Codes Level 1"
        ' 
        ' dgvHCPCSlvl1
        ' 
        dgvHCPCSlvl1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvHCPCSlvl1.Location = New Point(3, 991)
        dgvHCPCSlvl1.Name = "dgvHCPCSlvl1"
        dgvHCPCSlvl1.Size = New Size(602, 155)
        dgvHCPCSlvl1.TabIndex = 16
        ' 
        ' dgvMain
        ' 
        dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvMain.Location = New Point(605, 3)
        dgvMain.Name = "dgvMain"
        dgvMain.Size = New Size(367, 249)
        dgvMain.TabIndex = 15
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label15.Location = New Point(-1, 783)
        Label15.Name = "Label15"
        Label15.Size = New Size(40, 15)
        Label15.TabIndex = 14
        Label15.Text = "Drugs"
        Label15.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' dgvDrugs
        ' 
        dgvDrugs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDrugs.Location = New Point(0, 802)
        dgvDrugs.Name = "dgvDrugs"
        dgvDrugs.Size = New Size(602, 168)
        dgvDrugs.TabIndex = 13
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label14.Location = New Point(-1, 594)
        Label14.Name = "Label14"
        Label14.Size = New Size(103, 15)
        Label14.TabIndex = 12
        Label14.Text = "Facility Affiliation"
        ' 
        ' dgvAff
        ' 
        dgvAff.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvAff.Location = New Point(0, 612)
        dgvAff.Name = "dgvAff"
        dgvAff.Size = New Size(602, 168)
        dgvAff.TabIndex = 11
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(lblMailingZip)
        GroupBox1.Controls.Add(lblMailingState)
        GroupBox1.Controls.Add(Label23)
        GroupBox1.Controls.Add(Label9)
        GroupBox1.Controls.Add(Label5)
        GroupBox1.Controls.Add(S)
        GroupBox1.Controls.Add(lblPracticeStreet)
        GroupBox1.Controls.Add(Label22)
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
        GroupBox1.Controls.Add(lblMailingCity)
        GroupBox1.Controls.Add(lblPracticeZip)
        GroupBox1.Controls.Add(lblPracticeState)
        GroupBox1.Controls.Add(lblPCity)
        GroupBox1.Controls.Add(lblMailingStreet)
        GroupBox1.Controls.Add(lblLast)
        GroupBox1.Controls.Add(lblMiddle)
        GroupBox1.Controls.Add(lblFirst)
        GroupBox1.Controls.Add(tbNpiResult)
        GroupBox1.Controls.Add(Label11)
        GroupBox1.Controls.Add(Label10)
        GroupBox1.Controls.Add(lblmailc)
        GroupBox1.Controls.Add(Label8)
        GroupBox1.Controls.Add(Label7)
        GroupBox1.Controls.Add(Label6)
        GroupBox1.Controls.Add(Street)
        GroupBox1.Controls.Add(Label4)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Location = New Point(0, 3)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(602, 588)
        GroupBox1.TabIndex = 10
        GroupBox1.TabStop = False
        GroupBox1.Text = "Individual Profile"
        ' 
        ' lblMailingZip
        ' 
        lblMailingZip.AutoSize = True
        lblMailingZip.Location = New Point(230, 234)
        lblMailingZip.Name = "lblMailingZip"
        lblMailingZip.Size = New Size(10, 15)
        lblMailingZip.TabIndex = 40
        lblMailingZip.Text = "."
        ' 
        ' lblMailingState
        ' 
        lblMailingState.AutoSize = True
        lblMailingState.Location = New Point(230, 206)
        lblMailingState.Name = "lblMailingState"
        lblMailingState.Size = New Size(10, 15)
        lblMailingState.TabIndex = 39
        lblMailingState.Text = "."
        ' 
        ' Label23
        ' 
        Label23.AutoSize = True
        Label23.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label23.Location = New Point(3, 126)
        Label23.Name = "Label23"
        Label23.Size = New Size(94, 15)
        Label23.TabIndex = 38
        Label23.Text = "Mailing Address"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label9.Location = New Point(3, 230)
        Label9.Name = "Label9"
        Label9.Size = New Size(55, 15)
        Label9.TabIndex = 37
        Label9.Text = "Zip Code"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label5.Location = New Point(3, 204)
        Label5.Name = "Label5"
        Label5.Size = New Size(37, 15)
        Label5.TabIndex = 36
        Label5.Text = "State"
        ' 
        ' S
        ' 
        S.AutoSize = True
        S.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        S.Location = New Point(3, 282)
        S.Name = "S"
        S.Size = New Size(43, 15)
        S.TabIndex = 35
        S.Text = "Street"
        ' 
        ' lblPracticeStreet
        ' 
        lblPracticeStreet.AutoSize = True
        lblPracticeStreet.Location = New Point(230, 282)
        lblPracticeStreet.Name = "lblPracticeStreet"
        lblPracticeStreet.Size = New Size(10, 15)
        lblPracticeStreet.TabIndex = 34
        lblPracticeStreet.Text = "."
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label22.Location = New Point(3, 256)
        Label22.Name = "Label22"
        Label22.Size = New Size(145, 15)
        Label22.TabIndex = 33
        Label22.Text = "Primary Practice Address"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label17.Location = New Point(3, 542)
        Label17.Name = "Label17"
        Label17.Size = New Size(45, 15)
        Label17.TabIndex = 32
        Label17.Text = "Facility"
        ' 
        ' lblFacility
        ' 
        lblFacility.AutoSize = True
        lblFacility.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblFacility.Location = New Point(230, 542)
        lblFacility.Name = "lblFacility"
        lblFacility.Size = New Size(10, 15)
        lblFacility.TabIndex = 31
        lblFacility.Text = "."
        ' 
        ' lblGradYear
        ' 
        lblGradYear.AutoSize = True
        lblGradYear.Location = New Point(230, 516)
        lblGradYear.Name = "lblGradYear"
        lblGradYear.Size = New Size(10, 15)
        lblGradYear.TabIndex = 30
        lblGradYear.Text = "."
        ' 
        ' lblg
        ' 
        lblg.AutoSize = True
        lblg.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblg.Location = New Point(3, 516)
        lblg.Name = "lblg"
        lblg.Size = New Size(96, 15)
        lblg.TabIndex = 29
        lblg.Text = "Graduation Year"
        ' 
        ' lblMedSchool
        ' 
        lblMedSchool.AutoSize = True
        lblMedSchool.Location = New Point(230, 490)
        lblMedSchool.Name = "lblMedSchool"
        lblMedSchool.Size = New Size(10, 15)
        lblMedSchool.TabIndex = 28
        lblMedSchool.Text = "."
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label16.Location = New Point(3, 490)
        Label16.Name = "Label16"
        Label16.Size = New Size(90, 15)
        Label16.TabIndex = 27
        Label16.Text = "Medical School"
        ' 
        ' lblPhone
        ' 
        lblPhone.AutoSize = True
        lblPhone.Location = New Point(230, 464)
        lblPhone.Name = "lblPhone"
        lblPhone.Size = New Size(10, 15)
        lblPhone.TabIndex = 26
        lblPhone.Text = "."
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label13.Location = New Point(3, 464)
        Label13.Name = "Label13"
        Label13.Size = New Size(42, 15)
        Label13.TabIndex = 25
        Label13.Text = "Phone"
        ' 
        ' lblLicNum
        ' 
        lblLicNum.AutoSize = True
        lblLicNum.Location = New Point(230, 438)
        lblLicNum.Name = "lblLicNum"
        lblLicNum.Size = New Size(10, 15)
        lblLicNum.TabIndex = 24
        lblLicNum.Text = "."
        ' 
        ' lbllic
        ' 
        lbllic.AutoSize = True
        lbllic.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lbllic.Location = New Point(3, 438)
        lbllic.Name = "lbllic"
        lbllic.Size = New Size(97, 15)
        lbllic.TabIndex = 23
        lbllic.Text = "License Number"
        ' 
        ' lblTax
        ' 
        lblTax.AutoSize = True
        lblTax.Location = New Point(230, 412)
        lblTax.Name = "lblTax"
        lblTax.Size = New Size(10, 15)
        lblTax.TabIndex = 22
        lblTax.Text = "."
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label12.Location = New Point(3, 412)
        Label12.Name = "Label12"
        Label12.Size = New Size(119, 15)
        Label12.TabIndex = 21
        Label12.Text = "Taxonomy/Specialty"
        ' 
        ' lblGender
        ' 
        lblGender.AutoSize = True
        lblGender.Location = New Point(230, 386)
        lblGender.Name = "lblGender"
        lblGender.Size = New Size(10, 15)
        lblGender.TabIndex = 20
        lblGender.Text = "."
        ' 
        ' lblMailingCity
        ' 
        lblMailingCity.AutoSize = True
        lblMailingCity.Location = New Point(230, 178)
        lblMailingCity.Name = "lblMailingCity"
        lblMailingCity.Size = New Size(10, 15)
        lblMailingCity.TabIndex = 19
        lblMailingCity.Text = "."
        ' 
        ' lblPracticeZip
        ' 
        lblPracticeZip.AutoSize = True
        lblPracticeZip.Location = New Point(230, 360)
        lblPracticeZip.Name = "lblPracticeZip"
        lblPracticeZip.Size = New Size(10, 15)
        lblPracticeZip.TabIndex = 18
        lblPracticeZip.Text = "."
        ' 
        ' lblPracticeState
        ' 
        lblPracticeState.AutoSize = True
        lblPracticeState.Location = New Point(230, 334)
        lblPracticeState.Name = "lblPracticeState"
        lblPracticeState.Size = New Size(10, 15)
        lblPracticeState.TabIndex = 17
        lblPracticeState.Text = "."
        ' 
        ' lblPCity
        ' 
        lblPCity.AutoSize = True
        lblPCity.Location = New Point(230, 308)
        lblPCity.Name = "lblPCity"
        lblPCity.Size = New Size(10, 15)
        lblPCity.TabIndex = 16
        lblPCity.Text = "."
        ' 
        ' lblMailingStreet
        ' 
        lblMailingStreet.AutoSize = True
        lblMailingStreet.Location = New Point(230, 152)
        lblMailingStreet.Name = "lblMailingStreet"
        lblMailingStreet.Size = New Size(10, 15)
        lblMailingStreet.TabIndex = 15
        lblMailingStreet.Text = "."
        ' 
        ' lblLast
        ' 
        lblLast.AutoSize = True
        lblLast.Location = New Point(230, 100)
        lblLast.Name = "lblLast"
        lblLast.Size = New Size(10, 15)
        lblLast.TabIndex = 14
        lblLast.Text = "."
        ' 
        ' lblMiddle
        ' 
        lblMiddle.AutoSize = True
        lblMiddle.Location = New Point(230, 74)
        lblMiddle.Name = "lblMiddle"
        lblMiddle.Size = New Size(10, 15)
        lblMiddle.TabIndex = 13
        lblMiddle.Text = "."
        ' 
        ' lblFirst
        ' 
        lblFirst.AutoSize = True
        lblFirst.Location = New Point(230, 48)
        lblFirst.Name = "lblFirst"
        lblFirst.Size = New Size(10, 15)
        lblFirst.TabIndex = 12
        lblFirst.Text = "."
        ' 
        ' tbNpiResult
        ' 
        tbNpiResult.Location = New Point(230, 19)
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
        Label10.Location = New Point(3, 386)
        Label10.Name = "Label10"
        Label10.Size = New Size(49, 15)
        Label10.TabIndex = 9
        Label10.Text = "Gender"
        ' 
        ' lblmailc
        ' 
        lblmailc.AutoSize = True
        lblmailc.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblmailc.Location = New Point(3, 178)
        lblmailc.Name = "lblmailc"
        lblmailc.Size = New Size(28, 15)
        lblmailc.TabIndex = 8
        lblmailc.Text = "City"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label8.Location = New Point(3, 360)
        Label8.Name = "Label8"
        Label8.Size = New Size(55, 15)
        Label8.TabIndex = 7
        Label8.Text = "Zip Code"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label7.Location = New Point(3, 334)
        Label7.Name = "Label7"
        Label7.Size = New Size(37, 15)
        Label7.TabIndex = 6
        Label7.Text = "State"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label6.Location = New Point(3, 308)
        Label6.Name = "Label6"
        Label6.Size = New Size(28, 15)
        Label6.TabIndex = 5
        Label6.Text = "City"
        ' 
        ' Street
        ' 
        Street.AutoSize = True
        Street.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Street.Location = New Point(3, 152)
        Street.Name = "Street"
        Street.Size = New Size(43, 15)
        Street.TabIndex = 4
        Street.Text = "Street"
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
        ClientSize = New Size(1222, 1061)
        Controls.Add(Panel1)
        Name = "IndividualProfileForm"
        Text = "IndividualProfileForm"
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        CType(dgvOwner, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvGenPay, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvTax, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvHCPCSlvl2, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvResearch, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvHCPCSlvl1, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvMain, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvDrugs, ComponentModel.ISupportInitialize).EndInit()
        CType(dgvAff, ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label19 As Label
    Friend WithEvents dgvResearch As DataGridView
    Friend WithEvents Label18 As Label
    Friend WithEvents dgvHCPCSlvl1 As DataGridView
    Friend WithEvents dgvMain As DataGridView
    Friend WithEvents Label15 As Label
    Friend WithEvents dgvDrugs As DataGridView
    Friend WithEvents Label14 As Label
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
    Friend WithEvents Label21 As Label
    Friend WithEvents dgvGenPay As DataGridView
    Friend WithEvents Label20 As Label
    Friend WithEvents dgvTax As DataGridView
    Friend WithEvents lblPracticeStreet As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents S As Label
    Friend WithEvents lblMailingZip As Label
    Friend WithEvents lblMailingState As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents dgvOwner As DataGridView
    Friend WithEvents Button1 As Button
End Class

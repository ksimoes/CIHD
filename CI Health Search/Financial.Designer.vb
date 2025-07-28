<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Financial
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
        GroupBox1 = New BoldGroupBox()
        Label18 = New Label()
        lblTotAssetsResult = New Label()
        lblOtherAssetsResult = New Label()
        lblFixAssetsResult = New Label()
        lblCurAssetResult = New Label()
        Label8 = New Label()
        Label7 = New Label()
        Label6 = New Label()
        Label5 = New Label()
        Label19 = New Label()
        lblTlResult = New Label()
        lblCurLiabilitiesRes = New Label()
        lblTotFbResult = New Label()
        lblLtResult = New Label()
        Label9 = New Label()
        Label13 = New Label()
        Label12 = New Label()
        Label10 = New Label()
        lblCurrentLiab = New Label()
        lblTotLandFbResult = New Label()
        lblPedResult = New Label()
        lblNumMonthsPeriodResult = New Label()
        lblNumMonthsFin = New Label()
        lbligr = New Label()
        gbIncomeStatement = New BoldGroupBox()
        GroupBox3 = New BoldGroupBox()
        lblTotOtherExpensesResult = New Label()
        lblDepreciationExpenseResult = New Label()
        Label17 = New Label()
        lblOperatingIncome = New Label()
        lblNetIncomeResult = New Label()
        Label21 = New Label()
        Label22 = New Label()
        Label23 = New Label()
        Label24 = New Label()
        lblTotNonpatientResult = New Label()
        Label25 = New Label()
        Label26 = New Label()
        lblMiscNpRevResult = New Label()
        Label27 = New Label()
        Label28 = New Label()
        lblGovAppResult = New Label()
        lblTotOtherIncomeResult = New Label()
        lblIncomeFromInvestResult = New Label()
        lblContractAllowanceResult = New Label()
        Label20 = New Label()
        lblNetPatRevResult = New Label()
        lblTotPatRevResult = New Label()
        lblOutPatResult = New Label()
        lblInpRevResult = New Label()
        lblTotOperatingExpenseResult = New Label()
        Label16 = New Label()
        Label15 = New Label()
        Label14 = New Label()
        Label11 = New Label()
        Label2 = New Label()
        gbUncompensatedCare = New GroupBox()
        lblCcResult = New Label()
        lblTotUcResult = New Label()
        lblucpctResult = New Label()
        Labl2 = New Label()
        Label4 = New Label()
        Label3 = New Label()
        lblUncompResult = New Label()
        Label1 = New Label()
        btnOutpatientFinancial = New Button()
        btnInpatientFinancial = New Button()
        btnQualityFinancial = New Button()
        btnFInIndFinancial = New Button()
        btnDepartmentsFinancial = New Button()
        btnProfileFinancial = New Button()
        Button1 = New Button()
        Panel1 = New Panel()
        GroupBox2 = New BoldGroupBox()

        GroupBox1.SuspendLayout()
        gbIncomeStatement.SuspendLayout()
        GroupBox3.SuspendLayout()
        gbUncompensatedCare.SuspendLayout()
        Panel1.SuspendLayout()
        GroupBox2.SuspendLayout()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.BackColor = SystemColors.ButtonHighlight
        GroupBox1.Controls.Add(Label18)
        GroupBox1.Controls.Add(lblTotAssetsResult)
        GroupBox1.Controls.Add(lblOtherAssetsResult)
        GroupBox1.Controls.Add(lblFixAssetsResult)
        GroupBox1.Controls.Add(lblCurAssetResult)
        GroupBox1.Controls.Add(Label8)
        GroupBox1.Controls.Add(Label7)
        GroupBox1.Controls.Add(Label6)
        GroupBox1.Controls.Add(Label5)
        GroupBox1.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        GroupBox1.Location = New Point(11, 63)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(570, 321)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "Balance Sheet"
        ' 
        ' Label18
        ' 
        Label18.AutoSize = True
        Label18.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label18.Location = New Point(12, 22)
        Label18.Name = "Label18"
        Label18.Size = New Size(42, 15)
        Label18.TabIndex = 20
        Label18.Text = "Assets"
        ' 
        ' lblTotAssetsResult
        ' 
        lblTotAssetsResult.AutoSize = True
        lblTotAssetsResult.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblTotAssetsResult.Location = New Point(314, 124)
        lblTotAssetsResult.Name = "lblTotAssetsResult"
        lblTotAssetsResult.Size = New Size(42, 15)
        lblTotAssetsResult.TabIndex = 11
        lblTotAssetsResult.Text = "Result"
        ' 
        ' lblOtherAssetsResult
        ' 
        lblOtherAssetsResult.AutoSize = True
        lblOtherAssetsResult.Font = New Font("Segoe UI", 9F)
        lblOtherAssetsResult.Location = New Point(314, 98)
        lblOtherAssetsResult.Name = "lblOtherAssetsResult"
        lblOtherAssetsResult.Size = New Size(39, 15)
        lblOtherAssetsResult.TabIndex = 10
        lblOtherAssetsResult.Text = "Result"
        ' 
        ' lblFixAssetsResult
        ' 
        lblFixAssetsResult.AutoSize = True
        lblFixAssetsResult.Font = New Font("Segoe UI", 9F)
        lblFixAssetsResult.Location = New Point(314, 71)
        lblFixAssetsResult.Name = "lblFixAssetsResult"
        lblFixAssetsResult.Size = New Size(39, 15)
        lblFixAssetsResult.TabIndex = 9
        lblFixAssetsResult.Text = "Result"
        ' 
        ' lblCurAssetResult
        ' 
        lblCurAssetResult.AutoSize = True
        lblCurAssetResult.Font = New Font("Segoe UI", 9F)
        lblCurAssetResult.Location = New Point(314, 47)
        lblCurAssetResult.Name = "lblCurAssetResult"
        lblCurAssetResult.Size = New Size(39, 15)
        lblCurAssetResult.TabIndex = 8
        lblCurAssetResult.Text = "Result"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label8.Location = New Point(12, 124)
        Label8.Name = "Label8"
        Label8.Size = New Size(72, 15)
        Label8.TabIndex = 7
        Label8.Text = "Total Assets"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Font = New Font("Segoe UI", 9F)
        Label7.Location = New Point(13, 98)
        Label7.Name = "Label7"
        Label7.Size = New Size(73, 15)
        Label7.TabIndex = 6
        Label7.Text = "Other Assets"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Font = New Font("Segoe UI", 9F)
        Label6.Location = New Point(12, 71)
        Label6.Name = "Label6"
        Label6.Size = New Size(70, 15)
        Label6.TabIndex = 5
        Label6.Text = "Fixed Assets"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Font = New Font("Segoe UI", 9F)
        Label5.Location = New Point(12, 47)
        Label5.Name = "Label5"
        Label5.Size = New Size(83, 15)
        Label5.TabIndex = 4
        Label5.Text = "Current Assets"
        ' 
        ' Label19
        ' 
        Label19.AutoSize = True
        Label19.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label19.Location = New Point(3, 15)
        Label19.Name = "Label19"
        Label19.Size = New Size(162, 15)
        Label19.TabIndex = 21
        Label19.Text = "Liabilities and Fund Balances"
        ' 
        ' lblTlResult
        ' 
        lblTlResult.AutoSize = True
        lblTlResult.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblTlResult.Location = New Point(304, 99)
        lblTlResult.Name = "lblTlResult"
        lblTlResult.Size = New Size(42, 15)
        lblTlResult.TabIndex = 19
        lblTlResult.Text = "Result"
        ' 
        ' lblCurLiabilitiesRes
        ' 
        lblCurLiabilitiesRes.AutoSize = True
        lblCurLiabilitiesRes.Font = New Font("Segoe UI", 9F)
        lblCurLiabilitiesRes.Location = New Point(304, 45)
        lblCurLiabilitiesRes.Name = "lblCurLiabilitiesRes"
        lblCurLiabilitiesRes.Size = New Size(39, 15)
        lblCurLiabilitiesRes.TabIndex = 18
        lblCurLiabilitiesRes.Text = "Result"
        ' 
        ' lblTotFbResult
        ' 
        lblTotFbResult.AutoSize = True
        lblTotFbResult.Font = New Font("Segoe UI", 9F)
        lblTotFbResult.Location = New Point(304, 128)
        lblTotFbResult.Name = "lblTotFbResult"
        lblTotFbResult.Size = New Size(39, 15)
        lblTotFbResult.TabIndex = 17
        lblTotFbResult.Text = "Result"
        ' 
        ' lblLtResult
        ' 
        lblLtResult.AutoSize = True
        lblLtResult.Font = New Font("Segoe UI", 9F)
        lblLtResult.Location = New Point(304, 72)
        lblLtResult.Name = "lblLtResult"
        lblLtResult.Size = New Size(39, 15)
        lblLtResult.TabIndex = 17
        lblLtResult.Text = "Result"
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label9.Location = New Point(2, 155)
        Label9.Name = "Label9"
        Label9.Size = New Size(192, 15)
        Label9.TabIndex = 16
        Label9.Text = "Total Liabilities and Fund Balances"
        ' 
        ' Label13
        ' 
        Label13.AutoSize = True
        Label13.Font = New Font("Segoe UI", 9F)
        Label13.Location = New Point(2, 72)
        Label13.Name = "Label13"
        Label13.Size = New Size(64, 15)
        Label13.TabIndex = 15
        Label13.Text = "Long Term"
        ' 
        ' Label12
        ' 
        Label12.AutoSize = True
        Label12.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label12.Location = New Point(2, 99)
        Label12.Name = "Label12"
        Label12.Size = New Size(88, 15)
        Label12.TabIndex = 14
        Label12.Text = "Total Liabilities"
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Font = New Font("Segoe UI", 9F)
        Label10.Location = New Point(3, 128)
        Label10.Name = "Label10"
        Label10.Size = New Size(112, 15)
        Label10.TabIndex = 13
        Label10.Text = "Total Fund Balances"
        ' 
        ' lblCurrentLiab
        ' 
        lblCurrentLiab.AutoSize = True
        lblCurrentLiab.Font = New Font("Segoe UI", 9F)
        lblCurrentLiab.Location = New Point(2, 45)
        lblCurrentLiab.Name = "lblCurrentLiab"
        lblCurrentLiab.Size = New Size(99, 15)
        lblCurrentLiab.TabIndex = 12
        lblCurrentLiab.Text = "Current Liabilities"
        ' 
        ' lblTotLandFbResult
        ' 
        lblTotLandFbResult.AutoSize = True
        lblTotLandFbResult.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblTotLandFbResult.Location = New Point(304, 155)
        lblTotLandFbResult.Name = "lblTotLandFbResult"
        lblTotLandFbResult.Size = New Size(42, 15)
        lblTotLandFbResult.TabIndex = 10
        lblTotLandFbResult.Text = "Result"
        ' 
        ' lblPedResult
        ' 
        lblPedResult.AutoSize = True
        lblPedResult.Location = New Point(180, 10)
        lblPedResult.Name = "lblPedResult"
        lblPedResult.Size = New Size(39, 15)
        lblPedResult.TabIndex = 3
        lblPedResult.Text = "Result"
        ' 
        ' lblNumMonthsPeriodResult
        ' 
        lblNumMonthsPeriodResult.AutoSize = True
        lblNumMonthsPeriodResult.Location = New Point(180, 37)
        lblNumMonthsPeriodResult.Name = "lblNumMonthsPeriodResult"
        lblNumMonthsPeriodResult.Size = New Size(39, 15)
        lblNumMonthsPeriodResult.TabIndex = 2
        lblNumMonthsPeriodResult.Text = "Result"
        ' 
        ' lblNumMonthsFin
        ' 
        lblNumMonthsFin.AutoSize = True
        lblNumMonthsFin.Location = New Point(11, 37)
        lblNumMonthsFin.Name = "lblNumMonthsFin"
        lblNumMonthsFin.Size = New Size(145, 15)
        lblNumMonthsFin.TabIndex = 1
        lblNumMonthsFin.Text = "Number Months In period"
        ' 
        ' lbligr
        ' 
        lbligr.AutoSize = True
        lbligr.Location = New Point(11, 10)
        lbligr.Name = "lbligr"
        lbligr.Size = New Size(91, 15)
        lbligr.TabIndex = 0
        lbligr.Text = "Period End Date"
        ' 
        ' gbIncomeStatement
        ' 
        gbIncomeStatement.BackColor = Color.White
        gbIncomeStatement.Controls.Add(GroupBox3)
        gbIncomeStatement.Controls.Add(lblContractAllowanceResult)
        gbIncomeStatement.Controls.Add(Label20)
        gbIncomeStatement.Controls.Add(lblNetPatRevResult)
        gbIncomeStatement.Controls.Add(lblTotPatRevResult)
        gbIncomeStatement.Controls.Add(lblOutPatResult)
        gbIncomeStatement.Controls.Add(lblInpRevResult)
        gbIncomeStatement.Controls.Add(lblTotOperatingExpenseResult)
        gbIncomeStatement.Controls.Add(Label16)
        gbIncomeStatement.Controls.Add(Label15)
        gbIncomeStatement.Controls.Add(Label14)
        gbIncomeStatement.Controls.Add(Label11)
        gbIncomeStatement.Controls.Add(Label2)
        gbIncomeStatement.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        gbIncomeStatement.Location = New Point(11, 403)
        gbIncomeStatement.Name = "gbIncomeStatement"
        gbIncomeStatement.Size = New Size(570, 469)
        gbIncomeStatement.TabIndex = 1
        gbIncomeStatement.TabStop = False
        gbIncomeStatement.Text = "Income Statement"
        ' 
        ' GroupBox3
        ' 
        GroupBox3.BackColor = Color.LightSkyBlue
        GroupBox3.Controls.Add(lblTotOtherExpensesResult)
        GroupBox3.Controls.Add(lblDepreciationExpenseResult)
        GroupBox3.Controls.Add(Label17)
        GroupBox3.Controls.Add(lblOperatingIncome)
        GroupBox3.Controls.Add(lblNetIncomeResult)
        GroupBox3.Controls.Add(Label21)
        GroupBox3.Controls.Add(Label22)
        GroupBox3.Controls.Add(Label23)
        GroupBox3.Controls.Add(Label24)
        GroupBox3.Controls.Add(lblTotNonpatientResult)
        GroupBox3.Controls.Add(Label25)
        GroupBox3.Controls.Add(Label26)
        GroupBox3.Controls.Add(lblMiscNpRevResult)
        GroupBox3.Controls.Add(Label27)
        GroupBox3.Controls.Add(Label28)
        GroupBox3.Controls.Add(lblGovAppResult)
        GroupBox3.Controls.Add(lblTotOtherIncomeResult)
        GroupBox3.Controls.Add(lblIncomeFromInvestResult)
        GroupBox3.Location = New Point(0, 214)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(570, 254)
        GroupBox3.TabIndex = 43
        GroupBox3.TabStop = False
        GroupBox3.Text = "."
        ' 
        ' lblTotOtherExpensesResult
        ' 
        lblTotOtherExpensesResult.AutoSize = True
        lblTotOtherExpensesResult.Font = New Font("Segoe UI", 9F)
        lblTotOtherExpensesResult.Location = New Point(318, 196)
        lblTotOtherExpensesResult.Name = "lblTotOtherExpensesResult"
        lblTotOtherExpensesResult.Size = New Size(39, 15)
        lblTotOtherExpensesResult.TabIndex = 40
        lblTotOtherExpensesResult.Text = "Result"
        ' 
        ' lblDepreciationExpenseResult
        ' 
        lblDepreciationExpenseResult.AutoSize = True
        lblDepreciationExpenseResult.Font = New Font("Segoe UI", 9F)
        lblDepreciationExpenseResult.Location = New Point(318, 286)
        lblDepreciationExpenseResult.Name = "lblDepreciationExpenseResult"
        lblDepreciationExpenseResult.Size = New Size(39, 15)
        lblDepreciationExpenseResult.TabIndex = 42
        lblDepreciationExpenseResult.Text = "Result"
        ' 
        ' Label17
        ' 
        Label17.AutoSize = True
        Label17.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label17.Location = New Point(10, 22)
        Label17.Name = "Label17"
        Label17.Size = New Size(108, 15)
        Label17.TabIndex = 25
        Label17.Text = "Operating Income"
        ' 
        ' lblOperatingIncome
        ' 
        lblOperatingIncome.AutoSize = True
        lblOperatingIncome.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblOperatingIncome.Location = New Point(318, 15)
        lblOperatingIncome.Name = "lblOperatingIncome"
        lblOperatingIncome.Size = New Size(42, 15)
        lblOperatingIncome.TabIndex = 26
        lblOperatingIncome.Text = "Result"
        ' 
        ' lblNetIncomeResult
        ' 
        lblNetIncomeResult.AutoSize = True
        lblNetIncomeResult.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblNetIncomeResult.Location = New Point(318, 226)
        lblNetIncomeResult.Name = "lblNetIncomeResult"
        lblNetIncomeResult.Size = New Size(42, 15)
        lblNetIncomeResult.TabIndex = 41
        lblNetIncomeResult.Text = "Result"
        ' 
        ' Label21
        ' 
        Label21.AutoSize = True
        Label21.Font = New Font("Segoe UI", 9F)
        Label21.Location = New Point(10, 50)
        Label21.Name = "Label21"
        Label21.Size = New Size(80, 15)
        Label21.TabIndex = 27
        Label21.Text = "Other Income"
        ' 
        ' Label22
        ' 
        Label22.AutoSize = True
        Label22.Font = New Font("Segoe UI", 9F)
        Label22.Location = New Point(10, 76)
        Label22.Name = "Label22"
        Label22.Size = New Size(145, 15)
        Label22.TabIndex = 28
        Label22.Text = "Income From Investments"
        ' 
        ' Label23
        ' 
        Label23.AutoSize = True
        Label23.Font = New Font("Segoe UI", 9F)
        Label23.Location = New Point(10, 101)
        Label23.Name = "Label23"
        Label23.Size = New Size(164, 15)
        Label23.TabIndex = 29
        Label23.Text = "Governmental Appropriations"
        ' 
        ' Label24
        ' 
        Label24.AutoSize = True
        Label24.Font = New Font("Segoe UI", 9F)
        Label24.Location = New Point(10, 133)
        Label24.Name = "Label24"
        Label24.Size = New Size(198, 15)
        Label24.TabIndex = 30
        Label24.Text = "Miscellaneous Non-Patient Revenue"
        ' 
        ' lblTotNonpatientResult
        ' 
        lblTotNonpatientResult.AutoSize = True
        lblTotNonpatientResult.Font = New Font("Segoe UI", 9F)
        lblTotNonpatientResult.Location = New Point(318, 163)
        lblTotNonpatientResult.Name = "lblTotNonpatientResult"
        lblTotNonpatientResult.Size = New Size(39, 15)
        lblTotNonpatientResult.TabIndex = 39
        lblTotNonpatientResult.Text = "Result"
        ' 
        ' Label25
        ' 
        Label25.AutoSize = True
        Label25.Font = New Font("Segoe UI", 9F)
        Label25.Location = New Point(10, 163)
        Label25.Name = "Label25"
        Label25.Size = New Size(149, 15)
        Label25.TabIndex = 31
        Label25.Text = "Total Non-Patient Revenue"
        ' 
        ' Label26
        ' 
        Label26.AutoSize = True
        Label26.Font = New Font("Segoe UI", 9F)
        Label26.Location = New Point(10, 196)
        Label26.Name = "Label26"
        Label26.Size = New Size(116, 15)
        Label26.TabIndex = 32
        Label26.Text = "Total Other Expenses"
        ' 
        ' lblMiscNpRevResult
        ' 
        lblMiscNpRevResult.AutoSize = True
        lblMiscNpRevResult.Font = New Font("Segoe UI", 9F)
        lblMiscNpRevResult.Location = New Point(318, 133)
        lblMiscNpRevResult.Name = "lblMiscNpRevResult"
        lblMiscNpRevResult.Size = New Size(39, 15)
        lblMiscNpRevResult.TabIndex = 38
        lblMiscNpRevResult.Text = "Result"
        ' 
        ' Label27
        ' 
        Label27.AutoSize = True
        Label27.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        Label27.Location = New Point(10, 226)
        Label27.Name = "Label27"
        Label27.Size = New Size(122, 15)
        Label27.TabIndex = 33
        Label27.Text = "Net Income or (Loss)"
        ' 
        ' Label28
        ' 
        Label28.AutoSize = True
        Label28.Font = New Font("Segoe UI", 9F)
        Label28.Location = New Point(10, 286)
        Label28.Name = "Label28"
        Label28.Size = New Size(119, 15)
        Label28.TabIndex = 34
        Label28.Text = "Depreciation Expense"
        ' 
        ' lblGovAppResult
        ' 
        lblGovAppResult.AutoSize = True
        lblGovAppResult.Font = New Font("Segoe UI", 9F)
        lblGovAppResult.Location = New Point(318, 101)
        lblGovAppResult.Name = "lblGovAppResult"
        lblGovAppResult.Size = New Size(39, 15)
        lblGovAppResult.TabIndex = 37
        lblGovAppResult.Text = "Result"
        ' 
        ' lblTotOtherIncomeResult
        ' 
        lblTotOtherIncomeResult.AutoSize = True
        lblTotOtherIncomeResult.Font = New Font("Segoe UI", 9F)
        lblTotOtherIncomeResult.Location = New Point(318, 50)
        lblTotOtherIncomeResult.Name = "lblTotOtherIncomeResult"
        lblTotOtherIncomeResult.Size = New Size(39, 15)
        lblTotOtherIncomeResult.TabIndex = 35
        lblTotOtherIncomeResult.Text = "Result"
        ' 
        ' lblIncomeFromInvestResult
        ' 
        lblIncomeFromInvestResult.AutoSize = True
        lblIncomeFromInvestResult.Font = New Font("Segoe UI", 9F)
        lblIncomeFromInvestResult.Location = New Point(318, 76)
        lblIncomeFromInvestResult.Name = "lblIncomeFromInvestResult"
        lblIncomeFromInvestResult.Size = New Size(39, 15)
        lblIncomeFromInvestResult.TabIndex = 36
        lblIncomeFromInvestResult.Text = "Result"
        ' 
        ' lblContractAllowanceResult
        ' 
        lblContractAllowanceResult.AutoSize = True
        lblContractAllowanceResult.Font = New Font("Segoe UI", 9F)
        lblContractAllowanceResult.Location = New Point(314, 114)
        lblContractAllowanceResult.Name = "lblContractAllowanceResult"
        lblContractAllowanceResult.Size = New Size(39, 15)
        lblContractAllowanceResult.TabIndex = 24
        lblContractAllowanceResult.Text = "Result"
        ' 
        ' Label20
        ' 
        Label20.AutoSize = True
        Label20.Font = New Font("Segoe UI", 9F)
        Label20.Location = New Point(6, 114)
        Label20.Name = "Label20"
        Label20.Size = New Size(132, 15)
        Label20.TabIndex = 23
        Label20.Text = "Contractual Allowances"
        ' 
        ' lblNetPatRevResult
        ' 
        lblNetPatRevResult.AutoSize = True
        lblNetPatRevResult.Font = New Font("Segoe UI", 9F)
        lblNetPatRevResult.Location = New Point(314, 145)
        lblNetPatRevResult.Name = "lblNetPatRevResult"
        lblNetPatRevResult.Size = New Size(39, 15)
        lblNetPatRevResult.TabIndex = 22
        lblNetPatRevResult.Text = "Result"
        ' 
        ' lblTotPatRevResult
        ' 
        lblTotPatRevResult.AutoSize = True
        lblTotPatRevResult.Font = New Font("Segoe UI", 9F)
        lblTotPatRevResult.Location = New Point(314, 87)
        lblTotPatRevResult.Name = "lblTotPatRevResult"
        lblTotPatRevResult.Size = New Size(39, 15)
        lblTotPatRevResult.TabIndex = 21
        lblTotPatRevResult.Text = "Result"
        ' 
        ' lblOutPatResult
        ' 
        lblOutPatResult.AutoSize = True
        lblOutPatResult.Font = New Font("Segoe UI", 9F)
        lblOutPatResult.Location = New Point(314, 63)
        lblOutPatResult.Name = "lblOutPatResult"
        lblOutPatResult.Size = New Size(39, 15)
        lblOutPatResult.TabIndex = 20
        lblOutPatResult.Text = "Result"
        ' 
        ' lblInpRevResult
        ' 
        lblInpRevResult.AutoSize = True
        lblInpRevResult.Font = New Font("Segoe UI", 9F)
        lblInpRevResult.Location = New Point(314, 38)
        lblInpRevResult.Name = "lblInpRevResult"
        lblInpRevResult.Size = New Size(39, 15)
        lblInpRevResult.TabIndex = 19
        lblInpRevResult.Text = "Result"
        ' 
        ' lblTotOperatingExpenseResult
        ' 
        lblTotOperatingExpenseResult.AutoSize = True
        lblTotOperatingExpenseResult.Font = New Font("Segoe UI", 9F)
        lblTotOperatingExpenseResult.Location = New Point(314, 172)
        lblTotOperatingExpenseResult.Name = "lblTotOperatingExpenseResult"
        lblTotOperatingExpenseResult.Size = New Size(39, 15)
        lblTotOperatingExpenseResult.TabIndex = 11
        lblTotOperatingExpenseResult.Text = "Result"
        ' 
        ' Label16
        ' 
        Label16.AutoSize = True
        Label16.Font = New Font("Segoe UI", 9F)
        Label16.Location = New Point(6, 151)
        Label16.Name = "Label16"
        Label16.Size = New Size(114, 15)
        Label16.TabIndex = 10
        Label16.Text = "Net Patient Revenue"
        ' 
        ' Label15
        ' 
        Label15.AutoSize = True
        Label15.Font = New Font("Segoe UI", 9F)
        Label15.Location = New Point(6, 180)
        Label15.Name = "Label15"
        Label15.Size = New Size(134, 15)
        Label15.TabIndex = 10
        Label15.Text = "Total Operating Expense"
        ' 
        ' Label14
        ' 
        Label14.AutoSize = True
        Label14.Font = New Font("Segoe UI", 9F)
        Label14.Location = New Point(6, 63)
        Label14.Name = "Label14"
        Label14.Size = New Size(112, 15)
        Label14.TabIndex = 9
        Label14.Text = "Outpatient Revenue"
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.Font = New Font("Segoe UI", 9F)
        Label11.Location = New Point(6, 87)
        Label11.Name = "Label11"
        Label11.Size = New Size(121, 15)
        Label11.TabIndex = 8
        Label11.Text = "Total Patient Revenue"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 9F)
        Label2.Location = New Point(6, 38)
        Label2.Name = "Label2"
        Label2.Size = New Size(102, 15)
        Label2.TabIndex = 7
        Label2.Text = "Inpatient Revenue"
        ' 
        ' gbUncompensatedCare
        ' 
        gbUncompensatedCare.Controls.Add(lblCcResult)
        gbUncompensatedCare.Controls.Add(lblTotUcResult)
        gbUncompensatedCare.Controls.Add(lblucpctResult)
        gbUncompensatedCare.Controls.Add(Labl2)
        gbUncompensatedCare.Controls.Add(Label4)
        gbUncompensatedCare.Controls.Add(Label3)
        gbUncompensatedCare.Controls.Add(lblUncompResult)
        gbUncompensatedCare.Controls.Add(Label1)
        gbUncompensatedCare.Location = New Point(11, 1055)
        gbUncompensatedCare.Name = "gbUncompensatedCare"
        gbUncompensatedCare.Size = New Size(570, 197)
        gbUncompensatedCare.TabIndex = 2
        gbUncompensatedCare.TabStop = False
        gbUncompensatedCare.Text = "Uncompensated Care"
        ' 
        ' lblCcResult
        ' 
        lblCcResult.AutoSize = True
        lblCcResult.Location = New Point(185, 84)
        lblCcResult.Name = "lblCcResult"
        lblCcResult.Size = New Size(39, 15)
        lblCcResult.TabIndex = 10
        lblCcResult.Text = "Result"
        ' 
        ' lblTotUcResult
        ' 
        lblTotUcResult.AutoSize = True
        lblTotUcResult.Location = New Point(197, 123)
        lblTotUcResult.Name = "lblTotUcResult"
        lblTotUcResult.Size = New Size(39, 15)
        lblTotUcResult.TabIndex = 9
        lblTotUcResult.Text = "Result"
        ' 
        ' lblucpctResult
        ' 
        lblucpctResult.AutoSize = True
        lblucpctResult.Location = New Point(185, 164)
        lblucpctResult.Name = "lblucpctResult"
        lblucpctResult.Size = New Size(39, 15)
        lblucpctResult.TabIndex = 8
        lblucpctResult.Text = "Result"
        ' 
        ' Labl2
        ' 
        Labl2.AutoSize = True
        Labl2.Location = New Point(24, 123)
        Labl2.Name = "Labl2"
        Labl2.Size = New Size(150, 15)
        Labl2.TabIndex = 7
        Labl2.Text = "Total Uncompensated Care"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(24, 84)
        Label4.Name = "Label4"
        Label4.Size = New Size(91, 15)
        Label4.TabIndex = 6
        Label4.Text = "Charity Charges"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(24, 164)
        Label3.Name = "Label3"
        Label3.Size = New Size(36, 15)
        Label3.TabIndex = 6
        Label3.Text = "UC %"
        ' 
        ' lblUncompResult
        ' 
        lblUncompResult.AutoSize = True
        lblUncompResult.Location = New Point(185, 42)
        lblUncompResult.Name = "lblUncompResult"
        lblUncompResult.Size = New Size(39, 15)
        lblUncompResult.TabIndex = 5
        lblUncompResult.Text = "Result"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(24, 42)
        Label1.Name = "Label1"
        Label1.Size = New Size(101, 15)
        Label1.TabIndex = 4
        Label1.Text = "Bad Debt Charges"
        ' 
        ' btnOutpatientFinancial
        ' 
        btnOutpatientFinancial.Location = New Point(982, 0)
        btnOutpatientFinancial.Name = "btnOutpatientFinancial"
        btnOutpatientFinancial.Size = New Size(88, 29)
        btnOutpatientFinancial.TabIndex = 20
        btnOutpatientFinancial.Text = "Outp"
        btnOutpatientFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnInpatientFinancial
        ' 
        btnInpatientFinancial.Location = New Point(888, 0)
        btnInpatientFinancial.Name = "btnInpatientFinancial"
        btnInpatientFinancial.Size = New Size(88, 29)
        btnInpatientFinancial.TabIndex = 19
        btnInpatientFinancial.Text = "Inp"
        btnInpatientFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnQualityFinancial
        ' 
        btnQualityFinancial.Location = New Point(794, 0)
        btnQualityFinancial.Name = "btnQualityFinancial"
        btnQualityFinancial.Size = New Size(88, 29)
        btnQualityFinancial.TabIndex = 18
        btnQualityFinancial.Text = "Quality"
        btnQualityFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnFInIndFinancial
        ' 
        btnFInIndFinancial.Location = New Point(700, 0)
        btnFInIndFinancial.Name = "btnFInIndFinancial"
        btnFInIndFinancial.Size = New Size(88, 29)
        btnFInIndFinancial.TabIndex = 17
        btnFInIndFinancial.Text = "Fin Ind"
        btnFInIndFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnDepartmentsFinancial
        ' 
        btnDepartmentsFinancial.Location = New Point(606, 0)
        btnDepartmentsFinancial.Name = "btnDepartmentsFinancial"
        btnDepartmentsFinancial.Size = New Size(88, 29)
        btnDepartmentsFinancial.TabIndex = 15
        btnDepartmentsFinancial.Text = "Departments"
        btnDepartmentsFinancial.UseVisualStyleBackColor = True
        ' 
        ' btnProfileFinancial
        ' 
        btnProfileFinancial.Location = New Point(512, 0)
        btnProfileFinancial.Name = "btnProfileFinancial"
        btnProfileFinancial.Size = New Size(88, 29)
        btnProfileFinancial.TabIndex = 14
        btnProfileFinancial.Text = "Profile"
        btnProfileFinancial.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(606, 37)
        Button1.Name = "Button1"
        Button1.Size = New Size(108, 56)
        Button1.TabIndex = 21
        Button1.Text = "Return to Search"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Panel1
        ' 
        Panel1.AutoScroll = True
        Panel1.BackColor = Color.LightGray
        Panel1.Controls.Add(GroupBox2)
        Panel1.Controls.Add(gbUncompensatedCare)
        Panel1.Controls.Add(Button1)
        Panel1.Controls.Add(GroupBox1)
        Panel1.Controls.Add(btnOutpatientFinancial)
        Panel1.Controls.Add(btnInpatientFinancial)
        Panel1.Controls.Add(gbIncomeStatement)
        Panel1.Controls.Add(btnQualityFinancial)
        Panel1.Controls.Add(btnProfileFinancial)
        Panel1.Controls.Add(btnFInIndFinancial)
        Panel1.Controls.Add(btnDepartmentsFinancial)
        Panel1.Controls.Add(lblNumMonthsFin)
        Panel1.Controls.Add(lbligr)
        Panel1.Controls.Add(lblNumMonthsPeriodResult)
        Panel1.Controls.Add(lblPedResult)
        Panel1.Location = New Point(1, 1)
        Panel1.MinimumSize = New Size(1200, 1000)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1572, 2500)
        Panel1.TabIndex = 22
        ' 
        ' GroupBox2
        ' 
        GroupBox2.BackColor = Color.LightSkyBlue
        GroupBox2.Controls.Add(lblTotLandFbResult)
        GroupBox2.Controls.Add(Label19)
        GroupBox2.Controls.Add(lblCurrentLiab)
        GroupBox2.Controls.Add(Label10)
        GroupBox2.Controls.Add(Label12)
        GroupBox2.Controls.Add(lblTlResult)
        GroupBox2.Controls.Add(Label13)
        GroupBox2.Controls.Add(Label9)
        GroupBox2.Controls.Add(lblCurLiabilitiesRes)
        GroupBox2.Controls.Add(lblLtResult)
        GroupBox2.Controls.Add(lblTotFbResult)
        GroupBox2.Location = New Point(11, 211)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(570, 173)
        GroupBox2.TabIndex = 22
        GroupBox2.TabStop = False
        GroupBox2.Text = "."
        ' 
        ' Financial
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1563, 1061)
        Controls.Add(Panel1)
        Name = "Financial"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Financial"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        gbIncomeStatement.ResumeLayout(False)
        gbIncomeStatement.PerformLayout()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        gbUncompensatedCare.ResumeLayout(False)
        gbUncompensatedCare.PerformLayout()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox2.PerformLayout()
        ResumeLayout(False)
    End Sub
    Friend WithEvents lbligr As Label
    Friend WithEvents gbUncompensatedCare As GroupBox
    Friend WithEvents btnOutpatientFinancial As Button
    Friend WithEvents btnInpatientFinancial As Button
    Friend WithEvents btnQualityFinancial As Button
    Friend WithEvents btnFInIndFinancial As Button
    Friend WithEvents btnDepartmentsFinancial As Button
    Friend WithEvents btnProfileFinancial As Button
    Friend WithEvents lblNumMonthsFin As Label
    Friend WithEvents lblPedResult As Label
    Friend WithEvents lblNumMonthsPeriodResult As Label
    Friend WithEvents lblUncompResult As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblCcResult As Label
    Friend WithEvents lblTotUcResult As Label
    Friend WithEvents lblucpctResult As Label
    Friend WithEvents Labl2 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblTotAssetsResult As Label
    Friend WithEvents lblOtherAssetsResult As Label
    Friend WithEvents lblTotLandFbResult As Label
    Friend WithEvents lblFixAssetsResult As Label
    Friend WithEvents lblCurAssetResult As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblTlResult As Label
    Friend WithEvents lblCurLiabilitiesRes As Label
    Friend WithEvents lblTotFbResult As Label
    Friend WithEvents lblLtResult As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents lblCurrentLiab As Label
    Friend WithEvents lblTotOperatingExpenseResult As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblNetPatRevResult As Label
    Friend WithEvents lblTotPatRevResult As Label
    Friend WithEvents lblOutPatResult As Label
    Friend WithEvents lblInpRevResult As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents lblContractAllowanceResult As Label
    Friend WithEvents Label20 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents lblOperatingIncome As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents Label27 As Label
    Friend WithEvents Label26 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents lblTotOtherExpensesResult As Label
    Friend WithEvents lblTotNonpatientResult As Label
    Friend WithEvents lblMiscNpRevResult As Label
    Friend WithEvents lblGovAppResult As Label
    Friend WithEvents lblIncomeFromInvestResult As Label
    Friend WithEvents lblTotOtherIncomeResult As Label
    Friend WithEvents lblDepreciationExpenseResult As Label
    Friend WithEvents lblNetIncomeResult As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents GroupBox1 As BoldGroupBox
    Friend WithEvents gbIncomeStatement As BoldGroupBox
    Friend WithEvents GroupBox3 As BoldGroupBox
    Friend WithEvents GroupBox2 As BoldGroupBox
End Class

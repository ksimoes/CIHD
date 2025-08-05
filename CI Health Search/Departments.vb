Imports System.Security.Cryptography.X509Certificates
Imports Newtonsoft.Json.Linq

Public Class Departments

    Private currentHospital As HospitalContext

    Private Function GetBestFacilityName(ctx As HospitalContext) As String
        If ctx Is Nothing Then Return "N/A"
        If ctx.LastDataRow IsNot Nothing Then
            Dim row = ctx.LastDataRow
            If row.Table.Columns.Contains("FAC_NAME") Then Return row("FAC_NAME").ToString()
            If row.Table.Columns.Contains("PRVDR_NAME") Then Return row("PRVDR_NAME").ToString()
            If row.Table.Columns.Contains("ORGANIZATION NAME") Then Return row("ORGANIZATION NAME").ToString()
            If row.Table.Columns.Contains("organization_name") Then Return row("organization_name").ToString()
            If row.Table.Columns.Contains("Facility Name") Then Return row("Facility Name").ToString()
            If row.Table.Columns.Contains("Hospital Name") Then Return row("Hospital Name").ToString()
        End If
        If Not String.IsNullOrWhiteSpace(ctx.Name) Then Return ctx.Name
        Return "N/A"
    End Function

    ' New constructor to accept a HospitalContext
    Public Sub New(hosp As HospitalContext)
        InitializeComponent()
        currentHospital = hosp
    End Sub

    ' Default constructor for designer compatibility
    Public Sub New()
        InitializeComponent()
    End Sub
    Public Async Sub ShowDepartmentsDataApi(cmsNum As String)
        ' This method can be used to show departments data if needed
        ' Currently, it does not perform any actions
        Dim newURL As String = "https://data.cms.gov/data-api/v1/dataset/8ba0f9b4-9493-4aa0-9f82-44ea9468d1b5/data?"
        Dim filters As New List(Of String)
        If Not String.IsNullOrEmpty(cmsNum) Then filters.Add("keyword=" & Uri.EscapeDataString(cmsNum))
        'If Not String.IsNullOrEmpty(npi) Then filters.Add("keyword=" & Uri.EscapeDataString(npi))
        newURL &= String.Join("&", filters)
        filters.Add("size=1000")

        Dim myArray As JArray = Await GetAPIArrayAsync(newURL)


        If myArray.Count > 0 Then
            ' Find the exact match for Provider CCN if possible
            Dim provider = myArray.FirstOrDefault(Function(x) x("PRVDR_NUM") IsNot Nothing AndAlso x("PRVDR_NUM").ToString() = cmsNum)
            If provider Is Nothing Then provider = myArray(0)

            lblCrnaNumResult.Text = If(provider("CRNA_CNT") IsNot Nothing, provider("CRNA_CNT").ToString(), "N/A")
            lblDietNumResult.Text = If(provider("DIETN_CNT") IsNot Nothing, provider("DIETN_CNT").ToString(), "N/A")
            lblLpnNumResult.Text = If(provider("LPN_CNT") IsNot Nothing, provider("LPN_CNT").ToString(), "N/A")
            lblInhalationTherapistResult.Text = If(provider("INHLTN_THRPST_CNT") IsNot Nothing, provider("INHLTN_THRPST_CNT").ToString(), "N/A")
            lblRNNumResult.Text = If(provider("RN_CNT") IsNot Nothing, provider("RN_CNT").ToString(), "N/A")
            lblSPANumResult.Text = If(provider("SPCH_PTHLGST_AUDLGST_CNT") IsNot Nothing, provider("SPCH_PTHLGST_AUDLGST_CNT").ToString(), "N/A")
            lblOTNumResult.Text = If(provider("OCPTNL_THRPST_CNT") IsNot Nothing, provider("OCPTNL_THRPST_CNT").ToString(), "N/A")
            lblPANumResult.Text = If(provider("PHYSN_ASTNT_CNT") IsNot Nothing, provider("PHYSN_ASTNT_CNT").ToString(), "N/A")
            lblPharmacistsNumResult.Text = If(provider("REG_PHRMCST_CNT") IsNot Nothing, provider("REG_PHRMCST_CNT").ToString(), "N/A")
            lblSWNumResult.Text = If(provider("SCL_WORKR_CNT") IsNot Nothing, provider("SCL_WORKR_CNT").ToString(), "N/A")




        Else
            lblCrnaNumResult.Text = "No result"
            lblDietNumResult.Text = "No result"
            lblLpnNumResult.Text = "No result"
            lblInhalationTherapistResult.Text = "No result"
            lblRNNumResult.Text = "No result"
            lblSPANumResult.Text = "No result"
            lblOTNumResult.Text = "No result"
            lblPANumResult.Text = "No result"
            lblPharmacistsNumResult.Text = "No result"
            lblSWNumResult.Text = "No result"







        End If
    End Sub

    Private Sub Departments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If currentHospital IsNot Nothing Then
            ShowDepartmentsDataApi(currentHospital.CMSNum)
        Else
            ' Optionally clear labels or show a message
            ShowDepartmentsDataApi("")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileDepartments.Click
        Me.Hide()
        Dim cmsNum As String = currentHospital.CMSNum
        Dim fullContext As HospitalContext = Results.GetHospitalContextByCMSNum(cmsNum)
        If fullContext IsNot Nothing Then
            Dim profileForm As New Profile(fullContext)
            profileForm.Show()
        Else
            MessageBox.Show("Could not reload full hospital context.")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialDepartments.Click
        Me.Hide()
        Dim financialForm As New Financial(currentHospital)
        financialForm.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndDepartments.Click
        Me.Hide()
        Dim finIndForm As New FinInd(currentHospital)
        finIndForm.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityDepartments.Click
        Me.Hide()
        Dim qualityForm As New Quality(currentHospital)
        qualityForm.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientDepartments.Click
        Me.Hide()
        Dim inpatientForm As New Inpatient(currentHospital)
        inpatientForm.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientDepartments.Click
        Me.Hide()
        Dim outpatientForm As New Outpatient(currentHospital)
        outpatientForm.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
Imports System.Security.Cryptography.X509Certificates
Imports Newtonsoft.Json.Linq

Public Class Departments
    Public Sub ShowDepartmentsDataApi(cmsNum As String)
        ' This method can be used to show departments data if needed
        ' Currently, it does not perform any actions
        Dim newURL As String = "https://data.cms.gov/data-api/v1/dataset/8ba0f9b4-9493-4aa0-9f82-44ea9468d1b5/data?"
        Dim filters As New List(Of String)
        If Not String.IsNullOrEmpty(cmsNum) Then filters.Add("keyword=" & Uri.EscapeDataString(cmsNum))
        'If Not String.IsNullOrEmpty(npi) Then filters.Add("keyword=" & Uri.EscapeDataString(npi))
        newURL &= String.Join("&", filters)
        filters.Add("size=1000")

        Dim myArray As JArray = Profile.GetAPIArray(newURL)


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

        ShowDepartmentsDataApi(Results.SelectedHospital.CMSNum)



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileDepartments.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialDepartments.Click
        Me.Hide()
        Financial.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndDepartments.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityDepartments.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientDepartments.Click
        Me.Hide()
        Inpatient.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientDepartments.Click
        Me.Hide()
        Outpatient.Show()

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
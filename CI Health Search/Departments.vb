Imports Newtonsoft.Json.Linq

Public Class Departments
    ' Show department data using the centralized API helper
    Public Async Function ShowDepartmentsDataApi(cmsNum As String) As Task
        lblstatus.Text = "Loading department data..."
        lblstatus.Visible = True

        Try
            Dim url As String = ApiHelper.ApiUrls("DepartmentsApi") & "?keyword=" & Uri.EscapeDataString(cmsNum) & "&size=1000"
            Dim myArray As JArray = Await ApiHelper.GetApiDataAsync(url)

            If myArray.Count > 0 Then
                Dim provider = myArray.FirstOrDefault(Function(x) x("PRVDR_NUM") IsNot Nothing AndAlso x("PRVDR_NUM").ToString() = cmsNum)
                If provider Is Nothing Then provider = myArray(0)

                lblCrnaNumResult.Text = AppHelpers.SafeGet(provider, "CRNA_CNT")
                lblDietNumResult.Text = AppHelpers.SafeGet(provider, "DIETN_CNT")
                lblLpnNumResult.Text = AppHelpers.SafeGet(provider, "LPN_CNT")
                lblInhalationTherapistResult.Text = AppHelpers.SafeGet(provider, "INHLTN_THRPST_CNT")
                lblRNNumResult.Text = AppHelpers.SafeGet(provider, "RN_CNT")
                lblSPANumResult.Text = AppHelpers.SafeGet(provider, "SPCH_PTHLGST_AUDLGST_CNT")
                lblOTNumResult.Text = AppHelpers.SafeGet(provider, "OCPTNL_THRPST_CNT")
                lblPANumResult.Text = AppHelpers.SafeGet(provider, "PHYSN_ASTNT_CNT")
                lblPharmacistsNumResult.Text = AppHelpers.SafeGet(provider, "REG_PHRMCST_CNT")
                lblSWNumResult.Text = AppHelpers.SafeGet(provider, "SCL_WORKR_CNT")
            Else
                SetAllDepartmentLabels("No result")
            End If

            lblStatus.Text = ""
        Catch ex As Exception
            SetAllDepartmentLabels("No result")
            lblStatus.Text = "Error loading department data. Please try again."
        End Try

        lblStatus.Visible = False
    End Function

    ' Helper: Set all department labels to a value
    Private Sub SetAllDepartmentLabels(val As String)
        lblCrnaNumResult.Text = val
        lblDietNumResult.Text = val
        lblLpnNumResult.Text = val
        lblInhalationTherapistResult.Text = val
        lblRNNumResult.Text = val
        lblSPANumResult.Text = val
        lblOTNumResult.Text = val
        lblPANumResult.Text = val
        lblPharmacistsNumResult.Text = val
        lblSWNumResult.Text = val
    End Sub

    Private Async Sub Departments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await ShowDepartmentsDataApi(Results.SelectedHospital.CMSNum)
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
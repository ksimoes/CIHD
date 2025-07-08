Public Class Inpatient
    'NOTE MEDICARE_PROV_NUM = CCN Number
    Dim strURLPatientOrigin As String = "https://data.cms.gov/data-api/v1/dataset/8708ca8b-8636-44ed-8303-724cbfaf78ad/data"
    Dim strURLPatientOrigin2019 As String = "https://data.cms.gov/data-api/v1/dataset/2713ba99-c59e-4b25-9a3d-3661d35988da/data"
    Dim strURLPatientOrigin2023 As String = "https://data.cms.gov/data-api/v1/dataset/7f749f00-bfa9-4377-9a98-90c15cacc2f3/data"


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileInpatient.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentsInpatient.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialInpatient.Click
        Me.Hide()
        Financial.Show()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndInpatient.Click
        Me.Hide()
        FinInd.Show()

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityInpatient.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientInpatient.Click
        Me.Hide()
        Outpatient.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub

    Private Sub Label14_Click(sender As Object, e As EventArgs) Handles Label14.Click

    End Sub

    Private Sub dgvPatientOrigin_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPatientOrigin.CellContentClick

    End Sub


End Class
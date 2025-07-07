Public Class Inpatient
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
End Class
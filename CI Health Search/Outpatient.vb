Public Class Outpatient
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileOutpatient.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentsOutpatient.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialOutpatient.Click
        Me.Hide()
        Financial.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndOutpatient.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityOutpatient.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientOutpatient.Click
        Me.Hide()
        Inpatient.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
Public Class Financial
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileFinancial.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentsFinancial.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFInIndFinancial.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityFinancial.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientFinancial.Click
        Me.Hide()
        Inpatient.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientFinancial.Click
        Me.Hide()
        Outpatient.Show()
    End Sub
End Class
Public Class Quality
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileQuality.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentsQuality.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialQuality.Click
        Me.Hide()
        Financial.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndQuality.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientQuality.Click
        Me.Hide()
        Inpatient.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientQuality.Click
        Me.Hide()
        Outpatient.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
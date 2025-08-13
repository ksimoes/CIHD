Public Class SelectSearchTypeForm
    Public Property SelectedType As String = Nothing

    Private Sub btnIndividual_Click(sender As Object, e As EventArgs) Handles btnIndividual.Click
        SelectedType = "Individual"
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnOrganization_Click(sender As Object, e As EventArgs) Handles btnOrganization.Click
        SelectedType = "Organization"
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub


End Class
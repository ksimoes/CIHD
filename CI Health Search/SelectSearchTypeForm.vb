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

    Private Sub btnBoth_Click(sender As Object, e As EventArgs) Handles btnBoth.Click
        ' Show the individual search form
        Dim indForm As New Individual_Search()
        indForm.Show()

        ' Show the organization search form
        Dim orgForm As New Search() ' Replace 'Search' with your actual organization search form class if different
        orgForm.Show()

        ' Optionally, hide or close the select type form
        Me.Hide()
    End Sub

End Class
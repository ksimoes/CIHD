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
        ' Open both forms modelessly so they can be used at the same time
        Dim indForm As New Individual_Search()
        indForm.Show() ' Modeless

        Dim orgForm As New Search() ' Replace 'Search' with your actual organization search form class if different
        orgForm.Show() ' Modeless

        ' Hide this selector so it doesn't block interaction
        Me.Hide()
    End Sub

End Class
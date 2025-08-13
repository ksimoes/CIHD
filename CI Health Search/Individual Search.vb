Imports System.Net.NetworkInformation

Public Class Individual_Search
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub btnOrgSearch_Click(sender As Object, e As EventArgs) Handles btnOrgSearch.Click
        Search.Show()
        Me.Hide()

    End Sub

    Private Async Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim npi As String = tbNpi.Text.Trim()
        If Not String.IsNullOrWhiteSpace(npi) Then
            ' Go straight to profile if NPI is provided
            Dim profileForm As New IndividualProfileForm(npi)
            profileForm.ShowDialog()
            Return
        End If

        ' Otherwise, search by other criteria
        Dim firstName As String = tbFirst.Text.Trim()
        Dim lastName As String = tbLast.Text.Trim()
        Dim state As String = tbState.Text.Trim()
        ' Add more fields as needed

        Dim results = Await IndividualApiHelper.SearchNpiRegistryAsync(firstName:=firstName, lastName:=lastName, state:=state, limit:=25)
        If results Is Nothing OrElse results.Count = 0 Then
            MessageBox.Show("No individuals found with the given criteria.")
            Return
        End If

        ' Show results in a popup for user to select
        Dim resultsForm As New IndividualResultsForm(results)
        If resultsForm.ShowDialog() = DialogResult.OK AndAlso Not String.IsNullOrEmpty(resultsForm.SelectedNpi) Then
            Dim profileForm As New IndividualProfileForm(resultsForm.SelectedNpi)
            profileForm.ShowDialog()
        End If
    End Sub

End Class
Imports System.Net.NetworkInformation

Public Class Individual_Search
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub btnOrgSearch_Click(sender As Object, e As EventArgs) Handles btnOrgSearch.Click
        Search.Show()
        Me.Hide()

    End Sub

    Private Async Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim npi = tbNpi.Text.Trim()
        Dim hcpcs = tbHCPCS.Text.Trim()
        Dim firstName = tbFirst.Text.Trim()
        Dim lastName = tbLast.Text.Trim()
        Dim state = tbState.Text.Trim()

        ' 1. If both NPI and HCPCS are provided, go straight to profile
        If Not String.IsNullOrWhiteSpace(npi) AndAlso Not String.IsNullOrWhiteSpace(hcpcs) Then
            Dim profileForm As New IndividualProfileForm(npi)
            profileForm.ShowDialog()
            Return
        End If

        ' 2. If only NPI is provided, go straight to profile
        If Not String.IsNullOrWhiteSpace(npi) Then
            Dim profileForm As New IndividualProfileForm(npi)
            profileForm.ShowDialog()
            Return
        End If

        ' 3. If only HCPCS is provided, search by code and show results
        If Not String.IsNullOrWhiteSpace(hcpcs) Then
            Dim results = Await IndividualApiHelper.SearchByHCPCSAsync(hcpcs)
            If results Is Nothing OrElse results.Count = 0 Then
                MessageBox.Show("No individuals found for this HCPCS code.")
                Return
            End If
            Dim resultsForm As New IndividualResultsForm(results)
            If resultsForm.ShowDialog() = DialogResult.OK AndAlso Not String.IsNullOrEmpty(resultsForm.SelectedNpi) Then
                Dim profileForm As New IndividualProfileForm(resultsForm.SelectedNpi)
                profileForm.ShowDialog()
            End If
            Return
        End If

        ' 4. Otherwise, search by name/state/etc.
        Dim results2 = Await IndividualApiHelper.SearchNpiRegistryAsync(firstName:=firstName, lastName:=lastName, state:=state, limit:=25)
        If results2 Is Nothing OrElse results2.Count = 0 Then
            MessageBox.Show("No individuals found with the given criteria.")
            Return
        End If
        Dim resultsForm2 As New IndividualResultsForm(results2)
        If resultsForm2.ShowDialog() = DialogResult.OK AndAlso Not String.IsNullOrEmpty(resultsForm2.SelectedNpi) Then
            Dim profileForm As New IndividualProfileForm(resultsForm2.SelectedNpi)
            profileForm.ShowDialog()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        tbFirst.Clear()
        tbLast.Clear()
        tbNpi.Clear()
        tbState.Clear()
        tbTS.Clear()
        tbMiddle.Clear()
        tbAddress.Clear()
        tbCity.Clear()
        tbZip.Clear()
        tbAT.Clear()
        tbGender.Clear()
        tbHCPCS.Clear()
        tbStLic.Clear()
        tbStLicNum.Clear()
        tbProvEnroll.Clear()
        tbFacilityTyp.Clear()
        tbGradYear.Clear()
        tbMedSchool.Clear()

    End Sub
End Class
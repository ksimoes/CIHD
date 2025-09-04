Imports System.Net.NetworkInformation
Imports Newtonsoft.Json.Linq

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
        Dim state = tbState.Text.Trim().ToUpper()
        Dim stLic = tbStLic.Text.Trim().ToUpper()
        Dim provEnroll = tbProvEnroll.Text.Trim()
        Dim facilityTyp = tbFacilityTyp.Text.Trim()
        Dim gradYear = tbGradYear.Text.Trim()
        Dim medSchool = tbMedSchool.Text.Trim()
        Dim brandName = tbDrug.Text.Trim()
        Dim stLicNum = tbStLicNum.Text.Trim()
        Dim genericName = tbDrugGeneric.Text.Trim()
        Dim middleName = tbMiddle.Text.Trim()
        Dim city = tbCity.Text.Trim()
        Dim zip = tbZip.Text.Trim()
        Dim gender = tbGender.Text.Trim()

        ' Build a summary of the search criteria
        Dim filters As New List(Of String)
        If Not String.IsNullOrWhiteSpace(npi) Then filters.Add($"NPI: {npi}")
        If Not String.IsNullOrWhiteSpace(hcpcs) Then filters.Add($"HCPCS: {hcpcs}")
        If Not String.IsNullOrWhiteSpace(firstName) Then filters.Add($"First Name: {firstName}")
        If Not String.IsNullOrWhiteSpace(middleName) Then filters.Add($"Middle Name: {middleName}")
        If Not String.IsNullOrWhiteSpace(lastName) Then filters.Add($"Last Name: {lastName}")
        If Not String.IsNullOrWhiteSpace(state) Then filters.Add($"State: {state}")
        If Not String.IsNullOrWhiteSpace(city) Then filters.Add($"City: {city}")
        If Not String.IsNullOrWhiteSpace(zip) Then filters.Add($"Zip: {zip}")
        If Not String.IsNullOrWhiteSpace(gender) Then filters.Add($"Gender: {gender}")
        If Not String.IsNullOrWhiteSpace(stLic) Then filters.Add($"License State: {stLic}")
        If Not String.IsNullOrWhiteSpace(stLicNum) Then filters.Add($"License Number: {stLicNum}")
        If Not String.IsNullOrWhiteSpace(provEnroll) Then filters.Add($"Provider Enrollment: {provEnroll}")
        If Not String.IsNullOrWhiteSpace(facilityTyp) Then filters.Add($"Facility Type: {facilityTyp}")
        If Not String.IsNullOrWhiteSpace(gradYear) Then filters.Add($"Grad Year: {gradYear}")
        If Not String.IsNullOrWhiteSpace(medSchool) Then filters.Add($"Med School: {medSchool}")
        If Not String.IsNullOrWhiteSpace(brandName) Then filters.Add($"Brand: {brandName}")
        If Not String.IsNullOrWhiteSpace(genericName) Then filters.Add($"Generic: {genericName}")
        Dim searchSummary As String = "Search Filters: " & String.Join(" | ", filters)

        ' 1. If both NPI and HCPCS are provided, go straight to profile
        If Not String.IsNullOrWhiteSpace(npi) AndAlso Not String.IsNullOrWhiteSpace(hcpcs) Then
            Dim profileForm As New IndividualProfileForm(npi)
            profileForm.Show()
            Return
        End If

        ' 2. If only NPI is provided, go straight to profile
        If Not String.IsNullOrWhiteSpace(npi) Then
            Dim profileForm As New IndividualProfileForm(npi)
            profileForm.Show()
            Return
        End If

        ' 3. If HCPCS is provided, search by code and show results (with state filter)
        If Not String.IsNullOrWhiteSpace(hcpcs) Then
            Dim results = Await IndividualApiHelper.SearchByHCPCSAsync(hcpcs, state)
            If results Is Nothing OrElse results.Count = 0 Then
                MessageBox.Show("No individuals found for this HCPCS code.")
                Return
            End If
            Dim resultsForm As New IndividualResultsForm(results, showDrugColumns:=False, searchSummary:=searchSummary)
            resultsForm.Show()
            Return
        End If

        ' 4. If searching by drug brand or generic name
        If Not String.IsNullOrWhiteSpace(brandName) OrElse Not String.IsNullOrWhiteSpace(genericName) Then
            Dim results = Await IndividualApiHelper.SearchByDrugAsync(brandName, genericName)
            If results Is Nothing OrElse results.Count = 0 Then
                MessageBox.Show("No individuals found for this drug.")
                Return
            End If
            Dim resultsForm As New IndividualResultsForm(results, showDrugColumns:=True, searchSummary:=searchSummary)
            resultsForm.Show()
            Return
        End If

        ' 5. Otherwise, search by name/state/etc.
        ' Check for state-only or license state-only search
        Dim onlyState = Not String.IsNullOrWhiteSpace(state) AndAlso
            String.IsNullOrWhiteSpace(firstName) AndAlso
            String.IsNullOrWhiteSpace(lastName) AndAlso
            String.IsNullOrWhiteSpace(city) AndAlso
            String.IsNullOrWhiteSpace(zip) AndAlso
            String.IsNullOrWhiteSpace(middleName) AndAlso
            String.IsNullOrWhiteSpace(gender) AndAlso
            String.IsNullOrWhiteSpace(stLic)

        Dim onlyStLic = Not String.IsNullOrWhiteSpace(stLic) AndAlso
            String.IsNullOrWhiteSpace(firstName) AndAlso
            String.IsNullOrWhiteSpace(lastName) AndAlso
            String.IsNullOrWhiteSpace(city) AndAlso
            String.IsNullOrWhiteSpace(zip) AndAlso
            String.IsNullOrWhiteSpace(middleName) AndAlso
            String.IsNullOrWhiteSpace(gender) AndAlso
            String.IsNullOrWhiteSpace(state)

        If onlyState OrElse onlyStLic Then
            MessageBox.Show("Please enter at least one additional search field (such as name, city, or zip) when searching by state or license state.")
            Return
        End If

        ' --- KEY CHANGE: Use NDF if searching by grad year or med school ---
        Dim results2 As JArray = Nothing
        If Not String.IsNullOrWhiteSpace(gradYear) OrElse Not String.IsNullOrWhiteSpace(medSchool) Then
            results2 = Await IndividualApiHelper.SearchNationalDownloadableFileAsync(
                npi:=npi,
                firstName:=firstName,
                lastName:=lastName,
                gradYear:=gradYear,
                medSchool:=medSchool,
                state:=state,
                limit:=25
            )
        Else
            results2 = Await IndividualApiHelper.SearchNpiRegistryAsync(
                firstName:=firstName,
                middleName:=middleName,
                lastName:=lastName,
                city:=city,
                state:=state,
                zip:=zip,
                gender:=gender,
                licenseState:=stLic,
                licenseNumber:=stLicNum,
                enumerationType:="NPI-1",
                taxonomyDescription:=facilityTyp,
                graduationYear:=gradYear,
                medicalSchool:=medSchool,
                limit:=25
            )
        End If

        If results2 Is Nothing OrElse results2.Count = 0 Then
            MessageBox.Show("No individuals found with the given criteria.")
            Return
        End If
        Dim resultsForm2 As New IndividualResultsForm(results2, showDrugColumns:=False, searchSummary:=searchSummary)
        resultsForm2.Show()
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
        tbDrug.Clear()
        tbDrugGeneric.Clear()
    End Sub
End Class
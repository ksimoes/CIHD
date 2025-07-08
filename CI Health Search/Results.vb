Public Class Results

    Public Property SelectedState As String
    Private resultsTable As DataTable
    Public strCMSnum As String
    ' Shared context for selected hospital
    Public Shared SelectedHospital As New HospitalContext()

    ' Call this from Search form to set and display results
    Public Sub SetResults(dt As DataTable)
        resultsTable = dt

        CheckedListBox1.Items.Clear()
        ' Determine which column to use for display
        Dim displayCol As String = ""
        If dt.Columns.Contains("Facility Name") Then
            displayCol = "Facility Name"
        ElseIf dt.Columns.Contains("Hospital Name") Then
            displayCol = "Hospital Name"
        ElseIf dt.Columns.Count > 0 Then
            displayCol = dt.Columns(0).ColumnName ' fallback
        End If

        For Each row As DataRow In dt.Rows
            CheckedListBox1.Items.Add(row(displayCol).ToString())
        Next
        Dim items As New List(Of String)
        For Each item In CheckedListBox1.Items
            items.Add(item.ToString())
        Next

        ' Sort the list
        items.Sort()

        ' Clear the CheckedListBox and re-add sorted items
        CheckedListBox1.Items.Clear()
        For Each item In items
            CheckedListBox1.Items.Add(item)
        Next

    End Sub

    ' Profile button click
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckedListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If
        retrieveProfile()
        Profile.ShowProfile(SelectedHospital)
        Hide()
        Profile.Show()
    End Sub

    Public Function GetSelectedHospital() As HospitalContext
        retrieveProfile()
        Return SelectedHospital
    End Function

    Public Sub retrieveProfile()
        Dim selectedName As String = CheckedListBox1.SelectedItem.ToString().Trim()
        Dim selectedRow As DataRow = Nothing

        ' Match on the correct column
        If resultsTable.Columns.Contains("Facility Name") Then
            selectedRow = resultsTable.Select($"[Facility Name] = '{selectedName.Replace("'", "''")}'").FirstOrDefault()
        ElseIf resultsTable.Columns.Contains("Hospital Name") Then
            selectedRow = resultsTable.Select($"[Hospital Name] = '{selectedName.Replace("'", "''")}'").FirstOrDefault()
            SelectedHospital.CMSNum = Search.CleanMeUp(selectedRow(1))
            SelectedHospital.State = selectedRow(5)
            SelectedHospital.Name = Search.CleanMeUp(selectedRow(2))
            SelectedHospital.Address = Search.CleanMeUp(selectedRow(3))
            SelectedHospital.City = Search.CleanMeUp(selectedRow(4))
            SelectedHospital.CBSAnum = Search.CleanMeUp(selectedRow(8))
            SelectedHospital.TotalDays = Search.CleanMeUp(selectedRow(20), True)
            SelectedHospital.FacilityType = Search.CleanMeUp(selectedRow(10))
            SelectedHospital.NumOfBeds = Search.CleanMeUp(selectedRow(21), True)
            SelectedHospital.NumOfEmployees = Search.CleanMeUp(selectedRow(15))
            SelectedHospital.TotalDischarges = Search.CleanMeUp(selectedRow(26), True)
            SelectedHospital.TotalPatientRev = Search.CleanMeUp(selectedRow(105), True)
            SelectedHospital.NetPatientRev = Search.CleanMeUp(selectedRow(107), True)
            SelectedHospital.RuralOUrban = Search.CleanMeUp(selectedRow(9))
            SelectedHospital.charityCost = Search.CleanMeUp(selectedRow(38), True)
            SelectedHospital.uncompensatedCost = Search.CleanMeUp(selectedRow(40), True)
        End If

        If selectedRow IsNot Nothing Then
            Dim hospitalId As Integer = 0
            If resultsTable.Columns.Contains("LicenseNum") Then
                Integer.TryParse(selectedRow("LicenseNum").ToString(), hospitalId)
            End If

            Dim npi As String = If(resultsTable.Columns.Contains("NPI"), selectedRow("NPI").ToString(), "")
            Dim cmsNum As String = If(resultsTable.Columns.Contains("CMSNum"), selectedRow("CMSNum").ToString(), "")

            'SelectedHospital.HospitalId = hospitalId


            If (resultsTable.Columns.Count < 6) Then
                SelectedHospital.State = selectedRow(2)
                SelectedHospital.HospitalId = selectedRow(0)
                SelectedHospital.CMSNum = selectedRow(3)

            Else
                SelectedHospital.State = selectedRow(5)
                SelectedHospital.HospitalId = selectedRow(1)
                SelectedHospital.CMSNum = cmsNum
            End If


            SelectedHospital.CMSNum = selectedRow(1)


        End If
    End Sub

    Private Sub Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


End Class
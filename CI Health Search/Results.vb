Public Class Results

    Public Property SelectedState As String
    Private resultsTable As DataTable
    Public strCMSnum As String
    ' Shared context for selected hospital
    Public Shared SelectedHospitalContext As New HospitalContext()

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
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckedListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If

        Dim selectedName As String = CheckedListBox1.SelectedItem.ToString().Trim()
        Dim selectedRow As DataRow = Nothing

        ' Match on the correct column
        If resultsTable.Columns.Contains("Facility Name") Then
            selectedRow = resultsTable.Select($"[Facility Name] = '{selectedName.Replace("'", "''")}'").FirstOrDefault()
        ElseIf resultsTable.Columns.Contains("Hospital Name") Then
            selectedRow = resultsTable.Select($"[Hospital Name] = '{selectedName.Replace("'", "''")}'").FirstOrDefault()
            SelectedHospitalContext.CMSNum = selectedRow(1)
            SelectedHospitalContext.State = selectedRow(5)
            SelectedHospitalContext.Name = selectedRow(2)

        End If

        If selectedRow IsNot Nothing Then
            Dim hospitalId As Integer = 0
            If resultsTable.Columns.Contains("LicenseNum") Then
                Integer.TryParse(selectedRow("LicenseNum").ToString(), hospitalId)
            End If

            Dim npi As String = If(resultsTable.Columns.Contains("NPI"), selectedRow("NPI").ToString(), "")
            Dim cmsNum As String = If(resultsTable.Columns.Contains("CMSNum"), selectedRow("CMSNum").ToString(), "")

            'SelectedHospitalContext.HospitalId = hospitalId


            If (resultsTable.Columns.Count < 6) Then
                SelectedHospitalContext.State = selectedRow(2)
                SelectedHospitalContext.HospitalId = selectedRow(0)
                SelectedHospitalContext.CMSNum = selectedRow(3)

            Else
                SelectedHospitalContext.State = selectedRow(5)
                SelectedHospitalContext.HospitalId = selectedRow(1)
                SelectedHospitalContext.CMSNum = cmsNum
            End If


            SelectedHospitalContext.CMSNum = selectedRow(1)

            Profile.ShowProfile(SelectedHospitalContext.HospitalId, SelectedHospitalContext.State, npi, SelectedHospitalContext.CMSNum)
            Hide()
            Profile.Show()
        End If
    End Sub

    Private Sub Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


End Class
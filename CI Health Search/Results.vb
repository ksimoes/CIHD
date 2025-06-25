Public Class Results

    Public Property SelectedState As String
    Private resultsTable As DataTable

    ' Shared context for selected hospital
    Public Shared SelectedHospitalContext As New HospitalContext()

    ' Call this from Search form to set and display results
    Public Sub SetResults(dt As DataTable)
        resultsTable = dt
        CheckedListBox1.Items.Clear()
        For Each row As DataRow In dt.Rows
            ' Display hospital name, but store HospitalID for lookup
            CheckedListBox1.Items.Add(row("Facility Name").ToString())
        Next
    End Sub

    ' Profile button click
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckedListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If

        Dim selectedName As String = CheckedListBox1.SelectedItem.ToString().Trim()
        Dim selectedRow = resultsTable.Select($"[Facility Name] = '{selectedName.Replace("'", "''")}'").FirstOrDefault()
        If selectedRow IsNot Nothing Then
            Dim hospitalId As Integer = CInt(selectedRow("LicenseNum"))

            If String.IsNullOrEmpty(SelectedState) Then
                MessageBox.Show("State information is missing.")
                Return
            End If

            ' Set the shared context
            SelectedHospitalContext.HospitalId = hospitalId
            SelectedHospitalContext.State = SelectedState

            Profile.ShowProfile(hospitalId, SelectedState)
            Hide()
            Profile.Show()
        End If
    End Sub

End Class
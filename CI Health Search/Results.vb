Public Class Results
    Private resultsTable As DataTable

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

        ' Get selected hospital's HospitalID
        Dim selectedName As String = CheckedListBox1.SelectedItem.ToString()
        Dim selectedRow = resultsTable.Select($"[Facility Name] = '{selectedName.Replace("'", "''")}'").FirstOrDefault()
        If selectedRow IsNot Nothing Then
            Dim hospitalId As Integer = CInt(selectedRow("LicenseNum"))
            Profile.ShowProfile(hospitalId)
            Hide()
            Profile.Show()
        End If
    End Sub
End Class
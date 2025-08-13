Imports Newtonsoft.Json.Linq

Public Class IndividualResultsForm
    Public Property SelectedNpi As String = Nothing

    Public Sub New(results As JArray)
        InitializeComponent()
        ' Clear any existing columns
        DataGridView1.Columns.Clear()

        ' Add columns for NPI, First Name, Last Name, State
        DataGridView1.Columns.Add("NPI", "NPI")
        DataGridView1.Columns.Add("FirstName", "First Name")
        DataGridView1.Columns.Add("LastName", "Last Name")
        DataGridView1.Columns.Add("State", "State")
        ' Populate DataGridView with results
        For Each person As JObject In results
            Dim npi = person("number")?.ToString()
            Dim firstName = person("basic")?("first_name")?.ToString()
            Dim lastName = person("basic")?("last_name")?.ToString()
            Dim state = person("addresses")?(0)?("state")?.ToString()
            DataGridView1.Rows.Add(npi, firstName, lastName, state)
        Next
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then
            SelectedNpi = DataGridView1.Rows(e.RowIndex).Cells("NPI").Value.ToString()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class
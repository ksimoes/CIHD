Imports Newtonsoft.Json.Linq

Public Class IndividualResultsForm
    Public Property SelectedNpi As String = Nothing

    Public Sub New(results As JArray)
        InitializeComponent()
        DataGridView1.Columns.Clear()

        If results.Count > 0 Then
            Dim firstObj As JObject = CType(results(0), JObject)
            ' Detect if this is an HCPCS result (has "Rndrng_NPI") or NPI Registry (has "number")
            If firstObj.ContainsKey("Rndrng_NPI") Then
                ' HCPCS dataset
                DataGridView1.Columns.Add("NPI", "NPI")
                DataGridView1.Columns.Add("FirstName", "First Name")
                DataGridView1.Columns.Add("LastName", "Last Name")
                DataGridView1.Columns.Add("State", "State")
                For Each person As JObject In results
                    Dim npi = person("Rndrng_NPI")?.ToString()
                    Dim firstName = person("Rndrng_Prvdr_First_Name")?.ToString()
                    Dim lastName = person("Rndrng_Prvdr_Last_Org_Name")?.ToString()
                    Dim state = person("Rndrng_Prvdr_State_Abrvtn")?.ToString()
                    DataGridView1.Rows.Add(npi, firstName, lastName, state)
                Next
            Else
                ' NPI Registry dataset
                DataGridView1.Columns.Add("NPI", "NPI")
                DataGridView1.Columns.Add("FirstName", "First Name")
                DataGridView1.Columns.Add("LastName", "Last Name")
                DataGridView1.Columns.Add("State", "State")
                For Each person As JObject In results
                    Dim npi = person("number")?.ToString()
                    Dim firstName = person("basic")?("first_name")?.ToString()
                    Dim lastName = person("basic")?("last_name")?.ToString()
                    Dim state = person("addresses")?(0)?("state")?.ToString()
                    DataGridView1.Rows.Add(npi, firstName, lastName, state)
                Next
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then
            SelectedNpi = DataGridView1.Rows(e.RowIndex).Cells("NPI").Value.ToString()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class
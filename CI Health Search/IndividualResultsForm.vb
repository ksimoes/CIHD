Imports Newtonsoft.Json.Linq

Public Class IndividualResultsForm
    Public Property SelectedNpi As String = Nothing

    Public Sub New(results As JArray, Optional showDrugColumns As Boolean = False)
        InitializeComponent()
        DataGridView1.Columns.Clear()

        If results.Count > 0 Then
            Dim firstObj As JObject = CType(results(0), JObject)

            ' Drug search: Brnd_Name or Gnrc_Name present, or showDrugColumns forced
            If showDrugColumns OrElse firstObj.ContainsKey("Brnd_Name") OrElse firstObj.ContainsKey("Gnrc_Name") Then
                DataGridView1.Columns.Add("NPI", "NPI")
                DataGridView1.Columns.Add("FirstName", "First Name")
                DataGridView1.Columns.Add("LastName", "Last Name")
                DataGridView1.Columns.Add("Brnd_Name", "Brand Name")
                DataGridView1.Columns.Add("Gnrc_Name", "Generic Name")
                For Each person As JObject In results
                    Dim npi = person("Prscrbr_NPI")?.ToString()
                    Dim firstName = person("Prscrbr_First_Name")?.ToString()
                    Dim lastName = person("Prscrbr_Last_Name")?.ToString()
                    Dim brnd = person("Brnd_Name")?.ToString()
                    Dim gnrc = person("Gnrc_Name")?.ToString()
                    DataGridView1.Rows.Add(npi, firstName, lastName, brnd, gnrc)
                Next

            ElseIf firstObj.ContainsKey("Rndrng_NPI") Then
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
                    Dim firstName = If(person("basic")?("first_name") IsNot Nothing, person("basic")("first_name").ToString(), "N/A")
                    Dim lastName = If(person("basic")?("last_name") IsNot Nothing, person("basic")("last_name").ToString(), "N/A")
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
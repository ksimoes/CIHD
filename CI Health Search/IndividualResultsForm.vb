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
                DataGridView1.Columns.Add("Specialty", "Specialty")
                DataGridView1.Columns.Add("Phone", "Phone")
                DataGridView1.Columns.Add("Address", "Address")
                For Each person As JObject In results
                    Dim npi = person("number")?.ToString()
                    Dim firstName = If(person("basic")?("first_name") IsNot Nothing, person("basic")("first_name").ToString(), "N/A")
                    Dim lastName = If(person("basic")?("last_name") IsNot Nothing, person("basic")("last_name").ToString(), "N/A")
                    Dim state = person("addresses")?(0)?("state")?.ToString()
                    Dim specialty = ""
                    If person("taxonomies") IsNot Nothing AndAlso person("taxonomies").HasValues Then
                        specialty = person("taxonomies")?(0)?("desc")?.ToString()
                    End If
                    Dim phone = person("addresses")?(0)?("telephone_number")?.ToString()
                    Dim address = ""
                    If person("addresses") IsNot Nothing AndAlso person("addresses").HasValues Then
                        Dim addrObj = person("addresses")?(0)
                        address = addrObj?("address_1")?.ToString()
                        Dim addr2 = addrObj?("address_2")?.ToString()
                        If Not String.IsNullOrWhiteSpace(addr2) Then
                            address &= " " & addr2
                        End If
                        address &= ", " & addrObj?("city")?.ToString() & ", " & addrObj?("state")?.ToString() & " " & addrObj?("postal_code")?.ToString()
                    End If
                    DataGridView1.Rows.Add(npi, firstName, lastName, state, specialty, phone, address)
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
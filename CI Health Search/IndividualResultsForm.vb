Imports Newtonsoft.Json.Linq

Public Class IndividualResultsForm

    Private dgvFilterHelper As DataGridViewFilterHelper
    Public Property SelectedNpi As String = Nothing

    Public Sub New(results As JArray, Optional showDrugColumns As Boolean = False)
        InitializeComponent()
        DataGridView1.Columns.Clear()

        Dim dt As New DataTable()

        If results.Count > 0 Then
            Dim firstObj As JObject = CType(results(0), JObject)

            ' Drug search: Brnd_Name or Gnrc_Name present, or showDrugColumns forced
            If showDrugColumns OrElse firstObj.ContainsKey("Brnd_Name") OrElse firstObj.ContainsKey("Gnrc_Name") Then
                dt.Columns.Add("NPI")
                dt.Columns.Add("First Name")
                dt.Columns.Add("Last Name")
                dt.Columns.Add("Brand Name")
                dt.Columns.Add("Generic Name")
                For Each person As JObject In results
                    Dim npi = person("Prscrbr_NPI")?.ToString()
                    Dim firstName = person("Prscrbr_First_Name")?.ToString()
                    Dim lastName = person("Prscrbr_Last_Name")?.ToString()
                    Dim brnd = person("Brnd_Name")?.ToString()
                    Dim gnrc = person("Gnrc_Name")?.ToString()
                    dt.Rows.Add(npi, firstName, lastName, brnd, gnrc)
                Next

            ElseIf firstObj.ContainsKey("Rndrng_NPI") Then
                ' HCPCS dataset
                dt.Columns.Add("NPI")
                dt.Columns.Add("First Name")
                dt.Columns.Add("Last Name")
                dt.Columns.Add("State")
                For Each person As JObject In results
                    Dim npi = person("Rndrng_NPI")?.ToString()
                    Dim firstName = person("Rndrng_Prvdr_First_Name")?.ToString()
                    Dim lastName = person("Rndrng_Prvdr_Last_Org_Name")?.ToString()
                    Dim state = person("Rndrng_Prvdr_State_Abrvtn")?.ToString()
                    dt.Rows.Add(npi, firstName, lastName, state)
                Next

            Else
                ' NPI Registry dataset
                dt.Columns.Add("NPI")
                dt.Columns.Add("First Name")
                dt.Columns.Add("Last Name")
                dt.Columns.Add("State")
                dt.Columns.Add("Specialty")
                dt.Columns.Add("Phone")
                dt.Columns.Add("Address")
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
                    dt.Rows.Add(npi, firstName, lastName, state, specialty, phone, address)
                Next
            End If
        End If

        DataGridView1.DataSource = dt
        dgvFilterHelper = New DataGridViewFilterHelper(DataGridView1, Me)
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then
            SelectedNpi = DataGridView1.Rows(e.RowIndex).Cells("NPI").Value.ToString()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class
Imports Newtonsoft.Json.Linq

Public Class IndividualResultsForm

    Private dgvFilterHelper As DataGridViewFilterHelper
    Public Property SelectedNpi As String = Nothing

    Public Sub New(results As JArray, Optional showDrugColumns As Boolean = False)
        InitializeComponent()
        DataGridView1.Columns.Clear()

        Dim dt As New DataTable()
        dt.Columns.Add("NPI")
        dt.Columns.Add("First Name")
        dt.Columns.Add("Last Name")
        dt.Columns.Add("Phone")
        dt.Columns.Add("Address")
        dt.Columns.Add("Specialty")

        ' Add extra columns for drug/HCPCS if needed
        Dim isDrug = showDrugColumns OrElse (results.Count > 0 AndAlso CType(results(0), JObject).ContainsKey("Brnd_Name"))
        Dim isHCPCS = (results.Count > 0 AndAlso CType(results(0), JObject).ContainsKey("Rndrng_NPI"))

        If isDrug Then
            dt.Columns.Add("Brand Name")
            dt.Columns.Add("Generic Name")
        ElseIf isHCPCS Then
            dt.Columns.Add("State")
        Else
            dt.Columns.Add("State")
        End If

        For Each person As JObject In results
            Dim npi As String = ""
            Dim firstName As String = ""
            Dim lastName As String = ""
            Dim phone As String = ""
            Dim address As String = ""
            Dim specialty As String = ""
            Dim state As String = ""
            Dim brandName As String = ""
            Dim genericName As String = ""

            If isDrug Then
                npi = person("Prscrbr_NPI")?.ToString()
                firstName = person("Prscrbr_First_Name")?.ToString()
                lastName = person("Prscrbr_Last_Name")?.ToString()
                brandName = person("Brnd_Name")?.ToString()
                genericName = person("Gnrc_Name")?.ToString()
                ' Try to get phone/address/specialty if present (rare in this dataset)
                phone = person("Prscrbr_Phone")?.ToString()
                address = person("Prscrbr_Addr1")?.ToString()
                specialty = person("Prscrbr_Type")?.ToString()
                dt.Rows.Add(npi, firstName, lastName, phone, address, specialty, brandName, genericName)
            ElseIf isHCPCS Then
                npi = person("Rndrng_NPI")?.ToString()
                firstName = person("Rndrng_Prvdr_First_Name")?.ToString()
                lastName = person("Rndrng_Prvdr_Last_Org_Name")?.ToString()
                state = person("Rndrng_Prvdr_State_Abrvtn")?.ToString()
                ' Try to get phone/address/specialty if present (rare in this dataset)
                phone = person("Rndrng_Prvdr_Phone")?.ToString()
                address = person("Rndrng_Prvdr_Street_Addr")?.ToString()
                specialty = person("Rndrng_Prvdr_Type")?.ToString()
                dt.Rows.Add(npi, firstName, lastName, phone, address, specialty, state)
            Else
                npi = person("number")?.ToString()
                firstName = If(person("basic")?("first_name") IsNot Nothing, person("basic")("first_name").ToString(), "N/A")
                lastName = If(person("basic")?("last_name") IsNot Nothing, person("basic")("last_name").ToString(), "N/A")
                state = person("addresses")?(0)?("state")?.ToString()
                If person("taxonomies") IsNot Nothing AndAlso person("taxonomies").HasValues Then
                    specialty = person("taxonomies")?(0)?("desc")?.ToString()
                End If
                phone = person("addresses")?(0)?("telephone_number")?.ToString()
                If person("addresses") IsNot Nothing AndAlso person("addresses").HasValues Then
                    Dim addrObj = person("addresses")?(0)
                    address = addrObj?("address_1")?.ToString()
                    Dim addr2 = addrObj?("address_2")?.ToString()
                    If Not String.IsNullOrWhiteSpace(addr2) Then
                        address &= " " & addr2
                    End If
                    address &= ", " & addrObj?("city")?.ToString() & ", " & addrObj?("state")?.ToString() & " " & addrObj?("postal_code")?.ToString()
                End If
                dt.Rows.Add(npi, firstName, lastName, phone, address, specialty, state)
            End If
        Next

        DataGridView1.DataSource = dt
        dgvFilterHelper = New DataGridViewFilterHelper(DataGridView1, Me)
        DataGridView1.Top = DataGridView1.Top + dgvFilterHelper.FilterPanel.Height
        DataGridView1.Height = DataGridView1.Height - dgvFilterHelper.FilterPanel.Height
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then
            SelectedNpi = DataGridView1.Rows(e.RowIndex).Cells("NPI").Value.ToString()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub
End Class
Imports Newtonsoft.Json.Linq

Public Class IndividualResultsForm

    Private dgvFilterHelper As DataGridViewFilterHelper
    Public Property SelectedNpi As String = Nothing

    Private ReadOnly ResultsFieldMap As New Dictionary(Of String, String) From {
        {"NPI", "number|npi"},
        {"First Name", "basic.first_name|provider_first_name"},
        {"Last Name", "basic.last_name|provider_last_name"},
        {"Gender", "basic.sex|gndr"},
        {"Medical School", "med_sch"},
        {"Graduation Year", "grd_yr"},
        {"Specialty", "taxonomies[0].desc|pri_spec"},
        {"City", "addresses[0].city|citytown"},
        {"State", "addresses[0].state|state"}
    }
    ' Add more as needed

    Public Sub New(results As JArray, Optional showDrugColumns As Boolean = False, Optional searchSummary As String = "")
        InitializeComponent()
        If Not String.IsNullOrWhiteSpace(searchSummary) Then
            lblSearchSummary.Text = searchSummary
            lblSearchSummary.Visible = True
        Else
            lblSearchSummary.Visible = False
        End If
        DataGridView1.Columns.Clear()

        Dim dt As New DataTable()
        ' After setting DataSource:
        DataGridView1.DataSource = dt

        ' Add these lines for alternating row colors:
        DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow
        DataGridView1.DefaultCellStyle.BackColor = Color.White
        Dim isDrug = showDrugColumns OrElse (results.Count > 0 AndAlso CType(results(0), JObject).ContainsKey("Brnd_Name"))
        Dim isHCPCS = (results.Count > 0 AndAlso CType(results(0), JObject).ContainsKey("Rndrng_NPI"))

        Dim columns As New List(Of (Display As String, Field As String))

        If isDrug Then
            columns.Add(("NPI", "Prscrbr_NPI"))
            columns.Add(("First Name", "Prscrbr_First_Name"))
            columns.Add(("Last Name", "Prscrbr_Last_Org_Name"))

            columns.Add(("City", "Prscrbr_City"))
            columns.Add(("State", "Prscrbr_State_Abrvtn"))
            columns.Add(("Specialty", "Prscrbr_Type"))

            columns.Add(("Brand Name", "Brnd_Name"))
            columns.Add(("Generic Name", "Gnrc_Name"))

            columns.Add(("Total Claims", "Tot_Clms"))
            columns.Add(("Total 30 Day Fills", "Tot_30day_Fills"))
            columns.Add(("Total Day Supply", "Tot_Day_Suply"))
            columns.Add(("Total Drug Cost", "Tot_Drug_Cst"))
            columns.Add(("Total 'Benes'", "Tot_Benes"))

            columns.Add(("Total Claims <65", "GE65_Tot_Clms"))
            columns.Add(("Total 30 Day Fills <65", "GE65_Tot_30day_Fills"))
            columns.Add(("Total Day Supply <65", "GE65_Tot_Day_Suply"))
            columns.Add(("Total Drug Cost <65", "GE65_Tot_Drug_Cst"))
            columns.Add(("Total 'Benes' <65", "GE65_Tot_Benes"))
        ElseIf isHCPCS Then 'Healthcare Common Procedure Coding System
            columns.Add(("NPI", "Rndrng_NPI"))
            columns.Add(("First Name", "Rndrng_Prvdr_First_Name"))
            columns.Add(("Last Name", "Rndrng_Prvdr_Last_Org_Name"))
            columns.Add(("Gender", "Rndrng_Prvdr_Gndr"))
            columns.Add(("Street Address", "Rndrng_Prvdr_Street_Addr"))
            columns.Add(("City", "Rndrng_Prvdr_City"))
            columns.Add(("State", "Rndrng_Prvdr_State_Abrvtn"))
            columns.Add(("Zip", "Rndrng_Prvdr_Zip5"))
            columns.Add(("HCPCS Code", "HCPCS_Cd"))
        Else
            columns.Add(("NPI", "number|npi"))
            columns.Add(("First Name", "basic.first_name|provider_first_name"))
            columns.Add(("Last Name", "basic.last_name|provider_last_name"))
            columns.Add(("Gender", "basic.sex|gndr"))
            columns.Add(("Medical School", "med_sch"))
            columns.Add(("Graduation Year", "grd_yr"))
            columns.Add(("Specialty", "taxonomies[0].desc|pri_spec"))
            columns.Add(("Phone", "addresses[0].telephone_number|telephone_number"))
            columns.Add(("Street Address", "addresses[0].address_1|adr_ln_1"))
            columns.Add(("City", "addresses[0].city|citytown"))
            columns.Add(("State", "addresses[0].state|state"))
            columns.Add(("Zip", "addresses[0].postal_code|zip_code"))
        End If

        For Each col In columns
            dt.Columns.Add(col.Display)
        Next

        If results.Count > 0 Then
            ShowScrollableJson(results(0).ToString(), "First Result JSON")
        End If

        For Each result As JObject In results
            Dim row As New List(Of String)
            For Each col In columns
                Dim val As String
                If col.Field.Contains("|") Then
                    val = GetJsonValue(result, col.Field)
                Else
                    val = result(col.Field)?.ToString()
                End If
                If String.IsNullOrWhiteSpace(val) Then val = "N/A"
                row.Add(val)
            Next
            dt.Rows.Add(row.ToArray())
        Next

        DataGridView1.DataSource = dt
        dgvFilterHelper = New DataGridViewFilterHelper(DataGridView1, Me)
        DataGridView1.Top = DataGridView1.Top + dgvFilterHelper.FilterPanel.Height
        DataGridView1.Height = DataGridView1.Height - dgvFilterHelper.FilterPanel.Height
        txtNumResults.Text = dt.Rows.Count
    End Sub

    Private Function GetJsonValue(obj As JObject, path As String) As String
        For Each tryPath In path.Split("|"c)
            Try
                Dim parts = tryPath.Split("."c)
                Dim current As JToken = obj
                For Each part In parts
                    If part.Contains("[") Then
                        Dim arrName = part.Substring(0, part.IndexOf("["))
                        Dim idx = Integer.Parse(part.Substring(part.IndexOf("[") + 1, part.IndexOf("]") - part.IndexOf("[") - 1))
                        current = current(arrName)
                        If current Is Nothing OrElse Not current.HasValues Then GoTo NextPath
                        current = current(idx)
                    Else
                        current = current(part)
                    End If
                    If current Is Nothing Then GoTo NextPath
                Next
                Return current.ToString()
            Catch
                ' Try next path
            End Try
NextPath:
        Next
        Return ""
    End Function

    Private Sub ShowScrollableJson(text As String, Optional title As String = "JSON Debug")
        Dim frm As New Form With {
            .Text = title,
            .Width = 800,
            .Height = 600
        }
        Dim txt As New TextBox With {
            .Multiline = True,
            .ScrollBars = ScrollBars.Both,
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .Font = New Font("Consolas", 10),
            .Text = text
        }
        frm.Controls.Add(txt)
        frm.Show()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then
            SelectedNpi = DataGridView1.Rows(e.RowIndex).Cells("NPI").Value.ToString()
            Me.DialogResult = DialogResult.OK
            'Me.Close()
            Dim profileForm As New IndividualProfileForm(SelectedNpi)
            profileForm.Show()
            profileForm.tbNpiResult.Text = SelectedNpi

        End If
    End Sub


End Class
Imports Newtonsoft.Json.Linq

Public Class IndividualProfileForm
    Private _npi As String

    ' Map control names to NPI Registry API JSON paths
    Private ReadOnly NpiFieldMap As New Dictionary(Of String, String) From {
        {"tbNpiResult", "number"},
        {"lblFirst", "basic.first_name"},
        {"lblMiddle", "basic.middle_name"},
        {"lblLast", "basic.last_name"},
        {"lblGender", "basic.sex"},
        {"lblStreetAddress", "addresses[0].address_1"},
        {"lblCity", "addresses[0].city"},
        {"lblState", "addresses[0].state"},
        {"lblZip", "addresses[0].postal_code"},
        {"lblAddressType", "addresses[0].address_purpose"}}
    ' Add more mappings here as you add more controls/fields


    Public Sub New(npi As String)
        InitializeComponent()
        _npi = npi
    End Sub

    Private Async Sub IndividualProfileForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim results = Await IndividualApiHelper.SearchNpiRegistryAsync(npi:=_npi)
        If results IsNot Nothing AndAlso results.Count > 0 Then
            Dim person = results(0)
            For Each kvp In NpiFieldMap
                Dim ctrl = Me.Controls.Find(kvp.Key, True).FirstOrDefault()
                If ctrl IsNot Nothing Then
                    Dim value = GetJsonValue(person, kvp.Value)
                    If TypeOf ctrl Is Label Then
                        CType(ctrl, Label).Text = value
                    ElseIf TypeOf ctrl Is TextBox Then
                        CType(ctrl, TextBox).Text = value
                    End If
                End If
            Next
        Else
            MessageBox.Show("No data found for this NPI.")
        End If
    End Sub

    ' Helper to get a value from a JObject using a dot/bracket path
    Private Function GetJsonValue(obj As JObject, path As String) As String
        Try
            Dim parts = path.Split("."c)
            Dim current As JToken = obj
            For Each part In parts
                If part.Contains("[") Then
                    ' Handle array index, e.g., addresses[0]
                    Dim arrName = part.Substring(0, part.IndexOf("["))
                    Dim idx = Integer.Parse(part.Substring(part.IndexOf("[") + 1, part.IndexOf("]") - part.IndexOf("[") - 1))
                    current = current(arrName)
                    If current Is Nothing OrElse Not current.HasValues Then Return ""
                    current = current(idx)
                Else
                    current = current(part)
                End If
                If current Is Nothing Then Return ""
            Next
            Return current.ToString()
        Catch
            Return ""
        End Try
    End Function

    ' Optional: Remove if not needed
    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter
    End Sub
End Class
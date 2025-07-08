Imports System.Data.SqlClient
Imports System.Net.Http
Imports Google.Apis.Requests
Imports Newtonsoft.Json.Linq

Public Class Search
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    Public Shared SearchHospital As New HospitalContext()

    Public Function useSQL(ByRef selectedState As String) As Boolean
        ' Determine if SQL should be used based on the selected state
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand("Select UseSql from dbo.States Where StateCode = '" + selectedState + "'", conn)
                'Dim da As New SqlDataAdapter(cmd)
                conn.Open()

                Return cmd.ExecuteScalar()
            End Using
        End Using
    End Function

    Private Sub SetupHospProfile()
        SearchHospital.CMSNum = VerifiySearch(txtCmsCertNumDemoAll)
        SearchHospital.State = If(lbStateAll.SelectedItem IsNot Nothing, lbStateAll.SelectedItem.ToString().Trim(), "")
        SearchHospital.Name = VerifiySearch(txtHospitalNameAll)
        SearchHospital.City = VerifiySearch(txtCityAll)
        SearchHospital.Zip = VerifiySearch(txtZipCodeDemoAll)
        SearchHospital.Phone = VerifiySearch(txtAreaCodeAll)
        SearchHospital.NPI = VerifiySearch(txtNpiAll)
    End Sub

    Public Function VerifiySearch(txtSearched As TextBox) As String
        If (txtSearched.Text IsNot Nothing) Or txtSearched.Text.Length > 0 Then
            Return txtSearched.Text.Trim()
        Else
            Return Nothing
        End If
    End Function

    Public Function CleanMeUp(strMydata As String, Optional ByRef isNumber As Boolean = False) As String
        If strMydata Is Nothing Then
            Select Case isNumber
                Case True
                    Return 0
                Case False
                    Return "N/A"
            End Select
            Return ""
        End If
        If (strMydata Is Nothing) Or String.IsNullOrEmpty(strMydata) Or strMydata.Length = 0 Or strMydata = "Result" Then
            If isNumber = True Then
                Return 0
            Else
                Return "N/A"
            End If
        ElseIf isNumber = True Then
            Return CDec(strMydata).ToString("N")
        Else
            Return strMydata.ToString
        End If
        'Return String.IsNullOrEmpty(strMydata) Or strMydata.Length = 0 OrElse strMydata.Trim() = ""
    End Function

    Private Async Sub btnSearchAll_Click(sender As Object, e As EventArgs) Handles btnSearchAll.Click
        ' Declare filters and parameters at the top so they are always in scope
        Dim filters As New List(Of String)
        Dim parameters As New List(Of SqlParameter)

        ' Get selected state from ListBox
        Dim selectedState As String = ""
        If lbStateAll.SelectedItem IsNot Nothing Then
            selectedState = lbStateAll.SelectedItem.ToString().Trim()
        End If

        'Dim useSql As Boolean = (selectedState = "TN" Or selectedState = "TX") ' Adjust for your states with SQL data

        If useSQL(selectedState) Then
            ' --- SQL Search ---
            If Not String.IsNullOrEmpty(selectedState) Then
                filters.Add("State = @State")
                parameters.Add(New SqlParameter("@State", selectedState))
            End If
            If Not String.IsNullOrEmpty(txtCityAll.Text) Then
                filters.Add("City = @City")
                parameters.Add(New SqlParameter("@City", txtCityAll.Text.Trim()))
            End If
            'If Not String.IsNullOrEmpty(txtCountyAll.Text) Then
            '    filters.Add("County = @County")
            '    parameters.Add(New SqlParameter("@County", txtCountyAll.Text.Trim()))
            'End If
            'If Not String.IsNullOrEmpty(txtBeds.Text) Then
            '    filters.Add("Beds = @Beds")
            '    parameters.Add(New SqlParameter("@Beds", txtBeds.Text.Trim()))
            'End If
            'If Not String.IsNullOrEmpty(txtCmsCertNumDemoAll.Text) Then
            'fil'ters.Add("CMSNum = @CMSNum")
            'parameters.Add(New SqlParameter("@CMSNum", txtCmsCertNumDemoAll.Text.Trim()))
            'End If
            'If Not String.IsNullOrEmpty(txtNpiAll.Text) Then
            'fil'ters.Add("NPI = @NPI")
            'parameters.Add(New SqlParameter("@NPI", txtNpiAll.Text.Trim()))
            'End If
            ' Add more filters as needed

            Dim query As String
            If selectedState = "TN" Then
                query = "SELECT LicenseNum, [Facility Name], State, City, County FROM tn.AdminCon WHERE 1=1"
            ElseIf selectedState = "TX" Then
                query = "SELECT id AS LicenseNum, [Facility Name], State, City, County FROM tx.Utilization WHERE 1=1"
            Else
                query = "" ' Should not happen
            End If

            If filters.Count > 0 Then
                query &= " AND " & String.Join(" AND ", filters)
            End If

            Dim dt As New DataTable()
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddRange(parameters.ToArray())
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using

            Results.SetResults(dt)
            Results.SelectedState = selectedState
            Hide()
            Results.Show()
            Await SearchByApiAsync(selectedState)
        Else
            ' --- API Search ---
            Await SearchByApiAsync(selectedState)
        End If
    End Sub

    Private Async Function SearchByApiAsync(selectedState As String) As Task
        ' Use the correct dataset and keyword parameter
        Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?"
        Dim filters As New List(Of String)

        ' Add keyword parameters for broad API search
        If Not String.IsNullOrEmpty(selectedState) Then filters.Add("keyword=" & Uri.EscapeDataString(selectedState))
        If Not String.IsNullOrEmpty(txtCityAll.Text) Then filters.Add("keyword=" & Uri.EscapeDataString(txtCityAll.Text.Trim()))
        If Not String.IsNullOrEmpty(txtCmsCertNumDemoAll.Text) Then filters.Add("keyword=" & Uri.EscapeDataString(txtCmsCertNumDemoAll.Text.Trim()))
        If Not String.IsNullOrEmpty(txtNpiAll.Text) Then filters.Add("keyword=" & Uri.EscapeDataString(txtNpiAll.Text.Trim()))
        If Not String.IsNullOrEmpty(txtHospitalNameAll.Text) Then filters.Add("keyword=" & Uri.EscapeDataString(txtHospitalNameAll.Text.Trim()))
        ' Add more keyword filters as needed

        filters.Add("size=1000") ' Increase size as needed

        apiUrl &= String.Join("&", filters)

        'MessageBox.Show(apiUrl) ' For debugging

        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)
                If data.Count > 0 Then
                    Dim dt As New DataTable()
                    For Each col In data(0).ToObject(Of JObject)().Properties()
                        dt.Columns.Add(col.Name)
                    Next
                    For Each item In data
                        Dim row = dt.NewRow()
                        For Each col In dt.Columns
                            row(col.ToString()) = item(col.ToString())
                        Next
                        dt.Rows.Add(row)
                    Next      ' --- Client-side filtering for exact matches ---
                    If Not String.IsNullOrEmpty(selectedState) AndAlso dt.Columns.Contains("State Code") Then
                        dt = dt.Select($"[State Code] = '{selectedState}'").CopyToDataTable()
                    End If
                    If Not String.IsNullOrEmpty(txtCityAll.Text) AndAlso dt.Columns.Contains("City") Then
                        dt = dt.Select($"[City] = '{txtCityAll.Text.Trim()}'").CopyToDataTable()
                    End If
                    If Not String.IsNullOrEmpty(txtCmsCertNumDemoAll.Text) AndAlso dt.Columns.Contains("Provider CCN") Then
                        dt = dt.Select($"[Provider CCN] = '{txtCmsCertNumDemoAll.Text.Trim()}'").CopyToDataTable()
                    End If
                    If Not String.IsNullOrEmpty(txtHospitalNameAll.Text) AndAlso dt.Columns.Contains("Hospital Name") Then
                        dt = dt.Select($"[Hospital Name] = '{txtHospitalNameAll.Text.Trim()}'").CopyToDataTable()
                    End If
                    If Not String.IsNullOrEmpty(txtNpiAll.Text) AndAlso dt.Columns.Contains("NPI") Then
                        dt = dt.Select($"[NPI] = '{txtNpiAll.Text.Trim()}'").CopyToDataTable()
                    End If

                    Results.SetResults(dt)
                    Results.SelectedState = selectedState
                    Hide()
                    Results.Show()
                Else
                    MessageBox.Show("No results found for your search.")
                End If
            Else
                MessageBox.Show("API error: " & response.StatusCode.ToString())
            End If
        End Using
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        lbStateAll.ClearSelected()
        txtCityAll.Clear()


        txtCmsCertNumDemoAll.Clear()
        txtNpiAll.Clear()
        txtHospitalNameAll.Clear()
        txtAreaCodeAll.Clear()

        txtZipCodeDemoAll.Clear()
        txtMaxTotalBedsAll.Clear()
        txtMinTotalBedsAll.Clear()
        txtNpiAll.Clear()


    End Sub

    Public Function GetAPIArray(strAPIurl As String) As JArray
        Dim client As New HttpClient()
        Dim response As HttpResponseMessage = client.GetAsync(strAPIurl).Result
        If response.IsSuccessStatusCode Then
            Dim jsonString As String = response.Content.ReadAsStringAsync().Result
            Return JArray.Parse(jsonString)
        Else
            Throw New Exception("API call failed with status: " & response.StatusCode.ToString())
        End If
    End Function

    Private Sub Search_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class


Imports System.Net.Http
Imports Newtonsoft.Json.Linq
Imports System.Data

Public Module ApiHelper
    ' Centralized API URLs
    Public ReadOnly ApiUrls As New Dictionary(Of String, String) From {
        {"PatientOrigin", "https://data.cms.gov/data-api/v1/dataset/8708ca8b-8636-44ed-8303-724cbfaf78ad/data"},
        {"PatientOrigin2019", "https://data.cms.gov/data-api/v1/dataset/2713ba99-c59e-4b25-9a3d-3661d35988da/data"},
        {"PatientOrigin2023", "https://data.cms.gov/data-api/v1/dataset/7f749f00-bfa9-4377-9a98-90c15cacc2f3/data"},
        {"CeoApi", "https://data.cms.gov/data-api/v1/dataset/029c119f-f79c-49be-9100-344d31d10344/data"},
        {"NewApi", "https://data.cms.gov/data-api/v1/dataset/690ddc6c-2767-4618-b277-420ffb2bf27c/data"},
        {"ApcApi", "https://data.cms.gov/data-api/v1/dataset/ccbc9a44-40d4-46b4-a709-5caa59212e50/data"},
        {"DepartmentsApi", "https://data.cms.gov/data-api/v1/dataset/8ba0f9b4-9493-4aa0-9f82-44ea9468d1b5/data"},
        {"MainProfileApi", "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data"},
        {"ProfileMainUrl", "https://data.cms.gov/data-api/v1/dataset/8143cbc7-484f-438b-9dfa-2e81d5d6a1ed/data"}}
    ' Add more as needed


    ' Generic async API call that returns a JArray
    Public Async Function GetApiDataAsync(url As String) As Task(Of JArray)
        Try
            Using client As New HttpClient()
                Dim response = Await client.GetAsync(url)
                If response.IsSuccessStatusCode Then
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Return JArray.Parse(json)
                End If
            End Using
        Catch
            ' Optionally log error
        End Try
        Return New JArray()
    End Function

    ' Generic async API call that returns a DataTable
    Public Async Function GetTableFromApiAsync(url As String) As Task(Of DataTable)
        Dim dt As New DataTable()
        Dim data As JArray = Await GetApiDataAsync(url)
        If data.Count > 0 Then
            For Each prop In CType(data(0), JObject).Properties()
                dt.Columns.Add(prop.Name)
            Next
            For Each item As JObject In data
                Dim row As DataRow = dt.NewRow()
                For Each prop In item.Properties()
                    row(prop.Name) = prop.Value.ToString()
                Next
                dt.Rows.Add(row)
            Next
        End If
        Return dt
    End Function
End Module
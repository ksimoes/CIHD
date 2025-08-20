Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Module IndividualApiHelper
    ' NPI Registry API (search and details)
    Public Async Function SearchNpiRegistryAsync(
    Optional npi As String = "",
    Optional firstName As String = "",
    Optional middleName As String = "",
    Optional lastName As String = "",
    Optional city As String = "",
    Optional state As String = "",
    Optional zip As String = "",
    Optional gender As String = "",
    Optional licenseState As String = "",
    Optional licenseNumber As String = "",
    Optional enumerationType As String = "",
    Optional taxonomyDescription As String = "",
    Optional graduationYear As String = "",
    Optional medicalSchool As String = "",
    Optional limit As Integer = 10,
    Optional skip As Integer = 0
) As Task(Of JArray)
        Dim baseUrl As String = "https://npiregistry.cms.hhs.gov/api/?version=2.1"
        Dim query As New List(Of String)

        If Not String.IsNullOrWhiteSpace(npi) Then query.Add("number=" & Uri.EscapeDataString(npi))
        If Not String.IsNullOrWhiteSpace(firstName) Then query.Add("first_name=" & Uri.EscapeDataString(firstName))
        If Not String.IsNullOrWhiteSpace(middleName) Then query.Add("middle_name=" & Uri.EscapeDataString(middleName))
        If Not String.IsNullOrWhiteSpace(lastName) Then query.Add("last_name=" & Uri.EscapeDataString(lastName))
        If Not String.IsNullOrWhiteSpace(city) Then query.Add("city=" & Uri.EscapeDataString(city))
        If Not String.IsNullOrWhiteSpace(state) Then query.Add("state=" & Uri.EscapeDataString(state))
        If Not String.IsNullOrWhiteSpace(zip) Then query.Add("postal_code=" & Uri.EscapeDataString(zip))
        If Not String.IsNullOrWhiteSpace(gender) Then query.Add("gender=" & Uri.EscapeDataString(gender))
        If Not String.IsNullOrWhiteSpace(licenseState) Then query.Add("license_state=" & Uri.EscapeDataString(licenseState))
        If Not String.IsNullOrWhiteSpace(licenseNumber) Then query.Add("license_number=" & Uri.EscapeDataString(licenseNumber))
        If Not String.IsNullOrWhiteSpace(enumerationType) Then query.Add("enumeration_type=" & Uri.EscapeDataString(enumerationType))
        If Not String.IsNullOrWhiteSpace(taxonomyDescription) Then query.Add("taxonomy_description=" & Uri.EscapeDataString(taxonomyDescription))
        If Not String.IsNullOrWhiteSpace(graduationYear) Then query.Add("graduation_year=" & Uri.EscapeDataString(graduationYear))
        If Not String.IsNullOrWhiteSpace(medicalSchool) Then query.Add("medical_school=" & Uri.EscapeDataString(medicalSchool))
        query.Add("limit=" & limit.ToString())
        query.Add("skip=" & skip.ToString())

        Dim url As String = baseUrl & "&" & String.Join("&", query)
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(url)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                Return TryCast(obj("results"), JArray)
            End If
        End Using
        Return Nothing
    End Function

    Public Async Function SearchNationalDownloadableFileAsync(
    Optional npi As String = "",
    Optional firstName As String = "",
    Optional lastName As String = "",
    Optional gradYear As String = "",
    Optional medSchool As String = "",
    Optional limit As Integer = 10
) As Task(Of JArray)
        Dim url As String = "https://data.cms.gov/provider-data/api/1/datastore/query/mj5m-pzi6/0"
        Dim conditions As New List(Of JObject)
        If Not String.IsNullOrWhiteSpace(npi) Then
            conditions.Add(New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "npi"),
            New JProperty("value", npi),
            New JProperty("operator", "=")
        ))
        End If
        If Not String.IsNullOrWhiteSpace(firstName) Then
            conditions.Add(New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "provider_first_name"),
            New JProperty("value", firstName),
            New JProperty("operator", "=")
        ))
        End If
        If Not String.IsNullOrWhiteSpace(lastName) Then
            conditions.Add(New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "provider_last_name"),
            New JProperty("value", lastName),
            New JProperty("operator", "=")
        ))
        End If
        If Not String.IsNullOrWhiteSpace(gradYear) Then
            conditions.Add(New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "grd_yr"),
            New JProperty("value", gradYear),
            New JProperty("operator", "=")
        ))
        End If
        If Not String.IsNullOrWhiteSpace(medSchool) Then
            conditions.Add(New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", "med_sch"),
            New JProperty("value", medSchool),
            New JProperty("operator", "=")
        ))
        End If

        Dim postBody As New JObject(
        New JProperty("conditions", New JArray(conditions)),
        New JProperty("limit", limit)
    )

        Using client As New HttpClient()
            Dim content = New StringContent(postBody.ToString(), Text.Encoding.UTF8, "application/json")
            Dim response = Await client.PostAsync(url, content)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                Return TryCast(obj("results"), JArray)
            End If
        End Using
        Return Nothing
    End Function

    ' National Downloadable File
    Public Async Function GetNationalDownloadableFileAsync(Optional limit As Integer = 3) As Task(Of JArray)
        Dim url As String = "https://data.cms.gov/provider-data/api/1/datastore/query/mj5m-pzi6/0"
        Dim body = $"{{""conditions"":[{{""resource"":""t"",""property"":""record_number"",""value"":1,""operator"":"">""}}],""limit"":{limit}}}"
        Using client As New HttpClient()
            Dim content = New StringContent(body, Text.Encoding.UTF8, "application/json")
            Dim response = Await client.PostAsync(url, content)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                Return TryCast(obj("results"), JArray)
            End If
        End Using
        Return Nothing
    End Function

    ' Facility Affiliations Data
    Public Async Function GetFacilityAffiliationsAsync(Optional limit As Integer = 3) As Task(Of JArray)
        Dim url As String = "https://data.cms.gov/provider-data/api/1/datastore/query/27ea-46a8/0"
        Dim body = $"{{""conditions"":[{{""resource"":""t"",""property"":""record_number"",""value"":1,""operator"":"">""}}],""limit"":{limit}}}"
        Using client As New HttpClient()
            Dim content = New StringContent(body, Text.Encoding.UTF8, "application/json")
            Dim response = Await client.PostAsync(url, content)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                Return TryCast(obj("data"), JArray)
            End If
        End Using
        Return Nothing
    End Function

    ' Utilization Data
    Public Async Function GetUtilizationDataAsync(Optional limit As Integer = 3) As Task(Of JArray)
        Dim url As String = "https://data.cms.gov/provider-data/api/1/datastore/query/n0yb-util/0"
        Dim body = $"{{""conditions"":[{{""resource"":""t"",""property"":""record_number"",""value"":1,""operator"":"">""}}],""limit"":{limit}}}"
        Using client As New HttpClient()
            Dim content = New StringContent(body, Text.Encoding.UTF8, "application/json")
            Dim response = Await client.PostAsync(url, content)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                Return TryCast(obj("data"), JArray)
            End If
        End Using
        Return Nothing
    End Function

    ' Order and Referring
    Public Async Function GetOrderAndReferringAsync(Optional offset As Integer = 0, Optional size As Integer = 10) As Task(Of JArray)
        Dim url As String = $"https://data.cms.gov/data-api/v1/dataset/c99b5865-1119-4436-bb80-c5af2773ea1f/data?offset={offset}&size={size}"
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(url)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Return JArray.Parse(json)
            End If
        End Using
        Return Nothing
    End Function

    ' Revalidation Due Date List
    Public Async Function GetRevalidationDueDatesAsync(Optional offset As Integer = 0, Optional size As Integer = 10) As Task(Of JArray)
        Dim url As String = $"https://data.cms.gov/data-api/v1/dataset/3746498e-874d-45d8-9c69-68603cafea60/data?offset={offset}&size={size}"
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(url)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Return JArray.Parse(json)
            End If
        End Using
        Return Nothing
    End Function

    ' Revalidation Reassignments
    Public Async Function GetRevalidationReassignmentsAsync(Optional offset As Integer = 0, Optional size As Integer = 10) As Task(Of JArray)
        Dim url As String = $"https://data.cms.gov/data-api/v1/dataset/20f51cff-4137-4f3a-b6b7-bfc9ad57983b/data?offset={offset}&size={size}"
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(url)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Return JArray.Parse(json)
            End If
        End Using
        Return Nothing
    End Function

    ' Medicare Fee-for-service public enrollment
    Public Async Function GetMedicareEnrollmentAsync(Optional offset As Integer = 0, Optional size As Integer = 10) As Task(Of JArray)
        Dim url As String = $"https://data.cms.gov/data-api/v1/dataset/2457ea29-fc82-48b0-86ec-3b0755de7515/data?offset={offset}&size={size}"
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(url)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Return JArray.Parse(json)
            End If
        End Using
        Return Nothing
    End Function

    ' Fetch National Downloadable File by NPI
    Public Async Function GetNationalDownloadableFileByNpiAsync(npi As String) As Task(Of JArray)
        Dim url As String = "https://data.cms.gov/provider-data/api/1/datastore/query/mj5m-pzi6/0"
        Dim body = $"{{""conditions"":[{{""resource"":""t"",""property"":""npi"",""value"":""{npi}"",""operator"":""=""}}],""limit"":1}}"
        Using client As New HttpClient()
            Dim content = New StringContent(body, Text.Encoding.UTF8, "application/json")
            Dim response = Await client.PostAsync(url, content)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                Return TryCast(obj("results"), JArray)
            End If
        End Using
        Return Nothing
    End Function

    Public Async Function SearchByHCPCSAsync(hcpcsCode As String) As Task(Of JArray)
        Dim url As String = $"https://data.cms.gov/data-api/v1/dataset/92396110-2aed-4d63-a6a2-5d6207d46a29/data?filter[HCPCS_Cd]={Uri.EscapeDataString(hcpcsCode)}&size=100"
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(url)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Return JArray.Parse(json)
            End If
        End Using
        Return Nothing
    End Function

    ' Fetch Facility Affiliations by NPI
    Public Async Function GetFacilityAffiliationsByNpiAsync(npi As String) As Task(Of JArray)
        Dim url As String = "https://data.cms.gov/provider-data/api/1/datastore/query/27ea-46a8/0"
        Dim body = $"{{""conditions"":[{{""resource"":""t"",""property"":""NPI"",""value"":""{npi}"",""operator"":""=""}}],""limit"":100}}"
        Using client As New HttpClient()
            Dim content = New StringContent(body, Text.Encoding.UTF8, "application/json")
            Dim response = Await client.PostAsync(url, content)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                Return TryCast(obj("data"), JArray)
            End If
        End Using
        Return Nothing
    End Function

    Public Async Function SearchByDrugAsync(Optional brandName As String = "", Optional genericName As String = "") As Task(Of JArray)
        Dim baseUrl As String = "https://data.cms.gov/data-api/v1/dataset/9552739e-3d05-4c1b-8eff-ecabf391e2e5/data?"
        Dim filters As New List(Of String)
        If Not String.IsNullOrWhiteSpace(brandName) Then
            filters.Add($"filter[Brnd_Name]={Uri.EscapeDataString(brandName)}")
        End If
        If Not String.IsNullOrWhiteSpace(genericName) Then
            filters.Add($"filter[Gnrc_Name]={Uri.EscapeDataString(genericName)}")
        End If
        filters.Add("size=100")
        Dim url As String = baseUrl & String.Join("&", filters)
        Using client As New HttpClient()
            Dim response = Await client.GetAsync(url)
            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Return JArray.Parse(json)
            End If
        End Using
        Return Nothing
    End Function
End Module
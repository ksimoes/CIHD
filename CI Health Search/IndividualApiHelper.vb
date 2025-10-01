Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json.Linq

''' <summary>
''' Provides access to CMS Provider APIs for individual healthcare provider searches.
''' This module simplifies interaction with multiple CMS data sources including NPI Registry,
''' National Downloadable Files, and various provider datasets.
''' 
''' Usage Examples:
''' - Search by NPI: SearchNpiRegistryAsync(npi:="1234567890")
''' - Search by name: SearchNpiRegistryAsync(firstName:="John", lastName:="Smith", state:="CA")
''' - Search by HCPCS: SearchByHCPCSAsync("99213", "CA")
''' - Search by drug: SearchByDrugAsync(brandName:="Lipitor")
''' </summary>
Module IndividualApiHelper

#Region "Configuration and Constants"

    ' Shared HttpClient for connection reuse and better performance
    Private ReadOnly _httpClient As New HttpClient() With {
        .Timeout = TimeSpan.FromSeconds(30)
    }

    ' API Base URLs - centralized for easy maintenance
    Private Const NPI_REGISTRY_BASE_URL As String = "https://npiregistry.cms.hhs.gov/api/?version=2.1"
    Private Const CMS_PROVIDER_DATA_BASE_URL As String = "https://data.cms.gov/provider-data/api/1/datastore/query"
    Private Const CMS_DATA_API_BASE_URL As String = "https://data.cms.gov/data-api/v1/dataset"

    ' Dataset IDs for different CMS data sources
    Private Const NATIONAL_DOWNLOADABLE_FILE_ID As String = "mj5m-pzi6"
    Private Const FACILITY_AFFILIATIONS_ID As String = "27ea-46a8"
    Private Const UTILIZATION_DATA_ID As String = "n0yb-util"
    Private Const ORDER_REFERRING_ID As String = "c99b5865-1119-4436-bb80-c5af2773ea1f"
    Private Const REVALIDATION_DUE_DATES_ID As String = "3746498e-874d-45d8-9c69-68603cafea60"
    Private Const REVALIDATION_REASSIGNMENTS_ID As String = "20f51cff-4137-4f3a-b6b7-bfc9ad57983b"
    Private Const MEDICARE_ENROLLMENT_ID As String = "2457ea29-fc82-48b0-86ec-3b0755de7515"
    Private Const HCPCS_DATASET_ID As String = "92396110-2aed-4d63-a6a2-5d6207d46a29"
    Private Const DRUG_DATASET_ID As String = "9552739e-3d05-4c1b-8eff-ecabf391e2e5"

#End Region

#Region "Primary Search Methods"

    ''' <summary>
    ''' Searches the NPI Registry with comprehensive filtering options.
    ''' This is the main method for searching providers by demographics and professional info.
    ''' </summary>
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
        Optional taxonomyDescription As String = "",
        Optional graduationYear As String = "",
        Optional medicalSchool As String = "",
        Optional limit As Integer = 200,
        Optional skip As Integer = 0
    ) As Task(Of JArray)

        Try
            ' Input validation
            If limit <= 0 OrElse limit > 200 Then limit = 50
            If skip < 0 Then skip = 0

            Dim queryParams As New Dictionary(Of String, String) From {
                {"limit", limit.ToString()},
                {"skip", skip.ToString()}
            }

            ' Build query parameters efficiently
            AddParameterIfNotEmpty(queryParams, "number", npi)
            If Not String.IsNullOrWhiteSpace(firstName) Then
                AddParameterIfNotEmpty(queryParams, "first_name", firstName & "*")
            End If
            If Not String.IsNullOrWhiteSpace(middleName) Then
                AddParameterIfNotEmpty(queryParams, "middle_name", middleName & "*")
            End If
            If Not String.IsNullOrWhiteSpace(lastName) Then
                AddParameterIfNotEmpty(queryParams, "last_name", lastName & "*")
            End If
            AddParameterIfNotEmpty(queryParams, "city", city)
            AddParameterIfNotEmpty(queryParams, "state", state)
            AddParameterIfNotEmpty(queryParams, "postal_code", zip)
            AddParameterIfNotEmpty(queryParams, "gender", gender)
            AddParameterIfNotEmpty(queryParams, "license_state", licenseState)
            AddParameterIfNotEmpty(queryParams, "license_number", licenseNumber)
            'AddParameterIfNotEmpty(queryParams, "enumeration_type", If(String.IsNullOrWhiteSpace(enumerationType), "NPI-1", enumerationType))
            AddParameterIfNotEmpty(queryParams, "taxonomy_description", taxonomyDescription)
            AddParameterIfNotEmpty(queryParams, "graduation_year", graduationYear)
            AddParameterIfNotEmpty(queryParams, "medical_school", medicalSchool)

            Dim url As String = BuildUrlWithQuery(NPI_REGISTRY_BASE_URL, queryParams)
            Return Await ExecuteGetRequestAsync(url, "results")

        Catch ex As Exception
            ' Return empty array on error rather than throwing
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Searches the National Downloadable File with filtering.
    ''' Best for searches involving graduation year, medical school, or taxonomy.
    ''' </summary>
    Public Async Function SearchNationalDownloadableFileAsync(
    Optional npi As String = "",
    Optional firstName As String = "",
    Optional middleName As String = "",
    Optional lastName As String = "",
    Optional gender As String = "",
    Optional gradYear As String = "",
    Optional medSchool As String = "",
    Optional state As String = "",
    Optional taxonomy As String = "",
    Optional limit As Integer = 10, Optional isExact As Boolean = False) As Task(Of JArray)

        Try
            Dim conditions As New List(Of JObject)

            ' Add ALL exact-match conditions to the API query
            AddConditionIfNotEmpty(conditions, "npi", npi, True)
            AddConditionIfNotEmpty(conditions, "provider_first_name", firstName, isExact)
            AddConditionIfNotEmpty(conditions, "provider_middle_name", middleName, isExact)
            AddConditionIfNotEmpty(conditions, "provider_last_name", lastName, isExact)
            AddConditionIfNotEmpty(conditions, "gndr", gender, True)
            AddConditionIfNotEmpty(conditions, "grd_yr", gradYear, True)
            AddConditionIfNotEmpty(conditions, "med_sch", medSchool, isExact)
            AddConditionIfNotEmpty(conditions, "state", state, True)

            ' Validate we have at least some search criteria
            If conditions.Count = 0 AndAlso String.IsNullOrEmpty(taxonomy) Then
                Return New JArray() ' No search criteria provided
            End If

            ' Request maximum results
            Dim results = Await ExecuteCmsPostRequestAsync(NATIONAL_DOWNLOADABLE_FILE_ID, conditions, 200, "results")

            If results Is Nothing OrElse results.Count = 0 Then
                Return New JArray()
            End If

            ' Apply client-side filters ONLY if needed for partial matches
            ' (StartsWith logic for when user types "Ju" to find "Juan")
            If Not String.IsNullOrEmpty(firstName) Then
                results = FilterByName(results, "provider_first_name", firstName)
            End If

            If Not String.IsNullOrEmpty(middleName) Then
                results = FilterByName(results, "provider_middle_name", middleName)
            End If

            If Not String.IsNullOrEmpty(lastName) Then
                results = FilterByName(results, "provider_last_name", lastName)
            End If

            ' Apply client-side filter for taxonomy across all specialty fields
            If Not String.IsNullOrEmpty(taxonomy) Then
                results = FilterByTaxonomy(results, taxonomy)
            End If

            ' Apply limit after all filtering
            If results.Count > limit Then
                Return New JArray(results.Take(limit))
            End If

            Return results

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function
    Private Function CreateSpecialtyConditionGroup(specialty As String) As JObject
        Return New JObject(
        New JProperty("groupOperator", "OR"),
        New JProperty("conditions", New JArray(
            New JObject(
                New JProperty("property", "pri_spec"),
                New JProperty("value", specialty),
                New JProperty("operator", "=")
            ),
            New JObject(
                New JProperty("property", "sec_spec_1"),
                New JProperty("value", specialty),
                New JProperty("operator", "=")
            ),
            New JObject(
                New JProperty("property", "sec_spec_2"),
                New JProperty("value", specialty),
                New JProperty("operator", "=")
            ),
            New JObject(
                New JProperty("property", "sec_spec_3"),
                New JProperty("value", specialty),
                New JProperty("operator", "=")
            ),
            New JObject(
                New JProperty("property", "sec_spec_4"),
                New JProperty("value", specialty),
                New JProperty("operator", "=")
            )
        ))
    )
    End Function

    Private Function FilterByTaxonomy(results As JArray, taxonomy As String) As JArray
        Dim filtered As New JArray()

        For Each result As JObject In results
            ' Check if taxonomy matches in any specialty field
            Dim priSpec = result("pri_spec")?.ToString()
            Dim secSpec1 = result("sec_spec_1")?.ToString()
            Dim secSpec2 = result("sec_spec_2")?.ToString()
            Dim secSpec3 = result("sec_spec_3")?.ToString()
            Dim secSpec4 = result("sec_spec_4")?.ToString()

            If taxonomy.Equals(priSpec, StringComparison.OrdinalIgnoreCase) OrElse
           taxonomy.Equals(secSpec1, StringComparison.OrdinalIgnoreCase) OrElse
           taxonomy.Equals(secSpec2, StringComparison.OrdinalIgnoreCase) OrElse
           taxonomy.Equals(secSpec3, StringComparison.OrdinalIgnoreCase) OrElse
           taxonomy.Equals(secSpec4, StringComparison.OrdinalIgnoreCase) Then
                filtered.Add(result)
            End If
        Next

        Return filtered
    End Function



    ''' <summary>
    ''' Searches by HCPCS procedure codes.
    ''' Use when looking for providers who perform specific procedures.
    ''' </summary>
    Public Async Function SearchByHCPCSAsync(hcpcsCode As String, Optional state As String = "") As Task(Of JArray)
        Try
            If String.IsNullOrWhiteSpace(hcpcsCode) Then
                Return New JArray()
            End If

            Dim filters As New Dictionary(Of String, String) From {
                {"filter[HCPCS_Cd]", hcpcsCode},
                {"size", "100"}
            }

            If Not String.IsNullOrWhiteSpace(state) Then
                filters.Add("filter[Rndrng_Prvdr_State_Abrvtn]", state)
            End If

            Dim url As String = BuildUrlWithQuery($"{CMS_DATA_API_BASE_URL}/{HCPCS_DATASET_ID}/data", filters)
            Return Await ExecuteGetRequestAsync(url)

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function


    ''' <summary>
    ''' Searches by drug name (brand or generic).
    ''' Use when looking for providers who prescribe specific medications.
    ''' </summary>
    Public Async Function SearchByDrugAsync(params As SearchParameters) As Task(Of JArray)
        Try
            'If String.IsNullOrWhiteSpace(params.BrandDrug) AndAlso String.IsNullOrWhiteSpace(params.GenericDrug) Then
            '    Return New JArray()
            'End If

            Dim filters As New Dictionary(Of String, String) From {{"size", "1000"}}

            If Not String.IsNullOrWhiteSpace(params.BrandDrug) Then filters.Add("filter[Brnd_Name]", params.BrandDrug)
            If Not String.IsNullOrWhiteSpace(params.GenericDrug) Then filters.Add("filter[Gnrc_Name]", params.GenericDrug)
            If Not String.IsNullOrWhiteSpace(params.State) Then filters.Add("filter[Prscrbr_State_Abrvtn]", params.State)
            If Not String.IsNullOrWhiteSpace(params.FirstName) Then filters.Add("filter[Prscrbr_First_Name]", params.FirstName & "*")
            If Not String.IsNullOrWhiteSpace(params.LastName) Then filters.Add("filter[Prscrbr_Last_Org_Name]", params.LastName & "*")
            If Not String.IsNullOrWhiteSpace(params.NPI) Then filters.Add("filter[Prscrbr_NPI]", params.NPI)
            If Not String.IsNullOrWhiteSpace(params.Taxonomy) Then filters.Add("filter[Prscrbr_Type]", params.Taxonomy)
            If Not String.IsNullOrWhiteSpace(params.City) Then filters.Add("filter[Prscrbr_City]", params.City)

            Dim url As String = BuildUrlWithQuery($"{CMS_DATA_API_BASE_URL}/{DRUG_DATASET_ID}/data", filters)
            Return Await ExecuteGetRequestAsync(url)

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Gets detailed provider information by specific NPI.
    ''' </summary>
    Public Async Function GetNationalDownloadableFileByNpiAsync(npi As String) As Task(Of JArray)
        Try
            If String.IsNullOrWhiteSpace(npi) Then
                Return New JArray()
            End If

            Dim conditions As New List(Of JObject) From {
                CreateCondition("npi", npi, "=")
            }

            Return Await ExecuteCmsPostRequestAsync(NATIONAL_DOWNLOADABLE_FILE_ID, conditions, 1, "results")

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

#End Region

#Region "Specialized Data Retrieval Methods"

    ''' <summary>
    ''' Gets facility affiliations for a specific provider.
    ''' </summary>
    Public Async Function GetFacilityAffiliationsByNpiAsync(npi As String) As Task(Of JArray)
        Try
            If String.IsNullOrWhiteSpace(npi) Then
                Return New JArray()
            End If

            Dim conditions As New List(Of JObject) From {
                CreateCondition("NPI", npi, "=")
            }

            Return Await ExecuteCmsPostRequestAsync(FACILITY_AFFILIATIONS_ID, conditions, 100, "data")

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Gets general facility affiliations data with pagination.
    ''' </summary>
    Public Async Function GetFacilityAffiliationsAsync(Optional limit As Integer = 10) As Task(Of JArray)
        Try
            Dim conditions As New List(Of JObject) From {
                CreateCondition("record_number", 1, ">")
            }
            Return Await ExecuteCmsPostRequestAsync(FACILITY_AFFILIATIONS_ID, conditions, limit, "data")

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Gets utilization data with pagination.
    ''' </summary>
    Public Async Function GetUtilizationDataAsync(Optional limit As Integer = 10) As Task(Of JArray)
        Try
            Dim conditions As New List(Of JObject) From {
                CreateCondition("record_number", 1, ">")
            }
            Return Await ExecuteCmsPostRequestAsync(UTILIZATION_DATA_ID, conditions, limit, "data")

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Gets order and referring data with pagination.
    ''' </summary>
    Public Async Function GetOrderAndReferringAsync(Optional offset As Integer = 0, Optional size As Integer = 10) As Task(Of JArray)
        Try
            Dim url As String = $"{CMS_DATA_API_BASE_URL}/{ORDER_REFERRING_ID}/data?offset={offset}&size={size}"
            Return Await ExecuteGetRequestAsync(url)

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Gets revalidation due dates with pagination.
    ''' </summary>
    Public Async Function GetRevalidationDueDatesAsync(Optional offset As Integer = 0, Optional size As Integer = 10) As Task(Of JArray)
        Try
            Dim url As String = $"{CMS_DATA_API_BASE_URL}/{REVALIDATION_DUE_DATES_ID}/data?offset={offset}&size={size}"
            Return Await ExecuteGetRequestAsync(url)

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Gets revalidation reassignments with pagination.
    ''' </summary>
    Public Async Function GetRevalidationReassignmentsAsync(Optional offset As Integer = 0, Optional size As Integer = 10) As Task(Of JArray)
        Try
            Dim url As String = $"{CMS_DATA_API_BASE_URL}/{REVALIDATION_REASSIGNMENTS_ID}/data?offset={offset}&size={size}"
            Return Await ExecuteGetRequestAsync(url)

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Gets Medicare enrollment data with pagination.
    ''' </summary>
    Public Async Function GetMedicareEnrollmentAsync(Optional offset As Integer = 0, Optional size As Integer = 10) As Task(Of JArray)
        Try
            Dim url As String = $"{CMS_DATA_API_BASE_URL}/{MEDICARE_ENROLLMENT_ID}/data?offset={offset}&size={size}"
            Return Await ExecuteGetRequestAsync(url)

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Gets National Downloadable File data with pagination.
    ''' </summary>
    Public Async Function GetNationalDownloadableFileAsync(Optional limit As Integer = 10) As Task(Of JArray)
        Try
            Dim conditions As New List(Of JObject) From {
                CreateCondition("record_number", 1, ">")
            }
            Return Await ExecuteCmsPostRequestAsync(NATIONAL_DOWNLOADABLE_FILE_ID, conditions, limit, "results")

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

#End Region

#Region "Internal Helper Methods"

    ''' <summary>
    ''' Executes GET requests to CMS APIs.
    ''' </summary>
    Private Async Function ExecuteGetRequestAsync(url As String, Optional resultProperty As String = Nothing) As Task(Of JArray)
        Try
            Dim response = Await _httpClient.GetAsync(url)

            If Not response.IsSuccessStatusCode Then
                Return New JArray()
            End If

            Dim json = Await response.Content.ReadAsStringAsync()

            If String.IsNullOrEmpty(resultProperty) Then
                Return JArray.Parse(json)
            Else
                Dim obj = JObject.Parse(json)
                Dim resultArray = TryCast(obj(resultProperty), JArray)
                Return If(resultArray, New JArray())
            End If

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Executes POST requests to CMS Provider Data APIs.
    ''' </summary>
    Private Async Function ExecuteCmsPostRequestAsync(datasetId As String, conditions As List(Of JObject), limit As Integer, resultProperty As String) As Task(Of JArray)
        Try
            Dim url As String = $"{CMS_PROVIDER_DATA_BASE_URL}/{datasetId}/0"

            Dim requestBody As New JObject(
                New JProperty("conditions", New JArray(conditions)),
                New JProperty("limit", Math.Min(limit, 200))
            )

            Using content = New StringContent(requestBody.ToString(), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PostAsync(url, content)

                If Not response.IsSuccessStatusCode Then
                    Return New JArray()
                End If

                Dim json = Await response.Content.ReadAsStringAsync()
                Dim obj = JObject.Parse(json)
                Dim resultArray = TryCast(obj(resultProperty), JArray)
                Return If(resultArray, New JArray())
            End Using

        Catch ex As Exception
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Builds complete URL with query parameters.
    ''' </summary>
    Private Function BuildUrlWithQuery(baseUrl As String, parameters As Dictionary(Of String, String)) As String
        If parameters Is Nothing OrElse parameters.Count = 0 Then
            Return baseUrl
        End If

        Dim queryString = String.Join("&", parameters.Select(Function(kvp) $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"))
        Dim separator = If(baseUrl.Contains("?"), "&", "?")
        Return $"{baseUrl}{separator}{queryString}"
    End Function

    ''' <summary>
    ''' Adds parameter to dictionary if value is not empty.
    ''' </summary>
    Private Sub AddParameterIfNotEmpty(parameters As Dictionary(Of String, String), key As String, value As String)
        If Not String.IsNullOrWhiteSpace(value) Then
            parameters.Add(key, value)
        End If
    End Sub
    Private Function FilterByName(results As JArray, fieldName As String, searchValue As String) As JArray
        Dim filtered As New JArray()
        Dim searchLower = searchValue.ToLower()

        For Each result As JObject In results
            Dim fieldValue = result(fieldName)?.ToString()

            If Not String.IsNullOrEmpty(fieldValue) AndAlso
           fieldValue.ToLower().StartsWith(searchLower) Then
                filtered.Add(result)
            End If
        Next

        Return filtered
    End Function
    ''' <summary>
    ''' Adds condition to list if value is not empty.
    ''' </summary>
    Private Sub AddConditionIfNotEmpty(conditions As List(Of JObject), propertyName As String, value As String, Optional isExact As Boolean = False)
        If Not String.IsNullOrWhiteSpace(value) Then
            If isExact = False Then
                value = value.Replace("*", "%")
                conditions.Add(CreateCondition(propertyName, value, "LIKE"))
            Else
                conditions.Add(CreateCondition(propertyName, value, "="))
            End If

        End If
    End Sub

    ''' <summary>
    ''' Creates a condition object for CMS API queries.
    ''' </summary>
    Private Function CreateCondition(propertyName As String, value As Object, MyOperator As String) As JObject
        Return New JObject(
            New JProperty("resource", "t"),
            New JProperty("property", propertyName),
            New JProperty("value", value),
            New JProperty("operator", MyOperator)
        )
    End Function

    ''' <summary>
    ''' Disposes the shared HttpClient when module is unloaded.
    ''' </summary>
    Public Sub Dispose()
        _httpClient?.Dispose()
    End Sub

#End Region

End Module
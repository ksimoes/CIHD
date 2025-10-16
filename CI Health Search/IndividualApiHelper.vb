Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json.Linq
Imports System.Collections.Concurrent

''' <summary>
''' Unified & Optimized API helper for CMS Provider searches.
''' Uses FilterBuilder module for ALL filter creation - eliminates ALL redundancy.
''' 
''' KEY IMPROVEMENTS:
''' - Single source of truth for all filters (FilterBuilder)
''' - No duplicate filter building logic
''' - All methods use consistent patterns
''' - Better caching strategy
''' - Simplified method signatures
''' </summary>
Module IndividualApiHelper

#Region "Configuration"

    Private ReadOnly _httpClient As New HttpClient() With {
        .Timeout = TimeSpan.FromSeconds(30),
        .MaxResponseContentBufferSize = 10485760
    }

    ' API Base URLs
    Private Const NPI_REGISTRY_BASE_URL As String = "https://npiregistry.cms.hhs.gov/api/?version=2.1"
    Private Const CMS_PROVIDER_DATA_BASE_URL As String = "https://data.cms.gov/provider-data/api/1/datastore/query"
    Private Const CMS_DATA_API_BASE_URL As String = "https://data.cms.gov/data-api/v1/dataset"

    ' Dataset IDs
    Private Const NATIONAL_DOWNLOADABLE_FILE_ID As String = "mj5m-pzi6"
    Private Const HCPCS_DATASET_ID As String = "92396110-2aed-4d63-a6a2-5d6207d46a29"
    Private Const DRUG_DATASET_ID As String = "9552739e-3d05-4c1b-8eff-ecabf391e2e5"

    ' Caching
    Private ReadOnly _cache As New ConcurrentDictionary(Of String, CacheEntry)
    Private Const CACHE_DURATION_MINUTES As Integer = 30

    Private Class CacheEntry
        Public Property Data As JArray
        Public Property ExpiresAt As DateTime
    End Class

#End Region

#Region "Unified Search Methods - All Use FilterBuilder"

    ''' <summary>
    ''' UNIFIED NPI Registry search - works with SearchParameters OR individual NPI.
    ''' This single method replaces both overloads and uses FilterBuilder for consistency.
    ''' </summary>
    ''' <param name="params">Search parameters (use New SearchParameters With {.NPI = "..."} for single NPI)</param>
    ''' <param name="limit">Maximum results (default 200, max 1000)</param>
    ''' <param name="skip">Pagination offset</param>
    ''' <returns>JArray of provider results</returns>
    Public Async Function SearchNpiRegistryAsync(
        params As SearchParameters,
        Optional limit As Integer = 200,
        Optional skip As Integer = 0
    ) As Task(Of JArray)
        Try
            ' Validate parameters
            If params Is Nothing Then Return New JArray()
            If limit <= 0 OrElse limit > 1000 Then limit = 1000
            If skip < 0 Then skip = 0

            ' Use FilterBuilder - single source of truth for all filters
            Dim filters = FilterBuilder.BuildFilters(params, FilterBuilder.GetNpiRegistryConfig())
            filters("limit") = limit.ToString()
            filters("skip") = skip.ToString()

            Dim url = BuildUrlWithQuery(NPI_REGISTRY_BASE_URL, filters)

            ' Check cache
            Dim cacheKey = $"npi_registry_{url.GetHashCode()}"
            Dim cached = GetCachedResult(cacheKey)
            If cached IsNot Nothing Then Return cached

            ' Execute request
            Dim results = Await ExecuteGetRequestAsync(url, "results")

            ' Cache successful results
            CacheResult(cacheKey, results, CACHE_DURATION_MINUTES)

            Return results

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"SearchNpiRegistryAsync Error: {ex.Message}")
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Convenience overload for NPI-only searches.
    ''' Example: SearchNpiRegistryAsync("1234567890")
    ''' </summary>
    Public Async Function SearchNpiRegistryAsync(npi As String) As Task(Of JArray)
        If String.IsNullOrWhiteSpace(npi) Then Return New JArray()
        Return Await SearchNpiRegistryAsync(New SearchParameters With {.NPI = npi}, 1, 0)
    End Function

    ''' <summary>
    ''' UNIFIED National Downloadable File search - uses FilterBuilder for consistency.
    ''' This single method replaces the old version with many parameters.
    ''' </summary>
    ''' <param name="params">Search parameters</param>
    ''' <param name="limit">Maximum results (default 10)</param>
    ''' <param name="isExact">True for exact match, False for partial/StartsWith match</param>
    ''' <returns>JArray of provider results</returns>
    Public Async Function SearchNationalDownloadableFileAsync(
        params As SearchParameters,
        Optional limit As Integer = 10,
        Optional isExact As Boolean = False
    ) As Task(Of JArray)
        Try
            If params Is Nothing Then Return New JArray()

            ' Use FilterBuilder - single source of truth
            Dim conditions = FilterBuilder.BuildCmsConditions(params,
                FilterBuilder.GetNationalDownloadableConfig(isExact))

            ' Validate we have search criteria
            If conditions.Count = 0 AndAlso String.IsNullOrEmpty(params.Taxonomy) Then
                Return New JArray()
            End If

            ' Execute API request (max 200 results from API)
            Dim results = Await ExecuteCmsPostRequestAsync(
                NATIONAL_DOWNLOADABLE_FILE_ID, conditions, 200, "results")

            If results Is Nothing OrElse results.Count = 0 Then
                Return New JArray()
            End If

            ' Apply client-side filtering for partial matches and taxonomy
            results = ApplyClientSideFilters(results, params)

            ' Apply limit after filtering
            If results.Count > limit Then
                Return New JArray(results.Take(limit))
            End If

            Return results

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"SearchNationalDownloadableFileAsync Error: {ex.Message}")
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Gets provider by NPI from National Downloadable File.
    ''' Convenience method with extended caching (60 min).
    ''' </summary>
    Public Async Function GetNationalDownloadableFileByNpiAsync(npi As String) As Task(Of JArray)
        Try
            If String.IsNullOrWhiteSpace(npi) Then Return New JArray()

            ' Check cache with longer expiration for direct lookups
            Dim cacheKey = $"natl_download_{npi}"
            Dim cached = GetCachedResult(cacheKey)
            If cached IsNot Nothing Then Return cached

            ' Search using unified method
            Dim params As New SearchParameters With {.NPI = npi}
            Dim results = Await SearchNationalDownloadableFileAsync(params, 1, True)

            ' Cache with longer expiration (2x normal duration)
            CacheResult(cacheKey, results, CACHE_DURATION_MINUTES * 2)

            Return results

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"GetNationalDownloadableFileByNpiAsync Error: {ex.Message}")
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Alias for GetNationalDownloadableFileByNpiAsync for backward compatibility.
    ''' </summary>
    Public Async Function GetByNpiAsync(npi As String) As Task(Of JArray)
        Return Await GetNationalDownloadableFileByNpiAsync(npi)
    End Function

    ''' <summary>
    ''' HCPCS procedure code search - uses FilterBuilder.
    ''' </summary>
    Public Async Function SearchByHCPCSAsync(params As SearchParameters) As Task(Of JArray)
        Try
            If params Is Nothing OrElse String.IsNullOrWhiteSpace(params.HCPCS) Then
                Return New JArray()
            End If

            ' Use FilterBuilder
            Dim filters = FilterBuilder.BuildFilters(params, FilterBuilder.GetHcpcsConfig())
            Dim url = BuildUrlWithQuery($"{CMS_DATA_API_BASE_URL}/{HCPCS_DATASET_ID}/data", filters)

            Return Await ExecuteGetRequestAsync(url)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"SearchByHCPCSAsync Error: {ex.Message}")
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Drug prescription search - uses FilterBuilder.
    ''' </summary>
    Public Async Function SearchByDrugAsync(params As SearchParameters) As Task(Of JArray)
        Try
            If params Is Nothing OrElse
               (String.IsNullOrWhiteSpace(params.BrandDrug) AndAlso
                String.IsNullOrWhiteSpace(params.GenericDrug)) Then
                Return New JArray()
            End If

            ' Use FilterBuilder
            Dim filters = FilterBuilder.BuildFilters(params, FilterBuilder.GetDrugConfig())
            Dim url = BuildUrlWithQuery($"{CMS_DATA_API_BASE_URL}/{DRUG_DATASET_ID}/data", filters)

            Return Await ExecuteGetRequestAsync(url)

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"SearchByDrugAsync Error: {ex.Message}")
            Return New JArray()
        End Try
    End Function

#End Region

#Region "Client-Side Filtering - Optimized Single-Pass"

    ''' <summary>
    ''' Applies all client-side filters in a single pass.
    ''' Used for partial name matching (StartsWith) and taxonomy filtering.
    ''' </summary>
    Private Function ApplyClientSideFilters(results As JArray, params As SearchParameters) As JArray
        ' Convert to LINQ enumerable for efficient filtering
        Dim filtered = results.Cast(Of JObject)().AsEnumerable()

        ' Apply name filters (StartsWith for partial matching)
        If Not String.IsNullOrEmpty(params.FirstName) Then
            Dim searchLower = params.FirstName.ToLower()
            filtered = filtered.Where(Function(r)
                                          Dim val = r("provider_first_name")?.ToString()
                                          Return Not String.IsNullOrEmpty(val) AndAlso val.ToLower().StartsWith(searchLower)
                                      End Function)
        End If

        If Not String.IsNullOrEmpty(params.MiddleName) Then
            Dim searchLower = params.MiddleName.ToLower()
            filtered = filtered.Where(Function(r)
                                          Dim val = r("provider_middle_name")?.ToString()
                                          Return Not String.IsNullOrEmpty(val) AndAlso val.ToLower().StartsWith(searchLower)
                                      End Function)
        End If

        If Not String.IsNullOrEmpty(params.LastName) Then
            Dim searchLower = params.LastName.ToLower()
            filtered = filtered.Where(Function(r)
                                          Dim val = r("provider_last_name")?.ToString()
                                          Return Not String.IsNullOrEmpty(val) AndAlso val.ToLower().StartsWith(searchLower)
                                      End Function)
        End If

        ' Apply taxonomy filter across all specialty fields
        If Not String.IsNullOrEmpty(params.Taxonomy) Then
            Dim specialtyFields = {"pri_spec", "sec_spec_1", "sec_spec_2", "sec_spec_3", "sec_spec_4"}
            filtered = filtered.Where(Function(r)
                                          Return specialtyFields.Any(Function(field)
                                                                         Dim value = r(field)?.ToString()
                                                                         Return params.Taxonomy.Equals(value, StringComparison.OrdinalIgnoreCase)
                                                                     End Function)
                                      End Function)
        End If

        ' Materialize results in single enumeration
        Return New JArray(filtered)
    End Function

#End Region

#Region "HTTP Execution - Unified & Clean"

    ''' <summary>
    ''' Executes GET request and returns JArray.
    ''' </summary>
    Private Async Function ExecuteGetRequestAsync(
        url As String,
        Optional resultProperty As String = Nothing
    ) As Task(Of JArray)
        Try
            Using response = Await _httpClient.GetAsync(url)
                If Not response.IsSuccessStatusCode Then
                    System.Diagnostics.Debug.WriteLine($"HTTP {response.StatusCode}: {url}")
                    Return New JArray()
                End If

                Dim json = Await response.Content.ReadAsStringAsync()
                If String.IsNullOrWhiteSpace(json) Then Return New JArray()

                ' Parse based on structure
                If String.IsNullOrEmpty(resultProperty) Then
                    Return JArray.Parse(json)
                Else
                    Dim obj = JObject.Parse(json)
                    Return If(TryCast(obj(resultProperty), JArray), New JArray())
                End If
            End Using

        Catch ex As HttpRequestException
            System.Diagnostics.Debug.WriteLine($"HTTP Error: {ex.Message}")
            Return New JArray()
        Catch ex As TaskCanceledException
            System.Diagnostics.Debug.WriteLine($"Timeout: {ex.Message}")
            Return New JArray()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Request Error: {ex.Message}")
            Return New JArray()
        End Try
    End Function

    ''' <summary>
    ''' Executes POST request to CMS APIs and returns JArray.
    ''' </summary>
    Private Async Function ExecuteCmsPostRequestAsync(
        datasetId As String,
        conditions As List(Of JObject),
        limit As Integer,
        resultProperty As String
    ) As Task(Of JArray)
        Try
            Dim url = $"{CMS_PROVIDER_DATA_BASE_URL}/{datasetId}/0"
            Dim requestBody As New JObject(
                New JProperty("conditions", New JArray(conditions)),
                New JProperty("limit", Math.Min(limit, 200))
            )

            Using content = New StringContent(requestBody.ToString(), Encoding.UTF8, "application/json")
                Using response = Await _httpClient.PostAsync(url, content)
                    If Not response.IsSuccessStatusCode Then
                        System.Diagnostics.Debug.WriteLine($"HTTP {response.StatusCode}: {datasetId}")
                        Return New JArray()
                    End If

                    Dim json = Await response.Content.ReadAsStringAsync()
                    If String.IsNullOrWhiteSpace(json) Then Return New JArray()

                    Dim obj = JObject.Parse(json)
                    Return If(TryCast(obj(resultProperty), JArray), New JArray())
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"POST Error: {ex.Message}")
            Return New JArray()
        End Try
    End Function

#End Region

#Region "URL Building"

    ''' <summary>
    ''' Builds URL with query parameters using StringBuilder.
    ''' </summary>
    Private Function BuildUrlWithQuery(
        baseUrl As String,
        parameters As Dictionary(Of String, String)
    ) As String
        If parameters Is Nothing OrElse parameters.Count = 0 Then Return baseUrl

        Dim sb As New StringBuilder(baseUrl)
        sb.Append(If(baseUrl.Contains("?"), "&", "?"))

        Dim first = True
        For Each kvp In parameters
            If Not first Then sb.Append("&")
            sb.Append(kvp.Key)
            sb.Append("=")
            sb.Append(Uri.EscapeDataString(kvp.Value))
            first = False
        Next

        Return sb.ToString()
    End Function

#End Region

#Region "Cache Management - Unified & Clean"

    ''' <summary>
    ''' Gets cached result if available and not expired.
    ''' </summary>
    Private Function GetCachedResult(cacheKey As String) As JArray
        Dim cached As CacheEntry = Nothing
        If _cache.TryGetValue(cacheKey, cached) AndAlso cached.ExpiresAt > DateTime.UtcNow Then
            System.Diagnostics.Debug.WriteLine($"Cache HIT: {cacheKey}")
            Return cached.Data
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Caches result with specified duration in minutes.
    ''' </summary>
    Private Sub CacheResult(cacheKey As String, data As JArray, durationMinutes As Integer)
        If data IsNot Nothing AndAlso data.Count > 0 Then
            _cache(cacheKey) = New CacheEntry With {
                .Data = data,
                .ExpiresAt = DateTime.UtcNow.AddMinutes(durationMinutes)
            }
            System.Diagnostics.Debug.WriteLine($"Cached {data.Count} results for {durationMinutes} min")
        End If
    End Sub

    ''' <summary>
    ''' Clears expired cache entries.
    ''' </summary>
    Public Sub ClearExpiredCache()
        Dim now = DateTime.UtcNow

        ' Find all expired entries - use proper LINQ on the dictionary
        Dim expiredKeys As New List(Of String)
        For Each kvp In _cache
            If kvp.Value.ExpiresAt < now Then
                expiredKeys.Add(kvp.Key)
            End If
        Next

        ' Remove them
        For Each key In expiredKeys
            Dim removed As CacheEntry = Nothing
            _cache.TryRemove(key, removed)
        Next

        If expiredKeys.Count > 0 Then
            System.Diagnostics.Debug.WriteLine($"Cleared {expiredKeys.Count} expired entries")
        End If
    End Sub

    ''' <summary>
    ''' Clears all cached data.
    ''' </summary>
    Public Sub ClearAllCache()
        Dim count = _cache.Count
        _cache.Clear()
        System.Diagnostics.Debug.WriteLine($"Cleared all cache ({count} entries)")
    End Sub

    ''' <summary>
    ''' Gets cache statistics.
    ''' </summary>
    Public Function GetCacheStats() As String
        Dim total = _cache.Count
        Dim now = DateTime.UtcNow

        ' Count expired entries manually
        Dim expired As Integer = 0
        For Each entry In _cache.Values
            If entry.ExpiresAt < now Then
                expired += 1
            End If
        Next

        Dim valid = total - expired
        Return $"Cache: {valid} valid, {expired} expired, {total} total"
    End Function

#End Region

#Region "Cleanup"

    ''' <summary>
    ''' Disposes resources.
    ''' </summary>
    Public Sub Dispose()
        Try
            _httpClient?.Dispose()
            _cache?.Clear()
            System.Diagnostics.Debug.WriteLine("IndividualApiHelper disposed")
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine($"Dispose Error: {ex.Message}")
        End Try
    End Sub

#End Region

End Module
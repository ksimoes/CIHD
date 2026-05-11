' ============================================================================
' Optimized ApiHelper.vb - Centralized API access with caching and error handling
' ============================================================================

Imports System.Net.Http
Imports Newtonsoft.Json.Linq
Imports System.Data

Public Module ApiHelper

    ' ============================================================================
    ' SHARED RESOURCES
    ' ============================================================================

    ' Shared HttpClient for connection pooling and better performance
    Private ReadOnly _sharedHttpClient As New Lazy(Of HttpClient)(
        Function()
            Dim client = New HttpClient()
            client.Timeout = TimeSpan.FromSeconds(60)
            client.DefaultRequestHeaders.Add("User-Agent", "HospitalSearchApp/1.0")
            Return client
        End Function)

    Private ReadOnly Property SharedClient As HttpClient
        Get
            Return _sharedHttpClient.Value
        End Get
    End Property

    ' API response caching
    Private ReadOnly _apiCache As New Dictionary(Of String, (Data As Object, Timestamp As DateTime))
    Private ReadOnly _cacheLock As New Object()
    Private Const CacheExpirationMinutes As Integer = 15
    Private Const MaxCacheSize As Integer = 50

    ' ============================================================================
    ' API URLS - Centralized configuration
    ' ============================================================================

    Public ReadOnly ApiUrls As New Dictionary(Of String, String) From {
        {"PatientOrigin", "https://data.cms.gov/data-api/v1/dataset/8708ca8b-8636-44ed-8303-724cbfaf78ad/data"},
        {"PatientOrigin2019", "https://data.cms.gov/data-api/v1/dataset/2713ba99-c59e-4b25-9a3d-3661d35988da/data"},
        {"PatientOrigin2023", "https://data.cms.gov/data-api/v1/dataset/7f749f00-bfa9-4377-9a98-90c15cacc2f3/data"},
        {"CeoApi", "https://data.cms.gov/data-api/v1/dataset/029c119f-f79c-49be-9100-344d31d10344/data"},
        {"NewApi", "https://data.cms.gov/data-api/v1/dataset/690ddc6c-2767-4618-b277-420ffb2bf27c/data"},
        {"ApcApi", "https://data.cms.gov/data-api/v1/dataset/ccbc9a44-40d4-46b4-a709-5caa59212e50/data"},
        {"DepartmentsApi", "https://data.cms.gov/data-api/v1/dataset/8ba0f9b4-9493-4aa0-9f82-44ea9468d1b5/data"},
        {"MainProfileApi", "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data"},
        {"ProfileMainUrl", "https://data.cms.gov/data-api/v1/dataset/8143cbc7-484f-438b-9dfa-2e81d5d6a1ed/data"},
        {"HCPCSApi", "https://data.cms.gov/data-api/v1/dataset/92396110-2aed-4d63-a6a2-5d6207d46a29/data"}
    }

    ' ============================================================================
    ' MAIN API METHODS - WITH CACHING AND RETRY LOGIC
    ' ============================================================================

    ''' <summary>
    ''' Generic async API call that returns a JArray with caching support
    ''' </summary>
    Public Async Function GetApiDataAsync(url As String, Optional useCache As Boolean = True, Optional maxRetries As Integer = 3) As Task(Of JArray)
        ' Check cache first
        If useCache Then
            Dim cached = GetFromCache(Of JArray)(url)
            If cached IsNot Nothing Then
                Return cached
            End If
        End If

        ' Retry logic
        For retryCount = 0 To maxRetries - 1
            Dim shouldRetry As Boolean = False
            Dim delayMs As Integer = 0
            Dim lastError As String = ""

            Try
                Dim response = Await SharedClient.GetAsync(url)

                If response.IsSuccessStatusCode Then
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim data = JArray.Parse(json)

                    ' Cache successful result
                    If useCache Then
                        AddToCache(url, data)
                    End If

                    Return data

                ElseIf response.StatusCode = Net.HttpStatusCode.TooManyRequests Then
                    shouldRetry = True
                    delayMs = CInt(Math.Pow(2, retryCount + 1) * 1000)
                    lastError = "Rate limited"
                Else
                    lastError = $"HTTP {response.StatusCode}"
                    Exit For
                End If

            Catch ex As TaskCanceledException
                shouldRetry = True
                delayMs = 1000
                lastError = "Request timed out"
            Catch ex As Exception
                LogError($"API error for {url}: {ex.Message}")
                Exit For
            End Try

            ' Handle retry outside Catch block (VB.NET requirement)
            If shouldRetry AndAlso retryCount < maxRetries - 1 Then
                LogError($"API: {lastError}, retrying in {delayMs}ms (attempt {retryCount + 1})")
                Await Task.Delay(delayMs)
            End If
        Next

        ' All retries failed
        LogError($"API call failed after {maxRetries} attempts: {url}")
        Return New JArray()
    End Function

    ''' <summary>
    ''' Generic async API call that returns a DataTable with caching support
    ''' </summary>
    Public Async Function GetTableFromApiAsync(url As String, Optional useCache As Boolean = True) As Task(Of DataTable)
        ' Check cache first
        If useCache Then
            Dim cached = GetFromCache(Of DataTable)(url & "_table")
            If cached IsNot Nothing Then
                Return cached.Copy() ' Return copy to prevent modification
            End If
        End If

        Dim dt As New DataTable()
        Dim data As JArray = Await GetApiDataAsync(url, useCache)

        If data.Count > 0 Then
            ' Add columns from first item
            For Each prop In CType(data(0), JObject).Properties()
                dt.Columns.Add(prop.Name)
            Next

            ' Add rows
            For Each item As JObject In data
                Dim row As DataRow = dt.NewRow()
                For Each prop In item.Properties()
                    row(prop.Name) = If(prop.Value IsNot Nothing, prop.Value.ToString(), DBNull.Value)
                Next
                dt.Rows.Add(row)
            Next

            ' Cache the result
            If useCache Then
                AddToCache(url & "_table", dt)
            End If
        End If

        Return dt
    End Function

    ''' <summary>
    ''' Compatibility method - returns JArray (kept for backwards compatibility)
    ''' </summary>
    Public Async Function GetAPIArrayAsync(strAPIurl As String) As Task(Of JArray)
        Return Await GetApiDataAsync(strAPIurl, True)
    End Function

    ' ============================================================================
    ' CACHE MANAGEMENT
    ' ============================================================================

    Private Function GetFromCache(Of T)(cacheKey As String) As T
        SyncLock _cacheLock
            If _apiCache.ContainsKey(cacheKey) Then
                Dim cached = _apiCache(cacheKey)
                If DateTime.Now.Subtract(cached.Timestamp).TotalMinutes < CacheExpirationMinutes Then
                    Return CType(cached.Data, T)
                Else
                    _apiCache.Remove(cacheKey)
                End If
            End If
        End SyncLock
        Return Nothing
    End Function

    Private Sub AddToCache(cacheKey As String, data As Object)
        SyncLock _cacheLock
            ' Enforce cache size limit
            If _apiCache.Count >= MaxCacheSize Then
                Dim oldestKeys = _apiCache.OrderBy(Function(x) x.Value.Timestamp).Take(10).Select(Function(x) x.Key).ToList()
                For Each key In oldestKeys
                    _apiCache.Remove(key)
                Next
            End If

            _apiCache(cacheKey) = (data, DateTime.Now)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Clears all cached API data
    ''' </summary>
    Public Sub ClearCache()
        SyncLock _cacheLock
            _apiCache.Clear()
        End SyncLock
    End Sub

    ''' <summary>
    ''' Gets cache statistics for monitoring
    ''' </summary>
    Public Function GetCacheStats() As (Count As Integer, OldestEntry As DateTime?)
        SyncLock _cacheLock
            Dim oldest As DateTime? = Nothing
            If _apiCache.Count > 0 Then
                oldest = _apiCache.Values.Min(Function(x) x.Timestamp)
            End If
            Return (_apiCache.Count, oldest)
        End SyncLock
    End Function

    ' ============================================================================
    ' HELPER METHODS
    ' ============================================================================

    ''' <summary>
    ''' Simple logging for debugging and monitoring
    ''' </summary>
    Private Sub LogError(message As String)
        Try
            Dim logPath = IO.Path.Combine(Application.StartupPath, "ApiLog.txt")
            IO.File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}")
        Catch
            ' Don't let logging errors crash the app
        End Try
    End Sub

End Module
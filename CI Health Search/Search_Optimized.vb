'' Optimized Search.vb - Key Methods
'' This file contains optimized versions of the most critical methods

'Imports System.Data.SqlClient
'Imports System.Net.Http
'Imports System.Text.RegularExpressions
'Imports System.Threading
'Imports Newtonsoft.Json.Linq

'Public Class Search
'    ' ============================================================================
'    ' CONFIGURATION - Move to App.config in production
'    ' ============================================================================
'    Private ReadOnly connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

'    Public Shared SearchHospital As New HospitalContext()

'    ' ============================================================================
'    ' SHARED RESOURCES - Reusable across all instances
'    ' ============================================================================

'    ' Shared HttpClient for better performance and connection pooling
'    Private Shared ReadOnly _httpClient As New Lazy(Of HttpClient)(
'        Function()
'            Dim client = New HttpClient()
'            client.Timeout = TimeSpan.FromSeconds(30)
'            client.DefaultRequestHeaders.Add("User-Agent", "HospitalSearchApp/1.0")
'            Return client
'        End Function)

'    Private Shared ReadOnly Property SharedHttpClient As HttpClient
'        Get
'            Return _httpClient.Value
'        End Get
'    End Property

'    ' Compiled regex for better performance
'    Private Shared ReadOnly WhitespaceRegex As New Regex("\s+", RegexOptions.Compiled)

'    ' API response caching
'    Private Shared ReadOnly _apiCache As New Dictionary(Of String, (Data As DataTable, Timestamp As DateTime))
'    Private Shared ReadOnly _cacheLock As New Object()
'    Private Const CacheExpirationMinutes As Integer = 15
'    Private Const MaxCacheSize As Integer = 50

'    ' Normalization caching
'    Private Shared ReadOnly _normalizedNameCache As New Dictionary(Of String, String)
'    Private Shared ReadOnly _normalizedAddressCache As New Dictionary(Of String, String)
'    Private Shared ReadOnly _normalizationLock As New Object()

'    ' Cancellation support
'    Private searchCancellation As CancellationTokenSource

'    ' ============================================================================
'    ' LOOKUP DICTIONARIES
'    ' ============================================================================

'    Private ReadOnly FacilityTypeMap As New Dictionary(Of String, String) From {
'        {"Childrens", "CH"},
'        {"Critical Access", "CAH"},
'        {"Long Term", "LTCH"},
'        {"Psychiatric", "PH"},
'        {"Rehabilitation", "RH"},
'        {"Rural Emergency Hospital", "RHC"},
'        {"Short Term Acute Care", "STH"},
'        {"Federally Qualified Health Centers", "FQHC"},
'        {"Rural Health Clinic", "RHC"}
'    }

'    Private ReadOnly TypeOfControlMap As New Dictionary(Of String, String) From {
'        {"1", "Voluntary Non‐Profit‐Church"},
'        {"2", "Voluntary Non‐Profit‐Other"},
'        {"3", "Proprietary‐Individual"},
'        {"4", "Proprietary‐Corporation"},
'        {"5", "Proprietary‐Partnership"},
'        {"6", "Proprietary‐Other"},
'        {"7", "Governmental‐Federal"},
'        {"8", "Governmental‐City‐County"},
'        {"9", "Governmental‐County"},
'        {"10", "Governmental‐State"},
'        {"11", "Governmental‐Hospital District"},
'        {"12", "Governmental‐City"},
'        {"13", "Governmental‐Other"}
'    }

'    ' ============================================================================
'    ' DATABASE METHODS - SQL INJECTION FIXED
'    ' ============================================================================

'    ''' <summary>
'    ''' Check if SQL should be used for a state (FIXED: SQL Injection vulnerability)
'    ''' </summary>
'    Public Function useSQL(ByRef selectedState As String) As Boolean
'        Using conn As New SqlConnection(connectionString)
'            Using cmd As New SqlCommand("SELECT UseSql FROM dbo.States WHERE StateCode = @StateCode", conn)
'                cmd.Parameters.AddWithValue("@StateCode", selectedState)
'                conn.Open()
'                Dim result = cmd.ExecuteScalar()
'                Return If(result IsNot Nothing, CBool(result), False)
'            End Using
'        End Using
'    End Function

'    ' ============================================================================
'    ' API METHODS - WITH CACHING AND RETRY LOGIC
'    ' ============================================================================

'    ''' <summary>
'    ''' Get JSON data from API with caching support (CORRECTED - No Await in Catch)
'    ''' </summary>
'    Private Async Function GetJsonDataAsync(apiUrl As String,
'                                            Optional filterFunc As Func(Of JToken, Boolean) = Nothing,
'                                            Optional errorPrefix As String = "API",
'                                            Optional useCache As Boolean = True,
'                                            Optional maxRetries As Integer = 3) As Task(Of DataTable)
'        ' Check cache first
'        If useCache Then
'            Dim cachedData = GetFromCache(apiUrl)
'            If cachedData IsNot Nothing Then
'                Return cachedData
'            End If
'        End If

'        ' Retry logic (moved delay outside Catch block)
'        For retryCount = 0 To maxRetries - 1
'            Dim shouldRetry As Boolean = False
'            Dim delayMs As Integer = 0
'            Dim lastError As String = ""

'            Try
'                Dim response = Await SharedHttpClient.GetAsync(apiUrl)

'                If response.IsSuccessStatusCode Then
'                    Dim json = Await response.Content.ReadAsStringAsync()
'                    Dim dt = ProcessJsonResponse(json, filterFunc)

'                    ' Cache successful result
'                    If useCache AndAlso dt IsNot Nothing Then
'                        AddToCache(apiUrl, dt)
'                    End If

'                    Return dt

'                ElseIf response.StatusCode = Net.HttpStatusCode.TooManyRequests Then
'                    ' Rate limited
'                    shouldRetry = True
'                    delayMs = CInt(Math.Pow(2, retryCount + 1) * 1000) ' 2s, 4s, 8s
'                    lastError = "Rate limited"
'                Else
'                    lastError = $"HTTP {response.StatusCode}: {response.ReasonPhrase}"
'                    Exit For ' Don't retry for other HTTP errors
'                End If

'            Catch ex As TaskCanceledException
'                lastError = "Request timed out"
'                shouldRetry = True
'                delayMs = 1000
'            Catch ex As Exception
'                lastError = ex.Message
'                Exit For ' Don't retry for unexpected errors
'            End Try

'            ' Handle retry/delay outside of Catch block (VB.NET requirement)
'            If shouldRetry AndAlso retryCount < maxRetries - 1 Then
'                LogError($"{errorPrefix}: {lastError}, waiting {delayMs}ms before retry {retryCount + 1}")
'                Await Task.Delay(delayMs)
'            ElseIf Not shouldRetry Then
'                ' Non-retryable error occurred
'                LogError($"{errorPrefix} error: {lastError}")
'                MessageBox.Show($"{errorPrefix} error: {lastError}")
'                Return Nothing
'            End If
'        Next

'        ' All retries exhausted
'        MessageBox.Show($"{errorPrefix} error after {maxRetries} attempts")
'        Return Nothing
'    End Function

'    ''' <summary>
'    ''' Process JSON response into DataTable with optional filtering
'    ''' </summary>
'    Private Function ProcessJsonResponse(json As String, Optional filterFunc As Func(Of JToken, Boolean) = Nothing) As DataTable
'        Try
'            Dim data = JArray.Parse(json)
'            If data.Count = 0 Then Return Nothing

'            Dim dt As New DataTable()

'            ' Add columns from first item
'            For Each col In data(0).ToObject(Of JObject)().Properties()
'                dt.Columns.Add(col.Name)
'            Next

'            ' Add rows with optional filtering
'            For Each item In data
'                If filterFunc Is Nothing OrElse filterFunc(item) Then
'                    Dim row = dt.NewRow()
'                    For Each col In dt.Columns
'                        row(col.ToString()) = item(col.ToString())
'                    Next
'                    dt.Rows.Add(row)
'                End If
'            Next

'            Return dt
'        Catch ex As Exception
'            LogError($"JSON processing error: {ex.Message}")
'            Return Nothing
'        End Try
'    End Function

'    ''' <summary>
'    ''' Get NPI Registry information
'    ''' </summary>
'    Private Async Function GetNpiRegistryInfoAsync(npi As String) As Task(Of JObject)
'        Dim apiUrl As String = $"https://npiregistry.cms.hhs.gov/api/?number={Uri.EscapeDataString(npi)}&version=2.1"
'        Try
'            Dim response = Await SharedHttpClient.GetAsync(apiUrl)
'            If response.IsSuccessStatusCode Then
'                Dim json = Await response.Content.ReadAsStringAsync()
'                Dim obj = JObject.Parse(json)
'                If obj("results") IsNot Nothing AndAlso obj("results").HasValues Then
'                    Return obj("results")(0)
'                End If
'            End If
'        Catch ex As Exception
'            LogError($"NPI Registry API error: {ex.Message}")
'        End Try
'        Return Nothing
'    End Function

'    ' ============================================================================
'    ' CACHE MANAGEMENT
'    ' ============================================================================

'    Private Function GetFromCache(cacheKey As String) As DataTable
'        SyncLock _cacheLock
'            If _apiCache.ContainsKey(cacheKey) Then
'                Dim cached = _apiCache(cacheKey)
'                If DateTime.Now.Subtract(cached.Timestamp).TotalMinutes < CacheExpirationMinutes Then
'                    LogError($"Cache hit: {cacheKey.Substring(0, Math.Min(50, cacheKey.Length))}")
'                    Return cached.Data.Copy() ' Return a copy to prevent modification
'                Else
'                    ' Expired - remove it
'                    _apiCache.Remove(cacheKey)
'                End If
'            End If
'        End SyncLock
'        Return Nothing
'    End Function

'    Private Sub AddToCache(cacheKey As String, data As DataTable)
'        SyncLock _cacheLock
'            ' Enforce cache size limit
'            If _apiCache.Count >= MaxCacheSize Then
'                ' Remove oldest entries
'                Dim oldestKeys = _apiCache.OrderBy(Function(x) x.Value.Timestamp).Take(10).Select(Function(x) x.Key).ToList()
'                For Each key In oldestKeys
'                    _apiCache.Remove(key)
'                Next
'                LogError($"Cache cleanup: Removed {oldestKeys.Count} old entries")
'            End If

'            _apiCache(cacheKey) = (data.Copy(), DateTime.Now)
'            LogError($"Cache add: {cacheKey.Substring(0, Math.Min(50, cacheKey.Length))}")
'        End SyncLock
'    End Sub

'    Public Shared Sub ClearCache()
'        SyncLock _cacheLock
'            _apiCache.Clear()
'        End SyncLock
'    End Sub

'    ' ============================================================================
'    ' FUZZY MATCHING - WITH CACHING
'    ' ============================================================================

'    ''' <summary>
'    ''' Normalize name for fuzzy matching (with caching)
'    ''' </summary>
'    Private Function NormalizeName(name As String) As String
'        If String.IsNullOrEmpty(name) Then Return ""

'        ' Check cache first
'        SyncLock _normalizationLock
'            If _normalizedNameCache.ContainsKey(name) Then
'                Return _normalizedNameCache(name)
'            End If
'        End SyncLock

'        ' Perform normalization
'        Dim cleaned = name.ToLower().Trim()
'        cleaned = cleaned.Replace(".", "").Replace(",", "").Replace("-", " ")

'        Dim commonWords = New String() {"hospital", "center", "medical", "the", "of", "and"}
'        For Each word In commonWords
'            cleaned = cleaned.Replace(word, "")
'        Next

'        cleaned = WhitespaceRegex.Replace(cleaned, " ").Trim()

'        ' Add to cache
'        SyncLock _normalizationLock
'            If _normalizedNameCache.Count > 1000 Then
'                _normalizedNameCache.Clear() ' Prevent unbounded growth
'            End If
'            _normalizedNameCache(name) = cleaned
'        End SyncLock

'        Return cleaned
'    End Function

'    ''' <summary>
'    ''' Normalize address for fuzzy matching (with caching)
'    ''' </summary>
'    Private Function NormalizeAddress(addr As String) As String
'        If String.IsNullOrWhiteSpace(addr) Then Return ""

'        ' Check cache first
'        SyncLock _normalizationLock
'            If _normalizedAddressCache.ContainsKey(addr) Then
'                Return _normalizedAddressCache(addr)
'            End If
'        End SyncLock

'        ' Perform normalization
'        Dim cleaned = addr.ToLower().Trim()
'        cleaned = cleaned.Replace("us highway", "hwy").Replace("us hwy", "hwy").Replace("highway", "hwy")
'        cleaned = cleaned.Replace("street", "st").Replace("avenue", "ave").Replace("road", "rd")
'        cleaned = cleaned.Replace("drive", "dr").Replace("boulevard", "blvd").Replace("lane", "ln")
'        cleaned = cleaned.Replace("east", "e").Replace("west", "w").Replace("north", "n").Replace("south", "s")
'        cleaned = cleaned.Replace(".", "").Replace(",", "")
'        cleaned = WhitespaceRegex.Replace(cleaned, " ")

'        ' Add to cache
'        SyncLock _normalizationLock
'            If _normalizedAddressCache.Count > 1000 Then
'                _normalizedAddressCache.Clear()
'            End If
'            _normalizedAddressCache(addr) = cleaned
'        End SyncLock

'        Return cleaned
'    End Function

'    Private Function IsFuzzyMatch(npiName As String, facilityName As String) As Boolean
'        Dim normNpi = NormalizeName(npiName)
'        Dim normFacility = NormalizeName(facilityName)
'        Return normFacility.Contains(normNpi) OrElse normNpi.Contains(normFacility)
'    End Function

'    Private Function IsFuzzyAddressMatch(addr1 As String, addr2 As String) As Boolean
'        Dim norm1 = NormalizeAddress(addr1)
'        Dim norm2 = NormalizeAddress(addr2)
'        If String.IsNullOrEmpty(norm1) OrElse String.IsNullOrEmpty(norm2) Then Return False

'        Dim tokens1 = norm1.Split(" "c).Where(Function(t) t.Length > 1).ToArray()
'        Dim tokens2 = norm2.Split(" "c).Where(Function(t) t.Length > 1).ToArray()
'        Dim overlap = tokens1.Intersect(tokens2).Count()
'        Return overlap >= 3 OrElse norm1.Contains(norm2) OrElse norm2.Contains(norm1)
'    End Function

'    ' ============================================================================
'    ' HELPER METHODS
'    ' ============================================================================

'    Private Sub SetupHospProfile()
'        SearchHospital.CMSNum = If(Not String.IsNullOrWhiteSpace(txtCmsCertNumDemoAll.Text), txtCmsCertNumDemoAll.Text.Trim(), TextBox39.Text.Trim())
'        SearchHospital.Zip = If(Not String.IsNullOrWhiteSpace(txtZipCodeDemoAll.Text), txtZipCodeDemoAll.Text.Trim(), TextBox40.Text.Trim())
'        SearchHospital.State = If(lbStateAll.SelectedItem IsNot Nothing, lbStateAll.SelectedItem.ToString().Trim(), "")
'        SearchHospital.Name = VerifiySearch(txtHospitalNameAll)
'        SearchHospital.City = VerifiySearch(txtCityAll)
'        SearchHospital.Phone = VerifiySearch(txtAreaCodeAll)
'        SearchHospital.NPI = VerifiySearch(txtNpiAll)
'    End Sub

'    Public Function VerifiySearch(txtSearched As TextBox) As String
'        If txtSearched IsNot Nothing AndAlso txtSearched.Text.Length > 0 Then
'            Return txtSearched.Text.Trim()
'        Else
'            Return Nothing
'        End If
'    End Function

'    Public Function CleanMeUp(strMydata As Object, Optional ByRef isNumber As Boolean = False) As String
'        If strMydata Is Nothing Then
'            Return If(isNumber, "0", "N/A")
'        End If

'        Dim strVal As String = strMydata.ToString().Trim()
'        If String.IsNullOrEmpty(strVal) OrElse strVal = "Result" OrElse strVal = "N/A" Then
'            Return If(isNumber, "0", "N/A")
'        End If

'        If isNumber Then
'            Dim dec As Decimal
'            If Decimal.TryParse(strVal.Replace("$", "").Replace(",", ""), dec) Then
'                Return dec.ToString("N")
'            Else
'                Return "0"
'            End If
'        Else
'            Return strVal
'        End If
'    End Function

'    ''' <summary>
'    ''' Simple logging for debugging and monitoring
'    ''' </summary>
'    Private Sub LogError(message As String)
'        Try
'            Dim logPath = IO.Path.Combine(Application.StartupPath, "SearchLog.txt")
'            IO.File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}")
'        Catch
'            ' Don't let logging errors crash the app
'        End Try
'    End Sub

'    Private Sub PopulateTypeOfControlList()
'        lbControl.Items.Clear()
'        lbControl.Items.Add(New ControlTypeItem With {.Code = "", .Desc = "All"})
'        For Each kvp In TypeOfControlMap
'            lbControl.Items.Add(New ControlTypeItem With {.Code = kvp.Key, .Desc = kvp.Value})
'        Next
'        lbControl.ClearSelected()
'    End Sub

'    ' ============================================================================
'    ' EVENT HANDLERS
'    ' ============================================================================

'    Private Sub Search_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        PopulateTypeOfControlList()
'    End Sub

'    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
'        ' Clear all search fields
'        lbStateAll.ClearSelected()
'        txtCityAll.Clear()
'        txtCmsCertNumDemoAll.Clear()
'        txtNpiAll.Clear()
'        txtHospitalNameAll.Clear()
'        txtAreaCodeAll.Clear()
'        txtZipCodeDemoAll.Clear()
'        txtMaxTotalBedsAll.Clear()
'        txtMinTotalBedsAll.Clear()
'        txtNpiAll.Clear()
'        lbTypeFacilityCharAll.ClearSelected()
'        txtcountygeoall.Clear()
'        txtMinTotPatRevAll.Clear()
'        txtMaxTotPatRevAll.Clear()
'        lbControl.ClearSelected()
'    End Sub

'    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
'        Individual_Search.Show()
'    End Sub

'End Class

'Public Class ControlTypeItem
'    Public Property Code As String
'    Public Property Desc As String
'    Public Overrides Function ToString() As String
'        Return Desc
'    End Function
'End Class

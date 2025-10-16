Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Newtonsoft.Json.Linq
Imports System.Windows.Forms.VisualStyles
''' <summary>
''' Individual Provider Search Form
''' 
''' This form provides a clean, user-friendly interface for searching healthcare providers.
''' It intelligently routes searches to the most appropriate data source based on user input:
''' 
''' Search Priority:
''' 1. NPI (opens profile directly)
''' 2. HCPCS codes (procedure-based search) 
''' 3. Medications (drug prescription search)
''' 4. General demographics/education search
''' 
''' The form includes proper validation, error handling, and user feedback.
''' </summary>
Public Class Individual_Search

#Region "Private Fields"

    ' For managing async operations and preventing concurrent searches
    Private _cancellationTokenSource As CancellationTokenSource
    Private _isSearchInProgress As Boolean = False

#End Region

#Region "Event Handlers"

    ''' <summary>
    ''' Main search button - orchestrates the entire search process
    ''' </summary>
    Private Async Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If Not cboSearchType.SelectedIndex = 0 Then
            ' Prevent multiple simultaneous searches
            If _isSearchInProgress Then
                MessageBox.Show("A search is already running. Please wait for it to complete.",
                          "Search In Progress", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Try
                Await PerformSearchAsync()
            Catch ex As OperationCanceledException
                ShowMessage("Search was cancelled.", "Search Cancelled", MessageBoxIcon.Information)
            Catch ex As Exception
                ShowMessage($"Search failed: {ex.Message}", "Search Error", MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("Please select a search type from the dropdown.", "Select Search Type", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

    End Sub

    ''' <summary>
    ''' Switch to organization search form
    ''' </summary>
    Private Sub btnOrgSearch_Click(sender As Object, e As EventArgs) Handles btnOrgSearch.Click
        Try
            Search.Show()
            Me.Hide()
        Catch ex As Exception
            ShowMessage($"Could not open organization search: {ex.Message}", "Error", MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Clear all search fields
    ''' </summary>
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ClearAllFields()
    End Sub


#End Region

#Region "Search Logic"

    ''' <summary>
    ''' Main search orchestrator - manages the entire search workflow
    ''' </summary>
    Private Async Function PerformSearchAsync() As Task
        ' Setup for async operation
        _cancellationTokenSource = New CancellationTokenSource()
        SetSearchInProgress(True)

        Try
            ' Extract and validate search parameters
            Dim searchParams = GetSearchParameters()
            Dim validation = ValidateSearchParameters(searchParams)

            If Not validation.IsValid Then
                ShowMessage(validation.ErrorMessage, "Invalid Search", MessageBoxIcon.Warning)
                Return
            End If

            ' Route to appropriate search method
            Await RouteSearch(searchParams)

        Finally
            SetSearchInProgress(False)
        End Try
    End Function

    ''' <summary>
    ''' Routes search to the best method based on user input
    ''' </summary>
    Private Async Function RouteSearch(params As SearchParameters) As Task
        ' Priority 1: Direct NPI lookup
        Select Case cboSearchType.SelectedItem.ToString
            Case "NPI Registry"
                Await HandleNpiRegistrySearch(params)
            Case "NPI Downloadable Update"
                If Not String.IsNullOrWhiteSpace(params.NPI) Then HandleDirectNpiLookup(params.NPI)
            Case "Healthcare Common Procedure Code"
                If Not String.IsNullOrWhiteSpace(params.HCPCS) Then Await HandleHCPCSSearch(params)
            Case "Drugs Prescribed"
                Await HandleDrugSearch(params)
            Case "Medical School"
                Await HandleMedicalSchoolSearch(params)
        End Select

        ' Priority 2: HCPCS procedure code search
        ' Priority 3: Medication search
        ' Priority 4: General provider search

    End Function

#End Region

#Region "Specific Search Handlers"

    ''' Handles direct NPI lookup - opens profile immediately
    Private Sub HandleDirectNpiLookup(npi As String)
        Try
            Dim profileForm As New IndividualProfileForm(npi)
            profileForm.Show()
            profileForm.tbNpiResult.Text = npi
        Catch ex As Exception
            ShowMessage($"Could Not open provider profile for NPI {npi}:  {ex.Message}",
                      "Profile Error", MessageBoxIcon.Error)
        End Try
    End Sub

    ''' Handles HCPCS procedure code searches
    Private Async Function HandleHCPCSSearch(params As SearchParameters) As Task
        Dim results = Await IndividualApiHelper.SearchByHCPCSAsync(params)

        If results Is Nothing OrElse results.Count = 0 Then
            Dim stateText = If(String.IsNullOrWhiteSpace(params.State), "", $" in {params.State}")
            ShowMessage($"No providers found who perform procedure {params.HCPCS}{stateText}.",
                      "No Results", MessageBoxIcon.Information)
            Return
        End If

        ShowResults(results, False)
    End Function

    Public Async Function HandleNpiRegistrySearch(params As SearchParameters) As Task
        Dim results = Await IndividualApiHelper.SearchNpiRegistryAsync(params, 1000)
        ShowResultsOrNoResults(results, False, "providers matching your criteria")
    End Function
    Private Async Function HandleDrugSearch(params As SearchParameters) As Task
        Dim results = Await IndividualApiHelper.SearchByDrugAsync(params)

        If results Is Nothing OrElse results.Count = 0 Then
            Dim drugName = If(Not String.IsNullOrWhiteSpace(params.BrandDrug), params.BrandDrug, params.GenericDrug)
            ShowMessage($"No providers found who prescribe {drugName}.",
                      "No Results", MessageBoxIcon.Information)
            Return
        End If

        ShowResults(results, True)
    End Function

    ''' Handles medical school searches.
    Private Async Function HandleMedicalSchoolSearch(params As SearchParameters) As Task
        Dim results = Await IndividualApiHelper.SearchNationalDownloadableFileAsync(params, 1000, ckExact.Checked)
        ShowResultsOrNoResults(results, False, "providers matching your criteria")
    End Function



    Private Sub ShowResultsOrNoResults(results As JArray, showDrugColumns As Boolean, searchDescription As String)
        If results Is Nothing OrElse results.Count = 0 Then
            ShowMessage($"No {searchDescription}. Try broadening your search.", "No Results", MessageBoxIcon.Information)
            Return
        End If

        ShowResults(results, showDrugColumns)
    End Sub

    Public Sub LoadTaxonomyCombo()
        Dim connectionString As String = "Server=tcp:cihg-sql1.database.windows.net,1433;Initial Catalog=PapaSmurf;Persist Security Info=False;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=400000;"

        ' Fixed query - removed extra space after comma in server name
        Dim query As String = "SELECT DISTINCT [Provider Taxonomy Description Type] FROM dbo.TaxonomyCodes ORDER BY [Provider Taxonomy Description Type]"
        'If cboSearchType.SelectedIndex = 4 Then
        query = "SELECT Distinct [Specialty] FROM [keys].[MasterTaxonomy] ORDER BY Specialty"

        'End If


        Using connection As New SqlConnection(connectionString)
            Using command As New SqlCommand(query, connection)
                Try
                    connection.Open()

                    Using reader As SqlDataReader = command.ExecuteReader()
                        cboTaxonomy.Items.Clear()

                        ' Add a default item
                        cboTaxonomy.Items.Add("-- Select Taxonomy --")

                        Dim count As Integer = 0
                        While reader.Read()
                            ' FIX: Use column name without brackets in reader, or use ordinal position
                            ' Method 1: Use column name without brackets
                            If Not reader.IsDBNull(0) Then
                                Dim taxonomyValue As String
                                'If cboSearchType.SelectedIndex = 4 Then
                                taxonomyValue = reader("Specialty").ToString()
                                'Else
                                'taxonomyValue = reader("Provider Taxonomy Description Type").ToString()
                                'End If
                                If Not String.IsNullOrWhiteSpace(taxonomyValue) Then
                                    cboTaxonomy.Items.Add(taxonomyValue)
                                    count += 1
                                End If
                            End If
                            If count > 400 Then Exit While
                        End While

                        ' Set default selection
                        If cboTaxonomy.Items.Count > 0 Then
                            cboTaxonomy.SelectedIndex = 0
                        End If

                        ' Show success message
                        If count > 0 Then
                            'MessageBox.Show($"Successfully loaded {count} taxonomy codes!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("No taxonomy codes found in the database.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using

                Catch ex As SqlException
                    MessageBox.Show("SQL Error loading taxonomy: " & ex.Message & vbCrLf &
                               "Error Number: " & ex.Number.ToString() & vbCrLf &
                               "Server: " & ex.Server,
                               "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    MessageBox.Show("Error loading taxonomy: " & ex.Message & vbCrLf &
                               "Type: " & ex.GetType().Name,
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Handles general provider searches by demographics/education
    ''' </summary>

#End Region

#Region "Helper Methods"

    ''' <summary>
    ''' Extracts search parameters from form controls
    ''' </summary>

    Private Function GetSearchParameters() As SearchParameters
        Return New SearchParameters With {
            .NPI = GetTrimmedText(tbNpi),
            .HCPCS = GetTrimmedText(tbHCPCS),
            .FirstName = GetTrimmedText(tbFirst),
            .MiddleName = GetTrimmedText(tbMiddle),
            .LastName = GetTrimmedText(tbLast),
            .City = GetTrimmedText(tbCity),
            .State = GetTrimmedText(tbState).ToUpper(),
            .ZipCode = GetTrimmedText(tbZip),
            .Gender = GetTrimmedText(rdoMale),
            .LicenseState = GetTrimmedText(tbStLic).ToUpper(),
            .LicenseNumber = GetTrimmedText(tbStLicNum),
            .Specialty = GetTrimmedText(cboTaxonomy, True),
            .GradYear = GetTrimmedText(tbGradYear),
            .MedSchool = GetTrimmedText(tbMedSchool),
            .BrandDrug = GetTrimmedText(tbDrug),
            .GenericDrug = GetTrimmedText(tbDrugGeneric),
            .Taxonomy = GetTrimmedText(cboTaxonomy, True)
        }
    End Function

    Private Function GetControlText(ctrl As Control) As String
        Return If(ctrl?.Text?.Trim(), "")
    End Function

    Private Function GetGender() As String
        If rdoMale.Checked Then Return "M"
        If rdoFemale.Checked Then Return "F"
        Return ""
    End Function

    Private Function GetTaxonomy() As String
        Dim text = GetControlText(cboTaxonomy)
        If text = "Cardiology" Then Return "CARDIOVASCULAR DISEASE (CARDIOLOGY)"
        Return text
    End Function
    ''' Safely gets trimmed text from a control
    ''' </summary>
    Private Function GetTrimmedText(control As Control, Optional isTaxonomy As Boolean = False) As String
        Select Case True
            Case TypeOf control Is RadioButton
                If rdoMale.Checked Then Return "M"
                If rdoFemale.Checked Then Return "F"
        End Select
        If isTaxonomy Then
            Select Case control.Text
                Case "Cardiology"
                    Return "CARDIOVASCULAR DISEASE (CARDIOLOGY)"
            End Select
        End If
        If control Is Nothing Then Return ""
        Return control.Text.Trim()
    End Function

    ''' Validates search parameters

    Private Function ValidateSearchParameters(params As SearchParameters) As ValidationResult
        Dim errors As New List(Of String)

        ' Validate graduation year
        If Not String.IsNullOrWhiteSpace(params.GradYear) Then
            Dim year As Integer
            If Not Integer.TryParse(params.GradYear, year) OrElse year < 1900 OrElse year > Date.Now.Year + 10 Then
                errors.Add("Please enter a valid graduation year (1900-" & (Date.Now.Year + 10).ToString() & ")")
            End If
        End If

        ' Validate ZIP code
        If Not String.IsNullOrWhiteSpace(params.ZipCode) Then
            If Not System.Text.RegularExpressions.Regex.IsMatch(params.ZipCode, "^\d{5}(-\d{4})?$") Then
                errors.Add("Please enter a valid ZIP code (e.g., 12345 or 12345-6789)")
            End If
        End If

        ' Check for state-only searches (not allowed)
        If IsStateOnlySearch(params) Then
            errors.Add("Please enter additional search criteria (such as name, city, or ZIP code) when searching by state alone.")
        End If

        Return New ValidationResult(errors)
    End Function

    ''' <summary>
    ''' Checks if this is a problematic state-only search
    ''' </summary>
    Private Function IsStateOnlySearch(params As SearchParameters) As Boolean
        Dim hasOnlyState = Not String.IsNullOrWhiteSpace(params.State) AndAlso
            String.IsNullOrWhiteSpace(params.FirstName) AndAlso
            String.IsNullOrWhiteSpace(params.LastName) AndAlso
            String.IsNullOrWhiteSpace(params.City) AndAlso
            String.IsNullOrWhiteSpace(params.ZipCode) AndAlso
            String.IsNullOrWhiteSpace(params.MiddleName) AndAlso
            String.IsNullOrWhiteSpace(params.Gender) AndAlso
            String.IsNullOrWhiteSpace(params.LicenseState)

        Dim hasOnlyLicenseState = Not String.IsNullOrWhiteSpace(params.LicenseState) AndAlso
            String.IsNullOrWhiteSpace(params.FirstName) AndAlso
            String.IsNullOrWhiteSpace(params.LastName) AndAlso
            String.IsNullOrWhiteSpace(params.City) AndAlso
            String.IsNullOrWhiteSpace(params.ZipCode) AndAlso
            String.IsNullOrWhiteSpace(params.MiddleName) AndAlso
            String.IsNullOrWhiteSpace(params.Gender) AndAlso
            String.IsNullOrWhiteSpace(params.State)

        Return hasOnlyState OrElse hasOnlyLicenseState
    End Function

    ''' <summary>
    ''' Determines whether to use education search vs demographics search
    ''' </summary>
    Private Function ShouldUseEducationSearch(params As SearchParameters) As Boolean
        Return Not String.IsNullOrWhiteSpace(params.GradYear) OrElse
               Not String.IsNullOrWhiteSpace(params.MedSchool)
    End Function

    ''' <summary>
    ''' Shows search results in the results form
    ''' </summary>
    Private Sub ShowResults(results As JArray, showDrugColumns As Boolean)
        Try
            Dim searchSummary = BuildSearchSummary()
            Dim resultsForm As New IndividualResultsForm(results, showDrugColumns, searchSummary)
            resultsForm.Show()
        Catch ex As Exception
            ShowMessage($"Could not display search results: {ex.Message}", "Display Error", MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Builds a user-friendly summary of what was searched
    ''' </summary>
    Private Function BuildSearchSummary(Optional isDrugs As Boolean = False) As String
        Dim filters As New List(Of String)

        AddToSummaryIfNotEmpty(filters, "NPI", tbNpi)
        AddToSummaryIfNotEmpty(filters, "HCPCS", tbHCPCS)
        AddToSummaryIfNotEmpty(filters, "First Name", tbFirst)
        AddToSummaryIfNotEmpty(filters, "Middle Name", tbMiddle)
        AddToSummaryIfNotEmpty(filters, "Last Name", tbLast)
        AddToSummaryIfNotEmpty(filters, "State", tbState)
        AddToSummaryIfNotEmpty(filters, "City", tbCity)
        AddToSummaryIfNotEmpty(filters, "ZIP", tbZip)
        AddToSummaryIfNotEmpty(filters, "Gender", rdoMale)
        AddToSummaryIfNotEmpty(filters, "License State", tbStLic)
        AddToSummaryIfNotEmpty(filters, "License Number", tbStLicNum)
        AddToSummaryIfNotEmpty(filters, "Specialty", tbFacilityTyp)
        AddToSummaryIfNotEmpty(filters, "Graduation Year", tbGradYear)
        AddToSummaryIfNotEmpty(filters, "Medical School", tbMedSchool)
        AddToSummaryIfNotEmpty(filters, "Brand Drug", tbDrug)
        AddToSummaryIfNotEmpty(filters, "Generic Drug", tbDrugGeneric)
        AddToSummaryIfNotEmpty(filters, "Taxonomy", cboTaxonomy)

        Return "Search Filters: " & String.Join(" | ", filters)
    End Function

    ''' <summary>
    ''' Helper to add non-empty values to search summary
    ''' </summary>
    Private Sub AddToSummaryIfNotEmpty(filters As List(Of String), label As String, control As Control)
        If control Is Nothing Then Return
        Dim value As String = ""
        ' Handle RadioButton specially
        If TypeOf control Is RadioButton Then
            value = GetTrimmedText(control)
        Else
            value = control.Text.Trim()
        End If
        If Not String.IsNullOrWhiteSpace(value) Then
            filters.Add($"{label}: {value}")
        End If
    End Sub

    ''' <summary>
    ''' Shows a consistent message to the user
    ''' </summary>
    Private Sub ShowMessage(message As String, title As String, icon As MessageBoxIcon)
        MessageBox.Show(message, title, MessageBoxButtons.OK, icon)
    End Sub

    ''' <summary>
    ''' Clears all search input fields
    ''' </summary>
    Private Sub ClearAllFields()
        Try
            ' Clear all text boxes
            Dim textBoxesToClear() As System.Windows.Forms.TextBox = {
                tbFirst, tbLast, tbNpi, tbState, tbMiddle, tbAddress,
                tbCity, tbZip, tbAddressType, tbHCPCS, tbStLic,
                tbStLicNum, tbProvEnroll, tbFacilityTyp, tbGradYear,
                tbMedSchool, tbDrug, tbDrugGeneric
            }

            For Each textBox In textBoxesToClear
                If textBox IsNot Nothing Then
                    textBox.Clear()
                End If
            Next

            ' Reset combo box
            If cboTaxonomy IsNot Nothing Then
                cboTaxonomy.SelectedIndex = -1
            End If
            rdoMale.Checked = False
            rdoFemale.Checked = False

        Catch ex As Exception
            ShowMessage($"Error clearing fields: {ex.Message}", "Clear Error", MessageBoxIcon.Warning)
        End Try
    End Sub

#End Region

#Region "UI State Management"

    ''' <summary>
    ''' Controls the form state during search operations
    ''' </summary>
    Private Sub SetSearchInProgress(inProgress As Boolean)
        _isSearchInProgress = inProgress

        ' Update search button
        btnSearch.Enabled = Not inProgress
        btnSearch.Text = If(inProgress, "Searching...", "Search")

        ' Update other buttons
        Button1.Enabled = Not inProgress ' Clear button
        btnOrgSearch.Enabled = Not inProgress ' Organization search button

        ' Update cursor to show activity
        Me.Cursor = If(inProgress, Cursors.WaitCursor, Cursors.Default)

        ' Force UI update
        Application.DoEvents()
    End Sub

#End Region

#Region "Form Lifecycle"

    ''' <summary>
    ''' Handles form closing - cleanup ongoing operations
    ''' </summary>
    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        Try
            _cancellationTokenSource?.Cancel()
            _cancellationTokenSource?.Dispose()
        Catch
            ' Ignore cleanup errors
        Finally
            MyBase.OnFormClosing(e)
        End Try
    End Sub


    Private Sub cboSearchType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSearchType.SelectedIndexChanged
        If cboSearchType.SelectedIndex = 0 Then
            cboSearchType.ForeColor = Color.Gray
        Else
            cboSearchType.ForeColor = Color.Black
            LoadTaxonomyCombo()
        End If
        Select Case cboSearchType.SelectedItem.ToString
            Case "NPI Registry"
                tbNpi.Visible = True
                cboTaxonomy.Visible = True
                tbFirst.Visible = True
                tbMiddle.Visible = True
                tbLast.Visible = True
                tbState.Visible = True
                lblTax.Visible = True
                lblFirst.Visible = True
                lblMiddle.Visible = True
                lblLast.Visible = True
                lblState.Visible = True
                ShowGender(False)


                groupPersonal.Visible = True
                groupAddress.Visible = True
                groupMedSchool.Visible = True
                groupBoxOther.Visible = True
                groupMeds.Visible = False

            Case "NPI Downloadable Update"
                tbNpi.Visible = True
                cboTaxonomy.Visible = False
                tbFirst.Visible = False
                tbMiddle.Visible = False
                tbLast.Visible = False
                tbState.Visible = False
                rdoFemale.Visible = False
                rdoMale.Visible = False
                ckExact.Visible = False
                lblTax.Visible = False
                lblFirst.Visible = False
                lblMiddle.Visible = False
                lblLast.Visible = False
                lblState.Visible = False
                lblGender.Visible = False


                groupPersonal.Visible = True
                groupAddress.Visible = False
                groupMedSchool.Visible = False
                groupBoxOther.Visible = False
                groupMeds.Visible = False

            Case "Healthcare Common Procedure Code"
                groupPersonal.Visible = False
                groupAddress.Visible = False
                groupMedSchool.Visible = False
                groupBoxOther.Visible = True
                groupMeds.Visible = False
            Case "Drugs Prescribed"
                ShowGender(False)
                tbNpi.Visible = True
                tbFirst.Visible = True
                tbLast.Visible = True
                tbState.Visible = True
                tbMiddle.Visible = False
                lblMiddle.Visible = False

                groupPersonal.Visible = True
                groupAddress.Visible = False
                groupMedSchool.Visible = False
                groupBoxOther.Visible = False
                groupMeds.Visible = True


            Case "Medical School"
                tbNpi.Visible = False
                cboTaxonomy.Visible = False
                tbFirst.Visible = True
                tbMiddle.Visible = False
                tbLast.Visible = False
                tbState.Visible = False


                groupPersonal.Visible = False
                groupAddress.Visible = False
                groupMedSchool.Visible = True
                groupBoxOther.Visible = False
                groupMeds.Visible = False

        End Select
    End Sub

    Public Sub ShowGender(TurnOn As Boolean)
        If TurnOn Then
            rdoMale.Visible = True
            rdoFemale.Visible = True
            lblGender.Visible = True
        Else
            rdoMale.Visible = False
            rdoFemale.Visible = False
            lblGender.Visible = False
        End If
    End Sub

    Private Sub tbState_TextChanged(sender As Object, e As EventArgs) Handles tbState.TextChanged
        If tbAddressState.Text <> tbState.Text Then
            tbAddressState.Text = tbState.Text
        End If
    End Sub

    Private Sub tbAddressState_TextChanged(sender As Object, e As EventArgs) Handles tbAddressState.TextChanged
        If tbState.Text <> tbAddressState.Text Then
            tbState.Text = tbAddressState.Text
        End If
    End Sub

    Private Sub cboTaxonomy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTaxonomy.SelectedIndexChanged
        'LoadTaxonomyCombo()
    End Sub


#End Region

End Class

#Region "Support Classes"

''' <summary>
''' Encapsulates all search parameters from the form
''' </summary>
Public Class SearchParameters
    Public Property NPI As String = ""
    Public Property HCPCS As String = ""
    Public Property FirstName As String = ""
    Public Property MiddleName As String = ""
    Public Property LastName As String = ""
    Public Property City As String = ""
    Public Property State As String = ""
    Public Property ZipCode As String = ""
    Public Property Gender As String = ""
    Public Property LicenseState As String = ""
    Public Property LicenseNumber As String = ""
    Public Property Specialty As String = ""
    Public Property GradYear As String = ""
    Public Property MedSchool As String = ""
    Public Property BrandDrug As String = ""
    Public Property GenericDrug As String = ""
    Public Property Taxonomy As String = ""
End Class

''' <summary>
''' Represents the result of validation operations
''' </summary>
Public Class ValidationResult
    Public Property IsValid As Boolean
    Public Property ErrorMessage As String

    Public Sub New(errors As List(Of String))
        If errors Is Nothing OrElse errors.Count = 0 Then
            IsValid = True
            ErrorMessage = ""
        Else
            IsValid = False
            ErrorMessage = String.Join(Environment.NewLine, errors)
        End If
    End Sub
End Class

#End Region

#Region "Usage Documentation"

' HOW TO USE THIS SEARCH FORM:
' ============================
'
' Search Types (in priority order):
' 
' 1. NPI Search (Direct Profile Access):
'    - Enter just the NPI number: "1234567890"
'    - Form opens provider profile immediately
'    - Fastest and most direct method
'
' 2. HCPCS Procedure Search:
'    - Enter procedure code: "99213"
'    - Optionally add state: "99213" + "CA"
'    - Finds providers who perform that procedure
'
' 3. Medication Search:
'    - Enter brand name: "Lipitor"
'    - OR enter generic name: "atorvastatin"
'    - Finds providers who prescribe that medication
'
' 4. General Provider Search:
'    - Use any combination of demographics/education fields
'    - Examples:
'      * Name + State: "Smith" + "CA"
'      * Full name + City: "John" + "Smith" + "Los Angeles"
'      * Education: "Harvard Medical School" + "2010"
'
' VALIDATION RULES:
' =================
' - State-only searches are not allowed (too broad)
' - ZIP codes must be 5 digits or 9 digits (12345 or 12345-6789)
' - Graduation years must be between 1900 and current year + 10
' - Form prevents multiple simultaneous searches
'
' SEARCH TIPS:
' ============
' - More specific searches return faster, more accurate results
' - The form automatically chooses the best data source
' - Education searches use National Downloadable File
' - General searches use NPI Registry
' - All operations are cancellable if form is closed

#End Region
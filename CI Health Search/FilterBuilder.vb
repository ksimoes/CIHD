Imports System.Collections.Generic
Imports Newtonsoft.Json.Linq

''' <summary>
''' Provides a unified, reusable filter building system for healthcare provider searches.
''' Eliminates code duplication and provides consistent filter handling across the application.
''' </summary>
Public Module FilterBuilder

#Region "Core Filter Building"

    ''' <summary>
    ''' Universal filter builder - creates filters for any search type with flexible configuration.
    ''' </summary>
    ''' <param name="params">Search parameters object</param>
    ''' <param name="config">Filter configuration defining mappings and behavior</param>
    ''' <returns>Dictionary of filters ready for API calls</returns>
    Public Function BuildFilters(params As SearchParameters, config As FilterConfig) As Dictionary(Of String, String)
        Dim filters As New Dictionary(Of String, String)

        ' Add base parameters (like size limits)
        If config.BaseParams IsNot Nothing Then
            For Each kvp In config.BaseParams
                filters(kvp.Key) = kvp.Value
            Next
        End If

        ' Process each field mapping
        For Each mapping In config.FieldMappings
            Dim value As String = GetParameterValue(params, mapping.SourceProperty)

            If Not String.IsNullOrWhiteSpace(value) Then
                ' Apply wildcard if configured
                If mapping.AppendWildcard Then value &= "*"

                ' Apply filter prefix if configured
                Dim key As String = If(String.IsNullOrEmpty(mapping.FilterPrefix),
                           mapping.TargetKey,
                           $"{mapping.FilterPrefix}[{mapping.TargetKey}]")

                filters(key) = value
            End If
        Next

        Return filters
    End Function

    ''' <summary>
    ''' Builds CMS API conditions from search parameters using reflection.
    ''' </summary>
    Public Function BuildCmsConditions(params As SearchParameters, config As CmsFilterConfig) As List(Of JObject)
        Dim conditions As New List(Of JObject)

        For Each mapping In config.FieldMappings
            Dim value As String = GetParameterValue(params, mapping.SourceProperty)

            If Not String.IsNullOrWhiteSpace(value) Then
                Dim operatorType As String = If(mapping.ExactMatch, "=", "LIKE")

                If operatorType = "LIKE" Then
                    value = value.Replace("*", "%")
                End If

                conditions.Add(New JObject(
                    New JProperty("resource", "t"),
                    New JProperty("property", mapping.TargetKey),
                    New JProperty("value", value),
                    New JProperty("operator", operatorType)
                ))
            End If
        Next

        Return conditions
    End Function

    ''' <summary>
    ''' Builds human-readable search summary from parameters.
    ''' </summary>
    Public Function BuildSearchSummary(params As SearchParameters, Optional separator As String = " | ") As String
        Dim filters As New List(Of String)
        Dim props = GetType(SearchParameters).GetProperties()

        For Each prop In props
            Dim value = TryCast(prop.GetValue(params), String)
            If Not String.IsNullOrWhiteSpace(value) Then
                Dim displayName = FormatPropertyName(prop.Name)
                filters.Add($"{displayName}: {value}")
            End If
        Next

        If filters.Count = 0 Then Return "No filters applied"
        Return "Search Filters: " & String.Join(separator, filters)
    End Function

    ''' <summary>
    ''' Gets a parameter value using reflection.
    ''' </summary>
    Private Function GetParameterValue(params As SearchParameters, propertyName As String) As String
        Try
            Dim prop = GetType(SearchParameters).GetProperty(propertyName)
            If prop Is Nothing Then Return ""

            Dim value = prop.GetValue(params)
            Return If(value?.ToString(), "")
        Catch
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Formats property names for display (e.g., "FirstName" -> "First Name").
    ''' </summary>
    Private Function FormatPropertyName(propName As String) As String
        Return System.Text.RegularExpressions.Regex.Replace(propName, "([A-Z])", " $1").Trim()
    End Function

#End Region

#Region "Pre-configured Filter Builders"

    ''' <summary>
    ''' Gets filter configuration for HCPCS searches.
    ''' </summary>
    Public Function GetHcpcsConfig() As FilterConfig
        Return New FilterConfig With {
            .BaseParams = New Dictionary(Of String, String) From {{"size", "1000"}},
            .FieldMappings = New List(Of FieldMapping) From {
                New FieldMapping("HCPCS", "HCPCS_Cd", "filter"),
                New FieldMapping("FirstName", "Rndrng_Prvdr_First_Name", "filter", True),
                New FieldMapping("LastName", "Rndrng_Prvdr_Last_Org_Name", "filter", True),
                New FieldMapping("State", "Rndrng_Prvdr_State_Abrvtn", "filter"),
                New FieldMapping("NPI", "Rndrng_Prvdr_NPI", "filter"),
                New FieldMapping("City", "Rndrng_Prvdr_City", "filter"),
                New FieldMapping("ZipCode", "Rndrng_Prvdr_Zip5", "filter")
            }
        }
    End Function

    ''' <summary>
    ''' Gets filter configuration for drug searches.
    ''' </summary>
    Public Function GetDrugConfig() As FilterConfig
        Return New FilterConfig With {
            .BaseParams = New Dictionary(Of String, String) From {{"size", "1000"}},
            .FieldMappings = New List(Of FieldMapping) From {
                New FieldMapping("NPI", "Prscrbr_NPI", "filter"),
                New FieldMapping("BrandDrug", "Brnd_Name", "filter"),
                New FieldMapping("GenericDrug", "Gnrc_Name", "filter"),
                New FieldMapping("State", "Prscrbr_State_Abrvtn", "filter"),
                New FieldMapping("FirstName", "Prscrbr_First_Name", "filter", True),
                New FieldMapping("LastName", "Prscrbr_Last_Org_Name", "filter", True),
                New FieldMapping("Taxonomy", "Prscrbr_Type", "filter"),
                New FieldMapping("City", "Prscrbr_City", "filter"),
                New FieldMapping("ZipCode", "Prscrbr_Zip", "filter")
            }
        }
    End Function

    ''' <summary>
    ''' Gets filter configuration for NPI Registry searches.
    ''' </summary>
    Public Function GetNpiRegistryConfig() As FilterConfig
        Return New FilterConfig With {
            .FieldMappings = New List(Of FieldMapping) From {
                New FieldMapping("NPI", "number"),
                New FieldMapping("FirstName", "first_name", "", True),
                New FieldMapping("MiddleName", "middle_name", "", True),
                New FieldMapping("LastName", "last_name", "", True),
                New FieldMapping("City", "city"),
                New FieldMapping("State", "state"),
                New FieldMapping("ZipCode", "postal_code"),
                New FieldMapping("LicenseState", "license_state"),
                New FieldMapping("LicenseNumber", "license_number"),
                New FieldMapping("Taxonomy", "taxonomy_description"),
                New FieldMapping("GradYear", "graduation_year"),
                New FieldMapping("MedSchool", "medical_school")
            }
        }
    End Function

    ''' <summary>
    ''' Gets CMS filter configuration for National Downloadable File searches.
    ''' </summary>
    Public Function GetNationalDownloadableConfig(isExact As Boolean) As CmsFilterConfig
        Return New CmsFilterConfig With {
            .FieldMappings = New List(Of CmsFieldMapping) From {
                New CmsFieldMapping("NPI", "npi", True),
                New CmsFieldMapping("FirstName", "provider_first_name", isExact),
                New CmsFieldMapping("MiddleName", "provider_middle_name", isExact),
                New CmsFieldMapping("LastName", "provider_last_name", isExact),
                New CmsFieldMapping("Gender", "gndr", True),
                New CmsFieldMapping("GradYear", "grd_yr", True),
                New CmsFieldMapping("MedSchool", "med_sch", isExact),
                New CmsFieldMapping("State", "state", True)
            }
        }
    End Function

#End Region

#Region "Configuration Classes"

    ''' <summary>
    ''' Configuration for building URL query parameter filters.
    ''' </summary>
    Public Class FilterConfig
        Public Property BaseParams As Dictionary(Of String, String)
        Public Property FieldMappings As List(Of FieldMapping)
    End Class

    ''' <summary>
    ''' Maps a source property to a target filter key.
    ''' </summary>
    Public Class FieldMapping
        Public Property SourceProperty As String
        Public Property TargetKey As String
        Public Property FilterPrefix As String
        Public Property AppendWildcard As Boolean

        Public Sub New(source As String, target As String, Optional prefix As String = "", Optional wildcard As Boolean = False)
            SourceProperty = source
            TargetKey = target
            FilterPrefix = prefix
            AppendWildcard = wildcard
        End Sub
    End Class

    ''' <summary>
    ''' Configuration for building CMS API conditions.
    ''' </summary>
    Public Class CmsFilterConfig
        Public Property FieldMappings As List(Of CmsFieldMapping)
    End Class

    ''' <summary>
    ''' Maps a source property to a CMS API condition.
    ''' </summary>
    Public Class CmsFieldMapping
        Public Property SourceProperty As String
        Public Property TargetKey As String
        Public Property ExactMatch As Boolean

        Public Sub New(source As String, target As String, exact As Boolean)
            SourceProperty = source
            TargetKey = target
            ExactMatch = exact
        End Sub
    End Class

#End Region

End Module
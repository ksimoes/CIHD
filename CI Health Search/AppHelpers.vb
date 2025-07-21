Imports Newtonsoft.Json.Linq
Imports System.Data
Imports System.Data.SqlClient

Public Module AppHelpers
    ' Safe get for DataRow
    Public Function SafeGet(row As DataRow, columnName As String) As String
        Try
            If row.Table.Columns.Contains(columnName) AndAlso row(columnName) IsNot Nothing Then
                Return row(columnName).ToString()
            End If
        Catch
        End Try
        Return "N/A"
    End Function

    ' Safe get for JObject (single key)
    Public Function SafeGet(obj As JObject, key As String) As String
        If obj(key) IsNot Nothing AndAlso Not String.IsNullOrEmpty(obj(key).ToString()) Then
            Return obj(key).ToString()
        End If
        Return "N/A"
    End Function

    ' Safe get for JObject (multiple fallback keys)
    Public Function SafeGet(obj As JObject, ParamArray keys() As String) As String
        For Each key In keys
            If obj(key) IsNot Nothing AndAlso Not String.IsNullOrEmpty(obj(key).ToString()) Then
                Return obj(key).ToString()
            End If
        Next
        Return "N/A"
    End Function

    ' Safe get for SqlDataReader
    Public Function SafeGet(reader As SqlDataReader, columnName As String) As String
        Try
            Dim ordinal = reader.GetOrdinal(columnName)
            If Not reader.IsDBNull(ordinal) Then
                Return reader.GetValue(ordinal).ToString()
            End If
        Catch
        End Try
        Return "N/A"
    End Function

    ' Safe get for date fields
    Public Function SafeGetDate(obj As JObject, key As String) As String
        Try
            If obj(key) IsNot Nothing AndAlso Not String.IsNullOrEmpty(obj(key).ToString()) Then
                Return CDate(obj(key)).ToString("MM/dd/yyyy")
            End If
        Catch
        End Try
        Return "N/A"
    End Function

    ' Format currency
    Public Function FormatCurrency(val As String) As String
        Dim dec As Decimal
        If Decimal.TryParse(val.Replace("$", "").Replace(",", ""), dec) Then
            Return dec.ToString("N0")
        End If
        Return "N/A"
    End Function
End Module
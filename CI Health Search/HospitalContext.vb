Imports Newtonsoft.Json.Linq
Imports System.Net.Http

Public Class HospitalContext
    Public Property HospitalId As String
    Public Property State As String
    Public Property NPI As String
    Public Property CMSNum As String
    Public Property Name As String

    Public Property Address As String

    Public Property City As String
    Public Property Zip As String
    Public Property Phone As String
    'Public Property CEO As String
    'Public Property CountyFIPS As String
    Public Property website As String

    Public Property CBSAnum As String
    Public Property NumOfEmployees As String
    Public Property NumOfBeds As Integer
    Public Property TotalDays As Integer
    Public Property TotalDischarges As Integer
    Public Property FacilityType As String

    Public Property TotalPatientRev As String
    Public Property NetPatientRev As String
    Public Property RuralOUrban As String
    Public Property charityCost As String
    Public Property uncompensatedCost As String





    'Public Property TotalPatientDays As String
    'Public Property TotalOperatingRevenue As String
    'Public Property TotalOperatingExpense As String



    'Public Overrides Function ToString() As String
    '    Return Name
    'End Function


End Class
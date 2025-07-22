Imports Newtonsoft.Json.Linq
Imports System.Net.Http

Public Class HospitalContext
    ' Core Identifiers
    Public Property HospitalId As Integer
    Public Property State As String
    Public Property NPI As String
    Public Property CMSNum As String
    Public Property Name As String

    ' Location & Contact
    Public Property Address As String
    Public Property City As String
    Public Property Zip As String
    Public Property County As String
    Public Property Phone As String
    Public Property Website As String

    ' Classification
    Public Property CBSAnum As String
    Public Property FacilityType As String
    Public Property RuralOUrban As String

    ' Capacity & Staffing
    Public Property NumOfBeds As Integer
    Public Property NumOfEmployees As Integer
    Public Property TotalDays As Integer
    Public Property TotalDischarges As Integer

    ' Financials
    Public Property TotalPatientRev As Decimal
    Public Property NetPatientRev As Decimal
    Public Property CharityCost As Decimal
    Public Property UncompensatedCost As Decimal

    ' Additional/Expandable fields
    Public Property TotalCurrentAssets As Decimal
    Public Property TotalAssets As Decimal
    Public Property NetIncome As Decimal
    Public Property TotalOperatingRevenue As Decimal
    Public Property TotalOperatingExpense As Decimal
    Public Property TotalLiabilities As Decimal
    Public Property TotalCurrentLiabilities As Decimal
    Public Property TotalLongTermLiabilities As Decimal
    Public Property DepreciationCost As Decimal
    Public Property LeaseCost As Decimal
    Public Property Inventory As Decimal
    Public Property NotesReceivable As Decimal
    Public Property MarketSecurities As Decimal
    Public Property Investments As Decimal



    ' Add more fields as needed for new APIs or features

    ' Optionally, override ToString for debugging
    Public Overrides Function ToString() As String
        Return $"{Name} ({CMSNum}) - {City}, {State}"
    End Function
End Class
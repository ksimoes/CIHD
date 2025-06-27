Public Class HospitalContext
    Public Property HospitalId As String
    Public Property State As String
    Public Property NPI As String
    Public Property CMSNum As String
    Public Property Name As String

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
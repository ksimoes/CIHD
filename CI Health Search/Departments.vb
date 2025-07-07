Imports System.Security.Cryptography.X509Certificates
Imports Newtonsoft.Json.Linq

Public Class Departments
    Public Sub ShowDepartmentsDataApi(cmsNum As String)
        ' This method can be used to show departments data if needed
        ' Currently, it does not perform any actions
        Dim newURL As String = "https://data.cms.gov/provider-characteristics/hospitals-and-other-facilities/provider-of-services-file-hospital-non-hospital-facilities/data"
        Dim filters As New List(Of String)
        If Not String.IsNullOrEmpty(cmsNum) Then filters.Add("keyword=" & Uri.EscapeDataString(cmsNum))
        'If Not String.IsNullOrEmpty(npi) Then filters.Add("keyword=" & Uri.EscapeDataString(npi))
        newURL &= String.Join("&", filters)
        filters.Add("size=1000")

        Dim myArray As JArray = Profile.GetAPIArray(newURL)


        If myArray.Count > 0 Then
            ' Find the exact match for Provider CCN if possible
            Dim provider = myArray.FirstOrDefault(Function(x) x("PRVDR_NUM") IsNot Nothing AndAlso x("PRVDR_NUM").ToString() = cmsNum)
            If provider Is Nothing Then provider = myArray(0)
        End If
    End Sub

    Private Sub Departments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'ShowDepartmentsDataApi() 





    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileDepartments.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialDepartments.Click
        Me.Hide()
        Financial.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndDepartments.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityDepartments.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientDepartments.Click
        Me.Hide()
        Inpatient.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientDepartments.Click
        Me.Hide()
        Outpatient.Show()

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
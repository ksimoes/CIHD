Imports System.Windows.Forms

Public Class NavigationPopupForm
    Private hospitalContext As HospitalContext

    ' Constructor for passing context
    Public Sub New(ctx As HospitalContext)
        InitializeComponent()
        hospitalContext = ctx
    End Sub

    ' Default constructor for designer compatibility
    Public Sub New()
        InitializeComponent()
    End Sub

    ' Profile button
    Private Sub btnProfile_Click(sender As Object, e As EventArgs) Handles btnProfile.Click
        If hospitalContext Is Nothing Then
            MessageBox.Show("No hospital selected.")
            Return
        End If
        Dim profileForm As New Profile(hospitalContext)
        profileForm.Show()
    End Sub

    ' Financial button
    Private Sub btnFinancial_Click(sender As Object, e As EventArgs) Handles btnFinancial.Click
        If hospitalContext Is Nothing Then
            MessageBox.Show("No hospital selected.")
            Return
        End If
        Dim financialForm As New Financial(hospitalContext)
        financialForm.Show()
    End Sub

    ' Quality button
    Private Sub btnQuality_Click(sender As Object, e As EventArgs) Handles btnQuality.Click
        If hospitalContext Is Nothing Then
            MessageBox.Show("No hospital selected.")
            Return
        End If
        Dim qualityForm As New Quality(hospitalContext)
        qualityForm.Show()
    End Sub

    ' Inpatient button
    Private Sub btnInpatient_Click(sender As Object, e As EventArgs) Handles btnInpatient.Click
        If hospitalContext Is Nothing Then
            MessageBox.Show("No hospital selected.")
            Return
        End If
        Dim inpatientForm As New Inpatient(hospitalContext)
        inpatientForm.Show()
    End Sub

    ' Outpatient button
    Private Sub btnOutpatient_Click(sender As Object, e As EventArgs) Handles btnOutpatient.Click
        If hospitalContext Is Nothing Then
            MessageBox.Show("No hospital selected.")
            Return
        End If
        Dim outpatientForm As New Outpatient(hospitalContext)
        outpatientForm.Show()
    End Sub

    ' Departments button
    Private Sub btnDepartments_Click(sender As Object, e As EventArgs) Handles btnDepartments.Click
        If hospitalContext Is Nothing Then
            MessageBox.Show("No hospital selected.")
            Return
        End If
        Dim departmentsForm As New Departments(hospitalContext)
        departmentsForm.Show()
    End Sub

    ' Financial Indicators button
    Private Sub btnFinInd_Click(sender As Object, e As EventArgs) Handles btnFinInd.Click
        If hospitalContext Is Nothing Then
            MessageBox.Show("No hospital selected.")
            Return
        End If
        Dim finIndForm As New FinInd(hospitalContext)
        finIndForm.Show()
    End Sub

    '' Optional: Close button
    'Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
    '    Me.Close()
    'End Sub
End Class
Public Class Quality

    Private currentHospital As HospitalContext

    Private Function GetBestFacilityName(ctx As HospitalContext) As String
        If ctx Is Nothing Then Return "N/A"
        If ctx.LastDataRow IsNot Nothing Then
            Dim row = ctx.LastDataRow
            If row.Table.Columns.Contains("FAC_NAME") Then Return row("FAC_NAME").ToString()
            If row.Table.Columns.Contains("PRVDR_NAME") Then Return row("PRVDR_NAME").ToString()
            If row.Table.Columns.Contains("ORGANIZATION NAME") Then Return row("ORGANIZATION NAME").ToString()
            If row.Table.Columns.Contains("organization_name") Then Return row("organization_name").ToString()
            If row.Table.Columns.Contains("Facility Name") Then Return row("Facility Name").ToString()
            If row.Table.Columns.Contains("Hospital Name") Then Return row("Hospital Name").ToString()
        End If
        If Not String.IsNullOrWhiteSpace(ctx.Name) Then Return ctx.Name
        Return "N/A"
    End Function

    ' New constructor to accept a HospitalContext


    Public Sub New(hosp As HospitalContext)
        InitializeComponent()
        currentHospital = hosp
    End Sub

    ' Default constructor for designer compatibility
    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Quality_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If currentHospital IsNot Nothing Then
            lblHN.Text = GetBestFacilityName(currentHospital)
        Else
            lblHN.Text = "No hospital context"
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileQuality.Click
        Me.Hide()
        Dim profileForm As New Profile(currentHospital)
        profileForm.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentsQuality.Click
        Me.Hide()
        Dim departmentsForm As New Departments(currentHospital)
        departmentsForm.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialQuality.Click
        Me.Hide()
        Dim financialForm As New Financial(currentHospital)
        financialForm.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndQuality.Click
        Me.Hide()
        Dim finIndForm As New FinInd(currentHospital)
        finIndForm.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientQuality.Click
        Me.Hide()
        Dim inpatientForm As New Inpatient(currentHospital)
        inpatientForm.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientQuality.Click
        Me.Hide()
        Dim outpatientForm As New Outpatient(currentHospital)
        outpatientForm.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
' Add these at the top of your Outpatient.vb
Imports System.Net.Http
Imports Newtonsoft.Json.Linq
Imports System.Data

Public Class Outpatient
    ' Replace with your actual API URL
    Dim strUrlApcApi As String = "https://data.cms.gov/data-api/v1/dataset/ccbc9a44-40d4-46b4-a709-5caa59212e50/data"

    Private currentHospital As HospitalContext
    Private dgvFilterHelper As DataGridViewFilterHelper
    Private filterPanelRef As Panel

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

    ' Async function to get a DataTable from the API
    Public Async Function GetTablefromAPI(strAPI As String) As Task(Of DataTable)
        Dim dt As New DataTable()
        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync(strAPI)
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)

                If data.Count > 0 Then
                    For Each prop In CType(data(0), JObject).Properties()
                        dt.Columns.Add(prop.Name)
                    Next
                    For Each item As JObject In data
                        Dim row As DataRow = dt.NewRow()
                        For Each prop In item.Properties()
                            row(prop.Name) = prop.Value.ToString()
                        Next
                        dt.Rows.Add(row)
                    Next
                End If
            End If
            Return dt
        End Using
    End Function

    ' Async function to load filtered API data into dgvAPC
    Public Async Function LoadApcDataAsync(myHospital As HospitalContext) As Task
        ' Replace "HospitalIdColumn" with the actual column name used for filtering
        Dim apiUrl As String = strUrlApcApi & "?filter[Rndrng_Prvdr_CCN]=" & myHospital.CMSNum
        Dim rawDt As DataTable = Await GetTablefromAPI(apiUrl)
        Dim dtCustom As New DataTable()

        ' Add your display columns here
        dtCustom.Columns.Add("APC Number")
        dtCustom.Columns.Add("Description")
        dtCustom.Columns.Add("Patient Claims")
        dtCustom.Columns.Add("Units of Service")
        dtCustom.Columns.Add("Avg Payment")
        ' ... add more as needed

        For Each row As DataRow In rawDt.Rows
            dtCustom.Rows.Add(
                row("APC_Cd").ToString(),
                row("APC_Desc").ToString(),
                row("Bene_Cnt").ToString(),
                row("CAPC_Srvcs").ToString(),
                row("Avg_Mdcr_Alowd_Amt"))

            ' ... add more as needed

        Next

        dgvAPC.DataSource = dtCustom
        If filterPanelRef IsNot Nothing AndAlso Me.Controls.Contains(filterPanelRef) Then
            Me.Controls.Remove(filterPanelRef)
            filterPanelRef.Dispose()
            filterPanelRef = Nothing
        End If
        dgvFilterHelper = New DataGridViewFilterHelper(dgvAPC, Me)
        filterPanelRef = dgvFilterHelper.FilterPanel


        FormatApcTable()
    End Function

    ' Optional: Format the DataGridView for readability
    Private Sub FormatApcTable()
        With dgvAPC
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
            .DefaultCellStyle.Font = New Font("Segoe UI", 10)
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .RowHeadersVisible = False
        End With
    End Sub

    ' Call this in your form load event
    Private Async Sub Outpatient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If currentHospital IsNot Nothing Then
            lblHN.Text = GetBestFacilityName(currentHospital)
            Await LoadApcDataAsync(currentHospital)
        Else
            lblHN.Text = "No hospital context"
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileOutpatient.Click
        Me.Hide()
        Dim cmsNum As String = currentHospital.CMSNum
        Dim fullContext As HospitalContext = Results.GetHospitalContextByCMSNum(cmsNum)
        If fullContext IsNot Nothing Then
            Dim profileForm As New Profile(fullContext)
            profileForm.Show()
        Else
            MessageBox.Show("Could not reload full hospital context.")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentsOutpatient.Click
        Me.Hide()
        Dim departmentsForm As New Departments(currentHospital)
        departmentsForm.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialOutpatient.Click
        Me.Hide()
        Dim financialForm As New Financial(currentHospital)
        financialForm.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndOutpatient.Click
        Me.Hide()
        Dim finIndForm As New FinInd(currentHospital)
        finIndForm.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityOutpatient.Click
        Me.Hide()
        Dim qualityForm As New Quality(currentHospital)
        qualityForm.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientOutpatient.Click
        Me.Hide()
        Dim inpatientForm As New Inpatient(currentHospital)
        inpatientForm.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub


End Class
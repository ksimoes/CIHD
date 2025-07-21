Imports Newtonsoft.Json.Linq
Imports System.Data

Public Class Outpatient
    ' Load APC data into the DataGridView
    Public Async Function LoadApcDataAsync(myHospital As HospitalContext) As Task
        lblstatus.Text = "Loading outpatient APC data..."
        lblstatus.Visible = True

        Try
            Dim apiUrl As String = ApiHelper.ApiUrls("ApcApi") & "?filter[Rndrng_Prvdr_CCN]=" & myHospital.CMSNum
            Dim rawDt As DataTable = Await ApiHelper.GetTableFromApiAsync(apiUrl)
            Dim dtCustom As New DataTable()

            dtCustom.Columns.Add("APC Number")
            dtCustom.Columns.Add("Description")
            dtCustom.Columns.Add("Patient Claims")
            dtCustom.Columns.Add("Units of Service")
            dtCustom.Columns.Add("Avg Payment")

            For Each row As DataRow In rawDt.Rows
                dtCustom.Rows.Add(
                    AppHelpers.SafeGet(row, "APC_Cd"),
                    AppHelpers.SafeGet(row, "APC_Desc"),
                    AppHelpers.SafeGet(row, "Bene_Cnt"),
                    AppHelpers.SafeGet(row, "CAPC_Srvcs"),
                    AppHelpers.SafeGet(row, "Avg_Mdcr_Alowd_Amt")
                )
            Next

            dgvAPC.DataSource = dtCustom
            FormatApcTable()
            lblstatus.Text = ""
        Catch ex As Exception
            lblstatus.Text = "Error loading outpatient APC data. Please try again."
        End Try

        lblstatus.Visible = False
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

    ' Form load event
    Private Async Sub Outpatient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadApcDataAsync(Results.SelectedHospital)
    End Sub

    ' Navigation buttons
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnProfileOutpatient.Click
        Me.Hide()
        Profile.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentsOutpatient.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialOutpatient.Click
        Me.Hide()
        Financial.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndOutpatient.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityOutpatient.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientOutpatient.Click
        Me.Hide()
        Inpatient.Show()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub
End Class
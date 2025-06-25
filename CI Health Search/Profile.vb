Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class Profile
    ' Use your actual Azure SQL connection string
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    ' Now accepts a state parameter
    Public Sub ShowProfile(hospitalId As Integer, state As String)
        Dim query As String = ""
        If state = "TN" Then
            query = "SELECT * FROM tn.AdminCon WHERE LicenseNum = @LicenseNum"
        ElseIf state = "TX" Then
            query = "SELECT * FROM tx.Utilization WHERE id = @id"
        Else
            MessageBox.Show("Unsupported state selected.")
            Exit Sub
        End If

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                If state = "TN" Then
                    cmd.Parameters.AddWithValue("@LicenseNum", hospitalId)
                ElseIf state = "TX" Then
                    cmd.Parameters.AddWithValue("@id", hospitalId)
                End If
                conn.Open()
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim schemaTable = reader.GetSchemaTable()
                        Dim columns = schemaTable.Rows.Cast(Of DataRow)().Select(Function(r) r("ColumnName").ToString()).ToList()

                        lblNameAddressResult.Text = If(columns.Contains("Facility Name") AndAlso Not IsDBNull(reader("Facility Name")), reader("Facility Name").ToString(), "N/A")
                        lblPhoneNumResult.Text = If(columns.Contains("Phone") AndAlso Not IsDBNull(reader("Phone")), reader("Phone").ToString(), If(columns.Contains("ContactNumber") AndAlso Not IsDBNull(reader("ContactNumber")), reader("ContactNumber").ToString(), "N/A"))
                        lblCeoPresResult.Text = If(columns.Contains("Admin") AndAlso Not IsDBNull(reader("Admin")), reader("Admin").ToString(), If(columns.Contains("CEO") AndAlso Not IsDBNull(reader("CEO")), reader("CEO").ToString(), "N/A"))
                        lblCountyFipsResult.Text = If(columns.Contains("County") AndAlso Not IsDBNull(reader("County")), reader("County").ToString(), "N/A")
                        lblTotalPatientDaysResult.Text = If(columns.Contains("Inpatient Days") AndAlso Not IsDBNull(reader("Inpatient Days")), reader("Inpatient Days").ToString(), "N/A")
                    Else
                        lblNameAddressResult.Text = "No result"
                        lblPhoneNumResult.Text = "No result"
                        lblCeoPresResult.Text = "No result"
                        lblCountyFipsResult.Text = "No result"
                        lblTotalPatientDaysResult.Text = "No result"
                    End If
                End Using
            End Using
        End Using
    End Sub

    ' Navigation buttons (already in your code)
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentProfile.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialProfile.Click
        Me.Hide()
        Financial.Show()
        Financial.ShowFinancialData(Results.SelectedHospitalContext.HospitalId, Results.SelectedHospitalContext.State)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnFinIndProfile.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnQualityProfile.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles btnInpatientProfile.Click
        Me.Hide()
        Inpatient.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientProfile.Click
        Me.Hide()
        Outpatient.Show()
    End Sub
End Class
Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class Profile
    ' Use your actual Azure SQL connection string
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    Public Sub ShowProfile(hospitalId As Integer)
        ' Query for full details
        Dim query As String = "SELECT * FROM tn.AdminCon WHERE LicenseNum = @LicenseNum"
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@LicenseNum", hospitalId)
                conn.Open()
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ' Replace with your actual label names and column names
                        lblNameAddressResult.Text = reader("Facility Name").ToString()
                        lblPhoneNumResult.Text = reader("Phone").ToString()
                        'lblCmsCertNumProfileResult.Text = reader("CMSCertNum").ToString()
                        ' ...populate other labels as needed
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
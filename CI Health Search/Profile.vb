Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class Profile
    ' Use your actual Azure SQL connection string
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    ' Now accepts a state parameter
    Public Sub ShowProfile(hospitalId As Integer, state As String)
        Dim query As String = ""
        Dim colName As String = ""
        Dim colPhone As String = ""
        Dim colAdmin As String = ""
        Dim colCounty As String = ""

        ' Set table and column names based on state
        If state = "TN" Then
            query = "SELECT * FROM tn.AdminCon WHERE LicenseNum = @LicenseNum"
            colName = "Facility Name"
            colPhone = "Phone"
            colAdmin = "Admin"
            colCounty = "County"
        ElseIf state = "TX" Then
            query = "SELECT * FROM tx.Utilization WHERE id = @id"
            colName = "Facility Name"
            colPhone = "City"
            colAdmin = "Ownership"
            colCounty = "County"
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
                        lblNameAddressResult.Text = If(IsDBNull(reader(colName)), "No result", reader(colName).ToString())
                        lblPhoneNumResult.Text = If(IsDBNull(reader(colPhone)), "No result", reader(colPhone).ToString())
                        lblCeoPresResult.Text = If(IsDBNull(reader(colAdmin)), "No result", reader(colAdmin).ToString())
                        lblCountyFipsResult.Text = If(IsDBNull(reader(colCounty)), "No result", reader(colCounty).ToString())
                        ' Add more mappings as needed
                    Else
                        lblNameAddressResult.Text = "No result"
                        lblPhoneNumResult.Text = "No result"
                        lblCeoPresResult.Text = "No result"
                        lblCountyFipsResult.Text = "No result"
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
Imports System.Data.SqlClient

Public Class Financial
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    ' Call this method to load data for the selected hospital
    Public Sub ShowFinancialData(hospitalId As Integer, state As String)
        Dim query As String = ""

        ' Choose the correct table based on state
        Select Case state
            Case "TN"
                query = "SELECT * FROM tn.Financials WHERE LicenseNum = @LicenseNum"
            Case "TX"
                query = "SELECT * FROM tx.Finance WHERE id = @id"
            Case Else
                MessageBox.Show("Unsupported state selected.")
                Exit Sub
        End Select

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
                        ' Example: Set your labels or controls here
                        lbligrresult.Text = If(Not IsDBNull(reader("Total Gross Inpatient Revenue")), reader("Total Gross Inpatient Revenue").ToString(), "N/A")
                        lblogrresult.Text = If(Not IsDBNull(reader("Total Gross Outpatient Revenue")), reader("Total Gross Outpatient Revenue").ToString(), "N/A")

                        ' Add more fields as needed
                    Else
                        lbligrresult.Text = "No result"
                        lblogrresult.Text = "No result"
                    End If
                End Using
            End Using
        End Using
    End Sub

    ' Optionally, call this automatically when the form loads
    Private Sub Financial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Results.SelectedHospitalContext IsNot Nothing Then
            ShowFinancialData(Results.SelectedHospitalContext.HospitalId, Results.SelectedHospitalContext.State)
        End If
    End Sub

End Class
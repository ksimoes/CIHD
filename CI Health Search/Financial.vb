Imports System.Data.SqlClient

Public Class Financial
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    ' Call this method to load data for the selected hospital
    Public Sub ShowFinancialData(hospitalId As Integer, state As String)
        Dim queryFinance As String = ""
        Dim queryCharity As String = ""

        ' Choose the correct table based on state
        Select Case state
            Case "TN"
                queryFinance = "SELECT * FROM tn.Financials WHERE LicenseNum = @LicenseNum"
            ' Add TN charity query here if needed
            Case "TX"
                queryFinance = "SELECT * FROM tx.Finance WHERE id = @id"
                queryCharity = "SELECT * FROM tx.Charity WHERE id = @id"
            Case Else
                MessageBox.Show("Unsupported state selected.")
                Exit Sub
        End Select

        ' --- Query 1: Finance Table ---
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(queryFinance, conn)
                If state = "TN" Then
                    cmd.Parameters.AddWithValue("@LicenseNum", hospitalId)
                ElseIf state = "TX" Then
                    cmd.Parameters.AddWithValue("@id", hospitalId)
                End If
                conn.Open()
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        lbligrresult.Text = If(Not IsDBNull(reader("Total Gross Inpatient Revenue")), reader("Total Gross Inpatient Revenue").ToString(), "N/A")
                        lblogrresult.Text = If(Not IsDBNull(reader("Total Gross Outpatient Revenue")), reader("Total Gross Outpatient Revenue").ToString(), "N/A")
                        ' Add more fields as needed
                    Else
                        lbligrresult.Text = "No result"
                        lblogrresult.Text = "No result"
                    End If
                End Using
                conn.Close()
            End Using
        End Using

        ' --- Query 2: Charity Table (TX only) ---
        If state = "TX" Then
            Using conn2 As New SqlConnection(connectionString)
                Using cmd2 As New SqlCommand(queryCharity, conn2)
                    cmd2.Parameters.AddWithValue("@id", hospitalId)
                    conn2.Open()
                    Using reader2 = cmd2.ExecuteReader()
                        If reader2.Read() Then
                            ' Example: Set your labels or controls for charity data here
                            ' For example, if you have a column "CharityCareAmount":
                            lblTotUcResult.Text = If(Not IsDBNull(reader2("Total Uncompensated Care")), reader2("Total Uncompensated Care").ToString(), "N/A")
                            lblUncompResult.Text = If(Not IsDBNull(reader2("Bad Debt Charges")), reader2("Bad Debt Charges").ToString(), "N/A")
                            lblCcResult.Text = If(Not IsDBNull(reader2("Charity Charges")), reader2("Charity Charges").ToString(), "N/A")
                            lblucpctResult.Text = If(Not IsDBNull(reader2("Uncompensated Care as pcnt of GPR")), reader2("Uncompensated Care as pcnt of GPR").ToString(), "N/A")
                            ' Add more fields as needed
                        Else
                            lblTotUcResult.Text = "No result"
                            lblUncompResult.Text = "No result"
                            lblCcResult.Text = "No result"
                            lblucpctResult.Text = "No result"
                        End If
                    End Using
                End Using
            End Using
        End If
    End Sub

    ' Optionally, call this automatically when the form loads
    Private Sub Financial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Results.SelectedHospitalContext IsNot Nothing Then
            ShowFinancialData(Results.SelectedHospitalContext.HospitalId, Results.SelectedHospitalContext.State)
        End If
    End Sub

End Class
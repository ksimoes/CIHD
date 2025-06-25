Imports System.Data.SqlClient

Public Class Search
    ' Use your actual Azure SQL connection string
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    Private Sub btnSearchAll_Click(sender As Object, e As EventArgs) Handles btnSearchAll.Click
        ' Get selected state from the ListBox
        Dim selectedState As String = TryCast(lbStateAll.SelectedItem, String)

        Dim query As String = ""
        Dim dt As New DataTable()

        If selectedState = "TN" Then
            query = "SELECT LicenseNum,[Facility Name],State FROM tn.AdminCon WHERE 1=1"
            If Not String.IsNullOrEmpty(selectedState) Then
                query &= " AND State = @State"
            End If
        ElseIf selectedState = "TX" Then
            query = "SELECT id AS LicenseNum, [Facility Name], State FROM tx.Utilization WHERE 1=1"
            If Not String.IsNullOrEmpty(selectedState) Then
                query &= " AND State = @State"
            End If
        Else
            MessageBox.Show("Please select a valid state.")
            Return
        End If

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, conn)
                If Not String.IsNullOrEmpty(selectedState) Then
                    cmd.Parameters.AddWithValue("@State", selectedState)
                End If
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using

        ' Pass results to Results form
        Results.SetResults(dt)
        Results.SelectedState = selectedState
        Hide()
        Results.Show()
    End Sub
End Class
Imports System.Data.SqlClient

Public Class Search
    ' Use your actual Azure SQL connection string
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

    Private Sub btnSearchAll_Click(sender As Object, e As EventArgs) Handles btnSearchAll.Click
        ' Get selected state from the ListBox
        Dim selectedState As String = TryCast(lbStateAll.SelectedItem, String)

        ' Build SQL query with filters
        Dim query As String = "SELECT LicenseNum,[Facility Name],State FROM tn.AdminCon WHERE 1=1"
        If Not String.IsNullOrEmpty(selectedState) Then
            query &= " AND State = @State"
        End If

        Dim dt As New DataTable()
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
        Hide()
        Results.Show()
    End Sub
End Class
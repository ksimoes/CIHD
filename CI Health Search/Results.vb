Public Class Results

    Public Property SelectedState As String
    Private resultsTable As DataTable
    Public strCMSnum As String
    Public Shared SelectedHospital As New HospitalContext()

    ' Call this from Search form to set and display results
    Public Sub SetResults(dt As DataTable)
        lblstatus.Text = "Loading results..."
        lblstatus.Visible = True

        Try
            resultsTable = dt

            CheckedListBox1.Items.Clear()
            Dim displayCol As String = ""
            If dt.Columns.Contains("facility_name") Then
                displayCol = "facility_name"
            ElseIf dt.Columns.Contains("provider_name") Then
                displayCol = "provider_name"
            ElseIf dt.Columns.Contains("Rndrng_Prvdr_Org_Name") Then
                displayCol = "Rndrng_Prvdr_Org_Name"
            ElseIf dt.Columns.Contains("Hospital Name") Then
                displayCol = "Hospital Name"
            ElseIf dt.Columns.Contains("Facility Name") Then
                displayCol = "Facility Name"
            ElseIf dt.Columns.Count > 0 Then
                displayCol = dt.Columns(0).ColumnName
            End If

            For Each row As DataRow In dt.Rows
                CheckedListBox1.Items.Add(row(displayCol).ToString())
            Next
            Dim items As New List(Of String)
            For Each item In CheckedListBox1.Items
                items.Add(item.ToString())
            Next

            items.Sort()
            CheckedListBox1.Items.Clear()
            For Each item In items
                CheckedListBox1.Items.Add(item)
            Next

            lblstatus.Text = ""
        Catch ex As Exception
            lblstatus.Text = "Error loading results. Please try again."
        End Try

        lblstatus.Visible = False
    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If CheckedListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If
        Try
            retrieveProfile()
            Profile.ShowProfile(SelectedHospital)
            Hide()
            Profile.Show()
        Catch ex As Exception
            lblstatus.Text = "Error loading profile. Please try again."
        End Try
    End Sub

    Public Function GetSelectedHospital() As HospitalContext
        retrieveProfile()
        Return SelectedHospital
    End Function

    Public Sub retrieveProfile()
        Dim selectedName As String = CheckedListBox1.SelectedItem.ToString().Trim()
        Dim selectedRow As DataRow = Nothing

        If resultsTable.Columns.Contains("Facility Name") Then
            selectedRow = resultsTable.Select($"[Facility Name] = '{selectedName.Replace("'", "''")}'").FirstOrDefault()
        ElseIf resultsTable.Columns.Contains("Hospital Name") Then
            selectedRow = resultsTable.Select($"[Hospital Name] = '{selectedName.Replace("'", "''")}'").FirstOrDefault()

            SelectedHospital.CMSNum = Search.CleanMeUp(selectedRow(1))
            SelectedHospital.State = selectedRow(5)
            SelectedHospital.Name = Search.CleanMeUp(selectedRow(2))
            SelectedHospital.Address = Search.CleanMeUp(selectedRow(3))
            SelectedHospital.City = Search.CleanMeUp(selectedRow(4))
            SelectedHospital.CBSAnum = Search.CleanMeUp(selectedRow(8))
            SelectedHospital.TotalDays = Search.CleanMeUp(selectedRow(20), True)
            SelectedHospital.FacilityType = Search.CleanMeUp(selectedRow(10))
            SelectedHospital.NumOfBeds = Search.CleanMeUp(selectedRow(21), True)
            SelectedHospital.NumOfEmployees = Search.CleanMeUp(selectedRow(15))
            SelectedHospital.TotalDischarges = Search.CleanMeUp(selectedRow(26), True)
            SelectedHospital.TotalPatientRev = Search.CleanMeUp(selectedRow(105), True)
            SelectedHospital.NetPatientRev = Search.CleanMeUp(selectedRow(107), True)
            SelectedHospital.RuralOUrban = Search.CleanMeUp(selectedRow(9))
            SelectedHospital.charityCost = Search.CleanMeUp(selectedRow(38), True)
            SelectedHospital.uncompensatedCost = Search.CleanMeUp(selectedRow(40), True)
        End If

        If selectedRow IsNot Nothing Then
            Dim hospitalId As Integer = 0
            If resultsTable.Columns.Contains("LicenseNum") Then
                Integer.TryParse(selectedRow("LicenseNum").ToString(), hospitalId)
            End If

            Dim npi As String = If(resultsTable.Columns.Contains("NPI"), selectedRow("NPI").ToString(), "")
            Dim cmsNum As String = If(resultsTable.Columns.Contains("CMSNum"), selectedRow("CMSNum").ToString(), "")

            If (resultsTable.Columns.Count < 6) Then
                SelectedHospital.State = selectedRow(2)
                SelectedHospital.HospitalId = selectedRow(0)
                SelectedHospital.CMSNum = selectedRow(3)
            Else
                SelectedHospital.State = selectedRow(5)
                SelectedHospital.HospitalId = selectedRow(1)
                SelectedHospital.CMSNum = cmsNum
            End If

            SelectedHospital.CMSNum = selectedRow(1)
        End If
    End Sub

    Private Sub Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblstatus.Text = ""
        lblstatus.Visible = False
    End Sub

    Private Async Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If CheckedListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If
        Try
            retrieveProfile()
            Hide()
            Inpatient.Show()
            Await Inpatient.LoadPatientOriginDataAsync(Results.SelectedHospital)
            Await Inpatient.LoadCeoDataAsync(Results.SelectedHospital)
        Catch ex As Exception
            lblstatus.Text = "Error loading inpatient data. Please try again."
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If CheckedListBox1.SelectedIndex = -1 Then
            MessageBox.Show("Select a hospital first.")
            Return
        End If
        Try
            retrieveProfile()
            Hide()
            Financial.Show()
            Await Financial.ShowFinancialData(Results.SelectedHospital.HospitalId, Results.SelectedHospital.State)
        Catch ex As Exception
            lblstatus.Text = "Error loading financial data. Please try again."
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Hide()
        FinInd.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Hide()
        Quality.Show()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Me.Hide()
        Outpatient.Show()
    End Sub
End Class
Imports System.Data.SqlClient
Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Public Class Profile
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    Public strCMSnum As String
    Public Async Sub ShowProfile(hospitalId As Integer, state As String, npi As String, cmsNum As String)
        Dim useSql As Boolean = (state = "TN" Or state = "TX")
        strCMSnum = cmsNum
        If useSql Then
            Dim queryProfile As String = If(state = "TN",
                "SELECT * FROM tn.AdminCon WHERE LicenseNum = @LicenseNum",
                "SELECT * FROM tx.Utilization WHERE id = @id")
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(queryProfile, conn)
                    If state = "TN" Then
                        cmd.Parameters.AddWithValue("@LicenseNum", hospitalId)
                    ElseIf state = "TX" Then
                        cmd.Parameters.AddWithValue("@id", hospitalId)
                    End If
                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            lblNameAddressResult.Text = SafeGet(reader, "Facility Name")
                            lblPhoneNumResult.Text = SafeGet(reader, "Phone")
                            lblCeoPresResult.Text = SafeGet(reader, "Admin")
                            lblCountyFipsResult.Text = SafeGet(reader, "County")
                            lblTotalPatientDaysResult.Text = SafeGet(reader, "Inpatient Days")
                        Else
                            lblNameAddressResult.Text = "No result"
                            lblPhoneNumResult.Text = "No result"
                            lblCeoPresResult.Text = "No result"
                            lblCountyFipsResult.Text = "No result"
                            lblTotalPatientDaysResult.Text = "No result"
                        End If
                        Await ShowApiProfileAsync(npi, cmsNum)
                    End Using
                End Using
            End Using
        Else
            Await ShowApiProfileAsync(npi, cmsNum)
        End If
    End Sub

    ' Helper function to safely get column value by name
    Private Function SafeGet(reader As SqlDataReader, columnName As String) As String
        Try
            Dim ordinal = reader.GetOrdinal(columnName)
            If Not reader.IsDBNull(ordinal) Then
                Return reader.GetValue(ordinal).ToString()
            End If
        Catch ex As IndexOutOfRangeException
            ' Column does not exist
        End Try
        Return "N/A"
    End Function

    Public Async Function ShowApiProfileAsync(npi As String, cmsNum As String) As Task
        Dim apiUrl As String = "https://data.cms.gov/data-api/v1/dataset/8015f175-35cc-4cab-a664-b7c87d91a027/data?"
        Dim mainURL As String = "https://data.cms.gov/data-api/v1/dataset/8143cbc7-484f-438b-9dfa-2e81d5d6a1ed/data?"
        Dim filters As New List(Of String)
        If Not String.IsNullOrEmpty(cmsNum) Then filters.Add("keyword=" & Uri.EscapeDataString(cmsNum))
        If Not String.IsNullOrEmpty(npi) Then filters.Add("keyword=" & Uri.EscapeDataString(npi))
        apiUrl &= String.Join("&", filters)
        filters.Add("size=1000")
        strCMSnum = cmsNum
        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)
                If data.Count > 0 Then
                    ' Find the exact match for Provider CCN if possible
                    Dim provider = data.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = cmsNum)
                    If provider Is Nothing Then provider = data(0)

                    lblCmsCertNumProfileResult.Text = If(provider("Provider CCN") IsNot Nothing, provider("Provider CCN").ToString(), "N/A")
                    lblNameAddressResult.Text = If(provider("Hospital Name") IsNot Nothing, provider("Hospital Name").ToString(), "N/A")
                    lblFacilityResult.Text = If(provider("CCN Facility Type") IsNot Nothing, provider("CCN Facility Type").ToString(), "N/A")
                    lbladdy.Text = If(provider("Street Address") IsNot Nothing, provider("Street Address").ToString(), "N/A")
                    lblCountyFipsResult.Text = If(provider("County Name") IsNot Nothing, provider("County Name").ToString(), "N/A")

                    If lblCountyFipsResult.Text.Equals("N/A") Then lblCountyFipsResult.Text = If(provider("County") IsNot Nothing, provider("County").ToString(), "N/A")
                    lblCbsaResult.Text = If(provider("Medicare CBSA Number") IsNot Nothing, provider("Medicare CBSA Number").ToString(), "N/A")
                    lblGeneralMedSurgBedsResult.Text = If(provider("Number of Beds") IsNot Nothing, provider("Number of Beds").ToString(), "N/A")
                    lblTotalEmployeesResult.Text = If(If(provider("FTE - Total Employees On Payroll") IsNot Nothing, provider("FTE - Total Employees On Payroll").ToString(), "N/A").Equals("N/A"), If(provider("FTE - Employees On Payroll") IsNot Nothing, provider("FTE - Employees On Payroll").ToString(), "N/A"), "N/A")
                    lblTotalDischargesResult.Text = If(provider("Total Discharges Title V") IsNot Nothing, provider("Total Discharges Title V").ToString(), "N/A")
                    lblTotalPatientRevenueResult.Text = If(provider("Total Patient Revenue") IsNot Nothing, provider("Total Patient Revenue").ToString(), "N/A")
                    lblTypeControlResult.Text = If(provider("Type of Control") IsNot Nothing, provider("Type of Control").ToString(), "N/A")

                    lblCmsUrbRurDesigResult.Text = If(provider("Rural Versus Urban") IsNot Nothing, provider("Rural Versus Urban").ToString(), "N/A")

                Else
                    lblCmsCertNumProfileResult.Text = "No result"
                    lblNameAddressResult.Text = "No result"
                    lbladdy.Text = "No result"
                    lblCountyFipsResult.Text = "No result"
                    lblCbsaResult.Text = "No result"
                    lblGeneralMedSurgBedsResult.Text = "No result"
                    lblTotalEmployeesResult.Text = "No result"
                    lblTotalDischargesResult.Text = "No result"
                    lblCmsUrbRurDesigResult.Text = "No result"

                End If
            Else
                lblCmsCertNumProfileResult.Text = "API error"
                lblNameAddressResult.Text = "API error"
                lbladdy.Text = "API error"
                lblCountyFipsResult.Text = "API error"
                lblCbsaResult.Text = "API error"
                lblGeneralMedSurgBedsResult.Text = "API error"
                lblTotalEmployeesResult.Text = "API error"
                lblTotalDischargesResult.Text = "API error"
                lblCmsUrbRurDesigResult.Text = "API error"

            End If
        End Using

        Using mainClient As New HttpClient()
            Dim response As HttpResponseMessage = Await mainClient.GetAsync(mainURL & "filter[PRVDR_NUM]=" & cmsNum & "&offset=0&size=1")
            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim data As JArray = JArray.Parse(json)

                If Data.Count > 0 Then
                    ' Find the exact match for Provider CCN if possible
                    Dim provider = Data.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = cmsNum)
                    If provider Is Nothing Then provider = data(0)
                    lblPhoneNumResult.Text = If(provider("PHNE_NUM") IsNot Nothing, provider("PHNE_NUM").ToString(), "N/A")
                    lblCbsaResult.Text = If(provider("CBSA_CD") IsNot Nothing, provider("CBSA_CD").ToString(), "N/A")
                    lblMedicareCertifiedBedsResult.Text = Financial.cleanMeUp(provider("MDCR_SNF_BED_CNT"))
                End If
            End If
        End Using

    End Function

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
        FinInd.ShowFinancialDataApi(lblCmsCertNumProfileResult.Text)
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

    Private Sub Profile_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub lblPhoneNum_Click(sender As Object, e As EventArgs) Handles lblPhoneNum.Click

    End Sub
End Class
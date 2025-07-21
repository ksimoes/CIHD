Imports System.Data.SqlClient
Imports Newtonsoft.Json.Linq

Public Class Profile
    Private connectionString As String = "Data Source=cihg-sql1.database.windows.net;Initial Catalog=CIHData;User ID=cihgadmin;Password=P!bxbFrHw4-jCvU*;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    Public strCMSnum As String

    ' Facility type mapping
    Private ReadOnly FacilityTypeMap As New Dictionary(Of String, String) From {
        {"STH", "Short-term"},
        {"FQHC", "Federally Qualified Health Center"},
        {"ADH", "Alcohol/Drug Hospitals"},
        {"MAF", "Medical Assistance Facilitie"},
        {"CAH", "Critical Access Hospital"},
        {"CCMHC", "Continuation of Community Mental Health Center"},
        {"HOS", "Hospice"},
        {"RNMHC", "Religious Non-medical Health Care Institution"},
        {"LTCH", "Long-Term Care Hospital"},
        {"HBRDF", "Hospital-based Renal Dialysis Facility"},
        {"IDRF", "Independent Renal Dialysis Facility"},
        {"ISPRDF", "Independent Special Purpose Renal Dialysis Facility"},
        {"FTH", "Formerly Tuberculosis Hospital"},
        {"RH", "Rehabilitation Hospital"},
        {"HHA", "Home Health Agency"},
        {"CCORF", "Continuation of Comprehensive Outpatient Rehabilitation Facility"},
        {"CH", "Children’s Hospital"},
        {"RHC", "Continuation of Rural Health Clinic"},
        {"HBSRDF", "Hospital-based Special Purpose Renal Dialysis Facility"},
        {"PH", "Psychiatric Hospital"},
        {"CORF", "Comprehensive Outpatient Rehabilitation Facility"},
        {"CMHC", "Community Mental Health Center"},
        {"SNF", "Skilled Nursing Facility"},
        {"OPTS", "Outpatient Physical Therapy Services"},
        {"NR", "Numbers Reserved"},
        {"CHHA", "Continuation of Home Health Agency"},
        {"TC", "Transplant Center"},
        {"RFU", "Reserved for Future Use"}
    }

    ' Entry point for showing a profile
    Public Async Sub ShowProfile(foundHospital As HospitalContext)
        Results.SelectedHospital = foundHospital
        strCMSnum = foundHospital.CMSNum

        lblstatus.Text = "Loading profile..."
        lblstatus.Visible = True

        Try
            If foundHospital.State = "TN" OrElse foundHospital.State = "TX" Then
                Await ShowSqlProfileAsync(foundHospital)
            Else
                Await ShowApiProfileAsync(foundHospital)
            End If
            lblstatus.Text = ""
        Catch ex As Exception
            SetAllProfileLabels("No result")
            lblstatus.Text = "Error loading profile. Please try again."
        End Try

        lblstatus.Visible = False
    End Sub

    ' SQL profile loading
    Private Async Function ShowSqlProfileAsync(foundHospital As HospitalContext) As Task
        Dim queryProfile As String = If(foundHospital.State = "TN",
            "SELECT * FROM tn.AdminCon WHERE LicenseNum = @LicenseNum",
            "SELECT * FROM tx.Utilization WHERE id = @id")

        Try
            Using conn As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(queryProfile, conn)
                    If foundHospital.State = "TN" Then
                        cmd.Parameters.AddWithValue("@LicenseNum", foundHospital.HospitalId)
                    Else
                        cmd.Parameters.AddWithValue("@id", foundHospital.HospitalId)
                    End If
                    conn.Open()
                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            lblNameAddressResult.Text = AppHelpers.SafeGet(reader, "Facility Name")
                            lblPhoneNumResult.Text = AppHelpers.SafeGet(reader, "Phone")
                            lblCeoPresResult.Text = AppHelpers.SafeGet(reader, "Admin")
                            lblCountyFipsResult.Text = AppHelpers.SafeGet(reader, "County")
                            lblTotalPatientDaysResult.Text = AppHelpers.SafeGet(reader, "Inpatient Days")
                        Else
                            SetAllProfileLabels("No result")
                        End If
                    End Using
                End Using
            End Using
        Catch
            SetAllProfileLabels("No result")
        End Try

        Await ShowApiProfileAsync(foundHospital)
    End Function

    ' API profile loading
    Public Async Function ShowApiProfileAsync(foundHosp As HospitalContext) As Task
        Dim apiUrl = ApiHelper.ApiUrls("MainProfileApi") & "?keyword=" & Uri.EscapeDataString(foundHosp.CMSNum) & "&size=1000"
        Dim mainUrl = ApiHelper.ApiUrls("ProfileMainUrl") & "?filter[PRVDR_NUM]=" & foundHosp.CMSNum & "&offset=0&size=1"

        Try
            Dim myArray As JArray = Await ApiHelper.GetApiDataAsync(apiUrl)
            If myArray.Count > 0 Then
                Dim provider = myArray.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = foundHosp.CMSNum)
                If provider Is Nothing Then provider = myArray(0)

                lblFacilityResult.Text = GetFacilityType(provider)
                lblCountyFipsResult.Text = AppHelpers.SafeGet(provider, "County Name", "County")
                lblCmsCertNumProfileResult.Text = foundHosp.CMSNum
                lblNameAddressResult.Text = foundHosp.Name
                lbladdy.Text = foundHosp.Address
                lblCbsaResult.Text = foundHosp.CBSAnum
                lblGeneralMedSurgBedsResult.Text = foundHosp.NumOfBeds.ToString()
                lblTotalEmployeesResult.Text = foundHosp.NumOfEmployees
                lblTotalDischargesResult.Text = foundHosp.TotalDischarges.ToString()
                lblCmsUrbRurDesigResult.Text = foundHosp.RuralOUrban
                lblRuralReferralResult.Text = If(lblCmsUrbRurDesigResult.Text = "R", "Y", If(lblCmsUrbRurDesigResult.Text = "U", "N", ""))
                lblZipCodeResult.Text = foundHosp.Zip
                lblTotalPatientRevenueResult.Text = AppHelpers.SafeGet(provider, "Total Patient Revenue")
                lblTypeControlResult.Text = AppHelpers.SafeGet(provider, "Type of Control")
                lblZipCodeResult.Text = AppHelpers.SafeGet(provider, "Zip Code")
                lblTotalPatientDaysResult.Text = AppHelpers.SafeGet(provider, "Hospital Total Days (V + XVIII + XIX + Unknown) For Adults & Peds ")
            Else
                SetAllProfileLabels("No result")
            End If

            ' Load additional data from mainUrl
            Dim dataobject As JArray = Await ApiHelper.GetApiDataAsync(mainUrl)
            If dataobject.Count > 0 Then
                Dim provider = dataobject.FirstOrDefault(Function(x) x("Provider CCN") IsNot Nothing AndAlso x("Provider CCN").ToString() = foundHosp.CMSNum)
                If provider Is Nothing Then provider = dataobject(0)
                lblPhoneNumResult.Text = AppHelpers.SafeGet(provider, "PHNE_NUM")
                lblCbsaResult.Text = AppHelpers.SafeGet(provider, "CBSA_CD")
                lblMedicareCertifiedBedsResult.Text = Search.CleanMeUp(AppHelpers.SafeGet(provider, "MDCR_SNF_BED_CNT"))
            End If
        Catch
            SetAllProfileLabels("No result")
        End Try
    End Function

    ' Helper: Get facility type description
    Private Function GetFacilityType(provider As JObject) As String
        Dim acronym As String = AppHelpers.SafeGet(provider, "CCN Facility Type")
        If FacilityTypeMap.ContainsKey(acronym) Then
            Return FacilityTypeMap(acronym)
        End If
        Return acronym
    End Function

    ' Helper: Set all profile labels to a value
    Private Sub SetAllProfileLabels(val As String)
        lblCmsCertNumProfileResult.Text = val
        lblNameAddressResult.Text = val
        lbladdy.Text = val
        lblCountyFipsResult.Text = val
        lblCbsaResult.Text = val
        lblGeneralMedSurgBedsResult.Text = val
        lblTotalEmployeesResult.Text = val
        lblTotalDischargesResult.Text = val
        lblCmsUrbRurDesigResult.Text = val
        lblZipCodeResult.Text = val
        lblTotalPatientRevenueResult.Text = val
        lblTypeControlResult.Text = val
        lblTotalPatientDaysResult.Text = val
        lblPhoneNumResult.Text = val
        lblMedicareCertifiedBedsResult.Text = val
        lblRuralReferralResult.Text = val
        lblFacilityResult.Text = val
    End Sub


    ' Navigation buttons
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnDepartmentProfile.Click
        Me.Hide()
        Departments.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnFinancialProfile.Click
        Me.Hide()
        Financial.Show()
        Financial.ShowFinancialData(Results.SelectedHospital.HospitalId, Results.SelectedHospital.State)
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
        Inpatient.LoadPatientOriginDataAsync(Results.SelectedHospital)
        Inpatient.LoadCeoDataAsync(Results.SelectedHospital)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles btnOutpatientProfile.Click
        Me.Hide()
        Outpatient.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
        Search.Show()
    End Sub

    Private Sub Profile_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

' Safe get for JObject with fallback keys

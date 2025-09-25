Public Class NavigationPopupForm
    Private hospitalContext As HospitalContext

    Public Sub New(ctx As HospitalContext)
        InitializeComponent()
        hospitalContext = ctx
    End Sub

End Class
Public Class DataTableViewerForm
    Public Sub New(dt As DataTable, title As String)
        InitializeComponent()
        Me.Text = title
        DataGridView1.DataSource = dt.Copy()
    End Sub
End Class
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ProviderDetailsForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        dgvDetails = New DataGridView()
        CType(dgvDetails, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' dgvDetails
        ' 
        dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvDetails.Location = New Point(54, 90)
        dgvDetails.Name = "dgvDetails"
        dgvDetails.Size = New Size(711, 366)
        dgvDetails.TabIndex = 0
        ' 
        ' ProviderDetailsForm
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1088, 633)
        Controls.Add(dgvDetails)
        Name = "ProviderDetailsForm"
        Text = "ProviderDetailsForm"
        CType(dgvDetails, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents dgvDetails As DataGridView
End Class
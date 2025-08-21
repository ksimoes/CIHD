<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SelectSearchTypeForm
    Inherits System.Windows.Forms.Form

    Private Sub InitializeComponent()
        Me.btnIndividual = New System.Windows.Forms.Button()
        Me.btnOrganization = New System.Windows.Forms.Button()
        Me.btnBoth = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        ' btnIndividual
        '
        Me.btnIndividual.Location = New System.Drawing.Point(20, 20)
        Me.btnIndividual.Name = "btnIndividual"
        Me.btnIndividual.Size = New System.Drawing.Size(120, 40)
        Me.btnIndividual.TabIndex = 0
        Me.btnIndividual.Text = "Individual"
        Me.btnIndividual.UseVisualStyleBackColor = True
        '
        ' btnOrganization
        '
        Me.btnOrganization.Location = New System.Drawing.Point(160, 20)
        Me.btnOrganization.Name = "btnOrganization"
        Me.btnOrganization.Size = New System.Drawing.Size(120, 40)
        Me.btnOrganization.TabIndex = 1
        Me.btnOrganization.Text = "Organization"
        Me.btnOrganization.UseVisualStyleBackColor = True
        '
        ' btnBoth
        '
        Me.btnBoth.Location = New System.Drawing.Point(90, 75)
        Me.btnBoth.Name = "btnBoth"
        Me.btnBoth.Size = New System.Drawing.Size(120, 40)
        Me.btnBoth.TabIndex = 2
        Me.btnBoth.Text = "Both"
        Me.btnBoth.UseVisualStyleBackColor = True
        '
        ' SelectSearchTypeForm
        '
        Me.AcceptButton = Me.btnIndividual
        Me.CancelButton = Me.btnOrganization
        Me.ClientSize = New System.Drawing.Size(300, 140)
        Me.Controls.Add(Me.btnIndividual)
        Me.Controls.Add(Me.btnOrganization)
        Me.Controls.Add(Me.btnBoth)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "SelectSearchTypeForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Search Type"
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents btnIndividual As System.Windows.Forms.Button
    Friend WithEvents btnOrganization As System.Windows.Forms.Button
    Friend WithEvents btnBoth As System.Windows.Forms.Button
End Class
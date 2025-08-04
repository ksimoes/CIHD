<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Results
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Button1 = New Button()
        Button2 = New Button()
        Button3 = New Button()
        Button4 = New Button()
        Button5 = New Button()
        Button6 = New Button()
        Button7 = New Button()
        Label1 = New Label()
        dgvResults = New DataGridView()
        CType(dgvResults, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.BackColor = SystemColors.ButtonHighlight
        Button1.Location = New Point(859, 3)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 26)
        Button1.TabIndex = 7
        Button1.Text = "Profile"
        Button1.UseVisualStyleBackColor = False
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(940, 3)
        Button2.Name = "Button2"
        Button2.Size = New Size(75, 26)
        Button2.TabIndex = 8
        Button2.Text = "Departments"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(1021, 3)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 26)
        Button3.TabIndex = 9
        Button3.Text = "Financial"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(1102, 3)
        Button4.Name = "Button4"
        Button4.Size = New Size(75, 26)
        Button4.TabIndex = 10
        Button4.Text = "Fin Ind"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(1183, 3)
        Button5.Name = "Button5"
        Button5.Size = New Size(75, 26)
        Button5.TabIndex = 11
        Button5.Text = "Quality"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Button6
        ' 
        Button6.Location = New Point(1264, 3)
        Button6.Name = "Button6"
        Button6.Size = New Size(75, 26)
        Button6.TabIndex = 12
        Button6.Text = "Inp"
        Button6.UseVisualStyleBackColor = True
        ' 
        ' Button7
        ' 
        Button7.Location = New Point(1345, 3)
        Button7.Name = "Button7"
        Button7.Size = New Size(75, 26)
        Button7.TabIndex = 13
        Button7.Text = "Outp"
        Button7.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 15F, FontStyle.Bold)
        Label1.Location = New Point(129, 73)
        Label1.Name = "Label1"
        Label1.Size = New Size(162, 28)
        Label1.TabIndex = 16
        Label1.Text = "Hospital Names"
        ' 
        ' dgvResults
        ' 
        dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvResults.Location = New Point(129, 104)
        dgvResults.Name = "dgvResults"
        dgvResults.Size = New Size(805, 392)
        dgvResults.TabIndex = 17
        ' 
        ' Results
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1423, 552)
        Controls.Add(dgvResults)
        Controls.Add(Label1)
        Controls.Add(Button7)
        Controls.Add(Button6)
        Controls.Add(Button5)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Name = "Results"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Results"
        CType(dgvResults, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents VScrollBar1 As VScrollBar
    Friend WithEvents dgvResults As DataGridView
End Class

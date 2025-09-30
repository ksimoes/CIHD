Public Class Login
    'Example: Hardcoded users For demonstration
    Private validUsers As New Dictionary(Of String, String) From {
        {"admin", "password123"},
        {"user1", "letmein"},
        {"user2", "secret"},
        {"me", "you"},
        {"p", "p"},
        {"", ""}
    }

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = txtPassword.Text

        If validUsers.ContainsKey(username) AndAlso validUsers(username) = password Then
            Me.Hide()
            Using dlg As New SelectSearchTypeForm()
                If dlg.ShowDialog() = DialogResult.OK Then
                    If dlg.SelectedType = "Individual" Then
                        Dim indForm As New Individual_Search()
                        indForm.Show()
                        indForm.cboSearchType.SelectedIndex = 0
                        indForm.LoadTaxonomyCombo()
                    ElseIf dlg.SelectedType = "Organization" Then
                        Search.Show()
                    End If
                Else
                    ' If user cancels, show login again
                    Me.Show()
                End If
            End Using
        Else
            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtPassword.Clear()
            txtPassword.Focus()
        End If
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
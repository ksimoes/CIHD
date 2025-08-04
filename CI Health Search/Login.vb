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
            Search.Show()
        Else
            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtPassword.Clear()
            txtPassword.Focus()
        End If
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
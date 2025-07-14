Public Class BoldGroupBox
    Inherits GroupBox

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Draw bold border
        Dim borderColor As Color = Color.Black
        Dim borderWidth As Integer = 2

        Dim rect As Rectangle = Me.ClientRectangle
        rect.Width -= 1
        rect.Height -= 1

        Using pen As New Pen(borderColor, borderWidth)
            e.Graphics.DrawRectangle(pen, rect)
        End Using

        ' Draw the text over the border for a clean look
        Dim textSize = e.Graphics.MeasureString(Me.Text, Me.Font)
        Dim textRect = New Rectangle(8, 0, CInt(textSize.Width), CInt(textSize.Height))
        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), textRect)
        e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), 8, 0)
    End Sub
End Class
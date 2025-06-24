

Imports System.Data.SqlClient

Public Class Form1
    Dim progressbarrunit As Double
    Dim progressbarwidth, progressbarheight, progressbarcomplte As Integer
    Dim bmp As Bitmap
    Dim g As Graphics
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        progressbarwidth = PictureBox1.Width
        progressbarheight = PictureBox1.Height
        progressbarrunit = progressbarwidth / 100
        progressbarcomplte = 0

        bmp = New Bitmap(progressbarwidth, progressbarheight)
        Timer1.Start()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        g = Graphics.FromImage(bmp)

        g.Clear(Color.White)

        g.FillRectangle(Brushes.DeepSkyBlue, New Rectangle(0, 0, CInt(progressbarcomplte * progressbarrunit), progressbarheight))

        g.DrawString(progressbarcomplte & "%", New Font("Segoe UI", progressbarheight / 2), Brushes.Black, New PointF(progressbarwidth / 2 - progressbarheight, progressbarheight / 10))

        PictureBox1.Image = bmp
        progressbarcomplte += 1
        If (progressbarcomplte > 100) Then
            g.Dispose()
            Me.Hide()
            Timer1.Stop()

            Dim log = New Login
            log.Show()
            Timer1.Enabled = False

        End If
    End Sub

    Private Sub lblSearch_Click(sender As Object, e As EventArgs) Handles lblSearch.Click

    End Sub






End Class




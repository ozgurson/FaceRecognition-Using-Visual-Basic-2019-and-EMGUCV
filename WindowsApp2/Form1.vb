Imports Emgu.CV
Imports Emgu.Util
Imports Emgu.CV.Structure
Imports Emgu.CV.Face
Imports System.IO


Public Class DefinetelyNotForm1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim pname As String = ""
        Dim detectedname As String = ""
        Dim filename As String = ""

        Dim detectedface As Image(Of Gray, Byte)
        Dim facedetection As New CascadeClassifier(global_cascadeFile)
        Dim minsize, maxsize As Size
        'Configure 
        Const rect_thickness As Byte = 16 'Thickness of the rectangle to draw around recognized fence
        minsize.Width = 100  'Minimum size of a face -Width
        minsize.Height = 100 'Minimum size of face -Height
        maxsize.Width = 1000 'Maximum size of a face -Width
        maxsize.Height = 1000 'Maximum size of a face -Height
        Dim rect_thickess As Byte = 16

        'Fill the face images and labels , also initial train.
        GetFaceList()

        OpenFileDialog1.ShowDialog()
        filename = OpenFileDialog1.FileName
        Dim photo As New Image(Of Bgr, Byte)(filename)
        PictureBox1.Image = photo.ToBitmap
        PictureBox1.Refresh()

        Dim img As Emgu.CV.Image(Of Gray, Byte) = photo.Convert(Of Gray, Byte)()
        For Each face As Rectangle In facedetection.DetectMultiScale(img, 1.1, 8, minsize, maxsize)
            'Draw a green rectangle for the first recognized face
            photo.Draw(face, New Bgr(Color.Green), 16)
            PictureBox1.Image = photo.ToBitmap : PictureBox1.Refresh()
            detectedface = photo.Copy(face).Convert(Of Gray, Byte)()

            If imageList.Size > 0 Then
                Dim result As FaceRecognizer.PredictionResult = recognizer.Predict(detectedface.Resize(100, 100, CvEnum.Inter.Cubic))
                detectedname = namelist(result.Label)
                If Not IsNothing(detectedname) Then
                    If MsgBox("Is this " + detectedname, 36, "Check") = vbYes Then
                        pname = detectedname
                    Else
                        pname = InputBox("Please enter the name for detected face [in green frame]")
                    End If
                End If
            Else
                pname = InputBox("Please enter the name for detected face [in green frame]")
            End If

            If pname <> "" Then
                detectedface = detectedface.Resize(100, 100, CvEnum.Inter.Cubic)
                detectedface.Save(global_trainedpictures_path + (facelist.Count + 1).ToString + ".jpg")
                Using sw As New StreamWriter(File.Open(global_app_path + "\facelist.txt", FileMode.Append))
                    sw.WriteLine((facelist.Count + 1).ToString + ".jpg" + ":" + pname)
                    sw.Close()
                    'Draw a black rectangle on the current green one, son when the next rect drawn it will be green and the ones processed will be black.
                    photo.Draw(face, New Bgr(Color.Black), rect_thickness)
                    'fill and train again!
                    GetFaceList()
                End Using
            Else
                photo.Draw(face, New Bgr(Color.Black), rect_thickness)
            End If
        Next
        photo = Nothing

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Check if the directories, exist create if they are not. Also check the files necessary (facelist.txt, haarcascade_frontalface_default.xml)
        CheckPrepareStructure()
    End Sub

    Public Sub GetFaceList()
        Dim i As Integer()
        Dim x As Integer
        facelist.Clear()
        Dim results = (From line In System.IO.File.ReadAllLines(global_facerecognitionNameFile)
                       Let value = line.Split(":") Let FaceImageName = value(0) Let PersonName = value(1))

        For Each line In results
            Dim FaceInstance = New FaceData
            FaceInstance.FaceImage = New Image(Of Gray, Byte)(global_trainedpictures_path + line.FaceImageName).Resize(100, 100, CvEnum.Inter.Cubic)
            FaceInstance.PersonName = line.PersonName
            facelist.Add(FaceInstance)
        Next

        For Each FaceX In facelist
            i = {x}
            imageList.Push(FaceX.FaceImage.Mat)
            namelist.Add(FaceX.PersonName)
            labellist.Push(i)
            'labelList.Push(new[] { i++ }); C#
            x += 1
        Next
        If (imageList.Size > 0) Then
            recognizer = New EigenFaceRecognizer(imageList.Size)
            recognizer.Train(imageList, labellist)
        End If
    End Sub

End Class

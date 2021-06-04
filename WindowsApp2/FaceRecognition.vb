Imports Emgu.CV
Imports Emgu.CV.Util
Imports Emgu.CV.Structure
Imports Emgu.CV.Face
Imports System.IO
Module FaceRecognition
    Public facelist As New List(Of FaceData)
    Public global_app_path As String = Microsoft.VisualBasic.Left(System.Reflection.Assembly.GetExecutingAssembly().Location.ToString, InStrRev(System.Reflection.Assembly.GetExecutingAssembly().Location.ToString, "\"))
    Public global_facerecognitionNameFile As String = global_app_path + "facelist.txt"
    Public global_trainedpictures_path As String = global_app_path + "faceimages\"
    Public global_cascadeFile = global_app_path + "haarcascade_frontalface_default.xml"
    Public imageList As New VectorOfMat()
    Public namelist As New List(Of String)
    Public labellist As New VectorOfInt
    Public recognizer As EigenFaceRecognizer
    Class FaceData
        Public Property _PersonName As String
        Public _FaceImage As Emgu.CV.Image(Of Gray, Byte)
        Public Property PersonName As String
            Get
                Return _PersonName
            End Get
            Set(ByVal value As String)
                _PersonName = value
            End Set
        End Property
        Public Property FaceImage As Emgu.CV.Image(Of Gray, Byte)
            Get
                Return _FaceImage
            End Get
            Set(value As Emgu.CV.Image(Of Gray, Byte))
                _FaceImage = value
            End Set
        End Property
    End Class
    Public Sub CheckPrepareStructure()
        If Not My.Computer.FileSystem.DirectoryExists(global_trainedpictures_path) Then
            Try
                My.Computer.FileSystem.CreateDirectory(global_app_path + "\faceimages")
            Catch
                MsgBox("Unable to create recognized face images directory under" + vbCrLf + global_app_path + vbCrLf + "Will terminate now!", MsgBoxStyle.Critical, "Error")
                End
            End Try
        End If
        If Not My.Computer.FileSystem.FileExists(global_facerecognitionNameFile) Then
            Try
                Using sw As New StreamWriter(File.Open(global_app_path + "\facelist.txt", FileMode.OpenOrCreate))
                    sw.Close()
                End Using
            Catch
                MsgBox("Unable to create recognized faces list file under" + vbCrLf + global_app_path + vbCrLf + "Will terminate now!", MsgBoxStyle.Critical, "Error")
                End
            End Try
        End If

        If Not My.Computer.FileSystem.FileExists(global_cascadeFile) Then
            MsgBox("Unable to find file 'haarcascade_frontalface_default.xml' under" + vbCrLf + global_app_path + vbCrLf + "Will terminate now!", MsgBoxStyle.Critical, "Error")
            End
        End If
    End Sub
End Module

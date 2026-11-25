using System;
using OpenCvSharp;

namespace YMH219_FaceRecognition
{
    /// <summary>
    /// Web kamerasını açar, kare okur ve kapatır.
    /// </summary>
    public class CameraService : IDisposable
    {
        private VideoCapture? capture;

        public bool IsRunning
        {
            get { return capture != null; }
        }

        /// <summary>
        /// Kamerayı başlatır. Kamera yoksa veya açılamazsa InvalidOperationException fırlatır.
        /// </summary>
        public void Start(int cameraIndex = 0)
        {
            Stop();

            // DirectShow Windows'ta hızlı açılır; açılamazsa varsayılan arka uç denenir.
            VideoCapture opened = new VideoCapture(cameraIndex, VideoCaptureAPIs.DSHOW);
            if (!opened.IsOpened())
            {
                opened.Dispose();
                opened = new VideoCapture(cameraIndex);
            }

            if (!opened.IsOpened())
            {
                opened.Dispose();
                throw new InvalidOperationException(
                    "Kamera bulunamadı veya başka bir uygulama tarafından kullanılıyor.");
            }

            capture = opened;
        }

        /// <summary>
        /// Kameradan bir kare okuyup verilen Mat içine yazar. Okunamazsa false döner.
        /// </summary>
        public bool TryReadFrame(Mat frame)
        {
            return capture != null && capture.Read(frame) && !frame.Empty();
        }

        public void Stop()
        {
            if (capture == null)
                return;

            capture.Release();
            capture.Dispose();
            capture = null;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}

using System;
using System.IO;
using System.Linq;
using OpenCvSharp;
using CvSize = OpenCvSharp.Size;

namespace YMH219_FaceRecognition
{
    /// <summary>
    /// Haar cascade modeli ile görüntüdeki yüzleri bulur ve yüz bölgesini tanıma için hazırlar.
    /// </summary>
    public class FaceDetector : IDisposable
    {
        public const string CascadeFileName = "haarcascade_frontalface_default.xml";

        /// <summary>Tanımada kullanılan kare yüz görüntüsünün kenar uzunluğu (piksel).</summary>
        public const int FaceSize = 200;

        private const int MinFaceSize = 60;

        private CascadeClassifier? cascade;

        public bool IsLoaded
        {
            get { return cascade != null; }
        }

        /// <summary>
        /// Model dosyasını yükler. Dosya yoksa FileNotFoundException, bozuksa InvalidDataException fırlatır.
        /// </summary>
        public void Load(string cascadePath)
        {
            if (!File.Exists(cascadePath))
                throw new FileNotFoundException("Yüz algılama model dosyası bulunamadı.", cascadePath);

            CascadeClassifier loaded = new CascadeClassifier(cascadePath);
            if (loaded.Empty())
            {
                loaded.Dispose();
                throw new InvalidDataException("Model dosyası okunamadı veya bozuk.");
            }

            cascade?.Dispose();
            cascade = loaded;
        }

        /// <summary>
        /// Görüntüdeki tüm yüzlerin dikdörtgenlerini büyükten küçüğe sıralı döndürür.
        /// </summary>
        public Rect[] Detect(Mat image)
        {
            if (cascade == null)
                throw new InvalidOperationException("Yüz algılama modeli yüklenmedi.");

            using (Mat gray = ToGray(image))
            {
                Cv2.EqualizeHist(gray, gray);   // ışık farklarını azaltır

                Rect[] faces = cascade.DetectMultiScale(
                    gray,
                    scaleFactor: 1.2,
                    minNeighbors: 5,
                    minSize: new CvSize(MinFaceSize, MinFaceSize));

                return faces.OrderByDescending(f => f.Width * f.Height).ToArray();
            }
        }

        /// <summary>
        /// Yüz bölgesini keser; gri tonlamalı, sabit boyutlu ve kontrastı dengelenmiş bir görüntü döndürür.
        /// Eğitim ve tahminde aynı ön işleme kullanılsın diye tek yerde tanımlıdır.
        /// </summary>
        public Mat ExtractFace(Mat image, Rect faceRect)
        {
            using (Mat gray = ToGray(image))
            using (Mat cropped = new Mat(gray, faceRect))
            {
                Mat face = new Mat();
                Cv2.Resize(cropped, face, new CvSize(FaceSize, FaceSize));
                Cv2.EqualizeHist(face, face);
                return face;
            }
        }

        private static Mat ToGray(Mat image)
        {
            Mat gray = new Mat();

            if (image.Channels() == 1)
                image.CopyTo(gray);
            else
                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

            return gray;
        }

        public void Dispose()
        {
            cascade?.Dispose();
            cascade = null;
        }
    }
}

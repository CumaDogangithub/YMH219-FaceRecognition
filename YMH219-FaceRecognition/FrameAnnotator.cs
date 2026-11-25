using System;
using OpenCvSharp;
using CvPoint = OpenCvSharp.Point;

namespace YMH219_FaceRecognition
{
    /// <summary>
    /// Algılanan yüzleri görüntü üzerinde dikdörtgen ve etiket yazısı ile işaretler.
    /// </summary>
    public static class FrameAnnotator
    {
        // OpenCV renkleri BGR sırasındadır.
        private static readonly Scalar KnownColor = new Scalar(0, 200, 0);       // yeşil: tanınan yüz
        private static readonly Scalar UnknownColor = new Scalar(0, 0, 255);     // kırmızı: bilinmeyen yüz
        private static readonly Scalar DetectedColor = new Scalar(255, 160, 0);  // mavi: model eğitilmemiş

        public static void Draw(Mat image, Rect face, RecognitionResult result)
        {
            Scalar color = DetectedColor;
            string text = "Yuz";

            if (result.HasScore)
            {
                color = result.IsKnown ? KnownColor : UnknownColor;
                text = ToAscii(result.Label) + " %" + result.ConfidencePercent;
            }

            // Çizgi kalınlığı ve yazı boyutu görüntü genişliğine göre ölçeklenir.
            int thickness = Math.Max(2, image.Width / 320);
            double fontScale = Math.Max(0.6, image.Width / 900.0);

            Cv2.Rectangle(image, face, color, thickness);

            // Yazı yüzün üstüne sığmıyorsa altına yazılır.
            int textY = face.Y > 40 ? face.Y - 10 : face.Y + face.Height + (int)(30 * fontScale);
            Cv2.PutText(image, text, new CvPoint(face.X, textY),
                HersheyFonts.HersheySimplex, fontScale, color, thickness);
        }

        /// <summary>
        /// OpenCV'nin yazı tipleri Türkçe karakterleri çizemediği için ASCII karşılıkları kullanılır.
        /// </summary>
        private static string ToAscii(string text)
        {
            return text
                .Replace('ç', 'c').Replace('Ç', 'C')
                .Replace('ğ', 'g').Replace('Ğ', 'G')
                .Replace('ı', 'i').Replace('İ', 'I')
                .Replace('ö', 'o').Replace('Ö', 'O')
                .Replace('ş', 's').Replace('Ş', 'S')
                .Replace('ü', 'u').Replace('Ü', 'U');
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;
using OpenCvSharp.Face;

namespace YMH219_FaceRecognition
{
    /// <summary>
    /// LBPH (Local Binary Patterns Histograms) algoritması ile yüz tanıma yapar.
    /// Yerel örneklerle eğitilir ve yeni bir yüz için etiket tahmin eder.
    /// </summary>
    public class FaceIdentifier : IDisposable
    {
        public const double DefaultThreshold = 80.0;

        private readonly List<string> labelNames = new List<string>();
        private LBPHFaceRecognizer? recognizer;

        /// <summary>Bu mesafenin üzerindeki tahminler "Bilinmiyor" sayılır.</summary>
        public double Threshold { get; set; } = DefaultThreshold;

        public bool IsTrained
        {
            get { return recognizer != null; }
        }

        /// <summary>
        /// Modeli verilen örneklerle eğitir. En az iki farklı etiket gerekir.
        /// </summary>
        public void Train(IReadOnlyList<FaceSample> samples)
        {
            List<string> names = samples.Select(s => s.Label).Distinct().OrderBy(n => n).ToList();
            if (names.Count < 2)
                throw new InvalidOperationException("Eğitim için en az iki farklı etiket gerekir.");

            // LBPH etiketleri sayı olarak ister: her ismin listedeki sırası onun numarasıdır.
            List<Mat> images = samples.Select(s => s.Image).ToList();
            List<int> labels = samples.Select(s => names.IndexOf(s.Label)).ToList();

            LBPHFaceRecognizer trained = LBPHFaceRecognizer.Create();
            trained.Train(images, labels);

            recognizer?.Dispose();
            recognizer = trained;

            labelNames.Clear();
            labelNames.AddRange(names);
        }

        /// <summary>
        /// Hazırlanmış (gri, sabit boyutlu) yüz görüntüsü için etiket ve güven skoru tahmin eder.
        /// </summary>
        public RecognitionResult Identify(Mat face)
        {
            if (recognizer == null)
                return RecognitionResult.DetectedOnly();

            recognizer.Predict(face, out int labelIndex, out double distance);

            if (labelIndex < 0 || labelIndex >= labelNames.Count)
                return RecognitionResult.Unknown(distance);

            return RecognitionResult.FromPrediction(labelNames[labelIndex], distance, Threshold);
        }

        public void Dispose()
        {
            recognizer?.Dispose();
            recognizer = null;
        }
    }
}

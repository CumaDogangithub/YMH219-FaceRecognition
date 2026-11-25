using System;

namespace YMH219_FaceRecognition
{
    /// <summary>
    /// Tek bir yüz için tanıma sonucunu tutar: etiket, LBPH mesafesi ve güven yüzdesi.
    /// </summary>
    public class RecognitionResult
    {
        public const string UnknownLabel = "Bilinmiyor";

        /// <summary>Tahmin edilen etiket (kişi adı) veya "Bilinmiyor".</summary>
        public string Label { get; }

        /// <summary>LBPH mesafesi. Küçük değer daha iyi eşleşme demektir.</summary>
        public double Distance { get; }

        /// <summary>Mesafe eşik değerinin altındaysa true.</summary>
        public bool IsKnown { get; }

        /// <summary>Model eğitilmemişse skor yoktur; yalnızca yüz algılanmıştır.</summary>
        public bool HasScore { get; }

        private RecognitionResult(string label, double distance, bool isKnown, bool hasScore)
        {
            Label = label;
            Distance = distance;
            IsKnown = isKnown;
            HasScore = hasScore;
        }

        /// <summary>Model eğitilmemişken kullanılan sonuç: yüz var ama etiket yok.</summary>
        public static RecognitionResult DetectedOnly()
        {
            return new RecognitionResult("Yüz", 0, false, false);
        }

        /// <summary>Eşleşme bulunamadığında kullanılan sonuç.</summary>
        public static RecognitionResult Unknown(double distance)
        {
            return new RecognitionResult(UnknownLabel, distance, false, true);
        }

        /// <summary>LBPH tahmininden sonuç üretir; mesafe eşiği aşarsa etiket "Bilinmiyor" olur.</summary>
        public static RecognitionResult FromPrediction(string label, double distance, double threshold)
        {
            if (distance > threshold)
                return Unknown(distance);

            return new RecognitionResult(label, distance, true, true);
        }

        /// <summary>
        /// LBPH mesafesini 0-100 arası kaba bir güven yüzdesine çevirir (100 - mesafe).
        /// </summary>
        public int ConfidencePercent
        {
            get { return (int)Math.Round(Math.Max(0, Math.Min(100, 100 - Distance))); }
        }

        public override string ToString()
        {
            if (!HasScore)
                return "Yüz algılandı (model eğitilmedi)";

            return string.Format("{0} - Güven: %{1} (mesafe {2:F1})", Label, ConfidencePercent, Distance);
        }
    }
}
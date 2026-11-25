using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenCvSharp;

namespace YMH219_FaceRecognition
{
    /// <summary>
    /// Eğitim örneklerini yerel diskte "dataset/etiket/" klasörlerinde saklar ve geri yükler.
    /// </summary>
    internal class FaceDataset
    {

        private const string ImageExtension = ".png";
        public string RootPath { get; }
        public FaceDataset(string rootPath)
        {
            RootPath = rootPath;
        }

        /// <summary>
        /// Etiket klasör adı olarak kullanıldığı için yalnızca harf, rakam, boşluk, '-' ve '_' kabul edilir.
        /// </summary>
        
        public static bool IsValidLabel(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                return false;
            }
            return label.All(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_');
        }

        /// <summary>
        /// Yüz görüntüsünü etiketin klasörüne kaydeder ve o etiketteki toplam örnek sayısını döndürür.
        /// </summary>
        
        public int SaveSample(string label,Mat faceImage)
        {
            string folder = Path.Combine(RootPath, label);
            Directory.CreateDirectory(folder);

            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + ImageExtension;

            //Görüntü bellekte PNG'ye çevrilip .NET ile yazılır.Böylece türkçe karakterli yollar sorun çıkarmaz.
            Cv2.ImEncode(ImageExtension, faceImage, out byte[] bytes);
            File.WriteAllBytes(Path.Combine(folder, fileName), bytes);

            return Directory.GetFiles(folder, "*" + ImageExtension).Length;
        }

        public SortedDictionary<string,int> GetLabelCounts()
        {
            var counts = new SortedDictionary<string, int>();
            if (!Directory.Exists(RootPath))
                return counts;
            foreach (string folder in Directory.GetDirectories(RootPath))
            {
                int count = Directory.GetFiles(folder, "*" + ImageExtension).Length;
                if (count > 0)
                    counts[Path.GetFileName(folder)] = count;
            }

            return counts;
        }

        /// <summary>
        /// Diskteki tüm örnekleri gri tonlamalı olarak yükler. Dönen örnekleri çağıran taraf Dispose etmelidir.
        /// </summary>
        
        public List<FaceSample> LoadSamples()
        {
            var samples = new List<FaceSample>();
            if (!Directory.Exists(RootPath))
                return samples;
            foreach (string folder in Directory.GetDirectories(RootPath))
            {
                string label = Path.GetFileName(folder);

                foreach (string file in Directory.GetFiles(folder, "*" + ImageExtension))
                {
                    Mat image = Cv2.ImDecode(File.ReadAllBytes(file), ImreadModes.Grayscale);
                    if (image.Empty())
                    {
                        image.Dispose();
                        continue;
                    }
                    samples.Add(new FaceSample(label, image));
                       
                }
            }
            return samples;
        }
    }
}

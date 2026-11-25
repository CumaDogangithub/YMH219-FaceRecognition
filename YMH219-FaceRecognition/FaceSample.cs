using System;
using OpenCvSharp;

namespace YMH219_FaceRecognition
{
    /// <summary>
    /// Eğitimde kullanılan tek bir örnek: gri tonlamalı yüz görüntüsü ve ait olduğu etiket.
    /// </summary>
    public class FaceSample : IDisposable
    {
        public string Label { get; }
        public Mat Image { get; }

        public FaceSample(string label, Mat image)
        {
            Label = label;
            Image = image;
        }

        public void Dispose()
        {
            Image.Dispose();
        }
    }
}

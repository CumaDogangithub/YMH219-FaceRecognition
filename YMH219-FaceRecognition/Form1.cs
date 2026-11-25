using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using CvSize = OpenCvSharp.Size;

namespace YMH219_FaceRecognition
{
    public partial class Form1 : Form
    {
        /// <summary>Seçilen büyük fotoğraflar işlem hızı için bu genişliğe küçültülür.</summary>
        private const int MaxImageWidth = 1280;

        private readonly CameraService camera = new CameraService();
        private readonly FaceDetector detector = new FaceDetector();
        private readonly FaceIdentifier identifier = new FaceIdentifier();
        private readonly FaceDataset dataset;

        /// <summary>Üzerine çizim yapılmamış son görüntü. Örnek eklerken bu kullanılır.</summary>
        private Mat? currentFrame;

        public Form1()
        {
            InitializeComponent();
            dataset = new FaceDataset(Path.Combine(AppContext.BaseDirectory, "dataset"));
        }

        // ------------------------------------------------------------------
        // Açılış ve kapanış
        // ------------------------------------------------------------------

        private void Form1_Load(object sender, EventArgs e)
        {
            nudThreshold.Value = (decimal)FaceIdentifier.DefaultThreshold;

            LoadDetectionModel();
            SetCameraButtons(false);
            TrainFromDataset(false);
            UpdateDatasetSummary();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cameraTimer.Stop();
            camera.Dispose();
            detector.Dispose();
            identifier.Dispose();
            currentFrame?.Dispose();

            Image? lastImage = picPreview.Image;
            picPreview.Image = null;
            lastImage?.Dispose();
        }

        /// <summary>
        /// Haar cascade model dosyasını yükler. Dosya yoksa uygulama kapanmaz;
        /// kullanıcı bilgilendirilir ve algılamaya bağlı düğmeler kapatılır.
        /// </summary>
        private void LoadDetectionModel()
        {
            string cascadePath = Path.Combine(AppContext.BaseDirectory, FaceDetector.CascadeFileName);

            try
            {
                detector.Load(cascadePath);
                SetStatus("Hazır. Kamerayı başlatın veya bir görüntü seçin.");
            }
            catch (Exception ex)
            {
                btnStartCamera.Enabled = false;
                btnSelectImage.Enabled = false;
                btnAddSample.Enabled = false;
                btnTrain.Enabled = false;

                SetStatus("Model dosyası yüklenemedi.");
                ShowError(
                    "Yüz algılama model dosyası yüklenemedi:" + Environment.NewLine +
                    cascadePath + Environment.NewLine + Environment.NewLine +
                    ex.Message + Environment.NewLine + Environment.NewLine +
                    "Dosyanın projede bulunduğundan ve 'Copy to Output Directory' ayarının " +
                    "'Copy if newer' olduğundan emin olun.");
            }
        }

        // ------------------------------------------------------------------
        // Kamera
        // ------------------------------------------------------------------

        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            // Kameranın açılması birkaç saniye sürebilir; kullanıcıya beklediğini gösteriyoruz.
            SetStatus("Kamera açılıyor...");
            statusStrip.Refresh();
            Cursor = Cursors.WaitCursor;

            try
            {
                camera.Start();
            }
            catch (Exception ex)
            {
                SetStatus("Kamera başlatılamadı.");
                ShowError("Kamera başlatılamadı." + Environment.NewLine + Environment.NewLine + ex.Message);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            cameraTimer.Start();
            SetCameraButtons(true);
            SetStatus("Kamera çalışıyor.");
        }

        private void btnStopCamera_Click(object sender, EventArgs e)
        {
            StopCamera();
            SetStatus("Kamera durduruldu.");
        }

        private void StopCamera()
        {
            cameraTimer.Stop();
            camera.Stop();
            SetCameraButtons(false);
        }

        /// <summary>Zamanlayıcı her çalıştığında kameradan bir kare okunur ve işlenir.</summary>
        private void cameraTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (Mat frame = new Mat())
                {
                    if (!camera.TryReadFrame(frame))
                    {
                        StopCamera();
                        SetStatus("Kamera bağlantısı kesildi.");
                        ShowError("Kameradan görüntü alınamadı. Kamera bağlantısını kontrol edin.");
                        return;
                    }

                    ProcessFrame(frame);
                }
            }
            catch (Exception ex)
            {
                StopCamera();
                SetStatus("Görüntü işlenirken hata oluştu.");
                ShowError("Görüntü işlenirken hata oluştu." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        // ------------------------------------------------------------------
        // Dosyadan görüntü
        // ------------------------------------------------------------------

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            if (camera.IsRunning)
                StopCamera();

            try
            {
                using (Mat image = LoadImage(openFileDialog.FileName))
                {
                    if (image.Empty())
                    {
                        ShowError("Seçilen dosya bir görüntü olarak okunamadı.");
                        return;
                    }

                    ProcessFrame(image);
                }

                SetStatus("Görüntü yüklendi: " + Path.GetFileName(openFileDialog.FileName));
            }
            catch (Exception ex)
            {
                SetStatus("Görüntü açılamadı.");
                ShowError("Görüntü açılamadı." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        /// <summary>
        /// Görüntü dosyasını okur; çok büyükse genişliği MaxImageWidth olacak şekilde küçültür.
        /// </summary>
        private static Mat LoadImage(string path)
        {
            Mat image = Cv2.ImDecode(File.ReadAllBytes(path), ImreadModes.Color);
            if (image.Empty() || image.Width <= MaxImageWidth)
                return image;

            double scale = (double)MaxImageWidth / image.Width;
            Mat resized = new Mat();
            Cv2.Resize(image, resized, new CvSize(), scale, scale);
            image.Dispose();
            return resized;
        }

        // ------------------------------------------------------------------
        // Algılama, tanıma ve gösterim
        // ------------------------------------------------------------------

        /// <summary>
        /// Bir görüntüyü işler: yüzleri bulur, tanır, dikdörtgenle işaretler ve ekranda gösterir.
        /// </summary>
        private void ProcessFrame(Mat frame)
        {
            Mat clean = frame.Clone();   // çizim yapılmamış kopya
            currentFrame?.Dispose();
            currentFrame = clean;

            Rect[] faces = detector.Detect(clean);
            var lines = new List<string>();

            for (int i = 0; i < faces.Length; i++)
            {
                RecognitionResult result;
                using (Mat face = detector.ExtractFace(clean, faces[i]))
                {
                    result = identifier.Identify(face);
                }

                FrameAnnotator.Draw(frame, faces[i], result);
                lines.Add((i + 1) + ") " + result);
            }

            ShowFrame(frame);
            lblFaceCount.Text = "Algılanan yüz: " + faces.Length;
            lblResults.Text = lines.Count > 0
                ? string.Join(Environment.NewLine, lines)
                : "Yüz algılanmadı.";
        }

        private void ShowFrame(Mat frame)
        {
            Image? oldImage = picPreview.Image;
            picPreview.Image = BitmapConverter.ToBitmap(frame);
            oldImage?.Dispose();   // eski Bitmap bellekte birikmesin
        }

        /// <summary>
        /// Kamera kapalıyken ekranda duran görüntüyü yeniden işler (eğitim veya eşik değişince).
        /// </summary>
        private void RefreshStillImage()
        {
            if (camera.IsRunning || currentFrame == null || !detector.IsLoaded)
                return;

            using (Mat copy = currentFrame.Clone())
            {
                ProcessFrame(copy);
            }
        }

        // ------------------------------------------------------------------
        // Örnek ekleme ve eğitim
        // ------------------------------------------------------------------

        private void btnAddSample_Click(object sender, EventArgs e)
        {
            string label = txtLabel.Text.Trim();

            if (!FaceDataset.IsValidLabel(label))
            {
                ShowWarning("Lütfen bir etiket (kişi adı) yazın. Yalnızca harf, rakam, boşluk, '-' ve '_' kullanılabilir.");
                txtLabel.Focus();
                return;
            }

            if (currentFrame == null)
            {
                ShowWarning("Önce kamerayı başlatın veya bir görüntü seçin.");
                return;
            }

            try
            {
                Rect[] faces = detector.Detect(currentFrame);
                if (faces.Length == 0)
                {
                    ShowWarning("Görüntüde yüz algılanamadı. Kameraya dönük ve iyi ışıkta tekrar deneyin.");
                    return;
                }

                int count;
                using (Mat face = detector.ExtractFace(currentFrame, faces[0]))   // en büyük yüz
                {
                    count = dataset.SaveSample(label, face);
                }

                UpdateDatasetSummary();
                SetStatus("Örnek kaydedildi: " + label + " (" + count + " örnek). Bitince 'Modeli Eğit' düğmesine basın.");
            }
            catch (Exception ex)
            {
                ShowError("Örnek kaydedilemedi." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            TrainFromDataset(true);
        }

        /// <summary>
        /// Diskteki örneklerle tanıma modelini eğitir. Açılışta sessizce, düğmeyle mesaj göstererek çalışır.
        /// </summary>
        private void TrainFromDataset(bool showMessages)
        {
            List<FaceSample> samples = new List<FaceSample>();

            try
            {
                samples = dataset.LoadSamples();
                int labelCount = samples.Select(s => s.Label).Distinct().Count();

                if (labelCount < 2)
                {
                    lblModel.Text = "Model: eğitilmedi (en az 2 etiket gerekli)";
                    if (showMessages)
                        ShowWarning("Eğitim için en az iki farklı etikete ait örnek gerekir. Şu anki etiket sayısı: " + labelCount);
                    return;
                }

                Cursor = Cursors.WaitCursor;
                identifier.Train(samples);

                lblModel.Text = "Model: eğitildi (" + labelCount + " etiket, " + samples.Count + " örnek)";
                if (showMessages)
                    SetStatus("Model eğitildi.");

                RefreshStillImage();
            }
            catch (Exception ex)
            {
                lblModel.Text = "Model: eğitilemedi";
                ShowError("Model eğitilemedi." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                foreach (FaceSample sample in samples)
                    sample.Dispose();
            }
        }

        private void nudThreshold_ValueChanged(object sender, EventArgs e)
        {
            identifier.Threshold = (double)nudThreshold.Value;
            RefreshStillImage();
        }

        private void UpdateDatasetSummary()
        {
            SortedDictionary<string, int> counts = dataset.GetLabelCounts();

            lblDataset.Text = counts.Count == 0
                ? "Henüz örnek yok."
                : string.Join(Environment.NewLine, counts.Select(c => c.Key + ": " + c.Value + " örnek"));
        }

        // ------------------------------------------------------------------
        // Arayüz yardımcıları
        // ------------------------------------------------------------------

        private void SetCameraButtons(bool cameraRunning)
        {
            // Model yüklenemediyse kamera hiç başlatılamaz.
            btnStartCamera.Enabled = !cameraRunning && detector.IsLoaded;
            btnStopCamera.Enabled = cameraRunning;
        }

        private void SetStatus(string message)
        {
            lblStatus.Text = message;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(this, message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
namespace YMH219_FaceRecognition
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            picPreview = new PictureBox();
            grpSource = new GroupBox();
            btnSelectImage = new Button();
            btnStopCamera = new Button();
            btnStartCamera = new Button();
            grpTraining = new GroupBox();
            lblModel = new Label();
            lblDataset = new Label();
            btnTrain = new Button();
            btnAddSample = new Button();
            txtLabel = new TextBox();
            lblLabelCaption = new Label();
            grpResult = new GroupBox();
            lblResults = new Label();
            lblFaceCount = new Label();
            nudThreshold = new NumericUpDown();
            lblThresholdCaption = new Label();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            cameraTimer = new System.Windows.Forms.Timer(components);
            openFileDialog = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
            grpSource.SuspendLayout();
            grpTraining.SuspendLayout();
            grpResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudThreshold).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            //
            // picPreview
            //
            picPreview.BackColor = Color.Black;
            picPreview.BorderStyle = BorderStyle.FixedSingle;
            picPreview.Location = new Point(12, 12);
            picPreview.Name = "picPreview";
            picPreview.Size = new Size(640, 480);
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.TabIndex = 0;
            picPreview.TabStop = false;
            //
            // grpSource
            //
            grpSource.Controls.Add(btnSelectImage);
            grpSource.Controls.Add(btnStopCamera);
            grpSource.Controls.Add(btnStartCamera);
            grpSource.Location = new Point(664, 12);
            grpSource.Name = "grpSource";
            grpSource.Size = new Size(324, 98);
            grpSource.TabIndex = 1;
            grpSource.TabStop = false;
            grpSource.Text = "Görüntü Kaynağı";
            //
            // btnSelectImage
            //
            btnSelectImage.Location = new Point(12, 60);
            btnSelectImage.Name = "btnSelectImage";
            btnSelectImage.Size = new Size(300, 28);
            btnSelectImage.TabIndex = 2;
            btnSelectImage.Text = "Görüntü Seç";
            btnSelectImage.UseVisualStyleBackColor = true;
            btnSelectImage.Click += btnSelectImage_Click;
            //
            // btnStopCamera
            //
            btnStopCamera.Location = new Point(166, 26);
            btnStopCamera.Name = "btnStopCamera";
            btnStopCamera.Size = new Size(146, 28);
            btnStopCamera.TabIndex = 1;
            btnStopCamera.Text = "Kamerayı Durdur";
            btnStopCamera.UseVisualStyleBackColor = true;
            btnStopCamera.Click += btnStopCamera_Click;
            //
            // btnStartCamera
            //
            btnStartCamera.Location = new Point(12, 26);
            btnStartCamera.Name = "btnStartCamera";
            btnStartCamera.Size = new Size(146, 28);
            btnStartCamera.TabIndex = 0;
            btnStartCamera.Text = "Kamerayı Başlat";
            btnStartCamera.UseVisualStyleBackColor = true;
            btnStartCamera.Click += btnStartCamera_Click;
            //
            // grpTraining
            //
            grpTraining.Controls.Add(lblModel);
            grpTraining.Controls.Add(lblDataset);
            grpTraining.Controls.Add(btnTrain);
            grpTraining.Controls.Add(btnAddSample);
            grpTraining.Controls.Add(txtLabel);
            grpTraining.Controls.Add(lblLabelCaption);
            grpTraining.Location = new Point(664, 116);
            grpTraining.Name = "grpTraining";
            grpTraining.Size = new Size(324, 200);
            grpTraining.TabIndex = 2;
            grpTraining.TabStop = false;
            grpTraining.Text = "Örnek Ekleme ve Eğitim";
            //
            // lblModel
            //
            lblModel.Location = new Point(12, 172);
            lblModel.Name = "lblModel";
            lblModel.Size = new Size(300, 20);
            lblModel.TabIndex = 5;
            lblModel.Text = "Model: eğitilmedi";
            //
            // lblDataset
            //
            lblDataset.Location = new Point(12, 94);
            lblDataset.Name = "lblDataset";
            lblDataset.Size = new Size(300, 74);
            lblDataset.TabIndex = 4;
            lblDataset.Text = "Henüz örnek yok.";
            //
            // btnTrain
            //
            btnTrain.Location = new Point(166, 58);
            btnTrain.Name = "btnTrain";
            btnTrain.Size = new Size(146, 28);
            btnTrain.TabIndex = 3;
            btnTrain.Text = "Modeli Eğit";
            btnTrain.UseVisualStyleBackColor = true;
            btnTrain.Click += btnTrain_Click;
            //
            // btnAddSample
            //
            btnAddSample.Location = new Point(12, 58);
            btnAddSample.Name = "btnAddSample";
            btnAddSample.Size = new Size(146, 28);
            btnAddSample.TabIndex = 2;
            btnAddSample.Text = "Örnek Ekle";
            btnAddSample.UseVisualStyleBackColor = true;
            btnAddSample.Click += btnAddSample_Click;
            //
            // txtLabel
            //
            txtLabel.Location = new Point(126, 26);
            txtLabel.MaxLength = 30;
            txtLabel.Name = "txtLabel";
            txtLabel.Size = new Size(186, 23);
            txtLabel.TabIndex = 1;
            //
            // lblLabelCaption
            //
            lblLabelCaption.AutoSize = true;
            lblLabelCaption.Location = new Point(12, 29);
            lblLabelCaption.Name = "lblLabelCaption";
            lblLabelCaption.Size = new Size(96, 15);
            lblLabelCaption.TabIndex = 0;
            lblLabelCaption.Text = "Etiket (kişi adı):";
            //
            // grpResult
            //
            grpResult.Controls.Add(lblResults);
            grpResult.Controls.Add(lblFaceCount);
            grpResult.Controls.Add(nudThreshold);
            grpResult.Controls.Add(lblThresholdCaption);
            grpResult.Location = new Point(664, 322);
            grpResult.Name = "grpResult";
            grpResult.Size = new Size(324, 170);
            grpResult.TabIndex = 3;
            grpResult.TabStop = false;
            grpResult.Text = "Tanıma Sonucu";
            //
            // lblResults
            //
            lblResults.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResults.Location = new Point(12, 80);
            lblResults.Name = "lblResults";
            lblResults.Size = new Size(300, 82);
            lblResults.TabIndex = 3;
            lblResults.Text = "Yüz algılanmadı.";
            //
            // lblFaceCount
            //
            lblFaceCount.AutoSize = true;
            lblFaceCount.Location = new Point(12, 58);
            lblFaceCount.Name = "lblFaceCount";
            lblFaceCount.Size = new Size(92, 15);
            lblFaceCount.TabIndex = 2;
            lblFaceCount.Text = "Algılanan yüz: 0";
            //
            // nudThreshold
            //
            nudThreshold.Location = new Point(126, 26);
            nudThreshold.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            nudThreshold.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            nudThreshold.Name = "nudThreshold";
            nudThreshold.Size = new Size(70, 23);
            nudThreshold.TabIndex = 1;
            nudThreshold.Value = new decimal(new int[] { 80, 0, 0, 0 });
            nudThreshold.ValueChanged += nudThreshold_ValueChanged;
            //
            // lblThresholdCaption
            //
            lblThresholdCaption.AutoSize = true;
            lblThresholdCaption.Location = new Point(12, 29);
            lblThresholdCaption.Name = "lblThresholdCaption";
            lblThresholdCaption.Size = new Size(83, 15);
            lblThresholdCaption.TabIndex = 0;
            lblThresholdCaption.Text = "Eşik (mesafe):";
            //
            // statusStrip
            //
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Location = new Point(0, 504);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1000, 22);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 4;
            statusStrip.Text = "statusStrip";
            //
            // lblStatus
            //
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(34, 17);
            lblStatus.Text = "Hazır";
            //
            // cameraTimer
            //
            cameraTimer.Interval = 40;
            cameraTimer.Tick += cameraTimer_Tick;
            //
            // openFileDialog
            //
            openFileDialog.Filter = "Görüntü dosyaları|*.jpg;*.jpeg;*.png;*.bmp|Tüm dosyalar|*.*";
            openFileDialog.Title = "Görüntü Seç";
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 526);
            Controls.Add(statusStrip);
            Controls.Add(grpResult);
            Controls.Add(grpTraining);
            Controls.Add(grpSource);
            Controls.Add(picPreview);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YMH219 - Yüz Algılama ve Tanıma";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
            grpSource.ResumeLayout(false);
            grpTraining.ResumeLayout(false);
            grpTraining.PerformLayout();
            grpResult.ResumeLayout(false);
            grpResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudThreshold).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picPreview;
        private GroupBox grpSource;
        private Button btnSelectImage;
        private Button btnStopCamera;
        private Button btnStartCamera;
        private GroupBox grpTraining;
        private Label lblModel;
        private Label lblDataset;
        private Button btnTrain;
        private Button btnAddSample;
        private TextBox txtLabel;
        private Label lblLabelCaption;
        private GroupBox grpResult;
        private Label lblResults;
        private Label lblFaceCount;
        private NumericUpDown nudThreshold;
        private Label lblThresholdCaption;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Timer cameraTimer;
        private OpenFileDialog openFileDialog;
    }
}

namespace Happy
{
    partial class PhotoViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhotoViewer));
            this.pbox = new System.Windows.Forms.PictureBox();
            this.panel_photo = new System.Windows.Forms.Panel();
            this.panel_video = new System.Windows.Forms.Panel();
            this.videoPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.runner = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).BeginInit();
            this.panel_photo.SuspendLayout();
            this.panel_video.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // pbox
            // 
            this.pbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbox.Location = new System.Drawing.Point(0, 0);
            this.pbox.Margin = new System.Windows.Forms.Padding(0);
            this.pbox.Name = "pbox";
            this.pbox.Size = new System.Drawing.Size(1000, 1000);
            this.pbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbox.TabIndex = 1;
            this.pbox.TabStop = false;
            this.pbox.WaitOnLoad = true;
            this.pbox.Click += new System.EventHandler(this.Pbox_Click);
            // 
            // panel_photo
            // 
            this.panel_photo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_photo.Controls.Add(this.pbox);
            this.panel_photo.Location = new System.Drawing.Point(0, 0);
            this.panel_photo.Name = "panel_photo";
            this.panel_photo.Size = new System.Drawing.Size(1000, 1000);
            this.panel_photo.TabIndex = 2;
            this.panel_photo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Panel_photo_MouseClick);
            // 
            // panel_video
            // 
            this.panel_video.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_video.Controls.Add(this.videoPlayer);
            this.panel_video.Location = new System.Drawing.Point(0, 0);
            this.panel_video.Margin = new System.Windows.Forms.Padding(0);
            this.panel_video.Name = "panel_video";
            this.panel_video.Size = new System.Drawing.Size(1000, 1000);
            this.panel_video.TabIndex = 2;
            // 
            // videoPlayer
            // 
            this.videoPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoPlayer.Enabled = true;
            this.videoPlayer.Location = new System.Drawing.Point(0, 0);
            this.videoPlayer.Margin = new System.Windows.Forms.Padding(0);
            this.videoPlayer.Name = "videoPlayer";
            this.videoPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("videoPlayer.OcxState")));
            this.videoPlayer.Size = new System.Drawing.Size(977, 945);
            this.videoPlayer.TabIndex = 0;
            this.videoPlayer.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(this.VideoPlayer_MediaError);
            // 
            // runner
            // 
            this.runner.WorkerReportsProgress = true;
            this.runner.WorkerSupportsCancellation = true;
            this.runner.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Runner_DoWork);
            this.runner.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Runner_ProgressChanged);
            this.runner.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Runner_RunWorkerCompleted);
            // 
            // PhotoViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 944);
            this.Controls.Add(this.panel_photo);
            this.Controls.Add(this.panel_video);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PhotoViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PhotoViewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PhotoViewer_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PhotoViewer_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbox)).EndInit();
            this.panel_photo.ResumeLayout(false);
            this.panel_video.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.videoPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbox;
        private System.Windows.Forms.Panel panel_photo;
        private System.Windows.Forms.Panel panel_video;
        private AxWMPLib.AxWindowsMediaPlayer videoPlayer;
        private System.ComponentModel.BackgroundWorker runner;
    }
}
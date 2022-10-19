using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WMPLib;

namespace Happy
{
    public partial class PhotoViewer : Form
    {
        public string tmpPath;
        public bool playVideo = false;
        public bool authorize;
        public Dictionary<string, Object> dec;
        public byte[] finalObj;
        public bool v_mode;
        public bool isActivated = false;
        Load loadform;
        SecurityService service;

        public PhotoViewer(int index)
        {
            InitializeComponent();
            global.photoIndex = index;
            anlyze();

        }

        private void PhotoViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.playVideo == true) return;
            if (e.KeyCode == Keys.Right)
            {
                int endCount = global.happySecurityModelList.Count - 1;
                if (global.photoIndex >= endCount)
                {

                }
                else
                {
                    global.photoIndex++;
                    anlyze();
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                int endCount = global.happySecurityModelList.Count - 1;

                if (global.photoIndex == 0)
                {

                }
                else
                {
                    global.photoIndex--;
                    anlyze();
                }
            }
        }
        public void resize(Image img)
        {
            int[] size = Util.resizeImageMeasure(img.Width, img.Height, 1000, 1000);
            this.Height = size[1] + 10;
            this.Width = size[0] + 10;
            this.panel_photo.Width = size[0];
            this.panel_photo.Height = size[1];

            this.pbox.Height = size[1];
            this.pbox.Width = size[0];
        }

        public void loadPhoto()
        {
            this.v_mode = false;
            this.playVideo = false;
            this.panel_photo.BringToFront();
            this.Text = global.happySecurityModelList[global.photoIndex].Name;
            service = new SecurityService();
            if (service.isAuthorize(global.happySecurityModelList[global.photoIndex]) == false)
            {
                MessageBox.Show("You are not authorized", "ERROR");
                this.authorize = false;
                return;
            }
            this.authorize = true;
            dec = new Dictionary<string, object>();
            dec.Add("type", "p");
            dec.Add("object", global.happySecurityModelList[global.photoIndex]);

            this.runner.RunWorkerAsync();

        }
        public void loadVideo()
        {
            if (global.authorize == false)
            {
                MessageBox.Show("Not authorized", "Error");
                this.authorize = false;
                return;
            }
            this.v_mode = true;
            this.authorize = true;
            this.playVideo = true;
            this.panel_video.BringToFront();
            this.Height = 500;
            this.Width = 500;
            this.tmpPath = global.tempPath;
            service = new SecurityService();

            dec = new Dictionary<string, object>();
            dec.Add("type", "v");
            dec.Add("object", global.happySecurityModelList[global.photoIndex]);
            loadform = new Load();
            loadform.Show();
            loadform.TopMost = true;
            this.runner.RunWorkerAsync();
        }
        public void anlyze()
        {
            HappySecurityModel obj = global.happySecurityModelList[global.photoIndex];
            if (obj.Type == "mp4" || obj.Type == "MP4")
            {
                loadVideo();
            }
            else if (obj.Type == "jpg" || obj.Type == "JPG")
            {
                loadPhoto();
            }
            else if (obj.Type == "JPEG" || obj.Type == "jpeg")
            {
                loadPhoto();
            }
            else if (obj.Type == "png" || obj.Type == "PNG")
            {
                loadPhoto();
            }
            else
            {

            }
        }

        private void VideoPlayer_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            // If the Player encounters a corrupt or missing file, 
            // show the hexadecimal error code and URL.
            {
                IWMPMedia2 errSource = e.pMediaObject as IWMPMedia2;
                IWMPErrorItem errorItem = errSource.Error;
                MessageBox.Show("Error " + errorItem.errorCode.ToString("X")
                                + " in " + errSource.sourceURL);
            }
            catch (InvalidCastException)
            // In case pMediaObject is not an IWMPMedia item.
            {
                MessageBox.Show("Error.");
            }
        }

        private void PhotoViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.videoPlayer.Ctlcontrols.stop();
            if (File.Exists(this.tmpPath))
            {
               // File.Delete(this.tmpPath);
            }

        }

        private void Panel_photo_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Pbox_Click(object sender, EventArgs e)
        {
            if (this.playVideo == true) return;

            int endCount = global.happySecurityModelList.Count - 1;
            if (global.photoIndex >= endCount)
            {

            }
            else
            {
                global.photoIndex++;
                anlyze();

            }
        }

        private void Runner_DoWork(object sender, DoWorkEventArgs e)
        {
            this.finalObj = service.decryptToView(dec);
        }

        private void Runner_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void Runner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.v_mode == true)
            {
                this.loadform.Visible = false;
                this.videoPlayer.URL = tmpPath;
            }
            else
            {
                if (this.finalObj != null)
                {
                    Image img = Util.byteArrayToImage(this.finalObj);
                    resize(img);
                    this.pbox.Image = img;
                }
            }
            if (this.isActivated == false)
            {
                this.Show();
                this.isActivated = true;
            }

        }
    }
}

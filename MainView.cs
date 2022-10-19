using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Happy
{
    public partial class MainView : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private string rootPath_register;
        private int splashConter = 0;
        private SecurityService securityService;

        private Load loadingForm = new Load();
        private bool galleryListView = false;
        private bool encrypt;
        private string pathEncryption = "";

        public void test()
        {
          //  Util.videoThumbnail("C:\\Users\\KhineMyae\\Downloads\\Video\\pp.mp4");
        }
        public MainView()
        {
           // test();
            InitializeComponent();
            buttonOnOff(false);
            this.timerSplash.Enabled = true;
            this.panel_splash.BringToFront();

            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
        }
        public void init()
        {
            this.tb_password.Text = string.Empty;
            this.tb_id.Text = string.Empty;
            this.securityService = new SecurityService();
            this.panel_login.BringToFront();
            this.lblTitle_header.Text = "Login";
            this.tb_password.Focus();

        }


        #region"panel_header"
        private void BtnReLogin_header_Click(object sender, EventArgs e)
        {
            buttonOnOff(false);
            init();
        }
        private void buttonOnOff(bool b)
        {
            this.btn_menu_header.Visible = b;
            this.btnReLogin_header.Visible = b;

        }
        private void Btn_close_header_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Btn_menu_header_Click(object sender, EventArgs e)
        {
            this.panel_category.BringToFront();
            this.lblTitle_header.Text = "Menu";
        }

        private void Panel_header_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

        #region "panel_login"
        private void Tb_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginClick();
            }

        }
        private void Btn_login_Click(object sender, EventArgs e)
        {
            loginClick();

        }
        private void loginClick()
        {
            if (this.securityService.login(this.tb_password.Text, this.tb_id.Text))
            {
                buttonOnOff(true);
                this.panel_category.BringToFront();
                this.lblTitle_header.Text = "Menu";
            }
            else
            {
                return;
            }
        }
        #endregion

        #region"panel category"
        private void BtnSetting_category_Click(object sender, EventArgs e)
        {
            this.gb_reenterpass.BringToFront();
            this.panel_inputBox.BringToFront();
            this.tb_password_setting.Enabled = false;
            this.btnChangePass_setting.Enabled = false;
            this.tb_pass_gb_reenterpass.Text = "";
            this.lblTitle_header.Text = "Setting";
        }
        private void Btn_security_category_Click(object sender, EventArgs e)
        {
            this.panel_security.BringToFront();
            this.panel_security_view.BringToFront();
            this.panel_security_view_main.BringToFront();
            this.lblTitle_header.Text = "Security";
            this.checkBox_Original.Checked = false;
            this.progressBar_imageLoad.Visible = false;
            this.rdIconView_security_view_main.Visible = false;
            this.rdListView_security_main_view.Visible = false;
            this.btnBackPage_security_view_main.Visible = false;
            this.btnForwardPage_security_view_main.Visible = false;
            if (this.dgvGallery_security_view_main.Rows.Count != 0)
            {
                this.dgvGallery_security_view_main.Rows.Clear();
            }
            this.panel_iconView.BringToFront();
            global.itemList = null;
            this.galleryListView = false;
            this.dgvGallery_security_view_main.Visible = true;
            this.dgvGalleryViewList__security_view_main.Visible = false;
            loadFolder();
        }


        #endregion

        #region"panel security"
        #region"panel security view"
        #region"panel security view view"
        private void BtnBack_panel_security_view_view_Click(object sender, EventArgs e)
        {
            this.panel_security_view_main.BringToFront();
        }

        #endregion
        #region"panel security view main"
        private void RdIconView_security_view_main_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdListView_security_main_view.Checked)
            {
                this.dgvGalleryViewList__security_view_main.Visible = true;
                this.dgvGallery_security_view_main.Visible = false;
                this.panel_listView.BringToFront();

                this.panel_loading.BringToFront();
                this.worker_listView.RunWorkerAsync();
            }
            else
            {
                if (global.itemList == null) return;
                this.panel_loading.BringToFront();
                this.panel_iconView.BringToFront();
                this.dgvGalleryViewList__security_view_main.Visible = false;
                this.dgvGallery_security_view_main.Visible = true;
                global.from = 0;
                this.worker_initializing.RunWorkerAsync();

            }
        }
        private void DgvGallery_security_view_main_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rindex = e.RowIndex;
            if (rindex == -1) return;
            global.photoIndex = rindex;
            PhotoViewer photoViewer = new PhotoViewer(global.photoIndex);

        }
        private void BtnAdd_security_view_main_Click(object sender, EventArgs e)
        {
            this.panel_security_add.BringToFront();
            this.panel_enc_1.BringToFront();
        }

        private void loadFolder()
        {

            try
            {
                global.folderList = Directory.GetDirectories(global.rootFilePath);
                this.panel_loading.BringToFront();
                this.cbFolderList_security_view_main.Items.Clear();
                foreach (string folder in global.folderList)
                {
                    this.cbFolderList_security_view_main.Items.Add(Util.GetFileName(folder));
                }
                this.panel_security.BringToFront();
                this.panel_security_view.BringToFront();
                this.panel_security_view_main.BringToFront();


            }
            catch (Exception eg)
            {
                MessageBox.Show("Error occur.Restarting");
                init();
            }
        }

        private void CbFolderList_security_view_main_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                global.from = 0;
                int folderIndex = this.cbFolderList_security_view_main.SelectedIndex;
                global.currentFolderIndex = folderIndex;
                Util.regroupFile(Directory.GetFiles(global.folderList[folderIndex]));

                global.currentFolderName = Util.GetFileName(global.folderList[folderIndex]);
                this.encrypt = false;
                this.panel_loading.BringToFront();
                this.panel_iconView.BringToFront();
                this.worker_initializing.RunWorkerAsync();
            }
            catch (FileNotFoundException fnfe)
            {
                MessageBox.Show("folder missing" + fnfe.Message);
            }

        }

        private void loadList()
        {
            if (global.happySecurityModelList == null) return;
            this.dgvGallery_security_view_main.Rows.Clear();
            this.worker_imageLoad.RunWorkerAsync();
        }

        private void BtnExport_security_view_main_Click(object sender, EventArgs e)
        {
            try
            {
                string exportPath = Util.directoryChooser();
                if (exportPath == null) return;
                Directory.CreateDirectory(exportPath + "\\" + global.currentFolderName);
                securityService.exportPath = exportPath + "\\" + global.currentFolderName;
                this.panel_loading.BringToFront();
                this.worker_folderReading.RunWorkerAsync();
            }
            catch (Exception exportExc)
            {
                throw exportExc;
            }
        }

        private void CheckBox_Original_CheckedChanged(object sender, EventArgs e)
        {
            this.panel_loading.BringToFront();
            loadList();
            this.panel_security.BringToFront();
            this.panel_security_view.BringToFront();
            this.panel_security_view_main.BringToFront();
        }

        private void BtnBackPage_security_view_main_Click(object sender, EventArgs e)
        {
            global.from = global.from - 30;
            this.panel_loading.BringToFront();
            this.worker_initializing.RunWorkerAsync();
        }

        private void BtnForwardPage_security_view_main_Click(object sender, EventArgs e)
        {
            global.from = global.to + 1;
            this.panel_loading.BringToFront();
            this.worker_initializing.RunWorkerAsync();
        }
        private void changeView()
        {
            this.dgvGallery_security_view_main.Visible = false;
            this.dgvGalleryViewList__security_view_main.Visible = true;
            this.worker_imageLoad.RunWorkerAsync();

        }

        private void BtnAddItem_security_view_main_Click(object sender, EventArgs e)
        {
            string[] items = Util.fileChooser(true, global.filter_alltype);
            if (items == null) return;
            this.pathEncryption = global.folderList[global.currentFolderIndex];
            global.itemList = items.ToList<string>();
            this.panel_loading.BringToFront();
            buttonOnOff(false);
            this.worker_encryption.RunWorkerAsync();
        }
        #endregion
        #endregion
        #region"panel security add"
        private void BtnBack_security_add_Click(object sender, EventArgs e)
        {
            this.panel_security_view.BringToFront();
            this.panel_security_view_main.BringToFront();
            this.panel_enc_1.BringToFront();
        }
        private void BtnChoose_enc_Click(object sender, EventArgs e)
        {
            string[] listItems = Util.fileChooser(true, global.filter_alltype);
            if (listItems != null)
            {
                global.itemList = listItems.ToList<string>();

                this.txtBoxPassword.Text = global.loginPassword;
                this.txtBoxPassword.Enabled = false;
                this.panel_enc_3.BringToFront();
            }
        }
        private void BtnCencel_security_add_Click(object sender, EventArgs e)
        {
            this.panel_enc_1.BringToFront();
        }
        private void BtnProcess_security_add_Click(object sender, EventArgs e)
        {
            this.panel_inputBox.BringToFront();
            this.tb_title.Focus();
            this.gb_title.BringToFront();
        }

        #endregion

        #endregion

        #region"panel inputBox"
        private void BtnOk_gbReenterpass_Click(object sender, EventArgs e)
        {
            string pass = this.tb_pass_gb_reenterpass.Text;
            if (pass != global.loginPassword) return;
            this.tb_pass_gb_reenterpass.Text = "";
            lbl_rootPath_setting.Text = File.ReadAllLines(global.SYSTEM_INI)[2];
            this.panel_setting.BringToFront();
        }
        private void BtnOk_gbT_title_Click(object sender, EventArgs e)
        {
            if (this.tb_title.Text != "")
            {
                global.title_inputBox = this.tb_title.Text;
                this.panel_loading.BringToFront();
                this.encrypt = true;
                this.pathEncryption = global.rootFilePath + "\\" + global.title_inputBox;
                if (!Directory.Exists(this.pathEncryption))
                    Directory.CreateDirectory(this.pathEncryption);
                this.worker_encryption.RunWorkerAsync();
            }
        }
        #endregion

        #region"panel register"
        private void BtnRegister_register_Click(object sender, EventArgs e)
        {
            if (this.tbPassword_register.Text.Equals(this.tbRePassword_register.Text))
            {
                if (this.rootPath_register != null && this.rootPath_register != "")
                {
                    RegisterModel rm = new RegisterModel();
                    rm.ID = this.tbId_register.Text;
                    rm.Password = this.tbPassword_register.Text;
                    rm.RootPath = this.rootPath_register;

                    if (Util.register(rm))
                    {
                        init();
                    }
                }

            }
            else
            {
                MessageBox.Show("Not match password", "");

            }

        }

        private void BtnChooseRootPath_register_Click(object sender, EventArgs e)
        {
            this.rootPath_register = Util.directoryChooser();
        }
        #endregion

        #region"setting"
        private void BtnPassChange_setting_Click(object sender, EventArgs e)
        {
            this.tb_password_setting.Enabled = true;
            this.btnChangePass_setting.Enabled = true;
        }
        private void BtnChangePass_setting_Click(object sender, EventArgs e)
        {
            if (securityService.changePassword(this.tb_password_setting.Text))
            {
                this.tb_password_setting.Text = "";
                this.tb_password_setting.Enabled = false;
                this.btnChangePass_setting.Enabled = false;
                MessageBox.Show("Success");
                init();
            }
            else
            {
                MessageBox.Show("Fail");
            }
        }
        private void BtnRootPathChange_setting_Click(object sender, EventArgs e)
        {
            string path = Util.directoryChooser();
            if (securityService.changeRootPath(path))
            {
                MessageBox.Show("Success");
                init();
            }
        }
        #endregion
        #region"worker encryption"
        private void Worker_encryption_DoWork(object sender, DoWorkEventArgs e)
        {

            global.errorFiles = new List<Dictionary<string,string>>();
            int count = 1;

            for (int i = 0; i < global.itemList.Count; i++)
            {

                securityService.encrypt(global.itemList[i], this.pathEncryption);

                this.worker_encryption.ReportProgress(count);
                count++;
            }

        }

        private void Worker_encryption_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblMessage_loading.Text = e.ProgressPercentage + " of " + global.itemList.Count;
        }

        private void Worker_encryption_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (global.errorFiles.Count != 0)
            {
                string msg = "";
                if (global.errorFiles.Count > 0 || global.errorFiles != null)
                {
                    foreach (Dictionary<string, string> er in global.errorFiles)
                    {
                        msg += er["NAME"] + "<>" + er["ERMSG"] + "\n";
                    }
                    MessageBox.Show("skip " + msg);
                }
            }
            else
            {
                //if(!this.viewImage)
                MessageBox.Show("Success", "");
                this.lblMessage_loading.Text = "";
            }
            buttonOnOff(true);
            this.panel_security.BringToFront();
            this.panel_security_view.BringToFront();
            this.panel_security_view_main.BringToFront();

            loadFolder();


        }
        #endregion;

        #region"worker Decrypting"

        private void Worker_initializing_DoWork(object sender, DoWorkEventArgs e)
        {
            int limit = 1;

            global.errorFiles = new List<Dictionary<string,string>>();
            global.happySecurityModelList = new List<HappySecurityModel>();
            for (int i = global.from; i < global.itemList.Count; i++)
            {
                securityService.decrypt(global.itemList[i]);
                this.worker_initializing.ReportProgress(i + 1);
                global.to = i;
                if (limit == 30) break;
                limit++;
            }

        }

        private void Worker_initializing_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblMessage_loading.Text = "decrypting " + e.ProgressPercentage;
        }

        private void Worker_initializing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar_imageLoad.Visible = true;
            this.progressBar_imageLoad.Value = 0;

            this.lblImageCount.Text = (global.from + 1) + " to " + (global.to + 1) + " of ( total - " + (global.itemList.Count) + " )";
            if (global.from == 0) this.btnBackPage_security_view_main.Visible = false;
            else this.btnBackPage_security_view_main.Visible = true;

            if (global.to == (global.itemList.Count - 1)) this.btnForwardPage_security_view_main.Visible = false;
            else this.btnForwardPage_security_view_main.Visible = true;
            loadList();

        }

        private void Worker_listView_DoWork(object sender, DoWorkEventArgs e)
        {
            if (global.itemList == null) return;
            System.GC.Collect();
            global.happySecurityModelList = new List<HappySecurityModel>();
            foreach (string f in global.itemList)
            {
                securityService.decrypt(f);
            }
        }

        private void Worker_listView_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void Worker_listView_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (global.happySecurityModelList == null) goto Skip;
            this.dgvGalleryViewList__security_view_main.Rows.Clear();
            int no = 0;
            foreach (HappySecurityModel obj in global.happySecurityModelList)
            {
                this.dgvGalleryViewList__security_view_main.Rows.Add(no++, obj.Name, obj.Type);
            }
        Skip:
            this.panel_security.BringToFront();
            this.panel_security_view.BringToFront();
            this.panel_security_view_main.BringToFront();
        }

        private void Worker_imageLoad_DoWork(object sender, DoWorkEventArgs e)
        {
        Error:
            int index = 0;
            try
            {
                int size = 0;
                int size1 = 0;
                global.imageList = new List<Image>();
                global.imagethumbnailList = new List<Image>();
                System.GC.Collect();
                Size s = new Size();
                s.Width = 150;
                s.Height = 150;
                for (int i = index; i < global.happySecurityModelList.Count; i++)
                {

                    if (this.galleryListView == false)
                    {
                        global.imagethumbnailList.Add(Util.ResizeBitmapOnWhiteCanvas(new Bitmap(global.happySecurityModelList[i].Thumbnail), s, this.checkBox_Original.Checked));
                    }
                    else
                    {
                        global.imageNameList.Add(global.happySecurityModelList[i].Name);
                    }
                    int percentage = (i + 1) * 100 / global.happySecurityModelList.Count;

                    this.worker_imageLoad.ReportProgress(percentage);

                    index++;
                }
            }
            catch (Exception exce)
            {
                if (exce is OutOfMemoryException)
                {
                    this.galleryListView = true;
                    global.imageNameList = new List<string>();
                    changeView();
                }
                if (exce is ArgumentException)
                {
                    throw exce;
                    //index = index + 1;
                    //goto Error;
                }


            }
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {

                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void Worker_imageLoad_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar_imageLoad.Value = e.ProgressPercentage;
            this.lblMessage_loading.Text = "loading images and files " + e.ProgressPercentage;
        }

        private void Worker_imageLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            int idex = global.from + 1;
            if (this.galleryListView == false)
            {
                for (int i=0;i <global.imagethumbnailList.Count;i++)
                {
                    this.dgvGallery_security_view_main.Rows.Add(idex,global.happySecurityModelList[i].Type, global.imagethumbnailList[i]);
                    idex++;
                }
            }
            else
            {
                foreach (string name in global.imageNameList)
                {
                    this.dgvGalleryViewList__security_view_main.Rows.Add(name);
                }
            }
            this.progressBar_imageLoad.Visible = false;
            this.panel_security.BringToFront();
            this.panel_security_view.BringToFront();
            this.panel_security_view_main.BringToFront();

        }
        #endregion

        #region"worker export"

        private void Worker_folderReading_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            foreach (string file in global.itemList)
            {
                securityService.export(file);

                this.worker_folderReading.ReportProgress(i);
                i++;
            }

        }

        private void Worker_folderReading_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lblMessage_loading.Text = e.ProgressPercentage + " of " + global.itemList.Count;
        }

        private void Worker_folderReading_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.panel_security.BringToFront();
            this.panel_security_view.BringToFront();
            this.panel_security_view_main.BringToFront();
            string msg = "" ;
            if (global.errorFiles.Count > 0 || global.errorFiles != null)
            {
                foreach(Dictionary<string,string> er in global.errorFiles)
                {
                    msg += er["NAME"]+"<>"+ er["ERMSG"]+"\n";
                }
                MessageBox.Show("skip " + msg);
            }
        }



        #endregion

        private void TimerSplash_Tick(object sender, EventArgs e)
        {
            if (this.splashConter > 2)
            {
                this.timerSplash.Enabled = false;
               // this.pb_splash.Image = global::Happy.Properties.Resources.splash;
                if (Util.checkRegister())
                {
                    init();
                }
                else
                {
                    this.lblTitle_header.Text = "Register";
                    this.panel_Register.BringToFront();
                }
            }
            else
            {
                this.splashConter++;
            }
        }

        private void Pb_loading_Click(object sender, EventArgs e)
        {

        }

        private void Panel_loading_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DeleteTempFile_DoWork(object sender, DoWorkEventArgs e)
        {
            if (File.Exists(global.tempPath))
            {
                 File.Delete(global.tempPath);
            }
        }

        private void MainView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.deleteTempFile.RunWorkerAsync();
        }

        private void label_info_Click(object sender, EventArgs e)
        {

        }
    }
}

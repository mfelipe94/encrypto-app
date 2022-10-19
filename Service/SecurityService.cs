using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Converters;

namespace Happy
{
    public class SecurityService
    {
        public EncryptionService encService = new EncryptionService();
        public HappySecurityModel encObj;
        public string exportPath = "";

        public bool changePassword(string changePass)
        {

            string[] data = File.ReadAllLines(global.SYSTEM_INI);
            this.encService = new EncryptionService();
            string encPass = encService.EncryptPass(changePass);
            data[1] = encPass;
            File.WriteAllText(global.SYSTEM_INI, "");
            return Util.writeTxtToFile(global.SYSTEM_INI, data, true);


        }

        public bool changeRootPath(string path)
        {
            if (path == null) return false;
            string newPath = "";
            if (path.Contains(global.TITLE))
            {
                newPath = path.Replace("\\" + global.TITLE, "");
            }
            else
            {
                newPath = path;
            }
            string[] data = File.ReadAllLines(global.SYSTEM_INI);
            data[2] = newPath;
            File.WriteAllText(global.SYSTEM_INI, "");
            return Util.writeTxtToFile(global.SYSTEM_INI, data, true);
        }
        public bool login(string pass_tb, string id)
        {
            string[] data = File.ReadAllLines(global.SYSTEM_INI);
            EncryptionService enc = new EncryptionService();
            global.loginPassword = pass_tb;
            global.loginId = id;


            if (pass_tb == enc.DecryptPass(data[1]))
            {
                global.rootPath = data[2];
                global.rootFilePath = data[2] + "\\" + global.TITLE + "\\" + global.FILE;
                return Util.checkPath(data);
            }
            else
            {
                if (adminMode() == true)
                {

                    global.rootPath = data[2];
                    global.rootFilePath = data[2] + "\\" + global.TITLE + "\\" + global.FILE;
                    return Util.checkPath(data);
                }
                return false;
            }
        }

        public bool encrypt(string file, string path)
        {
            int count = 3;
            HappySecurityModel detail = new HappySecurityModel();
            string name = Util.redamName();
            try
            {
                encService = new EncryptionService();
                string type = Util.getFileType(file);   
                string fileName = Util.GetFileName(file);        
                encService.Encryption(file, path + "\\" + name);
                count = 1;
               
                    detail.Name = encService.EncryptPass(fileName);
                    detail.id = encService.EncryptPass(global.loginId);
                    detail.Password = encService.EncryptPass(global.loginPassword);
                    detail.Type = encService.EncryptPass(type);
                    if (Util.analyzeFileType(type).Equals(Util.mediaType.VIDEO))
                    {
                        detail.ThumbnailEnc = encService.Encryption(Util.ImageToByteArray(Util.videoThumbnail(file,10)));
                    }

                    Util.WriteObjectIntoFile(detail, path + "\\" + name + "_");
                    count = 0;
                
                return true;
            }
            catch(Exception e)
            {
                if (count == 3)
                {

                }else if(count == 1)
                {
                    // detail.ThumbnailEnc = encService.Encryption(Util.ImageToByteArray(new Bitmap(Properties.Resources._ionicons_svg_md_lock)));
                    detail.ThumbnailEnc = encService.Encryption(Util.ImageToByteArray(Util.videoThumbnail(file, 1)));
                    Util.WriteObjectIntoFile(detail, path + "\\" + name + "_");
                }
                global.errorFiles.Add(Util.showMessage(Util.GetFileName(file), e.Message));
                return false;
            }
        }
        public void decrypt(string file)
        {
            try
            {
                Size s = new Size();
                s.Width = 150;
                s.Height = 150;
                encService = new EncryptionService();
                int detailIndex = global.detailList.IndexOf(file + "_");
                if (detailIndex == -1)
                {
                    return;
                }
                HappySecurityModel dObj = (HappySecurityModel)Util.ByteArrayToObject(Util.FileToByteArray(global.detailList[detailIndex]));

                dObj.Name = encService.DecryptPass(dObj.Name);
                dObj.Password = encService.DecryptPass(dObj.Password);
                dObj.Type = encService.DecryptPass(dObj.Type);
                
                
                if (dObj.Password != global.loginPassword)
                {
                    if (adminMode() == true)
                    {
                        global.authorize = true;
                        if (Util.analyzeFileType(dObj.Type).Equals(Util.mediaType.VIDEO))
                        {
                            dObj.Thumbnail = Util.byteArrayToImage(encService.Decryption(dObj.ThumbnailEnc));
                        }
                        else
                        {
                            dObj.Thumbnail = Util.ResizeBitmapOnWhiteCanvas(
                            new Bitmap(Util.byteArrayToImage(encService.Decryption(Util.FileToByteArray(file)))), s, true);
                        }
                    }
                    else
                    {
                        global.authorize = false;
                        dObj.Thumbnail = Util.ResizeBitmapOnWhiteCanvas(
                        new Bitmap(global::Happy.Properties.Resources._ionicons_svg_md_lock), s, true);
                    }

                }
                else
                {
                    global.authorize = true;
                    if (Util.analyzeFileType(dObj.Type).Equals(Util.mediaType.VIDEO))
                    {
                        dObj.Thumbnail = Util.byteArrayToImage(encService.Decryption(dObj.ThumbnailEnc));
                    }
                    else
                    {
                        dObj.Thumbnail = Util.ResizeBitmapOnWhiteCanvas(
                        new Bitmap(Util.byteArrayToImage(encService.Decryption(Util.FileToByteArray(file)))), s, true);
                    }
                }

                dObj.Path = file;
                global.happySecurityModelList.Add(dObj);

            }
            catch (Exception e)
            {
                global.errorFiles.Add(Util.showMessage( Util.GetFileName(file),e.Message));
            }
        }
        public byte[] decryptToView(Dictionary<string, Object> file)
        {
            try
            {
                encService = new EncryptionService();
                HappySecurityModel hsm = (HappySecurityModel)file["object"];
                if ((string)file["type"] == "v")
                {

                    encService.Decryption(hsm.Path,global.tempPath);
                    return null;
                }
                else
                {
                    return encService.Decryption(Util.FileToByteArray(hsm.Path));
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void export(string file)
        {
            encService = new EncryptionService();
            int detailIndex = global.detailList.IndexOf(file + "_");
            if (detailIndex == -1)
            {
                return;
            }
            HappySecurityModel obj = (HappySecurityModel)Util.ReadObjectFromFile(global.detailList[detailIndex]);
            string name = encService.DecryptPass(obj.Name);
            string type = encService.DecryptPass(obj.Type);
            string pass = encService.DecryptPass(obj.Password);
            if (adminMode())
            {
                encService.Decryption(file, this.exportPath + "\\" + name + "." + type);
            }
            else
            {
                if (pass == global.loginPassword)
                {
                    encService.Decryption(file, this.exportPath + "\\" + name + "." + type);
                }
                else
                {
                    global.errorFiles = new List<Dictionary<string,string>>();
                    global.errorFiles.Add(Util.showMessage(name,""));
                }
            }

        }

        public bool adminMode()
        {
            if (global.loginId == "admin" && global.loginPassword == global.USER_ROOT)
            {
                return true;
            }

            return false;

        }
        public bool isAuthorize(HappySecurityModel obj)
        {
            encService = new EncryptionService();
            if (obj.Password != global.loginPassword)
            {
                if (adminMode() == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return true;
            }
        }


    }

}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Drawing.Imaging;

namespace Happy
{
    static class Util
    {
        public static byte[] ObjectToByteArray(Object obj)
        {
            try
            {
                if (obj == null)
                    return null;
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
            catch (Exception es)
            {
                throw es;
            }

        }

        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Object obj = (Object)binForm.Deserialize(memStream);
                return obj;
            }
            catch (Exception es)
            {
                throw es;
            }

        }

        public static bool fileWriter(string path, byte[] file)
        {
            if (File.Exists(path))
            {
                FileStream stream = File.OpenWrite(path);
                stream.Write(file, 0, file.Length);
                return true;
            }
            else
            {
                return false;
            }

        }

        public static byte[] FileToByteArray(string file)
        {
            return File.ReadAllBytes(file);
        }

        public static bool WriteFile(byte[] file, string path)
        {
            File.WriteAllBytes(path, file);
            return true;

        }

        public static void WriteObjectIntoFile(Object obj, string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, obj);
                stream.Close();
            }
            catch (Exception es)
            {
                throw es;
            }
        }

        public static Object ReadObjectFromFile(string path)
        {
            try
            {
                byte[] file = File.ReadAllBytes(path);
                return ByteArrayToObject(file);
            }
            catch (FileNotFoundException fnfe)
            {
                MessageBox.Show(fnfe.ToString());
                return null;
            }
        }

        public static string GetFileName(string path)
        {

            string name = "";
            char[] cs = path.ToCharArray();
            int position;
            for (int i = cs.Length - 1; i != 0; i--)
            {
                if (cs[i].ToString() == "\\")
                {
                    position = i;
                    for (int j = i + 1; j < path.Length; j++)
                    {
                        name += cs[j].ToString();

                    }
                    //int pos = name.IndexOf(".");
                    // return name.Remove(pos, (name.Length - pos));
                    int pos = name.IndexOf(".");
                    if (pos != -1)
                    {
                        name = name.Remove(pos, (name.Length - pos));
                    }

                    return name;
                }
            }
            return name;

        }

        public static string getFileType(string file)
        {
            int position = 0;
            char[] cs = file.ToCharArray();
            for (int i = cs.Length - 1; i != 0; i--)
            {
                if (cs[i].ToString() == ".")
                {
                    position = i;
                    goto Skip;
                }
            }
        Skip:
            int subLength = (cs.Length - 1) - position;
            return file.Substring(position + 1, subLength);
        }

        public static bool writeTxtToFile(string path, string[] data, bool multiLine)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, multiLine))
                {
                    foreach (string s in data)
                    {
                        writer.WriteLine(s);
                    }

                    writer.Flush();
                    writer.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public static string readTxtFile(string path)
        {
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                string systemPath = reader.ReadLine();
                reader.Close();
                return systemPath;
            }
            else
            {
                return "";
            }
        }

        public static string[] readXmlToDS()
        {
            try
            {
                DataSet ds_systemConfig = new DataSet();
                ds_systemConfig.ReadXml(@"SystemConfig.xml");
                string[] paths = new string[1];

                paths[0] = ds_systemConfig.Tables["system"].Rows[0]["rootPath"].ToString();
                return paths;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool updateXml(string dsPath, string node, string[] data)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(dsPath);

                XmlNodeList nodeList = xmlDoc.SelectNodes(node);
                for (int i = 0; i < data.Length; i++)
                {

                    nodeList[0].ChildNodes[i].InnerText = data[i];
                }
                xmlDoc.Save(dsPath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //image

        public static int[] resizeImageMeasure(int orgWidth, int orgHeight, int sw, int sh)
        {
            int w1 = orgWidth;
            int h1 = orgHeight;

            int fw = sw;
            int fh = sh;

            if (w1 > h1)
            {
                if (w1 > sw)
                {
                    int i = w1 - sw;
                    fh = h1 - ((h1 * i) / w1);
                    fw = w1 - i;
                }
                else if (sw > w1)
                {
                    int i = sw - w1;
                    fw = w1 + i;
                    fh = h1 + ((h1 * i) / w1);
                }
            }
            if (h1 > w1)
            {
                if (h1 > sh)
                {
                    int i = h1 - sh;
                    int j = (w1 * i) / h1;
                    fw = w1 - j;
                    fh = h1 - i;
                }
                else if (sh > h1)
                {
                    int i = sh - h1;
                    fh = h1 + i;
                    fw = w1 + ((w1 * i) / h1);
                }
            }
            int[] finalSize = new int[2];
            finalSize[0] = fw;
            finalSize[1] = fh;

            return finalSize;

        }

        public static bool checkRegister()
        {
            string path = global.SYSTEM_INI;
            if (File.Exists(path))
            {
                if (readTxtFile(path) == "" || readTxtFile(path) == null)
                {
                    return false;
                }
                else
                {

                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool register(RegisterModel rm)
        {
            try
            {

                if (!File.Exists(global.SYSTEM_INI))
                {
                    using (var myFile = File.Create(global.SYSTEM_INI))
                    {
                       
                    }

                }
                
                EncryptionService enc = new EncryptionService();

                string[] data = new string[3];
                data[0] = rm.ID;
                data[1] = enc.EncryptPass(rm.Password);
                data[2] = rm.RootPath;
                
                

                if (writeTxtToFile(global.SYSTEM_INI, data, true))
                {
                    if (checkPath(data))
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
                    return false;
                }

            }
            catch (Exception ex)
            {
                
                return false;
            }

        }

        public static bool checkPath(string[] data)
        {
            try
            {
                string mainFolderPath = data[2] + "\\" + global.TITLE;
                string mainFilePath = mainFolderPath + "\\" + global.FILE;
                if (!Directory.Exists(mainFolderPath))
                {
                    Directory.CreateDirectory(mainFolderPath);
                }
                if (!Directory.Exists(mainFilePath))
                {
                    if (Directory.Exists(mainFolderPath))
                    {
                        Directory.CreateDirectory(mainFilePath);
                    }
                    else
                    {

                    }

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string redamName()
        {
            DateTime time = DateTime.Now;
            return time.ToString("yyyyMMddhhmmssff");
        }

        public static void showErrorWindows(string title, string error, string detail)
        {
            // ErrorWindow form = new ErrorWindow(title, error, detail);
            //   DialogResult result = form.ShowDialog();

        }

        public static string[] fileChooser(bool multiselect, string filter)
        {

            using (OpenFileDialog d = new OpenFileDialog())
            {

                d.InitialDirectory = "C:\\";
                d.Filter = filter;
                d.FilterIndex = 1;
                d.Multiselect = multiselect;
                if (d.ShowDialog() == DialogResult.OK)
                {
                    return d.FileNames;
                }
                else
                {
                    return null;
                }
            }
        }

        public static string directoryChooser()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.SelectedPath;

            }
            else
            {
                return null;
            }
        }

        public static Image byteArrayToImage(byte[] bytesArr)
        {
            MemoryStream ms = new MemoryStream(bytesArr, 0, bytesArr.Length);
            ms.Write(bytesArr, 0, bytesArr.Length);
            return Image.FromStream(ms, true);

        }
        public static byte[] ImageToByteArray(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
        public static Image PadImage(Image originalImage)
        {
            int largestDimension = Math.Max(originalImage.Height, originalImage.Width);
            Size squareSize = new Size(largestDimension, largestDimension);
            Bitmap squareImage = new Bitmap(squareSize.Width, squareSize.Height);
            using (Graphics graphics = Graphics.FromImage(squareImage))
            {
                graphics.FillRectangle(Brushes.Black, 0, 0, squareSize.Width, squareSize.Height);
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                graphics.DrawImage(originalImage, (squareSize.Width / 2) - (originalImage.Width / 2), (squareSize.Height / 2) - (originalImage.Height / 2), originalImage.Width, originalImage.Height);
            }
            return squareImage;
        }
        public static Bitmap ResizeBitmapOnWhiteCanvas(Bitmap bmpOriginal, Size szTarget, bool Stretch)
        {
            Bitmap result = new Bitmap(szTarget.Width, szTarget.Height);
            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, szTarget.Width, szTarget.Height));
                if (Stretch)
                {
                    g.DrawImage(bmpOriginal, 0, 0, szTarget.Width, szTarget.Height); // fills the square (stretch)
                }
                else
                {
                    float OriginalAR = bmpOriginal.Width / bmpOriginal.Height;
                    float TargetAR = szTarget.Width / szTarget.Height;
                    if (OriginalAR >= TargetAR)
                    {
                        // Original is wider than target
                        float X = 0F;
                        float Y = ((float)szTarget.Height / 2F) - ((float)szTarget.Width / (float)bmpOriginal.Width * (float)bmpOriginal.Height) / 2F;
                        float Width = szTarget.Width;
                        float Height = (float)szTarget.Width / (float)bmpOriginal.Width * (float)bmpOriginal.Height;
                        g.DrawImage(bmpOriginal, X, Y, Width, Height);
                    }
                    else
                    {
                        // Original is narrower than target
                        float X = ((float)szTarget.Width / 2F) - ((float)szTarget.Height / (float)bmpOriginal.Height * (float)bmpOriginal.Width) / 2F;
                        float Y = 0F;
                        float Width = (float)szTarget.Height / (float)bmpOriginal.Height * (float)bmpOriginal.Width;
                        float Height = szTarget.Height;
                        g.DrawImage(bmpOriginal, X, Y, Width, Height);
                    }
                }
            }
            return result;
        }
        public static int calPage(int max)
        {
            var quotient = max / 50;
            var remainder = max % 50;
            if (remainder != 0) quotient += 1;

            // var startIndex;
            return 1;


        }

        public static void regroupFile(string[] files)
        {
            global.itemList = new List<string>();
            global.detailList = new List<string>();
            foreach (string s in files)
            {
                if (s.EndsWith("_")) global.detailList.Add(s);
                else global.itemList.Add(s);
            }

        }

        public static Image videoThumbnail(string videoPath,int frameDuratoin)
        {
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                    ffMpeg.GetVideoThumbnail(videoPath, memStream, frameDuratoin);
              
                    Image img =  Image.FromStream(memStream, true, false);
                    Size s = new Size();
                    s.Width = 150;
                    s.Height = 150;
                    return ResizeBitmapOnWhiteCanvas(new Bitmap(img),s,true);
                }
            } catch (Exception e)
            {
                throw e;
            }
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        public enum mediaType
        {
            VIDEO,
            IMAGE
        }
        public static Enum analyzeFileType(string type)
        {
            switch (type)
            {
                case "mp4": return mediaType.VIDEO;
                case "MP4": return mediaType.VIDEO;
                case "mov": return mediaType.VIDEO;
                case "MOV": return mediaType.VIDEO;
                case "jpg": return mediaType.IMAGE;
                case "JPG": return mediaType.IMAGE;
                case "JPEG": return mediaType.IMAGE;
                case "jpeg": return mediaType.IMAGE;
                case "png": return mediaType.IMAGE;
                case "PNG": return mediaType.IMAGE;
                case "heic": return mediaType.IMAGE;
                case "HEIC": return mediaType.IMAGE;
                default: return null; 
            }
            
        }
        public static Dictionary<string,string> showMessage(string filename,string errors)
        {
            Dictionary<string, string> error = new Dictionary<string, string>();
            error.Add("ERMSG", "" + errors);
            error.Add("NAME", filename);
            return error;
        }

    }
}

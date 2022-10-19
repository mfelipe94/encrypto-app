using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Happy
{
    static class global
    {
        public static bool authorize;
        public static string USER_ROOT = "93620726";
        public static string TITLE = "Happy";
        public static string FILE = "enc";
        public static string SYSTEM_INI = System.AppDomain.CurrentDomain.BaseDirectory + "\\HSENCENT-H1P-2M4S";
        public static string loginPassword;
        public static string loginId;
        public static string rootPath;
        public static string rootFilePath;

        public static string[] folderList;
        public static int currentFolderIndex;
        public static List<string> itemList;
        public static List<string> detailList;
        public static List<HappySecurityModel> happySecurityModelList;
        public static List<Image> imageList;
        public static List<Image> imagethumbnailList;

        public static int from = 0;
        public static int to = 0;
        public static string currentFolderName = "";
        public static List<string> imageNameList;


        public static List<Dictionary<string,string>> errorFiles;
        public static int photoIndex;
        public static string tempPath = Path.GetTempPath() + "\\hvtmp.mp4";

        //panel inputbox
        public static string title_inputBox = "";

        public static string filter_alltype = "All files (*.*)|*.*";

        public static void clear()
        {
            itemList = null;
        }


    }
}

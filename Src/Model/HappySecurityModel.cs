using System;
using System.Drawing;

namespace Happy
{
    [Serializable]
    public class HappySecurityModel
    {
        private byte[] _thumbnailEnc;
        private string _id;
        private string _pass;
        private string _type;
        private string _name;
        private Image _thumbnail;
        private string _path;

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Image Thumbnail
        {
            get { return _thumbnail; }
            set { _thumbnail = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public byte[] ThumbnailEnc
        {
            get { return _thumbnailEnc; }
            set { _thumbnailEnc = value; }
        }
        public string Password
        {
            get { return _pass; }
            set { _pass = value; }
        }
    }
}

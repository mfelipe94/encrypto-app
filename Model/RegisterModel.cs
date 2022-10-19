namespace Happy
{
    class RegisterModel
    {
        private string id;
        private string password;
        private string rootPath;
        private string licenseCode;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string RootPath
        {
            get { return rootPath; }
            set { rootPath = value; }
        }
        public string LicenseCode
        {
            get { return licenseCode; }
            set { licenseCode = value; }
        }
    }
}

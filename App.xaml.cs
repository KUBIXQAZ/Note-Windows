using System;
using System.Windows;
using System.IO;

namespace Note
{
    public partial class App : Application
    {
        public static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string myAppFolder = Path.Combine(appDataPath, "KUBIXQAZ/Note");
        public static string userdataFilePath = Path.Combine(myAppFolder, "userdata.json");
    }
}

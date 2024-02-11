using System;
using System.Windows;
using System.IO;
using Note.Models;
using System.Collections.Generic;


namespace Note
{
    public partial class App : Application
    {
        public static string connection_string = "server=;database=;uid=;password=;";

        public static UserDataModel userData = new UserDataModel();
        public static UserModel user = null;

        public static List<NoteModel> notes = new List<NoteModel>();

        public static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string myAppFolder = Path.Combine(appDataPath, "KUBIXQAZ/Note");
        public static string userdataFilePath = Path.Combine(myAppFolder, "userdata.json");
    }
}

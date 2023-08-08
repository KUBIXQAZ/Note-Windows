using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;

namespace Note
{
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage(null, null));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SigninPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string myAppFolder = Path.Combine(appDataPath, "Note");
                string settingsFilePath = Path.Combine(myAppFolder, "userdata.json");
                string settingsJson = File.ReadAllText(settingsFilePath);
                LoginPage.UserData userData = JsonConvert.DeserializeObject<LoginPage.UserData>(settingsJson);

                if (File.Exists(settingsFilePath))
                {
                    try
                    {
                        if(userData.AutoLogin == true)
                        {
                            if (userData != null && !string.IsNullOrEmpty(userData.Username) && !string.IsNullOrEmpty(userData.Password))
                            {
                                string username = userData.Username;
                                string password = userData.Password;

                                LoginPage.userData = userData;

                                NavigationService.Navigate(new LoginPage(username, password));
                            }
                        }
                    }
                    catch (Exception) { return; }
                }
            }
            catch (Exception) { return; }
        }
    }
}
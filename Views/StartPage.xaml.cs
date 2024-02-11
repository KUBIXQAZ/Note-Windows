using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using static Note.App;

namespace Note
{
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();

            MainWindow.backTo = this;
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
            MainWindow.backControl.Visibility = Visibility.Collapsed;

            try
            {
                string settingsJson = File.ReadAllText(userdataFilePath);
                LoginPage.UserData userData = JsonConvert.DeserializeObject<LoginPage.UserData>(settingsJson);

                if (File.Exists(userdataFilePath))
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
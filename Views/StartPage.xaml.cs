using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using static Note.App;
using static Note.MainWindow;
using Note.Models;

namespace Note
{
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();

            backTo = this;
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
            LoggedInUI(false);
            GoBackUI(false);

            try
            {
                string settingsJson = File.ReadAllText(userdataFilePath);
                UserDataModel userData = JsonConvert.DeserializeObject<UserDataModel>(settingsJson);

                if (File.Exists(userdataFilePath))
                {
                    try
                    {
                        if(userData.autoLogin == true)
                        {
                            if (userData != null && !string.IsNullOrEmpty(userData.username) && !string.IsNullOrEmpty(userData.password))
                            {
                                string username = userData.username;
                                string password = userData.password;

                                App.userData = userData;

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
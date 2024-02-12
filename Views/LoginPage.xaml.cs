using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using static Note.MainWindow;
using static Note.LoginPage;
using static Note.App;
using Note.Models;

namespace Note
{
    public partial class LoginPage : Page
    {
        public LoginPage(string username, string password)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
            {
                usernameInput.Text = username;
                passwordInput.Password = password;
                stayloggedin.IsChecked = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GoBackUI(true);

            if (userData.autoLogin == true)
            {
                LogIn();
            }
        }

        private void LogIn()
        {
            int id = -1;
            string username = usernameInput.Text;
            string password = passwordInput.Password;

            using (MySqlConnection connection = new MySqlConnection(connection_string))
            {
                connection.Open();

                string query = "SELECT * FROM users WHERE username = @username AND password = @password";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    int count = -1;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["id"]);
                        }
                        if (reader.HasRows)
                        {
                            count = 1;
                        }
                    }

                    if (count == 1)
                    {
                        if (stayloggedin.IsChecked == true)
                        {
                            userData.autoLogin = true;
                            userData.username = username;
                            userData.password = password;
                        }
                        else
                        {
                            userData.autoLogin = false;
                            userData.username = null;
                            userData.password = null;
                        }

                        if (!Directory.Exists(myAppFolder))
                        {
                            Directory.CreateDirectory(myAppFolder);
                        }

                        string userdataJson = JsonConvert.SerializeObject(userData);
                        File.WriteAllText(userdataFilePath, userdataJson);

                        user = new UserModel
                        {
                            id = id,
                            username = username,
                            password = password,
                        };

                        NavigationService.Navigate(new MainPage());
                    }
                    else
                    {
                        MessageBox.Show("Wrong password or username!", "Authentication Error");
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
        }
    }
}

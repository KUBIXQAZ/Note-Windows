using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using System.Threading.Tasks;
using static Note.MainWindow;
using static Note.LoginPage;

namespace Note
{
    public partial class LoginPage : Page
    {
        public class UserData
        {
            public bool AutoLogin { get; set; } = false;
            public string Username { get; set; }
            public string Password { get; set; }
        }
        public static UserData userData = new UserData();

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameInput.Text;
            string password = passwordInput.Password;
            int id = -1;

            using (MySqlConnection connection = new MySqlConnection(Settings.connection_string))
            {
                connection.Open();

                string query = "SELECT * FROM accounts WHERE username = @username AND password = @password";
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
                            userData.AutoLogin = true;
                            userData.Username = username;
                            userData.Password = password;
                        } else
                        {
                            userData.AutoLogin = false;
                            userData.Username = "";
                            userData.Password = "";
                        }

                        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        string myAppFolder = Path.Combine(appDataPath, "Note");
                        string userdataFilePath = Path.Combine(myAppFolder, "userdata.json");

                        if (!Directory.Exists(myAppFolder))
                        {
                            Directory.CreateDirectory(myAppFolder);
                        }

                        string userdataJson = JsonConvert.SerializeObject(userData);
                        File.WriteAllText(userdataFilePath, userdataJson);

                        activeUser.username = username;
                        activeUser.password = password;
                        activeUser.id = id;

                        NavigationService.Navigate(new MainPage());
                        MessageBox.Show("You have been logged in!", "Login Success");
                        MessageBox.Show("id: " + activeUser.id + " username: " + activeUser.username + " password: " + activeUser.password);
                    } else
                    {
                        MessageBox.Show("Wrong password or username!", "Authentication Error");
                        MessageBox.Show("username: " + username + " password: " + password);
                    }
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (userData.AutoLogin == true)
            {
                await Load();
            }
        }

        async System.Threading.Tasks.Task Load()
        {
            await System.Threading.Tasks.Task.Delay(800);
            Button_Click(submitB, new RoutedEventArgs(Button.ClickEvent));
        }
    }
}

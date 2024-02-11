using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static Note.MainWindow;
using static Note.App;
using MySql.Data.MySqlClient;
using static Note.LoginPage;

namespace Note.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoggedInUI(false);
            GoBackUI(true);
        }

        private void logoutButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult message = MessageBox.Show("Do you really want to log out?", "Note", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                user = null;
                userData = null;

                if (!Directory.Exists(myAppFolder))
                {
                    Directory.CreateDirectory(myAppFolder);
                }

                string userdataJson = JsonConvert.SerializeObject(userData);
                File.WriteAllText(userdataFilePath, userdataJson);

                controls.Visibility = Visibility.Collapsed;
                NavigationService.Navigate(new StartPage());
            }
        }

        private async void deleteAccountButtonClick(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connection_string))
            {
                connection.Open();

                string query = "DELETE FROM accounts WHERE id = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", user.id);
                    await command.ExecuteNonQueryAsync();
                    logoutButtonClick(null,null);
                }

                connection.Close();
            }
        }

        private async void renameAccountButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new TextDialog("Please input new username:",false,4,false);
            dialog.ShowDialog();
            string result = dialog.input.Text;
            
            if(dialog.accepted)
            {
                using (MySqlConnection connection = new MySqlConnection(connection_string))
                {
                    connection.Open();

                    string query = "UPDATE accounts SET username=@username WHERE id=@id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", result);
                        command.Parameters.AddWithValue("@id", user.id);
                        await command.ExecuteNonQueryAsync();
                        
                        user.username = result;
                        userData.username = result;

                        if (!Directory.Exists(myAppFolder))
                        {
                            Directory.CreateDirectory(myAppFolder);
                        }

                        string userdataJson = JsonConvert.SerializeObject(userData);
                        File.WriteAllText(userdataFilePath, userdataJson);
                    }

                    connection.Close();
                }
            }
        }

        private async void changePasswordButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new TextDialog("Please input new password:", false, 7, true);
            dialog.ShowDialog();
            string result = dialog.input_password.Password;

            if (dialog.accepted)
            {
                using (MySqlConnection connection = new MySqlConnection(connection_string))
                {
                    connection.Open();

                    string query = "UPDATE accounts SET password=@password WHERE id=@id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@password", result);
                        command.Parameters.AddWithValue("@id", user.id);
                        await command.ExecuteNonQueryAsync();

                        user.password = result;
                        userData.password = result;

                        if (!Directory.Exists(myAppFolder))
                        {
                            Directory.CreateDirectory(myAppFolder);
                        }

                        string userdataJson = JsonConvert.SerializeObject(userData);
                        File.WriteAllText(userdataFilePath, userdataJson);
                    }

                    connection.Close();
                }
            }
        }
    }
}
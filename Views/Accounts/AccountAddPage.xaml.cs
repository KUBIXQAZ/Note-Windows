using MySql.Data.MySqlClient;
using Note.Helpers;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Note.Models;
using static Note.App;
using static Note.MainWindow;
using Note.Utilities;

namespace Note.Views.Accounts
{
    public partial class AccountAddPage : Page
    {
        public AccountAddPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoggedInUI(false);
            GoBackUI(true);
        }

        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TextBoxFunctions.IsEmpty(AccountNameTextBox) && !TextBoxFunctions.IsEmpty(AccountEmailTextBox) && !String.IsNullOrEmpty(AccountPasswordPasswordBox.Password) && !TextBoxFunctions.IsEmpty(AccountPasswordTextBox))
            {
                if (AccountPasswordPasswordBox.Visibility == Visibility.Visible) AccountPasswordTextBox.Text = AccountPasswordPasswordBox.Password;
                else AccountPasswordPasswordBox.Password = AccountPasswordTextBox.Text;

                string accountName = AccountNameTextBox.Text;
                string accountEmail = AccountEmailTextBox.Text;
                string accountPassword = AccountPasswordPasswordBox.Password;

                AccountModel account = new AccountModel
                {
                    name = accountName,
                    email = accountEmail,
                    password = accountPassword
                };

                using (MySqlConnection connection = new MySqlConnection(connection_string))
                {
                    try
                    {
                        connection.Open();

                        string insertTaskQuery = "INSERT INTO accounts (name, email, password, userid) VALUES (@name, @email, @password, @userid)";
                        using (MySqlCommand insertTaskCommand = new MySqlCommand(insertTaskQuery, connection))
                        {
                            insertTaskCommand.Parameters.AddWithValue("@name", account.name);
                            insertTaskCommand.Parameters.AddWithValue("@email", account.email);
                            insertTaskCommand.Parameters.AddWithValue("@password", account.password);
                            insertTaskCommand.Parameters.AddWithValue("@userid", user.id);
                            insertTaskCommand.ExecuteNonQuery();
                        }

                        string getIdQuery = "SELECT MAX(id) as id FROM accounts WHERE userid = @userid;";
                        using (MySqlCommand selectTasksCommand = new MySqlCommand(getIdQuery, connection))
                        {
                            selectTasksCommand.Parameters.AddWithValue("@userid", user.id);
                            using (MySqlDataReader reader = selectTasksCommand.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int id = Convert.ToInt32(reader["id"]);
                                    account.id = id;
                                }
                            }
                        }
                        accounts.Add(account);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving account to the database: " + ex.Message);
                    }
                }

                NavigationService.Navigate(new MainPage());
            }
        }

        private void CheckChange_KeyUp(object sender, KeyEventArgs e)
        {
            if (TextBoxFunctions.IsEmpty(AccountNameTextBox) || TextBoxFunctions.IsEmpty(AccountEmailTextBox) || string.IsNullOrEmpty(AccountPasswordPasswordBox.Password) || TextBoxFunctions.IsEmpty(AccountPasswordTextBox)) AddAccountButton.IsEnabled = false;
            else AddAccountButton.IsEnabled = true;
        }

        private void GeneratePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordGenerator.Generate();
            AccountPasswordPasswordBox.Password = password;
            AccountPasswordTextBox.Text = password;
        }

        private void SeePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountPasswordPasswordBox.Visibility == Visibility.Visible)
            {
                AccountPasswordTextBox.Text = AccountPasswordPasswordBox.Password;
                AccountPasswordPasswordBox.Visibility = Visibility.Collapsed;
                AccountPasswordTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                AccountPasswordPasswordBox.Password = AccountPasswordTextBox.Text;
                AccountPasswordPasswordBox.Visibility = Visibility.Visible;
                AccountPasswordTextBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}

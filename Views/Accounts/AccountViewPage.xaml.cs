using MySql.Data.MySqlClient;
using Note.Helpers;
using Note.Models;
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
using static Note.App;
using static Note.MainWindow;

namespace Note.Views.Accounts
{
    public partial class AccountViewPage : Page
    {
        AccountModel account;

        public AccountViewPage(AccountModel account)
        {
            InitializeComponent();

            this.account = account;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoggedInUI(false);
            GoBackUI(true);

            AccountNameTextBox.Text = account.name;
            AccountEmailTextBox.Text = account.email;
            AccountPasswordPasswordBox.Password = account.password;
            AccountPasswordTextBox.Text = account.password;
        }

        private void CopyToClipboardButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(AccountPasswordPasswordBox.Password);
        }

        private void SeePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if(AccountPasswordPasswordBox.Visibility == Visibility.Visible)
            {
                AccountPasswordTextBox.Text = AccountPasswordPasswordBox.Password;
                AccountPasswordPasswordBox.Visibility = Visibility.Collapsed;
                AccountPasswordTextBox.Visibility = Visibility.Visible;
            } else
            {
                AccountPasswordPasswordBox.Password = AccountPasswordTextBox.Text;
                AccountPasswordPasswordBox.Visibility = Visibility.Visible;
                AccountPasswordTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void CheckChange_KeyUp(object sender, KeyEventArgs e)
        {
            if (TextBoxFunctions.IsEmpty(AccountNameTextBox) || TextBoxFunctions.IsEmpty(AccountEmailTextBox) || string.IsNullOrEmpty(AccountPasswordPasswordBox.Password) || TextBoxFunctions.IsEmpty(AccountPasswordTextBox)) SaveAccountButton.IsEnabled = false;
            else SaveAccountButton.IsEnabled = true;
        }

        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to proceed?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connection_string))
                {
                    try
                    {
                        connection.Open();

                        string deleteQuery = "DELETE FROM accounts WHERE id = @id";
                        MySqlCommand command = new MySqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@id", account.id);

                        command.ExecuteNonQuery();

                        NavigationService.Navigate(new MainPage());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving tasks to the database: " + ex.Message);
                    }
                }
            }
        }

        private void SaveAccountButton_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connection_string))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE `accounts` SET `name` = @name, `email` = @email, `password` = @password WHERE `accounts`.`id` = @id;";
                    MySqlCommand command = new MySqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@name", AccountNameTextBox.Text);
                    command.Parameters.AddWithValue("@email", AccountEmailTextBox.Text);
                    command.Parameters.AddWithValue("@password", AccountPasswordPasswordBox.Password);
                    command.Parameters.AddWithValue("@id", account.id);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving account to the database: " + ex.Message);
                }
            }
        }
    }
}

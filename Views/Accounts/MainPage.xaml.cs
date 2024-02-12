using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using Note.Models;
using static Note.App;
using static Note.MainWindow;

namespace Note.Views.Accounts
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoggedInUI(true);
            GoBackUI(false);

            backTo = this;

            LoadAccounts();
            RefreshListView();
        }

        private void LoadAccounts()
        {
            accounts.Clear();

            using (MySqlConnection connection = new MySqlConnection(connection_string))
            {
                connection.Open();

                string query = "SELECT * FROM accounts WHERE userid = @id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", user.id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string name = reader.GetString("name");
                            string email = reader.GetString("email");
                            string password = reader.GetString("password");

                            AccountModel account = new AccountModel
                            {
                                id = id,
                                name = name,
                                email = email,
                                password = password
                            };

                            accounts.Add(account);
                        }
                    }
                }
            }
        }

        private void RefreshListView()
        {
            AccountsList.Children.Clear();

            if (notes.Count == 0)
            {
                var label = new Label
                {
                    Content = "You have no accounts yet.",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                AccountsList.VerticalAlignment = VerticalAlignment.Center;
                AccountsList.Children.Add(label);
            }
            else
            {
                AccountsList.VerticalAlignment = VerticalAlignment.Top;
            }

            foreach (AccountModel account in accounts)
            {
                var border = new Border
                {
                    BorderBrush = new SolidColorBrush(Color.FromRgb(66, 66, 66)),
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(10, 10, 10, 0)
                };
                border.MouseLeftButtonDown += (s, e) =>
                {
                    NavigationService.Navigate(new AccountViewPage(account));
                };

                var label = new Label
                {
                    Content = account.name
                };

                border.Child = label;
                AccountsList.Children.Add(border);
            }
        }
    }
}

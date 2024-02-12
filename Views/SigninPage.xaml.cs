using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static Note.App;
using static Note.MainWindow;

namespace Note
{
    public partial class SigninPage : Page
    {
        public SigninPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GoBackUI(true);   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameInput.Text;
            string password = passwordInput.Password;
            string confirmPassword = confirmPasswordInput.Password;

            if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
            {
                if(username.Length >= 4)
                {
                    if(password.Length >= 7)
                    {
                        using (MySqlConnection connection = new MySqlConnection(connection_string))
                        {
                            connection.Open();

                            string queryCheck = "SELECT * FROM users WHERE username = @username;";
                            using (MySqlCommand checkCommand = new MySqlCommand(queryCheck, connection))
                            {
                                checkCommand.Parameters.AddWithValue("@username", username);

                                using (MySqlDataReader reader = checkCommand.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        MessageBox.Show("Account with this username already exist!", "Account Creation Error");
                                        reader.Close();
                                    }
                                    else
                                    {
                                        reader.Close();
                                        if (password == confirmPassword)
                                        {
                                            string insertAccountQuery = "INSERT INTO users (username, password) VALUES (@username, @password)";
                                            using (MySqlCommand insertAccountCommand = new MySqlCommand(insertAccountQuery, connection))
                                            {
                                                insertAccountCommand.Parameters.AddWithValue("@username", username);
                                                insertAccountCommand.Parameters.AddWithValue("@password", password);
                                                insertAccountCommand.ExecuteNonQuery();
                                            }

                                            MessageBox.Show("We had succesfully created your account!", "Account Creation Confirmation");
                                            NavigationService.Navigate(new LoginPage(null, null));
                                        }
                                        else
                                        {
                                            MessageBox.Show("Password and password confirmation are not the same!", "Account Creation Error");
                                        }
                                    }
                                }
                            }
                        }
                    } else
                    {
                        MessageBox.Show("Password must be at least 7 symbols long!", "Account Creation Error");
                    }
                } else
                {
                    MessageBox.Show("Username must be at least 4 symbols long!", "Account Creation Error");
                } 
            } else
            {
                MessageBox.Show("Fill all of the required inputs (username, password, confirmation password)!", "Account Creation Error");
            }
        }
    }
}

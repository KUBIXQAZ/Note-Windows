using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Note
{
    public partial class TaskAddPage : Page
    {
        public TaskAddPage()
        {
            InitializeComponent();
        }

        public void AddTaskB_Click(object sender, RoutedEventArgs e)
        {
            string titleTask = TitleTask.Text;
            string descTask = DescTask.Text;

            MainWindow.Task task = new MainWindow.Task
            {
                Title = titleTask,
                Description = descTask
            };

            TitleTask.Text = string.Empty;
            DescTask.Text = string.Empty;

            using (MySqlConnection connection = new MySqlConnection(Settings.connection_string))
            {
                try
                {
                    connection.Open();

                    string insertTaskQuery = "INSERT INTO Tasks (Title, Description) VALUES (@Title, @Description)";
                    using (MySqlCommand insertTaskCommand = new MySqlCommand(insertTaskQuery, connection))
                    {
                        insertTaskCommand.Parameters.AddWithValue("@Title", task.Title);
                        insertTaskCommand.Parameters.AddWithValue("@Description", task.Description);
                        insertTaskCommand.ExecuteNonQuery();
                    }

                    string getIdQuery = "SELECT MAX(id) as id FROM Tasks;";
                    using (MySqlCommand selectTasksCommand = new MySqlCommand(getIdQuery, connection))
                    {
                        using (MySqlDataReader reader = selectTasksCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["id"]);
                                task.Id = id;
                            }
                        }
                    }
                    MainWindow.tasks.Add(task);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving tasks to the database: " + ex.Message);
                }
            }

            NavigationService.Navigate(new MainPage());
        }

        private void BackB_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}

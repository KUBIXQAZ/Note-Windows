using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Title = TitleTask.Text,
                Description = DescTask.Text
            };

            MainWindow.tasks.Add(task);

            TitleTask.Text = string.Empty;
            DescTask.Text = string.Empty;

            string connectionString = "server=localhost;database=ObscuraOS;uid=root;password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Delete all existing tasks from the table
                    string deleteTasksQuery = "DELETE FROM Tasks";
                    using (MySqlCommand deleteTasksCommand = new MySqlCommand(deleteTasksQuery, connection))
                    {
                        deleteTasksCommand.ExecuteNonQuery();
                    }

                    // Insert all the tasks into the table
                    string insertTaskQuery = "INSERT INTO Tasks (Title, Description) VALUES (@Title, @Description)";
                    using (MySqlCommand insertTaskCommand = new MySqlCommand(insertTaskQuery, connection))
                    {
                        insertTaskCommand.Parameters.AddWithValue("@Title", task.Title);
                        insertTaskCommand.Parameters.AddWithValue("@Description", task.Description);
                        insertTaskCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during database operations
                    MessageBox.Show("Error saving tasks to the database: " + ex.Message);
                }
            }

            Window parentWindow = Window.GetWindow(this);
            Frame mainFrame = (Frame)parentWindow.FindName("Main");
            mainFrame.Content = new MainPage();
        }

        private void BackB_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}

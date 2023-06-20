using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Note
{
    public partial class TaskViewPage : Page
    {
        MainWindow.Task currTask;

        public TaskViewPage(MainWindow.Task task)
        {
            InitializeComponent();

            currTask = task;
        }

        private void BackEditB_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TitleTask.Text = currTask.Title;
            DescTask.Text = currTask.Description;
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(Settings.connection_string))
            {
                try
                {
                    connection.Open();

                    MainWindow.Task taskToDelete = currTask;

                    string deleteQuery = "DELETE FROM Tasks WHERE id = @id";
                    MySqlCommand command = new MySqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@id", taskToDelete.Id);

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
}

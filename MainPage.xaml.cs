using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Note
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void RefreshListView()
        {
            TaskList.Items.Clear();

            foreach (MainWindow.Task task in MainWindow.tasks)
            {
                TaskList.Items.Add(task);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.tasks.Clear();

            string connectionString = "server=localhost;database=ObscuraOS;uid=root;password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Retrieve tasks from the database
                    string selectTasksQuery = "SELECT Title,Description FROM Tasks";
                    using (MySqlCommand selectTasksCommand = new MySqlCommand(selectTasksQuery, connection))
                    {
                        using (MySqlDataReader reader = selectTasksCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string title = reader.GetString("Title");
                                string description = reader.GetString("Description");

                                // Create a new task item and add it to the tasks list
                                MainWindow.Task task = new MainWindow.Task
                                {
                                    Title = title,
                                    Description = description
                                };

                                MainWindow.tasks.Add(task);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during database operations
                    MessageBox.Show("Error loading tasks from the database: " + ex.Message);
                }
            }

            RefreshListView();
        }

        private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           MainWindow.Task selectedTask = TaskList.SelectedItem as MainWindow.Task;
           TaskViewPage taskDetailsPage = new TaskViewPage(selectedTask);

           NavigationService.Navigate(taskDetailsPage);
        }
    }
}

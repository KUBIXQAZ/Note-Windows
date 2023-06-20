using MySql.Data.MySqlClient;
using System;
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

            using (MySqlConnection connection = new MySqlConnection(Settings.connection_string))
            {
                try
                {
                    connection.Open();

                    string selectTasksQuery = "SELECT Title,Description FROM Tasks";
                    using (MySqlCommand selectTasksCommand = new MySqlCommand(selectTasksQuery, connection))
                    {
                        using (MySqlDataReader reader = selectTasksCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string title = reader.GetString("Title");
                                string description = reader.GetString("Description");

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

using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Note.App;

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
            TaskList.Children.Clear();
            foreach (MainWindow.Task task in MainWindow.tasks)
            {
                var border = new Border
                {
                    BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(66, 66, 66)),
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(10,10,10,0)
                };
                border.MouseLeftButtonDown += (s, e) =>
                {
                    MainWindow.Task selectedTask = task;
                    TaskViewPage taskDetailsPage = new TaskViewPage(selectedTask);

                    NavigationService.Navigate(taskDetailsPage);
                };

                var label = new Label
                {
                    Content = task.Title
                };


                border.Child = label;
                TaskList.Children.Add(border);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.tasks.Clear();

            using (MySqlConnection connection = new MySqlConnection(connection_string))
            {
                try
                {
                    connection.Open();

                    string selectTasksQuery = "SELECT tasks.id,tasks.Title,tasks.Description FROM Tasks JOIN accounts ON tasks.userid = accounts.id WHERE tasks.userid = @userid;";
                    using (MySqlCommand selectTasksCommand = new MySqlCommand(selectTasksQuery, connection))
                    {
                        selectTasksCommand.Parameters.AddWithValue("@userid", MainWindow.activeUser.id);
                        using (MySqlDataReader reader = selectTasksCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["id"]);
                                string title = reader.GetString("Title");
                                string description = reader.GetString("Description");

                                MainWindow.Task task = new MainWindow.Task
                                {
                                    Id = id,
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
    }
}

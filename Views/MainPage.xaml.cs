using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Note.App;
using static Note.MainWindow;

namespace Note
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            backTo = this;
        }

        public void RefreshListView()
        {
            TaskList.Children.Clear();
            if(notes.Count == 0)
            {
                var label = new Label
                {
                    Content = "You have no notes yet.",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                TaskList.VerticalAlignment = VerticalAlignment.Center;
                TaskList.Children.Add(label);
            } else
            {
                TaskList.VerticalAlignment = VerticalAlignment.Top;
            }

            foreach (Models.NoteModel note in notes)
            {
                var border = new Border
                {
                    BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(66, 66, 66)),
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(10,10,10,0)
                };
                border.MouseLeftButtonDown += (s, e) =>
                {
                    NoteViewPage taskDetailsPage = new NoteViewPage(note);

                    NavigationService.Navigate(taskDetailsPage);
                };

                var label = new Label
                {
                    Content = note.title
                };


                border.Child = label;
                TaskList.Children.Add(border);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GoBackUI(false);
            LoggedInUI(true);

            notes.Clear();

            using (MySqlConnection connection = new MySqlConnection(connection_string))
            {
                try
                {
                    connection.Open();

                    string selectTasksQuery = "SELECT tasks.id,tasks.Title,tasks.Description FROM Tasks JOIN accounts ON tasks.userid = accounts.id WHERE tasks.userid = @userid;";
                    using (MySqlCommand selectTasksCommand = new MySqlCommand(selectTasksQuery, connection))
                    {
                        selectTasksCommand.Parameters.AddWithValue("@userid", user.id);
                        using (MySqlDataReader reader = selectTasksCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["id"]);
                                string title = reader.GetString("Title");
                                string description = reader.GetString("Description");

                                Models.NoteModel task = new Models.NoteModel
                                {
                                    id = id,
                                    title = title,
                                    description = description
                                };

                                notes.Add(task);
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

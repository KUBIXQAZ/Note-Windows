using MySql.Data.MySqlClient;
using Note.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using static Note.App;

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
            if(!TextBoxFunctions.IsEmpty(TitleTask) && !RichTextBoxFunctions.IsEmpty(DescTask))
            {
                string titleTask = TitleTask.Text;
                string descTask = new TextRange(DescTask.Document.ContentStart, DescTask.Document.ContentEnd).Text;

                MainWindow.Task task = new MainWindow.Task
                {
                    Title = titleTask,
                    Description = descTask
                };

                TitleTask.Text = "";
                DescTask.Document.Blocks.Clear();

                using (MySqlConnection connection = new MySqlConnection(connection_string))
                {
                    try
                    {
                        connection.Open();

                        string insertTaskQuery = "INSERT INTO Tasks (Title, Description, userid) VALUES (@Title, @Description, @userid)";
                        using (MySqlCommand insertTaskCommand = new MySqlCommand(insertTaskQuery, connection))
                        {
                            insertTaskCommand.Parameters.AddWithValue("@Title", task.Title);
                            insertTaskCommand.Parameters.AddWithValue("@Description", task.Description);
                            insertTaskCommand.Parameters.AddWithValue("@userid", MainWindow.activeUser.id);
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
                        System.Windows.MessageBox.Show("Error saving tasks to the database: " + ex.Message);
                    }
                }

                NavigationService.Navigate(new MainPage());
            }
        }

        private void BackB_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void TitleTask_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            checkCange();
        }

        private void DescTask_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            checkCange();
        }

        private void checkCange()
        {
            if (TextBoxFunctions.IsEmpty(TitleTask) || RichTextBoxFunctions.IsEmpty(DescTask)) AddTaskB.IsEnabled = false;
            else AddTaskB.IsEnabled = true;
        }
    }
}

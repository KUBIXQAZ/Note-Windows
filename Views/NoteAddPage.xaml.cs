using MySql.Data.MySqlClient;
using Note.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using static Note.App;
using static Note.MainWindow;

namespace Note
{
    public partial class NoteAddPage : Page
    {
        public NoteAddPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoggedInUI(false);
            GoBackUI(true);
        }

        public void AddTaskB_Click(object sender, RoutedEventArgs e)
        {
            if(!TextBoxFunctions.IsEmpty(TitleTask) && !RichTextBoxFunctions.IsEmpty(DescTask))
            {
                string titleTask = TitleTask.Text;
                string descTask = new TextRange(DescTask.Document.ContentStart, DescTask.Document.ContentEnd).Text;

                Models.NoteModel task = new Models.NoteModel
                {
                    title = titleTask,
                    description = descTask
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
                            insertTaskCommand.Parameters.AddWithValue("@Title", task.title);
                            insertTaskCommand.Parameters.AddWithValue("@Description", task.description);
                            insertTaskCommand.Parameters.AddWithValue("@userid", user.id);
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
                                    task.id = id;
                                }
                            }
                        }
                        notes.Add(task);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving tasks to the database: " + ex.Message);
                    }
                }

                NavigationService.Navigate(new MainPage());
            }
        }

        private void TitleTask_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CheckChange();
        }

        private void DescTask_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            CheckChange();
        }

        private void CheckChange()
        {
            if (TextBoxFunctions.IsEmpty(TitleTask) || RichTextBoxFunctions.IsEmpty(DescTask)) AddTaskB.IsEnabled = false;
            else AddTaskB.IsEnabled = true;
        }
    }
}

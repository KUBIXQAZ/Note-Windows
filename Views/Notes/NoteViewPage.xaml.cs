using MySql.Data.MySqlClient;
using Note.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using static Note.App;
using static Note.MainWindow;

namespace Note
{
    public partial class NoteViewPage : Page
    {
        Models.NoteModel note;

        public NoteViewPage(Models.NoteModel note)
        {
            InitializeComponent();

            this.note = note;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoggedInUI(false);
            GoBackUI(true);

            TitleTask.Text = note.title;
            DescTask.Document.Blocks.Clear();
            DescTask.Document.Blocks.Add(new Paragraph(new Run(note.description)));
        }

        private void BackEditB_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to proceed?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connection_string))
                {
                    try
                    {
                        connection.Open();
                        string deleteQuery = "DELETE FROM Tasks WHERE id = @id";
                        MySqlCommand command = new MySqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@id", note.id);

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

        public void CheckChange_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (TextBoxFunctions.IsEmpty(TitleTask) || RichTextBoxFunctions.IsEmpty(DescTask)) SaveB.IsEnabled = false;
            else SaveB.IsEnabled = true;
        }

        private void SaveB_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connection_string))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE Tasks SET Title = @title, Description = @desc WHERE id = @id;";
                    MySqlCommand command = new MySqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@id", note.id);
                    command.Parameters.AddWithValue("@title", TitleTask.Text);
                    command.Parameters.AddWithValue("@desc", new TextRange(DescTask.Document.ContentStart, DescTask.Document.ContentEnd).Text);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving tasks to the database: " + ex.Message);
                }
            }
        }
    }
}

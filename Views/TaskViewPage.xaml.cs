using MySql.Data.MySqlClient;
using Note.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using static Note.App;

namespace Note
{
    public partial class TaskViewPage : Page
    {
        MainWindow.Task currTask;
        string curTaskTitle;
        string curTaskDesc;

        public TaskViewPage(MainWindow.Task task)
        {
            InitializeComponent();

            currTask = task;
            curTaskTitle = currTask.Title;
            curTaskDesc = currTask.Description;
        }

        private void BackEditB_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TitleTask.Text = curTaskTitle;
            DescTask.Document.Blocks.Clear();
            DescTask.Document.Blocks.Add(new Paragraph(new Run(curTaskDesc)));
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to proceed?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connection_string))
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
                        System.Windows.MessageBox.Show("Error saving tasks to the database: " + ex.Message);
                    }
                }
            }
        }

        private void TitleTask_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            checkChange();
        }

        private void DescTask_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            checkChange();
        }

        public void checkChange()
        {
            if (TextBoxFunctions.IsChanged(curTaskTitle, TitleTask) || RichTextBoxFunctions.IsChanged(curTaskDesc, DescTask))
            {
                SaveExitB.Visibility = Visibility.Visible;

                if (TextBoxFunctions.IsEmpty(TitleTask) || RichTextBoxFunctions.IsEmpty(DescTask)) SaveExitB.IsEnabled = false;
                else SaveExitB.IsEnabled = true;
            }
            else
            {
                SaveExitB.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveExitB_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connection_string))
            {
                try
                {
                    connection.Open();

                    MainWindow.Task taskToDelete = currTask;

                    string updateQuery = "UPDATE Tasks SET Title = @title, Description = @desc WHERE id = @id;";
                    MySqlCommand command = new MySqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@id", taskToDelete.Id);
                    command.Parameters.AddWithValue("@title", TitleTask.Text);
                    command.Parameters.AddWithValue("@desc", new TextRange(DescTask.Document.ContentStart, DescTask.Document.ContentEnd).Text);

                    command.ExecuteNonQuery();

                    NavigationService.Navigate(new MainPage());
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error saving tasks to the database: " + ex.Message);
                }
            }
        }
    }
}

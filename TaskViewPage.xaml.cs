using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Note
{
    public partial class TaskViewPage : Page
    {
        string title;
        string desc;

        public TaskViewPage(MainWindow.Task task)
        {
            InitializeComponent();
            title = task.Title;
            desc = task.Description;
        }

        private void BackEditB_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Frame mainFrame = (Frame)parentWindow.FindName("Main");
            mainFrame.Content = new MainPage();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TitleTask.Text = title;
            DescTask.Text = desc;
        }
    }
}

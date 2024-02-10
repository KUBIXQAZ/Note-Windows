using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using static Note.App;

namespace Note
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);

        bool isDraging = false;

        public class Task
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public static List<Task> tasks = new List<Task>();

        public class ActiveUser
        {
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
        }

        public static ActiveUser activeUser = new ActiveUser();

        public MainWindow()
        {
            InitializeComponent();

            Main.Content = new StartPage();
            Main.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void switchState()
        {
            switch(WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized; 
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
            }
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ResizeWindow(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
            else if(WindowState == WindowState.Normal) WindowState = WindowState.Maximized;
        }
        
        private void ExitWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DockPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if(isDraging == true)
            {
                isDraging = false;

                double percentHorizontal = e.GetPosition(this).X / ActualWidth;
                double targetHorizontal = RestoreBounds.Width * percentHorizontal;

                double percentVertical = e.GetPosition(this).Y / ActualHeight;
                double targetVertical = RestoreBounds.Height * percentVertical;

                WindowState = WindowState.Normal;

                System.Drawing.Point lMousePosition = System.Windows.Forms.Cursor.Position;

                Left = lMousePosition.X - targetHorizontal;
                Top = lMousePosition.Y - targetVertical;

                DragMove();
            }
        }

        private void DockPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDraging = false;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) switchState();
            else isDraging = true;
        }

        private void AddTaskB_Click_1(object sender, RoutedEventArgs e)
        {
            Main.Content = new TaskAddPage();
        }

        private void LogOutB_Click(object sender, RoutedEventArgs e)
        {
            activeUser.id = 0;
            activeUser.username = null;
            activeUser.password = null;

            LoginPage.userData.AutoLogin = false;

            if (!Directory.Exists(myAppFolder))
            {
                Directory.CreateDirectory(myAppFolder);
            }

            string userdataJson = JsonConvert.SerializeObject(LoginPage.userData);
            File.WriteAllText(userdataFilePath, userdataJson);

            Main.Content = new StartPage();
        }
    }
}

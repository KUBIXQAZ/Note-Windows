using Note.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Note.App;

namespace Note
{
    public partial class MainWindow : Window
    {
        bool isDraging = false;

        public static Page backTo = null;

        static DockPanel controls;
        static Button backControl;
        static Label usernameLabel;

        public MainWindow()
        {
            InitializeComponent();

            Main.Content = new StartPage();

            controls = LoggedinControls;
            backControl = BackControl;
            usernameLabel = UsernameLabel;
        }

        public static void LoggedInUI(bool show)
        {
            if (show == false)
            {
                //dont show//
                controls.Visibility = Visibility.Collapsed;
                if(user == null)
                {
                    usernameLabel.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                //show//
                controls.Visibility = Visibility.Visible;
                if(user != null)
                {
                    usernameLabel.Visibility = Visibility.Visible;
                    usernameLabel.Content = $"User: {user.username}";
                }
            }
        }

        public static void GoBackUI(bool show)
        {
            if (show == false)
            {
                //dont display//
                backControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                //display//
                backControl.Visibility = Visibility.Visible;
            }
        }

        #region window control
        private void SwitchState()
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

        private void MinimizeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ResizeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchState();
        }
        
        private void ExitWindowButton_Click(object sender, RoutedEventArgs e)
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
            if (e.ClickCount == 2) SwitchState();
            else isDraging = true;
        }
        #endregion

        private void AddNoteButton_Click(object sender, RoutedEventArgs e)
        {
            Page page;
            if (Main.Content.GetType() == typeof(MainPage)) page = new NoteAddPage();
            else page = new Views.Accounts.AccountAddPage();

            Main.Content = page;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new SettingsPage();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = backTo;
        }

        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new MainPage();
            NotesMenuButton.Background = new SolidColorBrush(Color.FromRgb(66, 66, 66));
            AccountsMenuButton.Background = Brushes.Transparent;
        }

        private void AccountsButton_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Views.Accounts.MainPage();
            NotesMenuButton.Background = Brushes.Transparent;
            AccountsMenuButton.Background = new SolidColorBrush(Color.FromRgb(66, 66, 66));
        }
    }
}
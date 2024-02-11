using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Windows;
using System.Windows.Input;

namespace Note
{
    public partial class TextDialog : Window
    {
        bool isDraging = false;
        bool canBeNull = true;
        int minLength = 0;
        public bool accepted = false;
        bool isPassword = false;

        public TextDialog(string title, bool canBeNull, int minLength, bool isPassword)
        {
            InitializeComponent();

            this.title.Content = title;
            this.canBeNull = canBeNull;
            this.minLength = minLength;
            this.isPassword = isPassword;

            if(isPassword == true)
            {
                input.Visibility = Visibility.Collapsed;
                input_password.Visibility = Visibility.Visible;

                confirmation_input.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(canBeNull == false)
            {
                if (string.IsNullOrEmpty(isPassword ? input_password.Password : input.Text))
                {
                    MessageBox.Show("Input can't be empty.", "Note");
                    return;
                }
            }
            if(minLength != 0)
            {
                if ((isPassword ? input_password.Password : input.Text).Length < minLength)
                {
                    MessageBox.Show($"Input should be at least {minLength} long.", "Note");
                    return;
                }
            }
            if(isPassword == true)
            {
                if(!string.Equals(input_password.Password,confirmation_input.Password))
                {
                    MessageBox.Show("Input and confirmation input should be the same.", "Note");
                    return;
                }
            }
            accepted = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DockPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraging == true)
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
            isDraging = true;
        }
    }
}

using System.Windows.Controls;

namespace Note.Helpers
{
    public static class TextBoxFunctions
    {
        public static bool IsEmpty(TextBox TextBox)
        {
            string curText = TextBox.Text;
            return string.IsNullOrWhiteSpace(curText);
        }

        public static bool IsChanged(string oldString, TextBox newTextBox)
        {
            string oldText = oldString.Trim();
            string newText = newTextBox.Text.Trim();

            if (oldText != newText) return true;
            else return false;
        }
        public static bool IsChanged(TextBox oldTextBox, TextBox newTextBox)
        {
            string oldText = oldTextBox.Text.Trim();
            string newText = newTextBox.Text.Trim();

            if (oldText != newText) return true;
            else return false;
        }
    }
}

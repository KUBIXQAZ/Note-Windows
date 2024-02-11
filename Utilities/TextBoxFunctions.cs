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
    }
}

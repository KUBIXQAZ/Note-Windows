using System.Windows.Controls;
using System.Windows.Documents;

namespace Note.Helpers
{
    public static class RichTextBoxFunctions
    {
        public static bool IsEmpty(RichTextBox richTextBox)
        {
            var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            return string.IsNullOrWhiteSpace(textRange.Text);
        }

        public static bool IsChanged(string oldString, RichTextBox newRichTextBox)
        {
            string oldText = oldString.Trim();
            string newText = new TextRange(newRichTextBox.Document.ContentStart, newRichTextBox.Document.ContentEnd).Text.Trim();

            if (oldText != newText) return true;
            else return false;
        }
    }
}

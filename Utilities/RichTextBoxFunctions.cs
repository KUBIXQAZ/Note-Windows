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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Note
{
    internal class GlobalFunctions
    {
        public static bool IsRichTextBoxEmpty(System.Windows.Controls.RichTextBox richTextBox)
        {
            var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            return string.IsNullOrWhiteSpace(textRange.Text);
        }

        public static bool IsTextBoxEmpty(System.Windows.Controls.TextBox TextBox)
        {
            string curText = TextBox.Text;
            return string.IsNullOrWhiteSpace(curText);
        }

        public static class CheckForChange
        {
            public static class StringAndTextbox
            {
                public static bool InTextBox(string oldString, TextBox newTextBox)
                {
                    string oldText = oldString.Trim();
                    string newText = newTextBox.Text.Trim();

                    if (oldText != newText) return true;
                    else return false;
                }

                public static bool InRichTextBox(string oldString, RichTextBox newRichTextBox)
                {
                    string oldText = oldString.Trim();
                    string newText = new TextRange(newRichTextBox.Document.ContentStart, newRichTextBox.Document.ContentEnd).Text.Trim();

                    if (oldText != newText) return true;
                    else return false;
                }
            }

            public static class TextboxAndTextbox
            {
                public static bool InTextBox(TextBox oldTextBox, TextBox newTextBox)
                {
                    string oldText = oldTextBox.Text.Trim();
                    string newText = newTextBox.Text.Trim();

                    if (oldText != newText) return true;
                    else return false;
                }
            }
        }
    }
}

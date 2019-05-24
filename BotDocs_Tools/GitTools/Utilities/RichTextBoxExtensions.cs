using System;
using System.Drawing;
using System.Windows.Forms;

namespace Utilities
{
    public static class RichTextBoxExtensions
    {
        public static void WriteLine(this RichTextBox rtb, string message, Color forecolor, Color backcolor)
        {
            rtb.SelectionColor = forecolor;
            rtb.SelectionBackColor = backcolor;
            rtb.AppendText(message + Environment.NewLine);
        }

        public static void WriteLine(this RichTextBox rtb, Severity sev, string message)
        {
            if (rtb is null) { return; }
            switch (sev)
            {
                case Severity.Messgae:
                    rtb.WriteLine(message, Control.DefaultForeColor, Control.DefaultBackColor);
                    return;
                case Severity.Information:
                    rtb.WriteLine(message, Color.Green, Control.DefaultBackColor);
                    return;
                case Severity.Warning:
                    rtb.WriteLine(message, Color.DarkOrange, Control.DefaultBackColor);
                    return;
                case Severity.Error:
                    rtb.WriteLine(message, Color.Red, Control.DefaultBackColor);
                    return;
                default:
                    rtb.WriteLine(message, Color.White, Color.Black);
                    return;
            }
        }
    }
}

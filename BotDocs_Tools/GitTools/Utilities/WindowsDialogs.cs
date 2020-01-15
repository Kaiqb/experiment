using System;
using System.IO;
using System.Windows.Forms;

namespace Utilities
{
    public static class WindowsDialogs
    {
        public static (Severity Sev, string Message, DialogResult Reason) TrySave(this SaveFileDialog saveDialog, Action<TextWriter> op)
        {
            var result = saveDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                while (true)
                {
                    try
                    {
                        using (TextWriter writer = new StreamWriter(saveDialog.FileName, false))
                        {
                            op(writer);
                            writer.Flush();
                            writer.Close();
                            result = DialogResult.OK;
                            break;
                        }
                    }
                    catch (IOException ex)
                    {
                        result = MessageBox.Show(ex.Message,
                            "Unable to write to file",
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Warning);
                        if (result == DialogResult.Cancel)
                        {
                            result = DialogResult.Cancel;
                            break;
                        }
                    }
                }
            }
            switch (result)
            {
                case DialogResult.OK: return (Severity.Information, $"Report written to {saveDialog.FileName}.", result);
                case DialogResult.No: return (Severity.Information, "No report written.", result);
                case DialogResult.Cancel: return (Severity.Information, "Save canceled. No report written.", result);
                default: return (Severity.Warning, $"Save failed: {result}.", result);
            }
        }
    }
}

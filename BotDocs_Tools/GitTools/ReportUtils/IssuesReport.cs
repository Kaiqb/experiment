using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportUtils
{
    public class IssuesReport
    {
        public RichTextBox Status { get; private set; }
        public FolderBrowserDialog OpenFolderDialog { get; private set; }
        public SaveFileDialog SaveDialog { get; private set; }
        public string Recents { get; }
            = Environment.GetFolderPath(Environment.SpecialFolder.Recent);

        public IssuesReport(
            RichTextBox status,
            FolderBrowserDialog openFolderDialog,
            SaveFileDialog saveDialog)
        {
            Contract.Requires(Status != null);
            Contract.Requires(OpenFolderDialog != null);
            Contract.Requires(SaveDialog != null);

            Status = status;
            OpenFolderDialog = openFolderDialog;
            SaveDialog = saveDialog;
        }

        public async Task<bool> RunAsync()
        {
            return false;
            // Get local repo root?
            // Extrapolate to on-line repo?
            // Get issues (and filter?)
            // Generate and save report.
        }
    }
}

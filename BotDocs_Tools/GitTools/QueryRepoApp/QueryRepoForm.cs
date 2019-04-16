using RepoTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QueryRepoApp
{
    public partial class QueryRepoForm : Form
    {
        private const string outputFileBase = "QueryRepo_";

        private static readonly IReadOnlyList<string> RepoDirectories
            = new List<string> { "samples/javascript_nodejs", "samples/csharp_dotnetcore" }
            .AsReadOnly();
        private static readonly IReadOnlyList<string> FileExtensions
            = new List<string> { ".md", ".cs", ".js", ".json", ".env" }
            .AsReadOnly();

        private string outfile;

        public QueryRepoForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SinceDatePicker.MaxDate = DateTime.Now;
            SinceDatePicker.MinDate = DateTime.Now.AddMonths(-1);
        }

        private void RepoRootTextBox_TextChanged(object sender, EventArgs e)
        {
            FolderPicker.SelectedPath = RepoRootTextBox.Text;
        }

        private void SelectRepoButton_Click(object sender, EventArgs e)
        {
            Enabled = false;
            var result = FolderPicker.ShowDialog();
            if (result == DialogResult.OK)
            {
                RepoRootTextBox.Text = FolderPicker.SelectedPath;
            }
            Enabled = true;
        }

        private void OutputDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
            OutputBrowser.SelectedPath = OutputDirectoryTextBox.Text;
        }

        private void SelectDirectoryButton_Click(object sender, EventArgs e)
        {
            Enabled = false;
            var result = OutputBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                OutputDirectoryTextBox.Text = OutputBrowser.SelectedPath;
            }
            Enabled = true;
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            Enabled = false;
            OutputRichTextBox.Clear();

            Run();

            Enabled = true;
        }

        private void Run()
        {
            var root = RepoRootTextBox.Text;
            var outdir = OutputDirectoryTextBox.Text;

            if (!Directory.Exists(root))
            {
                OutputRichTextBox.AppendText(
                    $"Repo directory `{root}` does not exist!");
                return;
            }
            if (!Directory.Exists(Path.Combine(root, ".git")))
            {
                OutputRichTextBox.AppendText(
                    $"Repo directory `{root}` does not contain a repository!");
                return;
            }
            if (!Directory.Exists(outdir))
            {
                OutputRichTextBox.AppendText(
                    $"output directory `{outdir}` does not exist!");
                return;
            }

            Dictionary<string, ChangeInfo> changes = null;
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                Console.SetError(writer);

                var now = DateTimeOffset.Now;
                var logFile = outputFileBase + now.AsShortDate() + ".csv";
                outfile = Path.Combine(OutputDirectoryTextBox.Text, logFile);

                var helper = new RepoHelper
                {
                    AcceptableDirectories = RepoDirectories,
                    AcceptableExtensions = FileExtensions,
                    SinceDate = SinceDatePicker.Value,
                };

                using (var repo = helper.GetRepository(RepoRootTextBox.Text))
                {
                    changes = helper.GetChanges(repo);
                    if (changes is null)
                    {
                        Console.Error.WriteLine("Failed to generate content for the log.");
                    }
                }

                Console.WriteLine($"Generating log `{outfile}`...");

                Console.Out.Flush();
                Console.Error.Flush();
                OutputRichTextBox.Text = writer.ToString();
            }

            if (changes != null && changes.Count > 0)
            {
                try
                {
                    using (TextWriter writer = new StreamWriter(outfile, false))
                    {
                        writer.WriteLine(ChangeInfo.CsvHeader);
                        foreach (var change in changes.Values)
                        {
                            writer.WriteLine(change.AsCsv);
                        }

                        writer.Flush();
                        writer.Close();
                    }
                }
                catch (Exception ex)
                {
                    OutputRichTextBox.AppendText($"{ex.GetType().Name} encountered writing output." +
                        Environment.NewLine + ex.Message);
                    OutputRichTextBox.AppendText("No information gathered to report.");
                }
            }
            else
            {
                OutputRichTextBox.AppendText("No information gathered to report.");
            }
        }
    }
}

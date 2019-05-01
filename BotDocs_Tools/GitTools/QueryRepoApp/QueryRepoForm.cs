using RepoTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ReportUtils;

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

        private void QueryRepoForm_Load(object sender, EventArgs e)
        {
            DatePicker.MaxDate = DateTime.Now;
            DatePicker.MinDate = DateTime.Now.AddMonths(-1);

            dlg_ChooseRepoRoot.SelectedPath =
                string.IsNullOrWhiteSpace(Properties.Settings.Default.LastRepoRoot)
                ? @"c:\" : Properties.Settings.Default.LastRepoRoot;
            RepoRootTextBox.Text = dlg_ChooseRepoRoot.SelectedPath;

            dlg_SaveOutput.InitialDirectory =
                string.IsNullOrWhiteSpace(Properties.Settings.Default.LastOutputDirectory)
                ? @"c:\" : Properties.Settings.Default.LastOutputDirectory;
        }

        private void QueryRepoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void RepoRoot_TextChanged(object sender, EventArgs e)
        {
            dlg_ChooseRepoRoot.SelectedPath = RepoRootTextBox.Text;
        }

        private void SelectRepoButton_Click(object sender, EventArgs e)
        {
            Enabled = false;
            var result = dlg_ChooseRepoRoot.ShowDialog();
            if (result == DialogResult.OK)
            {
                RepoRootTextBox.Text = dlg_ChooseRepoRoot.SelectedPath;
            }
            Enabled = true;
        }

        private void RunAkaLinkReport_Click(object sender, EventArgs e)
        {
            Enabled = false;
            rtb_Output.Clear();

            var report = new AkaLinkReport(rtb_Output, dlg_SaveOutput) { DocPath = RepoRootTextBox.Text };
            report.Run();
            rtb_Output.ScrollToCaret();

            Enabled = true;
        }

        private void Run()
        {
            var root = RepoRootTextBox.Text;
            Properties.Settings.Default.LastRepoRoot = root;

            if (!Directory.Exists(root))
            {
                Warning($"Repo directory `{root}` does not exist!");
                return;
            }
            if (!Directory.Exists(Path.Combine(root, ".git")))
            {
                Warning($"Repo directory `{root}` does not contain a repository!");
                return;
            }

            Dictionary<string, ChangeInfo> changes = null;
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                Console.SetError(writer);

                var helper = new RepoHelper
                {
                    AcceptableDirectories = RepoDirectories,
                    AcceptableExtensions = FileExtensions,
                    SinceDate = DatePicker.Value,
                };

                using (var repo = helper.GetRepository(RepoRootTextBox.Text))
                {
                    changes = helper.GetChanges(repo);
                    if (changes is null)
                    {
                        Error("Failed to generate content for the log.");
                    }
                }


                Console.Out.Flush();
                Console.Error.Flush();
                Message(writer.ToString());
            }

            if (changes != null && changes.Count > 0)
            {
                try
                {
                    Info($"Found {changes.Count} changed files.");
                    rtb_Output.ScrollToCaret();

                    var now = DateTimeOffset.Now;
                    outfile = outputFileBase + now.AsShortDate() + ".csv";
                    dlg_SaveOutput.FileName = Path.Combine(dlg_SaveOutput.InitialDirectory, outfile);
                    var result = dlg_SaveOutput.ShowDialog();

                    if (result is DialogResult.OK)
                    {
                        outfile = dlg_SaveOutput.FileName;
                        Properties.Settings.Default.LastOutputDirectory = Path.GetDirectoryName(outfile);

                        Info($"Generating log `{outfile}`...");
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
                        Info("Done");
                    }
                    else
                    {
                        Info("No report saved.");
                    }
                }
                catch (Exception ex)
                {
                    Warning($"{ex.GetType().Name} encountered writing output:" + Environment.NewLine + ex.Message);
                }
            }
            else
            {
                Info("No information gathered to report.");
            }
        }

        private void Message(string message)
        {
            rtb_Output.WriteLine(Severity.Messgae, message);
        }

        private void Info(string message)
        {
            rtb_Output.WriteLine(Severity.Information, message);
        }

        private void Warning(string message)
        {
            rtb_Output.WriteLine(Severity.Warning, message);
        }

        private void Error(string message)
        {
            rtb_Output.WriteLine(Severity.Error, message);
        }
    }
}

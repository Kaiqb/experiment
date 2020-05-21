using ReportUtils;
using RepoTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Utilities;

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

        private ItemPicker dlg_ItemPicker;

        public QueryRepoForm()
        {
            InitializeComponent();

            dlg_ItemPicker = new ItemPicker();
        }

        private string LastDocRepoRoot { get; set; }
        private string LastCodeRepoRoot { get; set; }
        private string LastOutputDirectory { get; set; }

        private void QueryRepoForm_Load(object sender, EventArgs e)
        {
            //DatePicker.MaxDate = DateTime.Now;
            //DatePicker.MinDate = DateTime.Now.AddMonths(-1);

            //var defaultPath = Path.Combine(
            //    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            //    @"..\source\repos");
            var home = Environment.GetEnvironmentVariable("HOMEPATH");
            var defaultPath = string.IsNullOrWhiteSpace(home)
                ? @"c:\"
                : Path.Combine(home, @"source\repos");

            LastDocRepoRoot = string.IsNullOrWhiteSpace(Properties.Settings.Default.LastDocRepoRoot)
                ? defaultPath
                : Properties.Settings.Default.LastDocRepoRoot;
            LastCodeRepoRoot = string.IsNullOrWhiteSpace(Properties.Settings.Default.LastCodeRepoRoot)
                ? defaultPath
                : Properties.Settings.Default.LastCodeRepoRoot;
            LastOutputDirectory = string.IsNullOrWhiteSpace(Properties.Settings.Default.LastOutputDirectory)
                ? @"C:\"
                : Properties.Settings.Default.LastOutputDirectory;

            tb_DocRepoRoot.Text = LastDocRepoRoot;
            tb_CodeRepoRoot.Text = LastCodeRepoRoot;
            dateTimePicker.Checked = false;
            dateTimePicker.MinDate = DateTime.Now.AddYears(-2);
            dateTimePicker.MaxDate = DateTime.Now;
            dateTimePicker.Value = DateTime.Now.AddMonths(-1);
            dlg_SaveOutput.InitialDirectory = LastOutputDirectory;
        }

        private void QueryRepoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dlg_ItemPicker != null)
            {
                dlg_ItemPicker.Dispose();
                dlg_ItemPicker = null;
            }

            Properties.Settings.Default.Save();
        }

        private void SelectDocRepoButton_Click(object sender, EventArgs e)
        {
            Enabled = false;
            dlg_ChooseRepoRoot.SelectedPath = LastDocRepoRoot;
            dlg_ChooseRepoRoot.Description = "Choose the local folder for the doc repo";
            var result = dlg_ChooseRepoRoot.ShowDialog();
            if (result == DialogResult.OK
                && tb_DocRepoRoot.Text != dlg_ChooseRepoRoot.SelectedPath)
            {
                LastDocRepoRoot = dlg_ChooseRepoRoot.SelectedPath;
                tb_DocRepoRoot.Text = LastDocRepoRoot;
                Properties.Settings.Default.LastDocRepoRoot = LastDocRepoRoot;
                Properties.Settings.Default.Save();
            }
            Enabled = true;
        }

        private void SelectCodeRepoButton_Click(object sender, EventArgs e)
        {
            Enabled = false;
            dlg_ChooseRepoRoot.SelectedPath = LastCodeRepoRoot;
            dlg_ChooseRepoRoot.Description = "Choose the local folder for the code repo";
            var result = dlg_ChooseRepoRoot.ShowDialog();
            if (result == DialogResult.OK
                && tb_CodeRepoRoot.Text != dlg_ChooseRepoRoot.SelectedPath)
            {
                LastCodeRepoRoot = dlg_ChooseRepoRoot.SelectedPath;
                tb_CodeRepoRoot.Text = LastCodeRepoRoot;
                Properties.Settings.Default.LastCodeRepoRoot= LastCodeRepoRoot;
                Properties.Settings.Default.Save();
            }
            Enabled = true;
        }

        private void RunAkaLinkReport_Click(object sender, EventArgs e)
        {
            Enabled = false;
            Cursor = Cursors.WaitCursor;
            rtb_Output.Clear();

            var report = new AkaLinkReport(rtb_Output, dlg_SaveOutput)
            {
                DocPath = tb_DocRepoRoot.Text,
            };
            report.Run();

            rtb_Output.ScrollToCaret();
            Cursor = DefaultCursor;
            Enabled = true;
        }

        private void RunCodeLinkReport_Click(object sender, EventArgs e)
        {
            Enabled = false;
            Cursor = Cursors.WaitCursor;
            rtb_Output.Clear();

            var report = new CodeLinkReport(rtb_Output, dlg_ChooseRepoRoot, dlg_SaveOutput)
            {
                DocPath = tb_DocRepoRoot.Text,
            };
            if (dateTimePicker.Checked) { report.FreshnessDate = dateTimePicker.Value; }

            var (codeInfo, codeRoot) = SelectCodeRepo(report);
            if (codeRoot != null)
            {
                report.SetCodeTarget(codeInfo, codeRoot);
                report.Run();
            }

            rtb_Output.ScrollToCaret();
            Cursor = DefaultCursor;
            Enabled = true;
        }

        private (RepositoryInfo, string) SelectCodeRepo(CodeLinkReport report)
        {
            var depRepos = report.GetPotentialTargets();
            RepositoryInfo codeInfo = null;
            string codeRoot = null;
            DialogResult result;
            switch (depRepos.Count)
            {
                case 0:
                    // No linked code repos!
                    break;
                case 1:
                    // Just one linked [potential] code repo.
                    codeInfo = depRepos[0];

                    if (!string.IsNullOrWhiteSpace(LastCodeRepoRoot))
                    {
                        // Is this the right directory?
                        if (MessageBox.Show(
                            $"{LastCodeRepoRoot}, is this the correct code directory?",
                            "Verify directory",
                            MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            codeRoot = LastCodeRepoRoot;
                            break;
                        }
                    }

                    dlg_ChooseRepoRoot.Description = $"Select the local root for the {codeInfo.Branch} " +
                        $"branch of the '{codeInfo.PathToRoot}' repo.";
                    result = dlg_ChooseRepoRoot.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        // Abort.
                        break;
                    }
                    codeRoot = dlg_ChooseRepoRoot.SelectedPath;
                    break;
                default:
                    // More than one to choose from.
                    // TODO
                    // - rework this as a loop where each is resolved or ignored.
                    // - will require rewiring the report logic to work against multiple repos.
                    dlg_ItemPicker.SetItems(depRepos);
                    result = dlg_ItemPicker.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        break;
                    }
                    codeInfo = depRepos[dlg_ItemPicker.SelectedIndex];
                    dlg_ChooseRepoRoot.Description = $"Select the local root for the {codeInfo.Branch} " +
                        $"branch of the '{codeInfo.PathToRoot}' repo.";
                    result = dlg_ChooseRepoRoot.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        break;
                    }
                    codeRoot = dlg_ChooseRepoRoot.SelectedPath;
                    LastCodeRepoRoot = codeRoot;
                    tb_CodeRepoRoot.Text = codeRoot;
                    Properties.Settings.Default.LastCodeRepoRoot = codeRoot;
                    Properties.Settings.Default.Save();
                    break;
            }

            return (codeInfo, codeRoot);
        }

        // This is the old code link report
        //private void Run()
        //{
        //    var root = tb_DocRepoRoot.Text;
        //    Properties.Settings.Default.LastDocRepoRoot = root;

        //    if (!Directory.Exists(root))
        //    {
        //        Warning($"Repo directory `{root}` does not exist!");
        //        return;
        //    }
        //    if (!Directory.Exists(Path.Combine(root, ".git")))
        //    {
        //        Warning($"Repo directory `{root}` does not contain a repository!");
        //        return;
        //    }

        //    Dictionary<string, ChangeInfo> changes = null;
        //    using (var writer = new StringWriter())
        //    {
        //        Console.SetOut(writer);
        //        Console.SetError(writer);

        //        var helper = new RepoHelper
        //        {
        //            AcceptableDirectories = RepoDirectories,
        //            AcceptableExtensions = FileExtensions,
        //            //SinceDate = DatePicker.Value,
        //        };

        //        using (var repo = helper.GetRepository(tb_DocRepoRoot.Text))
        //        {
        //            changes = helper.GetChanges(repo, DateTimeOffset.Now.AddDays(-14));
        //            if (changes is null)
        //            {
        //                Error("Failed to generate content for the log.");
        //            }
        //        }

        //        Console.Out.Flush();
        //        Console.Error.Flush();
        //        Message(writer.ToString());
        //    }

        //    if (changes != null && changes.Count > 0)
        //    {
        //        try
        //        {
        //            Info($"Found {changes.Count} changed files.");
        //            rtb_Output.ScrollToCaret();

        //            var now = DateTimeOffset.Now;
        //            outfile = outputFileBase + now.AsShortDate() + ".csv";
        //            dlg_SaveOutput.FileName = Path.Combine(dlg_SaveOutput.InitialDirectory, outfile);
        //            var result = dlg_SaveOutput.ShowDialog();

        //            if (result is DialogResult.OK)
        //            {
        //                outfile = dlg_SaveOutput.FileName;
        //                Properties.Settings.Default.LastOutputDirectory = Path.GetDirectoryName(outfile);

        //                Info($"Generating log `{outfile}`...");
        //                using (TextWriter writer = new StreamWriter(outfile, false))
        //                {
        //                    writer.WriteLine(ChangeInfo.CsvHeader);
        //                    foreach (var change in changes.Values)
        //                    {
        //                        writer.WriteLine(change.AsCsv);
        //                    }

        //                    writer.Flush();
        //                    writer.Close();
        //                }
        //                Info("Done");
        //            }
        //            else
        //            {
        //                Info("No report saved.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Warning($"{ex.GetType().Name} encountered writing output:" + Environment.NewLine + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        Info("No information gathered to report.");
        //    }
        //}

        //private void Message(string message)
        //{
        //    rtb_Output.WriteLine(Severity.Messgae, message);
        //}

        //private void Info(string message)
        //{
        //    rtb_Output.WriteLine(Severity.Information, message);
        //}

        //private void Warning(string message)
        //{
        //    rtb_Output.WriteLine(Severity.Warning, message);
        //}

        //private void Error(string message)
        //{
        //    rtb_Output.WriteLine(Severity.Error, message);
        //}
    }
}

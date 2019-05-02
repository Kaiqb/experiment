﻿using RepoTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ReportUtils;
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

        private void QueryRepoForm_Load(object sender, EventArgs e)
        {
            //DatePicker.MaxDate = DateTime.Now;
            //DatePicker.MinDate = DateTime.Now.AddMonths(-1);

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
            if (dlg_ItemPicker != null)
            {
                dlg_ItemPicker.Dispose();
                dlg_ItemPicker = null;
            }

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

            var report = new AkaLinkReport(rtb_Output, dlg_SaveOutput)
            {
                DocPath = RepoRootTextBox.Text,
            };
            report.Run();

            rtb_Output.ScrollToCaret();

            Enabled = true;
        }

        private void btn_RunCodeLinkReport_Click(object sender, EventArgs e)
        {
            Enabled = false;
            rtb_Output.Clear();

            var report = new CodeLinkReport(rtb_Output, dlg_ChooseRepoRoot, dlg_SaveOutput)
            {
                DocPath = RepoRootTextBox.Text,
            };
            RepositoryInfo codeInfo;
            string codeRoot;
            (codeInfo, codeRoot) = SelectCodeRepo(report);
            if (codeRoot != null)
            {
                report.SetCodeTarget(codeInfo, codeRoot);
                report.Run();
            }

            rtb_Output.ScrollToCaret();
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
                    break;
            }

            return (codeInfo, codeRoot);
        }

        // This is the old code link report
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
                    //SinceDate = DatePicker.Value,
                };

                using (var repo = helper.GetRepository(RepoRootTextBox.Text))
                {
                    changes = helper.GetChanges(repo, DateTimeOffset.Now.AddDays(-14));
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

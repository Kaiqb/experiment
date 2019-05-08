using LibGit2Sharp;
using Newtonsoft.Json;
using RepoTools;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utilities;

namespace ReportUtils
{
    public class CodeLinkReport : BaseReport
    {
        public FolderBrowserDialog ChooseFolder { get; private set; }

        public string DocPath { get; set; }

        public string CodePath { get; private set; }

        public RepositoryInfo CodeInfo { get; private set; }

        private CodeLinkMap LinkMap { get; } = new CodeLinkMap();

        private bool ReadyToRun { get; set; } = false;

        public CodeLinkReport(
            RichTextBox status, FolderBrowserDialog chooseFolder, SaveFileDialog saveDialog)
            : base(status, saveDialog)
        {
            ChooseFolder = chooseFolder;
        }

        public IList<RepositoryInfo> GetPotentialTargets()
        {
            base.Run();

            Contract.Requires(DocPath != null);
            Contract.Requires(Directory.Exists(DocPath));
            Contract.Requires(Directory.Exists(Path.Combine(DocPath, ".git")));
            Contract.Requires(Directory.Exists(Path.Combine(DocPath, ArticlesRoot)));
            Contract.Requires(File.Exists(Path.Combine(DocPath, DocConfigFile)));

            var file = Path.Combine(DocPath, DocConfigFile);
            var config = JsonConvert.DeserializeObject<PublishConfig>(File.ReadAllText(file));

            return config.DependentRepositories.Where(r => !r.PathToRoot.StartsWith("_theme")).ToList();
        }

        public void SetCodeTarget(RepositoryInfo codeInfo, string codeRoot)
        {
            CodePath = codeRoot;
            CodeInfo = codeInfo;
        }

        public override bool Run()
        {
            if (CodeInfo is null || CodePath is null)
            {
                Status.WriteLine(Severity.Error,
                    "Application error. Must choose the code repo against which to run this report.");
            }

            Contract.Requires(Directory.Exists(CodePath));
            Contract.Requires(Directory.Exists(Path.Combine(CodePath, ".git")));

            LinkMap.Clear();

            var helper = new RepoHelper();

            using (var docRepo = helper.GetRepository(DocPath))
            using (var codeRepo = helper.GetRepository(CodePath))
            {
                FindRelevantCodeLinks(docRepo, codeRepo);
                BackfillCommitDates(LinkMap.DocFileIndex.Keys, docRepo, helper);
                BackfillCommitDates(LinkMap.CodeFileIndex.Keys, codeRepo, helper);
            }

            if (LinkMap.IsEmpty)
            {
                Status.WriteLine(Severity.Warning, "No code links found in this repo.");
                return false;
            }
            else
            {
                Status.WriteLine(Severity.Information, "Found links to " +
                    $"{LinkMap.CodeFileIndex.Count} code files, linked to from" +
                    $"{LinkMap.DocFileIndex.Count} doc files.");

                SaveDialog.Title = "Choose where to save the code link report:";
                var result = SaveDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WriteReport();
                    Status.WriteLine(Severity.Information, $"Report written to {SaveDialog.FileName}.");
                    return true;
                }
            }
            Status.WriteLine(Severity.Warning, "No report written.");
            return false;
        }

        private void FindRelevantCodeLinks(Repository docRepo, Repository codeRepo)
        {
            var dir = Path.Combine(DocPath, ArticlesRoot);
            var files = Directory.GetFiles(dir, "*.md", SearchOption.AllDirectories);
            var pattern1 = "[!code-";
            var pattern2 = $"](~/../{CodeInfo.PathToRoot}/";

            foreach (var file in files)
            {
                var relDocPath = file.Substring(DocPath.Length+1);
                var docFile = new CodeLinkMap.FileData
                {
                    BranchName = docRepo.Head.FriendlyName,
                    RelFilePath = relDocPath.Replace('\\','/'),
                };
                var lines = File.ReadAllLines(file).ToList();
                for (var lineNum = 0; lineNum < lines.Count; lineNum++)
                {
                    var line = lines[lineNum];
                    if (line.StartsWith(pattern1, System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        var pos = line.IndexOf(pattern2);
                        if (pos < 0)
                        {
                            Status.WriteLine(Severity.Information,
                                $"Ignoring other-repo code link in {relDocPath} at line {lineNum}.");
                            continue;
                        }
                        var start = pos + pattern2.Length;
                        pos = line.IndexOf('?', start);
                        var end = line.IndexOf(")]", start);
                        if (end < 0)
                        {
                            Status.WriteLine(Severity.Warning,
                                $"Poorly formed code link in {relDocPath} at line {lineNum}.");
                            continue;
                        }
                        string relCodePath, queryParameters;
                        if (pos < 0)
                        {
                            relCodePath = line.Substring(start, end - start);
                            queryParameters = null;
                        }
                        else
                        {
                            relCodePath = line.Substring(start, pos - start);
                            pos++;
                            queryParameters = line.Substring(pos, end - pos);
                        }
                        var codeFile = new CodeLinkMap.FileData
                        {
                            BranchName = codeRepo.Head.FriendlyName,
                            RelFilePath = relCodePath,
                        };
                        LinkMap.Add(docFile, codeFile, lineNum, queryParameters);
                    }
                }
            }
        }

        private void BackfillCommitDates(
            IEnumerable<CodeLinkMap.FileData> files, Repository repo, RepoHelper helper)
        {
            // This is currently a very inefficient algorithm.

            // In theory, we just want information about the last commit to touch each file.
            var filesMap = files.ToDictionary(f => f.RelFilePath);
            var todo = filesMap.Count;
            Status.WriteLine(Severity.Information, $"Looking for commit information for {todo} files in repo {repo}.");
            Status.ScrollToCaret();

            // Ignore merge commits (with 2 parents).
            foreach (var commit in repo.Commits.Where(c => c.Parents.Count() is 1))
            {
                // Search each change in each commit for the files we're interested in.
                var diff = repo.Diff.Compare<TreeChanges>(commit.Parents.First().Tree, commit.Tree);
                foreach (var change in diff.Where(i => i.Status != ChangeKind.Unmodified))
                {
                    if (filesMap.ContainsKey(change.Path))
                    {
                        // If we've found a match, fill in the commit information.
                        var fileInfo = filesMap[change.Path];

                        fileInfo.CommitSha = commit.Id.Sha;
                        fileInfo.Author = commit.Author.Name;
                        fileInfo.LastCommitDate = commit.Author.When;
                        fileInfo.LastChangeStatus = change.Status.ToString();

                        // Remove this file from the list of files to search for.
                        if (filesMap.Remove(change.Path))
                        {
                            todo--;
                        }

                        if (todo is 0)
                        {
                            break;
                        }
                    }
                }
                if (todo is 0)
                {
                    break;
                }
            }
            if (filesMap.Count > 0)
            {
                Status.WriteLine(Severity.Warning, $"Failed to get commit data for {filesMap.Count} files.");
                foreach (var fileInfo in filesMap.Values)
                {
                    fileInfo.CommitSha = "unknown";
                    fileInfo.Author = "unknown";
                    fileInfo.LastChangeStatus = "unknown";
                }
            }
        }

        private void WriteReport()
        {
            using (TextWriter writer = new StreamWriter(SaveDialog.FileName, false))
            {
                writer.WriteLine(
                    "Doc file path" +
                    ",Code file path" +
                    ",Line in doc" +
                    ",Code query params" +
                    ",Doc repo branch" +
                    ",Doc commit date" +
                    ",Doc commit author" +
                    ",Code repo handle" +
                    ",Code repo branch" +
                    ",Code commit date" +
                    ",Code commit author" +
                    "");

                // Write out all the link data.
                foreach (var entry in LinkMap.DocFileIndex)
                {
                    var docFile = entry.Key;
                    foreach (var subentry in entry.Value)
                    {
                        var codeFile = subentry.Key;
                        foreach (var lineData in subentry.Value)
                        {
                            writer.WriteLine(
                                docFile.RelFilePath.CsvEscape()
                                + "," + codeFile.RelFilePath.CsvEscape()
                                + "," + lineData.DocLine
                                + "," + lineData.QueryParams.CsvEscape()
                                + "," + docFile.BranchName.CsvEscape()
                                + "," + docFile.LastCommitDate.ToString().CsvEscape()
                                + "," + docFile.Author.CsvEscape()
                                + "," + CodeInfo.PathToRoot.CsvEscape()
                                + "," + codeFile.BranchName.CsvEscape()
                                + "," + codeFile.LastCommitDate.ToString().CsvEscape()
                                + "," + codeFile.Author.CsvEscape()
                                );
                        }
                    }
                }

                writer.Flush();
                writer.Close();
            }
        }
    }
}

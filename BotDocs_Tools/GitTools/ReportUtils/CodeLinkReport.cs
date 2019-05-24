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
        private class PathComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(string x)
            {
                return x.GetHashCode();
            }

            private PathComparer() { }

            public static PathComparer Default { get; } = new PathComparer();
        }

        public FolderBrowserDialog ChooseFolder { get; private set; }

        public string DocPath { get; set; }

        public string CodePath { get; private set; }

        public RepositoryInfo CodeInfo { get; private set; }

        public DateTimeOffset? FreshnessDate { get; set; }

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
                    $"{LinkMap.CodeFileIndex.Count} code files, linked to from " +
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
                var relDocPath = file.Substring(DocPath.Length + 1);
                var docFile = new CodeLinkMap.FileData
                {
                    BranchName = docRepo.Head.FriendlyName,
                    RelFilePath = relDocPath.Trim().Replace('\\', '/'),
                };

                var lines = File.ReadAllLines(file).ToList();
                for (var lineNum = 0; lineNum < lines.Count; lineNum++)
                {
                    // The array of lines is 0-based, but "line numbers" are 1-based.
                    var line = lines[lineNum];
                    if (line.StartsWith(pattern1, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var pos = line.IndexOf(pattern2, StringComparison.InvariantCultureIgnoreCase);
                        if (pos < 0)
                        {
                            Status.WriteLine(Severity.Information,
                                $"Ignoring link in {relDocPath,-64} line {lineNum + 1,3}:  {line}");
                            continue;
                        }

                        var start = pos + pattern2.Length;
                        var end = line.IndexOf(")]", start);
                        if (end < 0)
                        {
                            Status.WriteLine(Severity.Warning,
                                $"Poorly formed code link in {relDocPath} at line {lineNum + 1}.");
                            continue;
                        }

                        pos = line.IndexOf('?', start);
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
                            RelFilePath = relCodePath.Trim(),
                        };

                        LinkMap.Add(docFile, codeFile, lineNum + 1, queryParameters);
                    }
                }
            }
        }

        private void BackfillCommitDates(
            IEnumerable<CodeLinkMap.FileData> files, Repository repo, RepoHelper helper)
        {
            // This is currently a very inefficient algorithm.
            // In theory, we just want information about the last commit to touch each file.

            //var filesMap = new Dictionary<string, CodeLinkMap.FileData>(files.Count());
            //foreach (var file in files) { filesMap[file.RelFilePath] = file; }
            var filesMap = files.ToDictionary(f => f.RelFilePath, f => f, CodeLinkMap.FileData.PathComparer);

            var todo = filesMap.Count;
            Status.WriteLine(Severity.Information, $"Looking for commit information for {todo} files in repo.");
            Status.ScrollToCaret();

            // Ignore merge commits (with 2 parents).
            foreach (var commit in repo.Commits.Where(c => c.Parents.Count() is 1))
            {
                // Search each change in each commit for the files we're interested in.
                var diff = repo.Diff.Compare<TreeChanges>(commit.Parents.First().Tree, commit.Tree);
                foreach (var change in diff.Where(i => i.Status != ChangeKind.Unmodified))
                {
                    // Not sure why `filesMap.ContainsKey(change.Path)` was having issues with string casing.
                    var key = filesMap.Keys.FirstOrDefault(
                        k => string.Equals(change.Path, k, StringComparison.InvariantCultureIgnoreCase));
                    if (key != null)
                    {
                        // If we've found a match, fill in the commit information.
                        var fileEntry = filesMap[key];

                        fileEntry.CommitSha = commit.Id.Sha;
                        fileEntry.Author = commit.Author.Name;
                        fileEntry.LastCommitDate = commit.Author.When;
                        fileEntry.LastChangeStatus = change.Status.ToString();

                        // Remove this file from the list of files to search for.
                        // Use the key that was used to create the dictionary, not the path used to find the entry.
                        if (filesMap.Remove(key))
                        {
                            todo--;
                        }
                        else
                        {
                            Status.WriteLine(Severity.Error,
                                $"Failed to remove {change.Path} from the temporary collection.");
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
                Status.WriteLine(Severity.Warning,
                    $"Failed to get commit data for {filesMap.Count} files.");
                foreach (var fileEntry in filesMap.Values)
                {
                    fileEntry.CommitSha = "unknown";
                    fileEntry.Author = "unknown";
                    fileEntry.LastChangeStatus = "unknown";
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
                    ",Doc commit author" +
                    ",Doc commit date" +
                    ",Code repo handle" +
                    ",Code repo branch" +
                    ",Code commit author" +
                    ",Code commit date" +
                    ",Code fresher than doc" +
                    ",Since date" +
                    ",Code fresher than since date" +
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
                            writer.Write(docFile.RelFilePath.CsvEscape());
                            writer.Write("," + codeFile.RelFilePath.CsvEscape());
                            writer.Write("," + lineData.DocLine);
                            writer.Write("," + lineData.QueryParams.CsvEscape());
                                writer.Write("," + docFile.BranchName.CsvEscape());
                            writer.Write("," + docFile.Author.CsvEscape());
                            writer.Write("," + docFile.LastCommitDate.ToString().CsvEscape());
                            writer.Write("," + CodeInfo.PathToRoot.CsvEscape());
                            writer.Write("," + codeFile.BranchName.CsvEscape());
                            writer.Write("," + codeFile.Author.CsvEscape());
                            writer.Write("," + codeFile.LastCommitDate.ToString().CsvEscape());
                            writer.Write("," + (codeFile.LastCommitDate > docFile.LastCommitDate));

                            if (FreshnessDate.HasValue)
                            {
                                writer.Write("," + FreshnessDate.Value.ToString().CsvEscape());
                                writer.Write("," + (codeFile.LastCommitDate > FreshnessDate.Value));
                            }
                            else
                            {
                                writer.Write(",na,na");
                            }

                            writer.WriteLine();
                        }
                    }
                }

                writer.Flush();
                writer.Close();
            }
        }
    }
}

using LibGit2Sharp;
using ReportUtils;
using System;
using System.IO;

namespace RepoTools
{
    /// <summary>Describes the last [relevant] change to a file.</summary>
    public class ChangeInfo
    {
        public ChangeInfo(Commit commit, TreeEntryChanges change)
        {
            Author = commit.Author.Name;
            CommitId = commit.Id;
            Directory = Path.GetDirectoryName(change.Path);
            Extension = Path.GetExtension(change.Path);
            FileName = Path.GetFileName(change.Path);
            FullPath = change.Path;
            Kind = change.Status;
            When = commit.Author.When;
        }

        /// <summary>The change date-time with offset.</summary>
        public DateTimeOffset When { get; private set; }

        /// <summary>The commit ID.</summary>
        public ObjectId CommitId { get; private set; }

        /// <summary>The commit author's name.</summary>
        public string Author { get; private set; }

        /// <summary>The file's change "kind".</summary>
        public ChangeKind Kind { get; private set; }

        /// <summary>The file's full path.</summary>
        public string FullPath { get; private set; }

        /// <summary>The file's directory.</summary>
        public string Directory { get; private set; }

        /// <summary>The file's name.</summary>
        public string FileName { get; private set; }

        /// <summary>The file's extension.</summary>
        public string Extension { get; private set; }

        /// <summary>Gets the change info as a properly-escaped set of comma-separated values.</summary>
        public string AsCsv
        {
            get
            {
                return When.ToString().CsvEscape()
                    + "," + CommitId.Sha.CsvEscape()
                    + "," + Author.CsvEscape()
                    + "," + Kind.ToString().CsvEscape()
                    + "," + FullPath.CsvEscape()
                    + "," + Directory.CsvEscape()
                    + "," + FileName.CsvEscape()
                    + "," + Extension.CsvEscape();
            }
        }

        /// <summary>A heading entry to use when generating CSV output.</summary>
        public static string CsvHeader
            = "When,Commit ID (sha),Author,Kind,Full Path,Directory,File Name,Extension";
    }
}

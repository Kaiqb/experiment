using System;
using System.Collections.Generic;

namespace ReportUtils
{
    public class CodeLinkMap
    {
        public class FileData : IEquatable<FileData>
        {
            public string RepoName { get; set; }
            public string RelFilePath { get; set; }
            public DateTimeOffset LastCommitDate { get; set; }
            public string CommitSha { get; set; }

            public bool Equals(FileData other)
            {
                // For now, we don't have to worry if these are of the same type or not.
                return other != null
                    && other.RepoName.Equals(RepoName, StringComparison.InvariantCultureIgnoreCase)
                    && other.RelFilePath.Equals(RelFilePath, StringComparison.InvariantCultureIgnoreCase);
            }

            public override int GetHashCode()
            {
                // Provide a simple hash code that will work with our sense of Equals.
                return RepoName.GetHashCode() << 8 ^ RelFilePath.GetHashCode() >> 8;
            }

            public override bool Equals(object obj)
            {
                // Provide this so we can use this class as a Dictionary key.
                return Equals(obj as FileData);
            }
        }

        /// <summary>Describes a code link in a doc file.</summary>
        public class CodeLinkData : IEquatable<CodeLinkData>
        {
            /// <summary>The line number in the doc file containing the link.</summary>
            public int DocLine { get; set; }

            /// <summary>The query parameters of the code link URL.</summary>
            /// <remarks>This includes the code lines of interest.</remarks>
            public string QueryParams { get; set; }

            public bool Equals(CodeLinkData other)
            {
                // There can be only one code link per line.
                return other != null
                    && other.DocLine.Equals(DocLine);
            }

            public override int GetHashCode()
            {
                return DocLine.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                // Do this to allow this class to be used in a hash set properly.
                return Equals(obj as CodeLinkData);
            }
        }

        /// <summary>Index of code links, indexed by doc file, then by code file.</summary>
        public Dictionary<FileData, Dictionary<FileData, HashSet<CodeLinkData>>> DocFileIndex { get; }
            = new Dictionary<FileData, Dictionary<FileData, HashSet<CodeLinkData>>>();

        /// <summary>Index of code links, indexed by code file, then by doc file.</summary>
        public Dictionary<FileData, Dictionary<FileData, HashSet<CodeLinkData>>> CodeFileIndex { get; }
            = new Dictionary<FileData, Dictionary<FileData, HashSet<CodeLinkData>>>();

        public void Clear()
        {
            DocFileIndex.Clear();
            CodeFileIndex.Clear();
        }

        public bool Add(FileData docFile, FileData codeFile, int docLine, string queryParams)
        {
            var added = false;

            var link = new CodeLinkData { DocLine = docLine, QueryParams = queryParams };

            if (!DocFileIndex.ContainsKey(docFile))
            {
                DocFileIndex[docFile] = new Dictionary<FileData, HashSet<CodeLinkData>>();
            }
            if (!DocFileIndex[docFile].ContainsKey(codeFile))
            {
                DocFileIndex[docFile][codeFile] = new HashSet<CodeLinkData>();
            }
            added |= DocFileIndex[docFile][codeFile].Add(link);

            if (!CodeFileIndex.ContainsKey(codeFile))
            {
                CodeFileIndex[codeFile] = new Dictionary<FileData, HashSet<CodeLinkData>>();
            }
            if (!CodeFileIndex[codeFile].ContainsKey(docFile))
            {
                CodeFileIndex[codeFile][docFile] = new HashSet<CodeLinkData>();
            }
            added |= CodeFileIndex[codeFile][docFile].Add(link);

            // Every addition should be unique, unless we index the same line twice.
            return added;
        }

        public bool IsEmpty => DocFileIndex.Count is 0;
    }
}

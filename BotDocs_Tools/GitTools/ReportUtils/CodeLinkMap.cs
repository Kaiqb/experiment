using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportUtils
{
    /// <summary>Many-to-many mapping of doc files to code files, along with all associated line numbers.</summary>
    public class CodeLinkMap
    {
        /// <summary>Information about a file (doc or code) in a repo.</summary>
        public class FileData
        {
            public string BranchName { get; set; }
            public string RelFilePath { get; set; }
            public string CommitSha { get; set; }
            public string Author { get; set; }
            public DateTimeOffset LastCommitDate { get; set; }
            public string LastChangeStatus { get; set; }

            public static IEqualityComparer<FileData> FileComparer { get; } = Comparer.Default;
            public static IEqualityComparer<string> PathComparer { get; } = Comparer.Default;

            private class Comparer : IEqualityComparer<FileData>, IEqualityComparer<string>
            {
                public static Comparer Default { get; } = new Comparer();

                public bool Equals(FileData x, FileData y)
                {
                    return Equals(x.RelFilePath, y.RelFilePath);
                }

                public bool Equals(string x, string y)
                {
                    return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
                }

                public int GetHashCode(FileData obj)
                {
                    return GetHashCode(obj.RelFilePath);
                }

                public int GetHashCode(string obj)
                {
                    return obj.GetHashCode();
                }
            }
        }

        /// <summary>Describes a link to a code file from a doc file.</summary>
        public class LinkData
        {
            /// <summary>The line number in the doc file containing the link.</summary>
            public int DocLine { get; set; }

            /// <summary>The query parameters of the code link URL.</summary>
            /// <remarks>This includes the code lines of interest.</remarks>
            public string QueryParams { get; set; }

            public static IEqualityComparer<LinkData> LinkComparer { get; } = new Comparer();

            private class Comparer : IEqualityComparer<LinkData>
            {
                public bool Equals(LinkData x, LinkData y)
                {
                    return (x is null && y is null)
                        || (x != null && y != null && Equals(x.DocLine, y.DocLine));
                }

                public int GetHashCode(LinkData obj)
                {
                    return obj?.DocLine.GetHashCode() ?? 0;
                }
            }
        }

        /// <summary>Code link index, by doc file, by code file.</summary>
        public Dictionary<FileData, Dictionary<FileData, HashSet<LinkData>>> DocFileIndex { get; }
            = new Dictionary<FileData, Dictionary<FileData, HashSet<LinkData>>>(FileData.FileComparer);

        /// <summary>Code link index, by code file, by doc file.</summary>
        public Dictionary<FileData, Dictionary<FileData, HashSet<LinkData>>> CodeFileIndex { get; }
            = new Dictionary<FileData, Dictionary<FileData, HashSet<LinkData>>>(FileData.FileComparer);

        /// <summary>Removes all entries from the map.</summary>
        public void Clear()
        {
            DocFileIndex.Clear();
            CodeFileIndex.Clear();
        }

        /// <summary>Adds an entry to the map.</summary>
        /// <param name="docFile">Info about the doc file containing the link.</param>
        /// <param name="codeFile">Info about the code file linked to.</param>
        /// <param name="docLine">The doc file line number that contains the link.</param>
        /// <param name="queryParams">The query parameters associated with the link.</param>
        /// <returns>True if the entry was added; otherwise, false.</returns>
        public bool Add(FileData docFile, FileData codeFile, int docLine, string queryParams)
        {
            var added = false;

            FileData doc, code;
            var link = new LinkData { DocLine = docLine, QueryParams = queryParams };

            if (!DocFileIndex.ContainsKey(docFile))
            {
                doc = docFile;
                DocFileIndex[doc] = new Dictionary<FileData, HashSet<LinkData>>(FileData.FileComparer);
            }
            else
            {
                // else if the key and the incoming docFile are not the same object, use the existing key.
                doc = DocFileIndex.Keys.First(k => FileData.FileComparer.Equals(k, docFile));
            }
            if (!CodeFileIndex.ContainsKey(codeFile))
            {
                code = codeFile;
                CodeFileIndex[code] = new Dictionary<FileData, HashSet<LinkData>>(FileData.FileComparer);
            }
            else
            {
                // else if the key and the incoming codeFile are not the same object, use the existing key.
                code = CodeFileIndex.Keys.First(k => FileData.FileComparer.Equals(k, codeFile));
            }

            if (!DocFileIndex[doc].ContainsKey(code))
            {
                DocFileIndex[doc][code] = new HashSet<LinkData>(LinkData.LinkComparer);
            }
            added |= DocFileIndex[doc][code].Add(link);

            if (!CodeFileIndex[code].ContainsKey(doc))
            {
                CodeFileIndex[code][doc] = new HashSet<LinkData>(LinkData.LinkComparer);
            }
            added |= CodeFileIndex[code][doc].Add(link);

            // Every addition should be unique, unless we index the same line more than once.
            return added;
        }

        /// <summary>Indicates whether the map is currently empty (contains no entries).</summary>
        public bool IsEmpty => DocFileIndex.Count is 0;
    }
}

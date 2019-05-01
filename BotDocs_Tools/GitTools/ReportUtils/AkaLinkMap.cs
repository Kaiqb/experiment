using System.Collections.Generic;

namespace ReportUtils
{
    public class AkaLinkMap
    {
        public Dictionary<string, AkaFileData> FileIndex { get; }
            = new Dictionary<string, AkaFileData>();

        public Dictionary<string, AkaLinkData> LinkIndex { get; }
            = new Dictionary<string, AkaLinkData>();

        public void Clear()
        {
            FileIndex.Clear();
            LinkIndex.Clear();
        }

        public bool Add(string filePath, string akaLink)
        {
            var added = false;

            if (!FileIndex.ContainsKey(filePath))
            {
                FileIndex[filePath] = new AkaFileData { FullPath = filePath };
            }
            added |= FileIndex[filePath].ContainedAkaLinks.Add(akaLink);

            if (!LinkIndex.ContainsKey(akaLink))
            {
                LinkIndex[akaLink] = new AkaLinkData { Url = akaLink };
            }
            added |= LinkIndex[akaLink].FilesContainingLink.Add(filePath);

            return added;
        }

        public bool Add(string filePath, IEnumerable<string> links)
        {
            var added = false;
            foreach (var link in links)
            {
                added |= Add(filePath, link);
            }
            return added;
        }

        public bool IsEmpty => FileIndex.Count is 0;

    }
}

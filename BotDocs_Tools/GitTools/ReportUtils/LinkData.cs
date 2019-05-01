using System;
using System.Collections.Generic;

namespace ReportUtils
{
    public class LinkData
    {
        private const string akaRoot = "://aka.ms/";

        public string Url { get; set; }
        public HashSet<string> FilesContainingLink { get; } = new HashSet<string>();

        public static string GetShortUrl(string akaLink)
        {
                var i = akaLink.IndexOf(akaRoot, StringComparison.InvariantCultureIgnoreCase);
                return akaLink.Substring(i + akaRoot.Length);
        }
    }
}

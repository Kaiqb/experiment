using System;
using System.Collections.Generic;

namespace ReportUtils
{
    public class AkaLinkData
    {
        private const string akaRoot = "://aka.ms/";

        public string Url { get; set; }
        public HashSet<string> FilesContainingLink { get; } = new HashSet<string>();


        public string ShortUrl => GetShortUrl(Url);

        public static string GetShortUrl(string akaLink)
        {
            var i = akaLink.IndexOf(akaRoot, StringComparison.InvariantCultureIgnoreCase);
            return akaLink.Substring(i + akaRoot.Length);
        }
    }
}

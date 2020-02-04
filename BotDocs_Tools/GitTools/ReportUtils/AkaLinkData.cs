using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Utilities;

namespace ReportUtils
{
    public class AkaLinkData
    {
        public enum IssueType { BadLink, NotSecure, NotLanguageNeutral, NeedsQueryParamReview }

        public class Issue
        {
            public IssueType Type { get; internal set; }
            public int Severity { get; internal set; }
            public string Description { get; internal set; }
            public Func<AkaLinkData, bool> Rule { get; internal set; }
        }

        private const string DocsSite = "docs.microsoft.com/";

        static AkaLinkData()
        {
            var dict = new Dictionary<IssueType, Issue>();
            dict.Add(IssueType.BadLink, new Issue
            {
                Type = IssueType.BadLink,
                Severity = 3,
                Description = "Not 2xx",
                Rule = data => !(data.Status.StartsWith("2") && data.Status.Length == 3),
            });
            dict.Add(IssueType.NotSecure, new Issue
            {
                Type = IssueType.NotSecure,
                Severity = 2,
                Description = "Not HTTPS",
                Rule = data => !data.TargetUrl?.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ?? false,
            });
            dict.Add(IssueType.NotLanguageNeutral, new Issue
            {
                Type = IssueType.NotLanguageNeutral,
                Severity = 2,
                Description = "Includes locale",
                Rule = data =>
                {
                    var start = data.TargetUrl?.IndexOf(DocsSite, StringComparison.OrdinalIgnoreCase) ?? -1;
                    if (start < 0) { return false; }
                    start += DocsSite.Length;
                    var end = data.TargetUrl.IndexOf('/', start);
                    if (end < 0) { return false; }
                    var fragment = data.TargetUrl.Substring(start, end - start);
                    return fragment.IsCultureId();
                },
            });
            dict.Add(IssueType.NeedsQueryParamReview, new Issue
            {
                Type = IssueType.NeedsQueryParamReview,
                Severity = 1,
                Description = "Query params",
                Rule = data =>
                {
                    if ((data.TargetUrl?.IndexOf(DocsSite, StringComparison.OrdinalIgnoreCase) ?? -1) < 0) { return false; }
                    var parts = data.TargetUrl.Split(new char[] { '?' }, 2);
                    if (parts.Length < 2) { return false; }
                    var parameters = parts[1].Split(new char[] { '&' }).Select(p =>
                    {
                        var index = p.IndexOf('=');
                        return index < 0 ? p : p.Substring(0, index);
                    });
                    return parameters.Any(p =>
                        p.StartsWith("view", StringComparison.OrdinalIgnoreCase) ||
                        p.StartsWith("tabs", StringComparison.OrdinalIgnoreCase));
                },
            });
            AvailableIssues = new ReadOnlyDictionary<IssueType, Issue>(dict);
        }

        public static IReadOnlyDictionary<IssueType, Issue> AvailableIssues { get; private set; }

        private const string akaRoot = "://aka.ms/";

        public string Url { get; set; }
        public HashSet<string> FilesContainingLink { get; } = new HashSet<string>();


        public string ShortUrl => GetShortUrl(Url);

        public string Status { get; set; }

        public string Reason { get; set; }

        public string TargetUrl { get; set; }

        public IList<IssueType> Issues { get; set; }

        public static string GetShortUrl(string akaLink)
        {
            var i = akaLink.IndexOf(akaRoot, StringComparison.InvariantCultureIgnoreCase);
            return akaLink.Substring(i + akaRoot.Length);
        }
    }
}

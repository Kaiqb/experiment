using GitHubQl;
using GitHubQl.Models.GitHub;
using GitHubReports;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubReportCli
{
    class Program
    {
        private static class Strings
        {
            public const string About = @"
This tool uses the GitHub GraphQL API to gather repository information from GitHub. You will need a
personal access token that the tool can use to access GitHub on your behalf. See the GitHub API docs
for more information (https://developer.github.com/v4/guides/forming-calls/#authenticating-with-graphql).
This tool will remember your GitHub user name and token, after you enter them. If you need to change
these later. run the tool with the 'clear' argument.
";
        }

        // To simplify things, the initial report uses constants.
        // TODO Add command line args to allow the user to change things, or set this up with some sort of GUI.
        // TODO Refactor so this isn't a monolithic method.
        public static async Task Main(string[] args)
        {
            Console.WriteLine(Strings.About);

            if (args.Any(p => p.Equals("clear", StringComparison.InvariantCultureIgnoreCase))) SecretsManager.Clear();

            if (!GetSecret(SecretsManager.GitHubUsername, "What is your GitHub user name?", out var userName))
            {
                Console.WriteLine("This is needed...exiting");
                return;
            }
            else
            {
                SecretsManager.Set(SecretsManager.GitHubUsername, userName);
                Console.WriteLine($"Your GitHub username is {SecretsManager.Get(SecretsManager.GitHubUsername)}.");
            }
            Console.WriteLine();

            if (!GetSecret(SecretsManager.GitHubUserToken, "What is your GitHub user token?", out var userToken))
            {
                Console.WriteLine("This is needed...exiting");
                return;
            }
            else
            {
                SecretsManager.Set(SecretsManager.GitHubUserToken, userToken);
                GitHubQlService.SetAuthToken(userToken);
                Console.WriteLine("Your GitHub auth token is set.");
            }
            Console.WriteLine();

            SecretsManager.Save();
            Console.WriteLine("You can clear both of these by running this command with a `clear` argument.");
            Console.WriteLine();

            var nameToGet = "bot-docs";

            var repo = GitHubConstants.KnownRepos.FirstOrDefault(
                r => r.Name.Equals(nameToGet, StringComparison.InvariantCultureIgnoreCase));

            if (repo is null)
            {
                Console.WriteLine($"Sorry, The '{nameToGet}' repo is not defined in the constants file; exiting.");
                return;
            }

            var outputRoot = GitHubConstants.DefaultOutputRoot;
            if (!Directory.Exists(outputRoot)) { Directory.CreateDirectory(outputRoot); }

            var reportDirectory = "Output_" + DateTime.Now.ToString("yyyy_MM_dd");

            var outputPath = Path.Combine(outputRoot, reportDirectory);
            if (Directory.Exists(outputPath))
            {
                Console.Write($"The output directory '{outputPath}' already exists. Clobber existing reports? (y/N)");
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.Y:
                        break;
                    case ConsoleKey.N:
                    default:
                        Console.WriteLine("Ok. Operation cancelled; exiting.");
                        return;
                }
            }
            else
            {
                Directory.CreateDirectory(outputPath);
                Console.WriteLine($"Reports will be written to '{outputPath}'.");
            }
            Console.WriteLine();

            Console.WriteLine("Testing your user token...");
            try
            {
                var data = await GitHubQlService.ExecuteGraphQLRequest(Requests.GetUserInfo(userName));
                //await GitHubGraphQlService.Queries
                //    .GetUser(userName).ConfigureAwait(false);
                Console.WriteLine(data.User.ForConsole());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().Name} encountered: {ex.Message}...exiting.");
            }
            Console.WriteLine();

            // These entries define the columns to include in the CSV file.
            // This relies on the response payload, which is based on what was requested in the GitHub request payload.
            var repoFormatter = new Dictionary<string, Func<Repository, string>>
            {
                { "ID", r => r?.Id.CsvEscape() },
                { "Parent", r => r?.Parent?.NameWithOwner ?? r?.Parent?.Name ?? r?.Parent?.Id },
                { "URL", r => r?.Url },
                { "Is private", r => r?.IsPrivate.ToString() },
                { "Created at", r => r?.CreatedAt.ToShortLocal() },
                { "Pushed at", r => r?.PushedAt.ToShortLocal() },
                { "Updated at", r => r?.UpdatedAt.ToShortLocal() },
                { "Name", r => r?.Name },
                { "Name with owner", r => r?.NameWithOwner },
                { "Owner", r => r?.Owner?.Login ?? r?.Owner?.Id },
                { "Description", r => r?.Description },
                { "Short description HTML", r => r?.ShortDescriptionHtml },
                { "Fork count", r => r?.ForkCount.ToString() },
                { "Assignable users", r => r?.AssignableUsers.GetCount() },
                { "Collaborators", r => r?.Collaborators.GetCount() },
                { "Issues", r => r?.Issues.GetCount() },
                { "Labels", r => r?.Labels.GetCount() },
                { "Pull requests", r => r?.PullRequests.GetCount() },
                { "Stargazers", r => r?.Stargazers.GetCount() },
                { "Watchers", r => r?.Watchers.GetCount() },
            };

            var query = Requests.GetRepository(repo);
            var rData = await GitHubQlService.ExecuteGraphQLRequest(query);
            var repoReport = Path.Combine(outputPath, "Repository.csv");
            Console.WriteLine($"Writing general information about the repository to '{repoReport}'...");
            using (TextWriter writer = new StreamWriter(repoReport, false))
            {
                writer.WriteLine(string.Join(',', repoFormatter.Keys));
                writer.WriteLine(string.Join(',',
                    repoFormatter.Values.Select(func => func(rData.Repository)?.CsvEscape() ?? string.Empty)));
            }
            Console.WriteLine();

            // These entries define the columns to include in the CSV file.
            // This relies on the response payload, which is based on what was requested in the GitHub request payload.
            var issueFormatter = new Dictionary<string, Func<Issue, string>>
            {
                { "Repository", i => i?.Repository?.NameWithOwner ?? i?.Repository?.Name },
                { "Number", i => i?.Number.ToString() },
                { "URL", i => i?.Url },
                //{ "ID", i => i?.Id },
                { "Author", i => i?.Author?.Login },
                { "Author association", i => i?.AuthorAssociation.ToString() },
                //{ "Editor", i => i?.Editor?.Login },
                { "State", i => i?.State.ToString() },
                //{ "Closed", i => i?.Closed.ToString() },
                { "Title", i => i?.Title },
                { "Body text", i => i?.BodyText },
                { "Assignees", i =>
                    i?.Assignees.Concat(", ", u => u?.Login)
                    + (i?.Assignees.PageInfo.HasPreviousPage==true ? ",..." : string.Empty) },
                { "Participants", i => i?.Participants.GetCount() },
                { "Labels", i =>
                    i?.Labels.Concat(", ", l => l.Name)
                    + (i?.Labels.PageInfo.HasPreviousPage == true ? ",..." : string.Empty) },
                { "Comment count", i => i?.Comments?.TotalCount?.ToString() },
                { "Created at", i => i?.CreatedAt.ToShortLocal() },
                //{ "Published at", i => i?.PublishedAt.ToShortLocal() },
                { "Last edited at", i => i?.LastEditedAt.ToShortLocal() },
                //{ "Updated at", i => i?.UpdatedAt.ToShortLocal() },
                { "Closed at", i => i?.ClosedAt.ToShortLocal() },

                { "Last comment author", i => i?.Comments?.Nodes?.FirstOrDefault()?.Author?.Login },
                { "Last comment date", i => i?.Comments?.Nodes?.FirstOrDefault()?.CreatedAt.ToShortLocal() },

                // "Derived" information:
                { "Active since closed?",  i => HasCommentsSinceClosing(i).ToString() },
                { "Time idle (open w/o comment)", i => GetIdleDays(i)?.ToString() }
            };

            await GetDocRepoIssues(repo, outputPath, issueFormatter);

            var filePath = Path.Combine(outputPath, "CodeRepoIssues.csv");
            var repos = GitHubConstants.KnownRepos.Where(r => RepoIsOfType(r, GitHubConstants.RepoTypes.Code));
            var stateFilter = new IssueState[] { IssueState.OPEN };
            var labelFilter = new string[] { "documentation", "Docs", "DCR" };

            await GenerateIssuesReport(repos, issueFormatter, filePath, stateFilter, labelFilter);

            filePath = Path.Combine(outputPath, "SlaIssuesReport.csv");
            repos = GitHubConstants.KnownRepos.Where(r => r.Name.Equals("botframework-solutions", StringComparison.CurrentCultureIgnoreCase));

            await GenerateIssuesReport(repos, issueFormatter, filePath, null, null);

            //filePath = Path.Combine(outputPath, "DocRepoIssues.csv");
            //repos = GitHubConstants.KnownRepos.Where(r => RepoIsOfType(r, GitHubConstants.RepoTypes.Docs | GitHubConstants.RepoTypes.Private));

            //await GenerateIssuesReport(repos, issueFormatter, filePath);

            //var taClient = new TextAnalyticsService("westus2");

            //// The "documents" to analyze. These mat need to be batched. Review
            //// [Text Analytics Overview > Data limits](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/overview#data-limits).
            //var input = issues.Select(i => new MultiLanguageInput("en", i.Number.ToString(), i.BodyText)).ToList();

            //// Once we're done, this will be the aggregated output from analysis.
            //var output = new List<SentimentBatchResultItem>();

            //// TODO track what we've already analyzed, so that:
            //// 1. We don't reanalyze the same issue twice.
            //// 1. We can sanely add to the existing data set.

            //var docData = new CognitiveServicesDocData
            //{
            //    Docs = issues.Select(i => new CognitiveServicesDoc
            //    {
            //        Language = "en",
            //        Id = i.Number.ToString(),
            //        Text = i.Body,
            //    }).ToList(),
            //};
            //// TODO Make the call(s) to the Text Analytics service, aggregate results, generate a report.
            //Console.WriteLine();

            // Other reports that might be useful:
            // - Issues in other repos that have the "Docs" label applied.
            // - PRs in the main code repos that are labeled as "DCR".
            // - Merge activity in the samples repo.
            // - An orphaned or stale branch report for bot-docs-pr.

            // Any reports that crawl the file content may be better suited to the other tool:
            // - Topics that need review, such as their ms.date attribute is getting old, the author
            //   or manager is no longer on the team, or any other metadata maintenance we might need to do.
        }

        private static bool RepoIsOfType(GitHubConstants.RepoParams repo, GitHubConstants.RepoTypes type)
        {
            return (repo.Type & type) == type;
        }

        private static async Task GenerateIssuesReport(
            IEnumerable<GitHubConstants.RepoParams> repos,
            Dictionary<string, Func<Issue, string>> issueFormatter,
            string filePath,
            IssueState[] stateFilter = null,
            string[] labelFilter = null)
        {
            var issues = new List<Issue>();

            foreach (var repo in repos)
            {
                Console.Write($"Gatering issue data for the {repo.Owner}/{repo.Name} repo");
                string queryWithStartCursor(string start) => Requests.GetAllIssues(repo, start, stateFilter, labelFilter);
                await foreach (var sublist in GitHubQlService.GetConnectionAsync(
                    queryWithStartCursor, d => d.Repository.Issues, c => c.Nodes, new CancellationToken()))
                {
                    Console.Write(".");
                    issues.AddRange(sublist);
                }
                Console.WriteLine("done!");
                Console.WriteLine();
            }

            Console.WriteLine($"Writing issue information to '{filePath}'...");
            using (TextWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(string.Join(',', issueFormatter.Keys));
                foreach (var issue in issues)
                {
                    writer.WriteLine(string.Join(',',
                        issueFormatter.Values.Select(func => func(issue)?.CsvEscape() ?? string.Empty)));
                }
            }
            Console.WriteLine();
        }

        private static async Task GetDocRepoIssues(
            GitHubConstants.RepoParams repo,
            string outputPath,
            Dictionary<string, Func<Issue, string>> issueFormatter)
        {
            Console.Write("Gatering issue data for the docs repo a page at a time");
            var issues = new List<Issue>();
            string queryWithStartCursor(string start) => Requests.GetAllIssues(repo, start);
            await foreach (var sublist in GitHubQlService.GetConnectionAsync(
                queryWithStartCursor, d => d.Repository.Issues, c => c.Nodes, new CancellationToken()))
            {
                Console.Write(".");
                issues.AddRange(sublist);
            }
            Console.WriteLine("done!");
            Console.WriteLine();

            var issueReport = Path.Combine(outputPath, "DocRepoIssues.csv");
            Console.WriteLine($"Writing issue information to '{issueReport}'...");
            using (TextWriter writer = new StreamWriter(issueReport, false))
            {
                writer.WriteLine(string.Join(',', issueFormatter.Keys));
                foreach (var issue in issues)
                {
                    writer.WriteLine(string.Join(',',
                        issueFormatter.Values.Select(func => func(issue)?.CsvEscape() ?? string.Empty)));
                }
            }
            Console.WriteLine();
        }

        /// <summary>Wraps a call to the secrets manager, and queries the user for a value, if one
        /// isn't already on record.</summary>
        /// <param name="id">The secret's ID.</param>
        /// <param name="prompt">The string with which to ask the user for a value.</param>
        /// <param name="value">When this returns, contains the secret; or the null or empty string
        /// if the retrieval failed.</param>
        /// <returns>True if the retrieval or prompt succeeded; otherwise, false.</returns>
        private static bool GetSecret(string id, string prompt, out string value)
        {
            if (SecretsManager.TryGet(id, out value)
                && !string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            Console.Write(prompt + " ");
            value = Console.ReadLine();
            value = value.Trim();

            return !string.IsNullOrEmpty(value);
        }

        /// <summary>Returns the number of days since the last comment was made to an issue.</summary>
        /// <param name="issue">The issue to get the value for.</param>
        /// <returns>The number of days since the last comment; or null if the issue contains 
        /// insufficient information.</returns>
        private static double? GetIdleDays(Issue issue)
        {
            if (issue is null
                || issue.Closed == true) return null;

            DateTimeOffset lasstActivityDate;
            if (issue.Comments != null
                && issue.Comments.TotalCount > 0)
            {
                if (issue.Comments.Nodes != null
                    && issue.Comments.Nodes.Count > 0
                    && issue.Comments.Nodes[0].CreatedAt != null
                    && issue.Comments.Nodes[0].CreatedAt.HasValue)
                {
                    lasstActivityDate = issue.Comments.Nodes[0].CreatedAt.Value;
                }
                else
                {
                    return null;
                }
            }
            else if (issue.CreatedAt != null
                && issue.CreatedAt.HasValue)
            {
                lasstActivityDate = issue.CreatedAt.Value;
            }
            else
            {
                return null;
            }

            return Math.Round((DateTimeOffset.Now - lasstActivityDate).TotalDays, 2);
        }

        /// <summary>Indicates whether any comments were made to an issue since it was closed.</summary>
        /// <param name="issue">The issue to check.</param>
        /// <returns>True if there was activity since closing, false if not, or null if there's
        /// insufficient information.</returns>
        private static bool? HasCommentsSinceClosing(Issue issue)
        {
            if (!(issue?.CreatedAt.HasValue == true)) return null;

            return issue?.Closed == true
                && issue.Comments?.TotalCount > 0
                && issue.Comments.Nodes?.FirstOrDefault()?.CreatedAt > issue.ClosedAt;
        }
    }
}

using GitHubQl;
using GitHubQl.Models.GitHub;
using GitHubReports;
using System;
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

        private static readonly DateTimeOffset Now = DateTimeOffset.Now;

        // To simplify things, the initial report uses constants.
        // TODO Add command line args to allow the user to change things, or set this up with some sort of GUI.
        // TODO Refactor so this isn't a monolithic method.
        public static async Task Main(string[] args)
        {
            Console.WriteLine(Strings.About);

            if (args.Any(p => p.Equals("clear", StringComparison.InvariantCultureIgnoreCase))) SecretsManager.Clear();

            // Check for and collect credentials.
            if (!TryGetCredentials(out var userName, out var userToken))
            {
                return;
            }

            // Check for and create the output directory.
            var outputPath = TryCreateOutputDirectory(GitHubConstants.DefaultOutputRoot);
            if (outputPath is null)
            {
                return;
            }

            // Test the credentials.
            var nameToGet = "bot-docs";
            var repo = GitHubConstants.KnownRepos.FirstOrDefault(
                r => r.Name.Equals(nameToGet, StringComparison.InvariantCultureIgnoreCase));

            if (repo is null)
            {
                Console.WriteLine($"Sorry, The '{nameToGet}' repo is not defined in the constants file; exiting.");
                return;
            }

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
                return;
            }
            Console.WriteLine();

            // Generate a simple bot-docs report summarizing the contents of the repo.
            var summaryHelper = new RepoHelper(Now);
            var query = Requests.GetRepository(repo);
            var rData = await GitHubQlService.ExecuteGraphQLRequest(query);
            var repoReport = Path.Combine(outputPath, "Repository.csv");
            Console.Write($"Writing general information about the repository to '{repoReport}'...");
            using (TextWriter writer = new StreamWriter(repoReport, false))
            {
                writer.WriteLine(string.Join(',', summaryHelper.DefaultFormatter.Keys));
                writer.WriteLine(string.Join(',',
                    summaryHelper.DefaultFormatter.Values.Select(func => func(rData.Repository)?.CsvEscape() ?? string.Empty)));
            }
            Console.WriteLine("done.");
            Console.WriteLine();

            // Generate an issues report against the bot-docs repo.
            var issueHelper = new IssueHelper(Now);
            await GetDocRepoIssues(repo, outputPath, issueHelper.DefaultFormatter);

            // Generate report for open doc issues in the code repos.
            var filePath = Path.Combine(outputPath, "CodeRepoIssues.csv");
            var repos = GitHubConstants.KnownRepos.Where(r => RepoIsOfType(r, GitHubConstants.RepoTypes.Code));
            var stateFilter = new IssueState[] { IssueState.OPEN };
            var labelFilter = new string[] { "documentation", "Docs", "DCR" };
            await GenerateIssuesReport(repos, issueHelper.DefaultFormatter, filePath, stateFilter, labelFilter);

            // Generate an issues report against the botframework-solutions repo.
            filePath = Path.Combine(outputPath, "SlaIssuesReport.csv");
            repos = GitHubConstants.KnownRepos.Where(r => r.Name.Equals("botframework-solutions", StringComparison.CurrentCultureIgnoreCase));
            await GenerateIssuesReport(repos, issueHelper.DefaultFormatter, filePath, null, null);

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

        private static bool TryGetCredentials(out string userName, out string userToken)
        {
            userName = userToken = null;
            if (!GetSecret(SecretsManager.GitHubUsername, "What is your GitHub user name?", out userName))
            {
                Console.WriteLine("This is needed...exiting");
                userName = userToken = null;
                return false;
            }
            else
            {
                SecretsManager.Set(SecretsManager.GitHubUsername, userName);
                Console.WriteLine($"Your GitHub username is {SecretsManager.Get(SecretsManager.GitHubUsername)}.");
            }

            if (!GetSecret(SecretsManager.GitHubUserToken, "What is your GitHub user token?", out userToken))
            {
                Console.WriteLine("This is needed...exiting");
                userName = userToken = null;
                return false;
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

            return true;
        }

        private static string TryCreateOutputDirectory(string outputRoot)
        {
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
                        Console.WriteLine("OK. Operation canceled; exiting.");
                        return null;
                }
            }
            else
            {
                Directory.CreateDirectory(outputPath);
                Console.WriteLine($"Reports will be written to '{outputPath}'.");
            }
            Console.WriteLine();

            return outputPath;
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
                Console.Write($"Gathering issue data for the {repo.Owner}/{repo.Name} repo");
                string queryWithStartCursor(string start) => Requests.GetAllIssues(repo, start, stateFilter, labelFilter);
                await foreach (var sublist in GitHubQlService.GetConnectionAsync(
                    queryWithStartCursor, d => d.Repository.Issues, c => c.Nodes, new CancellationToken()))
                {
                    Console.Write(".");
                    issues.AddRange(sublist);
                }
                Console.WriteLine("done!");
            }

            Console.Write($"Writing issue information to '{filePath}'...");
            using (TextWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(string.Join(',', issueFormatter.Keys));
                foreach (var issue in issues)
                {
                    writer.WriteLine(string.Join(',',
                        issueFormatter.Values.Select(func => func(issue)?.CsvEscape() ?? string.Empty)));
                }
            }
            Console.WriteLine("done!");
            Console.WriteLine();
        }

        /// <summary>Generates an issues report against a specific GitHu repo.</summary>
        /// <param name="repo">Description of the repo to generate the repo for.</param>
        /// <param name="outputPath">Path of the directory in which to generate the report.</param>
        /// <param name="issueFormatter">Map of the columns to include in the report. Associates a column name
        /// with a function for extracting the associated data from an issue object.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>The task is successful if it queries GitHub and generates the report.</remarks>
        private static async Task GetDocRepoIssues(
            GitHubConstants.RepoParams repo,
            string outputPath,
            Dictionary<string, Func<Issue, string>> issueFormatter)
        {
            Console.Write("Gathering issue data for the docs repo a page at a time");
            var issues = new List<Issue>();
            string queryWithStartCursor(string start) => Requests.GetAllIssues(repo, start);
            await foreach (var sublist in GitHubQlService.GetConnectionAsync(
                queryWithStartCursor, d => d.Repository.Issues, c => c.Nodes, new CancellationToken()))
            {
                Console.Write(".");
                issues.AddRange(sublist);
            }
            Console.WriteLine("done. ");

            var issueReport = Path.Combine(outputPath, "DocRepoIssues.csv");
            Console.Write($"Writing issue information to '{issueReport}'...");
            using (TextWriter writer = new StreamWriter(issueReport, false))
            {
                writer.WriteLine(string.Join(',', issueFormatter.Keys));
                foreach (var issue in issues)
                {
                    writer.WriteLine(string.Join(',',
                        issueFormatter.Values.Select(func => func(issue)?.CsvEscape() ?? string.Empty)));
                }
            }
            Console.WriteLine("done!");
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
                || issue.CreatedAt is null
                || !issue.CreatedAt.HasValue
                || issue.Closed is true) return null;

            DateTimeOffset lastActivityDate;
            if (issue.Comments != null
                && issue.Comments.TotalCount > 0)
            {
                if (issue.Comments.Nodes != null
                    && issue.Comments.Nodes.Count > 0
                    && issue.Comments.Nodes[0].CreatedAt != null
                    && issue.Comments.Nodes[0].CreatedAt.HasValue)
                {
                    lastActivityDate = issue.Comments.Nodes[0].CreatedAt.Value;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                lastActivityDate = issue.CreatedAt.Value;
            }

            return Math.Round((DateTimeOffset.Now - lastActivityDate).TotalDays, 2);
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

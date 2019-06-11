using GitHubQl;
using GitHubQl.Models.GitHub;
using GitHubReports;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

            // This relies on the format of the query used to retrieve the data from GitHub.
            var repoFormatter = new Dictionary<string, Func<Repository, string>>
            {
                { "ID", r => r?.Id.CsvEscape() },
                { "Parent", r => r?.Parent?.NameWithOwner ?? r?.Parent?.Name ?? r?.Parent?.Id },
                { "URL", r => r?.Url },
                { "Is private", r => r?.IsPrivate.ToString() },
                { "Created at", r => r?.CreatedAt.ToString() },
                { "Pushed at", r => r?.PushedAt.ToString() },
                { "Updated at", r => r?.UpdatedAt.ToString() },
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

            // This relies on the format of the query used to retrieve the data from GitHub.
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
                { "Comment count", i => i?.Comments?.TotalCount?.ToString() ?? string.Empty },
                { "Created at", i => i?.CreatedAt.ToShortLocal() },
                //{ "Published at", i => i?.PublishedAt.ToString() },
                { "Last edited at", i => i?.LastEditedAt.ToString() },
                //{ "Updated at", i => i?.UpdatedAt.ToString() },
                { "Closed at", i => i?.ClosedAt.ToShortLocal() },

                { "Last comment author", i => i?.Comments?.Nodes?.FirstOrDefault()?.Author?.Login },
                { "Last comment date", i => i?.Comments?.Nodes?.FirstOrDefault()?.CreatedAt.ToShortLocal() },

                // "Derived" information:
                { "Active since closed?",  i => HasCommentsSinceClosing(i).ToString() },
                { "Time idle (open w/o comment)", i => GetIdleDays(i).ToString() }
            };


            Console.Write("Gatering issue data for the repo a page at a time");
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

            var issueReport = Path.Combine(outputPath, "Issues.csv");
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


            var taClient = new TextAnalyticsService("westus2");

            // The "documents" to analyze. These mat need to be batched. Review
            // [Text Analytics Overview > Data limits](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/overview#data-limits).
            var input = issues.Select(i => new MultiLanguageInput("en", i.Number.ToString(), i.BodyText)).ToList();

            // Once we're done, this will be the aggregated output from analysis.
            var output = new List<SentimentBatchResultItem>();

            // TODO track what we've already analyzed, so that:
            // 1. We don't reanalyze the same issue twice.
            // 1. We can sanely add to the existing data set.

            var docData = new CognitiveServicesDocData
            {
                Docs = issues.Select(i => new CognitiveServicesDoc
                {
                    Language = "en",
                    Id = i.Number.ToString(),
                    Text = i.Body,
                }).ToList(),
            };
            Console.WriteLine();
        }

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

        private static double? GetIdleDays(Issue issue)
        {
            if (issue is null
                || issue.Closed == true) return null;

            var lastCommentDate = issue.CreatedAt.Value;
            if (issue.Comments != null
                && issue.Comments.TotalCount > 0
                && issue.Comments.Nodes != null
                && issue.Comments.Nodes.Count > 0
                && issue.Comments.Nodes[0].CreatedAt != null
                && issue.Comments.Nodes[0].CreatedAt.HasValue)
            {
                lastCommentDate = issue.Comments.Nodes[0].CreatedAt.Value;
            }

            return Math.Round((DateTimeOffset.Now - lastCommentDate).TotalDays, 2);
        }

        private static bool HasCommentsSinceClosing(Issue issue)
        {
            return issue?.Closed == true
                && issue.Comments?.TotalCount > 0
                && issue.Comments.Nodes?.FirstOrDefault()?.CreatedAt > issue.ClosedAt;
        }
    }
}

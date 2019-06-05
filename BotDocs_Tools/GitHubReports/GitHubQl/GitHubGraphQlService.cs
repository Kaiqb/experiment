using GitHubQl.Models.GitHub;
using GitHubQl.Models.GraphQl;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubQl
{
    public static class GitHubGraphQlService
    {
        readonly static Lazy<IGitHubAPI> _githubApiClientHolder
            = new Lazy<IGitHubAPI>(() => RestService.For<IGitHubAPI>(CreateHttpClient()));

        static IGitHubAPI GitHubApiClient => _githubApiClientHolder.Value;

        static HttpClient CreateHttpClient()
        {
            return new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip })
            {
                BaseAddress = new Uri(GitHubConstants.APIUrl)
            };
        }

        static async Task<T> ExecuteGraphQLRequest<T>(
            Func<Task<GraphQLResponse<T>>> action, int numRetries = 3)
        {
            var response = await Policy
                                .Handle<Exception>()
                                .WaitAndRetryAsync
                                (
                                    numRetries,
                                    pollyRetryAttempt
                                ).ExecuteAsync(action).ConfigureAwait(false);


            if (response.Errors != null)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));

            return response.Data;

            TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }

        public delegate Task<Connection<Issue>> IssuesQuery(
            string owner,
            string repoName,
            DateTimeOffset since,
            string startCursor);

        public struct Queries
        {
            public static async Task<Connection<Issue>> GetIssues(
                string owner,
                string repoName,
                DateTimeOffset since,
                string startCursor)
            {
                var cursor = string.IsNullOrWhiteSpace(startCursor) ? string.Empty : $"before: \"{startCursor}\"";
                var sinceString = since.ToString("yyyy-MM-ddT08:00:00");

                var requestString =
                    "query { " +
                        $"repository(owner:\"{owner}\" name:\"{repoName}\") {{ " +
                            $"issues(last:100 {cursor}, " +
                                $"filterBy: {{ since: \"{sinceString}\" }})" +
                            "{ nodes { title number state body createdAt closedAt author { login } " +
                                "labels(last: 5) { totalCount nodes { name } pageInfo { hasPreviousPage } } }, " +
                            "pageInfo { hasPreviousPage, startCursor }}" +
                        "}}";

                var data = await ExecuteGraphQLRequest(
                    () => GitHubApiClient.Query(new GraphQLRequest(requestString))).ConfigureAwait(false);

                return data.Repository.Issues;
            }

            public static async Task<Connection<Issue>> GetAllIssues(
                string owner,
                string repoName,
                DateTimeOffset since,
                string startCursor)
            {
                var start = string.IsNullOrWhiteSpace(startCursor) ? string.Empty : $"before: \"{startCursor}\"";

                var requestString =
                    "query { " +
                        $"repository(owner:\"{owner}\" name:\"{repoName}\") {{ " +
                            $"issues(last:100 {start})" +
                            "{ nodes { title number state body createdAt closedAt author { login } " +
                                "labels(last: 5) { nodes { name } pageInfo { hasPreviousPage } } }, " +
                            "pageInfo { hasPreviousPage, startCursor }}" +
                        "}}";

                var data = await ExecuteGraphQLRequest(
                    () => GitHubApiClient.Query(new GraphQLRequest(requestString))).ConfigureAwait(false);

                return data.Repository.Issues;
            }

            public static async Task<User> GetUser(string username)
            {
                var requestString = "query { " +
                    $"user(login:{ username})" +
                    "{ name,company,bio,createdAt,email,location,login,name,url" +
                    "  organizations(first:10){totalCount,nodes{name}},followers{totalCount}}}";

                var data = await ExecuteGraphQLRequest(
                    () => GitHubApiClient.Query(new GraphQLRequest(requestString))).ConfigureAwait(false);

                return data.User;
            }

            public static async Task<Repository> GetRepository(string owner, string reponame)
            {
                var requestString = "query { " +
                    $"repository(owner:\"{owner}\" name:\"{reponame}\")" +
                    "{ name,nameWithOwner,description,forkCount,owner{login},stargazers{totalCount}," +
                    "  issues{totalCount},parent{name},pullRequests{totalCount},url}}";

                var data = await ExecuteGraphQLRequest(() => GitHubApiClient.Query(new GraphQLRequest(requestString))).ConfigureAwait(false);

                return data.Repository;
            }
        }

        public static async IAsyncEnumerable<List<Issue>> GetRepositoryIssues(
            IssuesQuery query,
            string owner,
            string repoName,
            DateTimeOffset since = default,
            CancellationToken cancellationToken = default)
        {
            Connection<Issue> issues = null;

            do
            {
                issues = await query(owner, repoName, since, issues?.PageInfo?.StartCursor).ConfigureAwait(false);

                yield return issues.Nodes;
            }
            while (!cancellationToken.IsCancellationRequested && (issues?.PageInfo?.HasPreviousPage is true));
        }
    }
}

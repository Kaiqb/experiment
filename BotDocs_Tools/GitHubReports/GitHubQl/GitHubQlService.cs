using GitHubQl.Models.GitHub;
using GitHubQl.Models.GraphQl;
using Polly;
using Refit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubQl
{
    public static class GitHubQlService
    {
        /// <summary>Creates an HTTP client.</summary>
        /// <returns>The HTTP client.</returns>
        private static HttpClient CreateHttpClient()
        {
            return new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip })
            {
                BaseAddress = new Uri(GitHubConstants.APIUrl)
            };
        }

        /// <summary>Gets a lazy-initialized Refit-compatible description of the GitHub GraphQL service.</summary>
        private static IGitHubAPI GitHubApiClient =>
            new Lazy<IGitHubAPI>(() => RestService.For<IGitHubAPI>(CreateHttpClient())).Value;

        /// <summary>Makes the GitHub GraphQL request, using the Polly library.</summary>
        /// <param name="query">The query payload to use in the request</param>
        /// <param name="numRetries">The number of times to retry before failing.</param>
        /// <returns>The `data` object returned by the query.</returns>
        public static async Task<GitHubQlData> ExecuteGraphQLRequest(string query, int numRetries = 3)
        {
            TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
            var response = await Policy
                                .Handle<Exception>()
                                .WaitAndRetryAsync(
                                    numRetries,
                                    pollyRetryAttempt)
                                .ExecuteAsync(() => GitHubApiClient.Query(new GraphQLRequest(query)))
                                .ConfigureAwait(false);


            if (response.Errors != null)
                throw new AggregateException(response.Errors.Select(x => new Exception(x.Message)));

            return response.Data;
        }

        public static async IAsyncEnumerable<List<T>> GetConnectionAsync<T>(
            Func<string, string> queryWithStartCursor,
            Func<GitHubQlData, Connection<T>> getConnection,
            Func<Connection<T>, List<T>> connectionToList,
            CancellationToken cancellationToken = default)
        {
            Connection<T> connection = null;
            do
            {
                var startCursor = connection?.PageInfo?.StartCursor;
                var query = queryWithStartCursor(startCursor);

                connection = getConnection(await ExecuteGraphQLRequest(query).ConfigureAwait(false));

                yield return connectionToList(connection);
            }
            while (!cancellationToken.IsCancellationRequested && connection?.PageInfo?.HasPreviousPage is true);
        }

        /// <summary>Runs an issues query against a repo, returning one page of issues at a time.</summary>
        /// <param name="queryWithStartCursor">A function that takes a start cursor (or null) and returns a complete query.</param>
        /// <param name="getNodes">A function paired to the query that can get a list of nodes from the query results.</param>
        /// <param name="cancellationToken">A cancellation token for the async operation.</param>
        /// <returns>An async enumeration of the pages of issues.</returns>
        //public static async IAsyncEnumerable<List<Issue>> GetIssues(
        //    Func<string, string> queryWithStartCursor,
        //    Func<Connection<Issue>, List<Issue>> getNodes,
        //    CancellationToken cancellationToken = default)
        //{
        //    await foreach (var issues in GetConnectionAsync(
        //        queryWithStartCursor,
        //        rr => rr.Repository.Issues,
        //        getNodes,
        //        cancellationToken))
        //    {
        //        yield return issues;
        //    }
        //}
    }
}

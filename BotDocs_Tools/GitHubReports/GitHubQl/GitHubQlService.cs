﻿using GitHubQl.Models.GitHub;
using GitHubQl.Models.GraphQl;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubQl
{
    public static class GitHubQlService
    {
        /// <summary>Sets the user access token to use with the service.</summary>
        /// <param name="bearerToken">The user's bearer token.</param>
        /// <remarks>This needs to be set for the calls into the GitHub GraphQL to work.</remarks>
        public static void SetAuthToken(string bearerToken)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(bearerToken));
            _authorization = $"bearer {bearerToken.Trim()}";
        }
        private static string _authorization = null;

        /// <summary>Creates an HTTP client.</summary>
        /// <returns>The HTTP client.</returns>
        private static HttpClient CreateHttpClient()
        {
            Contract.Requires(!string.IsNullOrEmpty(_authorization));
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
            static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
            var response = await Policy
                                .Handle<Exception>()
                                .WaitAndRetryAsync(
                                    numRetries,
                                    pollyRetryAttempt)
                                .ExecuteAsync(() => GitHubApiClient.Query(new GraphQLRequest(query), _authorization))
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
    }
}

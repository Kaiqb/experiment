using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace GitHubTools
{
    public class GitHubClient : IDisposable
    {
        /// <summary>The GraphQL API v4 endpoint.</summary>
        private const string EndPoint = "https://api.github.com/graphql";

        private HttpClient InnerClient { get; set; }

        private const int PageSize = 50;

        /// <summary>The bearer token to use when making the query.</summary>
        /// <remarks>For more info, see "Creating a personal access token for the command line"
        /// (https://help.github.com/articles/creating-an-access-token-for-command-line-use/).
        /// In theory, you just need a token that has "Full control of private repositories".
        /// </remarks>
        private string UserToken { get; }

        public GitHubClient(string userToken)
        {
            UserToken = userToken;
            InnerClient = new HttpClient { BaseAddress = new Uri(EndPoint) };
            InnerClient.DefaultRequestHeaders.Add("Bearer", userToken);
        }

        private async Task<(HttpStatusCode, string)> Run(string query)
        {
            string result = null;
            var response = await InnerClient.PostAsync(query);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return (response.StatusCode, result);
        }

        public async Task<(bool, IList<Issue>)> GetIssues(string owner, string repoName,
            DateTimeOffset sinceDate = default(DateTimeOffset))
        {
            var query = GenerateIssuesQuery(owner, repoName, sinceDate);
            string cursor = null;
            var more = true;
            var success = true;
            IList<Issue> issues = new List<Issue>();
            do
            {
                var (status, result) = await Run(query);
                // parse results for issues, cursor, and whether there's more.
            } while (more);

            return (success, issues);
        }

        private static string GenerateIssuesQuery(
            string owner,
            string repoName,
            DateTimeOffset sinceDate = default(DateTimeOffset),
            string cursor = null)
        {
            var paging = $"last:{PageSize}" + (cursor != null ? $",before:\"{cursor}\"" : string.Empty);
            var filter = $"filterBy:{{since:\"{sinceDate.ToUniversalTime()}\"}}";

            var sb = new StringBuilder();
            sb.Append($"{{repository(owner: \"{owner}\", name: \"{repoName}\") {{");

            sb.Append($"issues(last:{paging},{filter}){{" +
                "edges{" +
                "  node{" +
                "    title number state createdAt body author {login}" +
                "    labels(last:5){nodes{name}pageInfo{hasPreviousPage}}");
            sb.Append("}}");
            sb.Append("pageInfo{endCursor hasPreviousPage}");

            sb.Append("}}}");

            return sb.ToString();
        }

        #region IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    InnerClient.Dispose();
                }
                finally
                {
                    InnerClient = null;
                }
            }

            disposed = true;
        }

        #endregion
    }
}

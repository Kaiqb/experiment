using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

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

        public RichTextBox Output { get; set; }

        public GitHubClient(string userToken)
        {
            UserToken = userToken;
            InnerClient = new HttpClient
            {
                BaseAddress = new Uri(EndPoint),
            };
            InnerClient.DefaultRequestHeaders.Add("Bearer", userToken);
        }

        //private async Task<(HttpStatusCode, string, bool)> Run(string query)
        //{
        //    return await InnerClient.PostAsync(query);
        //}

        public (bool, IList<Issue>) GetIssues(
            string owner,
            string repoName,
            DateTimeOffset sinceDate = default(DateTimeOffset))
        {
            var uri = InnerClient.BaseAddress;
            var query = GenerateIssuesQuery(owner, repoName, sinceDate);
            var success = true;
            IList<Issue> issues = new List<Issue>();
            while (true)
            {
                //var (status, result, succeeded) = await InnerClient.PostRequestAsync(query);
                //var (status, payload, succeeded, ex) = uri.Post(UserToken, query);
                //if (!succeeded)
                //{
                //    var msg = "Request to get a page of issues failed"
                //        + ((ex != null)
                //            ? ":" + Environment.NewLine + ex.ToString()
                //            : $" with status {status}.");
                //    Output?.WriteLine(Severity.Warning, msg);
                //    success = false;
                //    break;
                //}

                // parse results for issues, cursor, and whether there's more.
                //var (page, cursor, more) = ParseIssuesResult(payload);
                //issues.AddRange(page);

                //if (!more) { break; }

                //query = GenerateIssuesQuery(owner, repoName, sinceDate, cursor);
            }

            return (success, issues);
        }

        private (IList<Issue> issues, string cursor, bool more) ParseIssuesResult(string result)
        {
            var repoData = JsonConvert.DeserializeObject<GitHubData<RepoData>>(result);
            var issuesConnection = repoData.Data.Repository.Issues;
            return (
                issues: issuesConnection.Edges.Select(e => e.Node).ToList(),
                cursor: issuesConnection.PageInfo.EndCursor,
                more: issuesConnection.PageInfo.HasPreviousPage.Value);
        }

        /// <summary>Creates the query payload for retrieving issues from a repo in GitHub.</summary>
        /// <param name="owner">The repo owner.</param>
        /// <param name="repoName">The repo name.</param>
        /// <param name="sinceDate">The earliest issue to include in the result.</param>
        /// <param name="cursor">The earliest issue yet gathered, for paging.</param>
        /// <returns>The JSON payload data to use for this request.</returns>
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

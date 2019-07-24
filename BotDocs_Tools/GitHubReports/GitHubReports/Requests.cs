using GitHubQl.Models.GitHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GitHubQl.GitHubConstants;

namespace GitHubReports
{
    /// <summary>Contains "caned" requests, and could evenually contain request builders.</summary>
    /// <remarks>Use currying to reduce these down to a method that just takes a direction and a cursor;
    /// then use that to generate the call the GitHub graphQL service. That will take the remaining
    /// buisness logic out of that library.</remarks>
    public static class Requests
    {
        /// <summary>Generates a payload for a front-to-back issues query for a specific repository.</summary>
        /// <param name="repo">Describes the repository to include in the query.</param>
        /// <param name="sinceDate">Cut-off date. The response will ignore issues created before this date.</param>
        /// <param name="cursor">The start cursor to use to get the previous page.</param>
        /// <returns>An appropriate query payload for the GitHubQl service.</returns>
        public static string GetIssuesSince(RepoParams repo, DateTimeOffset sinceDate, string cursor)
        {
            var since = sinceDate.ToString("yyyy-MM-ddT08:00:00");

            var sb = new StringBuilder();

            sb.Append("{ ");
            sb.Append($"repository(owner:\"{repo.Owner}\" name:\"{repo.Name}\")" + " { ");
            sb.Append($"issues(last:{QueryPageSize} {cursor} filterBy: {{ since: \"{since}\" }})" + " { ");

            sb.Append("nodes { " + IssueData + " } ");
            sb.Append("pageInfo { hasPreviousPage, startCursor } ");

            sb.Append("} } }");

            return sb.ToString();
        }

        /// <summary>Generates a payload for a front-to-back issues query for a specific repository.</summary>
        /// <param name="repo">Describes the repository to include in the query.</param>
        /// <param name="cursor">The start cursor to use to get the previous page.</param>
        /// <param name="states">Optional, indicates which states to include, such as <see cref="IssueState.OPEN"/>.</param>
        /// <param name="labels">Optional, indicates which labels, by name, to include.</param>
        /// <returns>An appropriate query payload for the GitHubQl service.</returns>
        public static string GetAllIssues(RepoParams repo, string cursor, IssueState[] states = null, string[] labels = null)
        {
            var issueArgs = CreateIssueArguments(cursor, states, labels);

            var sb = new StringBuilder();

            sb.Append("{ ");
            sb.Append($"repository(owner:\"{repo.Owner}\" name:\"{repo.Name}\")" + " { ");
            sb.Append($"issues({issueArgs})" + " { ");

            sb.Append("nodes { " + IssueData + " } ");
            sb.Append("pageInfo { hasPreviousPage, startCursor } ");

            sb.Append("} } }");

            return sb.ToString();
        }

        public static string GetSlaIssues(
            RepoParams repo,
            string cursor,
            IEnumerable<IssueState> states = null,
            IEnumerable<string> labels = null)
        {
            var issueArgs = CreateIssueArguments(cursor, states, labels);

            var sb = new StringBuilder();

            sb.Append("{ ");
            sb.Append($"repository(owner:\"{repo.Owner}\" name:\"{repo.Name}\")" + " { ");
            sb.Append($"issues({issueArgs})" + " { ");

            sb.Append("nodes { " + IssueData + " } ");
            sb.Append("pageInfo { hasPreviousPage, startCursor } ");

            sb.Append("} } }");

            return sb.ToString();
        }

        private static string CreateIssueArguments(
            string cursor,
            IEnumerable<IssueState> states,
            IEnumerable<string> labels)
        {
            var issueArgs = $"last: {QueryPageSize}";

            if (!string.IsNullOrWhiteSpace(cursor))
            {
                issueArgs += $" before: \"{cursor}\"";
            }

            if (states != null && states.Count() > 0)
            {
                var list = states.Select(s => Enum.GetName(typeof(IssueState), s));
                issueArgs += $" states: [{string.Join(' ', list)}]";
            }

            if (labels != null && labels.Count() > 0)
            {
                var list = labels.Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => $"\"{l}\"");
                if (list.Count() > 0)
                {
                    issueArgs += $" labels: [{string.Join(' ', list)}]";
                }
            }

            return issueArgs;
        }

        public const string IssueData =
            "repository { nameWithOwner } number url id " +
            "author { login } authorAssociation editor { login } " +
            "state " +
            "labels(last: 10) { totalCount nodes { name } pageInfo { hasPreviousPage } } " +
            "title bodyText " +
            "assignees(last: 10) { totalCount nodes { login } pageInfo { hasPreviousPage } } " +
            "participants(last: 25) { totalCount nodes { login } pageInfo { hasPreviousPage } } " +
            "comments(last: 20) { totalCount nodes { author { login } createdAt } } " +
            "createdAt publishedAt lastEditedAt updatedAt closedAt";

        /// <summary>Generates a payload for general information about a specific repository.</summary>
        /// <param name="repo">Describes the repository to include in the query.</param>
        /// <returns>An appropriate query payload for the GitHubQl service.</returns>
        public static string GetRepository(RepoParams repo)
        {
            var sb = new StringBuilder();

            sb.Append("{ ");
            sb.Append($"repository(owner:\"{repo.Owner}\" name:\"{repo.Name}\")" + " { ");

            sb.Append(RepositoryData);

            sb.Append(" } }");

            return sb.ToString();
        }

        public const string RepositoryData =
            "id parent { nameWithOwner } url isPrivate " +
            "createdAt pushedAt updatedAt " +
            "name nameWithOwner owner { login } description shortDescriptionHTML " +
            "forkCount " +
            "assignableUsers(last:30) { totalCount } " +
            "collaborators(last:30) { totalCount } " +
            "issues { totalCount} " +
            "labels { totalCount } " +
            "pullRequests { totalCount } " +
            "stargazers(last:100) { totalCount } " +
            "watchers(last:100) { totalCount }";

        /// <summary>Generates a payload for general information about a specific GitHub user.</summary>
        /// <param name="username">The user's login name.</param>
        /// <returns>An appropriate query payload for the GitHubQl service.</returns>
        public static string GetUserInfo(string username)
        {
            var sb = new StringBuilder();

            sb.Append("{ ");
            sb.Append($"user(login:{username})" + " { ");

            sb.Append(UserData);

            sb.Append(" } }");

            return sb.ToString();
        }

        public const string UserData =
            "name company bio createdAt email location login name url " +
            "organizations(first:10) { totalCount nodes { name login } } " +
            "followers { totalCount }";
    }
}

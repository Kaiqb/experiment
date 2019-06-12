using System;
using System.Collections.Generic;
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
        /// <returns>An appropriate query payload for the GitHubQl service.</returns>
        public static string GetAllIssues(RepoParams repo, string cursor)
        {
            var start = string.IsNullOrWhiteSpace(cursor) ? string.Empty : $"before: \"{cursor}\"";

            var sb = new StringBuilder();

            sb.Append("{ ");
            sb.Append($"repository(owner:\"{repo.Owner}\" name:\"{repo.Name}\")" + " { ");
            sb.Append($"issues(last:{QueryPageSize} {start})" + " { ");

            sb.Append("nodes { " + IssueData + " } ");
            sb.Append("pageInfo { hasPreviousPage, startCursor } ");

            sb.Append("} } }");

            return sb.ToString();
        }

        public const string IssueData =
            "repository { nameWithOwner } number url id " +
            "author { login } authorAssociation editor { login } " +
            "state closed " +
            "title bodyText " +
            "assignees(last: 5) { totalCount nodes { login } pageInfo { hasPreviousPage } } " +
            "participants(last: 20) { totalCount nodes { login } pageInfo { hasPreviousPage } } " +
            "labels(last: 10) { totalCount nodes { name } pageInfo { hasPreviousPage } } " +
            "comments(last: 1) { totalCount nodes { author { login } createdAt } } " +
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

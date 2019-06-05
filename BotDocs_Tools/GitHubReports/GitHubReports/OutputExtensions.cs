using GitHubQl.Models.GitHub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections;
using GitHubQl;

namespace GitHubReports
{
    public static class OutputExtensions
    {
        public static string ForConsole(this User user)
        {
            var sb = new StringBuilder();

            sb.AppendLine("GitHub User:");
            if (user.AvatarUrl != null) { sb.AppendLine($"  Avatar URL: {user.AvatarUrl}"); }
            if (user.Bio != null) { sb.AppendLine($"  Bio: {user.Bio}"); }
            if (user.Company != null) { sb.AppendLine($"  Company: {user.Company}"); }
            if (user.CreatedAt != null) { sb.AppendLine($"  Created at: {user.CreatedAt}"); }
            if (user.Email != null) { sb.AppendLine($"  Email: {user.Email}"); }
            if (user.Followers != null) { sb.AppendLine($"  Followers (count): {user.Followers.TotalCount}"); }
            if (user.Id != null) { sb.AppendLine($"  ID: {user.Id}"); }
            if (user.Location != null) { sb.AppendLine($"  Location: {user.Location}"); }
            if (user.Login != null) { sb.AppendLine($"  Login: {user.Login}"); }
            if (user.Name != null) { sb.AppendLine($"  Name: {user.Name}"); }
            if (user.Organizations != null)
            {
                sb.Append($"  Organizations({user.Organizations.TotalCount}): ");
                sb.AppendLine((user.Organizations.Nodes != null && user.Organizations.Nodes.Count > 0)
                    ? $"{string.Join(", ", user.Organizations?.Nodes?.Select(o => (o?.Name) ?? "?"))}"
                    : string.Empty);
            }
            if (user.Url != null) { sb.AppendLine($"  URL: {user.Url}"); }

            return sb.ToString();
        }

        public static string ForConsole(this Repository repo)
        {
            var sb = new StringBuilder();

            sb.AppendLine("GitHub Repository:");
            if (repo.AssignableUsers != null) { sb.AppendLine($"  Assignable users (count): {repo.AssignableUsers.TotalCount}"); }
            if (repo.Collaborators != null) { sb.AppendLine($"  Collaborators (count): {repo.Collaborators.TotalCount}"); }
            if (repo.CreatedAt != null) { sb.AppendLine($"  Created at: {repo.CreatedAt}"); }
            if (repo.Description != null) { sb.AppendLine($"  Description: {repo.Description}"); }
            if (repo.ForkCount != null) { sb.AppendLine($"  Fork count: {repo.ForkCount}"); }
            if (repo.Id != null) { sb.AppendLine($"  ID: {repo.Id}"); }
            if (repo.IsPrivate != null) { sb.AppendLine($"  Private: {repo.IsPrivate}"); }
            if (repo.Issues != null) { sb.AppendLine($"  Issues (count): {repo.Issues.TotalCount}"); }
            if (repo.Labels != null) { sb.AppendLine($"  Labels (count): {repo.Labels.TotalCount}"); }
            if (repo.Name != null) { sb.AppendLine($"  Name: {repo.Name}"); }
            if (repo.NameWithOwner != null) { sb.AppendLine($"  Name (with owner): {repo.NameWithOwner}"); }
            if (repo.Owner != null) { sb.AppendLine($"  Owner: {repo.Owner?.Login}"); }
            if (repo.Parent != null) { sb.AppendLine($"  Parent (name): {repo.Parent?.Name}"); }
            if (repo.PullRequests != null) { sb.AppendLine($"  Pull requests (count): {repo.PullRequests.TotalCount}"); }
            if (repo.Url != null) { sb.AppendLine($"  URL: {repo.Url}"); }

            return sb.ToString();
        }

        public static string ForConsole(this Issue issue, string indent = null)
        {
            var pad = indent ?? string.Empty;
            var sb = new StringBuilder();

            sb.AppendLine($"{pad}Issue:");
            pad += "  ";

            if (issue.Assignees != null)
            {
                sb.AppendLine($"{pad}Assignees({issue.Assignees.TotalCount}): " +
                    $"{string.Join(", ", issue.Assignees.Nodes?.Select(a => a?.Login))}");
            }
            if (issue.Author != null) { sb.AppendLine($"{pad}Author: {issue.Author.Login}"); }
            if (issue.AuthorAssociation != null) { sb.AppendLine($"{pad}Author association: {issue.AuthorAssociation}"); }
            if (issue.Body != null) { sb.AppendLine($"{pad}Body: {issue.Body}"); }
            if (issue.ClosedAt != null) { sb.AppendLine($"{pad}Closed at: {issue.ClosedAt}"); }
            if (issue.Comments != null) { sb.AppendLine($"{pad}Comments: {issue.Comments}"); }
            if (issue.CreatedAt != null) { sb.AppendLine($"{pad}Created at: {issue.CreatedAt}"); }
            if (issue.Editor != null) { sb.AppendLine($"{pad}Editor: {issue.Editor}"); }
            if (issue.Id != null) { sb.AppendLine($"{pad}ID: {issue.Id}"); }
            if (issue.Labels != null)
            {
                sb.Append($"  Labels({issue.Labels.TotalCount}): ");
                sb.AppendLine((issue.Labels.Nodes != null && issue.Labels.Nodes.Count > 0)
                    ? $"{string.Join(", ", issue.Labels?.Nodes?.Select(l => (l?.Name) ?? "?"))}"
                    : string.Empty);
            }

            return sb.ToString();
        }

        /// <summary>CSV-escapes a string.</summary>
        /// <param name="value">The string to escape.</param>
        /// <returns>The CSV-escaped string.</returns>
        public static string CsvEscape(this string value)
        {
            if (value is null) return string.Empty;

            var escaped = value
                .Replace("\r", @"\r")
                .Replace("\n", @"\n")
                .Replace("\t", @"\t")
                .Replace("\"", "\"\"");

            return '"' + escaped + '"';
        }
    }
}

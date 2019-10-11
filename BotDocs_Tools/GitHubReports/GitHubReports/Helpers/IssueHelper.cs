using GitHubQl.Models.GitHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitHubReports
{
    /// <summary>
    /// Provides [some attempt at] standardized formatting for various types of GitHub
    /// GraphQL data items.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Truncates a string to a given length.
        /// </summary>
        /// <param name="input">This, the string to truncate.</param>
        /// <param name="maxLength">The maximum length of the output.</param>
        /// <returns>The truncated string.</returns>
        public static string Truncate(this string input, int maxLength = 256)
        {
            return (input != null && input.Length > maxLength)
                ? input.Substring(0, maxLength).Trim() + "…"
                : input;
        }

        /// <summary>
        /// Formats a connection as a list.
        /// </summary>
        /// <typeparam name="T">The type of items connected to.</typeparam>
        /// <param name="connection">This, the connection to format.</param>
        /// <param name="selector">A function to convert the item type to a string.</param>
        /// <returns>The list, represented as a string.</returns>
        public static string AsList<T>(this Connection<T> connection, Func<T, string> selector)
        {
            return connection?.Concat(", ", selector)
                + (connection?.PageInfo?.HasPreviousPage == true ? ",..." : string.Empty);
        }

        public static string AsListWithCount<T>(
            this Connection<T> connection,
            Func<T, string> selector,
            int len = 2)
        {
            if (connection is null
                || !(connection.Nodes?.Count > 0)) return string.Empty;

            string list = string.Join(", ", connection.Nodes.Take(len).Select(selector));

            if (connection.TotalCount.HasValue
                && connection.TotalCount > len)
            {
                return $"{list}, and {connection.TotalCount - len} more";
            }
            else if (connection.Nodes?.Count > len)
            {
                return $"{list}, and {connection.Nodes.Count - len} more";
            }
            else
            {
                return list;
            }
        }

        /// <summary>
        /// Gets a handle for a repository, with fall-back logic.
        /// </summary>
        /// <param name="repo">This, the repository to get a handle for.</param>
        /// <returns>The repository handle</returns>
        public static string Handle(this Repository repo)
        {
            return (repo is null) ? null
                : repo.NameWithOwner ?? repo.Name ?? repo.Id;
        }

        /// <summary>
        /// Gets a handle for an actor, with fall-back logic.
        /// </summary>
        /// <param name="actor">This, the actor to get a handle for.</param>
        /// <returns>The actor handle</returns>
        public static string Handle(this Actor actor)
        {
            return (actor is null) ? null
                : actor.Login ?? actor.Url ?? actor.ResourcePath;
        }

        /// <summary>
        /// Gets a handle for a repository owner, with fall-back logic.
        /// </summary>
        /// <param name="owner">This, the owner to get a handle for.</param>
        /// <returns>The owner handle</returns>
        public static string Handle(this RepositoryOwner owner)
        {
            return (owner is null) ? null
                : owner.Login ?? owner.Url ?? owner.ResourcePath ?? owner.Id;
        }

        /// <summary>
        /// Gets a handle for a user, with fall-back logic.
        /// </summary>
        /// <param name="user">This, the user to get a handle for.</param>
        /// <returns>The user handle</returns>
        public static string Handle(this User user)
        {
            return (user is null) ? null
                : user.Login ?? user.Url ?? user.Name ?? user.ResourcePath ?? user.Id;
        }

        /// <summary>
        /// Gets how old an issue is, in days.
        /// </summary>
        /// <param name="issue">This, the issue to evaluate</param>
        /// <param name="now">The reference time at which the report is being generated.</param>
        /// <returns>The age of the issue, in days.</returns>
        /// <remarks>If the issue is closed, the age is how long it was open for; otherwise,
        /// the age is from opening till now. If the issue was closed and reopened, that
        /// period is included in the age.</remarks>
        public static double? AgeInDays(this Issue issue, DateTimeOffset now)
        {
            if (issue is null || !issue.CreatedAt.HasValue) return null;

            var span = (issue.ClosedAt.HasValue)
                ? issue.ClosedAt.Value - issue.CreatedAt.Value
                : now - issue.CreatedAt.Value;

            return Math.Round(span.TotalDays, 2);
        }

        /// <summary>
        /// Gets how old an issue was before getting its first comment, in days.
        /// </summary>
        /// <param name="issue">This, the issue to evaluate</param>
        /// <param name="now">The reference time at which the report is being generated.</param>
        /// <returns>How old an issue was before getting its first comment, in days.</returns>
        /// <remarks>If the issue is closed without a comment, or the issue is open without comment,
        /// the issue's age as defined by <see cref="AgeInDays(Issue, DateTimeOffset)"/> is used.
        /// Otherwise, take the difference of the first comment date from the created date.</remarks>
        public static double? OpenWithoutCommentInDays(this Issue issue, DateTimeOffset now)
        {
            if (issue is null || !issue.CreatedAt.HasValue) return null;

            var comment = issue?.Comments?.Nodes.FirstOrDefault();
            if (comment != null)
            {
                var span = comment.CreatedAt.Value - issue.CreatedAt.Value;
                return Math.Round(span.TotalDays, 2);
            }
            else
            {
                return issue.AgeInDays(now);
            }
        }
    }

    /// <summary>
    /// Contains formatting information with which to generate a report.
    /// </summary>
    /// <typeparam name="T">The type of item the report is generated against.</typeparam>
    public abstract class GraphQlHelper<T>
    {
        /// <summary>
        /// Map of the columns to include in a report. Associates a column name
        /// with a function for extracting the associated data from an item.
        /// </summary>
        public Dictionary<string, Func<T, string>> Formatter { get; } =
            new Dictionary<string, Func<T, string>>();

        public Dictionary<string, Func<T, string>> DefaultFormatter { get; protected set; }
    }

    public class RepoHelper : GraphQlHelper<Repository>
    {
        private class Getter
        {
            public readonly DateTimeOffset _now;

            public Getter(DateTimeOffset? now)
            {
                _now = now ?? DateTimeOffset.Now;
            }

            public string AssignableUserCount(Repository repo) => repo?.AssignableUsers.GetCount();
            public string CollaboratorCount(Repository repo) => repo?.Collaborators.GetCount();
            public string CreatedAt(Repository repo) => repo?.CreatedAt.ToShortLocal();
            public string Descriiption(Repository repo) => repo?.Description.Truncate();
            public string ForkCount(Repository repo) => repo?.ForkCount.ToString();
            public string Id(Repository repo) => repo?.Id;
            public string IsPrivate(Repository repo) => repo?.IsPrivate.ToString();
            public string IssueCount(Repository repo) => repo?.Issues.GetCount();
            public string LabelCount(Repository repo) => repo?.Labels.GetCount();
            public string Name(Repository repo) => repo?.Name;
            public string NameWithOwner(Repository repo) => repo?.NameWithOwner;
            public string Owner(Repository repo) => repo?.Owner.Handle();
            public string ParentRepo(Repository repo) => repo?.Parent.Handle();
            public string PullRequestCount(Repository repo) => repo?.PullRequests.GetCount();
            public string PushedAt(Repository repo) => repo?.PushedAt.ToShortLocal();
            public string StargazerCount(Repository repo) => repo?.Stargazers.GetCount();
            public string UpdatedAt(Repository repo) => repo?.UpdatedAt.ToShortLocal();
            public string Url(Repository repo) => repo?.Url;
            public string WatcherCount(Repository repo) => repo?.Watchers.GetCount();
        }

        private Getter Accessors { get; }

        public RepoHelper(DateTimeOffset? now)
        {
            Accessors = new Getter(now);
            DefaultFormatter = new Dictionary<string, Func<Repository, string>> {
                { "Repo ID", Accessors.Id },
                { "Repo URL", Accessors.Url },
                { "Name", Accessors.Name },
                { "Owner", Accessors.Owner },
                { "Name with owner", Accessors.NameWithOwner },
                { "Is private", Accessors.IsPrivate },
                { "Parent Repo", Accessors.ParentRepo },
                { "Description", Accessors.Descriiption },
                { "Created at", Accessors.CreatedAt },
                { "Pushed at", Accessors.PushedAt },
                { "Updated at", Accessors.UpdatedAt },
                { "Fork count", Accessors.ForkCount },
                { "Assignable user count", Accessors.AssignableUserCount },
                { "Collaborator count", Accessors.CollaboratorCount },
                { "Issue count", Accessors.IssueCount },
                { "Label count", Accessors.LabelCount },
                { "Pull request count", Accessors.PullRequestCount },
                { "Stargazer count", Accessors.StargazerCount },
                { "Watcher count", Accessors.WatcherCount },
            };
        }
    }

    public class IssueHelper : GraphQlHelper<Issue>
    {
        /// <summary>
        /// Contains methods for getting the value of a query property,
        /// represented as a string.
        /// </summary>
        private class Getter
        {
            public readonly DateTimeOffset _now;

            public Getter(DateTimeOffset? now)
            {
                _now = now ?? DateTimeOffset.Now;
            }

            public string AssigneeCount(Issue issue) => issue?.Assignees?.GetCount();
            public string Assignees(Issue issue) => issue?.Assignees.AsList(user => user.Handle());
            public string AssigneesWithCount(Issue issue) =>
                issue?.Assignees.AsListWithCount(user => user.Handle());
            public string Author(Issue issue) => issue?.Author.Handle();
            public string AuthorAssociation(Issue issue) => issue?.AuthorAssociation.ToString();
            public string BodyText(Issue issue) => issue?.BodyText?.Truncate();
            public string Closed(Issue issue) => issue?.Closed.ToString();
            public string ClosedAt(Issue issue) => issue?.ClosedAt.ToShortLocal();
            public string CommentCount(Issue issue) => issue?.Comments?.GetCount();
            public string CreatedAt(Issue issue) => issue?.CreatedAt.ToShortLocal();
            public string Editor(Issue issue) => issue?.Editor.Handle();
            public string Id(Issue issue) => issue?.Id;
            public string LabelCount(Issue issue) => issue?.Labels?.GetCount();
            public string Labels(Issue issue) => issue?.Labels.AsList(label => label?.Name);
            public string LastCommentAuthor(Issue issue) =>
                issue?.Comments?.Nodes?.FirstOrDefault()?.Author.Handle();
            public string LastCommentDate(Issue issue) =>
                issue?.Comments?.Nodes?.FirstOrDefault()?.CreatedAt.ToShortLocal();
            public string LastEditedAt(Issue issue) => issue?.LastEditedAt.ToShortLocal();
            public string Number(Issue issue) => issue?.Number.ToString();
            public string ParticipantCount(Issue issue) => issue?.Participants?.GetCount();
            public string Participants(Issue issue) =>
                issue?.Participants.AsList(user => user.Handle());
            public string ParticipantsWithCount(Issue issue) =>
                issue?.Participants.AsListWithCount(user => user.Handle());
            public string PublishedAt(Issue issue) => issue?.PublishedAt.ToShortLocal();
            public string Repo(Issue issue) => issue?.Repository.Handle();
            public string State(Issue issue) => issue?.State.ToString();
            public string Title(Issue issue) => issue?.Title;
            public string UpdatedAt(Issue issue) => issue?.UpdatedAt.ToShortLocal();
            public string Url(Issue issue) => issue?.Url;

            public string AgeInDays(Issue issue) => issue?.AgeInDays(_now).ToString();
            public string DaysOpenWithoutComment(Issue issue) =>
                issue?.OpenWithoutCommentInDays(_now).ToString();
        }

        private Getter Accessors { get; }

        public IssueHelper(DateTimeOffset? now)
        {
            Accessors = new Getter(now);
            DefaultFormatter = new Dictionary<string, Func<Issue, string>> {
                { "Repo", Accessors.Repo },
                { "Issue #", Accessors.Number },
                { "Issue URL", Accessors.Url },
                { "Title", Accessors.Title },
                { "Author", Accessors.Author },
                { "Body", Accessors.BodyText },
                { "Labels", Accessors.Labels },
                { "Assignees", Accessors.AssigneesWithCount },
                { "Participants", Accessors.ParticipantsWithCount },
                { "Comment count", Accessors.CommentCount },
                { "Last comment author", Accessors.LastCommentAuthor },
                { "Last comment date", Accessors.LastCommentDate },
                { "State", Accessors.State },
                { "Created at", Accessors.CreatedAt },
                { "Closed at", Accessors.ClosedAt },
                { "Age (days)", Accessors.AgeInDays },
                { "Open w/o comment (days)", Accessors.DaysOpenWithoutComment },
            };
        }
    }
}

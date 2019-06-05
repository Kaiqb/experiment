using System;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>Represents a Git commit.</summary>
    [QlObject]
    public class Commit
    {
        /// <summary>The pull requests associated with a commit.</summary>
        [JsonProperty("associatedPullRequests")]
        public Connection<PullRequest> AssociatedPullRequests { get; set; }

        /// <summary>Comments made on the commit.</summary>
        [JsonProperty("comments")]
        public Connection<CommitComment> Comments { get; set; }

        /// <summary>The linear commit history starting from (and including) this commit, in the same order as `git log`.</summary>
        [JsonProperty("history")]
        public Connection<Commit> History { get; set; }

        /// <summary>The parents of a commit.</summary>
        [JsonProperty("parents")]
        public Connection<Commit> Parents { get; set; }

        /// <summary>The number of additions in this pull request.</summary>
        [JsonProperty("additions")]
        public int? Additions { get; set; }

        /// <summary>The actor who authored the pull request.</summary>
        [JsonProperty("author")]
        public Actor Author { get; set; }

        /// <summary>The datetime when this commit was authored.</summary>
        [JsonProperty("authoredDate")]
        public DateTimeOffset? AuthoredDate { get; set; }

        /// <summary>The number of changed files in this pull request.</summary>
        [JsonProperty("changedFiles")]
        public int? ChangedFiles { get; set; }

        /// <summary>The datetime when this commit was committed.</summary>
        [JsonProperty("committedDate")]
        public DateTimeOffset? CommittedDate { get; set; }

        /// <summary>The number of deleted files in this pull request.</summary>
        [JsonProperty("deletions")]
        public int? Deletions { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>The Git commit message.</summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>The Git commit message body.</summary>
        [JsonProperty("messageBody")]
        public string MessageBody { get; set; }

        /// <summary>The Git commit message body in HTML.</summary>
        [JsonProperty("messageBodyHTML")]
        public string MessageBodyHtml { get; set; }

        /// <summary>The Git commit message headline.</summary>
        [JsonProperty("messageHeadline")]
        public string MessageHeadline { get; set; }

        /// <summary>The Git commit message headline in HTML.</summary>
        [JsonProperty("messageHeadlineHTML")]
        public string MessageHeadlineHtml { get; set; }

        /// <summary>The datetime when this commit was pushed.</summary>
        [JsonProperty("pushedDate")]
        public DateTimeOffset? PushedDate { get; set; }

        /// <summary>The repository this commit belongs to.</summary>
        [JsonProperty("Repository")]
        public Repository Repository { get; set; }

        /// <summary>Status information for this commit.</summary>
        [JsonProperty("status")]
        public Status Status { get; set; }

        /// <summary>Commit's root Tree.</summary>
        [JsonProperty("tree")]
        public Tree Tree { get; set; }

        /// <summary>The URL for the commit.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        // and much more...
    }
}

using System;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary></summary>
    public class PullRequest
    {
        /// <summary>A list of Users assigned to this object.</summary>
        [JsonProperty("assignees")]
        public Connection<User> Assignees { get; set; }

        /// <summary>Lists the files changed within this pull request.</summary>
        [JsonProperty("files")]
        public Connection<PullRequestChangedFile> Files { get; set; }

        /// <summary>A list of labels associated with the object.</summary>
        [JsonProperty("labels")]
        public Connection<Label> Labels { get; set; }

        /// <summary>The number of additions in this pull request.</summary>
        [JsonProperty("additions")]
        public int? Additions { get; set; }

        /// <summary>The actor who authored the pull request.</summary>
        [JsonProperty("author")]
        public Actor Author { get; set; }

        /// <summary>Author's association with the subject of the pull request.</summary>
        [JsonProperty("authorAssociation")]
        public CommentAuthorAssociation? AuthorAssociation { get; set; }

        /// <summary>The repository associated with this pull request's base Ref.</summary>
        [JsonProperty("baseRepository")]
        public Repository BaseRepository { get; set; }

        /// <summary>The pull request description, as Markdown.</summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>The pull request description, as HTML.</summary>
        [JsonProperty("bodyHTML")]
        public string BodyHtml { get; set; }

        /// <summary>The pull request description, as plain text.</summary>
        [JsonProperty("bodyText")]
        public string BodyText { get; set; }

        /// <summary>The number of changed files in this pull request.</summary>
        [JsonProperty("changedFiles")]
        public int? ChangedFiles { get; set; }

        /// <summary>True if the pull request is closed.</summary>
        [JsonProperty("closed")]
        public bool? Closed { get; set; }

        /// <summary>Identifies the date and time when the pull request was closed.</summary>
        [JsonProperty("closedAt")]
        public DateTimeOffset? ClosedAt { get; set; }

        /// <summary>Identifies the date and time when the pull request was created.</summary>
        [JsonProperty("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>The number of deleted files in this pull request.</summary>
        [JsonProperty("deletions")]
        public int? Deletions { get; set; }

        /// <summary>The actor who last edited the pull request description.</summary>
        [JsonProperty("editor")]
        public Actor Editor { get; set; }

        /// <summary>The repository associated with this pull request's head Ref.</summary>
        [JsonProperty("headRepository")]
        public Repository HeadRepository { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>The commit that was created when this pull request was merged.</summary>
        [JsonProperty("mergeCommit")]
        public Commit MergeCommit { get; set; }

        /// <summary>True if the pull request was merged.</summary>
        [JsonProperty("merged")]
        public bool? Merged { get; set; }

        /// <summary>The permalink URI to the pull request.</summary>
        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        /// <summary>The repository associated with this pull request.</summary>
        [JsonProperty("Repository")]
        public Repository Repository { get; set; }

        /// <summary>The pull request title.</summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>Identifies the date and time when the pull request was last updated.</summary>
        [JsonProperty("updatedAt")]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>The URL for the pull request.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        // and much more...
    }
}

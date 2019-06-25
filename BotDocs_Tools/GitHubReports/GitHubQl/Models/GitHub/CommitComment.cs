using System;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    public class CommitComment
    {

        /// <summary>The actor who authored the comment.</summary>
        [JsonProperty("author")]
        public Actor Author { get; set; }

        /// <summary>Author's association with the subject of the comment.</summary>
        [JsonProperty("authorAssociation")]
        public CommentAuthorAssociation? AuthorAssociation { get; set; }

        /// <summary>The comment, as Markdown.</summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>The comment, as HTML.</summary>
        [JsonProperty("bodyHTML")]
        public string BodyHtml { get; set; }

        /// <summary>The comment, as plain text.</summary>
        [JsonProperty("bodyText")]
        public string BodyText { get; set; }

        /// <summary>Identifies the commit associated with the comment, if the commit exists.</summary>
        [JsonProperty("commit")]
        public Commit Commit { get; set; }

        /// <summary>Identifies the date and time when the comment was created.</summary>
        [JsonProperty("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>The actor who last edited the comment.</summary>
        [JsonProperty("editor")]
        public Actor Editor { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Identifies the date and time when the comment was last edited.</summary>
        [JsonProperty("lastEditedAt")]
        public DateTimeOffset? LastEditedAt { get; set; }

        /// <summary>Identifies the file path associated with the comment.</summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>Identifies the date and time when the comment was published.</summary>
        [JsonProperty("publishedAt")]
        public DateTimeOffset? PusblishedAt { get; set; }

        /// <summary>The repository associated with this comment.</summary>
        [JsonProperty("Repository")]
        public Repository Repository { get; set; }

        /// <summary>Identifies the date and time when the comment was last updated.</summary>
        [JsonProperty("updatedAt")]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>The permalink URL for the comment.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        // and much more...
    }
}

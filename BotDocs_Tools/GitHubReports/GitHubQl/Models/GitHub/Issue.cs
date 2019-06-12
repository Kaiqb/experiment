using System;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>An Issue is a place to discuss ideas, enhancements, tasks, and bugs for a project.</summary>
    public class Issue
    {
        ///<summary>A list of Users assigned to this object.</summary>
        [JsonProperty("assignees")]
        public Connection<User> Assignees { get; set; }

        ///<summary>A list of comments associated with the Issue.</summary>
        [JsonProperty("comments")]
        public Connection<IssueComment> Comments { get; set; }

        ///<summary>A list of labels associated with the object.</summary>
        [JsonProperty("labels")]
        public Connection<Label> Labels { get; set; }

        ///<summary>A list of Users that are participating in the Issue conversation.</summary>
        [JsonProperty("participants")]
        public Connection<User> Participants { get; set; }

        ///<summary>The actor who authored the issue.</summary>
        [JsonProperty("author")]
        public Actor Author { get; set; }

        ///<summary>Author's association with the subject of the comment.</summary>
        [JsonProperty("authorAssociation")]
        public CommentAuthorAssociation? AuthorAssociation { get; set; }

        /// <summary>The issue description, as Markdown.</summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>The issue description, as HTML.</summary>
        [JsonProperty("bodyHTML")]
        public string BodyHtml { get; set; }

        /// <summary>The issue description, as plain text.</summary>
        [JsonProperty("bodyText")]
        public string BodyText { get; set; }

        /// <summary>True if the issue is closed.</summary>
        [JsonProperty("closed")]
        public bool? Closed { get; set; }

        /// <summary>Identifies the date and time when the issue was closed.</summary>
        [JsonProperty("closedAt")]
        public DateTimeOffset? ClosedAt { get; set; }

        /// <summary>Identifies the date and time when the issue was created.</summary>
        [JsonProperty("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>The actor who last edited the issue description.</summary>
        [JsonProperty("editor")]
        public Actor Editor { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        ///<summary>The moment the editor made the last edit.</summary>
        [JsonProperty("lastEditedAt")]
        public DateTimeOffset? LastEditedAt { get; set; }

        ///<summary>Identifies the issue number.</summary>
        [JsonProperty("number")]
        public int? Number { get; set; }

        /// <summary>Identifies the date and time when the issue was published.</summary>
        [JsonProperty("publishedAt")]
        public DateTimeOffset? PublishedAt { get; set; }

        /// <summary>The repository associated with this pull request.</summary>
        [JsonProperty("Repository")]
        public Repository Repository { get; set; }

        ///<summary>Identifies the state of the issue.</summary>
        [JsonProperty("state")]
        public IssueState? State { get; set; }

        /// <summary>The issue title.</summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>Identifies the date and time when the issue was last updated.</summary>
        [JsonProperty("updatedAt")]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>The URL for the issue.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        // and much more...
    }
}

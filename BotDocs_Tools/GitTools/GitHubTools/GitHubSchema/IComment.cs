using Newtonsoft.Json;
using System;

namespace GitHubTools
{
    /// <summary>Represents a comment.</summary>
    public interface IComment
    {
        /// <summary>The actor who authored the comment.</summary>
        [JsonProperty(PropertyName = "author")]
        Actor Author { get; set; }

        /// <summary>Author's association with the subject of the comment.</summary>
        [JsonProperty(PropertyName = "author")]
        CommentAuthorAssociation AuthorAssociation { get; set; }

        /// <summary>The body as Markdown.</summary>
        [JsonProperty(PropertyName = "body")]
        string Body { get; set; }

        /// <summary>The date and time when the object was created.</summary>
        [JsonProperty(PropertyName = "createdAt")]
        DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }

        /// <summary>Identifies the date and time when the object was last updated.</summary>
        [JsonProperty(PropertyName = "updatedAt")]
        DateTime? UpdatedAt { get; set; }

        // and others...
    }
}
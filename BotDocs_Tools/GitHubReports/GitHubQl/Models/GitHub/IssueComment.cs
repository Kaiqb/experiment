using Newtonsoft.Json;
using System;

namespace GitHubQl.Models.GitHub
{
    [QlObject]
    public class IssueComment
    {
        /// <summary>The actor who authored the comment.</summary>
        [JsonProperty(PropertyName = "author")]
        public Actor Author { get; set; }

        /// <summary>Author's association with the subject of the comment.</summary>
        [JsonProperty(PropertyName = "authorAssociation")]
        public CommentAuthorAssociation AuthorAssociation { get; set; }

        /// <summary>Identifies the date and time when the item was created.</summary>
        [JsonProperty("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }

        // and many more...
    }
}

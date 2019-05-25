using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    public class IssueComment
    {
        /// <summary>The actor who authored the comment.</summary>
        [JsonProperty(PropertyName = "author")]
        public Actor Author { get; set; }

        /// <summary>Author's association with the subject of the comment.</summary>
        [JsonProperty(PropertyName = "authorAssociation")]
        public CommentAuthorAssociation AuthorAssociation { get; set; }

        // and many more...
    }
}

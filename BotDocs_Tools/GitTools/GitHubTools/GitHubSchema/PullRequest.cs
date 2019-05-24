using Newtonsoft.Json;
using System;

namespace GitHubTools
{
    public class PullRequest :
        IAssignable, IClosable, IComment, ILabelable, ILockable, INode, IReactable, IRepositoryNode,
        ISubscribable, IUniformResourceLocatable, IUpdatable, IUpdatableComment
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>The actor who authored the comment.</summary>
        [JsonProperty(PropertyName = "author")]
        public Actor Author { get; set; }

        /// <summary>Author's association with the subject of the comment.</summary>
        [JsonProperty(PropertyName = "author")]
        public CommentAuthorAssociation AuthorAssociation { get; set; }

        /// <summary>The body as Markdown.</summary>
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        /// <summary>The date and time when the object was created.</summary>
        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>Identifies the date and time when the object was last updated.</summary>
        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>true if the object is closed (definition of closed may depend on type)</summary>
        [JsonProperty(PropertyName = "closed")]
        public bool? Closed { get; set; }

        /// <summary>Identifies the date and time when the object was closed.</summary>
        [JsonProperty(PropertyName = "closedAt")]
        public DateTime ClosedAt { get; set; }
    }
}

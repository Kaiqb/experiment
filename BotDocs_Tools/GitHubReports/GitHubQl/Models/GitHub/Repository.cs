using System;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    public class Repository
    {
        /// <summary>A list of users that can be assigned to issues in this repository.</summary>
        [JsonProperty("assignableUsers")]
        public Connection<User> AssignableUsers { get; set; }

        /// <summary>A list of collaborators associated with the repository.</summary>
        [JsonProperty("assignableUsers")]
        public Connection<RepositoryCollaborator> Collaborators { get; set; }

        /// <summary>A list of direct forked repositories.</summary>
        [JsonProperty("forks")]
        public Connection<Repository> Forks { get; set; }

        /// <summary>A list of issues that have been opened in the repository.</summary>
        [JsonProperty("issues")]
        public Connection<Issue> Issues { get; set; }

        /// <summary>A list of labels associated with the repository.</summary>
        [JsonProperty("labels")]
        public Connection<Label> Labels { get; set; }

        /// <summary>A list of pull requests that have been opened in the repository.</summary>
        [JsonProperty("pullRequests")]
        public Connection<PullRequest> PullRequests { get; set; }

        /// <summary>Identifies the date and time when the repository was created.</summary>
        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>The repository's description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>The repository's description as HTML.</summary>
        [JsonProperty("descriptioHTML")]
        public string DescriptionHtml { get; set; }

        /// <summary>Returns how many forks there are of this repository in the whole network.</summary>
        [JsonProperty("forkCount")]
        public int? ForkCount { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Identifies if the repository is private.</summary>
        [JsonProperty("isPrivate")]
        public bool? IsPrivate { get; set; }

        /// <summary>The repository's name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The repository's name with owner.</summary>
        [JsonProperty("nameWithOwner")]
        public string NameWithOwner { get; set; }

        /// <summary>The User owner of the repository.</summary>
        [JsonProperty("owner")]
        public RepositoryOwner Owner { get; set; }

        /// <summary>The repository parent, if this is a fork.</summary>
        [JsonProperty("parent")]
        public Repository Parent { get; set; }

        /// <summary>Identifies when the repository was last pushed to.</summary>
        [JsonProperty("pushedAt")]
        public DateTimeOffset PushedAt { get; set; }

        /// <summary>The HTTP URL for this repository.</summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        // and much more...
    }
}

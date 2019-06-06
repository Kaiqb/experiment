using System;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>A label for categorizing Issues or Milestones with a given Repository.</summary>
    [QlObject]
    public class Label
    {
        ///<summary>A list of issues associated with this label.</summary>
        [JsonProperty("issues")]
        public Connection<Issue> Issues { get; set; }

        ///<summary>A list of pull requests associated with this label.</summary>
        [JsonProperty("pullRequests")]
        public Connection<PullRequest> PullRequests { get; set; }

        /// <summary>Identifies the date and time when the label was created.</summary>
        [JsonProperty("createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }

        ///<summary>A brief description of this label.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        ///<summary>The name of the label.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The repository associated with this label.</summary>
        [JsonProperty("Repository")]
        public Repository Repository { get; set; }

        /// <summary>Identifies the date and time when the label was last updated.</summary>
        [JsonProperty("updatedAt")]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>The URL for the label.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        // and more...
    }
}

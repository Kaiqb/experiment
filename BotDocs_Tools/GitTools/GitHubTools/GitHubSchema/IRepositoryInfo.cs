using Newtonsoft.Json;
using System;

namespace GitHubTools
{
    /// <summary>A subset of repository info.</summary>
    public interface IRepositoryInfo
    {
        /// <summary>The date and time when the object was created.</summary>
        [JsonProperty(PropertyName = "createdAt")]
        DateTime? CreatedAt { get; set; }

        /// <summary>The description of the repository.</summary>
        [JsonProperty(PropertyName = "description")]
        string Description { get; set; }

        /// <summary>The name of the repository.</summary>
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }

        /// <summary>The repository's name with owner.</summary>
        [JsonProperty(PropertyName = "nameWithOwner")]
        string NameWithOwner { get; set; }

        /// <summary>The user owner of the repository.</summary>
        [JsonProperty(PropertyName = "owner")]
        RepositoryOwner Owner { get; set; }

        // and others...
    }
}

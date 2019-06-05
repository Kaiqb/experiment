using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GitHubQl.Models.GitHub
{
    /// <summary>Since the GraphQL data object just contains the objects queried for, one common type
    /// should (?) work for any GitHubQL query.</summary>
    public class GitHubQlData
    {
        ///<summary>The organization that was queried for, if any.</summary>
        [JsonProperty("organization")]
        public Organization Organization { get; set; }

        ///<summary>The repository that was queried for, if any.</summary>
        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        ///<summary>The repository owner queried for, if any.</summary>
        [JsonProperty("repositoryOwner")]
        public RepositoryOwner RepositoryOwner { get; set; }

        ///<summary>The user queried for, if any.</summary>
        [JsonProperty("user")]
        public User User { get; set; }

        /// <remarks>The response map may also contain an entry with key `extensions`. This entry, if
        /// set, must have a map as its value. This entry is reserved for implementors to extend the
        /// protocol however they see fit, and hence there are no additional restrictions on its
        /// contents.</remarks>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditonalEntries { get; set; }
    }
}

using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>Represents a Git object.</summary>
    public class GitObject
    {
        ///<summary>An abbreviated version of the Git object ID.</summary>
        [JsonProperty("abbreviatedOid")]
        public string AbbreviatedOid { get; set; }

        ///<summary>The HTTP path for this Git object.</summary>
        [JsonProperty("commitResourcePath")]
        public string CommitResourcePath { get; set; }

        ///<summary>The HTTP URL for the Git object.</summary>
        [JsonProperty("commitUrl")]
        public string CommitUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        ///<summary>Entry file Git object ID.</summary>
        [JsonProperty("oid")]
        public string Oid { get; set; }

        /// <summary>The repository this object belongs to.</summary>
        [JsonProperty("Repository")]
        public Repository Repository { get; set; }
    }
}

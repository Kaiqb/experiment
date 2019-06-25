using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>Represents a Git tree entry.</summary>
    public class TreeEntry
    {
        ///<summary>Entry file mode.</summary>
        [JsonProperty("mode")]
        public int? Mode { get; set; }

        ///<summary>Entry file name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        ///<summary>Entry file object.</summary>
        [JsonProperty("object")]
        public GitObject Object { get; set; }

        ///<summary>Entry file Git object ID.</summary>
        [JsonProperty("oid")]
        public string Oid { get; set; }

        /// <summary>The repository this object belongs to.</summary>
        [JsonProperty("Repository")]
        public Repository Repository { get; set; }

        ///<summary>Entry file type.</summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}

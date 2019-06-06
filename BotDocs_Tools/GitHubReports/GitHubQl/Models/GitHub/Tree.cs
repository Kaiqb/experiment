using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>Represents a Git tree.</summary>
    [QlObject]
    public class Tree
    {
        ///<summary>The HTTP URL for this Git object.</summary>
        [JsonProperty("commitUrl")]
        public string CommitUrl { get; set; }

        ///<summary>A list of tree entries.</summary>
        [JsonProperty("entries")]
        public IList<TreeEntry> Entries { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>The repository this object belongs to.</summary>
        [JsonProperty("Repository")]
        public Repository Repository { get; set; }
    }
}

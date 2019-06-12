using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>A file changed in a pull request.</summary>
    public class PullRequestChangedFile
    {
        /// <summary>The number of additions to the file.</summary>
        [JsonProperty("additions")]
        public int? Additions { get; set; }

        /// <summary>The number of deletions in the file.</summary>
        [JsonProperty("deletions")]
        public int? Deletions { get; set; }

        /// <summary>The file path.</summary>
        [JsonProperty("path")]
        public string Path { get; set; }
    }
}

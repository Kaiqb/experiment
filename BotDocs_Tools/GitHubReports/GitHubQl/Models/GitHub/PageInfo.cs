using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>Information about pagination in a connection.</summary>
    [QlObject]
    public class PageInfo
    {
        /// <summary>When paginating forwards, the cursor to continue.</summary>
        [JsonProperty("endCursor")]
        public string EndCursor { get; set; }

        /// <summary>When paginating forwards, are there more items?</summary>
        [JsonProperty("hasNextPage")]
        public bool? HasNextPage { get; set; }

        /// <summary>When paginating backwards, are there more items?</summary>
        [JsonProperty("hasPreviousPage")]
        public bool? HasPreviousPage { get; set; }

        /// <summary>When paginating backwards, the cursor to continue.</summary>
        [JsonProperty("startCursor")]
        public string StartCursor { get; set; }
    }
}

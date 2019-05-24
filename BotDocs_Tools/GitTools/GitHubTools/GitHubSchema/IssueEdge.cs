using Newtonsoft.Json;

namespace GitHubTools
{
    /// <summary>An edge in a connection.</summary>
    public class IssueEdge
    {
        /// <summary>A cursor for use in pagination.</summary>
        [JsonProperty(PropertyName = "cursor")]
        public string Cursor { get; set; }

        /// <summary>The item at the end of the edge.</summary>
        [JsonProperty(PropertyName = "node")]
        public Issue Node { get; set; }
    }
}

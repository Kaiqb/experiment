using Newtonsoft.Json;
using System.Collections.Generic;

namespace GitHubTools
{
    /// <summary>A list of edges.</summary>
    public class PullRequestConnection
    {
        /// <summary>A list of edges.</summary>
        [JsonProperty(PropertyName = "edges")]
        public IList<PullRequestEdge> Edges { get; set; }

        /// <summary>A list of nodes.</summary>
        [JsonProperty(PropertyName = "nodes")]
        public IList<PullRequest> Nodes { get; set; }

        /// <summary>Pagination information.</summary>
        [JsonProperty(PropertyName = "pageInfo")]
        public PageInfo PageInfo { get; set; }

        /// <summary>The total count of items in the connection.</summary>
        [JsonProperty(PropertyName = "totalCount")]
        public int? TotalCount { get; set; }
    }
}

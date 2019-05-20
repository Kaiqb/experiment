using Newtonsoft.Json;
using System.Collections.Generic;

namespace GitHubTools
{
    /// <summary>The connection type for Issue.</summary>
    public class IssueConnection
    {
        /// <summary>A list of edges.</summary>
        [JsonProperty(PropertyName = "edges")]
        public IList<IssueEdge> Edges { get; set; }

        /// <summary>A list of nodes.</summary>
        [JsonProperty(PropertyName = "nodes")]
        public IList<Issue> Nodes { get; set; }

        /// <summary>Pagination information.</summary>
        [JsonProperty(PropertyName = "pageInfo")]
        public PageInfo PageInfo { get; set; }

        /// <summary>The total count of items in the connection.</summary>
        [JsonProperty(PropertyName = "totalCount")]
        public int? TotalCount { get; set; }
    }
}

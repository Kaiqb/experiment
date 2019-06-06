using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>Summarizes the connections to a given type.</summary>
    /// <typeparam name="T">The type of items connected to.</typeparam>
    [QlConnection]
    public class Connection<T>
    {
        /// <summary>The list of edges/connections.</summary>
        [JsonProperty("egdes"), QlArray]
        public IList<Edge<T>> Edges { get; set; }

        /// <summary>The list of nodes/items.</summary>
        [JsonProperty("nodes"), QlArray]
        public List<T> Nodes { get; set; }

        /// <summary>Pagination information.</summary>
        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        /// <summary>The total connections.</summary>
        [JsonProperty("totalCount")]
        public int? TotalCount { get; set; }
    }
}

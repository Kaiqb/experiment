using Newtonsoft.Json;

namespace GitHubQl.Models.GraphQl
{
    /// <summary>Describes a location in a GraphQL document.</summary>
    public class GraphQLLocation
    {
        /// <summary>1-based line number of location.</summary>
        [JsonProperty("line")]
        public long Line { get; }

        /// <summary>1-based column number of location.</summary>
        [JsonProperty("column")]
        public long Column { get; }
    }
}

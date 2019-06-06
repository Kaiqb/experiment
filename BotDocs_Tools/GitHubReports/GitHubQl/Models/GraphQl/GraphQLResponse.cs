using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GitHubQl.Models.GraphQl
{
    /// <summary>A response to a GraphQL operation.</summary>
    public class GraphQLResponse<T>
    {
        public GraphQLResponse(T data, GraphQLError[] errors) => (Data, Errors) = (data, errors);

        /// <summary>The requested information.</summary>
        /// <remarks>If the operation included execution, the response map must contain an entry with
        /// key `data`. The value of this entry is described in the "Data" section. If the operation
        /// failed before execution, due to a syntax error, missing information, or validation error,
        /// this entry must not be present.</remarks>
        [JsonProperty("data")]
        public T Data { get; }

        /// <summary>Any errors the GitHub GraphQL service identified.</summary>
        /// <remarks>If the operation encountered any errors, the response map must contain an entry
        /// with key `errors`. The value of this entry is described in the "Errors" section. If the operation
        /// completed without encountering any errors, this entry must not be present.</remarks>
        [JsonProperty("errors")]
        public GraphQLError[] Errors { get; }

        /// <remarks>The response map may also contain an entry with key `extensions`. This entry, if
        /// set, must have a map as its value. This entry is reserved for implementors to extend the
        /// protocol however they see fit, and hence there are no additional restrictions on its
        /// contents.</remarks>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditonalEntries { get; set; }
    }
}

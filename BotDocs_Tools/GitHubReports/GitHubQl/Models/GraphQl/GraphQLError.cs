using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GitHubQl.Models.GraphQl
{
    /// <summary>Describes an error encountered in a GraphQL document.</summary>
    public class GraphQLError
    {
        public GraphQLError(string message, GraphQLLocation[] locations) => (Message, Locations) = (message, locations);

        /// <summary>A description of the error.</summary>
        [JsonProperty("message")]
        public string Message { get; }

        /// <summary>A list of starting location of each associated syntax element.</summary>
        [JsonProperty("locations")]
        public GraphQLLocation[] Locations { get; }

        /// <remarks>If an error can be associated to a particular field in the GraphQL result, it
        /// must contain an entry with the key path that details the path of the response field which
        /// experienced the error. This allows clients to identify whether a null result is intentional
        /// or caused by a runtime error. A path is expressed as a series of strings and 0-based indices
        /// (for array elements).
        /// GraphQL services may provide an additional entry to errors with key extensions. This entry,
        /// if set, must have a map as its value. This entry is reserved for implementors to add additional
        /// information to errors however they see fit, and there are no additional restrictions on
        /// its contents.</remarks>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditonalEntries { get; set; }
    }
}

using Newtonsoft.Json;

namespace GitHubQl.Models.GraphQl
{
    /// <summary>The payload of a GraphQL request.</summary>
    public class GraphQLRequest
    {
        public GraphQLRequest(string query, string variables = null) => (Query, Variables) = (query, variables);

        /// <summary>If the operation is a query, the result of the operation is the result of executing
        /// the query's top level selection set with the query root object type.</summary>
        /// <remarks><see>https://graphql.github.io/graphql-spec/June2018/#sec-Query</see></remarks>
        [JsonProperty("query")]
        public string Query { get; }

        /// <summary>Any parameterized variables.</summary>
        /// <remarks><see>https://graphql.github.io/graphql-spec/June2018/#sec-Language.Variables</see></remarks>
        [JsonProperty("variables")]
        public string Variables { get; }
    }
}

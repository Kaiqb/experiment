using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>Represents a connection to an item.</summary>
    /// <typeparam name="T">The type of the item connected to.</typeparam>
    public class Edge<T>
    {
        /// <summary>A cursor for use in pagination.</summary>
        [JsonProperty("cursor")]
        public string Cursor { get; set; }

        /// <summary>The item at the end of the edge.</summary>
        [JsonProperty("node")]
        public T Node { get; set; }
    }
}

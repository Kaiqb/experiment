using Newtonsoft.Json;

namespace GitHubTools
{
    /// <summary>An object with an ID.</summary>
    public interface INode
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }
    }
}

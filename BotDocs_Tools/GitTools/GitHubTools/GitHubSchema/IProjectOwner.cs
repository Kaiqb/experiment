using Newtonsoft.Json;

namespace GitHubTools
{
    public interface IProjectOwner
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }

        // and others...
    }
}

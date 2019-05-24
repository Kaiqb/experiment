using Newtonsoft.Json;

namespace GitHubTools
{
    public interface IRegistryPackageOwner
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }
    }
}

using Newtonsoft.Json;

namespace GitHubTools
{
    public class RepositoryOwner
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "login")]
        /// <summary>The username used to log in.</summary>
        public string Login { get; set; }

        // and others...
    }
}

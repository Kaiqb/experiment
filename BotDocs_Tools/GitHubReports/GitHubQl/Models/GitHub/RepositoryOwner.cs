using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    public class RepositoryOwner
    {
        /// <summary>A URL pointing to the user's public avatar.</summary>
        [JsonProperty(PropertyName = "avatarUrl")]
        public string AvatarUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>The username used to login.</summary>
        [JsonProperty("login")]
        public string Login { get; set; }

        /// <summary>The HTTP path for this user.</summary>
        [JsonProperty(PropertyName = "resourcePath")]
        public string ResourcePath { get; set; }

        /// <summary>The HTTP URL for this user.</summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}

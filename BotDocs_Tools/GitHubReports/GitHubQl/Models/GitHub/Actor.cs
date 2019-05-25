using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    public class Actor
    {
        /// <summary>A URL pointing to the actor's public avatar.</summary>
        [JsonProperty(PropertyName = "avatarUrl")]
        public string AvatarUrl { get; set; }

        /// <summary>The username of the actor.</summary>
        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        /// <summary>The HTTP path for this actor.</summary>
        [JsonProperty(PropertyName = "resourcePath")]
        public string ResourcePath { get; set; }

        /// <summary>The HTTP URL for this actor.</summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}

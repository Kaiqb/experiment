using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>An account on GitHub, with one or more owners, that has repositories, members and teams.</summary>
    [QlObject]
    public class Organization
    {
        /// <summary>A list of users who are members of this organization.</summary>
        [JsonProperty("members")]
        public Connection<User> Members { get; set; }

        /// <summary>A list of repositories that the user owns.</summary>
        [JsonProperty("repositories")]
        public Connection<Repository> Repositories { get; set; }

        /// <summary>The organization's public profile description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>The organization's public email.</summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>The organization's public profile location.</summary>
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>The organization's login name.</summary>
        [JsonProperty("login")]
        public string Login { get; set; }

        /// <summary>The organization's public profile name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The HTTP path for this organization.</summary>
        [JsonProperty(PropertyName = "resourcePath")]
        public string ResourcePath { get; set; }

        /// <summary>The HTTP URL for this organization.</summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>The organization's public profile URL.</summary>
        [JsonProperty(PropertyName = "websiteUrl")]
        public string WebsiteUrl { get; set; }

        // and much more...
    }
}

using System;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>A user is an individual's account on GitHub that owns repositories and can make new content.</summary>
    [QlObject]
    public class User
    {
        /// <summary>A list of users the given user is followed by.</summary>
        [JsonProperty("followers")]
        public Connection<User> Followers { get; set; }

        /// <summary>A list of issue comments made by the user.</summary>
        [JsonProperty("issueComments")]
        public Connection<IssueComment> IssueComments { get; set; }

        /// <summary>A list of issues associated with the user.</summary>
        [JsonProperty("issues")]
        public Connection<Issue> Issues { get; set; }

        /// <summary>A list of organizations the user belongs to.</summary>
        [JsonProperty("organizations")]
        public Connection<Organization> Organizations { get; set; }

        /// <summary>A URL pointing to the user's public avatar.</summary>
        [JsonProperty(PropertyName = "avatarUrl")]
        public string AvatarUrl { get; set; }

        /// <summary>The user's public profile bio.</summary>
        [JsonProperty("bio")]
        public string Bio { get; set; }

        /// <summary>The user's public profile bio as HTML.</summary>
        [JsonProperty("bioHTML")]
        public string BioHtml { get; set; }

        /// <summary>The user's public profile company.</summary>
        [JsonProperty("company")]
        public string Company { get; set; }

        /// <summary>Identifies the date and time when the user's account was created.</summary>
        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>The user's publicly visible profile email.</summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Whether or not this user is a participant in the GitHub Security Bug Bounty.</summary>
        [JsonProperty("isBountyHunter")]
        public bool? IsBountyHunter { get; set; }

        /// <summary>The user's public profile location.</summary>
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>The username used to login.</summary>
        [JsonProperty("login")]
        public string Login { get; set; }

        /// <summary>The user's public profile name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The HTTP path for this user.</summary>
        [JsonProperty(PropertyName = "resourcePath")]
        public string ResourcePath { get; set; }

        /// <summary>The HTTP URL for this user.</summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        // and much more...
    }
}

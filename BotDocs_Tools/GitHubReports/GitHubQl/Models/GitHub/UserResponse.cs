using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary></summary>
    public class UserResponse
    {
        /// <summary></summary>
        [JsonProperty("user")]
        public User User { get; set; }
    }

    public class PullRequest
    {
    }

    public class Label
    {
    }

    public class RepositoryCollaborator
    {
    }

    public class Issue
    {
    }
}

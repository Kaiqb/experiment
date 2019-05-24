using Newtonsoft.Json;

namespace GitHubTools
{
    public class Actor
    {
        [JsonProperty(PropertyName = "login")]
        /// <summary>The username of the actor.</summary>
        string Login { get; set; }
        // and others...
    }
}
using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    /// <summary>Represents a commit status.</summary>
    [QlObject]
    public class Status
    {
        ///<summary>The commit this status is attached to.</summary>
        [JsonProperty("commit")]
        public Commit Commit { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        ///<summary>The combined commit status.</summary>
        [JsonProperty("state")]
        public StatusState? State { get; set; }
    }
}

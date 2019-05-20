using Newtonsoft.Json;
using System;

namespace GitHubTools
{
    /// <summary>An object that can be closed.</summary>
    public interface IClosable
    {
        /// <summary>true if the object is closed (definition of closed may depend on type)</summary>
        [JsonProperty(PropertyName = "closed")]
        bool? Closed { get; set; }

        /// <summary>Identifies the date and time when the object was closed.</summary>
        [JsonProperty(PropertyName = "closedAt")]
        DateTime ClosedAt { get; set; }
    }
}
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ReportUtils
{
    /// <summary>Information from the publishing config file for a dependent repository.</summary>
    public class RepositoryInfo
    {
        [JsonProperty(PropertyName = "path_to_root")]
        public string PathToRoot { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "branch")]
        public string Branch { get; set; }
    }

    /// <summary>Represents information contained in the doc repo's publishing confing file.</summary>
    public class PublishConfig
    {
        /// <summary>The repos listed in the `dependent_repositories` section.</summary>
        [JsonProperty(PropertyName = "dependent_repositories")]
        public IList<RepositoryInfo> DependentRepositories { get; set; }
    }
}

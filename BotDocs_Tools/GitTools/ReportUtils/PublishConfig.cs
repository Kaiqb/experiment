using Newtonsoft.Json;
using System.Collections.Generic;

namespace ReportUtils
{
    public class RepositoryInfo
    {
        [JsonProperty(PropertyName = "path_to_root")]
        public string PathToRoot { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "branch")]
        public string Branch { get; set; }
    }

    public class PublishConfig
    {
        [JsonProperty(PropertyName = "dependent_repositories")]
        public IList<RepositoryInfo> DependentRepositories { get; set; }
    }
}

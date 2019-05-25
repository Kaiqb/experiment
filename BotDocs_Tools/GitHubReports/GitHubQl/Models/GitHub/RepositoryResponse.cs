using Newtonsoft.Json;

namespace GitHubQl.Models.GitHub
{
    public class RepositoryResponse
    {
        public RepositoryResponse(Repository repository) => Repository = repository;

        [JsonProperty("repository")]
        public Repository Repository { get; }
    }
}

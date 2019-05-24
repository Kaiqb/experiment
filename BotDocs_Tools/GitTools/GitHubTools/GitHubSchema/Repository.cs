using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GitHubTools
{
    public class GitHubData<T>
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }

    public class RepoData
    {
        [JsonProperty(PropertyName = "repository")]
        public Repository Repository { get; set; }
    }

    /// <summary>A repository contains the content for a project.</summary>
    public class Repository :
        INode, IProjectOwner, IRegistryPackageOwner, IRepositoryInfo, IStarrable, ISubscribable,
        IUniformResourceLocatable
    {
        /// <summary>A list of issues that have been opened in the repository.</summary>
        [JsonProperty(PropertyName = "issues")]
        public IssueConnection Issues { get; set; }

        /// <summary>A list of pull requests that have been opened in the repository.</summary>
        [JsonProperty(PropertyName = "pullRequests")]
        public IList<PullRequestConnection> PullRequests { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>The date and time when the object was created.</summary>
        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>The description of the repository.</summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>The name of the repository.</summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>The repository's name with owner.</summary>
        [JsonProperty(PropertyName = "nameWithOwner")]
        public string NameWithOwner { get; set; }

        /// <summary>The user owner of the repository.</summary>
        [JsonProperty(PropertyName = "owner")]
        public RepositoryOwner Owner { get; set; }
    }
}

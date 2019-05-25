using Refit;
using System;
using System.Net.Http;

namespace GitHubQl
{
    public static class GitHubGraphQlService
    {
        readonly static Lazy<IGitHubAPI> _githubApiClientHolder
            = new Lazy<IGitHubAPI>(() => RestService.For<IGitHubAPI>(CreateHttpClient()));

        static IGitHubAPI GitHubApiClient => _githubApiClientHolder.Value;

        static HttpClient CreateHttpClient()
        {
            return new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip })
            {
                BaseAddress = new Uri(GitHubConstants.APIUrl)
            };
        }

    }
}

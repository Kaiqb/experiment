using GitHubQl.Models.GitHub;
using GitHubQl.Models.GraphQl;
using Refit;
using System.Threading.Tasks;

namespace GitHubQl
{
    /// <summary>Describes GitHub GraphQL as a REST service.</summary>
    /// <remarks>This has one endpoint, the base address of the HTTP client used to create the Refit REST service for this API.
    /// Attributes from the Refit library help describe features of the endpoint.</remarks>
    [Headers(
        "Accept-Encoding: gzip",
        "Content-Type: application/json",
        "Authorization: bearer " + GitHubConstants.PersonalAccessToken,
        "User-Agent: GitHubExplorer")]
    public interface IGitHubAPI
    {
        /// <summary>All queries have the same structure.</summary>
        /// <param name="request">The GraphQL query payload for GitHub.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>If the task is successful, the result contains the GitHub response payload.
        [Post("")]
        Task<GraphQLResponse<GitHubQlData>> Query([Body] GraphQLRequest request);
    }

    public delegate Task<GraphQLResponse<GitHubQlData>> QueryFunc([Body] GraphQLRequest request);
}

using GitHubQl.Models.GitHub;
using GitHubQl.Models.GraphQl;
using Refit;
using System.Threading.Tasks;

namespace GitHubQl
{
    [Headers(
        "Accept-Encoding: gzip",
        "Content-Type: application/json",
        "Authorization: bearer " + GitHubConstants.PersonalAccessToken,
        "User-Agent: GitHubExplorer")]
    public interface IGitHubAPI
    {
        [Post("")]
        Task<GraphQLResponse<UserResponse>> UserQuery([Body] GraphQLRequest request);

        [Post("")]
        Task<GraphQLResponse<RepositoryResponse>> RepositoryQuery([Body] GraphQLRequest request);
    }
}

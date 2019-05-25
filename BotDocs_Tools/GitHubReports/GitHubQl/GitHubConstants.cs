using System;
using System.Collections.Generic;

namespace GitHubQl
{
    public static class GitHubConstants
    {
        [Flags]
        public enum RepoTypes
        {
            Unknown = 0x0,
            None = 0x0,
            Docs = 0x01,
            Specs = 0x02,
            Code = 0x04,
            Public = 0x10,
            Private = 0x20,
        }

        public class RepoParams
        {
            public string Owner { get; set; }
            public string Name { get; set; }
            public RepoTypes Type { get; set; }
        }

        /// <summary>The base URL for GitHub GraphQL endpoint queries.</summary>
        public const string APIUrl = "https://api.github.com/graphql";

        /// <summary>The GitHub user name of the person running the report.</summary>
        /// <remarks>This should be a user property in the app project.</remarks>
        public const string UserName = "";

        /// <summary>The GitHub user token for the person running the report.</summary>
        /// <remarks>This should be a user property in the app project.</remarks>
        public const string PersonalAccessToken = "";

        /// <summary>The repositories we already know about and can run automated reports against.</summary>
        /// <remarks>This should be an app property in the app project.</remarks>
        public static IReadOnlyList<RepoParams> KnownRepos { get; } =
            new List<RepoParams> {
                new RepoParams {
                    Owner = "MicrosoftDocs", Name = "bot-docs", Type = RepoTypes.Docs | RepoTypes.Public },
                new RepoParams {
                    Owner = "MicrosoftDocs", Name = "bot-docs-pr", Type = RepoTypes.Docs | RepoTypes.Private },
                new RepoParams {
                    Owner = "microsoft", Name = "botframework-sdk", Type = RepoTypes.Specs | RepoTypes.Public },
                new RepoParams {
                    Owner = "microsoft", Name = "botbuilder-dotnet", Type = RepoTypes.Code | RepoTypes.Public },
                new RepoParams {
                    Owner = "microsoft", Name = "botbuilder-js", Type = RepoTypes.Code | RepoTypes.Public },
                new RepoParams {
                    Owner = "microsoft", Name = "BotBuilder-Samples", Type = RepoTypes.Code | RepoTypes.Public },
            };
    }
}

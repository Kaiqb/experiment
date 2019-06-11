# GitHub reporting tool for the Bot Framework docs team

Currently, this is set up as a self-contained, .NET 3.0 Core command-line app.
It includes a couple libraries to help write queries against the GitHub GraphQL API and generate reports.

Reports currently generated:
- basic stats on the bot-docs repo
- an issues report for the bot-docs repo

## To run

You will need a GitHub user access token with the following permissions:

- repo:status
- repo_deployment
- public_repo
- read:org
- read:public_key
- read:repo_hook
- user (all)
- read:gpg_key

See [Authenticating with GraphQL](https://developer.github.com/v4/guides/forming-calls/#authenticating-with-graphql) for more details.

1. If you've built this locally, navigate to the project's `bin\Release\netcoreapp3.0\publish\` directory; otherwise, navigate to the directory where you unzipped the release files.
1. Enter `GitHubReportCli.exe` on the command line.
    - The first time through, you'll be prompted for your GitHub user name and user access token.
    - To clear these and start fresh (if you need to use a different token) run `GitHubReportCli.exe clear`.

## To build

You'll need Visual Studio 2019 and .NET 3.0, both currently in preview.

This also uses the Polly and Refit NuGet packages to help with calls to GitHub's REST API.

## To "publish"

Right-click the **GitHubReportCli** project, and click **Publish**.

1. Double check the publishing profile. You may need to change the target location.
1. Zip the output directory. I've been adding a version number to .zip file to help keep track of what's been posted to Teams.
1. Post it to Teams/BCC/Files/In-house-tools/GitHubReportCli.

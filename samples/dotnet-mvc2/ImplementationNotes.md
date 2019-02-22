# Implementation notes

Create a new project: ASP.NET Core Web Application
- .NET Core, ASP.NET Core 2.1, API, no authentication

## NuGet/MyGet
- Add Microsoft.Bot.Builder.Integration.AspNet.Core 4.3.0

## Update Properties/launchSettings.json
- Change port to 3978
- Remove iisSettings/iisExpress/sslPort
- Remove profiles/IIS Express/launchUrl
- Remove profiles/<proj-name>/launchUrl
- Change profiles/<proj-name>/applicationUrl:
  - from: "https://localhost:5001;http://localhost:5000"
  - to:   "http://localhost:5000"

## Add wwwroot/default.html
-Add boilerplate launch page

## Startup
- Add the current boilerplate to `ConfigureServices`: register bot and adapter.
- Add `.UseDefaultFiles().UseStaticFiles()` to `Configure`.

## Controllers
In the Controllers directory:
- Rename controller to BotController.
- Replace its guts with boilerplate: constructor, properties, POST handler.
> ![IMPORTANT]
> A Route attribute of "api/[controller]" means that ASP.NET looks for an appropriate class named <controller>Controller. So, if your controller is BotController, your endpoint will be BotController.

## Bots/MyBot
Create the MyBot file and derive the class from ActivityHandler:

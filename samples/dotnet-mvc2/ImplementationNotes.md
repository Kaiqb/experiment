# Implementation notes

Create a new project: ASP.NET Core Web Application
- .NET Core, ASP.NET Core 2.1, API, no authentication.
- Leave **Configure for HTTPS** selected.

## NuGet/MyGet

- Add Microsoft.Bot.Builder.Integration.AspNet.Core 4.3.0

## Update Properties/launchSettings.json

- Optionally change `iisSettings/iisExpress/applicationUrl` and `iisSettings/iisExpress/sslPort` to user your preferred ports.
- Remove `profiles/IIS Express/launchUrl`.
- Remove `profiles/<proj-name>/launchUrl`.

## Add wwwroot/default.html

- Add boilerplate launch page

## Add a semi-customized adapter

```csharp
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Logging;

public class AdapterWithErrorHandler : BotFrameworkHttpAdapter
{
    public AdapterWithErrorHandler(
        ICredentialProvider credentialProvider,
        ILogger<BotFrameworkHttpAdapter> logger)
        : base(credentialProvider)
    {
        // Enable logging at the adapter level using OnTurnError.
        OnTurnError = async (turnContext, exception) =>
        {
            await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");

            logger.LogError(
                $"{exception.GetType().Name} encountered:\n" +
                $"{exception.Message}\n" +
                $"{exception.StackTrace}");
        };
    }
}
```

## Bots/MyBot

Create the `MyBot` file and derive the class from `ActivityHandler`

- Add a constructor and local properties if you want to access objects via dependency injection.
- Generate an override handlers for the activities you want to...handle, such as `OnMessageActivity`.

```csharp
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

public class MyBot : ActivityHandler
{
    // DI associated code.

    protected override async Task OnMessageActivityAsync(
        ITurnContext<IMessageActivity> turnContext,
        CancellationToken cancellationToken)
    {
        // ...
    }
}
```

## Startup

- Add the current boilerplate to `ConfigureServices`: register bot and adapter.
- Add `.UseDefaultFiles().UseStaticFiles().UseHttpsRedirection()` to the `app` in `Configure`.
  - Not entirely sure if UseHttpsRedirection is necessary.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

    // Create the credential provider to be used with the Bot Framework Adapter.
    services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();

    // Create the Bot Framework Adapter with error handling enabled. 
    services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

    // Create the bot as a transient.
    services.AddTransient<IBot>(sp => new MyBot());
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseHsts();
    }

    app
        .UseDefaultFiles()
        .UseStaticFiles()
        .UseHttpsRedirection()
        .UseMvc();
}
```

## Controllers

In the Controllers directory:

- Rename controller to BotController.
- Replace its guts with boilerplate: constructor, properties, POST handler.
- Change the Route attribute value to something that makes sense for the bot. The default should be "api/messages"?

```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;

[ApiController]
[Route("api/[controller]")]
public class BotController : ControllerBase
{
    private IBotFrameworkHttpAdapter _adapter;
    private IBot _bot;

    public BotController(IBotFrameworkHttpAdapter adapter, IBot bot)
    {
        _adapter = adapter;
        _bot = bot;
    }

    [HttpPost]
    public async Task PostAsync()
    {
        await _adapter.ProcessAsync(Request, Response, _bot);
    }
}
```

> IMPORTANT: A Route attribute of "api/[controller]" means that ASP.NET looks for an appropriate class named \<controller>Controller. So, if your controller is `BotController`, your endpoint will be `api/bot`.

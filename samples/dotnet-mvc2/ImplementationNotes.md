# Implementation notes

Create a new project: ASP.NET Core Web Application
- .NET Core, ASP.NET Core 2.1, API, no authentication.
- De-select **Configure for HTTPS**.

## NuGet/MyGet

- Add Microsoft.Bot.Builder.Integration.AspNet.Core 4.3.0

## Update Properties/launchSettings.json

- Change `iisSettings/iisExpress/applicationUrl` to use port 5000.
- Make sure `iisSettings/iisExpress/sslPort` is set to 0.
- Remove `profiles/IIS Express/launchUrl`.
- Remove `profiles/<proj-name>/launchUrl`.

## Add wwwroot/default.html

-Add boilerplate launch page

## Bots/MyBot

Create the `MyBot` file and derive the class from `ActivityHandler`

- Generate an override handlers for the activities you want to...handle, such as `OnMessageActivity`.

```csharp
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

public class MyBot : ActivityHandler
{
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

    // Add the Adapter as a singleton and our Bot as a transient.
    services.AddSingleton<IBotFrameworkHttpAdapter>(sp =>
        new BotFrameworkHttpAdapter
        {
            // Code to run when the adapter catches an otherwise unhandled exception.
            OnTurnError = async (turnContext, exception) =>
            {
                await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");

                // When running the app from VS, Console.Error routes to the ASP.NET Core Web Server output window.
                Console.Error.WriteLine($"{exception.GetType().Name} encountered:");
                Console.Error.WriteLine(exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
            }
        }
    );
    services.AddTransient<IBot>(sp => new MyBot());
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
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

> ![IMPORTANT]
> A Route attribute of "api/[controller]" means that ASP.NET looks for an appropriate class named \<controller>Controller. So, if your controller is `BotController`, your endpoint will be `api/bot`.

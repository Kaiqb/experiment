// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace RichMediaV2
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
    }
}

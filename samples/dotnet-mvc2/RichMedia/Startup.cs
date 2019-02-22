using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RichMedia
{
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

            // Add the Adapter as a singleton and our Bot as transient.
            services.AddSingleton<IBotFrameworkHttpAdapter>(sp =>
                new BotFrameworkHttpAdapter
                {
                    // Code to run when the adapter catches an othwise unhandled exception.
                    OnTurnError = async (turnContext, excepption) =>
                    {
                        await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");

                        Console.Error.WriteLine($"{excepption.GetType().Name} encountered:");
                        Console.Error.WriteLine(excepption.Message);
                        Console.Error.WriteLine(excepption.StackTrace);
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
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

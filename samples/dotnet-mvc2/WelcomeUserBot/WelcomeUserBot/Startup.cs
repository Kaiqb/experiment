// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using WelcomeUserBot.Bots;

namespace WelcomeUserBot
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
            services.AddSingleton<IBotFrameworkHttpAdapter>(sp => new BotFrameworkHttpAdapter());

            // Create conversation and user state with in-memory storage provider.
            IStorage storage = new MemoryStorage();
            // ConversationState conversationState = new ConversationState(storage);
            var userState = new UserState(storage);

            // Create the custom state accessor.
            // State accessors enable other components to read and write individual properties of state.
            var accessors = new WelcomeUserStateAccessors(userState)
            {
                WelcomeUserState = userState.CreateProperty<WelcomeUserState>(WelcomeUserStateAccessors.WelcomeUserName),
            };

            // ??? This fails either way ???
            services.AddTransient<IBot>(sp => new MyBot(accessors));
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

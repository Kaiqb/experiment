// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace dotnet_mvc2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;

    public class MyBot : ActivityHandler
    {
        // Add DI code here, for properties and constructor, as desired.

        /// <summary>Handle an incoming message activity from the user.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("Hello world");
        }
    }
}
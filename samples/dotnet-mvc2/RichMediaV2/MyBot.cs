// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace RichMediaV2
{
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
            await turnContext.SendActivityAsync("Hello world", cancellationToken: cancellationToken);
        }
    }
}

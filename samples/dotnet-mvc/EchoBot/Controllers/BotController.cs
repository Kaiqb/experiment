﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace EchoBot.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;

    /// <summary>Defines a controller for this route.</summary>
    [Route("api/messages")]
    public class BotController : Controller
    {
        /// <summary>The serializer to use for request and response payloads.</summary>
        public static readonly JsonSerializer BotMessageSerializer
            = JsonSerializer.Create(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = new ReadOnlyJsonContractResolver(),
                Converters = new List<JsonConverter> { new Iso8601TimeSpanConverter() },
            });

        /// <summary>Handles incoming POST requests.</summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [HttpPost]
        public async Task PostAsync()
        {
            // Get the activity from the HTTP request.
            Activity activity = default(Activity);
            using (JsonTextReader bodyReader
                = new JsonTextReader(new StreamReader(Request.Body, Encoding.UTF8)))
            {
                activity = BotMessageSerializer.Deserialize<Activity>(bodyReader);
            }

            // Create an adapter.
            var credentialProvider = new SimpleCredentialProvider();
            var botFrameworkAdapter = new BotFrameworkAdapter(credentialProvider);
            botFrameworkAdapter.OnTurnError = async (turnContext, excepption) =>
            {
                // Code to run when the adapter catches an othwise unhandled exception.
                await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");

                Console.Error.WriteLine($"{excepption.GetType().Name} encountered:");
                Console.Error.WriteLine(excepption.Message);
                Console.Error.WriteLine(excepption.StackTrace);
            };

            // Use the adapter to authenticate and forward incoming activities.
            InvokeResponse invokeResponse = await botFrameworkAdapter.ProcessActivityAsync(
                Request.Headers["Authorization"],
                activity,
                OnTurnAsync,
                default(CancellationToken));
            if (invokeResponse == null)
            {
                // For non-invoke activities, acknowledge the activity.
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                // For invoke activities, include a response payload.
                Response.ContentType = "application/json";
                Response.StatusCode = invokeResponse.Status;
                using (StreamWriter writer = new StreamWriter(Response.Body))
                using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                {
                    BotMessageSerializer.Serialize(jsonWriter, invokeResponse.Body);
                }
            }
        }

        /// <summary>Defines the turn handler for the bot.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Type is ActivityTypes.Message)
            {
                // On a message activity, echo back the user's input.
                await turnContext.SendActivityAsync($"You said, '{turnContext.Activity.Text}'.");
            }
            else
            {
                // Otherwise, note the type of activity received.
                await turnContext.SendActivityAsync($"Received a `{turnContext.Activity.Type}` activity.");
            }
        }
    }
}

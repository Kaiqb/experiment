// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace RichMedia.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

        /// <summary>Map of user options to functions for sending the associated card type.</summary>
        private static Dictionary<string, Func<ITurnContext, CancellationToken, Task>> ShowCardAsync
            => new Dictionary<string, Func<ITurnContext, CancellationToken, Task>>
            {
                ["Hero Card"] = ShowHeroCard,
                ["Thumbnail Card"] = ShowThumbnailCard,
                ["Animation Card"] = ShowAnimationCard,
                ["Audio Card"] = ShowAudioCard,
                ["Video Card"] = ShowVideoCard,
                ["Adaptive Card"] = ShowAdaptiveCard,
                ["Receipt Card"] = ShowReceiptCard,
                ["Sign-in Card"] = ShowSigninCard,
            };

        /// <summary>Sends a stock hero card.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private static async Task ShowHeroCard(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var activity = MessageFactory.Attachment(HeroCard.ToAttachment());
            await turnContext.SendActivityAsync(activity, cancellationToken);
        }

        /// <summary>A stock hero card.</summary>
        private static HeroCard HeroCard => new HeroCard
        {
            Title = "BotFramework Hero Card",
            Subtitle = "Microsoft Bot Framework",
            Text = "Build and connect intelligent bots to interact with your users naturally wherever they are," +
                       " from text/sms to Skype, Slack, Office 365 mail and other popular services.",
            Images = new List<CardImage>
            {
                new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg"),
            },
            Buttons = new List<CardAction>
            {
                new CardAction(ActionTypes.OpenUrl, "Get Started", value: "https://docs.microsoft.com/bot-framework"),
            },
        };

        /// <summary>Sends a stock thumbnail card.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private static async Task ShowThumbnailCard(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <summary>Sends a stock animation card.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private static async Task ShowAnimationCard(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <summary>Sends a stock audio card.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private static async Task ShowAudioCard(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <summary>Sends a stock video card.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private static async Task ShowVideoCard(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <summary>Sends a stock Adaptive card.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private static async Task ShowAdaptiveCard(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <summary>Sends a stock receipt card.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private static async Task ShowReceiptCard(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <summary>Sends a stock sign-in card.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private static async Task ShowSigninCard(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        private static IActivity defaultMessage =>
            MessageFactory.SuggestedActions(ShowCardAsync.Keys, "Please choose the type of card to display.");

        /// <summary>Handles incoming POST requests.</summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [HttpPost]
        public async Task PostAsync()
        {
            // Get the activity from the HTTP request.
            var activity = default(Activity);
            using (var bodyReader
                = new JsonTextReader(new StreamReader(Request.Body, Encoding.UTF8)))
            {
                activity = BotMessageSerializer.Deserialize<Activity>(bodyReader);
            }

            // Create an adapter.
            var credentialProvider = new SimpleCredentialProvider();
            var botFrameworkAdapter = new BotFrameworkAdapter(credentialProvider)
            {
                OnTurnError = async (turnContext, excepption) =>
                {
                    // Code to run when the adapter catches an othwise unhandled exception.
                    await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");

                    Console.Error.WriteLine($"{excepption.GetType().Name} encountered:");
                    Console.Error.WriteLine(excepption.Message);
                    Console.Error.WriteLine(excepption.StackTrace);
                }
            };

            // Use the adapter to authenticate and forward incoming activities.
            var invokeResponse = await botFrameworkAdapter.ProcessActivityAsync(
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
                using (var writer = new StreamWriter(Response.Body))
                using (var jsonWriter = new JsonTextWriter(writer))
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
                // On a message activity display the type of card they asked for.
                foreach(var cardType in ShowCardAsync.Keys)
                {
                    if (turnContext.Activity.Text.Equals(cardType, StringComparison.InvariantCultureIgnoreCase))
                    {
                        await ShowCardAsync[cardType](turnContext, cancellationToken);
                        return;
                    }
                }

                // If we didn't understand their input, send suggested actions.
                await SendDefaultMessage(turnContext, cancellationToken);
            }
            else if (turnContext.Activity.Type is ActivityTypes.ConversationUpdate)
            {
                // Trigger the welcome message when new users are are added to the conversation.
                var activity = turnContext.Activity.AsConversationUpdateActivity();
                var newUsers = activity.MembersAdded.Where(member => member.Id != activity.Recipient.Id);
                if (newUsers.Count() > 0)
                {
                    await SendWelcomeMessage(turnContext, newUsers);
                }
            }
            else
            {
                // Otherwise, note the type of activity received.
                await turnContext.SendActivityAsync($"Received a `{turnContext.Activity.Type}` activity.");
            }
        }

        private async Task SendWelcomeMessage(
            ITurnContext turnContext,
            IEnumerable<ChannelAccount> newUsers = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (newUsers != null)
            {
                await turnContext.SendActivityAsync(
                    "New users have joined the conversation.",
                    cancellationToken: cancellationToken);
            }
            await SendDefaultMessage(turnContext, cancellationToken);
        }

        private async Task SendDefaultMessage(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await turnContext.SendActivityAsync(
                defaultMessage,
                cancellationToken: cancellationToken);
        }
    }
}

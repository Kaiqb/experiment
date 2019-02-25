// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace RichMediaV2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;

    public class MyBot : ActivityHandler
    {
        /// <summary>Map of user options to sample attachments for each type of card.</summary>
        /// <seealso cref="https://github.com/Microsoft/BotBuilder/blob/master/specs/botframework-activity/botframework-cards.md"/>
        /// <seealso cref="https://docs.microsoft.com/adaptive-cards/"/>
        private static Dictionary<string, Attachment> ShowCardAsync
            => new Dictionary<string, Attachment>
            {
                ["Adaptive Card"] = Attachments.SampleAdaptiveCardAttachment,
                ["Hero Card"] = Attachments.SampleHeroCard,
                ["Thumbnail Card"] = Attachments.SampleThumbnailCard,
                ["Animation Card"] = Attachments.SampleAnimationCard,
                ["Audio Card"] = Attachments.SampleAudioCard,
                ["Video Card"] = Attachments.SampleVideoCard,
                ["Receipt Card"] = Attachments.SampleReceiptCard,
                ["Sign-in Card"] = Attachments.SampleSigninCard,
            };

        /// <summary>A message that contains suggested actions for the available card types to send.</summary>
        /// <remarks>Defines to key elements of the activity.</remarks>
        private static readonly IActivity prompt1 = new Activity
        {
            Type = ActivityTypes.Message,
            Text = "Please choose the type of card to display.",
            SuggestedActions = new SuggestedActions
            {
                Actions = ShowCardAsync.Keys.Select(key =>
                    new CardAction
                    {
                        Value = key,
                        Title = key,
                        Type = ActionTypes.ImBack,
                    }
                ).ToList(),
            }
        };

        /// <summary>A message that contains suggested actions for the available card types to send.</summary>
        /// <remarks>Uses the MessageFactory to create the activity.</remarks>
        private static readonly IActivity prompt2 = MessageFactory.SuggestedActions(
                text: "Please choose the type of card to display.",
                actions: ShowCardAsync.Keys
            );

        private static readonly IActivity prompt = prompt2;

        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            // Display the type of card they asked for.
            var cardType = ShowCardAsync.Keys.FirstOrDefault(
                key => key.Equals(
                    turnContext.Activity.Text, StringComparison.InvariantCultureIgnoreCase));
            if (cardType != null)
            {
                await turnContext.SendActivityAsync(
                    MessageFactory.Attachment(ShowCardAsync[cardType]),
                    cancellationToken);
            }

            // Resend suggested actions, whether or not we understood their input.
            await turnContext.SendActivityAsync(prompt, cancellationToken: cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded,
            ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {
            if (membersAdded.Any(member => !member.Id.Equals(turnContext.Activity.Recipient.Id)))
            {
                // Send suggested actions.
                await turnContext.SendActivityAsync(prompt, cancellationToken: cancellationToken);
            }
        }
    }
}

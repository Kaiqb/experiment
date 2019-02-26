﻿// Copyright (c) Microsoft Corporation. All rights reserved.
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

    public class AttachmentsBot : ActivityHandler
    {
        /// <summary>Map of user options to sample attachments for each type of card.</summary>
        /// <seealso cref="https://github.com/Microsoft/BotBuilder/blob/master/specs/botframework-activity/botframework-cards.md"/>
        /// <seealso cref="https://docs.microsoft.com/adaptive-cards/"/>
        private static Dictionary<string, Attachment> AttachmentMap
            => new Dictionary<string, Attachment>
            {
                ["Inline Attachment"] = Attachments.InlineAttachment,
                ["Internet Attachment"] = Attachments.InternetAttachment,
                ["Hero Card"] = Attachments.SampleHeroCard,
                ["Thumbnail Card"] = Attachments.SampleThumbnailCard,
                ["Animation Card"] = Attachments.SampleAnimationCard,
                ["Audio Card"] = Attachments.SampleAudioCard,
                ["Video Card"] = Attachments.SampleVideoCard,
                ["Receipt Card"] = Attachments.SampleReceiptCard,
                ["Sign-in Card"] = Attachments.SampleSigninCard,
                ["Adaptive Card"] = Attachments.SampleAdaptiveCardAttachment,
                ["Carousel"] = null,
            };

        /// <summary>A message that contains suggested actions for the available card types to send.</summary>
        /// <remarks>Defines to key elements of the activity.</remarks>
        private static readonly IActivity prompt1 = new Activity
        {
            Type = ActivityTypes.Message,
            Text = "Please choose the type of card to display.",
            SuggestedActions = new SuggestedActions
            {
                Actions = AttachmentMap.Keys.Select(key =>
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
                actions: AttachmentMap.Keys,
                text: "Please choose the type of card to display.");

        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            // Determine the type of card asked for.
            var cardType = AttachmentMap.Keys.FirstOrDefault(key =>
                key.Equals(turnContext.Activity.Text, StringComparison.InvariantCultureIgnoreCase));

            if (cardType != null)
            {
                if (cardType is "Carousel")
                {
                    // Create and send a carousel of attachments.
                    var activity = MessageFactory.Carousel(new Attachment[]
                    {
                        Attachments.InlineAttachment,
                        Attachments.SampleReceiptCard,
                        Attachments.SampleSigninCard,
                    });
                    await turnContext.SendActivityAsync(activity, cancellationToken: cancellationToken);
                }
                else
                {
                    // Everything else in the map is an attachment.
                    await turnContext.SendActivityAsync(
                        MessageFactory.Attachment(AttachmentMap[cardType]),
                        cancellationToken);
                }
            }

            // Resend suggested actions, whether or not we understood their input.
            await turnContext.SendActivityAsync(prompt2, cancellationToken: cancellationToken);
        }
    }
}

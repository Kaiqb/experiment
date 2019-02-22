using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RichMedia
{
    public class MyBot : ActivityHandler
    {
        /// <summary>Map of user options to sample attachments for each type of card.</summary>
        /// <seealso cref="https://docs.microsoft.com/adaptive-cards/"/>
        /// <seealso cref="https://github.com/Microsoft/BotBuilder/blob/master/specs/botframework-activity/botframework-cards.md"/>
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
        private static IActivity suggestedActions =>
            MessageFactory.SuggestedActions(
                ShowCardAsync.Keys,
                "Please choose the type of card to display.");

        // Handles message activities from a user.
        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            // Display the type of card they asked for.
            foreach (var cardType in ShowCardAsync.Keys)
            {
                if (turnContext.Activity.Text.Equals(
                    cardType, StringComparison.InvariantCultureIgnoreCase))
                {
                    await turnContext.SendActivityAsync(
                        MessageFactory.Attachment(ShowCardAsync[cardType]),
                        cancellationToken);
                    break;
                }
            }


            // Resend suggested actions, whether or not we understood their input.
            await turnContext.SendActivityAsync(
                suggestedActions,
                cancellationToken: cancellationToken);
        }

        // Handles one or more members being added to the conversation.
        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded,
            ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {
            if (membersAdded.Any(member=>
                member.Id != turnContext.Activity.Recipient.Id))
            {
                await turnContext.SendActivityAsync(
                    suggestedActions,
                    cancellationToken: cancellationToken);
            }
        }
    }
}


using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace WelcomeUserBot.Bots
{
    public class MyBot : ActivityHandler
    {
        // Messages sent to the user.
        private const string WelcomeMessage = @"This is a simple Welcome Bot sample.This bot will introduce you
                                                to welcoming and greeting users.";

        private const string InfoMessage = @"You are seeing this message because the bot received at least one
                                            'OnMembersAddedAsync' event, indicating you (and possibly others)
                                            joined the conversation. If you are using the emulator, press the
                                            'Start Over' button to trigger this event again or type any input to continue.";

        private const string PatternMessage = @"This is also a good time to send a more general message to your user,
                                              explaining what your bot can do. In this example, the bot handles the
                                              key words 'help', 'intro', and 'exit' or simply echoes back your input.
                                              To continue, try typing 'intro'.";

        // The bot state accessor object. Use this to access specific state properties.
        private readonly WelcomeUserStateAccessors _welcomeUserStateAccessors;
        /// <summary>
        /// Initializes a new instance of the <see cref="WelcomeUserBot"/> class.
        /// </summary>
        /// <param name="statePropertyAccessor"> Bot state accessor object.</param>
        public MyBot(WelcomeUserStateAccessors statePropertyAccessor)
        {
            _welcomeUserStateAccessors = statePropertyAccessor ?? throw new System.ArgumentNullException("state accessor can't be null");
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // use state accessor to extract the didBotWelcomeUser flag
            var didBotWelcomeUser = await _welcomeUserStateAccessors.WelcomeUserState.GetAsync(turnContext, () => new WelcomeUserState());
            // the channel should send the user name in the 'From' object
            var userName = turnContext.Activity.From.Name;
            // Is this the user's first visit?
            if (didBotWelcomeUser.DidBotWelcomeUser == false)
            {
                didBotWelcomeUser.DidBotWelcomeUser = true;
                // Update user state flag to reflect bot handled first user interaction.
                await _welcomeUserStateAccessors.WelcomeUserState.SetAsync(turnContext, didBotWelcomeUser);
                await _welcomeUserStateAccessors.UserState.SaveChangesAsync(turnContext);

                await turnContext.SendActivityAsync($"You will only see this message the very first time you send an input to this bot.", cancellationToken: cancellationToken);
                await turnContext.SendActivityAsync($"This is a good time to welcome your new user and provide a personalized greeting. For example, 'Welcome {userName}!'", cancellationToken: cancellationToken);
                await turnContext.SendActivityAsync(PatternMessage, cancellationToken: cancellationToken);             
            }
            else
            {
                var text = turnContext.Activity.Text.ToLowerInvariant();
                switch (text)
                {
                    case "intro":
                    case "help":
                        await SendIntroCardAsync(turnContext, cancellationToken);
                        break;
                    case "bye":
                    case "goodbye":
                    case "exit":
                        await turnContext.SendActivityAsync($"Goodbye {userName}.", cancellationToken: cancellationToken);
                        break;
                    default:
                        await turnContext.SendActivityAsync($"You said {text}.", cancellationToken: cancellationToken);
                        await turnContext.SendActivityAsync($"You may also enter 'help', 'intro', or 'exit'.", cancellationToken: cancellationToken);
                        break;
                }
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                await turnContext.SendActivityAsync($"Hi there {member.Name}. {WelcomeMessage}", cancellationToken: cancellationToken);
                await turnContext.SendActivityAsync(InfoMessage, cancellationToken: cancellationToken);
            }
        }

        private static async Task SendIntroCardAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var response = turnContext.Activity.CreateReply();

            var card = new HeroCard();
            card.Title = "Welcome to Bot Framework!";
            card.Text = @"This is the Welcome Users bot sample! This Introduction card
                         is a great way to direct your users to information that can
                         help them get started. Here's some useful resources for
                         learning more about creating and deploying bots.";
            card.Images = new List<CardImage>() { new CardImage("https://aka.ms/bf-welcome-card-image") };
            card.Buttons = new List<CardAction>()
            {
                new CardAction(ActionTypes.OpenUrl, "Get an overview", null, "Get an overview", "Get an overview", "https://docs.microsoft.com/en-us/azure/bot-service/?view=azure-bot-service-4.0"),
                new CardAction(ActionTypes.OpenUrl, "Ask a question", null, "Ask a question", "Ask a question", "https://stackoverflow.com/questions/tagged/botframework"),
                new CardAction(ActionTypes.OpenUrl, "Learn how to deploy", null, "Learn how to deploy", "Learn how to deploy", "https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-deploy-azure?view=azure-bot-service-4.0"),
            };

            response.Attachments = new List<Attachment>() { card.ToAttachment() };
            await turnContext.SendActivityAsync(response, cancellationToken);
        }
    }
}


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
                                            joined the conversation. If you are using the emulator, press
                                            the 'Start Over' button to trigger this event again.";

        private const string PatternMessage = @"It is a good pattern to use this event to send general greeting
                                              to your user, explaining what your bot can do. In this example, the bot
                                              handles 'hello', 'hi', 'help' and 'intro. Try it now, type 'hi'";


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
            if (didBotWelcomeUser.DidBotWelcomeUser == false)
            {
                didBotWelcomeUser.DidBotWelcomeUser = true;
                // Update user state flag to reflect bot handled first user interaction.
                await _welcomeUserStateAccessors.WelcomeUserState.SetAsync(turnContext, didBotWelcomeUser);
                await _welcomeUserStateAccessors.UserState.SaveChangesAsync(turnContext);

                // the channel should send the user name in the 'From' object
                var userName = turnContext.Activity.From.Name;

                await turnContext.SendActivityAsync($"You are seeing this message because this was your first message ever to this bot.", cancellationToken: cancellationToken);
                await turnContext.SendActivityAsync($"It is a good practice to welcome the user and provide personal greeting. For example, welcome {userName}.", cancellationToken: cancellationToken);
            }
            else
            {
                var text = turnContext.Activity.Text.ToLowerInvariant();
                await turnContext.SendActivityAsync($"You said {text}.", cancellationToken: cancellationToken);
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                
                await turnContext.SendActivityAsync($"Hi there - {member.Name}. {WelcomeMessage}", cancellationToken: cancellationToken);
                await turnContext.SendActivityAsync(InfoMessage, cancellationToken: cancellationToken);
                await turnContext.SendActivityAsync(PatternMessage, cancellationToken: cancellationToken);
            }
        }
    }
}

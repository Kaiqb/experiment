// Import required Bot Framework classes.
const { CardFactory } = require('botbuilder');
const { ActivityHandler} = require('botbuilder-core');
// Bot Card content
const IntroCard = require('./resources/IntroCard.json');

// Welcomed User property name
const WELCOMED_USER = 'welcomedUserProperty';

// this.welcomedUserProperty = userState.createProperty(WELCOMED_USER);
// this.userState = userState;

class WelcomeBot extends ActivityHandler{
     /**
     *
     * @param {UserState} User state to persist boolean flag to indicate
     *                    if the bot had already welcomed the user
     */
    constructor(userState) {
        super();

        // Creates a new user property accessor.
        // See https://aka.ms/about-bot-state-accessors to learn more about the bot state and state accessors.
        this.welcomedUserProperty = userState.createProperty(WELCOMED_USER);
        this.userState = userState;

        this.onMessage(async (turnContext, next) => {
            // debugger;
            const didBotWelcomedUser = await this.welcomedUserProperty.get(turnContext, false);
            // Your bot should proactively send a welcome message to a personal chat the first time
            // (and only the first time) a user initiates a personal chat with your bot.
            if (didBotWelcomedUser === false) {
                // The channel should send the user name in the 'From' object
                let userName = turnContext.activity.from.name;
                await turnContext.sendActivity('You are seeing this message because this was your first message ever sent to this bot.');
                await turnContext.sendActivity(`It is a good practice to welcome the user and provide a personal greeting. For example, welcome ${ userName }.`);
                await turnContext.sendActivity("This is also a good time to send a more general message to your user explaining what your bot can do. In this example," +
                                           "the bot handles the key words 'help', 'intro', and 'exit' or simply echoes back your input. " +
                                           "To continue, try typing 'intro'.");
    
                // Set the flag indicating the bot handled the user's first message.
                await this. welcomedUserProperty.set(turnContext, true);
                // Save state change
                await this.userState.saveChanges(turnContext);
            }
            else{
               // This example uses an exact match on user's input utterance.
                // Consider using LUIS or QnA for Natural Language Processing.
                let text = turnContext.activity.text.toLowerCase();
                switch (text) {
                    case 'help':
                    case 'intro':
                        await turnContext.sendActivity({
                           text: 'Intro Adaptive Card',
                            attachments: [CardFactory.adaptiveCard(IntroCard)]
                        });
                        break;
                    case 'bye':
                    case 'cancel':
                    case 'exit':
                    case 'goodbye':
                        await turnContext.sendActivity(`Goodbye!`);
                        break;
                    default :
                       await turnContext.sendActivity(`You said "${ turnContext.activity.text }".`);
                        await turnContext.sendActivity(`You may also enter 'help', 'intro', or 'exit'.`);
                        break;
                }
            } 
            // then `await next()` to continue processing
            await next();
        });
    
        this.onMembersAdded(async (turnContext, next) => {
            // debugger;
            // Do we have any new members added to the conversation?
            if (turnContext.activity.membersAdded.length !== 0) {
                // Iterate over all new members added to the conversation
                for (let member in turnContext.activity.membersAdded) {
                    if (turnContext.activity.membersAdded[member].id !== turnContext.activity.recipient.id) {
                        // Greet anyone that was not the target (recipient) of this message.
                        // Since the bot is the recipient for events from the channel,
                        // context.activity.membersAdded === context.activity.recipient.Id indicates the
                        // bot was added to the conversation, and the opposite indicates this is a user.
                       await turnContext.sendActivity("Welcome to the 'Welcome User' Bot. This bot will introduce you to welcoming and greeting users.");
                       await turnContext.sendActivity("You are seeing this message because the bot received at least one 'OnMembersAdded'" +
                                                'event,indicating you (and possibly others) joined the conversation. If you are using the emulator, ' +
                                                "press the 'Start Over' button to trigger this event again " +
                                                "or type and send any input to continue.");
                    }
                }
            } 
            await next();
        });
    } // end constructor

}  // end class.

module.exports.WelcomeBot = WelcomeBot;

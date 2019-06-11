# Run a simple bot
In this module, you'll learn how to process incoming message event to access and use the text of the user's message. We'll go over the concepts of an "activity" and how to appropriately respond to the incoming activities from the user.

## Simple Welcome for User
* OnMembersAddedAsync (C#) or onMembersAdded (JS)
  - 1 event per new user joining the conversation.
  - sendActivity displays a simple welcome message to the user.

## Find user input
* What is turn context?
  - what information does it contain?
  - how do I access and use this information?
  - message activities
  - text

* OnMessageActivityAsync (C#) or onMessage (JS)
  - creates turnContext.
  - find user input with turnContext.activity.text.
  
 ## Process user input
 * Process anticipated user response
   - switch statement for multiple cases.
 * Process unanticipated user reponse
   - pass to help or prompt function.
 * Process user exit response
   - how to cleanly end a conversation.
 
 ## Respond to user input
 * One turnContext.sendActivity per user input
   - response may provide answer or additional info request.
   - bots are stateless, each response considered independently.

Online documentation reference: [Send and receive text message](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-send-messages?view=azure-bot-service-4.0&tabs=csharp)

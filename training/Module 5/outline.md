# State and storage
A bot is inherently stateless. Once your bot is deployed, it may not run in the same process or on the same machine from one turn to the next. However, your bot may need to track the context of a conversation, so that it can manage its behavior and remember answers to previous questions. The state and storage features of the SDK allow you to add state to your bot.

## More state
* What is conversation state? 
  - where/when is it stored?
  - what can it store?
  - how is it accessed?

## Which state do I use for multiple turn conversations?
* What goes into user state?
* What goes into conversation state?

## Storage of state
* When to save state
  - persistence of user inputs.
  - persistence of user info.

## Types of storage - temporary and persistent
* Temporary storage - we will use this for our labs.
  - memory storage, how to create it.
  - memory storage, when to use it.
* Persistent storage to database
  - Cosmos DB, how to create it.
  - Cosmos DB, when to use it. 
  - Blob storage, how to create it.
  - Blob storage, when to use it.
  - Transcript storage, how to create it.
  - Transcript storage, when to use it.
  
 ## Download lab 2 sample code
 * Download BotBuilder sample "03.Welcome" for your language: [C3 Sample](https://aka.ms/bot-welcome-sample-cs), [JavaScript Sample](https://aka.ms/bot-welcome-sample-js)

# Lab 2, Persisting user information

In this lab we will begin with the downloaded welcome-user bot and explore how user information is acquired and persisted.

## Define your bot's services
Our lab sample code uses your computer's local memory for storage. This works well and is easy to configure for testing purposes. However, all stored information is lost whenever your bot is restarted. For a production bot, consider using CosmosDB or Blob storage to persist your bot's storage across multiple restarts. For now, let's look at how Memory storage is added as a service for your bot.
* C# - Locate and open the Startup.cs file.
  - Services are added to your C# bot within the ConfigureServices() method.
  - Within ConfigureServices() are two calls of interest to "services.AddSingleton" for MemoryStorage and UserState.
    - NOTE. UserState relies on there being some form of storage available. If MemoryStorage was not also defined your bot would fail to properly load and run.
  - Both of these services are defined within the library Microsoft.Bot.Builder and will be used by your running bot.
* JS - Locate and open the index.js file.
  - MemoryStorage and UserState are imported from the botbuilder librabry by the following call:
    - const { BotFrameworkAdapter, UserState, MemoryStorage } = require('botbuilder');
  - MemoryStorage and UserState are now defined and used by the following 4 calls:
    - let userState;  // creates a global variable for handling user state.
    - const memoryStorage = new MemoryStorage();  // creates a constant of type MemoryStorage().
    - userState = new UserState(memoryStorage);  // defines variable userState as type UserState() built using memoryStorage.
    - const bot = new WelcomeBot(userState);  // creates your bot's main dialog and passes in the newly defined userState.
  
## Connect and access userState.
Once userState has been defined for your bot, we now need to connect this service and add whatever user properties you want to persist. The welcome-user provides a simple example of persisting user state. The only thing saved is a boolean value, _DidBotWelcomeUser_ that tells us whether this user has perviously received our initial welcome message(s). But, this same logic and storage could be used to ask for information such as the user's name, age, address, etc and persist these values as well.
* C# - Locate and open file WelcomeUserState.cs
  - This is a C# class that defines a single property, DidBotWelcomeUser. Additional properties could be added here to this class.
    - public bool DidBotWelcomeUser { get; set; } = false;
  - Locate and open file WelcomeUserBot.cs found inside the 'Bots' folder.
  - Note in line 44 that a new instance of the WelcomeUserBot is created, public WelcomeUserBot(UserState userState) and the passed userState is associated with BotState \_userState: 
    - "_userState = userState;"  // use this as accessor for userState.
  - Whenever method OnMessageActivityAsync() is called, it accesses \_userState and stores the current state of _DidBotWelcomeUser_ into variable didBotWelcomeUser as follows:
    - var welcomeUserStateAccessor = _userState.CreateProperty<WelcomeUserState>(nameof(WelcomeUserState));  // create accessor.
    - var didBotWelcomeUser = await welcomeUserStateAccessor.GetAsync(turnContext, () => new WelcomeUserState()); // use accessor to obtain stored value.
* JS -


## process user input based on current userState
* If didBotWelcomeUser is 'false' (the original initialized value) then an initial welcome message is displayed to the user and this value is set to 'true'.
* If didBotWelcomeUser is 'true' (a persisted user value) then the user's input is processed using the bot's switch(text) statement.

## Persist your user's state
Once you have used userState to process your user's input, and have potentially set didBotWelcomeUser to 'true' the userState is saved to storage to use with the next user input. The following call stores any changes to the current userState:   
* C# - await \_userState.SaveChangesAsync(turnContext);  // uses BotState method SaveChangesAsync() to preserve state.
* JS - await this.userState.saveChanges(context);  // saves changes for defined userState.

## Run and debug your bot
Run this bot code and test it with your emulator in the same manner as you did for Lab 1. 
* It may be useful to add several breakpoints in your code and hover your cursor over _didUserWelcomeBot_. 
  - Notice how the value of _didUserWelcomeBot_ changes after the the initial welcome message is displayed.
  - Notice what happens to the value of _didUserWelcomeBot_ once your click "Restart conversation" in your emulator.
* Once again, you can also modify the messages displayed by your bot to make this more like your own voice.
  - Stop your bot, change any message you'd like, then restart and test.

## Some user inputs return a card.
Cards provide a useful and visual method of providing your users with both information and choices. They will be covered in detail during your next session.

 

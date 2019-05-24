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
  
## Connect and use userState.
Once userState has been defined for your bot, we now need to connect this service and add whatever user properties you want to persist. The welcome-user provides a simple example of persisting user state. The only thing saved is a boolean value, _DidBotWelcomeUser_ that tells us whether this user has perviously received our initial welcome message(s). But, this same logic and storage could be used to ask for information such as the user's name, age, address, etc and persist these values as well.
* C# - Locate and open file WelcomeUserState.cs
  - This is a C# class that defines a single property, DidBotWelcomeUser. Additional properties could be added here to this class.
    - public bool DidBotWelcomeUser { get; set; } = false;
  - Locate and open file WelcomeUserBot.cs found inside the 'Bots' folder.
  - Note in line 44 that a new instance of the WelcomeUserBot is created, public WelcomeUserBot(UserState userState) and the passed userState is associated with BotState \_userState: 
    - "_userState = userState;"  // use this as accessor for userState.
  - Whenever method OnMessageActivityAsync() is called, it accesses \_userState and stores the current state of _DidBotWelcomeUser_ into variable didBotWelcomeUser as follows:
    - var welcomeUserStateAccessor = _userState.CreateProperty<WelcomeUserState>(nameof(WelcomeUserState));  // create accessor.
    - var didBotWelcomeUser = await welcomeUserStateAccessor.GetAsync(turnContext, () => new WelcomeUserState()); // use accessor.
  
* JS - 

* Response to user input.
  - C# - 
  - JS - 

* run bot and catch break points for both events.
  - C# - 
  - JS - 


## Add a little 'Style' to your Echo Bot
* Personalize your bot's greeting. 
  - C# - 
  - JS -

* Add a 'tone' to your response bot's response.
  - C# - 
  - JS -

* rerun your bot and see these interactions in the emulator.

## Give users additional information
* customize your bot by adding an instructional 'what next' message following each user interaction.
  - C# - 
  - JS -

* rerun your bot and test for infromation with the emulator.

## More details to come
... 

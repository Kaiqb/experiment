# Lab 3, WeatherBot Cards
In this lab we will begin with by downloading a sample weatherBot that was created based on BotBuilder-Samples/.../06.using-cards.

> [!NOTE] You will need to use the OpenWeatherMap key you obtained by request at the beginning of Session 2 for this code to function properly.

* Download Lab3 source code from [here](../../Code/Lab3%20Cards%20MVC)   (or add different link)

This sample code uses a Waterfall dialog to interact with your user, find their city of choice selected from a fixed list of PromptOptions, queries OpenWeatherMap for current weather conditions, and returns an Adaptive Card showing weather information for the selected city's location. There's a lot going on in this code, so let's look at some of the key pieces.

## Add your OpenWeatherMap key to appsettings.json
* Locate and open file appsettings.json
* Add the key you obtained through your email account as follows:

```json
{
   "MicrosoftAppId": "",
  "MicrosoftAppPassword": "",
  "OpenWeatherMapKey": "<your-key-here>"
}
```
This key provides you with free online access to a limited set of OpenWeatherMap API calls.

## Looking at the Waterfall
* Locate and open file MainDialg.cs found inside of the 'Dialogs' folder.



  
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
* JS - Locate and open file welcomeBot.js
  - The following calls create an accessor for userState
    - this.welcomedUserProperty = userState.createProperty(WELCOMED_USER);  // defines property welcomedUserProperty of userState.
    - this.userState = userState;  // defines accessor for userState.
  - Now each time method onMessage() is called, it accesses userState and stores the current value of _welcomedUserProperty_ into variable didBotWelcomeUser with the following call:
    - const didBotWelcomedUser = await this.welcomedUserProperty.get(context, false);  // retrieve value into didBotWelcomeUser.
    - Note, if this is the first access of welcomedUserProperty it is intitialized to 'false'.

## Process user input based on current userState
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
Cards provide a useful and visual method of providing your users with both information and choices. 
* The method calls SendIntroCardAsync() (C#) and sendIntroCard() (JS) show how easy it is to create and display a card from within you bot code.

Cards will be covered in detail during our next session.



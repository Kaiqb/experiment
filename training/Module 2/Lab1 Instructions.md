# Lab 1: Basic Echo Bot functionality

The primary job of any bot is twofold: 
1. provide information when a new user connects.
2. Provide a response based on user input.
In this lab we wil find where each of these activities occurs, then add our own modifications to each process.


## Find your bot file 
* C# - Locate and open EcoBot.cs file inside of the 'Bots' folder.
* JS - Locate and open bot.js

## Set up Debugger for your bot.
* New user connection.
  - C# - Locate method _OnMembersAddedAsync_
       - Find line beginning with 'await turnContext.SendActivityAsync'.
       - click on area to the left of this line number to add a break point.
  - JS - Locate method _onMembersAdded_
       - Find line beginning with 'await context.sendActivity'.
       - click on area to the left of this line number to add a break point.

* Response to user input.
  - C# - Locate method _OnMessageActivityAsync_
       - Find line beginning with 'await turnContext.SendActivityAsync'.
       - click on area to the left of this line number to add a break point. 
  - JS - Locate method _onMessageActivity_
       - Find line beginning with 'await context.sendActivity'.
       - click on area to the left of this line number to add a break point.

* run your bot locally, interact with your emulator, and catch break points for both events.
  - When breakpoint is caught, you can hover over active elements to view their content. Right-click on an element to drill into that element's content.
  - Example, when user response breakpoint is caught, to see the user input captured by your bot hover over the value:
    - C# - 'Text' of turnContext.Activity.Text
    - JS - 'text' of context.activity.text

* To release captured breakpoint and continue, click green _continue_ arrow towards the top of your IDE screen.

## Add a little 'Style' to your Echo Bot
* Personalize your bot's greeting. 
  - C# - 
  - JS -

* Add a 'tone' to your response bot's response.
  - C# - 
  - JS -

* rerun your bot and see these interactions in the emulator.

## Provide users with additional information
* customize your bot by adding an instructional 'what next' message following each user interaction.
  - C# - 
  - JS -

* rerun your bot and test for infromation with the emulator.

## More details to come
This exercise started your exploration of bot code. Our next training module will walk through each of these major bot components in further detail. For now, feel free to add additional messaging to your Lab 1 bot to make it feel like your own voice. 

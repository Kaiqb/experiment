# Lab 5, Build weather bot with dispatch for multiple models

In this lab we will extend our use of _Azure Cognitive Services_ by creating both a natural Language Understanding (LUIS) and a Dispatch service that selects which service has the appropriate answer for your question. We will then connect these new services plus our QnA Maker service to this lab's DispatchWeatherBot code.

## Download code for Lab 5
* Access and save locally the lab5 sample code from here: Dispatch WeatherBot [C# Sample](https://github.com/Kaiqb/experiment/tree/master/training/Code/Lab5%20Dispatch)  (**add correct link when this is published**).
 
## Create a LUIS weather app in the LUIS portal
For this portion of our lab you can follow the steps detailed in the online documentation to [Add natural language understanding to your bot](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-v4-luis?view=azure-bot-service-4.0&tabs=csharp). 

* Summary of creation steps:
  - open and sign in to the [LUIS Portal](https://www.luis.ai/home). 
    - If you don't currently have a LUIS account, you can create one one here on this site.
  - from the MyApps page, click "^ Import new App".
  - click button "choose App File (JSON) format", browse on your drive to lab 5 code, select _/CognitiveModels/WeatherForecasts.json_
  - add an optional name for your new LUIS service, then click "Done."
  - close pop-up window (if present) and click red dotted button "Train" to train your new model.
    - "Train" button's dot should turn to green when complete.
  - click "Test" button and enter a question such as "Will it rain in Bellevue?"
    - Note - this question contains both a weather word (rain) and location word (Bellevue).
  - when LUIS provides the Top scoring intent (When_Condition) click small hot link "Inspect" on right.
    - click right arrows ">" to close these windows.
  - notice that LUIS returns not only your question's Top Intent but also entities for the _condition_ and _location_.
  - now select button "Publish" and "Production" to build and publish this new LUIS app.

Once publishing is complete, we can now gather information to connect our new LUIS app to our Lab 5 sample code.

## Gather LUIS connection information.
While still connected to your new service within the LUIS portal do the following.
* From left of Dashboard screen, record locally the value displayed for "Version:". Used as \<app-version-number> by Dispatch. 
  - this is typically "0.1" 
* Click the "Manage" tab located to the left of the "Train" button.
  - copy and save locally the GUID value shown for value "Application ID". Used as \<app-id-for-weather-app>.
  - copy and save locally the value shown for "Display name". Used as \<name-of-weather-app> by Dispatch.   
* From menu on left of screen, select "Keys and endpoints."
  - copy and save locally the value shown for "Authoring Key". Used as <your-luis-authoring-key>.
  - scroll down to the bottom of this page, copy and save locally the "Region" shown for your application. Used as <your-region>.
    - Example: this value usually contains "westus" or a similar Azure region.
  
You have now gathered all of the information necessary to connect your Lab 5 code to your new LUIS weather app. 

## Create a Dispatch service for your Lab 5 sample code
Now that you have both a QnA Maker weather app (created in Lab 4) and a new LUIS weather app it is now time to use the connection values gathered during the building of both of these apps to create a new Dispatch model. The step-by-step instructions for this can be found in the [Create the dispatch model](https://docs.microsoft.com/azure/bot-service/bot-builder-tutorial-dispatch?view=azure-bot-service-4.0&tabs=cs#create-the-dispatch-model) section of the online documentation referenced above, or follow this summary of key steps:
* Open a command prompt or terminal window, and change directories to the CognitiveModels directory within your locally saved copy of the lab 5 sample code..
* Run the following 2 commands to ensure you have current versions of npm and the Dispatch tool.
```cmd
npm i -g npm
npm i -g botdispatch
```
* Now use the _dispatch init_ command to create a .dispatch file for your dispatch model. Choose a \<filename-to-create> that you will remember.
```cmd
dispatch init -n <filename-to-create> --luisAuthoringKey "<your-luis-authoring-key>" --luisAuthoringRegion <your-region>
```
* Now add your LUIS weather app and QnA Maker knowledge base to the .dispatch file.
```cmd
dispatch add -t luis -i "<app-id-for-weather-app>" -n "<name-of-weather-app>" -v <app-version-number> -k "<your-luis-authoring-key>" --intentName l_Weather
dispatch add -t qna -i "<knowledge-base-id>" -n "<knowledge-base-name>" -k "<azure-qna-service-key1>" --intentName q_sample-qna
```
* Now use the _dispatch create_ command to generate a dispatch model using the .dispatch file you just created and populated.
```cmd
dispatch create
```
This command builds a .json file, then creates, trains, and publishes a LUIS-based dispatch app.
* Return to the LUIS portal and open your new Dispatch app to gather the LUIS connection information.
* Following the same steps detailed for your LUIS weather app above collect the following:
  - \<app-id-for-dispatch-app>
  - \<your-luis-authoring-key> - this should be the same as your weather app.
  - \<your-region> - this too should be the same as your weather app.
  
These values will be used in the next section to add connection information to your Lab 5 sample code.

## Add connection information to your code
We will now add the nescessary information you recorded from your QnA Maker knowledgebase into your code for Lab 5.
* Open the lab5 code you downloaded earlier from here [DispatchWeatherBot C# Sample](https://github.com/Kaiqb/experiment/tree/master/training/Code/Lab5%20Dispatch) (**add correct link when this is published**).
* Locate and open file appsettings.json
* Now add your saved connection values into this code:
  ``` JSON
  {  
     "MicrosoftAppId": "",
     "MicrosoftAppPassword": "",
 
     "DispatchLuisAppId": "<app-id-for-dispatch-app>",
     "DispatchLuisAPIKey": "<your-luis-authoring-key>",
     "DispatchLuisAPIHostName": "<your-region>",
     
     "QnAKnowledgebaseId": "<knowledge-base-id>",
     "QnAAuthKey": "<qna-maker-resource-key>",
     "QnAEndpointHostName": "<your-hostname>",
     
     "LuisAppId": "<app-id-for-weather-app>",
     "LuisAPIKey": "<your-luis-authoring-key>",
     "LuisAPIHostName": "<your-region>",

     "OpenWeatherMapKey": "<your-openweathermap-key-here>",

      "AllowedHosts": "*"
   }
   ```

Your code should now be able to run and connect to your QnA Maker knowledgebase.
  
## Explore the DispatchWeatherBot
There is quite a lot of code involved in Lab 5. For now, let's look at the main sections and logic flow.
* Locate and open file DispatchBot.cs found inside of the 'Bots' folder.
* Let's look at the main functionality of method OnMessageActivityAsync() called each time a user sends new input.
  - Observe that like previous labs, we once again create both a \_conversationState and \_userState to help control your bot's logic flow.
    - The state values used and saved can be examined within the two class files UserProfile.cs and ConversationData.cs
    - Note, Because we store the requested "Location" within a user's profile. Subsequent queries that do not include a location can still be run by using this user's Location information!
  - This program initially uses the call _botServices.Dispatch.RecognizeAsync()_ to pass each user input to your Dialog app and then retrieves the "TopIntent" from the recognizerResult.
    - Based on the user input Dispatch will return one of 2 intents "l_Weather" (LUIS weather) or "q_sample-qna" (QnA Maker sample).
  - After displaying the chosen intent to your user, the method _DispatchToTopIntentAsync()_ calls the appropriate method to contact the correct online service. 
  - _ProcessWeatherAsync()_ - connects to your LUIS weather app to see what type of question you have asked. Conditions detected include:
    - "Daily_Forecast", "Hourly_Forecast", "When_Condition", "When_Sun", and "User_Goodbye".
  - Control is then passed to the proper method(s) to process and display the OpenWeatherMap results, or say "goodbye".
  - _ProcessSampleQnAAsync()_ - simply connects to your QnA weather app and returns the rusults back to your user. 

## Run and debug your bot
Run this bot code and test it with your emulator in the same manner as you did for previous Labs.
* Emulator, OpenBot, http://localhost:3978.api.messages
* It may be useful to add several breakpoints at various locations within your code and hover your cursor over values of interest.

## Make this code your own
There are many ways to modify this code to help give it a bit of your own personality. Here are a few suggestions:
* Method _OnMessageActivityAsync()_ shows the top detected intent to your user. You can comment out this _SendActivityAsync()_ call to remove the return of that non-essential information.
* Method _FindHourlyForecast()_ by default displays (8) 3-hour time periods with each time period shown as 1 hour & 30 minutes past the beginning of the initial time block. You can adjust the values to change the number of time periods displayed or their starting times.
* Method _SignOutUser()_ displays a "goodbye" message to your user, you can modify that to add your own voice to your bot.



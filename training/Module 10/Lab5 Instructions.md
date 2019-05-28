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


## Run and debug your bot
Run this bot code and test it with your emulator in the same manner as you did for previous Labs.
* Emulator, OpenBot, http://localhost:3978.api.messages
* It may be useful to add several breakpoints in your code and hover your cursor over values of interest such as the _turnContext_ and _response_ to see how your data is being passed.

## Make this code your own



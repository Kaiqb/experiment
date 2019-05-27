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
* Click the "Manage" tab located to the left of the "Train" button.
  - copy and save locally the GUID value shown for value "Application ID".
* From menu on left of screen, select "Keys and endpoints."
  - copy and save locally the value shown for "Authoring Key". Used as <your-luis-authoring-key>.
  - scroll down to the bottom of this page, copy and save locally the "Region" shown for your application. Used as <your-region>.
    - Example: this value usually contains "westus" or a similar Azure region.
  
You have now gathered all of the information necessary to connect your Lab 5 code to your new LUIS weather app. You can now close your connection to the LUIS portal. 

## Create a Dispatch service for your Lab 5 sample code
Now that you have both a QnA Maker weather app (created in Lab 4) and a new LUIS weather app it is now time to use the connection values gathered during the building of both of these apps to create a new Dispatch model. The step-by-step instructions for this can be found in the "Create the dispatch model" section of the online documentation referenced above, or follow this summary of key steps:
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

Create this usinga filename you will recognize
  - An example is entering "Why does it rain?"
* Check that you knowledgebase returns an answer.
* Select _-> Test_ again to close the test pop-up screen.

## Publish your knowledgebase
* Select "Save and train" button to preserve your changes
* Now to the right of the "Edit" tab, find and select the "Publish" tab.
  - Click the _Publish_ button.
* Once published, scroll down, copy, and save locally all of the information presented within the *Postman* window.
  - some of this information will be used in the next steps to connect your code to this knowledgebase.
* You can now close your browser's QnA Maker tab (or keep it open to edit and play with your answers later).

## Add connection information to your code
We will now add the nescessary information you recorded from your QnA Maker knowledgebase into your code for lab4.
* Open the Postman information you saved locally and find the following values:
  - POST /knowledgebases/<*knowledge-base-id*>/generateAnswer
  - Host: <*your-hostname*> // This is the Full URL starting with https: and ending with /qnamaker
  - Authorization: EndpointKey <*your-endpoint-key*>
  
* Open the lab5 code you downloaded at the end of session 10, or download it now from here [DispatchWeatherBot C# Sample](https://github.com/Kaiqb/experiment/tree/master/training/Code/Lab5%20Dispatch) (**add correct link when this is published**).

* Locate and open file appsettings.json
* Now add your saved connection values into this code:
  ``` JSON
  {
     "MicrosoftAppId": "",
     "MicrosoftAppPassword": "",
  
     "QnA-sample-qna-kbId": "<knowledge-base-id>",
     "QnA-sample-qna-endpointKey": "<your-endpoint-key>",
     "QnA-sample-qna-hostname": "<your-hostname>"
   }
   ```
   ---
   
Your code should now be able to run and connect to your QnA Maker knowledgebase.
  
## Explore the QnAWeatherBot 
You will find that the code for this lab is quite simple. Most of the processing logic occurs within Azure and your QnA Maker knowledgebase.
* Locate and open file "QnAWeatherBot.cs" found inside of the "Bots" folder.
* Find method "OnMembersAddedAsync()" - this method is called whenever a new user connects to your bot.
  - Notice that this method Welcomes your user and explains how to interact with your bot.
* Now find method "OnMessageActivityAsync()" - this method is called each time a user sends a request to your bot.
  - Notice that a new connection to your QnA Maker knowledgebase (var qnaMaker) is created using the values you added in "appsetting.json" each time a new user request is received.
  - qnaMaker passes the user request information contained within _turnContext_ using method "GetAnswerAsync(turnContext)".
  - Your knowledgebase response, contained within (var response), is checked for _null_ and then passed back to your user.
These few actions cover the full logic flow for Lab 4's QnAWeatherBot code.

## Run and debug your bot
Run this bot code and test it with your emulator in the same manner as you did for previous Labs.
* Emulator, OpenBot, http://localhost:3978.api.messages
* It may be useful to add several breakpoints in your code and hover your cursor over values of interest such as the _turnContext_ and _response_ to see how your data is being passed.

## Make this code your own
Once this code is running successfully, you can modify both the available questions and your knowledgebase's response by opening your knowledgebase up within the QnA Maker portal and selecting the "Edit" tab. If you make changes be sure to:
* Test your knowledgebase response using the _<- Test_ button.
* Publish your new changes by selecting the "Publish" Tab and clicking the _Publish_ button.
Your changes will be saved, but no changes will be made to your bot connection values, so you can now run your bot again and see any updated responses. 



# Lab 4, Building the weather bot QnA knowledgebase

In this lab we will follow the steps to create a new QnA Maker Weather knowledgebase, then use our lab 4 code to send queries and process process the results from our new knowledgebase. 

## QnA Maker requires an Azure service URL
For this portion of our lab you can follow the steps detailed in the online documentation to [set up a QnA Maker service](https://docs.microsoft.com/en-us/azure/cognitive-services/qnamaker/how-to/set-up-qnamaker-service-azure). 

* Summary of creation steps:
  - Open and sign in to the [Azure Portal](https://portal.azure.com/signin/index).
  - select "+ Create a resource", search for "QnA Maker", and click "Create".
  - select a unique name for you QnA Maker service.
  - select create new resource group, or use an existing one you already have created.
  - pricing tier: choose F0 (free), searching "F" (free) if available.
  - search location "West US","West US 2", or close to where you are located.
  - App Insights - choose "Disable" to simplify this lab.
  - Select "Create"

Once deployment has successfully completed, you are done with this step.

## Create your knowledgebase.
To use the Azure service you just created, sign in the [Qna Maker portal](https://qnamaker.ai/) using the same Azure credentials you used to create your QnaMaker service.
* Select top Tab: "Create an knowledgebase"
* scroll down to Step 2 - "Connect your QnA service to your KB."
  - select your Azure Directory ID.
  - select your current subscription name.
  - select the Azure QnA service you just created.
* Scroll down to step 3 - "Name your KB."
  - Choose a name for your QnAWeatherBot knowledgebase.
* Scroll down to step 4 - "Populate your KB."
  - select "+ Add file".
  - On your local machine, locate and select the file "_Lab4WeatherBot-KB.tsv_ that you downloaded at the end of session 8.
  >!Note - you can if you want additionally add a "Chit-chat" voice that answers user inputs like "Hello."
* Scroll down to step 5 and click the button "Create your KB".

## Test your knowledgebase
Once deployed, QnA Maker opens up an interface for you within your new knowledgebase.
* In the upper right corner, click the _<- Test_ button.
* Enter a question that your knowledgebase should know:
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
* Open the lab4 code you downloaded at the end of session 8, or download it now from here [QnAWeatherBot C# Sample](https://github.com/Kaiqb/experiment/tree/master/training/Code/Lab4%20QnAWeatherBot) (**add correct link when this is published**).
* Now add your saved connection values into this code:
  - C# - open appsetting.json file and add the following:
  ``` JSON
  {
     "MicrosoftAppId": "",
     "MicrosoftAppPassword": "",
  
     "QnA-sample-qna-kbId": "<knowledge-base-id>",
     "QnA-sample-qna-endpointKey": "<your-endpoint-key>",
     "QnA-sample-qna-hostname": "<your-hostname>"
   }
   ```
   
   - JavaScript - open your .env file and add the following:
   ```file
   MicrosoftAppId=""
   MicrosoftAppPassword=""

   QnAKnowledgebaseId="<knowledge-base-id>"
   QnAAuthKey="<your-endpoint-key>"
   QnAEndpointHostName="<your-hostname>"
   ```
   ---
   
Your code should now be able to run and connect to your QnA Maker knowledgebase.
  
## Explore the QnAWeatherBot


## Run and debug your bot
Run this bot code and test it with your emulator in the same manner as you did for previous Labs. 
* It may be useful to add several breakpoints in your code and hover your cursor over 





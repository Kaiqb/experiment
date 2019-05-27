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

## Publish your knowledgebase


## Add connection information to your code


## Explore the QnAWeatherBot


## Run and debug your bot
Run this bot code and test it with your emulator in the same manner as you did for previous Labs. 
* It may be useful to add several breakpoints in your code and hover your cursor over 





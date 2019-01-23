# Course: Introduction to Bot Framework
This two-day course provides introduction to the Bot Framework SDK v4. The course covers concepts that are essential to building bots and provides opportunity to practice these concepts using hands-on labs. At the end of the course, participants will have a working Weather Bot that can be deployed to Azure.

## Course outline:
* Module 1: Introduction to the world of bots
* Module 2. Prep and setup
* Module 3: Run a simple bot
* Module 4: Greet users
* Module 5: State and Storage
* Module 6: Help users navigate 
* Module 7: Manage conversations using Dialogs library
* Module 8: Answer user’s questions using QnA Maker
* Module 9: Add natural language processing
* Module 10: Route messages using the Dispatch tool
* Module 11: Deploy bot to Azure
* Module 12: Add Channels (optional)
* Module 13: Authentication (optional)

## Module 1: Introduction to the world of bots
Objective: Familiarize participants with the an overview of the world of bots

In this module, you'll learn what a bot is, play with with a few bots, understand what is involved in making a bot, how do bots work, and what is the Azure Bot Service.

## Module 2: System set up

In this module, you will get your development environment set up to work with the [Azure Bot Service and the Bot Builder SDK.](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0) First, we
will walk through the tools needed to develop and run a bot locally on your laptop. Then, we'll put together the first pieces
of a brand new bot.

## Module 3: Run a simple bot
In this module, you'll learn how to process incoming message event to access and use the text of the user's message. We'll go over the concepts of an "activity" and how to appropriately respond to the incoming activities from the user to the bot. 

## Module 4: Greet users
The primary goal when creating any bot is to engage your user in a meaningful conversation. One of the best ways to achieve this goal is to ensure that from the moment a user first connects, they understand your bot’s main purpose and capabilities, the reason your bot was created. A welcome message should be generated whenever your users first interacts with your bot. To achieve this, you can monitor your bot’s Activity types and watch for new connections. 

## Module 5: State and Storage
A bot is inherently stateless. Once your bot is deployed, it may not run in the same process or on the same machine from one turn to the next. However, your bot may need to track the context of a conversation, so that it can manage its behavior and remember answers to previous questions. The state and storage features of the SDK allow you to add state to your bot.

## Module 6: Help users navigate
In a traditional application, the user interface (UI) is a series of screens. A single app or website can use one or more screens as needed to exchange information with the user. Most applications start with a main screen where users initially land and provide navigation that leads to other screens for various functions like starting a new order, browsing products, or looking for help.
Like apps and websites, bots have a UI, but it is made up of dialogs, rather than screens. Dialogs help preserve your place within a conversation, prompt users when needed, and execute input validation. 

In this module, you'll learn to use buttons, cards, and other types of attachments to help users navigate as they converse with your bot.

## Module 7: Manage conversations using Dialogs library
Dialogs enable the bot developer to logically separate various areas of bot functionality and guide conversational flow. For example, you may design one dialog to contain the logic that helps the user browse for products and a separate dialog to contain the logic that helps the user create a new order.
Dialogs may or may not have graphical interfaces. They may contain buttons, text, and other elements, or be entirely speech-based. Dialogs also contain actions to perform tasks such as invoking other dialogs or processing user input.

## Module 8: Answer user’s questions using QnA Maker
You can use QnA Maker service to add question and answer support to your bot. One of the basic requirements in creating your own QnA Maker service is to seed it with questions and answers. In many cases, the questions and answers already exist in content like FAQs or other documentation. Other times you would like to customize your answers to questions in a more natural, conversational way.

## Module 9: Add natural language processing
The ability to understand what your user means conversationally and contextually can be a difficult task, but can provide your bot a more natural conversation feel. Language Understanding, called LUIS, enables you to do just that so that your bot can recognize the intent of user messages, allow for more natural language from your user, and better direct the conversation flow.

## Module 10: Route messages using the Dispatch tool
In this tutorial, we demonstrate how to use the Dispatch service to route utterances when there are multiple LUIS models and QnA maker services for different scenarios supported by a bot. 

## Module 11: Deploy bot to Azure
After you have created your bot and tested it locally, you can deploy it to Azure to make it accessible from anywhere. Deploying your bot to Azure will involve paying for the services you use.

## Module 12: Add Channels (optional)
## Module 13: Authentication (optional)

# Route messages using the Dispatch service
In this tutorial, we demonstrate how to use the Dispatch service to route utterances when there are multiple LUIS models and QnA maker services for different scenarios supported by a bot.

## Introduction to the Dispatch service
* What is the Dispatch command line tool?
* Installing the Dispatch command line tool.
  - JS: npm install -g botdispatch.
* Initializing the Dispatch command line tool.

## Add dispatch key values into your code
* key values in .dispatch file.
* create dispatch using npm dispatch commands.
  - Clarify which keys to use for adding services, accessing services.
  - Where do I obtain these values? (Azure, Qnamaker.ai, Luis.ai)
* adding dispatch to weather bot code.
  - How to update appsetting.json or .env file.

## Run and test your dispatch model
* Logic paths based on top intent.
  - Steps to process QnA or LUIS result.
* Logic processing for none intent.

Online documentation reference: [Use multiple LUIS and QnA models](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-tutorial-dispatch?view=azure-bot-service-4.0&tabs=cs)

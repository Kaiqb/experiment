# Route messages using the Dispatch service
In this tutorial, we demonstrate how to use the Dispatch service to route utterances when there are multiple LUIS models and QnA maker services for different scenarios supported by a bot.

## Introduction to the Dispatch service
* What is the Dispatch command line tool?
* Installing the Dispatch command line tool.
  - JS: npm install -g botdispatch
* Initializing the Dispatch command line tool.

## Add dispatch key values into your code
* key values in .dispatch file.
* create dispatch using .bot file.
  - include "version": "Dispatch",

## Run and test your dispatch model
* Logic paths based on top intent.
  - Process QnA or LUIS result.
* Logic processing for none intent.

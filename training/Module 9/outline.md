# Add natural language processing
The ability to understand what your user means conversationally and contextually can be a difficult task, but can provide your bot a more natural conversation feel. Use Language Understanding so that your bot can recognize the intent of user messages, allow for more natural language from your user, and better direct the conversation flow.

## Introduction to LUIS
* LUIS app components
  - prebuilt LUIS models.
  - custom LUIS models.
  - user utterances
  - LUIS intents, entities, and scores
  
## Building your LUIS model
* LUIS programmatic APIs
  - how to find and use LUIS APIs.
* build you model using the LUIS portal.
  - how to structure intents.
  - use of the None intent.
  - applying utterances to train intents.
  - importing preconfigured intents and utterances for weather bot model.
  
## Modify your weather bot code to respond to LUIS intents
* code paths for expected intent results.
* coding logic for unexpected results or no results found.

## Querying your LUIS model
* obtaining key values and LUIS endpoint to query your model.
* adding LUIS connection values into your weather bot code.
* generating utterance queries that return LUIS model results.

## Interpreting LUIS results
* LUIS results structure
  - intent
  - entity
  - score
* interpreting LUIS score values.
  - handling multiple results.
* deciding when results contain no usable intents.

## Improving your LUIS model results
* reviewing endpoint utterances.
* how to assign utterance to intents, label entities
* how to train and test results.

Online documentation reference: [Add natural language understanding to your bot](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-v4-luis?view=azure-bot-service-4.0&tabs=csharp)

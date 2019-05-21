# Greet users
The primary goal when creating any bot is to engage your user in a meaningful conversation. One of the best ways to achieve this goal is to ensure that from the moment a user first connects, they understand your bot’s main purpose and capabilities, the reason your bot was created. A welcome message should be generated whenever your users first interacts with your bot. To achieve this, you can monitor your bot’s Activity types and watch for new connections.connection activitie

## Initial message - your bot's main purpose
* Determine what your bot does and does not do.
* Initial welcome message should explain your bot's purpose to connecting users.
  - Provide common key words to access information.
  - Helps users navigate your bot's capabilities.
  - Avoids user dissatisfaction and poor reviews.
  
## Understand connection activities
* Walkthrough of Activity types and when they occur.
* Discuss channel differences and dual channel update occurances.

## Working with state
* What is user state?
  - Where/when is it stored?
  - What can it store?
  - How is it accessed?

## Detecting and welcoming users
* How do we find a new user to greet them?
* querying didBotWelcomedUser - False
   - setting/saving didBotWelcomedUser
   - sending inital welcome message
* querying didBotWelcomedUser - True
   - sending welcome back message
   
Online documentation reference: [Send welcome message to users](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-send-welcome-message?view=azure-bot-service-4.0&tabs=csharp)
   


| Current Sample List           | Proposed Sample List  | Notes                                                                        | 
|-------------------------------|-----------------------|------------------------------------------------------------------------------| 
| 00.empty-bot                  |                       |**Remove**. Empty bot can be created using VSIX/Yeoman templates.       | 
| 01.console-echo               |                       |**Update**. Focus of this sample should be on creating and using an adapter. Add clear comments and provide better readme for this sample.        | 
| 02.a.echo-bot                 |                       |**Remove**. Echo bot can be created using VSIX/Yeoman templates. Use template's README to provide info on this type of bot. | 
| 02.b.echo-bot-with-counter    | echo-bot-with-counter |**Update**. This should use MVC pattern.          | 
| 03.welcome-user               | welcome-user          |**Update**. Simplify the sample by removing cards/attachments + fix bug. Use MVC pattern.  | 
| 04.simple-prompt              | simple-prompt         |**Update**. Handle ""cancel"" event so that the state is not corrupted. Use MVC pattern.   | 
| 05.multi-prompt               | multi-prompt          |**Update**. Use MVC pattern. Sample is used in the "Implement sequential conversation flow" topic.  | 
| 06.using-cards, 07.using-adaptive-cards, 08.suggested-actions  |  |**Combine**. Merge 06, 07, 08, and 15 into a single media-bot sample. | 
|                               | media-bot             |**Add**. New sample that combines 06, 07, 08, and 15 samples into one. Use MVC pattern. |
| 09.message-routing            |                       |**Remove**. Scenario not clear. It has RegEx, Dialogs, interruption, etc.| 
| 10.prompt-validations         |                       |**Remove**. Seems too basic for prompt validation. Use _PromptUsersforInput_ sample used in docs.                          | 
|                               | PromptUsersforInput   |**Add/Update**. Use this sample instead of _10.prompt-validation_ sample. Use MVC pattern.                                                                                  | 
|                               | StateBot              |**Add/Update**. This sample is used in "Save user and conversation data" topic. Use MVC pattern.                                                                                    | 
| 11.qnamaker                   | qnamaker              |**Update**. Use MVC pattern. Used in "QnA Maker to answer questions" topic"  | 
| 12.nlp-with-luis              | nlp-with-luis         |**Update**. Use MVC pattern. Used in "Add natural language understanding to your bot" topic". | 
| 13.basic-bot                  |                       |**Remove**. Conceptually, this is similar to the nlp-with-luis bot. | 
| 14.nlp-with-dispatch          | nlp-with-dispatch     |**Update**. Use MVC pattern. Used in "Use multiple LUIS and QnA models" topic.| 
| 15.handling-attachments       |                       |**Remove**. This sample will be combined with 06, 07, and 08 into media-bot sample.  | 
| 16.proactive-messages         | proactive-messages    |**Update**. Use MVC pattern. Used in ""Get notification from bots"" topic.  | 
|                               | DialogPrompt          |**Add/Update**. This is sample is used in dialogs topic. Update it to use MVC pattern.   | 
|                               | ComplexDialog         |**Add/Update**. This is sample is used in dialogs topic. Update it to use MVC pattern.    | 
| 17.multilingual-bot           | multilingual-bot      |**Update**. Use MVC pattern.            | 
| 18.bot-authentication         | bot-authentication    |**Update**. Use MVC pattern.    | 
| 19.custom-dialogs             | custom-dialogs        |**Update**. Use MVC pattern.                        | 
| 20.qna-with-appinsights       |                       |**Update**. Use MVC pattern. | 
| 21.luis-with-appinsights      |                       |**Update**. Use MVC pattern. |
| 22.conversation-history       |                       |**Update**. Use MVC pattern. This is about transcript logger. Rename?        | 
| 23.facebook-events            |                       |**Update**. Use MVC pattern.                                                 | 
| 24.bot-authentication-msgraph |                       |**Update**. Use MVC pattern.                                         | 
| 30.asp-mvc-bot                |                       |**Remove**. All of the samples will use MVC pattern.       | 
| 40.times-resolution           |                       |**Update**. Use MVC pattern.             | 
| 42.scaleout                   |                       |**Update**. Use MVC pattern.           | 
| 51.cafe-bot                   |                       |**Remove**.       |                     

### JS delta                                               
* diceroller-skill                                                                                                                  
* logger
* transcript-logger       

### Additional ideas:
* Handoff-to-human    
* Direct Line - A bot and a custom client communicating  using the Direct Line API. 
* Skype calling bot - Use Skype Bot Plaform for Calling API for receiving and handling Skype voice calls.


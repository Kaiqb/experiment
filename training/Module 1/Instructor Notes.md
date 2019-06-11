# Module 1: Introduction
In this module, you'll learn what a bot is, play with with a few bots, understand what is involved in making a bot, how do bots work, and what is the Azure Bot Service.

## What are bots?
* Quick history lesson
  - Bots have been part of the internet since the late 1980's. They have come in many forms some quite helpful, others quite harmful. Typse of applications grouped under the generic title of Bots include:
    - Web crawlers, spiders for page search engines.
    - chat bots, providing automated conversations and information. 
    - auction bots, allowing users to bid on items for sale.
    - automated trade bots, executing financial transactions based on news and current data.
    - control bots, often used for malicious activities and attacks.
* The modern bot landscape
  - Today bots account for almost 1/2 of all internet traffic. Many bots use AI to generate responses and can be indistinguishable from live human conversations.
  - Bots are also now entering the home & workplace. Just ask "Alexa", "Siri", or "Cortana".
* Uses and benefits of bots
  - Bots allow users to refine their searches and find useful data within a sea of data.
  - Bots also allow companies to answer many support questions without the need for as many paid support personnel.

## Hands on experience with a few bots.
There are many great examples of bots that people can use to become more familiar with them.

(**Instructors- chose bots of interest from this large list, or add your favorites**)
* Chatbot workplace examples
  - [HipMunk](https://www.hipmunk.com/hello) is your personal travel agent that you can chat with directly inside of Microsoft Teams, Skype or Facebook Messenger. You can write a message similar to what you would say to a real travel agent. For instance, “find me the cheapest non-stop flight from Seattle to Las Vegas January 6 - 10 departing after 1pm.” One benefit of the Hipmunk bot is that it can proactively reach out to you in the future to let you know if there are any deals or discounts on the flights you searched for.
  - [Polly](https://www.polly.ai/microsoft-teams) for Microsoft Teams allows you to very quickly create polls inside your Microsoft Teams Channel. It is a great way to capture structured data with polls running in tandem with your conversations, meaning your workflow is no longer interrupted. You can also get up-to-date insights into your polls with live responses to surveys and voting stats, all within Microsoft Teams.

* Chatbot Customer Service examples
  - [Amy](https://www.askus.business.hsbc.com.hk/english/bot.htm?isJSEnabled=1&businessArea=Root&channel=Root&startContext=Root.Cards.Business%20MasterCard) HSBC Bank has Amy, a bot that uses AI to operate in both English and Chinese.  Amy has an embedded customer feedback feature that helps her to learn over time enabling her to deal with a wider range of queries. When navigating to this page you will notice a welcome screen that also contains usage information and notifications about privacy and data collection.
  - [AVA](https://support.airasia.com/s/?language=en_GB)  Low-cost carrier AirAsia's chatbot AVA (for AirAsia Virtual Allstar) is another example of a customer service based chatbot. AVA assists customers using live chat and responds to inquiries instantly in any one of these eight languages: English, Bahasa Malaysia, Thai, Bahasa Indonesia, Vietnamese, Korean, Simplified Chinese and Traditional Chinese. To encourage customers to use AVA, AirAsia initially offered users a 19% discount for bookings via the app.
  - [IVY](https://www.youtube.com/watch?v=b96QIK8T2Wc). A 24-hour virtual concierge service developed by Caesars Entertainment, a casino-entertainment provider with 24,000 hotel rooms and suites across nine Las Vegas resorts, and used by various hotels across the country, texts its guests automatically (no download required) and customers can respond with any/all questions they may have such as "What is the Wi-Fi password", "what time is checkout" etc. thus freeing up the staff. It can also offer customers the option to check out late for a fee, ask customers to rate their stay so far and notify management if the rating is low and based on the questions asked by customers it can notify the front desk when needed.

* Chatbot marketing examples
    - [Hello Fresh](https://www.hellofresh.com/app/?redirectedFromAccountArea=true) provides a variety of well thought out prompts to help the customer determine what they want and place an order and stay away from open ended questions that are more difficult for the bot to process, which could result in a bad customer experience. They also have a built-in sales component that offers bot-exclusive discounts to encourage the use of their bot as well as helps them track the ROI of their bots.
    - [Dom](https://www.facebook.com/Dominos) Domino’s chatbot allows their customers to order pizzas just like they would online, and the ability to save and repeat orders makes it very convienent to order your favorite pizza without having to leave [Facebook](https://www.facebook.com/Dominos), or if watching TV [Apple TV](https://techcrunch.com/2016/08/30/you-can-now-order-pizza-from-your-apple-tv/?_ga=2.50997353.1525970220.1559158641-1926216467.1559158641).  Customer even have the option to use Domino’s chatbot with various other technologies such as Amazon Echo, Google Home, Ford Sync, SMS, Smart TV's, and even Smart Watches.
    - [Sephora](https://www.facebook.com/sephora). Another example of a marketing chatbot is Sephora.  Want to browse products? Book a makeover appointment? Try on makeup using augmented reality technology? This bot enables customers to do a variety of things without leaving Messenger, and demonstrates how companies can incorporate various marketing strategies into their bots to create a compelling user experience and help drive sales, both online and brick-and-mortar as this bot is able to tell you where specific products and makeovers are available nearest your current (or any specified) location. 
    - [Starbucks](https://botlist.co/bots/starbucks-barista) If you cannot live without your coffee and cannot communicate with people until you have had your morning dose, then the Starbucks Barista bot for Facebook Messenger may just be the most useful bot in your world, because it may just be the best way to get your morning fuel without any interaction with actual people.

* Chatbot Messenger Application examples
    - [Kik Messenger](https://en.wikipedia.org/wiki/Kik_Messenger).
    - [H&M](https://bots.kik.com/#/hm) Get outfit ideas and inspiration "Tell us a piece of clothing, and we’ll build an outfit around it for you. Anything from joggers and jeans to tops and shirts… we’ve got you covered! We’ll be your personal stylist for your lazy days or for your night outs."
    - [Vine](https://bots.kik.com/#/vine) See Vine videos based on keyword searches (like bunnies or kittens) "Vine is the entertainment network where the world's stories are captured, created and remixed."
    - [TheScore](https://bots.kik.com/#/scorebot) Real-time sports scores and updates "theScore brings you real-time sports scores and news from the teams you care about, with deep coverage of MLB, NBA, NFL, NHL and major soccer leagues!"
    - [Sensay](https://bots.kik.com/#/sensay) Connect to real humans to get advice "Want someone to talk to?  Sensay is a bot that connects you to real people to chat. It's totally anonymous so you can chat about anything you want. This isn't your mom's social network. Sensay is for talking to interesting strangers."
    - [Telegram](https://telegram.org/).
    - [National Geographic Bot](https://telegram.me/natgeobot) Sends you photos from National Geographic photographers.
    - [Amazon Bot](https://www.youtube.com/watch?v=f0Uq7na0vBk) Search for products on Amazon
    - [MyPokerBot]() Play Texas Holdem poker in a chat 

* Chatbot SMS examples (For those times when Texting is more convenient)
    - [Digit]() SMS bot that automatically monitors your bank account & saves you money.  More information on [venturebeat](https://venturebeat.com/2017/01/10/digit-bot-has-saved-people-250-million-and-is-now-available-on-facebook-messenger/)
    - [Magic](https://getmagic.com/) A personal Assistant that can accomplish various tasks for you, such as:
    - Schedule appointments with your hairdresser, dentist, or doctor
    - Plan a special event (including booking a venue and hiring a catering service)
    - Book your accommodations for flights, car services, hotel rooms, etc. 
    - Check your emails and alert you of urgent messages
    - Ordering items and having them delivered to you anywhere
    - Find you specific merchandise and look for the best price offer
* Maybe we could build our own fun one? How a bout a bot to tell us weather conditions in different cities?

## What is involved in making a bot?
While bots can do the same things as other Types  of software such as computational tasks, reading from and writing to files, connecting to databases as well as using other API provided in the .NET framework, what makes bots special and unique is their ability to communicate with humans in an interactive way as one person talking to another person.  The ability to do that is provided by both the Azure Bot Service and the Bot Framework that provide all the tools needed to build, test, deploy, and manage your bot.

### The different stages of bot development
#### Planning
As with all software development it is important to have a good understanding of what you are developing. What is the problem or set of problems that you are trying to solve? How can you create a bot that will solve these problems as good or better than other potential solutions such as direct contact with a person or another type of application or website? Your bot is in direct competition with these other solutions, so how can you increase your odds of developing a better solution that will attract and keep customers? That is why coming up with a design that prioritizes all of the required factors is critical to the success of your bot.  

Some questions to ask yourself when designing your bot include:
* Can the bot run on all the devices and platforms that your customers care about?
* Will using the bot be intuitive and easy to use for your customers?
* Does the bot solve your customers problems with the minimum number of steps?
* Does the bot solve your customers problems better, faster and easier than any other solution?

Another aspect of a good design is defining scenarios and in the case of bot development that will generally include creating mock conversations between your customer and your bot.  There is a transcript generator tool on GitHub designed for this purpose named [Chatdown](https://github.com/Microsoft/botbuilder-tools/tree/master/packages/Chatdown). 

#### Developing
You can develop your bot in the [Azure portal](https://docs.microsoft.com/sr-latn-rs/azure/bot-service/bot-service-quickstart?view=azure-bot-service-4.0), or use [C#](https://docs.microsoft.com/sr-latn-rs/azure/bot-service/dotnet/bot-builder-dotnet-sdk-quickstart?view=azure-bot-service-4.0) | [JavaScript](https://docs.microsoft.com/sr-latn-rs/azure/bot-service/javascript/bot-builder-javascript-quickstart?view=azure-bot-service-4.0) templates for local development.

There are several [samples](https://github.com/microsoft/botbuilder-samples) available on GitHub that demonstrate how to do the following tasks as well as many more:
* Receive and send messages.
* Use a waterfall dialog, prompts, and component dialog to create a simple interaction that asks the user for name, age, and prints back that information.
* Use card Types  including thumbnail, audio, media etc. 
* Use the multi-turn dialog to get user input for name and age.
* Create a web page with custom Web Chat component.

#### Testing
Bots are complex apps and like any other complex app you are bound to find some interesting and unexpected behavior. Before publishing your bot it is a good idea to test it. Here are a couple of ideas:

* The [Bot Framework Emulator](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-debug-emulator?view=azure-bot-service-4.0) is a stand-alone app that provides a chat interface as well as debugging and interrogation tools.  You can use it to help understand how and why your bot does what it does and it can be run locally alongside your bot application while in development.

* If your bot is created using the [Azure Bot Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-quickstart?view=azure-bot-service-4.0) and configured through the Azure portal it can be reached through a web chat interface which is a great way to give access to anyone who does not have direct access to the bot's running code.  You can do this to enable others to test your bots functionality or review the design.

#### Publishing
When you are ready for your bot to be available on the web, you can publish in to [Azure](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-deploy-az-cli?view=azure-bot-service-4.0&tabs=newrg) or to your own web service or data center. 

#### Connecting
You can connect your bot to various _channels_ such as Facebook, Messenger, Kik, Skype, Slack, Microsoft Teams, Telegram, text/SMS, Twilio, Cortana, Skype etc. The bot Framework does most of the work necessary to send and receive messages from all of these different platforms - your bot application receives a unified, normalized stream of messages regardless of the number and type of channels it is connected to. For information see [channels](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-manage-channels?view=azure-bot-service-4.0).

#### Evaluating
You can use data collected and stored in the Azure portal to evaluate how well it is meeting your objectives which will help you identify opportunities for improvement in terms of both capabilities and performance. You can get service-level and instrumentation data like traffic, latency, and integrations. Analytics also provides conversation-level reporting on user, message, and channel data. For more information, see [how to gather analytics](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-manage-analytics?view=azure-bot-service-4.0).

## How do bots work?
* Messages are received through APIs
* The message are processed by software. This is where the magic happens.
* Replies are generated somehow and sent back via APIs

## Sounds easy! What is the hard part?
* Isn’t this all about AI magic?
* Conversational software works differently than other type of apps
* Hidden complexity as you deal with conversation flow and choices.
* Humans talk funny. Understanding exactly what they mean is hard.
* Don't frustrate your users. Plan ahead to handle interruptions and changes of topic.

## What is the Azure Bot Service?
* It’s an integrated set of tools and services that makes it easier to design, build, test, deploy, measure and manage bots.
* You can use the framework provided by the SDK along with the other tools, templates, and AI services to create bots that use speech, understand natural language, handle questions and answers, and more. 
  - The SDKs currently (as of this writing) exist for C# and JavaScript (with SDKs for Java and Python under development.) Which provides tools for the various stages of bot development to help you design and build bots.

## Tools available for use with bots
* NLP things, can give LUIS or QnA as an example.
  - [LUIS](https://www.luis.ai/home). Language Understanding Intelligent Service (LUIS) offers a fast and effective way of adding language understanding to your bot. Designed to identify valuable information in conversations, LUIS is designed to interpret your customers intents by distilling the important information from their input. 
  - [QnA](https://www.qnamaker.ai/). QnA Maker is a cloud-based API service that creates a conversational, question and answer layer over your data.
* Other tools that come in helpful?
  - Dispatch used to determine where to send a user's request based on the "intent" of their request.

Online documentation reference: [About Azure Bot Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)


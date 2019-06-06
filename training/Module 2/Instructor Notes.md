# Module 2: Setup
Set up your development environment to work with the [Azure Bot Service and the Bot Framework SDK.](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0) First, we
will walk through the tools needed to develop and run a bot locally on your laptop. Then, we'll put together the first pieces
of a brand new bot.

## Obtain an OpenWeatherMap Key
Later today, we'll build a bot that your users can use to request current weather conditions. To do that, you will need to have a free key that allows you to send requests to the Open Source weather service, OpenWeatherMap.
* Use your email account to register and obtain a Free Subscription API key from OpenWeatherMap APIs.
  You can request your free subscription key here: http://home.openweathermap.org/users/sign_in

## Get started with Azure
* Set up/login to your Azure account. (Instructor, there's an assumption that if students don't have an Azure account one will be provided for the course)

## Install and set up Azure CLI
* Install and log into [AZ CLI](https://aka.ms/az-cli-download) using Azure account credentials.
  -  Open a command prompt to log in to the Azure portal.

  ```cmd
  az login
  ```
  - A browser window will open, allowing you to sign in.
  - Set the default subscription to use.

  ```cmd
  az account set --subscription "<azure-subscription>"
  ```
  - If you are not sure which subscription to use for deploying the bot, you can view the list of subscriptions for your account by using `az account list` command.

## Set up your development environment
* Install IDE for course language
  - C# [Visual Studio Dev Essentials](https://visualstudio.microsoft.com/dev-essentials/)
  - JS [Visual Studio Code](https://code.visualstudio.com/Download)

## First look at the sample code
* Install initial Sample app for course language: [C# EchoBot](https://aka.ms/cs-echobot-sample), [JavaScript EchoBot](https://aka.ms/js-echobot-sample)
* Install Microsoft.Bot libraries for the course language
  - C# - right click on Project, "Manage NuGet Packages..."
  - JS - Open console window, cd to directory with your code.
    - "nmp install npm".
    - "npm install restify".
    - "npm install path".
    - "npm install botbuilder".
* Install latest [Bot Framework Emulator](https://aka.ms/bot-framework-emulator-readme)
* Run initial sample bot and enter "Hello" in the Emulator.

Online documentation reference: [Create a bot with the Bot Framework SDK for .NET](https://docs.microsoft.com/en-us/azure/bot-service/dotnet/bot-builder-dotnet-sdk-quickstart?view=azure-bot-service-4.0)

Online documentation reference: [Create a bot with the Bot Framework SDK for JavaScript](https://docs.microsoft.com/en-us/azure/bot-service/javascript/bot-builder-javascript-quickstart?view=azure-bot-service-4.0)


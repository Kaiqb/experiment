# Lab 3, WeatherBot Cards
In this lab we will begin with by downloading a sample weatherBot that was created based on BotBuilder-Samples/.../06.using-cards.

> [!NOTE] You will need to use the OpenWeatherMap key you obtained by request at the beginning of Session 2 for this code to function properly.

* Download Lab3 source code from [here](../../Code/Lab3%20Cards%20MVC)   (or add different link)

This sample code uses a Waterfall dialog to interact with your user, find their city of choice selected from a fixed list of PromptOptions, queries OpenWeatherMap for current weather conditions, and returns an Adaptive Card showing weather information for the selected city's location. There's a lot going on in this code, so let's look at some of the key pieces.

## Add your OpenWeatherMap key to appsettings.json
* Locate and open file appsettings.json
* Add the key you obtained through your email account as follows:

```json
{
   "MicrosoftAppId": "",
  "MicrosoftAppPassword": "",
  "OpenWeatherMapKey": "<your-key-here>"
}
```
This key provides you with free online access to a limited set of OpenWeatherMap API calls.

## Looking at the Waterfall
* Locate and open file MainDialg.cs found inside of the 'Dialogs' folder.



  

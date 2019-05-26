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

## Looking at the Waterfall dialog in detail
Waterfall dialogs, once called, follow a fixed set of steps before finishing. 
Let's look at the weatherbot Waterfall dialog.
* Locate and open file MainDialg.cs found inside of the 'Dialogs' folder.
* Waterfall dialog, line 33, is created with 2 steps
  - ChoiceCardStepAsync - prompts the user to select a new weatherBot card (or retry on error).
  - ShowCardStepAsync - reponds to user selection with a current weather conditions card.
Each of these steps can be explored in more detail.

## ChoiceCardStepAsync
Method _ChoiceCardStepAsync_ 
* First uses a prompt to ask your user to select a city. 
* Calls method _getChoices()_ that provides your user with a fixed list of city choices.
* then waits to return the selected city name back to your waterfall method.
  - if a valid city is selected, the name is returned.
  - if an invalid input is received, _RetryPrompt_ will ask your user to try again.
 Once a valid choice is received, Our waterfall moves down to the ShowCardStepAsync step.
 
 ## ShowCardStepAsync
 Method _ShowCardStepAsync_
* Uses the passed value found in _stepContent.Result_ to call method _GetForecastInformation()_
  - This method returns a current weather forecast from OpenWeatherMap.org in JSON format.
* The switch statement below this call uses the returned JSON information, converted to a  NewtonSoft JObject, and calls the Card Class method _CreateAdaptiveCardAttachment()_ to build an adaptive card for the requested city.
  - You can explore the details of method _CreateAdaptiveCardAttachment()_ by finding and opening file Cards.cs.
  - Note that this architecture allows you to modify any received data prior to building the returned adaptive card.
* The next two _stepContext_ calls display the weatherBot forecast card adn then prompt you user to make another selection.
* finally, method _ShowCardStepAsync_ returns the call _EndDialogAsync()_ which tells our Waterfall dialog that all interactions for this user input are finished.

## Explore the running code
As in previous labs, you can set breakpoints at various places of interest. Then:
* Run the program and interact with it using the emulator
  - https://localhost:3978.api.messages 
* Hover over values such as _stepContext_ to see how data is passed from your user to your program.

## Modify the lab code
If you want to make this code your own, try changing one of the cities listed in the method _GetChoices()_ from its current value to your own home town instead. Remember that you will need to change the city name in all five locations where it is found.
* OpenWeatherMap will return the top selection for the city name you pass to it. If your home town name is the same as a much larger area, you might not receive the weather you are hoping for.


  

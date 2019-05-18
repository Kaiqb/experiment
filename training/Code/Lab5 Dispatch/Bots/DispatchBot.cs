// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace DispatchWeatherBot
{
    public class DispatchWeatherBot : ActivityHandler
    {
        private BotState _conversationState;
        private BotState _userState;
        private readonly IConfiguration _configuration;
        private ILogger<DispatchWeatherBot> _logger;
        private IBotServices _botServices;

        private string openMapKey;
        private const string DailyForecast = "daily";
        private const string HourlyForecast = "hourly";

        private class LUISEntities
        {
            public string Location { get; set; } = string.Empty;
            public string Condition { get; set; } = string.Empty;
            public string Sun { get; set; } = string.Empty;
        }

        public DispatchWeatherBot(ConversationState conversationState, UserState userState, IConfiguration configuration, IBotServices botServices, ILogger<DispatchWeatherBot> logger)
        {
            _conversationState = conversationState;
            _userState = userState;
            _configuration = configuration;
            _logger = logger;
            _botServices = botServices;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Get the state properties.
            var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

            var userStateAccessors = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            var userProfile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());

            // First, we use the dispatch model to determine which cognitive service (LUIS or QnA) to use.
            var recognizerResult = await _botServices.Dispatch.RecognizeAsync(turnContext, cancellationToken);
            
            // Top intent tell us which cognitive service to use.
            var topIntent = recognizerResult.GetTopScoringIntent();

            // Add message details to the conversation data.
            conversationData.Timestamp = turnContext.Activity.Timestamp.ToString();
            conversationData.ChannelId = turnContext.Activity.ChannelId.ToString();

            await turnContext.SendActivityAsync($"Returned intent: {topIntent.intent} ({topIntent.score}).");

            // Next, we call the dispatcher with the top intent.
            await DispatchToTopIntentAsync(turnContext, topIntent.intent, recognizerResult, cancellationToken);

            // Save any state changes that might have occured during this turn.
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            const string WelcomeText = "Type a greeting, or a question about the weather to get started.";

            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Welcome to Dispatch bot {member.Name}. {WelcomeText}"), cancellationToken);
                }
            }
        }

        private async Task DispatchToTopIntentAsync(ITurnContext<IMessageActivity> turnContext, string intent, RecognizerResult recognizerResult, CancellationToken cancellationToken)
        {
            switch (intent)
            {
                case "l_Weather":
                    await ProcessWeatherAsync(turnContext, recognizerResult.Properties["luisResult"] as LuisResult, cancellationToken);
                    break;
                case "q_sample-qna":
                    await ProcessSampleQnAAsync(turnContext, cancellationToken);
                    break;
                default:
                    _logger.LogInformation($"Dispatch unrecognized intent: {intent}.");
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Dispatch unrecognized intent: {intent}."), cancellationToken);
                    break;
            }
        }

        private async Task ProcessWeatherAsync(ITurnContext<IMessageActivity> turnContext, LuisResult luisResult, CancellationToken cancellationToken)
        {
            // Get the state properties.
            var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

            var userStateAccessors = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            var userProfile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());

            await turnContext.SendActivityAsync(MessageFactory.Text($"Sending your request to Luis."), cancellationToken);
            _logger.LogInformation("ProcessWeatherAsync");
            openMapKey = _configuration["OpenWeatherMapKey"];
            conversationData.DispatchIntent = luisResult.TopScoringIntent.Intent;

            var results = await _botServices.Forecasts.RecognizeAsync(turnContext, cancellationToken);

            // Top intent tells us which cognitive service to use.
            var topIntent = results.GetTopScoringIntent();
            conversationData.LuisIntent = topIntent.intent;
            
            // uncomment to debug returned weather intents.
            // await turnContext.SendActivityAsync(MessageFactory.Text($"Forecasts intent returned {topIntent.intent}."), cancellationToken);

            // See if LUIS found and used an entity to determine user intent.
            LUISEntities entityFound = ParseLuisForEntities(results);

            // save all returned entities within conversationData
            conversationData.Location = entityFound.Location;
            conversationData.Condition = entityFound.Condition;
            conversationData.Sun = entityFound.Sun;

            if (entityFound.Location == string.Empty)
            {
                if ((userProfile.Location != null) && (userProfile.Location != string.Empty))
                {
                    entityFound.Location = userProfile.Location;
                    conversationData.Location = userProfile.Location;
                }
                else
                {
                    // Inform the user we found no location.
                    await turnContext.SendActivityAsync("Sorry, no location was provided for your request. Please add a phrase like 'in Redmond' to your weather question.");
                }
            }
            else
            {
                // Save the location this user asked about for later use as well.
                userProfile.Location = entityFound.Location;
            }

            if (topIntent.intent != null && topIntent.intent != "None")
            {
                string debugInfo = string.Empty; // $"==> LUIS Top Scoring Intent: { topIntent.Value.intent}, LUIS location entity: { entityFound.Location}, Score: { topIntent.Value.score}\n ";
                if (entityFound.Location != string.Empty && topIntent.intent == "Daily_Forecast")
                {
                    // Use top intent and "entityFound" = location to call daily weather service here...
                    var jsonResult = GetForecastInformation(DailyForecast, entityFound);

                    var currentConditions = FindCurrentConditions(jsonResult);
                    var currentTemp = FindCurrentTemp(jsonResult);
                    await turnContext.SendActivityAsync(debugInfo + $"Daily weather forecast for {entityFound.Location}.\n {currentConditions}, temperature: {currentTemp}F");
                }
                else if (entityFound.Location != string.Empty && topIntent.intent == "Hourly_Forecast")
                {
                    // Use top intent and "entityFound" = location to call hourly weather service here...
                    var jsonResult = GetForecastInformation(HourlyForecast, entityFound);

                    // Call FindHourlyForecast
                    var currentForecast = FindHourlyForecast(jsonResult, entityFound);
                    await turnContext.SendActivityAsync(debugInfo + currentForecast);
                }
                else if (entityFound.Location != string.Empty && topIntent.intent == "When_Condition")
                {
                    // Get forecast information
                    var jsonResult = GetForecastInformation(HourlyForecast, entityFound);

                    // Find when that condition is happening
                    var currentForecast = FindHourlyForecast(jsonResult, entityFound);
                    await turnContext.SendActivityAsync(debugInfo + currentForecast);
                }
                else if (entityFound.Location != string.Empty && topIntent.intent == "When_Sun")
                {
                    // Get forecast information
                    var jsonResult = GetForecastInformation(DailyForecast, entityFound);

                    var sunStatus = FindSunTime(jsonResult, entityFound.Sun);
                    await turnContext.SendActivityAsync($"Today in {entityFound.Location} the sun will " + sunStatus);
                }
                else if (topIntent.intent == "User_Goodbye")
                {
                    // Say goodbye.
                    await SignOutUser(turnContext);
                }
            }
            else
            {
                var msg = @"Try weather inputs like: 'Show me weather for Redmond.' or 'Will it rain in Redmond?'.";
                await turnContext.SendActivityAsync(msg);
            }
        }

        private async Task ProcessSampleQnAAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ProcessSampleQnAAsync");

            var results = await _botServices.SampleQnA.GetAnswersAsync(turnContext);
            if (results.Any())
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(results.First().Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Sorry, could not find an answer in the Q and A system."), cancellationToken);
            }
        }

        private JObject GetForecastInformation(string forecastType, LUISEntities entityFound)
        {
            string forecastUrl = string.Empty;

            switch (forecastType)
            {
                case DailyForecast:
                    forecastUrl = "http://api.openweathermap.org/data/2.5/weather?q=" + entityFound.Location + "&APPID=" + openMapKey;
                    break;
                case HourlyForecast:
                    forecastUrl = "http://api.openweathermap.org/data/2.5/forecast?q=" + entityFound.Location + "&APPID=" + openMapKey;
                    break;
                default:
                    throw new InvalidOperationException($"Invalid forecast type requested.");
            }

            return GetFormattedJSON(forecastUrl);
        }

        // Gets JSON from OpenWeatherMap API and formats it into a JObject.
        private JObject GetFormattedJSON(string url)
        {
            // Create a web client.
            using (WebClient client = new WebClient())
            {
                // Get the response string from the URL.
                string resultJSON = client.DownloadString(url);
                JObject json = JObject.Parse(resultJSON);
                return json;
            }
        }

        // Examines the LUIS response for entities, namely a location for the weather forecast.
        // <returns>String containing the entities, if any.</returns>
        private LUISEntities ParseLuisForEntities(RecognizerResult recognizerResult)
        {
            LUISEntities result = new LUISEntities();
            var temp = recognizerResult.Entities.First;
            // recognizerResult.Entities returns type JObject.
            foreach (var entity in recognizerResult.Entities)
            {
                // use JsonConvert to convert entity.Value to a dynamic object.
                dynamic o = JsonConvert.DeserializeObject<dynamic>(entity.Value.ToString());

                if (result.Location.Equals(string.Empty))
                {
                    // Take the first entity
                    if (o.location != null)
                    {
                        // Grab first location entry
                        result.Location = o.location[0].text;

                        // Since it's a location, make sure first letter is capitalized
                        result.Location = result.Location.First().ToString().ToUpper() + result.Location.Substring(1);
                    }
                }
                if (result.Condition.Equals(string.Empty))
                {
                    // Take the first entity
                    if (o.condition != null)
                    {
                        result.Condition = o.condition[0].text;
                    }
                }
                if (result.Sun.Equals(string.Empty))
                {
                    // Take the first entity
                    if (o.sun != null)
                    {
                        result.Sun = o.sun[0].text;
                    }
                }
                // If we found entities, they will be together in the same entry. Return those results.
                if (result.Sun.Equals(string.Empty) &&
                    result.Condition.Equals(string.Empty) &&
                    result.Location.Equals(string.Empty))
                {
                    continue;
                }
                return result;
            }
            // No entities found.
            return result;
        }

        // Finds the current conditions from provided forecast information.
        private string FindCurrentConditions(JObject json)
        {
            string currentConditions = (string)json["weather"][0]["description"];
            string currentSkies = (string)json["weather"][0]["main"];

            // format and return conditions string
            string conditionsString = "Skies: " + currentSkies + ", conditions: " + currentConditions;
            return conditionsString;
        }

        // Finds the hourly forecast from provided forecast information.
        private string FindHourlyForecast(JObject json, LUISEntities entities)
        {
            string hourlyForecastString = $"Hourly weather forecasts for {entities.Location}.\n";
            string hourlyTempString = "00.00";
            string hourlyConditionString = "cloudy";
            string hourlyTimeString = string.Empty;
            string conditionString = string.Empty;
            string conditionStart = string.Empty;
            string condition = entities.Condition;

            int counter = 0;

            if (condition != string.Empty)
            {
                // Truncate string for grammar for precipitation
                condition = condition.Replace("ing", string.Empty);
                condition = condition.Replace("sunny", "sun");
                condition = condition.Replace("cloudy", "clouds");
                condition = condition.Replace("y", string.Empty);
            }

            // LINQ query to get the list of hourly forecasts
            var hourlyForecast =
                from f in json["list"]
                select f;

            foreach (var forecast in hourlyForecast)
            {
                // Get the temp and convert it.
                hourlyTempString = FindCurrentTemp(forecast as JObject);

                // Get the conditions.
                hourlyConditionString = FindCurrentConditions(forecast as JObject);

                // Get the current time from the forecast.
                DateTime start_time = (DateTime)forecast["dt_txt"];

                // Convert from UTC to local time.
                start_time = start_time.ToLocalTime();

                // Add 90 minutes to get to the middle of the interval.
                start_time += new TimeSpan(1, 30, 0);
                hourlyTimeString = start_time.ToShortTimeString();

                if (!condition.Equals(string.Empty) &&
                    hourlyConditionString.Contains(condition) &&
                    conditionStart.Equals(string.Empty))
                {
                    conditionStart = hourlyTimeString + "\n";
                }

                // Build the forecast string from information above and append it.
                hourlyForecastString = hourlyForecastString + "Forecast for: " + hourlyTimeString + ", Temperature: " + hourlyTempString + "F, " + hourlyConditionString + "\n";

                // Only give first 8, which is a full day.
                counter++;
                if (counter > 7)
                {
                    break;
                }
            }

            if (!condition.Equals(string.Empty))
            {
                if (conditionStart.Equals(string.Empty))
                {
                    conditionString = "There isn't any " + condition + " in the forecast for the next 24 hours!\n";
                }
                else
                {
                    conditionString = "It looks like you will see " + condition + " by about " + conditionStart + "\n";
                }
            }

            return conditionString + hourlyForecastString;
        }

        private string FindSunTime(JObject json, string sun)
        {
            string result = string.Empty;
            DateTimeOffset sunTime;

            // Strip 'sun' out of the string
            sun = sun.Replace("sun", string.Empty);

            if (sun.Contains("rise"))
            {
                // Grab the sunrise time
                sunTime = DateTimeOffset.FromUnixTimeSeconds((long)json["sys"]["sunrise"]);
            }
            else if (sun.Contains("set"))
            {
                // Grab the sunset time
                sunTime = DateTimeOffset.FromUnixTimeSeconds((long)json["sys"]["sunset"]);
            }
            else
            {
                return "Failed to get a sun state.";
            }

            // Convert from UTC to local time.
            result = sun + " at " + sunTime.LocalDateTime.ToShortTimeString();

            return result;
        }

        // Finds the current temp from provided JSON forecast.
        private string FindCurrentTemp(JObject json)
        {
            return KelvinToFahrenheit((double)json["main"]["temp"]);
        }

        // Converts from Kelvin to Fahrenheit.
        private string KelvinToFahrenheit(double kelvin)
        {
            string currentTempString = "00.00";

            double tempFahrenheit = (1.8 * (kelvin - 273.15)) + 32;
            currentTempString = Convert.ToString(tempFahrenheit);

            // truncate to xx.xx or -x.xx ...
            currentTempString = currentTempString.Substring(0, 5);

            return currentTempString;
        }

        private async Task SignOutUser(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Get the state properties.
            var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

            var userStateAccessors = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            var userProfile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());

            // Clean up any resources here...
            conversationData.Timestamp = string.Empty;
            conversationData.ChannelId = string.Empty;
            conversationData.Location = string.Empty;
            conversationData.Condition = string.Empty;
            conversationData.Sun = string.Empty;

            userProfile.Location = string.Empty;

            await turnContext.SendActivityAsync($"Thank you for using WeatherBot!");

            // Save the cleaned resources.
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
    }
}

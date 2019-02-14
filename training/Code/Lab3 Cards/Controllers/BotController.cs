// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace RichMedia.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Rest.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>Defines a controller for this route.</summary>
    [Route("api/messages")]
    public class BotController : Controller
    {
        /// <summary>The serializer to use for request and response payloads.</summary>
        public static readonly JsonSerializer BotMessageSerializer
            = JsonSerializer.Create(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = new ReadOnlyJsonContractResolver(),
                Converters = new List<JsonConverter> { new Iso8601TimeSpanConverter() },
            });

        /// <summary>Map of user options to sample attachments for each type of card.</summary>
        private static Dictionary<string, Attachment> ShowCardAsync
            => new Dictionary<string, Attachment>
            {
                ["Chicago"] = Attachments.ChicagoCardAttachment,
                ["London"] = Attachments.LondonCardAttachment,
                ["Miami"] = Attachments.MiamiCardAttachment,
                ["Seattle"] = Attachments.SeattleCardAttachment,
                ["Sydney"] = Attachments.SydneyCardAttachment,
            };

        // API key to access Free OpenWeatherMap APIs.
        // NOTE - Register at http://home.openweathermap.org/users/sign_in to obtain a free subscription key.
        private const string OpenWeatherMapKey = "64ff82cecc338ba76e3a1dc7f19a73ae";

        /// <summary>A message that contains suggested actions for the available card types to send.</summary>
        private static IActivity suggestedActions =>
            MessageFactory.SuggestedActions(ShowCardAsync.Keys, "Please choose the weather forecast location.");

        /// <summary>Handles incoming POST requests.</summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [HttpPost]
        public async Task PostAsync()
        {
            // Get the activity from the HTTP request.
            var activity = default(Activity);
            using (var bodyReader
                = new JsonTextReader(new StreamReader(Request.Body, Encoding.UTF8)))
            {
                activity = BotMessageSerializer.Deserialize<Activity>(bodyReader);
            }

            // Create an adapter.
            var credentialProvider = new SimpleCredentialProvider();
            var botFrameworkAdapter = new BotFrameworkAdapter(credentialProvider)
            {
                OnTurnError = async (turnContext, excepption) =>
                {
                    // Code to run when the adapter catches an othwise unhandled exception.
                    await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");

                    Console.Error.WriteLine($"{excepption.GetType().Name} encountered:");
                    Console.Error.WriteLine(excepption.Message);
                    Console.Error.WriteLine(excepption.StackTrace);
                }
            };

            // Use the adapter to authenticate and forward incoming activities.
            var invokeResponse = await botFrameworkAdapter.ProcessActivityAsync(
                Request.Headers["Authorization"],
                activity,
                OnTurnAsync,
                default(CancellationToken));
            if (invokeResponse == null)
            {
                // For non-invoke activities, acknowledge the activity.
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                // For invoke activities, include a response payload.
                Response.ContentType = "application/json";
                Response.StatusCode = invokeResponse.Status;
                using (var writer = new StreamWriter(Response.Body))
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    BotMessageSerializer.Serialize(jsonWriter, invokeResponse.Body);
                }
            }
        }

        /// <summary>Defines the turn handler for the bot.</summary>
        /// <param name="turnContext">The current turn context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        private async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Type is ActivityTypes.Message)
            {
                // On a message activity display the type of card they asked for.
                foreach (var cardType in ShowCardAsync.Keys)
                {
                    if (turnContext.Activity.Text.Equals(cardType, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Use cardType as location to get forecast from daily weather service.
                        var jsonResult = GetForecastInformation(cardType);

                        // Create an Adaptive Card for this forecast. 
                        string cardDetails = BuildWeatherCard(jsonResult, cardType);

                        // File name for storing the Adaptive Card information.
                        string detailsFileName = cardType + "Details.json";

                        // Save JSON file in the Resources folder.
                        System.IO.File.WriteAllText(@".\Resources\" + detailsFileName, cardDetails);

                        // Now send an Adaptive Card showing weather based on this constructed JSON file.
                        await turnContext.SendActivityAsync(
                            MessageFactory.Attachment(ShowCardAsync[cardType]),
                            cancellationToken);
                        break;
                    }
                }

                // Resend suggested actions, whether or not we understood their input.
                await turnContext.SendActivityAsync(
                    suggestedActions,
                    cancellationToken: cancellationToken);
            }
            else if (turnContext.Activity.Type is ActivityTypes.ConversationUpdate)
            {
                var activity = turnContext.Activity.AsConversationUpdateActivity();
                var newUsers = activity.MembersAdded.Where(member => member.Id != activity.Recipient.Id);
                if (newUsers.Count() > 0)
                {
                    // If new users joined the conversation, send suggested actions.
                    await turnContext.SendActivityAsync(
                        suggestedActions,
                        cancellationToken: cancellationToken);
                }
            }
            else
            {
                // Otherwise, note the type of activity received.
                await turnContext.SendActivityAsync($"Received a `{turnContext.Activity.Type}` activity.");
            }
        }
        private JObject GetForecastInformation(string locationName)
        {
            string forecastUrl = string.Empty;
            forecastUrl = "http://api.openweathermap.org/data/2.5/weather?q=" + locationName + "&APPID=" + OpenWeatherMapKey;
            return GetFormattedJSON(forecastUrl);
        }

        /// <summary>
        /// Gets JSON from weather API and formats it into a JObject.
        /// </summary>
        /// <param name="url">URL for weather API call.</param>
        /// <returns>JObject containing the returned weather information.</returns>
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

        private string BuildWeatherCard(JObject jsonResult, string forecastLocation)
        {
            string cardDetails = string.Empty;

            // Save weather forecast details.
            string forecastDate = System.DateTime.Now.ToLongDateString();
            // Find weather conditions from the forecast results: clear, clouds, rain, snow
            var currentConditions = FindCurrentConditions(jsonResult);
            // Find URL to display forecast weather icon.
            var conditionsURL = FindConditionsURL(currentConditions);
            var currentTemp = FindCurrentTemp(jsonResult);
            // Retrieve Detail pieces to construct our JSON
            var jsonString1 = System.IO.File.ReadAllText(@".\Resources\GenericDetails1.json");
            var jsonString2 = System.IO.File.ReadAllText(@".\Resources\GenericDetails2.json");
            var jsonString3 = System.IO.File.ReadAllText(@".\Resources\GenericDetails3.json");
            var jsonString4 = System.IO.File.ReadAllText(@".\Resources\GenericDetails4.json");
            var jsonString5 = System.IO.File.ReadAllText(@".\Resources\GenericDetails5.json");
            var jsonString6 = System.IO.File.ReadAllText(@".\Resources\GenericDetails6.json");

            // Build JSON with embedded weather details.
            cardDetails = jsonString1 + forecastLocation + jsonString2 + forecastDate + jsonString3 + conditionsURL + jsonString4 + currentTemp + jsonString5 + currentConditions + jsonString6;

            return cardDetails;
        }

        /// <summary>
        /// Finds the current conditions from provided forecast information.
        /// </summary>
        /// <param name="json">Forecast information from OpenWeather API.</param>
        /// <returns>String representation of current weather conditions.</returns>
        private string FindCurrentConditions(JObject json)
        {
            string currentConditions = (string)json["weather"][0]["description"];
            string currentSkies = (string)json["weather"][0]["main"];
            return currentSkies;
        }

        private string FindConditionsURL(string conditions)
        {
            conditions = conditions.ToLower();
            string conditionsURL = string.Empty;
            switch (conditions)
            {
                case "clear":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Sunny-Square.png";
                    break;
                case "clouds":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Cloudy-Square.png";
                    break;
                case "mist":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Drizzle-Square.png";
                    break;
                case "rain":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Drizzle-Square.png";
                    break;
                case "snow":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Snow-Square.png";
                    break;
                default:
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Sunny-Square.png";
                    break;

            }

            return conditionsURL;
        }

        /// <summary>
        /// Find the current temp from provided JSON forecast.
        /// </summary>
        /// <param name="json">Forecast information from OpenWeather API.</param>
        /// <returns>String representation of the current tempurature.</returns>
        private string FindCurrentTemp(JObject json)
        {
            return KelvinToFahrenheit((double)json["main"]["temp"]);
        }

        /// <summary>
        /// Converts from Kelvin to Fahrenheit.
        /// </summary>
        /// <param name="kelvin">Tempurature value, in Kelvin</param>
        /// <returns>String representation of the Fahrenheit tempurature.</returns>
        private string KelvinToFahrenheit(double kelvin)
        {
            string currentTempString = "00.0";

            double tempFahrenheit = (1.8 * (kelvin - 273.15)) + 32;
            currentTempString = Convert.ToString(tempFahrenheit);

            // truncate to xx.xx or -x.xx ...
            currentTempString = currentTempString.Substring(0, 4);

            return currentTempString;
        }
    }

}

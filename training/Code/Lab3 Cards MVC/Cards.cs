// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Microsoft.BotBuilderSamples
{
    public class Cards
    {
        public static Attachment CreateAdaptiveCardAttachment(string location, JObject jsonResult)
        {
            string detailsFile = location + "Details.json";
            var currentDate = System.DateTime.Now;
            // It's tomorrow in Oz!
            if (location == "Sydney")
            {
                currentDate = currentDate.AddDays(1);
            }
            string forecastDate = currentDate.ToLongDateString();
            
            // Conditions: clear, clouds, rain, snow
            var currentConditions = FindCurrentConditions(jsonResult);
            var conditionsURL = FindConditionsURL(currentConditions);
            var currentTemp = FindCurrentTemp(jsonResult);

            var jsonString1 = System.IO.File.ReadAllText(@".\Resources\GenericDetails1.json");
            var jsonString2 = System.IO.File.ReadAllText(@".\Resources\GenericDetails2.json");
            var jsonString3 = System.IO.File.ReadAllText(@".\Resources\GenericDetails3.json");
            var jsonString4 = System.IO.File.ReadAllText(@".\Resources\GenericDetails4.json");
            var jsonString5 = System.IO.File.ReadAllText(@".\Resources\GenericDetails5.json");
            var jsonString6 = System.IO.File.ReadAllText(@".\Resources\GenericDetails6.json");

            string cardDetails = jsonString1 + location + jsonString2 + forecastDate + jsonString3 + conditionsURL + jsonString4 + currentTemp + jsonString5 + currentConditions + jsonString6;

            System.IO.File.WriteAllText(@".\Resources\" + detailsFile, cardDetails);

            // combine path for cross platform support
            string[] paths = { ".", "Resources", detailsFile };
            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }

        //Finds the current conditions from provided forecast information.
        private static string FindCurrentConditions(JObject json)
        {
            string currentConditions = (string)json["weather"][0]["description"];
            string currentSkies = (string)json["weather"][0]["main"];
            // See if skies are just partly cloudy.
            if(currentSkies == "Clouds" && ((currentConditions == "few clouds")||(currentConditions == "scattered clouds")))
            {
                currentSkies = currentConditions;
            }
            return currentSkies;
        }

        // find URL to online image for current weather
        private static string FindConditionsURL(string conditions)
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
                case "few clouds":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Mostly Cloudy-Square.png";
                    break;
                case "scattered clouds":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Mostly Cloudy-Square.png";
                    break;
                case "rain":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Drizzle-Square.png";
                    break;
                case "snow":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Snow-Square.png";
                    break;
                case "thunderstorm":
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Drizzle-Square.png";
                    break;
                default:
                    conditionsURL = "http://messagecardplayground.azurewebsites.net/assets/Sunny-Square.png";
                    break;
            }

            return conditionsURL;
        }

        private static string FindCurrentTemp(JObject json)
        {
            return KelvinToFahrenheit((double)json["main"]["temp"]);
        }

        // Converts from Kelvin to Fahrenheit.
        private static string KelvinToFahrenheit(double kelvin)
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


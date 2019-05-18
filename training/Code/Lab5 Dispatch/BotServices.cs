// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;

namespace DispatchWeatherBot
{
    public class BotServices : IBotServices
    {
        public BotServices(IConfiguration configuration)
        {
            // Read the setting for cognitive services (LUIS, QnA) from the appsettings.json
            Dispatch = new LuisRecognizer(new LuisApplication(
                configuration["DispatchLuisAppId"],
                configuration["DispatchLuisAPIKey"],
                $"https://{configuration["DispatchLuisAPIHostName"]}.api.cognitive.microsoft.com"),
                new LuisPredictionOptions { IncludeAllIntents = true, IncludeInstanceData = true },
                true);

            SampleQnA = new QnAMaker(new QnAMakerEndpoint
            {
                KnowledgeBaseId = configuration["QnAKnowledgebaseId"],
                EndpointKey = configuration["QnAAuthKey"],
                Host = configuration["QnAEndpointHostName"]
            });

            Forecasts = new LuisRecognizer(new LuisApplication(
                configuration["LuisAppId"],
                configuration["LuisAPIKey"],
                $"https://{configuration["LuisAPIHostName"]}.api.cognitive.microsoft.com"),
                new LuisPredictionOptions { IncludeAllIntents = true, IncludeInstanceData = true },
                true);
        }
        public LuisRecognizer Dispatch { get; private set; }
        public LuisRecognizer Forecasts { get; private set; }
        public QnAMaker SampleQnA { get; private set; }
    }
}

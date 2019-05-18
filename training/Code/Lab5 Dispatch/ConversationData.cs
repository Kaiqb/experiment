using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DispatchWeatherBot
{
    public class ConversationData
    {
        // The time-stamp of the most recent incoming message.
        public string Timestamp { get; set; } = string.Empty;

        // The ID of the user's channel.
        public string ChannelId { get; set; } = string.Empty;

        // topIntent detected by Dispatch.
        public string DispatchIntent { get; set; } = string.Empty;

        // topIntent detected by LUIS.
        public string LuisIntent { get; set; } = string.Empty;

        // type of weather condition requested.
        // blank when requesting daily weather forecast.
        public string Condition { get; set; } = string.Empty;

        // request for sun status.
        // blank when requesting some weather forecasts.
        public string Sun { get; set; } = string.Empty;

        // location for weather condition requested.
        public string Location { get; set; } = string.Empty;
    }
}


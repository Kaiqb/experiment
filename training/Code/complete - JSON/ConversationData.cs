using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLP_With_Dispatch_Bot
{
    public class ConversationData
    {
        // The time-stamp of the most recent incoming message.
        public string Timestamp { get; set; }

        // The ID of the user's channel.
        public string ChannelId { get; set; }

        // topIntent detected by Dispatch.
        public string DispatchIntent { get; set; }

        // topIntent detected by LUIS.
        public string LuisIntent { get; set; }

        // type of weather condition requested.
        // blank when requesting daily weather forecast.
        public string Condition { get; set; }

        // request for sun status.
        // blank when requesting some weather forecasts.
        public string Sun { get; set; }

        // location for weather condition requested.
        public string Location { get; set; }

    }
}

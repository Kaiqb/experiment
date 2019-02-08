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

    }
}

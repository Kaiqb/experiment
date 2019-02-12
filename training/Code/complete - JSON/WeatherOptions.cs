using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLP_With_Dispatch_Bot
{
    public class WeatherOptions
    {
        // topIntent detected by LUIS.
        public string LuisIntent { get; set; }

        // type of weather condition requested.
        // blank when requesting daily weather forecast.
        public string Condition { get; set; }

        // location for weather condition requested.
        public string Location { get; set; }
    }
}

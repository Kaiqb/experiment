using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromptValidations
{
    public class UserProfile
    {
        public bool IsAnonomous { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public object Media { get; set; }
        public string MediaMimeType { get; set; }
        public string MediaDescription { get; set; }

        public string GetDescription()
        {
            var name = IsAnonomous ? "Anonymous" : Name;
            var summary =
                $"You are '{name}', {Age}, and posting a {MediaMimeType} with a description of '{MediaDescription}'.";
            return summary;
        }
    }
}

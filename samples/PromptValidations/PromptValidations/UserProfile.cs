using System.Collections.Generic;

namespace PromptValidations
{
    /// <summary>
    /// Contains user profile information for this bot.
    /// </summary>
    public class UserProfile
    {
        public bool IsAnonomous { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> MediaUrls { get; set; }
        public string MediaMimeType { get; set; }
        public string MediaDescription { get; set; }

        private string MediaSummary =>
            $"{MediaUrls.Count} {MediaMimeType} " +
            (MediaUrls.Count>0 ? "files" : "file") +
            (MediaDescription!=string.Empty? $", described as, '{MediaDescription}'" : ", with no description");

        /// <summary>
        /// Summarizes the information in the profile.
        /// </summary>
        /// <returns>A summary of the user profile.</returns>
        public string GetDescription()
        {
            var name = IsAnonomous ? "Anonymous" : Name;
            var summary =
                $"You are '{name}', {Age}, and posting {MediaSummary}.";
            return summary;
        }
    }
}

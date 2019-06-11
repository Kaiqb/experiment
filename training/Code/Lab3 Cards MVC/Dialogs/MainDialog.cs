// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;


namespace Microsoft.BotBuilderSamples
{
    public class MainDialog : ComponentDialog
    {
        private readonly IConfiguration _configuration;
        protected readonly ILogger _logger;
        private string openMapKey;

        public MainDialog(IConfiguration configuration, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _configuration = configuration;
            _logger = logger;
            
            // Define the main dialog and its related components.
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ChoiceCardStepAsync,
                ShowCardStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        // 1. Prompts the user if the user is not in the middle of a dialog.
        // 2. Re-prompts the user when an invalid input is received.
        private async Task<DialogTurnResult> ChoiceCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.ChoiceCardStepAsync");

            // Create the PromptOptions which contain the prompt and re-prompt messages.
            // PromptOptions also contains the list of choices available to the user.
            var options = new PromptOptions()
            {
                Prompt = MessageFactory.Text("What city would you like to see? You can click or type the city name"),
                RetryPrompt = MessageFactory.Text("That was not a valid choice, please select a city or number from 1 to 5."),
                Choices = GetChoices(),
            };

            // Prompt the user with the configured PromptOptions.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), options, cancellationToken);
        }

        // Send a Rich Card response to the user based on their choice.
        // This method is only called when a valid prompt response is parsed from the user's response to the ChoicePrompt.
        private async Task<DialogTurnResult> ShowCardStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("MainDialog.ShowCardStepAsync");
            // Get your Open Weather Map Key
            openMapKey = _configuration["OpenWeatherMapKey"];
            
            // Reply to the activity we received with an activity.
            var reply = stepContext.Context.Activity.CreateReply();

            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments on the activity.
            reply.Attachments = new List<Attachment>();

            // TEMP - moves call into the code.
            JObject jsonResult = GetForecastInformation(((FoundChoice)stepContext.Result).Value);

            // Decide which type of card(s) we are going to show the user
            switch (((FoundChoice)stepContext.Result).Value)
            {
                // Display an Adaptive Card
                case "Miami":
                    reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("Miami", jsonResult));
                    break;
                case "Chicago":
                    reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("Chicago", jsonResult));
                    break;
                case "Seattle":
                    reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("Seattle", jsonResult));
                    break;
                case "London":
                    reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("London", jsonResult));
                    break;
                case "Sydney":
                    reply.Attachments.Add(Cards.CreateAdaptiveCardAttachment("Sydney", jsonResult));
                    break;
            }

            // Send the card(s) to the user as an attachment to the activity
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            // Give the user instructions about what to do next
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Type anything to see another city."), cancellationToken);

            return await stepContext.EndDialogAsync();
        }

        private IList<Choice> GetChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = "Miami", Synonyms = new List<string>() { "miami" } },
                new Choice() { Value = "Chicago", Synonyms = new List<string>() { "chicago" } },
                new Choice() { Value = "Seattle", Synonyms = new List<string>() { "seattle" } },
                new Choice() { Value = "London", Synonyms = new List<string>() { "london" } },
                new Choice() { Value = "Sydney", Synonyms = new List<string>() { "sydney" } },
            };
            return cardOptions;
        }

        private JObject GetForecastInformation(string locationName)
        {
            string forecastUrl = string.Empty;
            forecastUrl = "http://api.openweathermap.org/data/2.5/weather?q=" + locationName + "&APPID=" + openMapKey;
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



    }
}

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot_PlusPromptValidation
{
    public class MainDialog : ComponentDialog
    {
        private const string AttachmentOption = "Attachment prompt (size, format)";
        private const string DateTimeOption = "DateTime prompt (within past year)";
        private const string NumberOption = "Number prompt (range)";
        private const string TextOption = "Text prompt (name)";
        private const string CancelOption = "Cancel";

        private static IList<Choice> Choices { get; }
            = new List<Choice>
            {
                new Choice
                {
                    Value = AttachmentOption,
                    Synonyms = new List<string> { "attachment", "size", "format", "image" },
                    Action = new CardAction { Type = ActionTypes.ImBack },
                },
                new Choice
                {
                    Value = DateTimeOption,
                    Synonyms = new List<string> { "date", "datetime", "time", "past year", "past" },
                    Action = new CardAction { Type = ActionTypes.ImBack },
                },
                new Choice
                {
                    Value = NumberOption,
                    Synonyms = new List<string> { "number", "range", "age" },
                    Action = new CardAction { Type = ActionTypes.ImBack },
                },
                new Choice
                {
                    Value = TextOption,
                    Synonyms = new List<string> { "text", "name" },
                    Action = new CardAction { Type = ActionTypes.ImBack },
                },
                new Choice
                {
                    Value = CancelOption,
                    Synonyms = new List<string> { "cancel", "stop", "exit" },
                    Action = new CardAction { Type = ActionTypes.ImBack },
                },
            };

        private struct Ids
        {
            public static string MainDialog = "main-dialog";
            public static string ChoicePrompt = "choice-prompt";
            public static string AttachmentPrompt = "image-prompt";
            public static string DateTimePrompt = "date-range-rompt";
            public static string NumberPrompt = "age-range-rompt";
            public static string TextPrompt = "name-rompt";
        }

        public class AttachmentValidationOptions
        {
            public List<string> ValidExtensions { get; set; }
        }

        private static int MaxTries { get; } = 3;

        public MainDialog(string dialogId) : base(dialogId)
        {
            var mainDialog = new WaterfallDialog(Ids.MainDialog, new WaterfallStep[]
            {
                InitialStepAsync,
                PromptStepAsync,
                FinalStepAsync,
            });

            AddDialog(mainDialog);
            AddDialog(new ChoicePrompt(Ids.ChoicePrompt, ChoiceValidatorAsync) { Style = ListStyle.List });
            AddDialog(new AttachmentPrompt(Ids.AttachmentPrompt, ImageValidatorAsync));
            AddDialog(new DateTimePrompt(Ids.DateTimePrompt, DateValidatorAsync));
            AddDialog(new NumberPrompt<int>(Ids.NumberPrompt, AgeValidatorAsync));
            AddDialog(new TextPrompt(Ids.TextPrompt, NameValidatorAsync));

            InitialDialogId = Ids.MainDialog;
        }

        private async Task<DialogTurnResult> InitialStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                Ids.ChoicePrompt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Please choose the type of prompt to display:"),
                    RetryPrompt = MessageFactory.Text("Please choose an option from the list:"),
                    Choices = Choices,
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> PromptStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // Start a prompt based on the user's choice.
            var choice = stepContext.Result as FoundChoice;
            switch (Choices[choice.Index].Value)
            {
                case AttachmentOption:
                    return await stepContext.PromptAsync(
                        Ids.AttachmentPrompt,
                        new PromptOptions
                        {
                            Prompt = MessageFactory.Text("Please include an attachment."),
                        },
                        cancellationToken);
                case DateTimeOption:
                    return await stepContext.PromptAsync(
                        Ids.DateTimePrompt,
                        new PromptOptions
                        {
                            Prompt = MessageFactory.Text("Please enter a date within the past year."),
                        },
                        cancellationToken);
                case NumberOption:
                    return await stepContext.PromptAsync(
                        Ids.NumberPrompt,
                        new PromptOptions
                        {
                            Prompt = MessageFactory.Text("Please enter your age."),
                        },
                        cancellationToken);
                case TextOption:
                    return await stepContext.PromptAsync(
                        Ids.TextPrompt,
                        new PromptOptions
                        {
                            Prompt = MessageFactory.Text("Please enter your name."),
                        },
                        cancellationToken);
                case CancelOption:
                default:
                    return await stepContext.CancelAllDialogsAsync(cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            switch (stepContext.Result)
            {
                case IEnumerable<Attachment> attachments:
                    break;
                case IEnumerable<DateTimeResolution> dates:
                    break;
                case int age:
                    break;
                case string name:
                    break;
                default:
                    break;
            }
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<bool> ChoiceValidatorAsync(
            PromptValidatorContext<FoundChoice> promptContext,
            CancellationToken cancellationToken)
        {
            if (promptContext.AttemptCount == MaxTries && !promptContext.Recognized.Succeeded)
            {
                // Force recognition of the "cancel" option.
                var choice = Choices.First(c => c.Value == CancelOption);
                promptContext.Recognized.Value = new FoundChoice
                {
                    Index = Choices.IndexOf(choice),
                    Value = choice.Value,
                };
                return true;
            }

            return promptContext.Recognized.Succeeded;
        }

        private async Task<bool> ImageValidatorAsync(
            PromptValidatorContext<IList<Attachment>> promptContext,
            CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                return false;
            }

            if (promptContext.Recognized.Value.Count() < 1
                || promptContext.Recognized.Value.Count() > 1)
            {
                await promptContext.Context.SendActivityAsync("Please include one attachment.");
                return false;
            }

            foreach(var attachment in promptContext.Recognized.Value)
            {
                Debug.WriteLine($"Content type: {attachment.ContentType}; URL: `{attachment.ContentUrl}`; name: {attachment.Name}.");
            }

            return true;
        }

        private async Task<bool> DateValidatorAsync(
            PromptValidatorContext<IList<DateTimeResolution>> promptContext,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> AgeValidatorAsync(
            PromptValidatorContext<int> promptContext,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> NameValidatorAsync(
            PromptValidatorContext<string> promptContext,
            CancellationToken cancellationToken)
        {
            if (promptContext.Recognized.Succeeded)
            {
                var name = promptContext.Recognized.Value.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    await promptContext.Context.SendActivityAsync(
                        "Your name must contain at least one non-whitespace character.",
                        cancellationToken: cancellationToken);
                    return false;
                }
                else
                {
                    promptContext.Recognized.Value = name;
                    return true;
                }
            }

            return false;
        }
    }
}

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromptValidations.Dialogs
{
    public class PromptsDialog : ComponentDialog
    {
        private static class Name
        {
            public const string ImageChoice = "Image";
            public const string VideoChoice = "Video";
            public const string NamePrompt = "NamePrompt";
            public const string AgePrompt = "AgePrompt";
            public const string ChoicePrompt = nameof(ChoicePrompt);
            public const string ImagePrompt = "ImagePrompt";
            public const string VideoPrompt = "VideoPrompt";
            public const string DescriptionPrompt = "DescriptionPrompt";
            public const string ConfirmPrompt = nameof(ConfirmPrompt);
            public const string UserProfile = nameof(UserProfile);
            public const string PromptFailure = "PromptFailure";
        }

        private static class Message
        {
            public const string ImageRestrictions = "";
            public const string VideoRestrictions = "";
        }

        private static List<Choice> AttachmentChoices { get; } = new List<Choice>
        {
            new Choice
            {
                Value = Name.ImageChoice,
                Synonyms = new List<string> { "Picture", "Still", "Photo", "Photograph" },
            },
            new Choice
            {
                Value = Name.VideoChoice,
                Synonyms = new List<string> { "Clip" },
            },
        };

        private class CustomPromptOptions
        {
            public int MaxRetries { get; set; } = 2;
            public string DefaultErrorMessage { get; set; }
        }

        private class AgeOptions : CustomPromptOptions
        {
            public int Min { get; set; } = 0;
            public int Max { get; set; } = 120;
        }

        private class AttachmentOptions : CustomPromptOptions
        {
            public int MinAttachments { get; set; } = 1;
            public int MaxAttachments { get; set; } = 1;
            public IEnumerable<string> SupportedMimeTypes { get; set; }
        }

        /// <summary>Sets a flag in the turn context to indicate that user input for a prompt has
        /// failed the maximum number of times.</summary>
        /// <typeparam name="T">The prompt type.</typeparam>
        /// <param name="promptContext">The validation context for the prompt validator.</param>
        /// <remarks>Call this from within a validator and return true. Then, in the next logical
        /// step of the waterfall, check <see cref="DidPrmptFail(WaterfallStepContext)"/> to see if
        /// the flag was set. We're using the turn state to communicate this information between the
        /// validator and the next waterfall step. This works because the next waterfall step receives
        /// control on the same turn that the prompt ends.</remarks>
        private static void FlagPromptFailure<T>(PromptValidatorContext<T> promptContext)
        {
            promptContext.Context.TurnState[Name.PromptFailure] = true;
        }

        /// <summary>Checks a flag in the turn context that indicates whether user input for the
        /// preceeding prompt failed.</summary>
        /// <param name="stepContext">The current waterfall step context.</param>
        /// <returns>True if the prompt "completed" due to user input failing the prompt validator's
        /// retry limit.</returns>
        /// <remarks>Call this from within a waterfall step to see if the validator used
        /// <see cref="FlagPromptFailure{T}(PromptValidatorContext{T})"/> to set the flag.
        /// We'reusing the turn state to communicate this information between the validator and the
        /// next waterfall step. This works because the next waterfall step receives control on the
        /// same turn that the prompt ends.</remarks>
        private static bool DidPrmptFail(WaterfallStepContext stepContext)
        {
            return stepContext.Context.TurnState.TryGetValue(Name.PromptFailure, out var obj)
                && obj is bool failure
                && failure;
        }

        /// <summary>Performs a case-insensitive serach to determine whether a list contains a
        /// specific value.</summary>
        /// <param name="list">The list to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <remarks>One might use LUIS for natural langugae support.</remarks>
        private static bool HasMatch(IEnumerable<string> list, string value)
        {
            return list.Any(s => s.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>Phrases to interpret as a user selectiing to be anonymous.</summary>
        private static string[] Anonymous { get; } =
            new string[] { "-", "--", "anon", "anonymous" };

        /// <summary>Phrases to interpret as a user providing no description.</summary>
        private static string[] NoDescription { get; } =
            new string[] { "no", "none" };

        /// <summary>Phrases that should indicate lack of consent.</summary>
        protected static string[] Maybe { get; } =
            new string[] { "maybe", "whatevs", "whatever", "why not" };

        public PromptsDialog(string dialogId) : base(dialogId)
        {
            // Declare the step to use in the waterfall dialog.
            var waterfallSteps = new WaterfallStep[]
            {
                NameStepAsync,
                AgeStepAsync,
                ChoiceStepAsync,
                AttachmentStepAsync,
                DescriptionStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            };

            // Add the waterfall dialog to the component dialog.
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // Add all the prompts used in the waterfall dialog.
            // Include the validators to use with them, as appropriate.
            AddDialog(new TextPrompt(Name.NamePrompt, NameValidatorAsync));
            AddDialog(new NumberPrompt<int>(Name.AgePrompt, AgeValidatorAsync));
            AddDialog(new ChoicePrompt(Name.ChoicePrompt, ChoiceValidatorAsync) { Style = ListStyle.HeroCard });
            AddDialog(new AttachmentPrompt(Name.ImagePrompt, ImageValidatorAsync));
            AddDialog(new AttachmentPrompt(Name.VideoPrompt, VideoValidatorAsync));
            AddDialog(new TextPrompt(Name.DescriptionPrompt));
            AddDialog(new ConfirmPrompt(Name.ConfirmPrompt, ConfirmValidatorAsync));

            // Explicitly set the intial dialog for the component.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> NameStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // Initialize the user profile to fill in.
            stepContext.Values[Name.UserProfile] = new UserProfile();

            // Prompt for the user's name.
            var options = new PromptOptions
            {
                Prompt = MessageFactory.Text("Please enter your name."),
                Validations = new CustomPromptOptions(),
            };

            return await stepContext.PromptAsync(Name.NamePrompt, options, cancellationToken);
        }


        private async Task<DialogTurnResult> AgeStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Name.UserProfile];

            // Record the user's name, or exit.
            var name = (string)stepContext.Result;
            if (name is null)
            {
                profile.IsAnonomous = true;
            }
            else
            {
                profile.IsAnonomous = false;
                profile.Name = name;

            }

            // Prompt for the user's age.
            var options = new PromptOptions
            {
                Prompt = MessageFactory.Text("Please enter your age."),
                Validations = new AgeOptions { Min = 18, Max = 120 },
            };

            return await stepContext.PromptAsync(Name.AgePrompt, options, cancellationToken);
        }

        private async Task<DialogTurnResult> ChoiceStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Name.UserProfile];

            // Record the user's age, or exit.
            switch (stepContext.Result)
            {
                case int age:

                    profile.Age = age;
                    break;

                default:

                    return await stepContext.EndDialogAsync(null, cancellationToken);
            }

            // Prompt the user to select the type of media to send.
            var options = new PromptOptions
            {
                Prompt = MessageFactory.Text("Please choose what type of media to send."),
                Choices = AttachmentChoices,
                Validations = new CustomPromptOptions(),
            };

            return await stepContext.PromptAsync(Name.ChoicePrompt, options, cancellationToken);
        }

        private async Task<DialogTurnResult> AttachmentStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Record the user's selection, or exit.
            switch (stepContext.Result)
            {
                case FoundChoice choice:

                    stepContext.Values[Name.ChoicePrompt] = choice.Value;
                    break;

                default:

                    return await stepContext.EndDialogAsync(null, cancellationToken);
            }

            // Prompt the user to send an attachment.
            switch (stepContext.Values[Name.ChoicePrompt])
            {
                case Name.ImageChoice:

                    var imageOptions = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"Please send the image. {Message.ImageRestrictions}"),
                        Validations = new CustomPromptOptions
                        {
                            DefaultErrorMessage = Message.ImageRestrictions,
                        },
                    };

                    return await stepContext.PromptAsync(Name.ImagePrompt, imageOptions, cancellationToken);

                case Name.VideoChoice:

                    var videoOptions = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"Please send the video. {Message.VideoRestrictions}"),
                        Validations = new CustomPromptOptions
                        {
                            DefaultErrorMessage = Message.VideoRestrictions,
                        },
                    };

                    return await stepContext.PromptAsync(Name.VideoPrompt, videoOptions, cancellationToken);

                default:

                    // Logic error
                    return await stepContext.EndDialogAsync(null, cancellationToken);
            }

        }

        private async Task<DialogTurnResult> DescriptionStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Name.UserProfile];

            // Record the user's attachment, or exit.
            switch (stepContext.Result)
            {
                case IList<Attachment> attachments:

                    // TODO Determine what information to save here.
                    profile.Media = attachments[0].Content;
                    profile.MediaMimeType = attachments[0].ContentType;
                    break;

                default:

                    return await stepContext.EndDialogAsync(null, cancellationToken);
            }

            // Prompt the user to add a description.
            var options = new PromptOptions
            {
                Prompt = MessageFactory.Text("Please add a description. Type `no` or `none` to omit a description."),
                Choices = AttachmentChoices,
                Validations = new CustomPromptOptions(),
            };

            return await stepContext.PromptAsync(Name.DescriptionPrompt, options, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // There's no prompt validator for description.

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Name.UserProfile];

            // Record the user's description, if one was provided.
            switch (stepContext.Result)
            {
                case string value:

                    if (HasMatch(NoDescription, value))
                    {
                        profile.MediaDescription = string.Empty;
                    }
                    else
                    {
                        profile.MediaDescription = value?.Trim() ?? string.Empty;
                    }

                    break;

                default:

                    return await stepContext.EndDialogAsync(null, cancellationToken);
            }

            // Prompt the user to confirm this information.
            var options = new PromptOptions
            {
                Prompt = MessageFactory.Text(profile.GetDescription() + " Is this correct?"),
                Validations = new CustomPromptOptions(),
            };

            return await stepContext.PromptAsync(Name.ConfirmPrompt, options, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Name.UserProfile];

            // Get the user's response.
            var confirmed = (bool)stepContext.Result;

            if (confirmed)
            {
                await stepContext.Context.SendActivityAsync(
                    "Excellent, we'll post that presently.",
                    cancellationToken: cancellationToken);
                return await stepContext.EndDialogAsync(profile, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(
                    "Cancelling.",
                    cancellationToken: cancellationToken);
                return await stepContext.EndDialogAsync(null, cancellationToken);
            }
        }

        private async Task<bool> NameValidatorAsync(
            PromptValidatorContext<string> promptContext,
            CancellationToken cancellationToken)
        {
            var options = promptContext.Options.Validations as CustomPromptOptions;
            var success = promptContext.Recognized.Succeeded;
            var value = promptContext.Recognized.Value?.Trim();

            if (success)
            {
                if (HasMatch(Anonymous, value))
                {
                    promptContext.Recognized.Value = null;
                    promptContext.State["IsAnonymous"] = true;
                }
                else
                {
                    promptContext.Recognized.Value = value;
                }
                return true;
            }

            if (promptContext.AttemptCount >= options.MaxRetries)
            {
                // Fail out gracefully.
                FlagPromptFailure(promptContext);
                return true;
            }

            if (options.DefaultErrorMessage != null)
            {
                await promptContext.Context.SendActivityAsync(
                    options.DefaultErrorMessage,
                    cancellationToken: cancellationToken);
            }
            return false;
        }

        private async Task<bool> AgeValidatorAsync(
            PromptValidatorContext<int> promptContext,
            CancellationToken cancellationToken)
        {
            var options = promptContext.Options.Validations as AgeOptions;
            var success = promptContext.Recognized.Succeeded;
            var value = promptContext.Recognized.Value;

            if (success)
            {
                if (value >= options.Min && value <= options.Max)
                {
                    return true;
                }
            }

            if (promptContext.AttemptCount >= options.MaxRetries)
            {
                // Fail out gracefully.
                FlagPromptFailure(promptContext);
                return true;
            }

            if (options.DefaultErrorMessage != null)
            {
                await promptContext.Context.SendActivityAsync(
                    options.DefaultErrorMessage,
                    cancellationToken: cancellationToken);
            }
            return false;
        }

        private async Task<bool> ChoiceValidatorAsync(
            PromptValidatorContext<FoundChoice> promptContext,
            CancellationToken cancellationToken)
        {
            var options = promptContext.Options.Validations as CustomPromptOptions;
            var success = promptContext.Recognized.Succeeded;
            var value = promptContext.Recognized.Value?.Value;

            if (success)
            {
                return true;
            }

            if (promptContext.AttemptCount >= options.MaxRetries)
            {
                // Fail out gracefully.
                FlagPromptFailure(promptContext);
                return true;
            }

            return false;
        }

        private async Task<bool> ImageValidatorAsync(
            PromptValidatorContext<IList<Attachment>> promptContext,
            CancellationToken cancellationToken)
        {
            var options = promptContext.Options.Validations as CustomPromptOptions;
            var success = promptContext.Recognized.Succeeded;
            var attachments = promptContext.Recognized.Value;

            if (success)
            {
                return true;
            }

            if (promptContext.AttemptCount >= options.MaxRetries)
            {
                // Fail out gracefully.
                FlagPromptFailure(promptContext);
                return true;
            }

            return false;
        }

        private async Task<bool> VideoValidatorAsync(
            PromptValidatorContext<IList<Attachment>> promptContext,
            CancellationToken cancellationToken)
        {
            var options = promptContext.Options.Validations as CustomPromptOptions;
            var success = promptContext.Recognized.Succeeded;
            var attachments = promptContext.Recognized.Value;

            if (success)
            {
                return true;
            }

            if (promptContext.AttemptCount >= options.MaxRetries)
            {
                // Fail out gracefully.
                FlagPromptFailure(promptContext);
                return true;
            }

            return false;
        }

        private async Task<bool> ConfirmValidatorAsync(
            PromptValidatorContext<bool> promptContext,
            CancellationToken cancellationToken)
        {
            var options = promptContext.Options.Validations as CustomPromptOptions;
            var success = promptContext.Recognized.Succeeded;
            var value = promptContext.Recognized.Value;
            var text = promptContext.Context.Activity.Text;

            if (success)
            {
                return true;
            }
            else if (Maybe.Any(s => string.Equals(s, text, StringComparison.InvariantCultureIgnoreCase)))
            {
                promptContext.Recognized.Value = false;
                return true;
            }

            if (promptContext.AttemptCount >= options.MaxRetries)
            {
                // Fail out gracefully.
                FlagPromptFailure(promptContext);
                return true;
            }

            return false;
        }
    }
}

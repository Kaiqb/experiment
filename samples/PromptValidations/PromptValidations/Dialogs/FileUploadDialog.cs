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
    /// <summary>
    /// A dialog for uploading an image or video file.
    /// </summary>
    public class FileUploadDialog : ComponentDialog
    {
        /// <summary>
        /// Contains IDs for choices, dialogs, and step state.
        /// </summary>
        private static class Ids
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

        /// <summary>
        /// Contains boilerplate text and messages for use in prompts and validators.
        /// </summary>
        private static class Messages
        {
            public static readonly string ImageRestrictions = "";
            public static readonly string VideoRestrictions = "";

            public static readonly Activity EnterName = MessageFactory.Text("What is your name?");
            public static readonly Activity EnterAge = MessageFactory.Text("How many years old are you?");
            public static readonly Activity ChooseFileType = MessageFactory.Text("What type of media do you want to upload?");
            public static readonly Activity UploadImage = MessageFactory.Text("Please attach an image file.");
            public static readonly Activity UploadVideo = MessageFactory.Text("Please attach a video file.");
            public static readonly Activity AddDescription = MessageFactory.Text("Enter a description for the file. ()");
            public static readonly Activity ConfirmInformation = MessageFactory.Text("Is this information correct?");
        }

        /// <summary>
        /// Describes synonym lists used by some of the validators.
        /// </summary>
        private static class Synonyms
        {
            /// <summary>Phrases to interpret as a user selectiing to be anonymous.</summary>
            public static string[] Anonymous { get; } =
                new string[] { "-", "--", "anon", "anonymous" };

            /// <summary>Phrases to interpret as a user providing no description for a file.</summary>
            public static string[] NoDescription { get; } =
                new string[] { "no", "none" };

            /// <summary>Phrases to interpret as the user failing to actively opt in.</summary>
            public static string[] Maybe { get; } =
                new string[] { "maybe", "whatevs", "whatever", "why not" };
        }

        /// <summary>
        /// Describes the choices to use with the choice prompt when selecting the type of file to upload.
        /// </summary>
        private static List<Choice> AttachmentChoices { get; } = new List<Choice>
        {
            new Choice
            {
                Value = Ids.ImageChoice,
                Synonyms = new List<string> { "Picture", "Still", "Photo", "Photograph" },
            },
            new Choice
            {
                Value = Ids.VideoChoice,
                Synonyms = new List<string> { "Clip" },
            },
        };

        /// <summary>
        /// Defines standard validation options for all prompt validators in this dialog.
        /// </summary>
        private class ValidationOptions
        {
            public int MaxRetries { get; set; } = 2;
            public string DefaultErrorMessage { get; set; }
        }

        /// <summary>
        /// Defines validation options for the age validator.
        /// </summary>
        private class AgeValidationOptions : ValidationOptions
        {
            public int Min { get; set; } = 0;
            public int Max { get; set; } = 120;
        }

        /// <summary>
        /// Defines validation options for the attachment validators.
        /// </summary>
        private class AttachmentValidationOptions : ValidationOptions
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
            promptContext.Context.TurnState[Ids.PromptFailure] = true;
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
            return stepContext.Context.TurnState.TryGetValue(Ids.PromptFailure, out var obj)
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

        /// <summary>
        /// Creates a new instance of the file upload dialog.
        /// </summary>
        /// <param name="dialogId">The ID to assign to this instance of the dialog.</param>
        public FileUploadDialog(string dialogId) : base(dialogId)
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
            AddDialog(new TextPrompt(Ids.NamePrompt, NameValidatorAsync));
            AddDialog(new NumberPrompt<int>(Ids.AgePrompt, AgeValidatorAsync));
            AddDialog(new ChoicePrompt(Ids.ChoicePrompt, ChoiceValidatorAsync) { Style = ListStyle.Auto });
            AddDialog(new AttachmentPrompt(Ids.ImagePrompt, ImageValidatorAsync));
            AddDialog(new AttachmentPrompt(Ids.VideoPrompt, VideoValidatorAsync));
            AddDialog(new TextPrompt(Ids.DescriptionPrompt));
            AddDialog(new ConfirmPrompt(Ids.ConfirmPrompt, ConfirmValidatorAsync));

            // Explicitly set the intial dialog for the component.
            InitialDialogId = nameof(WaterfallDialog);
        }

        /// <summary>
        /// Step to ask the user for their name.
        /// </summary>
        /// <param name="stepContext">The waterfall step context for the current turn of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>If the task is successful, the result indicates whether the dialog is still
        /// active after the turn has been processed.</remarks>
        private async Task<DialogTurnResult> NameStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // Initialize the user profile to fill in.
            stepContext.Values[Ids.UserProfile] = new UserProfile();

            var options = new PromptOptions
            {
                Prompt = Messages.EnterName,
                Validations = new ValidationOptions(),
            };

            // Prompt for the user's name.
            return await stepContext.PromptAsync(Ids.NamePrompt, options, cancellationToken);
        }

        /// <summary>
        /// Runs custom validation logic for the name prompt.
        /// </summary>
        /// <param name="promptContext">The prompt validation context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>Return <code>true</code> to indicate that the prompt should exit;
        /// otherwise, <code>false</code> to indicate that the prompt should retry.</remarks>
        private async Task<bool> NameValidatorAsync(
            PromptValidatorContext<string> promptContext,
            CancellationToken cancellationToken)
        {
            // Pull key information from the prompt validator context.
            var options = promptContext.Options.Validations as ValidationOptions;
            var success = promptContext.Recognized.Succeeded;
            var value = promptContext.Recognized.Value?.Trim();

            // If the user wishes to be anonymous, allow the prompt to succeed, and return null.
            if (success && HasMatch(Synonyms.Anonymous, value))
            {
                promptContext.Recognized.Value = null;
                return true;
            }

            // If the input is not empty, allow the prompt to succeed, and return the trimmed value.
            if (success && !string.IsNullOrEmpty(value))
            {
                promptContext.Recognized.Value = value;
                return true;
            }

            // Otherwise, this attempt failed.
            return await FailValidation(promptContext, options, cancellationToken);
        }

        /// <summary>
        /// Step to ask the user for their age.
        /// </summary>
        /// <param name="stepContext">The waterfall step context for the current turn of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>If the task is successful, the result indicates whether the dialog is still
        /// active after the turn has been processed.</remarks>
        private async Task<DialogTurnResult> AgeStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // If the previous prompt failed, exit.
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Ids.UserProfile];

            // Record the user's name.
            profile.Name = (string)stepContext.Result;
            profile.IsAnonomous = profile.Name is null;

            var options = new PromptOptions
            {
                Prompt = Messages.EnterAge,
                Validations = new AgeValidationOptions { Min = 18, Max = 120 },
            };

            // Prompt for the user's age.
            return await stepContext.PromptAsync(Ids.AgePrompt, options, cancellationToken);
        }

        /// <summary>
        /// Runs custom validation logic for the age prompt.
        /// </summary>
        /// <param name="promptContext">The prompt validation context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>Return <code>true</code> to indicate that the input is valid; otherwise,
        /// <code>false</code>.</remarks>
        private async Task<bool> AgeValidatorAsync(
            PromptValidatorContext<int> promptContext,
            CancellationToken cancellationToken)
        {
            // Pull key information from the prompt validator context.
            var options = promptContext.Options.Validations as AgeValidationOptions;
            var success = promptContext.Recognized.Succeeded;
            var value = promptContext.Recognized.Value;

            // If the input was recognized as a number and the value falls within the specified range,
            // allow the prompt to succeed, and return the recognized value.
            if (success && value >= options.Min && value <= options.Max)
            {
                return true;
            }

            // Otherwise, this attempt failed.
            return await FailValidation(promptContext, options, cancellationToken);
        }

        /// <summary>
        /// Step to ask the user for the type of file they wish to upload.
        /// </summary>
        /// <param name="stepContext">The waterfall step context for the current turn of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>If the task is successful, the result indicates whether the dialog is still
        /// active after the turn has been processed.</remarks>
        private async Task<DialogTurnResult> ChoiceStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // If the previous prompt failed, exit.
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Ids.UserProfile];

            // Record the user's age.
            profile.Age = (int)stepContext.Result;

            var options = new PromptOptions
            {
                Prompt = Messages.ChooseFileType,
                Choices = AttachmentChoices,
                Validations = new ValidationOptions(),
            };

            // Ask the user what type of media they want to upload.
            return await stepContext.PromptAsync(Ids.ChoicePrompt, options, cancellationToken);
        }

        /// <summary>
        /// Runs custom validation logic for the media type choice prompt.
        /// </summary>
        /// <param name="promptContext">The prompt validation context.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>Return <code>true</code> to indicate that the input is valid; otherwise,
        /// <code>false</code>.</remarks>
        private async Task<bool> ChoiceValidatorAsync(
            PromptValidatorContext<FoundChoice> promptContext,
            CancellationToken cancellationToken)
        {
            // Pull key information from the prompt validator context.
            var options = promptContext.Options.Validations as ValidationOptions;
            var success = promptContext.Recognized.Succeeded;

            // If the input was recognized as a valid choice, allow the prompt to succeed, and
            // return the recognized value.
            if (success)
            {
                return true;
            }

            // Otherwise, this attempt failed.
            return await FailValidation(promptContext, options, cancellationToken);
        }

        /// <summary>
        /// Step to ask the user to upload the file.
        /// </summary>
        /// <param name="stepContext">The waterfall step context for the current turn of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>If the task is successful, the result indicates whether the dialog is still
        /// active after the turn has been processed.</remarks>
        private async Task<DialogTurnResult> AttachmentStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // If the previous prompt failed, exit.
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Record the user's selection, or exit.
            switch (stepContext.Result)
            {
                case FoundChoice choice:

                    stepContext.Values[Ids.ChoicePrompt] = choice.Value;
                    break;

                default:

                    return await stepContext.EndDialogAsync(null, cancellationToken);
            }

            // Prompt the user to send an attachment.
            switch (stepContext.Values[Ids.ChoicePrompt])
            {
                case Ids.ImageChoice:

                    var imageOptions = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"Please send the image. {Messages.ImageRestrictions}"),
                        Validations = new ValidationOptions
                        {
                            DefaultErrorMessage = Messages.ImageRestrictions,
                        },
                    };

                    return await stepContext.PromptAsync(Ids.ImagePrompt, imageOptions, cancellationToken);

                case Ids.VideoChoice:

                    var videoOptions = new PromptOptions
                    {
                        Prompt = MessageFactory.Text($"Please send the video. {Messages.VideoRestrictions}"),
                        Validations = new ValidationOptions
                        {
                            DefaultErrorMessage = Messages.VideoRestrictions,
                        },
                    };

                    return await stepContext.PromptAsync(Ids.VideoPrompt, videoOptions, cancellationToken);

                default:

                    // Logic error
                    return await stepContext.EndDialogAsync(null, cancellationToken);
            }

        }

        private async Task<bool> ImageValidatorAsync(
            PromptValidatorContext<IList<Attachment>> promptContext,
            CancellationToken cancellationToken)
        {
            var options = promptContext.Options.Validations as ValidationOptions;
            var success = promptContext.Recognized.Succeeded;
            var attachments = promptContext.Recognized.Value;

            if (success)
            {
                return true;
            }

            // Otherwise, this attempt failed.
            return await FailValidation(promptContext, options, cancellationToken);
        }

        private async Task<bool> VideoValidatorAsync(
            PromptValidatorContext<IList<Attachment>> promptContext,
            CancellationToken cancellationToken)
        {
            var options = promptContext.Options.Validations as ValidationOptions;
            var success = promptContext.Recognized.Succeeded;
            var attachments = promptContext.Recognized.Value;

            if (success)
            {
                return true;
            }

            // Otherwise, this attempt failed.
            return await FailValidation(promptContext, options, cancellationToken);
        }

        /// <summary>
        /// Step to ask the user for an optional file description.
        /// </summary>
        /// <param name="stepContext">The waterfall step context for the current turn of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>If the task is successful, the result indicates whether the dialog is still
        /// active after the turn has been processed.</remarks>
        private async Task<DialogTurnResult> DescriptionStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Ids.UserProfile];

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
                Validations = new ValidationOptions(),
            };

            return await stepContext.PromptAsync(Ids.DescriptionPrompt, options, cancellationToken);
        }

        /// <summary>
        /// Step to ask the user to confirm the collected information.
        /// </summary>
        /// <param name="stepContext">The waterfall step context for the current turn of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>If the task is successful, the result indicates whether the dialog is still
        /// active after the turn has been processed.</remarks>
        private async Task<DialogTurnResult> ConfirmStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // There's no prompt validator for description.

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Ids.UserProfile];

            // Record the user's description, if one was provided.
            switch (stepContext.Result)
            {
                case string value:

                    if (HasMatch(Synonyms.NoDescription, value))
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
                Validations = new ValidationOptions(),
            };

            return await stepContext.PromptAsync(Ids.ConfirmPrompt, options, cancellationToken);
        }

        private async Task<bool> ConfirmValidatorAsync(
            PromptValidatorContext<bool> promptContext,
            CancellationToken cancellationToken)
        {
            var options = promptContext.Options.Validations as ValidationOptions;
            var success = promptContext.Recognized.Succeeded;
            var value = promptContext.Recognized.Value;
            var text = promptContext.Context.Activity.Text;

            if (success)
            {
                return true;
            }
            else if (Synonyms.Maybe.Any(s => string.Equals(s, text, StringComparison.InvariantCultureIgnoreCase)))
            {
                promptContext.Recognized.Value = false;
                return true;
            }

            // Otherwise, this attempt failed.
            return await FailValidation(promptContext, options, cancellationToken);
        }

        /// <summary>
        /// Final step of the dialog. If everything is in order, return the collected profile information;
        /// otherwise, return null.
        /// </summary>
        /// <param name="stepContext">The waterfall step context for the current turn of the conversation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>If the task is successful, the result indicates whether the dialog is still
        /// active after the turn has been processed.</remarks>
        private async Task<DialogTurnResult> FinalStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            if (DidPrmptFail(stepContext))
            {
                return await stepContext.EndDialogAsync(false, cancellationToken);
            }

            // Get the user profile for the dialog.
            var profile = (UserProfile)stepContext.Values[Ids.UserProfile];

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

        /// <summary>
        /// Signals that the prompt input failed recognition or validation, and allows the prompt to
        /// fail gracefully after too many attempts.
        /// </summary>
        /// <param name="promptContext">The prompt validation context.</param>
        /// <param name="options">The validation options for the prompt.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>Return <code>true</code> to indicate that the prompt should fail gracefully;
        /// otherwise, <code>false</code> to indicate that the prompt should retry.
        /// <para>This contains code common to all prompt validators in this dialog, and allows
        /// each prompt to "fail gracefully".</para></remarks>
        private static async Task<bool> FailValidation<T>(
            PromptValidatorContext<T> promptContext,
            ValidationOptions options,
            CancellationToken cancellationToken)
        {
            // If the input failed too many times, allow the prompt to succeed, but set a failure flag.
            // This allows the prompt dialog to exit and return control to the parent dialog.
            if (promptContext.AttemptCount >= options.MaxRetries)
            {
                FlagPromptFailure(promptContext);
                return true;
            }

            // If we have a default error message, send that to the user.
            if (options.DefaultErrorMessage != null)
            {
                await promptContext.Context.SendActivityAsync(
                    options.DefaultErrorMessage,
                    cancellationToken: cancellationToken);
            }

            // Signal that this attempt failed, and that the user should be reprompted for input.
            return false;
        }
    }
}

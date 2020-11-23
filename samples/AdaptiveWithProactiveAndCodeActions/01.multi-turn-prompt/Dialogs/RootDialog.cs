using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Generators;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Builder.LanguageGeneration;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdaptiveScratchBot
{
    public class RootDialog : AdaptiveDialog
    {
        private const string _codeHandlerCountdown = "dialog.CodeHandlerTest_Countdown";
        private const string _codeHandlerOptions = "dialog.CodeHandlerTest_Options";
        private const string _defaultContinuation = nameof(ContinuationTypes.CompleteAndWait);

        private static readonly string fullPath = Path.Combine(".", "Dialogs", $"RootDialog.lg");

        // Name of the dialogTurnResult value to use when exiting the code action.
        private string ResultToUse { get; set; }

        public ConcurrentDictionary<string, ConversationReference> ConversationReferences { get; }

        public RootDialog(ConcurrentDictionary<string, ConversationReference> conversationReferences) : base(nameof(RootDialog))
        {
            ConversationReferences = conversationReferences;

            Generator = new TemplateEngineLanguageGenerator(Templates.ParseFile(fullPath));
            AutoEndDialog = false;

            ChoiceInput _promptForInputForCodeHandler = new ChoiceInput
            {
                AllowInterruptions = "true",
                AlwaysPrompt = "true",
                Property = _codeHandlerOptions,
                Prompt = new ActivityTemplate("#2 How would you like to continue?"),
                InvalidPrompt = new ActivityTemplate(
                    "I'm sorry, that's not a valid choice. Please choose one of the suggested dialog turn status values."),
                DefaultValue = _defaultContinuation,
                DefaultValueResponse = new ActivityTemplate($"Defaulting to `{_defaultContinuation}`."),
                MaxTurnCount = 2,
                Choices = new ChoiceSet
                {
                    new Choice { Value = nameof(ContinuationTypes.WaitAndRepeat) },
                    new Choice { Value = nameof(ContinuationTypes.RepeatImmediately) },
                    new Choice { Value = nameof(ContinuationTypes.CompleteAndWait) },
                    new Choice { Value = nameof(ContinuationTypes.CompleteAndContinue) },
                    new Choice { Value = nameof(ContinuationTypes.CancelAll) },
                },
            };

            base.Triggers = new List<OnCondition>()
                {
                    new OnConversationUpdateActivity
                    {
                        // These steps are executed when this Adaptive Dialog begins
                        Actions = new List<Dialog>
                        {
                            // Auto-subscribe every user to proactive notifications.
                            new CodeAction(UpdateConversationsDictionaryAsync),
                            new SendActivity("Welcome message."),
                        },
                    },
                    new OnEventActivity
                    {
                        // Handle a proactive message as an interrupt.
                        Condition = "equals(turn.activity.name, 'ContinueConversation')",
                        Actions = new List<Dialog>
                        {
                            new SendActivity(
                                    "Received a proactive event with a value of (${turn.activity.value})."),
                            // This ends the enclosing adaptive dialog.
                            // Without it, the "interruption" ends, but the stack waits for more input before continuing.
                            // How, though, do we let the interrupt end in such a way that the next action runs on the same turn?
                            // new EndDialog(),
                        },
                    },
                    new OnEventActivity
                    {
                        Actions = new List<Dialog>
                        {
                            new IfCondition
                            {
                                Condition = "equals(turn.activity.name, 'ContinueConversation')",
                                Actions = new List<Dialog>
                                {
                                    new SendActivity(
                                    "Received a proactive event in the wrong trigger with a value of (${turn.activity.value})."),
                                },
                                ElseActions = new List<Dialog>
                                {
                                    new SendActivity(
                                        "Received an event named (${turn.activity.name}), with a value of (${turn.activity.value})."),
                                },
                            },
                        },
                    },
                    new OnUnknownIntent
                    {
                        Actions =
                        {
                            new SendActivity("#1 This is the first step of this trigger."),
                            _promptForInputForCodeHandler,
                            new SetProperty
                            {
                                Property = _codeHandlerCountdown,
                                Value = 2,
                            },
                            new CodeAction(TestFlowCodeHandlerAsync),
                            new SendActivity
                            {
                                Activity = new ActivityTemplate("#3 This is the last step of this trigger."),

                                // This appears to render incorrectly as, "1 This is the last step of this trigger."
                                //Activity = new ActivityTemplate("3) This is the last step of this trigger."),
                            },
                        },
                    }
                };
        }

        private async Task<DialogTurnResult> UpdateConversationsDictionaryAsync(DialogContext dc, object options)
        {
            // This assumes that every conversation is a 1-1 conversation.
            var conversationReference = dc.Context.Activity.GetConversationReference();
            ConversationReferences.AddOrUpdate(
                conversationReference.User.Id, conversationReference, (key, newValue) => conversationReference);
            return await dc.EndDialogAsync(null, default);
        }

        private static async Task<DialogTurnResult> TestFlowCodeHandlerAsync(DialogContext dc, object options)
        {
            var continuationValue = dc.State.GetValue(_codeHandlerOptions, () => "badValue");
            var count = dc.State.GetValue<int>(_codeHandlerCountdown, () => 2);
            var activities = new List<Activity>
            {
                MessageFactory.Text($"Entering the code action, with {_codeHandlerOptions} = `{continuationValue}`.")
            };
            try
            {
                var continuation = (ContinuationTypes)Enum.Parse(typeof(ContinuationTypes), continuationValue);
                switch (continuation)
                {
                    case ContinuationTypes.WaitAndRepeat:
                        activities.Add(MessageFactory.Text(
                            "This ends the turn and then repeats the code action twice before exiting."));
                        dc.State.SetValue(_codeHandlerCountdown, --count);
                        if (count > 0)
                        {
                            await dc.Context.SendActivitiesAsync(activities.ToArray(), default);
                            // This would make more sense if this action prompted for input.
                            return await dc.ReplaceDialogAsync(dc.ActiveDialog.Id, options, default);
                        }
                        else
                        {
                            goto case ContinuationTypes.CompleteAndWait;
                        }

                    case ContinuationTypes.RepeatImmediately:
                        activities.Add(MessageFactory.Text(
                            "This repeats the code action without ending the turn twice before exiting."));
                        dc.State.SetValue(_codeHandlerCountdown, --count);
                        if (count > 0)
                        {
                            await dc.Context.SendActivitiesAsync(activities.ToArray(), default);
                            // Don't do this.
                            return await TestFlowCodeHandlerAsync(dc, options);
                        }
                        else
                        {
                            goto case ContinuationTypes.CompleteAndContinue;
                        }

                    case ContinuationTypes.CompleteAndWait:
                        activities.Add(MessageFactory.Text("This ends the code action and the turn."));
                        await dc.Context.SendActivitiesAsync(activities.ToArray(), default);
                        return EndOfTurn;

                    case ContinuationTypes.CompleteAndContinue:
                        activities.Add(MessageFactory.Text("This ends the code action without ending the turn."));
                        await dc.Context.SendActivitiesAsync(activities.ToArray(), default);
                        return await dc.EndDialogAsync(null, default);

                    case ContinuationTypes.CancelAll:
                        activities.Add(MessageFactory.Text("This clears the dialog stack and ends the turn."));
                        await dc.Context.SendActivitiesAsync(activities.ToArray(), default);
                        return await dc.CancelAllDialogsAsync(default);

                    default:
                        // We should never get here, as the ChoiceInput doubles as validation.
                        activities.Add(MessageFactory.Text(
                            "Shouldn't get here. The adapter should catch an exception, " +
                            "clearing dialog state and forcing a restart of the conversation."));
                        await dc.Context.SendActivitiesAsync(activities.ToArray(), default);
                        return EndOfTurn;
                }
            }
            catch
            {
                // We should never get here, either, as the ChoiceInput doubles as validation.
                activities.Add(MessageFactory.Text(
                    "Shouldn't get here. The adapter should catch an exception, " +
                    "clearing dialog state and forcing a restart of the conversation."));
                await dc.Context.SendActivitiesAsync(activities.ToArray(), default);
                throw;
            }
        }
    }
}

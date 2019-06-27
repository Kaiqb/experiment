using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace PromptValidations.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        protected readonly UserState _userState;
        protected readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;
        protected readonly ILogger Logger;

        public MainDialog(UserState userState, ILogger<MainDialog> logger) : base(nameof(MainDialog))
        {
            _userState = userState;
            _userProfileAccessor = userState.CreateProperty<UserProfile>(nameof(UserProfile));

            var waterfallSteps = new WaterfallStep[]
            {
                FirstStepAsync,
                LastStepAsync,
            };

            AddDialog(new FileUploadDialog(nameof(FileUploadDialog)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FirstStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Let's get started.");
            return await stepContext.BeginDialogAsync(nameof(FileUploadDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> LastStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            switch (stepContext.Result)
            {
                case UserProfile profile:

                    // On success, the file upload dialog returns a user profile object.
                    await _userProfileAccessor.SetAsync(stepContext.Context, profile, cancellationToken);
                    await _userState.SaveChangesAsync(stepContext.Context, false, cancellationToken);

                    await stepContext.Context.SendActivityAsync($"Thanks {profile.Name}.");
                    break;

                case bool success:

                    // On failure, the file upload dialog returns false (a Boolean object).
                    await stepContext.Context.SendActivityAsync("Operation cancelled.");
                    break;

                default:

                    await stepContext.Context.SendActivityAsync("Thanks for participating.");
                    break;
            }

            return await stepContext.ReplaceDialogAsync(nameof(WaterfallDialog));
        }
    }
}

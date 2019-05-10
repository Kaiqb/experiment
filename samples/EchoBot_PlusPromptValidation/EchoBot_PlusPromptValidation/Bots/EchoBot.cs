// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.3.0

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace EchoBot_PlusPromptValidation.Bots
{
    public class EchoBot : ActivityHandler
    {
        private ConversationState ConversationState { get; set; }
        private MainDialog MainDialog { get; set; }
        private IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }

        public EchoBot(ConversationState conversationState, MainDialog mainDialog)
        {
            Contract.Requires(mainDialog != null);
            Contract.Requires(conversationState != null);

            MainDialog = mainDialog;
            ConversationState = conversationState;
            DialogStateAccessor = ConversationState.CreateProperty<DialogState>("dialogState");
        }

        public override async Task OnTurnAsync(
            ITurnContext turnContext,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            await ConversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(
            ITurnContext<IMessageActivity> turnContext,
            CancellationToken cancellationToken)
        {
            //await MainDialog.RunAsync(turnContext, DialogStateAccessor, cancellationToken);
            var dialogSet = new DialogSet(DialogStateAccessor);
            dialogSet.Add(MainDialog);

            var dc = await dialogSet.CreateContextAsync(turnContext, cancellationToken).ConfigureAwait(false);
            var results = await dc.ContinueDialogAsync(cancellationToken).ConfigureAwait(false);
            if (results.Status == DialogTurnStatus.Empty)
            {
                await dc.BeginDialogAsync(MainDialog.Id, null, cancellationToken).ConfigureAwait(false);
            }
        }

        protected override async Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded,
            ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and Welcome!"), cancellationToken);
                }
            }
        }
    }
}

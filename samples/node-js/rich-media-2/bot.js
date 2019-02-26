// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

const { ActivityTypes, MessageFactory } = require('botbuilder');

const { Attachments } = require('./attachments');

const attachmentMap = {
    'In-line Attachment': Attachments.getInlineAttachment(),
    'Internet Attachment': Attachments.getInternetAttachment(),
};

const keys = Object.keys(attachmentMap);

const prompt2 = MessageFactory.suggestedActions(keys, 'Please choose the type of card to display.');

class MyBot {
    /**
     *
     * @param {TurnContext} on turn context object.
     */
    async onTurn(turnContext) {
        // See https://aka.ms/about-bot-activity-message to learn more about the message and other activity types.
        if (turnContext.activity.type === ActivityTypes.Message) {
            const input = turnContext.activity.text || '';
            let cardType = null;
            for (i = 0; i < keys.length; i++) {
                if (keys[i].toLocaleLowerCase() == input.toLocaleLowerCase()) {
                    cardType = keys[i];
                    break;
                }
            }

            if (cardType) {
                await turnContext.sendActivity(MessageFactory.attachment(attachmentMap[cardType]));
            }

            await turnContext.sendActivity(prompt2);
        }
    }
}

module.exports.MyBot = MyBot;

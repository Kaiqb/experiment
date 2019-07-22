// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

const COUNT = 'COUNT_PROPERTY';

const { ActivityHandler } = require('botbuilder');


class MyBot extends ActivityHandler {
    constructor(conversationState, userState) {
        super();

        this.conversationState = conversationState;
        this.userState = userState;

        this.countAccessor = this.conversationState.createProperty(COUNT);

        // See https://aka.ms/about-bot-activity-message to learn more about the message and other activity types.
        this.onMessage(async (context, next) => {
            const count = await this.countAccessor.get(context, 1);
            await context.sendActivity(`${count} You said '${ context.activity.text }'`);
            // By calling next() you ensure that the next BotHandler is run.
            await this.countAccessor.set(context, count + 1);

            await this.conversationState.saveChanges(context);
            await this.userState.saveChanges(context);

            await next();
        });

        this.onMembersAdded(async (context, next) => {
            const membersAdded = context.activity.membersAdded;
            for (let cnt = 0; cnt < membersAdded.length; ++cnt) {
                if (membersAdded[cnt].id !== context.activity.recipient.id) {
                    await context.sendActivity('Hello and welcome!');
                }
            }
            // By calling next() you ensure that the next BotHandler is run.
            await next();
        });
    }
}

module.exports.MyBot = MyBot;

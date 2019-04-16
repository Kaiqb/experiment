// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

const { QnAMaker, QnAMakerEndPoint } = require('botbuilder-ai');

class QnAHelper{
   // @param {TurnContext} context
   static async executeQnAQuery(context,logger)
   {
       try {
           qnaEndPoint = new QnAMaker.QnAMakerEndPoint({
            knowledgeBaseId: process.env.kbId,
            endpointKey: process.env.endpointKey,
            host: process.env.hostname
           })
       }
       catch (err) {
          logger.warn(`QnA Maker Exception: ${ err } Check your QnA Maker configuration`);
       }

       try {
            qnaMaker = new QnAMaker(qnaEndPoint)
       }
       catch (err) {
         logger.warn(`QnA Maker Exception: ${ err } Check your QnA Maker configuration`);
       }
       const qnaResult = await qnaMaker.getAnswers(context);

       return qnaResult;

    }

}

module.exports.QnAHelper = QnAHelper;


// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

const { ActivityTypes } = require('botbuilder');

class Attachments {
    getInlineAttachment() {
        const imageData = fs.readFileSync(path.join(__dirname, '/resources/logo.png'));
        const base64Image = Buffer.from(imageData).toString('base64');

        return {
            name: 'Bot logo',
            contentType: 'image/png',
            contentUrl: `data:image/png;base64,${base64Image}`
        };
    }


    /**
     * Returns an attachment to be sent to the user from a HTTPS URL.
     * Content URLs must use HTTPS.
     */
    getInternetAttachment() {
        return {
            name: 'Architecture resize',
            contentType: 'image/png',
            contentUrl: 'https://docs.microsoft.com/azure/bot-service/media/bot-service-overview.png'
        };
    }
}

module.exports.Attachments = Attachments;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace RichMedia
{
    using System.Collections.Generic;
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json;

    /// <summary>Contains attachments for sample rich media cards.</summary>
    public static class Attachments
    {
        /// <summary>A sample Adaptive Weather card.</summary>
        public static Attachment ChicagoCardAttachment =>
            new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(
                    System.IO.File.ReadAllText(@".\Resources\ChicagoDetails.json")),
            };

        /// <summary>A sample Adaptive Weather card.</summary>
        public static Attachment LondonCardAttachment =>
            new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(
                    System.IO.File.ReadAllText(@".\Resources\LondonDetails.json")),
            };

        /// <summary>A sample Adaptive Weather card.</summary>
        public static Attachment MiamiCardAttachment =>
            new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(
                    System.IO.File.ReadAllText(@".\Resources\MiamiDetails.json")),
            };

        /// <summary>A sample Adaptive Weather card.</summary>
        public static Attachment SeattleCardAttachment =>
            new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(
                    System.IO.File.ReadAllText(@".\Resources\SeattleDetails.json")),
            };

        /// <summary>A sample Adaptive Weather card.</summary>
        public static Attachment SydneyCardAttachment =>
            new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(
                    System.IO.File.ReadAllText(@".\Resources\SydneyDetails.json")),
            };
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace RichMedia
{
    using System.Collections.Generic;
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json;

    /// <summary>Contains attachments for sample rich media cards.</summary>
    /// <seealso cref="https://docs.microsoft.com/adaptive-cards/"/>
    /// <seealso cref="https://github.com/Microsoft/BotBuilder/blob/master/specs/botframework-activity/botframework-cards.md"/>
    public static class Attachments
    {
        /// <summary>A sample Adaptive card.</summary>
        public static Attachment SampleAdaptiveCardAttachment =>
            new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(
                    System.IO.File.ReadAllText(@".\Resources\FlightDetails.json")),
            };

        /// <summary>A sample hero card.</summary>
        public static Attachment SampleHeroCard =>
                new HeroCard
                {
                    Title = "BotFramework Hero Card",
                    Subtitle = "Microsoft Bot Framework",
                    Text = "Build and connect intelligent bots to interact with your users naturally wherever they are," +
                       " from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                    Images = new List<CardImage>
                    {
                        new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg"),
                    },
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.OpenUrl, "Get Started",
                            value: "https://docs.microsoft.com/bot-framework"),
                    },
                }.ToAttachment();

        /// <summary>A sample thumbnail card.</summary>
        public static Attachment SampleThumbnailCard =>
            new ThumbnailCard
            {
                Title = "BotFramework Thumbnail Card",
                Subtitle = "Microsoft Bot Framework",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are," +
                   " from text/sms to Skype, Slack, Office 365 mail and other popular services.",
                Images = new List<CardImage>
                {
                    new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg")
                },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "Get Started",
                        value: "https://docs.microsoft.com/bot-framework")
                },
            }.ToAttachment();

        /// <summary>A sample animation card.</summary>
        public static Attachment SampleAnimationCard =>
            new AnimationCard
            {
                Title = "Microsoft Bot Framework",
                Subtitle = "Animation Card",
                Image = new ThumbnailUrl
                {
                    Url = "https://docs.microsoft.com/en-us/bot-framework/media/how-it-works/architecture-resize.png",
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://i.giphy.com/Ki55RUbOV5njy.gif",
                    },
                },
            }.ToAttachment();

        /// <summary>A sample audio card.</summary>
        public static Attachment SampleAudioCard =>
            new AudioCard
            {
                Title = "I am your father",
                Subtitle = "Star Wars: Episode V - The Empire Strikes Back",
                Text = "The Empire Strikes Back (also known as Star Wars: Episode V – The Empire Strikes Back)" +
                       " is a 1980 American epic space opera film directed by Irvin Kershner. Leigh Brackett and" +
                       " Lawrence Kasdan wrote the screenplay, with George Lucas writing the film's story and serving" +
                       " as executive producer. The second installment in the original Star Wars trilogy, it was produced" +
                       " by Gary Kurtz for Lucasfilm Ltd. and stars Mark Hamill, Harrison Ford, Carrie Fisher, Billy Dee Williams," +
                       " Anthony Daniels, David Prowse, Kenny Baker, Peter Mayhew and Frank Oz.",
                Image = new ThumbnailUrl
                {
                    Url = "https://upload.wikimedia.org/wikipedia/en/3/3c/SW_-_Empire_Strikes_Back.jpg",
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://www.wavlist.com/movies/004/father.wav",
                    },
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = "Read More",
                        Type = ActionTypes.OpenUrl,
                        Value = "https://en.wikipedia.org/wiki/The_Empire_Strikes_Back",
                    },
                },
            }.ToAttachment();

        /// <summary>A sample video card.</summary>
        public static Attachment SampleVideoCard =>
            new VideoCard
            {
                Title = "Big Buck Bunny",
                Subtitle = "by the Blender Institute",
                Text = "Big Buck Bunny (code-named Peach) is a short computer-animated comedy film by the Blender Institute," +
                    " part of the Blender Foundation. Like the foundation's previous film Elephants Dream," +
                    " the film was made using Blender, a free software application for animation made by the same foundation." +
                    " It was released as an open-source film under Creative Commons License Attribution 3.0.",
                Image = new ThumbnailUrl
                {
                    Url = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c5/Big_buck_bunny_poster_big.jpg/220px-Big_buck_bunny_poster_big.jpg",
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = "http://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4",
                    },
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = "Learn More",
                        Type = ActionTypes.OpenUrl,
                        Value = "https://peach.blender.org/",
                    },
                },
            }.ToAttachment();

        /// <summary>A sample receipt card.</summary>
        public static Attachment SampleReceiptCard =>
            new ReceiptCard
            {
                Title = "John Doe",
                Facts = new List<Fact>
                {
                    new Fact("Order Number", "1234"),
                    new Fact("Payment Method", "VISA 5555-****"),
                },
                Items = new List<ReceiptItem>
                {
                    new ReceiptItem(
                        "Data Transfer",
                        price: "$ 38.45",
                        quantity: "368",
                        image: new CardImage(
                            url: "https://github.com/amido/azure-vector-icons/raw/master/renders/traffic-manager.png")),
                    new ReceiptItem(
                        "App Service",
                        price: "$ 45.00",
                        quantity: "720",
                        image: new CardImage(
                            url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
                },
                Tax = "$ 7.50",
                Total = "$ 90.95",
                Buttons = new List<CardAction>
                {
                    new CardAction
                    {
                        Type = ActionTypes.OpenUrl,
                        Text = "More information", Title = "More information",
                        Value = "https://azure.microsoft.com/en-us/pricing/",
                    },
                },
            }.ToAttachment();

        /// <summary>A sample sign-in card.</summary>
        public static Attachment SampleSigninCard =>
            new SigninCard
            {
                Text = "BotFramework Sign-in Card",
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.Signin, "Sign-in",
                        value: "https://login.microsoftonline.com/")
                },
            }.ToAttachment();
    }
}

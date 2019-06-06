using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubReports
{
    internal static class TextAnalyticsConstants
    {
        public struct EndPoint
        {
            public const string Sentiment = "sentiment";
            public const string KeyPhrase = "keyPhrases";
            public const string Language = "languages";
            public const string Entities = "entities";
        }

        public const string SubscriptionKey = "";
    }

    public class TextAnalyticsService
    {
        public string Location { get; }

        public string ServiceEndpoint => $"https://{Location}.api.cognitive.microsoft.com";
        public string BaseUri => $"https://{Location}.api.cognitive.microsoft.com/text/analytics/v2.1";

        private TextAnalyticsClient Client { get; }

        public TextAnalyticsService(string location)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(location));

            Location = location.Trim();

            ServiceClientCredentials credentials =
                new ApiKeyServiceClientCredentials(TextAnalyticsConstants.SubscriptionKey);
            Client = new TextAnalyticsClient(credentials)
            {
                Endpoint = ServiceEndpoint,
            };
        }

        public async Task<SentimentBatchResult> SentimentAnalysis(List<MultiLanguageInput> input)
        {
            var result = await Client.SentimentAsync(false, new MultiLanguageBatchInput(input));
            return result;
        }
    }

    /// <summary>
    /// Allows authentication to the API by using a basic apiKey mechanism
    /// </summary>
    class ApiKeyServiceClientCredentials : ServiceClientCredentials
    {
        private readonly string subscriptionKey;

        /// <summary>
        /// Creates a new instance of the ApiKeyServiceClientCredentails class
        /// </summary>
        /// <param name="subscriptionKey">The subscription key to authenticate and authorize as</param>
        public ApiKeyServiceClientCredentials(string subscriptionKey)
        {
            this.subscriptionKey = subscriptionKey;
        }

        /// <summary>
        /// Add the Basic Authentication Header to each outgoing request
        /// </summary>
        /// <param name="request">The outgoing request</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            request.Headers.Add("Ocp-Apim-Subscription-Key", this.subscriptionKey);
            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}

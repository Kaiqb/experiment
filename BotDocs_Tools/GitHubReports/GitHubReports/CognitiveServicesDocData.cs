using Newtonsoft.Json;
using System.Collections.Generic;

namespace GitHubReports
{
    /// <summary>Represents the payload object for Cognitive Service key phrase and sentiment analysis.</summary>
    /// <remarks>You can have up to 1,000 items (IDs) per collection.
    /// For more information about limits, see
    /// [Text Analytics Overview > Data limits](https://docs.microsoft.com/en-us/azure/cognitive-services/text-analytics/overview#data-limits).
    /// <para>Resource endpoints are as follows (your region may vary):<list type="bullet">
    /// <item>https://westus.api.cognitive.microsoft.com/text/analytics/v2.1/sentiment</item>
    /// <item>https://westus.api.cognitive.microsoft.com/text/analytics/v2.1/keyPhrases</item>
    /// <item>https://westus.api.cognitive.microsoft.com/text/analytics/v2.1/languages</item>
    /// <item>https://westus.api.cognitive.microsoft.com/text/analytics/v2.1/entities</item>
    /// </list></para>
    /// <para>Calls take three headers:<list type="bullet">
    /// <item>`Ocp-Apim-Subscription-Key`: your access key, obtained from Azure portal.</item>
    /// <item>`Content-Type`: application/json.</item>
    /// <item>`Accept`: application/json.</item>
    /// </list></para>
    /// </remarks>
    public class CognitiveServicesDocData
    {
        /// <summary></summary>
        /// <remarks></remarks>
        [JsonProperty("documents")]
        public List<CognitiveServicesDoc> Docs { get; set; }
    }

    /// <summary>Describes a document (text) to analyze.</summary>
    /// <remarks>Document size must be under 5,120 characters per document.</remarks>
    public class CognitiveServicesDoc
    {
        /// <summary>A 2-character ISO 639-1 code indicating the language of the text to analyze.</summary>
        /// <remarks>Required for sentiment analysis, key phrase extraction, and entity linking;
        /// optional for language detection. There is no error if you exclude it, but the analysis is
        /// weakened without it. The language code should correspond to the text you provide.</remarks>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>The document ID.</summary>
        /// <remarks>The system uses the IDs you provide to structure the output. Language codes, 
        /// key phrases, and sentiment scores are generated for each ID in the request.</remarks>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>The document (text) to analyze.</summary>
        /// <remarks>For language detection, text can be expressed in any language. For sentiment analysis,
        /// key phrase extraction and entity identification, the text must be in a supported language.</remarks>
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

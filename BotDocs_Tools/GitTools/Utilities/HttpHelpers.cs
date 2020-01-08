using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utilities
{
    public static class HttpHelpers
    {
        public static async Task<(HttpStatusCode statusCode, string errorMessage)> TestUriAsync(
            this HttpClient client,
            string uri)
        {
            try
            {
                var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));
                var content = result.Content.ReadAsStringAsync();
                return (result.StatusCode, result.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return (HttpStatusCode.BadRequest, $"{ex.GetType().Name}: {ex.Message}");
            }
        }

        public static (string Status, string Reason) TestUrl(string url) => TestUrl(new Uri(url));
        public static (string Status, string Reason) TestUrl(Uri url)
        {
            using (var client = new HttpClient(new HttpClientHandler()
            { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = url,
                    Method = HttpMethod.Get,
                };

                try
                {
                    var response = client.SendAsync(request).Result;
                    var statusCode = (int)response.StatusCode;
                    var reason = response.ReasonPhrase;
                    // We want to handle redirects ourselves so that we can determine the final redirect Location (via header)
                    if (statusCode >= 300 && statusCode <= 399)
                    {
                        var redirectUri = response.Headers.Location;
                        if (!redirectUri.IsAbsoluteUri)
                        {
                            redirectUri = new Uri(request.RequestUri.GetLeftPart(UriPartial.Authority) + redirectUri);
                        }
                        var r2 = TestUrl(redirectUri);
                        reason = $"{statusCode} Redirect to {redirectUri} > ({r2.Status}) {r2.Reason}";
                    }
                    return (statusCode.ToString(), reason);
                }
                catch (Exception ex)
                {
                    return ("400", $"{ex.GetType().Name}: {ex.Message}");
                }
            }
        }
    }
}

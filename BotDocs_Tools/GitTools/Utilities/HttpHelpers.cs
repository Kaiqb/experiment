using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utilities
{
    public class UrlTestResult
    {
        public UrlTestResult(string url, string target, string status, string reason) =>
            (Url, Target, Status, Reason) = (url, target, status, reason);

        public string Url { get; private set; }
        public string Target { get; private set; }
        public string Status { get; private set; }
        public string Reason { get; private set; }
    }
    public static class HttpHelpers
    {
        public static UrlTestResult TestUrl(string url) => TestUrl(new Uri(url));
        public static UrlTestResult TestUrl(Uri url)
        {
            using (var client = new HttpClient(new HttpClientHandler()
            { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = url,
                    Method = HttpMethod.Get,
                };

                Uri target = url;
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
                    return new UrlTestResult(url.AbsolutePath, target.AbsolutePath, statusCode.ToString(), reason);
                }
                catch (Exception ex)
                {
                    return new UrlTestResult(url.AbsolutePath, target.AbsolutePath, "400", $"{ex.GetType().Name}: {ex.Message}");
                }
            }
        }
    }
}

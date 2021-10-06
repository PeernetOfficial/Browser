using Peernet.Browser.Application.Http;
using System.Net.Http;

namespace Peernet.Browser.Infrastructure
{
    public abstract class ServiceBase
    {
        protected ServiceBase(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateHttpClient();
        }

        public abstract string CoreSegment { get; }
        public HttpClient HttpClient { get; }

        protected string GetRelativeRequestPath(string consecutiveSegments)
        {
            return string.IsNullOrEmpty(consecutiveSegments) ? CoreSegment : $"{CoreSegment}/{consecutiveSegments}";
        }
    }
}
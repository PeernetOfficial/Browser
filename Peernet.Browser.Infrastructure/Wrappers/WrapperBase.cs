using Peernet.Browser.Application.Http;
using System.Net.Http;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public abstract class WrapperBase
    {
        protected WrapperBase(IHttpClientFactory httpClientFactory)
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
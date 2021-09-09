using Peernet.Browser.Application.Http;
using RestSharp;
using System;

namespace Peernet.Browser.Infrastructure
{
    public abstract class ServiceBase
    {
        protected ServiceBase(IRestClientFactory restClientFactory)
        {
            RestClient = restClientFactory.CreateRestClient();
        }

        public RestClient RestClient { get; }

        public abstract string CoreSegment { get; }

        protected Uri GetRelativeRequestPath(string consecutiveSegments)
        {
            return new Uri($"{CoreSegment}/{consecutiveSegments}", UriKind.Relative);
        }
    }
}

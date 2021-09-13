using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Services;
using RestSharp;
using System;

namespace Peernet.Browser.Infrastructure
{
    public abstract class ServiceBase
    {
        protected readonly ICmdClient cmdClient;

        protected ServiceBase(IRestClientFactory restClientFactory, ICmdClient cmdClient)
        {
            this.cmdClient = cmdClient;
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
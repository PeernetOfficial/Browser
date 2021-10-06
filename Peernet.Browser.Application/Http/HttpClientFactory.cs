using Peernet.Browser.Application.Services;
using System;
using System.Net.Http;
using Peernet.Browser.Application.Managers;

namespace Peernet.Browser.Application.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly ISettingsManager settingsManager;

        public HttpClientFactory(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(settingsManager.ApiUrl);

            return client;
        }
    }
}
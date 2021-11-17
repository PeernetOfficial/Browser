using Peernet.Browser.Application.Managers;
using System;
using System.Net.Http;

namespace Peernet.Browser.Infrastructure.Http
{
    internal class HttpClientFactory
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
            client.DefaultRequestHeaders.Add("x-api-key", settingsManager.ApiKey);
            return client;
        }
    }
}
using Peernet.Browser.Application.Serializers;
using Peernet.Browser.Application.Services;
using RestSharp;

namespace Peernet.Browser.Application.Http
{
    public class RestClientFactory : IRestClientFactory
    {
        private readonly ISettingsManager settingsManager;

        public RestClientFactory(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public RestClient CreateRestClient()
        {
            var client = new RestClient(settingsManager.ApiUrl);
            client.AddHandler("application/json", () => new CustomSerializer());

            return client;
        }
    }
}
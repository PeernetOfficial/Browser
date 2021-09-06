using Peernet.Browser.Application.Services;
using RestSharp;

namespace Peernet.Browser.Application.Http
{
    public class RestClientFactory : IRestClientFactory
    {
        private ISettingsManager settingsManager;

        public RestClientFactory(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public RestClient CreateRestClient()
        {
            return new RestClient(settingsManager.ApiUrl);
        }
    }
}

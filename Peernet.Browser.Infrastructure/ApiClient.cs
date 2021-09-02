using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class ApiClient : IApiClient
    {
        private readonly IRestClient restClient;
        
        public ApiClient(IRestClient restClient, ISettingsManager settings)
        {
            this.restClient = restClient;
            this.restClient.BaseUrl = new Uri(settings.ApiUrl);
        }

        public async Task<MyInfo> GetMyInfo()
        {
            RestRequest request = new("peer/self", DataFormat.Json);
            return await restClient.GetAsync<MyInfo>(request);
        }

        public Task<Status> GetStatus()
        {
            try
            {
                RestRequest request = new("status", DataFormat.Json);
                return restClient.GetAsync<Status>(request);
            }
            catch (System.Net.WebException)
            {
                return Task.FromResult<Status>(new Status());
            }
        }
    }
}

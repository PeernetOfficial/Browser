using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using RestSharp;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class ApiClient : IApiClient
    {
        private readonly IRestClient restClient;

        public ApiClient(IRestClient restClient)
        {
            this.restClient = restClient;
            this.restClient.BaseHost = "127.0.0.1:112";
        }

        public async Task<MyInfo> GetMyInfo()
        {
            var request = new RestRequest("peer/self", DataFormat.Json);
            return await restClient.GetAsync<MyInfo>(request);
        }

        public async Task<Status> GetStatus()
        {
            var request = new RestRequest("status", DataFormat.Json);
            return await restClient.GetAsync<Status>(request);
        }
    }
}

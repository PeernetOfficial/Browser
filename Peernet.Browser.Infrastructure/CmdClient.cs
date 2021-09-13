using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using RestSharp;
using System;

namespace Peernet.Browser.Infrastructure
{
    public class CmdClient : ICmdClient
    {
        private readonly IRestClient restClient;

        public CmdClient(IRestClientFactory restClientFactory, ISettingsManager settings)
        {
            restClient = restClientFactory.CreateRestClient();
        }

        public void AddFiles(ApiBlockRecordFile[] files)
        {
            try
            {
                var request = new RestRequest("blockchain/self/add/file", DataFormat.Json);
                request.AddParameter("Files", files);
                restClient.Post(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public MyInfo GetMyInfo() => GetResult<MyInfo>("peer/self");

        public Status GetStatus() => GetResult<Status>("status");

        private T GetResult<T>(string method)
        {
            try
            {
                var request = new RestRequest(method, DataFormat.Json);
                return restClient.Get<T>(request).Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default(T);
            }
        }
    }
}
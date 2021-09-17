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

        public ApiBlockchainBlockStatus AddFiles(ApiBlockchainAddFiles files)
        {
            try
            {
                var request = new RestRequest("blockchain/self/add/file", DataFormat.Json);
                request.AddJsonBody(files);
                var response = restClient.Post<ApiBlockchainBlockStatus>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public ApiResponsePeerSelf GetMyInfo() => GetResult<ApiResponsePeerSelf>("peer/self");

        public ApiResponseStatus GetStatus() => GetResult<ApiResponseStatus>("status") ?? new ApiResponseStatus();

        public ApiBlockchainBlock ReadBlock(int block)
        {
            return GetResult<ApiBlockchainBlock>($"/blockchain/self/read?block={block}");
        }

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
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return PostResult<ApiBlockchainBlockStatus>("blockchain/self/add/file", files);
        }

        public ApiResponsePeerSelf GetMyInfo() => GetResult<ApiResponsePeerSelf>("peer/self");

        public ApiResponseStatus GetStatus() => GetResult<ApiResponseStatus>("status") ?? new ApiResponseStatus();

        public ApiBlockchainBlock ReadBlock(int block)
        {
            return GetResult<ApiBlockchainBlock>($"/blockchain/self/read?block={block}");
        }

        public SearchResult ResturnSearch(string id, int? limit = null)
        {
            var p = new Dictionary<string, object>();
            p.Add("id", id);
            if (limit.HasValue) p.Add("limit", limit.Value);
            return GetResult<SearchResult>("/search/result", p);
        }

        public SearchRequestResponse SubmitSearch(SearchRequest request)
        {
            return PostResult<SearchRequestResponse>("/search", request);
        }

        public void TerminateSearch(string id)
        {
            GetResult<string>($"/search/terminate?id={id}");
        }

        private T GetResult<T>(string method, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            try
            {
                var request = new RestRequest(method, DataFormat.Json);
                if (!parameters.IsNullOrEmpty()) parameters.Foreach(x => request.AddParameter(x.Key, x.Value));
                return restClient.Get<T>(request).Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default(T);
            }
        }

        private T PostResult<T>(string method, object parameter)
        {
            try
            {
                var request = new RestRequest(method, DataFormat.Json);
                request.AddJsonBody(parameter);
                var response = restClient.Post<T>(request);
                return response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default(T);
            }
        }
    }
}
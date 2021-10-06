using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Peernet.Browser.Application.Helpers;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class DownloadWrapper : WrapperBase, IDownloadWrapper
    {
        private const string ActionSegment = "action";
        private const string StartSegment = "start";
        private const string StatusSegment = "status";

        public DownloadWrapper(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        public override string CoreSegment => "download";

        public async Task<ApiResponseDownloadStatus> GetAction(string id, DownloadAction action)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id,
                [nameof(action)] = ((int)action).ToString()
            };

            return await HttpHelper.GetResult<ApiResponseDownloadStatus>(HttpClient, HttpMethod.Get, GetRelativeRequestPath(ActionSegment), parameters);
        }

        public async Task<ApiResponseDownloadStatus> GetStatus(string id)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id
            };

            return await HttpHelper.GetResult<ApiResponseDownloadStatus>(HttpClient, HttpMethod.Get, GetRelativeRequestPath(StatusSegment), parameters);
        }

        public async Task<ApiResponseDownloadStatus> Start(string path, byte[] hash, byte[] node)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(path)] = path,
                [nameof(hash)] = Convert.ToHexString(hash),
                [nameof(node)] = Convert.ToHexString(node)
            };

            return await HttpHelper.GetResult<ApiResponseDownloadStatus>(HttpClient, HttpMethod.Get, GetRelativeRequestPath(StartSegment), parameters);
        }
    }
}
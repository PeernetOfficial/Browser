using System;
using Peernet.Browser.Application.Helpers;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class DownloadService : ServiceBase, IDownloadService
    {
        private const string ActionSegment = "action";
        private const string StartSegment = "start";
        private const string StatusSegment = "status";

        public DownloadService(IHttpClientFactory httpClientFactory)
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
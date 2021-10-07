using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class DownloadWrapper : WrapperBase, IDownloadWrapper
    {
        private const string ActionSegment = "action";
        private const string StartSegment = "start";
        private const string StatusSegment = "status";

        private readonly IHttpExecutor httpExecutor;

        public DownloadWrapper(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
        }

        public override string CoreSegment => "download";

        public async Task<ApiResponseDownloadStatus> GetAction(string id, DownloadAction action)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id,
                [nameof(action)] = ((int)action).ToString()
            };

            return await httpExecutor.GetResult<ApiResponseDownloadStatus>(HttpMethod.Get, GetRelativeRequestPath(ActionSegment), parameters);
        }

        public async Task<ApiResponseDownloadStatus> GetStatus(string id)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(id)] = id
            };

            return await httpExecutor.GetResult<ApiResponseDownloadStatus>(HttpMethod.Get, GetRelativeRequestPath(StatusSegment), parameters);
        }

        public async Task<ApiResponseDownloadStatus> Start(string path, byte[] hash, byte[] node)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(path)] = path,
                [nameof(hash)] = Convert.ToHexString(hash),
                [nameof(node)] = Convert.ToHexString(node)
            };

            return await httpExecutor.GetResult<ApiResponseDownloadStatus>(HttpMethod.Get, GetRelativeRequestPath(StartSegment), parameters);
        }
    }
}
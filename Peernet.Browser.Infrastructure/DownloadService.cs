using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using RestSharp;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class DownloadService : ServiceBase, IDownloadService
    {
        private const string StartSegment = "start";
        private const string StatusSegment = "status";

        public DownloadService(IRestClientFactory restClientFactory, ICmdClient cmdClient)
            : base(restClientFactory, cmdClient)
        {
        }

        public override string CoreSegment => "download";

        public ApiResponseDownloadStatus GetStatus(string hash, string blockchain)
        {
            var request = new RestRequest(GetRelativeRequestPath(StatusSegment), Method.GET);
            request.AddQueryParameter(nameof(hash), hash);
            request.AddQueryParameter(nameof(blockchain), blockchain);

            return Task.Run(async () => await RestClient.GetAsync<ApiResponseDownloadStatus>(request)).GetResultBlockingWithoutContextSynchronization();
        }

        public ApiResponseDownloadStatus Start(string path, string hash, string blockchain)
        {
            var request = new RestRequest(GetRelativeRequestPath(StartSegment), Method.GET);
            request.AddQueryParameter(nameof(path), path);
            request.AddQueryParameter(nameof(hash), hash);
            request.AddQueryParameter(nameof(blockchain), blockchain);

            return Task.Run(async () => await RestClient.GetAsync<ApiResponseDownloadStatus>(request)).GetResultBlockingWithoutContextSynchronization();
        }
    }
}
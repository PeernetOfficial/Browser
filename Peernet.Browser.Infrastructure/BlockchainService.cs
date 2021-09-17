using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class BlockchainService : ServiceBase, IBlockchainService
    {
        public BlockchainService(IRestClientFactory restClientFactory, ICmdClient cmdClient)
            : base(restClientFactory, cmdClient)
        {
        }

        public override string CoreSegment => "blockchain/self";

        public ApiBlockchainHeader GetSelfHeader()
        {
            var request = new RestRequest(GetRelativeRequestPath("header"), Method.GET);

            return Task.Run(() => RestClient.GetAsync<ApiBlockchainHeader>(request)).GetResultBlockingWithoutContextSynchronization();
        }

        public ApiBlockchainAddFiles GetSelfList()
        {
            var request = new RestRequest(GetRelativeRequestPath("list/file"), Method.GET);

            return Task.Run(() => RestClient.GetAsync<ApiBlockchainAddFiles>(request)).GetResultBlockingWithoutContextSynchronization();
        }

        public void AddFiles(IEnumerable<SharedNewFileModel> files)
        {
            var data = files
                .Select(x =>
                    new ApiBlockRecordFile
                    {
                        Description = x.Desc,
                        Name = x.FileName,
                        Folder = x.Directory,
                        Date = System.DateTime.Now
                    })
                .ToList();
            var res = cmdClient.AddFiles(new ApiBlockchainAddFiles { Files = data });
        }

        public void DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile)
        {
            var request = new RestRequest(GetRelativeRequestPath("delete/file"), Method.POST);
            request.AddJsonBody(apiBlockRecordFile);

            Task.Run(() => RestClient.PostAsync<ApiBlockchainAddFiles>(request)).GetResultBlockingWithoutContextSynchronization();
        }
    }
}
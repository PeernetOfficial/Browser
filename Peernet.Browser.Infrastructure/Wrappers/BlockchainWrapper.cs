using Peernet.Browser.Application.Helpers;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public class BlockchainWrapper : WrapperBase, IBlockchainWrapper
    {
        public BlockchainWrapper(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        public override string CoreSegment => "blockchain/self";

        public async Task<ApiBlockchainBlockStatus> AddFiles(ApiBlockchainAddFiles files)
        {
            return await HttpHelper.GetResult<ApiBlockchainBlockStatus>(HttpClient, HttpMethod.Post, GetRelativeRequestPath("add/file"));
        }

        public async Task DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile)
        {
            var content = JsonContent.Create(new ApiBlockchainAddFiles { Files = new List<ApiBlockRecordFile> { apiBlockRecordFile } });
            await HttpHelper.GetResult<ApiBlockchainBlockStatus>(HttpClient, HttpMethod.Post, GetRelativeRequestPath("delete/file"), content: content);
        }

        public async Task<ApiBlockchainHeader> GetSelfHeader()
        {
            return await HttpHelper.GetResult<ApiBlockchainHeader>(HttpClient, HttpMethod.Get, GetRelativeRequestPath("header"));
        }

        public async Task<ApiBlockchainAddFiles> GetSelfList()
        {
            return await HttpHelper.GetResult<ApiBlockchainAddFiles>(HttpClient, HttpMethod.Get, GetRelativeRequestPath("list/file"));
        }

        public async Task<ApiBlockchainBlock> ReadBlock(int block)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(block)] = block.ToString()
            };

            return await HttpHelper.GetResult<ApiBlockchainBlock>(HttpClient, HttpMethod.Get,
                GetRelativeRequestPath("read"),
                parameters);
        }
    }
}
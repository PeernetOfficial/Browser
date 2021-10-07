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
        private readonly IHttpExecutor httpExecutor;

        public BlockchainWrapper(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
        }

        public override string CoreSegment => "blockchain/self";

        public async Task<ApiBlockchainBlockStatus> AddFiles(ApiBlockchainAddFiles files)
        {
            return await httpExecutor.GetResult<ApiBlockchainBlockStatus>(HttpMethod.Post, GetRelativeRequestPath("add/file"));
        }

        public async Task DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile)
        {
            var content = JsonContent.Create(new ApiBlockchainAddFiles { Files = new List<ApiBlockRecordFile> { apiBlockRecordFile } });
            await httpExecutor.GetResult<ApiBlockchainBlockStatus>(HttpMethod.Post, GetRelativeRequestPath("delete/file"), content: content);
        }

        public async Task<ApiBlockchainHeader> GetSelfHeader()
        {
            return await httpExecutor.GetResult<ApiBlockchainHeader>(HttpMethod.Get, GetRelativeRequestPath("header"));
        }

        public async Task<ApiBlockchainAddFiles> GetSelfList()
        {
            return await httpExecutor.GetResult<ApiBlockchainAddFiles>(HttpMethod.Get, GetRelativeRequestPath("list/file"));
        }

        public async Task<ApiBlockchainBlock> ReadBlock(int block)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(block)] = block.ToString()
            };

            return await httpExecutor.GetResult<ApiBlockchainBlock>(HttpMethod.Get,
                GetRelativeRequestPath("read"),
                parameters);
        }
    }
}
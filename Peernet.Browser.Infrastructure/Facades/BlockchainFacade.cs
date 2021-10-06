using Peernet.Browser.Application.Facades;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Facades
{
    public class BlockchainFacade : IBlockchainFacade
    {
        private readonly IBlockchainWrapper blockchainWrapper;

        public BlockchainFacade(IBlockchainWrapper blockchainWrapper)
        {
            this.blockchainWrapper = blockchainWrapper;
        }

        // todo: it should consume some presentation model
        public async Task DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile)
        {
            await blockchainWrapper.DeleteSelfFile(apiBlockRecordFile);
        }

        public async Task<ApiBlockchainHeader> GetSelfHeader()
        {
            return await blockchainWrapper.GetSelfHeader();
        }

        public async Task<List<ApiBlockRecordFile>> GetSelfList()
        {
            return (await blockchainWrapper.GetSelfList()).Files;
        }

        public async Task AddFilesAsync(IEnumerable<SharedNewFileModel> files)
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

            await blockchainWrapper.AddFiles(new ApiBlockchainAddFiles { Files = data });
        }
    }
}
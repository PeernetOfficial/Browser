using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Peernet.Browser.Application.Facades;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.Infrastructure.Facades
{
    public class BlockchainFacade : IBlockchainFacade
    {
        private readonly IBlockchainService blockchainService;

        public BlockchainFacade(IBlockchainService blockchainService)
        {
            this.blockchainService = blockchainService;
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

            await blockchainService.AddFiles(new ApiBlockchainAddFiles { Files = data });
        }
    }
}

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
        private readonly IBlockchainWrapper blockchainService;

        public BlockchainFacade(IBlockchainWrapper blockchainService)
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
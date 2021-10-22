using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Presentation.Footer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    public class BlockchainService : IBlockchainService
    {
        private readonly IBlockchainClient blockchainClient;

        public BlockchainService(ISettingsManager settingsManager)
        {
            blockchainClient = new BlockchainClient(settingsManager);
        }

        // todo: it should consume some presentation model
        public async Task DeleteSelfFile(ApiFile apiFile)
        {
            await blockchainClient.DeleteSelfFile(apiFile);
        }

        public async Task<ApiBlockchainHeader> GetSelfHeader()
        {
            return await blockchainClient.GetSelfHeader();
        }

        public async Task<List<ApiFile>> GetSelfList()
        {
            return (await blockchainClient.GetSelfList()).Files;
        }

        public async Task AddFiles(IEnumerable<SharedNewFileModel> files)
        {
            var data = files
                .Select(x =>
                    new ApiFile
                    {
                        Description = x.Desc ?? string.Empty,
                        Name = x.FileName,
                        Folder = x.Directory,
                        Date = System.DateTime.Now,
                        Hash = x.Hash,
                        MetaData = new List<ApiFileMetadata>()
                    })
                .ToList();

            await blockchainClient.AddFiles(new ApiBlockchainAddFiles { Files = data });
        }
    }
}
using System;
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

        public async Task<ApiBlockchainBlockStatus> DeleteFile(ApiFile apiFile)
        {
            return await blockchainClient.DeleteFile(apiFile);
        }

        public async Task<ApiBlockchainBlockStatus> UpdateFile(FileModel fileModel)
        {
            var apiFile = new ApiFile
            {
                Id = fileModel.Id,
                Hash = fileModel.Hash,
                MetaData = fileModel.Metadata,
                NodeId = fileModel.NodeId,
                Name = fileModel.FileName,
                Description = fileModel.Description,
                Folder = fileModel.Directory,
                Date = fileModel.CreateDate,
                Type = fileModel.Type,
                Format = fileModel.Format
            };

            return await blockchainClient.UpdateFile(apiFile);
        }

        public async Task<ApiBlockchainHeader> GetHeader()
        {
            return await blockchainClient.GetHeader();
        }

        public async Task<List<ApiFile>> GetList()
        {
            return (await blockchainClient.GetList())?.Files ?? new List<ApiFile>();
        }

        public async Task<ApiBlockchainBlockStatus> AddFiles(IEnumerable<FileModel> files)
        {
            var data = files
                .Select(file =>
                    new ApiFile
                    {
                        Description = file.Description ?? string.Empty,
                        Name = file.FileName,
                        Folder = file.Directory,
                        Date = DateTime.Now,
                        Hash = file.Hash,
                        MetaData = new List<ApiFileMetadata>(),
                        Format = file.Format,
                        Type = file.Type
                    })
                .ToList();

            return await blockchainClient.AddFiles(new ApiBlockchainAddFiles { Files = data });
        }
    }
}
using Peernet.Browser.Application.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.Blockchain;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class BlockchainService : IBlockchainService
    {
        private readonly IBlockchainClient blockchainClient;
        private readonly IExploreClient exploreClient;

        public BlockchainService(IBlockchainClient blockchainClient, IExploreClient exploreClient)
        {
            this.blockchainClient = blockchainClient;
            this.exploreClient = exploreClient;
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
                Name = $"{fileModel.FileNameWithoutExtension}{fileModel.Extension}",
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
                        Name = $"{file.FileNameWithoutExtension}{file.Extension}",
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

        public async Task<List<ApiFile>> GetFilesForNode(string nodeId)
        {
            return (await exploreClient.GetFilesForNode(nodeId))?.Files ?? new();
        }
    }
}
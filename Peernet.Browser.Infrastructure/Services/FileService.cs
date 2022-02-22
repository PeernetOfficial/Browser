using Peernet.Browser.Application.Services;
using System.Threading.Tasks;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.File;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class FileService : IFileService
    {
        private readonly IFileClient fileClient;

        public FileService(IFileClient fileClient)
        {
            this.fileClient = fileClient;
        }

        public async Task<ApiResponseFileFormat> GetFormat(string path)
        {
            return await fileClient.GetFormat(path);
        }
    }
}
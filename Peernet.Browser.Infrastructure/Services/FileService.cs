using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.File;
using System.Threading.Tasks;

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
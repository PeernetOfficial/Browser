using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.File;
using System.Threading.Tasks;
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IFileClient fileClient;

        public FileService(ISettingsManager settingsManager)
        {
            fileClient = new FileClient(settingsManager);
        }

        public async Task<ApiResponseFileFormat> GetFormat(string path)
        {
            return await fileClient.GetFormat(path);
        }
    }
}
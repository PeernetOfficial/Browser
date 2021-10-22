using System.IO;
using System.Threading.Tasks;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Warehouse;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Infrastructure.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseClient warehouseClient;

        public WarehouseService(ISettingsManager settingsManager)
        {
            warehouseClient = new WarehouseClient(settingsManager);
        }

        public async Task<WarehouseResult> Create(SharedNewFileModel file)
        {
            var content = await File.ReadAllBytesAsync(file.FullPath, default);
            
            return await warehouseClient.Create(content);
        }
    }
}
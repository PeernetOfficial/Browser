using System.Threading.Tasks;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Warehouse;

namespace Peernet.Browser.Infrastructure.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseClient warehouseClient;

        public WarehouseService(ISettingsManager settingsManager)
        {
            warehouseClient = new WarehouseClient(settingsManager);
        }

        public async Task<WarehouseResult> Create(ApiBlockRecordFile file)
        {
            return await warehouseClient.Create(file);
        }
    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Warehouse;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Infrastructure.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseClient warehouseClient;
        private readonly ISettingsManager settingsManager;

        public WarehouseService(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            warehouseClient = new WarehouseClient(settingsManager);
        }

        public async Task<WarehouseResult> Create(FileModel file)
        {
            var stream = File.OpenRead(file.FullPath);
            return await warehouseClient.Create(stream);
        }

        public async Task<WarehouseResult> ReadPath(ApiFile file)
        {
            var fullPath = Path.Combine(Environment.ExpandEnvironmentVariables(settingsManager.DownloadPath),
                file.Name);
            var result = await warehouseClient.ReadPath(file.Hash, fullPath);
            GlobalContext.Notifications.Add(result.Status != WarehouseStatus.StatusOK
                ? new Notification($"Failed to save file to {fullPath}. Status: {result.Status}", MessagingHelper.GetInOutSummary(file, result), Severity.Error)
                : new Notification($"File saved to {fullPath}"));

            return result;
        }
    }
}
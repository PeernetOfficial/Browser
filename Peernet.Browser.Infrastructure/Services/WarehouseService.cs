using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Warehouse;
using Peernet.Browser.Models.Presentation.Footer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class WarehouseService : IWarehouseService
    {
        private readonly ISettingsManager settingsManager;
        private readonly IWarehouseClient warehouseClient;
        private readonly INotificationsManager notificationsManager;

        public WarehouseService(IWarehouseClient warehouseClient, ISettingsManager settingsManager, INotificationsManager notificationsManager)
        {
            this.settingsManager = settingsManager;
            this.warehouseClient = warehouseClient;
            this.notificationsManager = notificationsManager;
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
            notificationsManager.Notifications.Add(result.Status != WarehouseStatus.StatusOK
                ? new Notification($"Failed to save file to {fullPath}. Status: {result.Status}",
                    MessagingHelper.GetApiSummary($"{nameof(warehouseClient)}.{nameof(warehouseClient.ReadPath)}") +
                    MessagingHelper.GetInOutSummary(file, result), Severity.Error)
                : new Notification($"File saved to {fullPath}"));

            return result;
        }
    }
}
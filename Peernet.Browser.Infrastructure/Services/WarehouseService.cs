using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Client.Http;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Domain.Warehouse;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class WarehouseService : IWarehouseService
    {
        private readonly INotificationsManager notificationsManager;
        private readonly ISettingsManager settingsManager;
        private readonly IWarehouseClient warehouseClient;

        public WarehouseService(IWarehouseClient warehouseClient, ISettingsManager settingsManager, INotificationsManager notificationsManager)
        {
            this.settingsManager = settingsManager;
            this.warehouseClient = warehouseClient;
            this.notificationsManager = notificationsManager;
        }

        public async Task<WarehouseResult> Create(Guid id, FileModel file)
        {
            var stream = File.OpenRead(file.FullPath);
            return await warehouseClient.Create(id, stream);
        }

        public async Task<WarehouseResult> ReadPath(ApiFile file)
        {
            var strippedFileName = UtilityHelper.StripInvalidWindowsCharactersFromFileName(file.Name);
            var fullPath = Path.Combine(settingsManager.DownloadPath, strippedFileName);
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
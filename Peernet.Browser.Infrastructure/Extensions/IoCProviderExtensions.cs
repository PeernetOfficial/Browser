using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Services;

namespace Peernet.Browser.Infrastructure.Extensions
{
    public static class IoCProviderExtensions
    {
        public static void RegisterPeernetServices(this ServiceCollection provider)
        {
            provider.AddSingleton<IDataTransferManager, DataTransferManager>();
            provider.AddTransient<IFileService, FileService>();
            provider.AddTransient<IAccountService, AccountService>();
            provider.AddTransient<IBlockchainService, BlockchainService>();
            provider.AddTransient<IWarehouseService, WarehouseService>();
            provider.AddTransient<IExploreService, ExploreService>();
            provider.AddTransient<IProfileService, ProfileService>();
            provider.AddTransient<ISearchService, SearchService>();
            provider.AddTransient<IShutdownService, ShutdownService>();
            provider.AddTransient<IStatusService, StatusService>();
        }
    }
}
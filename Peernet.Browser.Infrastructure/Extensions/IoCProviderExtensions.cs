using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Infrastructure.Services;

namespace Peernet.Browser.Infrastructure.Extensions
{
    public static class IoCProviderExtensions
    {
        public static void RegisterPeernetServices(this ServiceCollection provider)
        {
            provider.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            provider.AddSingleton<IHttpExecutor, HttpExecutor>();

            provider.AddTransient<IDownloadClient, DownloadClient>();
            provider.AddTransient<IFileClient, FileClient>();
            provider.AddTransient<IAccountClient, AccountClient>();
            provider.AddTransient<IBlockchainClient, BlockchainClient>();
            provider.AddTransient<IWarehouseClient, WarehouseClient>();
            provider.AddTransient<IExploreClient, ExploreClient>();
            provider.AddTransient<IProfileClient, ProfileClient>();
            provider.AddTransient<ISearchClient, SearchClient>();
            provider.AddTransient<IShutdownClient, ShutdownClient>();
            provider.AddTransient<IApiClient, ApiClient>();

            provider.AddSingleton<IDownloadManager, DownloadManager>();
            provider.AddTransient<IFileService, FileService>();
            provider.AddTransient<IAccountService, AccountService>();
            provider.AddTransient<IBlockchainService, BlockchainService>();
            provider.AddTransient<IWarehouseService, WarehouseService>();
            provider.AddTransient<IExploreService, ExploreService>();
            provider.AddTransient<IProfileService, ProfileService>();
            provider.AddTransient<ISearchService, SearchService>();
            provider.AddTransient<IShutdownService, ShutdownService>();
            provider.AddTransient<IApiService, ApiService>();
        }
    }
}

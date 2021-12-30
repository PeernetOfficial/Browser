using MvvmCross.IoC;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Infrastructure.Services;

namespace Peernet.Browser.Infrastructure.Extensions
{
    public static class IoCProviderExtensions
    {
        public static void RegisterPeernetServices(this IMvxIoCProvider provider)
        {
            provider.RegisterType<IHttpClientFactory, HttpClientFactory>();
            provider.RegisterType<IHttpExecutor, HttpExecutor>();

            provider.RegisterType<IDownloadClient, DownloadClient>();
            provider.RegisterType<IFileClient, FileClient>();
            provider.RegisterType<IAccountClient, AccountClient>();
            provider.RegisterType<IBlockchainClient, BlockchainClient>();
            provider.RegisterType<IWarehouseClient, WarehouseClient>();
            provider.RegisterType<IExploreClient, ExploreClient>();
            provider.RegisterType<IProfileClient, ProfileClient>();
            provider.RegisterType<ISearchClient, SearchClient>();
            provider.RegisterType<IShutdownClient, ShutdownClient>();
            provider.RegisterType<IApiClient, ApiClient>();
   
            provider.RegisterType<IDownloadManager, DownloadManager>();
            provider.RegisterType<IFileService, FileService>();
            provider.RegisterType<IAccountService, AccountService>();
            provider.RegisterType<IBlockchainService, BlockchainService>();
            provider.RegisterType<IWarehouseService, WarehouseService>();
            provider.RegisterType<IExploreService, ExploreService>();
            provider.RegisterType<IProfileService, ProfileService>();
            provider.RegisterType<ISearchService, SearchService>();
            provider.RegisterType<IShutdownService, ShutdownService>();
            provider.RegisterType<IApiService, ApiService>();
        }
    }
}

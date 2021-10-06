using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Plugin;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Facades;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Infrastructure;
using Peernet.Browser.Infrastructure.Facades;
using Peernet.Browser.Infrastructure.Wrappers;
using Serilog;
using Serilog.Extensions.Logging;

namespace Peernet.Browser.WPF
{
    public class Setup : MvxWpfSetup<Application.App>
    {
        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);

            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Control.Plugin>(true);
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .CreateLogger();

            return new SerilogLoggerFactory(logger);
        }

        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override void RegisterBindingBuilderCallbacks(IMvxIoCProvider iocProvider)
        {
            // register managers
            CreatableTypes()
                .EndingWith("Manager")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            iocProvider.RegisterType<IHttpClientFactory, HttpClientFactory>();
            iocProvider.RegisterType<IApiWrapper, ApiWrapper>();
            iocProvider.RegisterType<ISocketClient, SocketClient>();
            iocProvider.RegisterType<IProfileWrapper, ProfileWrapper>();
            iocProvider.RegisterType<IProfileFacade, ProfileFacade>();
            iocProvider.RegisterSingleton<IUserContext>(() => new UserContext(iocProvider.Resolve<IProfileFacade>(), iocProvider.Resolve<IMvxNavigationService>()));
            iocProvider.RegisterType<IBlockchainWrapper, BlockchainWrapper>();
            iocProvider.RegisterType<IBlockchainFacade, BlockchainFacade>();
            iocProvider.RegisterType<IVirtualFileSystemFactory, VirtualFileSystemFactory>();
            iocProvider.RegisterType<IFilesToCategoryBinder, FilesToCategoryBinder>();
            iocProvider.RegisterType<IExploreWrapper, ExploreWrapper>();
            iocProvider.RegisterType<IExploreFacade, ExploreFacade>();
            iocProvider.RegisterType<ISearchWrapper, SearchWrapper>();
            iocProvider.RegisterType<ISearchFacade, SearchFacade>();
            iocProvider.RegisterType<IDownloadWrapper, DownloadWrapper>();
            iocProvider.RegisterSingleton<IDownloadManager>(new DownloadManager(iocProvider.Resolve<IDownloadWrapper>()));
            GlobalContext.UiThreadDispatcher = iocProvider.Resolve<IMvxMainThreadAsyncDispatcher>();
            ObserveNavigation(iocProvider);
        }

        private static void ObserveNavigation(IMvxIoCProvider iocProvider)
        {
            iocProvider.Resolve<IMvxNavigationService>().DidNavigate +=
                delegate (object sender, IMvxNavigateEventArgs args)
                {
                    GlobalContext.CurrentViewModel = args.ViewModel.GetType().Name;
                };
        }
    }
}
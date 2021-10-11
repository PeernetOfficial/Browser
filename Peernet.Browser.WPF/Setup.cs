using Microsoft.Extensions.Logging;
using MvvmCross.Base;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Plugin;
using Peernet.Browser.Application.Clients;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Infrastructure;
using Peernet.Browser.Infrastructure.Services;
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

            iocProvider.RegisterType<IApiService, ApiService>();
            iocProvider.RegisterType<ISocketClient, SocketClient>();
            iocProvider.RegisterType<IProfileService, ProfileService>();
            iocProvider.RegisterSingleton<IUserContext>(() => new UserContext(iocProvider.Resolve<IProfileService>(), iocProvider.Resolve<IMvxNavigationService>()));
            iocProvider.RegisterType<IBlockchainService, BlockchainService>();
            iocProvider.RegisterType<IVirtualFileSystemFactory, VirtualFileSystemFactory>();
            iocProvider.RegisterType<IFilesToCategoryBinder, FilesToCategoryBinder>();
            iocProvider.RegisterType<IExploreService, ExploreService>();
            iocProvider.RegisterType<ISearchService, SearchService>();
            iocProvider.RegisterSingleton<IDownloadManager>(new DownloadManager(iocProvider.Resolve<ISettingsManager>()));
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
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Plugin;
using Peernet.Browser.Application;
using Peernet.Browser.Application.Clients;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Infrastructure;
using Serilog;
using Serilog.Extensions.Logging;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Extensions;
using Peernet.Browser.Infrastructure.Tools;
using Peernet.Browser.WPF.Services;
using Serilog.Events;

namespace Peernet.Browser.WPF
{
    public class Setup : MvxWpfSetup<Application.App>
    {
        private CmdRunner cmdRunner;

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);

            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Control.Plugin>(true);
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            var settings = new SettingsManager();
            var backendPath = Path.GetFullPath(settings.Backend);
            var backendWorkingDirectory = Path.GetDirectoryName(backendPath);
            var logPath = Path.Combine(backendWorkingDirectory, settings.LogFile);
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .WriteTo.File(logPath, LogEventLevel.Error)
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

            iocProvider.RegisterPeernetServices();
            iocProvider.RegisterType<ISocketClient, SocketClient>();
            iocProvider.RegisterSingleton<IUserContext>(() => new UserContext(iocProvider.Resolve<IProfileService>()));
            iocProvider.RegisterType<IVirtualFileSystemFactory, VirtualFileSystemFactory>();
            iocProvider.RegisterType<IFilesToCategoryBinder, FilesToCategoryBinder>();
            iocProvider.RegisterType(typeof(ILogger<>), typeof(Logger<>));

            GlobalContext.UiThreadDispatcher = iocProvider.Resolve<IMvxMainThreadAsyncDispatcher>();
            ObserveNavigation(iocProvider);
            InitializeBackend(iocProvider);
        }

        public void InitializeBackend(IMvxIoCProvider iocProvider)
        {
            var settingsManager = iocProvider.Resolve<ISettingsManager>();
            if (settingsManager.ApiUrl == null)
            {
                cmdRunner = new CmdRunner(settingsManager, iocProvider.Resolve<IShutdownService>(), iocProvider.Resolve<IApiService>());
                cmdRunner.Run();
            }

            base.InitializeSecondary();
        }

        private static void ObserveNavigation(IMvxIoCProvider iocProvider)
        {
            iocProvider.Resolve<IMvxNavigationService>().DidNavigate +=
                delegate (object sender, IMvxNavigateEventArgs args)
                {
                    if (args.ViewModel is IModal) return;
                    GlobalContext.CurrentViewModel = args.ViewModel.GetType().Name;
                };
        }
    }
}
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Plugin;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure;
using RestSharp;
using Serilog;
using Serilog.Extensions.Logging;

namespace Peernet.Browser.WPF
{
    public class Setup : MvxWpfSetup<Application.App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            var Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .CreateLogger();

            return new SerilogLoggerFactory(Logger);
        }

        protected override void RegisterBindingBuilderCallbacks(IMvxIoCProvider iocProvider)
        {
            // register managers
            CreatableTypes()
                .EndingWith("Manager")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            iocProvider.RegisterType<IRestClient, RestClient>();
            iocProvider.RegisterType<IApiClient, ApiClient>();
            iocProvider.RegisterType<ISocketClient, SocketClient>();
        }

        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);

            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.Control.Plugin>(true);
        }
    }
}
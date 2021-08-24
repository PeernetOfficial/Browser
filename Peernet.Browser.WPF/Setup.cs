using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Wpf.Binding;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Plugin;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Infrastructure;
using RestSharp;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override void RegisterBindingBuilderCallbacks(IMvxIoCProvider iocProvider)
        {
            // register services
            iocProvider.RegisterType<IRestClient, RestClient>();
            iocProvider.RegisterType<IApiClient, ApiClient>();
            iocProvider.RegisterType<ISocketClient, SocketClient>();
            iocProvider.RegisterType<IApplicationManager, ApplicationManager>();
            
            iocProvider.RegisterType<NavigationBarViewModel>(
                () => new NavigationBarViewModel(iocProvider.Resolve<IMvxNavigationService>()));
            
            iocProvider.RegisterType<FooterViewModel>(
                () => new FooterViewModel(
                    iocProvider.Resolve<IApiClient>(),
                    iocProvider.Resolve<ISocketClient>()));

            iocProvider.RegisterType<ControlButtonsViewModel>(
                () => new ControlButtonsViewModel(
                    iocProvider.Resolve<IApplicationManager>()));
        }



        public override void LoadPlugins(IMvxPluginManager pluginManager)
        {
            base.LoadPlugins(pluginManager);

            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.MethodBinding.Plugin>(true);
            pluginManager.EnsurePluginLoaded<MvvmCross.Plugin.FieldBinding.Plugin>(true);
        }
    }
}

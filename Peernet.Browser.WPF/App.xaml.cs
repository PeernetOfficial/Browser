using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Plugins;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Infrastructure.Extensions;
using Peernet.Browser.Infrastructure.Tools;
using Peernet.Browser.WPF.Services;
using Peernet.Browser.WPF.Styles;
using Peernet.SDK.Client.Extensions;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using Serilog;
using Serilog.Events;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : System.Windows.Application
    {
        public static IServiceProvider ServiceProvider;
        private static CmdRunner cmdRunner;
        private readonly object lockObject = new();
        private INotificationsManager notificationsManager;

        static App()
        {
            var settings = new SettingsManager();
            GlobalContext.VisualMode = settings.DefaultTheme;
            ActivateCultureTracking();
        }

        public App()
        {
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var settingsManager = new SettingsManager();
            if (settingsManager.ApiUrl == null)
            {
                CmdRunner.ReserveAddress(settingsManager);
            }

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            new PeernetPluginsManager(settingsManager).LoadPlugins(services);
            ServiceProvider = services.BuildServiceProvider();

            notificationsManager = ServiceProvider.GetRequiredService<INotificationsManager>();
            PluginsContext.PlayButtonPlugEnabled = ServiceProvider.GetService<IPlayButtonPlug>() != null;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            InitializeBackend();
        }

        public static event RoutedEventHandler MainWindowClicked = delegate { };

        public static void RaiseMainWindowClick(object sender, RoutedEventArgs e)
        {
            MainWindowClicked.Invoke(sender, e);
        }

        public void InitializeBackend()
        {
            var settingsManager = ServiceProvider.GetRequiredService<ISettingsManager>();
            if (CmdRunner.SelfHosted)
            {
                cmdRunner = new CmdRunner(settingsManager, ServiceProvider.GetRequiredService<IShutdownService>(), ServiceProvider.GetRequiredService<IApiService>());
                cmdRunner.Run();
            }
        }

        public void UpdateAllResources()
        {
            foreach (ResourceDictionary dict in Resources.MergedDictionaries)
            {
                if (dict is ModeResourceDictionary skinDict)
                {
                    skinDict.UpdateSource();
                }
                else
                {
                    dict.Source = dict.Source;
                }
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ServiceProvider.GetRequiredService<ISettingsManager>().DefaultTheme = GlobalContext.VisualMode;
            cmdRunner?.Dispose();
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            BindingOperations.EnableCollectionSynchronization(ServiceProvider.GetRequiredService<INotificationsManager>().Notifications, lockObject);

            base.OnStartup(e);
        }

        private static void ActivateCultureTracking()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            notificationsManager.Notifications.Add(new("Unhandled Dispatcher exception occurred!", e.Exception.Message, Severity.Error, e.Exception));
            e.Handled = true;
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var settings = new SettingsManager();
            RegisterLogger(services, settings);

            services.AddSingleton<NotificationCollection>();
            services.AddSingleton<INotificationsManager, NotificationsManager>();
            services.AddSingleton<ISettingsManager>(settings);
            Action<HttpResponseMessage, string> onRequestFailure =
                (response, details) => notificationsManager?.Notifications.Add(
                    new($"Unexpected response status code: {response.StatusCode}", details, Severity.Error));
            services.RegisterPeernetClients(settings, onRequestFailure);
            services.RegisterPeernetServices();
            services.AddSingleton<IApplicationManager, ApplicationManager>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IModalNavigationService, ModalNavigationService>();
            services.AddSingleton<IUserContext, UserContext>();
            services.AddTransient<IVirtualFileSystemFactory, VirtualFileSystemFactory>();
            services.AddTransient<IFilesToCategoryBinder, FilesToCategoryBinder>();

            services.AddSingleton<IPlayButtonPlug>(sp => null);

            RegisterViewModels(services);
            RegisterWindows(services);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            notificationsManager.Notifications.Add(new("Unhandled Domain exception occurred!", exception.Message, Severity.Error, exception));
        }

        private void RegisterLogger(ServiceCollection services, ISettingsManager settings)
        {
            var backendPath = Path.GetFullPath(settings.Backend);
            var backendWorkingDirectory = Path.GetDirectoryName(backendPath);
            string logPath = string.Empty;
            if (!string.IsNullOrEmpty(settings.LogFile))
            {
                logPath = Path.Combine(backendWorkingDirectory, settings.LogFile);
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            }

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(
                new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .WriteTo.File(logPath, LogEventLevel.Error)
                .CreateLogger(), dispose: true));
        }

        private void RegisterViewModels(ServiceCollection services)
        {
            services.AddTransient<TerminalViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<FooterViewModel>();
            services.AddSingleton<NavigationBarViewModel>();
            services.AddSingleton<ExploreViewModel>();
            services.AddSingleton<DirectoryViewModel>();
            services.AddSingleton<AboutViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<EditProfileViewModel>();
            services.AddTransient<DeleteAccountViewModel>();
            services.AddTransient<ShareFileViewModel>();
            services.AddTransient<EditFileViewModel>();
            services.AddSingleton<FilePreviewViewModel>();
        }

        private void RegisterWindows(ServiceCollection services)
        {
            services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            notificationsManager.Notifications.Add(new("Unhandled TaskScheduler exception occurred!", e.Exception.Message, Severity.Error, e.Exception));
            e.SetObserved();
        }
    }
}
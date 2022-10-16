using DevExpress.Xpf.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private const string DXVersion = "v22.1";
        private static CmdRunner cmdRunner;
        private static Services.SplashScreenManager splashScreenManager = new();
        private readonly object lockObject = new();
        private readonly INotificationsManager notificationsManager;

        static App()
        {
            RegisterDXThemes();

            Settings = new SettingsManager();
            GlobalContext.VisualMode = Settings.DefaultTheme;

            splashScreenManager?.Start();
            if (!Settings.Validate())
            {
                var message = "Invalid or missing configuration!";
                MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ActivateCultureTracking();
        }

        public App()
        {
            var services = new ServiceCollection();
            splashScreenManager.SetState("Configuring services...");

            // Register Services
            ConfigureServices(services);

            // Register and get Logger to be able to create NotificationsManager
            var logger = CreateAndRegisterLogger(services, Settings);

            // Register and get NotificationsManager to be able to log exceptions before ServiceProvider is built
            notificationsManager = CreateAndRegisterNotificationsManager(services, logger);
            AssignExceptionHandlers();

            // Plugins Load method is registering its services in the service collection. It may result in the exception.
            // To be able to capture it and log the NotificationsManager with exception handles needs to be available prior to
            new PeernetPluginsManager(Settings, notificationsManager).LoadPlugins(services);
            ServiceProvider = services.BuildServiceProvider();
            
            splashScreenManager.SetState("Services configuration completed.");

            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            if (Settings.ApiUrl == null)
            {
                CmdRunner.ReserveAddress(Settings);
            }

            PluginsContext.PlayButtonPlugEnabled = ServiceProvider.GetService<IPlayButtonPlug>() != null;

            splashScreenManager.SetState("Preparing Backend...");
            InitializeBackend();
            splashScreenManager.SetState("Backend Started...");
        }

        public static event RoutedEventHandler MainWindowClicked = delegate { };

        public static IServiceProvider ServiceProvider { get; private set; }
        public static ISettingsManager Settings { get; private set; }

        public static void InitializeBackend()
        {
            var settingsManager = ServiceProvider.GetRequiredService<ISettingsManager>();
            if (CmdRunner.SelfHosted)
            {
                cmdRunner = new CmdRunner(settingsManager, ServiceProvider.GetRequiredService<IShutdownService>(), ServiceProvider.GetRequiredService<IStatusService>());
                cmdRunner.Run();
            }
        }

        private INotificationsManager CreateAndRegisterNotificationsManager(IServiceCollection services, Serilog.ILogger logger)
        {
            var notificationCollection = new NotificationCollection(logger);
            services.AddSingleton(notificationCollection);

            var notificationsManager = new NotificationsManager(notificationCollection);
            services.AddSingleton<INotificationsManager, NotificationsManager>(sp => notificationsManager);

            return notificationsManager;
        }

        public static void RaiseMainWindowClick(object sender, RoutedEventArgs e)
        {
            MainWindowClicked.Invoke(sender, e);
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
            splashScreenManager.SetState("Application startup complete. Opening MainWindow...");
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            App.Current.MainWindow = mainWindow;
            mainWindow.Show();

            splashScreenManager.Exit();

            BindingOperations.EnableCollectionSynchronization(ServiceProvider.GetRequiredService<INotificationsManager>().Notifications, lockObject);

            base.OnStartup(e);
        }

        private static void ActivateCultureTracking()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        private static void RegisterDXTheme(string name)
        {
            var theme = new Theme(name);
            theme.AssemblyName = $"DevExpress.Xpf.Themes.{name}.{DXVersion}";
            Theme.RegisterTheme(theme);
        }

        private static void RegisterDXThemes()
        {
            RegisterDXTheme("PeernetDarkTheme");
            RegisterDXTheme("PeernetLightTheme");
        }

        private static Serilog.ILogger CreateAndRegisterLogger(ServiceCollection services, ISettingsManager settings)
        {
            var backendPath = Path.GetFullPath(settings.Backend);
            var backendWorkingDirectory = Path.GetDirectoryName(backendPath);
            string logPath = string.Empty;
            if (!string.IsNullOrEmpty(settings.LogFile))
            {
                logPath = Path.Combine(backendWorkingDirectory, settings.LogFile);
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            }
            var logger =
                new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .WriteTo.File(logPath, LogEventLevel.Information)
                .CreateLogger();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger, dispose: true));

            return logger;
        }

        private static void RegisterViewModels(ServiceCollection services)
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
            services.AddSingleton<AdvancedSearchOptionsViewModel>();
        }

        private static void RegisterWindows(ServiceCollection services)
        {
            services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            notificationsManager.Notifications.Add(new("Unhandled Dispatcher exception occurred!", e.Exception.Message, Severity.Error, e.Exception));
            e.Handled = true;
        }

        private void AssignExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(Settings);
            Action<HttpResponseMessage, string> onRequestFailure =
                (response, details) => notificationsManager?.Notifications.Add(
                    new($"Unexpected response status code: {response.StatusCode}", details, Severity.Error));
            services.RegisterPeernetClients(Settings, onRequestFailure);
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

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            notificationsManager.Notifications.Add(new("Unhandled TaskScheduler exception occurred!", e.Exception.Message, Severity.Error, e.Exception));
            e.SetObserved();
        }
    }
}
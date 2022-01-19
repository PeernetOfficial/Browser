using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Infrastructure.Extensions;
using Peernet.Browser.Infrastructure.Tools;
using Peernet.Browser.Models.Presentation.Footer;
using Peernet.Browser.WPF.Controls;
using Peernet.Browser.WPF.Services;
using Peernet.Browser.WPF.Styles;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : System.Windows.Application
    {
        private INotificationsManager notificationsManager;
        private static CmdRunner cmdRunner;
        public static IServiceProvider ServiceProvider;

        static App()
        {
            GlobalContext.VisualMode = new SettingsManager().DefaultTheme;

            FrameworkElement.LanguageProperty.OverrideMetadata(

                typeof(FrameworkElement),

                new FrameworkPropertyMetadata(

                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        public App()
        {
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            notificationsManager = ServiceProvider.GetRequiredService<INotificationsManager>();
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
            new SettingsManager().DefaultTheme = GlobalContext.VisualMode;
            cmdRunner?.Dispose();
            base.OnExit(e);
        }

        private void ConfigureServices(ServiceCollection services)
        {
            RegisterViewModels(services);
            RegisterWindows(services);
            services.RegisterPeernetServices();
            
            services.AddSingleton<IUIThreadDispatcher, UIThreadDispatcher>(s => new(SynchronizationContext.Current));
            services.AddSingleton<ISettingsManager, SettingsManager>();
            services.AddSingleton<IApplicationManager, ApplicationManager>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IModalNavigationService, ModalNavigationService>();
            services.AddSingleton<IUserContext, UserContext>();
            services.AddTransient<IVirtualFileSystemFactory, VirtualFileSystemFactory>();
            services.AddTransient<IFilesToCategoryBinder, FilesToCategoryBinder>();
            services.AddSingleton<INotificationsManager, NotificationsManager>();
        }
        public void InitializeBackend()
        {
            var settingsManager = ServiceProvider.GetRequiredService<ISettingsManager>();
            if (settingsManager.ApiUrl == null)
            {
                cmdRunner = new CmdRunner(settingsManager, ServiceProvider.GetRequiredService<IShutdownService>(), ServiceProvider.GetRequiredService<IApiService>());
                cmdRunner.Run();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            notificationsManager.Notifications.Add(new("Unhandled Dispatcher exception occurred!", e.Exception.Message, Severity.Error, e.Exception));
            e.Handled = true;
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
        }

        private void RegisterWindows(ServiceCollection services)
        {
            services.AddSingleton(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
            services.AddSingleton(s => new TerminalWindow(s.GetRequiredService<TerminalViewModel>()));
        }
    }
}
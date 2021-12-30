using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.WPF.Services;
using Peernet.Browser.WPF.Styles;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        public static event RoutedEventHandler MainWindowClicked = delegate { };

        protected override void RegisterSetup() => this.RegisterSetupType<Setup>();

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        static App()
        {
            GlobalContext.VisualMode = new SettingsManager().DefaultTheme;

            FrameworkElement.LanguageProperty.OverrideMetadata(

                typeof(FrameworkElement),

                new FrameworkPropertyMetadata(

                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            new SettingsManager().DefaultTheme = GlobalContext.VisualMode;
            base.OnExit(e);
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

        public static void RaiseMainWindowClick(object sender, RoutedEventArgs e)
        {
            MainWindowClicked.Invoke(sender, e);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            GlobalContext.Notifications.Add(new("Unhandled Dispatcher exception occurred!", e.Exception.Message, Severity.Error));
            Mvx.IoCProvider.Resolve<ILogger<App>>().LogError(e.Exception, e.Exception.Message);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            GlobalContext.Notifications.Add(new("Unhandled Domain exception occurred!", exception.Message, Severity.Error));
            Mvx.IoCProvider.Resolve<ILogger<App>>().LogError(exception, exception.Message);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            GlobalContext.Notifications.Add(new("Unhandled TaskScheduler exception occurred!", e.Exception.Message, Severity.Error));
            Mvx.IoCProvider.Resolve<ILogger<App>>().LogError(e.Exception, e.Exception.Message);
            e.SetObserved();
        }
    }
}
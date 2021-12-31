using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation.Footer;
using Peernet.Browser.WPF.Services;
using Peernet.Browser.WPF.Styles;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
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
            Setup.GetBackendRunner()?.Dispose();
            base.OnExit(e);
        }

        protected override void RegisterSetup() => this.RegisterSetupType<Setup>();

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            GlobalContext.Notifications.Add(new("Unhandled Dispatcher exception occurred!", e.Exception.Message, Severity.Error, e.Exception));
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            GlobalContext.Notifications.Add(new("Unhandled Domain exception occurred!", exception.Message, Severity.Error, exception));
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            GlobalContext.Notifications.Add(new("Unhandled TaskScheduler exception occurred!", e.Exception.Message, Severity.Error, e.Exception));
            e.SetObserved();
        }
    }
}
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Infrastructure.Tools;
using Peernet.Browser.WPF.Services;
using Peernet.Browser.WPF.Styles;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        public static event RoutedEventHandler MainWindowClicked = delegate { };
        private CmdRunner cmdRunner;

        protected override void RegisterSetup() => this.RegisterSetupType<Setup>();

        static App()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(

                typeof(FrameworkElement),

                new FrameworkPropertyMetadata(

                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            cmdRunner?.Dispose();
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var settingsManager = new SettingsManager();
            if (settingsManager.ApiUrl == null)
            {
                cmdRunner = new CmdRunner(settingsManager);
                cmdRunner.Run();
            }

            base.OnStartup(e);
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
    }
}
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Infrastructure.Tools;
using Peernet.Browser.WPF.Services;
using System.Windows;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        private CmdRunner cmdRunner;

        protected override void RegisterSetup() => this.RegisterSetupType<Setup>();

        protected override void OnExit(ExitEventArgs e)
        {
            cmdRunner.Dispose();
            base.OnExit(e);
        }

        public override void ApplicationInitialized()
        {
            cmdRunner = new CmdRunner(new SettingsManager().CmdPath);
            cmdRunner.Run();

            base.ApplicationInitialized();
        }
    }
}
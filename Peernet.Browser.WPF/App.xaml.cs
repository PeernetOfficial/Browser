using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Infrastructure.Tools;
using System.Windows;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        private CmdRunner _runner;

        protected override void RegisterSetup() => this.RegisterSetupType<Setup>();

        protected override void OnExit(ExitEventArgs e)
        {
            _runner.Dispose();
            base.OnExit(e);
        }

        public override void ApplicationInitialized()
        {
            _runner = new CmdRunner(new SettingsManager().CmdPath);
            if (_runner.FileExist) _runner.Run();
            base.ApplicationInitialized();
        }
    }
}
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.WPF.Services
{
    public class ApplicationManager : IApplicationManager
    {
        public ApplicationManager()
        {
            SetWindow();
        }

        private MainWindow window;

        private void SetWindow()
        {
            if (window == null) window = (MainWindow)System.Windows.Application.Current.MainWindow;
        }

        public bool IsMaximized
        {
            get => window.WindowState == System.Windows.WindowState.Maximized;
        }

        public void Maximize() => window.WindowState = System.Windows.WindowState.Maximized;

        public void Minimize() => window.WindowState = System.Windows.WindowState.Minimized;

        public void Shutdown() => window.Close();

        public void Restore() => window.WindowState = System.Windows.WindowState.Normal;
    }
}
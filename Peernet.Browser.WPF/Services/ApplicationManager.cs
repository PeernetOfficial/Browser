using Microsoft.Win32;
using Peernet.Browser.Application.Managers;

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
            window ??= (MainWindow)System.Windows.Application.Current.MainWindow;
        }

        public bool IsMaximized => window.WindowState == System.Windows.WindowState.Maximized;

        public void Maximize() => window.WindowState = System.Windows.WindowState.Maximized;

        public void Minimize() => window.WindowState = System.Windows.WindowState.Minimized;

        public void Shutdown() => window.Close();

        public void Restore() => window.WindowState = System.Windows.WindowState.Normal;

        public string[] OpenFileDialog(bool multiselect = true, string filter = "")
        {
            var dialog = new OpenFileDialog { Multiselect = multiselect };
            dialog.Filter = filter;
            if (dialog.ShowDialog().GetValueOrDefault()) return dialog.FileNames;
            else return new string[0];
        }
    }
}
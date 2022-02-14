using Microsoft.Win32;
using Peernet.Browser.Application.Managers;
using System;
using System.Windows;

namespace Peernet.Browser.WPF.Services
{
    public class ApplicationManager : IApplicationManager
    {
        private Lazy<MainWindow> window = new Lazy<MainWindow>(GetWindow);

        public ApplicationManager()
        {
        }

        private static MainWindow GetWindow()
        {
            return (MainWindow)System.Windows.Application.Current.MainWindow;
        }

        public bool IsMaximized => window.Value.WindowState == WindowState.Maximized;

        public void Maximize() => window.Value.WindowState = WindowState.Maximized;

        public void Minimize() => window.Value.WindowState = WindowState.Minimized;

        public void Shutdown() => App.Current.Shutdown();

        public void Restore() => window.Value.WindowState = WindowState.Normal;

        public string[] OpenFileDialog(bool multiselect = true, string filter = "")
        {
            var dialog = new OpenFileDialog { Multiselect = multiselect };
            dialog.Filter = filter;
            if (dialog.ShowDialog().GetValueOrDefault()) return dialog.FileNames;
            else return new string[0];
        }
    }
}
using MvvmCross.Plugin.Control.Platforms.Wpf;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.WPF.Extensions;
using System.Windows;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for Footer.xaml
    /// </summary>
    public partial class FooterControl : MvxWpfControl
    {
        public FooterControl()
        {
            InitializeComponent();
            App.MainWindowClicked += OnMainWindowClicked;
        }

        private void OnMainWindowClicked(object sender, RoutedEventArgs e)
        {
            if (((DependencyObject)e.OriginalSource).FindParent<DownloadsPaneControl>() == null)
            {
                ((FooterViewModel)ViewModel).AreDownloadsCollapsed = true;
            }
        }
    }
}
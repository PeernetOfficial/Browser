using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Models.Presentation.Footer;
using System.Windows;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for FilePreviewWindow.xaml
    /// </summary>
    public partial class FilePreviewWindow : MvxWindow
    {
        private readonly DownloadModel model;

        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool),
                typeof(FilePreviewWindow), null);

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        public FilePreviewWindow(DownloadModel model, bool isEditable)
        {
            InitializeComponent();
            this.model = model;
            IsEditable = isEditable;
            Content = model;
        }

        private void Minimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Download_OnClick(object sender, RoutedEventArgs e)
        {
            Mvx.IoCProvider.Resolve<IDownloadManager>().QueueUpDownload(model);
        }
    }
}
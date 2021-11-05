using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Models.Presentation.Footer;
using System.Windows;
using System.Windows.Input;

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

        public static readonly DependencyProperty ActionButtonEnabledProperty =
            DependencyProperty.Register("ActionButtonEnabled", typeof(bool),
                typeof(FilePreviewWindow), null);

        public static readonly DependencyProperty ActionButtonContentProperty =
            DependencyProperty.Register("ActionButtonContent", typeof(string),
                typeof(FilePreviewWindow), null);

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        public bool ActionButtonEnabled
        {
            get => (bool)GetValue(ActionButtonEnabledProperty);
            set => SetValue(ActionButtonEnabledProperty, value);
        }

        public string ActionButtonContent
        {
            get => (string)GetValue(ActionButtonContentProperty);
            set => SetValue(ActionButtonContentProperty, value);
        }


        public FilePreviewWindow(DownloadModel model, bool isEditable, bool actionButtonEnabled, string actionButtonContent)
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
            this.model = model;
            IsEditable = isEditable;
            Content = model;
            ActionButtonEnabled = actionButtonEnabled;
            ActionButtonContent = actionButtonContent;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Download_OnClick(object sender, RoutedEventArgs e)
        {
            Mvx.IoCProvider.Resolve<IDownloadManager>().QueueUpDownload(model);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
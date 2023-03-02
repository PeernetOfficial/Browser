using AsyncAwaitBestPractices.MVVM;
using Peernet.SDK.Models.Presentation.Footer;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for DailyFeedWidgetItemControl.xaml
    /// </summary>
    public partial class DailyFeedWidgetItemControl : UserControl
    {
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register(
                "File",
                typeof(DownloadModel),
                typeof(DailyFeedWidgetItemControl),
                null);

        public static readonly DependencyProperty StreamFileCommandProperty =
            DependencyProperty.Register(
                "StreamFileCommand",
                typeof(IAsyncCommand<DownloadModel>),
                typeof(DailyFeedWidgetItemControl),
                null);

        public static readonly DependencyProperty DownloadFileCommandProperty =
            DependencyProperty.Register(
                "DownloadFileCommand",
                typeof(IAsyncCommand<DownloadModel>),
                typeof(DailyFeedWidgetItemControl),
                null);

        public DailyFeedWidgetItemControl()
        {
            InitializeComponent();
        }

        public DownloadModel File
        {
            get => (DownloadModel)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }

        public IAsyncCommand<DownloadModel> StreamFileCommand
        {
            get => (IAsyncCommand<DownloadModel>)GetValue(StreamFileCommandProperty);
            set => SetValue(StreamFileCommandProperty, value);
        }

        public IAsyncCommand<DownloadModel> DownloadFileCommand
        {
            get => (IAsyncCommand<DownloadModel>)GetValue(DownloadFileCommandProperty);
            set => SetValue(DownloadFileCommandProperty, value);
        }
    }
}
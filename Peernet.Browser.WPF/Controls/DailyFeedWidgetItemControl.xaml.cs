using AsyncAwaitBestPractices.MVVM;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Presentation.Footer;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
    }
}
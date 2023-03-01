using AsyncAwaitBestPractices.MVVM;
using Peernet.SDK.Models.Presentation.Footer;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for GenericEmbededPluginControl.xaml
    /// </summary>
    public partial class GenericEmbededPluginControl : UserControl
    {
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register(
                "File",
                typeof(DownloadModel),
                typeof(GenericEmbededPluginControl),
                null);

        public static readonly DependencyProperty OpenFileCommandProperty =
            DependencyProperty.Register(
                "OpenFileCommand",
                typeof(IAsyncCommand<DownloadModel>),
                typeof(GenericEmbededPluginControl),
                null);

        public GenericEmbededPluginControl()
        {
            InitializeComponent();
        }

        public DownloadModel File
        {
            get => (DownloadModel)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }

        public IAsyncCommand<DownloadModel> OpenFileCommand
        {
            get => (IAsyncCommand<DownloadModel>)GetValue(OpenFileCommandProperty);
            set => SetValue(OpenFileCommandProperty, value);
        }
    }
}
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for FilePreviewWindow.xaml
    /// </summary>
    [MvxWindowPresentation]
    public partial class FilePreviewWindow : IMvxOverridePresentationAttribute
    {
        public FilePreviewWindow()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            var instanceRequest = request as MvxViewModelInstanceRequest;
            var viewModel = instanceRequest?.ViewModelInstance as FilePreviewViewModel;

            return new MvxWindowPresentationAttribute
            {
                Identifier = $"{nameof(FilePreviewWindow)}.{viewModel?.File.Name}"
            };
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
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
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application;
using Peernet.Browser.Application.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for ShareNewFileView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(ShareNewFileViewModel))]
    public partial class ShareNewFileView : MvxWpfView, IModal
    {
        public ShareNewFileView() => InitializeComponent();

        private void ChangeVirtualDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            virtualDirectoryPath.IsReadOnly = false;
            virtualDirectoryPath.IsEnabled = true;
            virtualDirectoryPath.Focus();
            virtualDirectoryPath.CaretIndex = virtualDirectoryPath.Text.Length;
        }

        private void ConfirmChange_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                virtualDirectoryPath.IsReadOnly = true;
                virtualDirectoryPath.IsEnabled = false;
            }
        }
    }
}
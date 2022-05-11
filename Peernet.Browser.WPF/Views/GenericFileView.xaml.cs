using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application;
using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Application.ViewModels;
using Peernet.SDK.Models.Presentation.Footer;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for GenericFileView.xaml
    /// </summary>
    public partial class GenericFileView : UserControl, IModal
    {
        public GenericFileView() => InitializeComponent();

        private void ChangeVirtualDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            var directoryViewModel = App.ServiceProvider.GetRequiredService<DirectoryViewModel>();
            var model = (FileModel)((FrameworkElement)e.OriginalSource).DataContext;
            var changeFileLocationViewModel = new ChangeFileLocationViewModel(directoryViewModel.VirtualFileSystem.DeepClone(), directoryViewModel.PathElements.Last().AbsolutePath, model);
            new ChangleFileLocationWindow(changeFileLocationViewModel).Show();
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
using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;
using System.IO;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for EditProfileControl.xaml
    /// </summary>
    [MvxContentPresentation(WindowIdentifier = nameof(MainWindow), StackNavigation = true)]
    [MvxViewFor(typeof(EditProfileViewModel))]
    public partial class EditProfileView : MvxWpfView
    {
        public EditProfileView() => InitializeComponent();

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if( dialog.ShowDialog() == true)
            {
                var file = dialog.FileName;
                var content = File.ReadAllBytes(file);
                ((EditProfileViewModel)ViewModel).UserContext.User.Image = content;
            }
        }
    }
}

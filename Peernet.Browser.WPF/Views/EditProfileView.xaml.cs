using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application;
using Peernet.Browser.Application.ViewModels;
using System.IO;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for EditProfileView.xaml
    /// </summary>
    [MvxContentPresentation(WindowIdentifier = nameof(EditProfileView), StackNavigation = true)]
    [MvxViewFor(typeof(EditProfileViewModel))]
    public partial class EditProfileView : MvxWpfView, IModal
    {
        public EditProfileView() => InitializeComponent();

        private void OpenFileDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.png;*.jpg)|*.png;*.jpg";

            if (dialog.ShowDialog() == true)
            {
                var file = dialog.FileName;
                var content = File.ReadAllBytes(file);

                ((EditProfileViewModel)ViewModel).UserContext.User.Image = content;
            }
        }

        private void DeleteAccount_OnClick(object sender, MouseButtonEventArgs e)
        {
            ((EditProfileViewModel)ViewModel).RemovePhotoCommand.Execute();
        }
    }
}
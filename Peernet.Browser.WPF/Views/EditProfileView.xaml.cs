using Microsoft.Win32;
using Peernet.Browser.Application;
using Peernet.Browser.Application.ViewModels;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for EditProfileView.xaml
    /// </summary>
    public partial class EditProfileView : UserControl, IModal
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

                ((EditProfileViewModel)DataContext).UserContext.User.Image = content;
            }
        }

        private void DeleteAccount_OnClick(object sender, MouseButtonEventArgs e)
        {
            ((EditProfileViewModel)DataContext).RemovePhotoCommand.ExecuteAsync().Wait();
        }
    }
}
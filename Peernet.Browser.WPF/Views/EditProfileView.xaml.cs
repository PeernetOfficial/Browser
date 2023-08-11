using Microsoft.Win32;
using Peernet.Browser.Application;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.WPF.Utilities;
using System.Drawing.Imaging;
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
        private static long MaxFileSize = App.PeernetConfiguration.BlockSize;

        public EditProfileView() => InitializeComponent();

        private void OpenFileDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.png;*.jpg)|*.png;*.jpg";

            if (dialog.ShowDialog() == true)
            {
                var file = dialog.FileName;
                byte[] content = File.ReadAllBytes(file);
                if (content.Length > MaxFileSize)
                {
                    CompressImageToSize(file, MaxFileSize, ref content);
                }
                ((EditProfileViewModel)DataContext).User.Image = content;
            }
        }

        private void DeleteAccount_OnClick(object sender, MouseButtonEventArgs e)
        {
            ((EditProfileViewModel)DataContext).RemovePhotoCommand.ExecuteAsync().Wait();
        }

        private void CompressImageToSize(string file, long desiredSize, ref byte[] content)
        {
            var image = System.Drawing.Image.FromFile(file);
            while (content.Length > desiredSize)
            {
                image = ImageCompressor.CompressImage(image, 0.8, 0.8, 85);
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    content = ms.ToArray();
                }
            }

            image.Dispose();
        }
    }
}
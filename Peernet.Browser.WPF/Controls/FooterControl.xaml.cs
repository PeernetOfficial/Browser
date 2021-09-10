using Microsoft.Win32;
using MvvmCross.Plugin.Control.Platforms.Wpf;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for Footer.xaml
    /// </summary>
    public partial class FooterControl : MvxWpfControl
    {
        public FooterControl() => InitializeComponent();

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var d = new OpenFileDialog { Multiselect = true };
            if (d.ShowDialog().GetValueOrDefault())
            {
                var a = ViewModel as FooterViewModel;
                a.UploadFiles(d.FileNames);
            }
        }
    }
}
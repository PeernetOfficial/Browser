using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for ModalView.xaml
    /// </summary>
    [MvxWindowPresentation(Modal = true)]
    public partial class ModalView : MvxWindow
    {
        public ModalView()
        {
            Owner = System.Windows.Application.Current.MainWindow;
            InitializeComponent();
        }
    }
}
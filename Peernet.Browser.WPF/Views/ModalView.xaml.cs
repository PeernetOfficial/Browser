using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for ModalView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(ModalViewModel))]
    public partial class ModalView : MvxWpfView
    {
        public ModalView() => InitializeComponent();
    }
}
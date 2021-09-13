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
    [MvxViewFor(typeof(ShareNewFileViewModel))]
    public partial class ShareNewFileView : MvxWpfView
    {
        public ShareNewFileView() => InitializeComponent();
    }
}
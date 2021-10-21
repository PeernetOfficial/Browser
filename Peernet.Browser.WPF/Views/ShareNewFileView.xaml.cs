using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application;
using Peernet.Browser.Application.ViewModels;

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
    }
}
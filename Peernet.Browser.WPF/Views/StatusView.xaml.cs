using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for StatusView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(StatusViewModel))]
    public partial class StatusView : MvxWpfView
    {
        public StatusView() => InitializeComponent();
    }
}

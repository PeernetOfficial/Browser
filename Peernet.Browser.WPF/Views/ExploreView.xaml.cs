using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for ExploreView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(ExploreViewModel))]
    public partial class ExploreView : MvxWpfView
    {
        public ExploreView() => InitializeComponent();
    }
}
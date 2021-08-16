using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(HomeViewModel))]
    public partial class Home : MvxWpfView
    {
        public Home() => InitializeComponent();
    }
}

using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(UsersViewModel))]
    public partial class UsersView : MvxWpfView
    {
        public UsersView() => InitializeComponent();
    }
}
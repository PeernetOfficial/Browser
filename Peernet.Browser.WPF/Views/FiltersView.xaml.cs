using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for FiltersView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(FiltersViewModel))]
    public partial class FiltersView : MvxWpfView
    {
        public FiltersView()
        {
            InitializeComponent();
        }
    }
}
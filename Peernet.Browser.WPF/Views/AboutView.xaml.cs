using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(AboutViewModel))]
    public partial class AboutView : MvxWpfView
    {
        public AboutView()
        {
            InitializeComponent();
        }
    }
}
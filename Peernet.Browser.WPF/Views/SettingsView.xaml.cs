using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(SettingsViewModel))]
    public partial class SettingsView : MvxWpfView
    {
        public SettingsView()
        {
            InitializeComponent();
        }
    }
}
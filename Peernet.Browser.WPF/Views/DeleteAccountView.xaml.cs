using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for DeleteAccountView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(DeleteAccountViewModel))]
    public partial class DeleteAccountView : MvxWpfView, IModal
    {
        public DeleteAccountView()
        {
            InitializeComponent();
        }
    }
}
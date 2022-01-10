using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.ViewModels;
using System.Threading;
using System.Windows.Threading;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for TerminalWindow.xaml
    /// </summary>
    [MvxWindowPresentation]
    public partial class TerminalWindow : IMvxOverridePresentationAttribute
    {
        public TerminalWindow()
        {
            InitializeComponent();
            InputField.Focus();
            OutputPane.ScrollToEnd();
            Scroll.ScrollToBottom();
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            var instanceRequest = request as MvxViewModelInstanceRequest;
            var viewModel = instanceRequest?.ViewModelInstance as TerminalViewModel;
            viewModel.OnOutputChanged += ViewModel_OnOutputChanged;

            return new MvxWindowPresentationAttribute
            {
                Identifier = $"{nameof(TerminalWindow)}.{nameof(TerminalViewModel)}"
            };
        }

        private void ViewModel_OnOutputChanged(object sender, System.EventArgs e)
        {
            OutputPane.ScrollToEnd();
            Scroll.ScrollToBottom();
        }
    }
}
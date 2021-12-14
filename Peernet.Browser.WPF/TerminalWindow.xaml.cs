using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;
using System.ComponentModel;

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
            OutputPane.ScrollToBottom();
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            var instanceRequest = request as MvxViewModelInstanceRequest;
            var viewModel = instanceRequest?.ViewModelInstance as TerminalViewModel;
            viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            return new MvxWindowPresentationAttribute
            {
                Identifier = $"{nameof(TerminalWindow)}.{nameof(TerminalViewModel)}"
            };
        }

        private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(TerminalViewModel.CommandLineOutput))
            {
                OutputPane.ScrollToBottom();
            }
        }
    }
}
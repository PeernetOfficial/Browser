using System;
using System.ComponentModel;
using System.Windows.Input;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;

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
            OutputPane.ScrollToEnd();
            OutputPane.ScrollToBottom();
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            return new MvxWindowPresentationAttribute
            {
                Identifier = $"{nameof(TerminalWindow)}.{nameof(TerminalViewModel)}"
            };
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            OutputPane.ScrollToEnd();
            OutputPane.ScrollToBottom();
        }
    }
}
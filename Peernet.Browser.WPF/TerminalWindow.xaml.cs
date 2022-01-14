using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for TerminalWindow.xaml
    /// </summary>
    [MvxWindowPresentation]
    public partial class TerminalWindow : IMvxOverridePresentationAttribute
    {
        private bool anchorScrollToBottom = true;

        public TerminalWindow()
        {
            InitializeComponent();
            OutputPane.TextChanged += Output_TextChanged;
            InputField.Focus();
        }

        public MvxBasePresentationAttribute PresentationAttribute(ViewModelBaseRequest request)
        {
            var instanceRequest = request as ViewModelBaseInstanceRequest;
            var viewModel = instanceRequest?.ViewModelInstance as TerminalViewModel;

            return new MvxWindowPresentationAttribute
            {
                Identifier = $"{nameof(TerminalWindow)}.{nameof(TerminalViewModel)}"
            };
        }

        private void Output_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var max = textBox.ExtentHeight - textBox.ViewportHeight;
            var offset = textBox.VerticalOffset;
            
            if (offset == 0 && anchorScrollToBottom)
            {
            }
            else
            {
                anchorScrollToBottom = max == offset;
            }

            if (anchorScrollToBottom)
            {
                Dispatcher.BeginInvoke(() => textBox.ScrollToEnd(), DispatcherPriority.Loaded);
            }
        }

        private void OutputPane_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var max = OutputPane.ExtentHeight - OutputPane.ViewportHeight;
            var offset = OutputPane.VerticalOffset;
            if (offset != 0 && max != offset)
            {
                anchorScrollToBottom = false;
            }
        }
    }
}
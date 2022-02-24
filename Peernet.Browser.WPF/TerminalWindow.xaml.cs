using Peernet.Browser.Application.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for TerminalWindow.xaml
    /// </summary>
    public partial class TerminalWindow : Window
    {
        private bool anchorScrollToBottom = true;

        public TerminalWindow(object dataContext)
        {
            Initialized += Window_Initialized;
            ContentRendered += Window_ContentRendered;
            Owner = App.Current.MainWindow;
            WindowStartupLocation = App.Current.MainWindow.WindowStartupLocation;

            InitializeComponent();
            DataContext = dataContext;
            OutputPane.TextChanged += Output_TextChanged;
            InputField.Focus();
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

        protected override void OnClosed(System.EventArgs e)
        {
            ((TerminalViewModel)DataContext).Dispose();
            base.OnClosed(e);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.Topmost = false;
            Owner = null;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Topmost = true;
        }
    }
}
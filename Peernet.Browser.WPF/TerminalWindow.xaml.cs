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
    }
}
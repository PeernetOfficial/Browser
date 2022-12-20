using Peernet.Browser.Application.ViewModels;
using Peernet.SDK.WPF;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for WidgetsWindow.xaml
    /// </summary>
    public partial class WidgetsWindow : PeernetWindow
    {
        public WidgetsWindow(MainViewModel dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }
    }
}

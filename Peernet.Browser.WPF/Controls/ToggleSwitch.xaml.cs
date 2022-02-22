using Peernet.Browser.Application.Contexts;
using Peernet.SDK.Models.Presentation;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {
        public ToggleSwitch()
        {
            InitializeComponent();
        }

        private void Toggle_OnClick(object sender, MouseButtonEventArgs e)
        {
            if (GlobalContext.VisualMode == VisualMode.LightMode)
            {
                GlobalContext.VisualMode = VisualMode.DarkMode;
            }
            else if (GlobalContext.VisualMode == VisualMode.DarkMode)
            {
                GlobalContext.VisualMode = VisualMode.LightMode;
            }

            (App.Current as App).UpdateAllResources();
        }

        private void ToggleLight_OnClick(object sender, MouseButtonEventArgs e)
        {
            if (GlobalContext.VisualMode != VisualMode.LightMode)
            {
                GlobalContext.VisualMode = VisualMode.LightMode;
                (App.Current as App).UpdateAllResources();
            }
        }

        private void ToggleDark_OnClick(object sender, MouseButtonEventArgs e)
        {
            if (GlobalContext.VisualMode != VisualMode.DarkMode)
            {
                GlobalContext.VisualMode = VisualMode.DarkMode;
                (App.Current as App).UpdateAllResources();
            }
        }
    }
}
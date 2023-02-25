using Peernet.SDK.Models.Domain.Common;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for DailyFeedWidgetItemControl.xaml
    /// </summary>
    public partial class DailyFeedWidgetItemControl : UserControl
    {
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register(
                "File",
                typeof(ApiFile),
                typeof(DailyFeedWidgetItemControl),
                null);

        public DailyFeedWidgetItemControl()
        {
            InitializeComponent();
        }

        public ApiFile File
        {
            get => (ApiFile)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }
    }
}
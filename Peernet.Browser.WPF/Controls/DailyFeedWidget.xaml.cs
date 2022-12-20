using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for DailyFeedWidget.xaml
    /// </summary>
    public partial class DailyFeedWidget : UserControl
    {
        public DailyFeedWidget()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ResultsProperty = DependencyProperty.Register("Results", typeof(ObservableCollection<DownloadModel>), typeof(DailyFeedWidget), null);

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(DownloadModel),
            typeof(DailyFeedWidget),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ObservableCollection<DownloadModel> Results
        {
            get => (ObservableCollection<DownloadModel>)GetValue(ResultsProperty);
            set => SetValue(ResultsProperty, value);
        }
        
        public DownloadModel SelectedItem
        {
            get => (DownloadModel)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
    }
}

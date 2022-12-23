using Peernet.SDK.Common;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Widgets;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Peernet.Browser.Application.Widgets
{
    public class WidgetsService : IWidgetsService, INotifyPropertyChanged
    {
        private DownloadModel selectedItem;
        private readonly ISettingsManager settingsManager;

        public WidgetsService(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            Widgets = new ObservableCollection<WidgetModel>(GetWidgets());
        }

        private void Widget_SelectionChanged(object sender, System.EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedWidgets)));
        }

        public ObservableCollection<WidgetModel> Widgets { get; set; }

        public ObservableCollection<WidgetModel> SelectedWidgets => new(Widgets?.Where(w => w.IsSelected));

        public DownloadModel SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<WidgetModel> GetWidgets()
        {
            return new List<WidgetModel> { new WidgetModel(Widget_SelectionChanged, "Daily Feed", settingsManager.DailyFeedWidgetEnabled) };
        }
    }
}

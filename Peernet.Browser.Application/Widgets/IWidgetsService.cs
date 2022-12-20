using Peernet.SDK.Models.Presentation.Widgets;
using System.Collections.ObjectModel;

namespace Peernet.Browser.Application.Widgets
{
    public interface IWidgetsService
    {
        ObservableCollection<WidgetModel> Widgets { get; set; }
    }
}
using MvvmCross.ViewModels;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class FilterResultModel : MvxNotifyPropertyChanged
    {
        public string Content { get; set; }

        public SearchFiltersType Type { get; set; }
    }
}
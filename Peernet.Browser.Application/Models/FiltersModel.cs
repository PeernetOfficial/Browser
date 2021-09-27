using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class FiltersModel : MvxNotifyPropertyChanged
    {
        public FiltersModel()
        {
            ClearCommand = new MvxCommand(() => Results.Clear());
            Results.CollectionChanged += (o, s) => RaisePropertyChanged(nameof(IsVisible));
        }

        public void Refresh(IEnumerable<string> news)
        {
            Results.Clear();
            Results.AddRange(news.Select(x => new FilterResultModel(Remove) { Content = x }));
        }

        public MvxObservableCollection<FilterResultModel> Results { get; } = new MvxObservableCollection<FilterResultModel>();

        public bool IsVisible => Results.Any();

        public IMvxCommand ClearCommand { get; }

        private void Remove(FilterResultModel o) => Results.Remove(o);
    }
}
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class FiltersModel : MvxNotifyPropertyChanged
    {
        public Action CloseAction { get; set; }
        public SearchFilterResultModel SearchFilterResult { get; } = new SearchFilterResultModel();

        public FiltersModel()
        {
            ClearCommand = new MvxCommand(() => Results.Clear());
            CancelCommand = new MvxCommand(Hide);
            ApplyFiltersCommand = new MvxCommand(ApplyFilters);

            DateFilters = new DateFilterModel();
            FileFormatFilters = new FileFormatFilterModel();
            SortOrderFilters = new SortFormatFilterModel();
            HealthFiltes = new HealthFilterModel();
            RangeFilter = new RangeSliderModel();

            Results.CollectionChanged += (o, s) => RaisePropertyChanged(nameof(IsVisible));
        }

        public void Refresh() => Refresh(SearchFilterResult.Get());

        public void Refresh(IEnumerable<string> news)
        {
            var toAdd = news.Select(x => new FilterResultModel(Remove) { Content = x }).ToArray();
            Results.Clear();
            Results.AddRange(toAdd);
        }

        public MvxObservableCollection<FilterResultModel> Results { get; } = new MvxObservableCollection<FilterResultModel>();

        public bool IsVisible => Results.Any();

        private void Remove(FilterResultModel o) => Results.Remove(o);

        public DateFilterModel DateFilters { get; }
        public FileFormatFilterModel FileFormatFilters { get; }
        public SortFormatFilterModel SortOrderFilters { get; }
        public RangeSliderModel RangeFilter { get; }
        public HealthFilterModel HealthFiltes { get; }

        public IMvxCommand CancelCommand { get; }

        public IMvxCommand ApplyFiltersCommand { get; }

        public IMvxCommand ClearCommand { get; }

        public void Reset(int min, int max)
        {
            SearchFilterResult.HealthType = HealthType.Green;
            SearchFilterResult.FileFormat = FileFormats.All;
            SearchFilterResult.Order = SortOrders.MostRelevant;
            SearchFilterResult.Time = TimePeriods.Any;

            SearchFilterResult.SizeMin = min;
            SearchFilterResult.SizeMax = max;

            Bind();
        }

        private void ApplyFilters()
        {
            SearchFilterResult.Order = SortOrderFilters.GetSelected();
            SearchFilterResult.FileFormat = FileFormatFilters.GetSelected();
            SearchFilterResult.Time = DateFilters.GetSelected();
            SearchFilterResult.HealthType = HealthFiltes.GetSelected();

            SearchFilterResult.SizeFrom = RangeFilter.CurrentMin;
            SearchFilterResult.SizeTo = RangeFilter.CurrentMin;
            SearchFilterResult.SizeMin = RangeFilter.Min;
            SearchFilterResult.SizeMax = RangeFilter.Max;

            Hide();
        }

        private void Hide() => CloseAction?.Invoke();

        private void Bind()
        {
            DateFilters.Set(SearchFilterResult.Time);
            FileFormatFilters.Set(SearchFilterResult.FileFormat);
            SortOrderFilters.Set(SearchFilterResult.Order);
            HealthFiltes.Set(SearchFilterResult.HealthType);

            RangeFilter.CurrentMax = SearchFilterResult.SizeTo ?? SearchFilterResult.SizeMax;
            RangeFilter.CurrentMin = SearchFilterResult.SizeFrom ?? SearchFilterResult.SizeMin;
            RangeFilter.Max = SearchFilterResult.SizeMax;
            RangeFilter.Min = SearchFilterResult.SizeMin;
        }
    }
}
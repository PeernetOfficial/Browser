using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Enums;
using System;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class FiltersModel : MvxNotifyPropertyChanged
    {
        private readonly Func<SearchFilterResultModel, SearchResultModel> refreshAction;
        private readonly string inputText;
        private int min;
        private int max;
        public Action CloseAction { get; set; }
        public SearchFilterResultModel SearchFilterResult { get; private set; }

        public FiltersModel(string inputText, Func<SearchFilterResultModel, SearchResultModel> refreshAction)
        {
            this.inputText = inputText;
            this.refreshAction = refreshAction;

            ClearCommand = new MvxCommand(() => Reset());
            CancelCommand = new MvxCommand(Hide);
            ApplyFiltersCommand = new MvxCommand(ApplyFilters);

            DateFilters = new DateFilterModel();
            FileFormatFilters = new FileFormatFilterModel();
            SortOrderFilters = new SortFormatFilterModel();
            HealthFiltes = new HealthFilterModel();
            RangeFilter = new RangeSliderModel();

            Results.CollectionChanged += (o, s) => RaisePropertyChanged(nameof(IsVisible));

            InitSearch();
        }

        private void RefreshTabs()
        {
            var toAdd = SearchFilterResult.Get();
            Results.Clear();
            toAdd.Foreach(x => Results.Add(x));
        }

        public MvxObservableCollection<FilterResultModel> Results { get; } = new MvxObservableCollection<FilterResultModel>();

        public bool IsVisible => Results.Any();

        public SearchResultModel GetData(Action<SearchResultRowModel> downloadAction)
        {
            var res = refreshAction(SearchFilterResult);
            SetMinMax(res.Size.Item1, res.Size.Item2);
            res.Rows.Foreach(x => x.DownloadAction = downloadAction);
            return res;
        }

        public DateFilterModel DateFilters { get; }
        public FileFormatFilterModel FileFormatFilters { get; }
        public SortFormatFilterModel SortOrderFilters { get; }
        public RangeSliderModel RangeFilter { get; }
        public HealthFilterModel HealthFiltes { get; }

        public IMvxCommand CancelCommand { get; }

        public IMvxCommand ApplyFiltersCommand { get; }

        public IMvxCommand ClearCommand { get; }

        public void SetMinMax(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public void Reset(bool withApply = false)
        {
            Reset(SearchFiltersType.FileFormats);
            Reset(SearchFiltersType.Sortorder);
            Reset(SearchFiltersType.TimePeriods);
            Reset(SearchFiltersType.Size);
            Reset(SearchFiltersType.HealthType);

            if (withApply) Apply();
        }

        private void Hide() => CloseAction?.Invoke();

        private void ApplyFilters()
        {
            Apply();
            Hide();
        }

        public void BindFromSearchFilterResult()
        {
            DateFilters.Set(SearchFilterResult.Time);
            FileFormatFilters.Set(SearchFilterResult.FileFormat);
            SortOrderFilters.Set(SearchFilterResult.Order);
            HealthFiltes.Set(SearchFilterResult.Health);

            RangeFilter.CurrentMax = SearchFilterResult.SizeTo;
            RangeFilter.CurrentMin = SearchFilterResult.SizeFrom;
            RangeFilter.Max = SearchFilterResult.SizeMax;
            RangeFilter.Min = SearchFilterResult.SizeMin;
        }

        private void Apply()
        {
            InitSearch();
            SearchFilterResult.Order = SortOrderFilters.IsSelected ? SortOrderFilters.GetSelected() : null;
            SearchFilterResult.FileFormat = FileFormatFilters.IsSelected ? FileFormatFilters.GetSelected() : null;
            SearchFilterResult.Time = DateFilters.IsSelected ? DateFilters.GetSelected() : null;
            SearchFilterResult.Health = HealthFiltes.IsSelected ? HealthFiltes.GetSelected() : null;

            SearchFilterResult.SizeFrom = RangeFilter.CurrentMin;
            SearchFilterResult.SizeTo = RangeFilter.CurrentMax;
            SearchFilterResult.SizeMin = RangeFilter.Min;
            SearchFilterResult.SizeMax = RangeFilter.Max;

            RefreshTabs();
        }

        private void InitSearch() => SearchFilterResult = new SearchFilterResultModel { OnRemoveAction = RemoveAction, SizeMin = min, SizeMax = max, InputText = inputText };

        private void RemoveAction(SearchFiltersType type)
        {
            Reset(type);
            Apply();
        }

        private void Reset(SearchFiltersType type)
        {
            switch (type)
            {
                case SearchFiltersType.Size:
                    RangeFilter.Min = SearchFilterResult.SizeMin;
                    RangeFilter.Max = SearchFilterResult.SizeMax;
                    RangeFilter.CurrentMax = SearchFilterResult.SizeMax;
                    RangeFilter.CurrentMin = SearchFilterResult.SizeMin;
                    break;

                case SearchFiltersType.HealthType:
                    HealthFiltes.DeselctAll();
                    break;

                case SearchFiltersType.FileFormats:
                    FileFormatFilters.DeselctAll();
                    break;

                case SearchFiltersType.Sortorder:
                    SortOrderFilters.DeselctAll();
                    break;

                case SearchFiltersType.TimePeriods:
                    DateFilters.DeselctAll();
                    break;
            }
        }
    }
}
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Models
{
    public class FiltersModel : MvxNotifyPropertyChanged
    {
        private readonly string inputText;
        private readonly Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction;
        private int max;
        private int min;
        private string uuId;

        public FiltersModel(string inputText, Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction)
        {
            this.inputText = inputText;
            this.refreshAction = refreshAction;

            ClearCommand = new MvxCommand(() => Reset());
            CancelCommand = new MvxCommand(Hide);
            ApplyFiltersCommand = new MvxCommand(ApplyFilters);

            DateFilters = new DateFilterModel((x) => ShowCalendar = x);
            FileFormatFilters = new FileFormatFilterModel();
            HealthFiltes = new HealthFilterModel();
            RangeFilter = new RangeSliderModel();

            Results.CollectionChanged += (o, s) => RaisePropertyChanged(nameof(IsVisible));

            InitSearch();
        }

        private bool showCalendar;

        public bool ShowCalendar
        {
            get => showCalendar;
            set => SetProperty(ref showCalendar, value);
        }

        public IMvxCommand ApplyFiltersCommand { get; }
        public IMvxCommand CancelCommand { get; }
        public IMvxCommand ClearCommand { get; }
        public Action<bool> CloseAction { get; set; }
        public DateFilterModel DateFilters { get; }
        public FileFormatFilterModel FileFormatFilters { get; }
        public HealthFilterModel HealthFiltes { get; }
        public bool IsVisible => Results.Any();
        public RangeSliderModel RangeFilter { get; }
        public MvxObservableCollection<FilterResultModel> Results { get; } = new MvxObservableCollection<FilterResultModel>();
        public SearchFilterResultModel SearchFilterResult { get; private set; }

        public void BindFromSearchFilterResult()
        {
            DateFilters.Set(SearchFilterResult.Time);
            FileFormatFilters.Set(SearchFilterResult.FileFormats);
            HealthFiltes.Set(SearchFilterResult.Healths);

            RangeFilter.Max = SearchFilterResult.SizeMax;
            RangeFilter.Min = SearchFilterResult.SizeMin;
            RangeFilter.CurrentMax = SearchFilterResult.SizeTo;
            RangeFilter.CurrentMin = SearchFilterResult.SizeFrom;
        }

        public async Task<SearchResultModel> GetData(Action<SearchResultRowModel> downloadAction)
        {
            var res = await refreshAction(SearchFilterResult);
            uuId = res.Id;
            SetMinMax(res.Size.Item1, res.Size.Item2);
            res.Rows.Foreach(x => x.DownloadAction = downloadAction);
            return res;
        }

        public void Reset(bool withApply = false)
        {
            Reset(SearchFiltersType.FileFormats);
            Reset(SearchFiltersType.TimePeriods);
            Reset(SearchFiltersType.Size);
            Reset(SearchFiltersType.HealthType);

            if (withApply) Apply();
        }

        public void SetMinMax(int min, int max)
        {
            this.min = min;
            this.max = max;
            SearchFilterResult.SizeMax = max;
            SearchFilterResult.SizeMin = min;
            if (SearchFilterResult.SizeTo > max || SearchFilterResult.SizeTo <= min) SearchFilterResult.SizeTo = max;
            if (SearchFilterResult.SizeFrom < min || SearchFilterResult.SizeFrom >= max) SearchFilterResult.SizeFrom = min;
        }

        private void Apply()
        {
            InitSearch();
            SearchFilterResult.FileFormats = FileFormatFilters.IsSelected ? FileFormatFilters.GetAllSelected() : null;
            SearchFilterResult.Time = DateFilters.IsSelected ? DateFilters.GetSelected() : null;
            SearchFilterResult.Healths = HealthFiltes.IsSelected ? HealthFiltes.GetAllSelected() : null;

            SearchFilterResult.SizeFrom = RangeFilter.CurrentMin;
            SearchFilterResult.SizeTo = RangeFilter.CurrentMax;
            SearchFilterResult.SizeMin = RangeFilter.Min;
            SearchFilterResult.SizeMax = RangeFilter.Max;

            RefreshTabs();
        }

        private void ApplyFilters()
        {
            Apply();
            CloseAction?.Invoke(true);
        }

        private void Hide() => CloseAction?.Invoke(false);

        private void InitSearch() => SearchFilterResult = new SearchFilterResultModel { OnRemoveAction = RemoveAction, SizeMin = min, SizeMax = max, SizeFrom = min, SizeTo = max, InputText = inputText, PrevId = uuId };

        private void RefreshTabs()
        {
            var toAdd = SearchFilterResult.Get();
            Results.Clear();
            toAdd.Foreach(x => Results.Add(x));
        }

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

                case SearchFiltersType.TimePeriods:
                    DateFilters.DeselctAll();
                    break;
            }
        }
    }
}
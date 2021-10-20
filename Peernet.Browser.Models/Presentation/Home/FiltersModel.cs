using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class FiltersModel : MvxNotifyPropertyChanged
    {
        private readonly string inputText;
        private readonly Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction;
        private bool showCalendar;
        public string UuId { get; private set; }

        public FiltersModel(string inputText, Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction)
        {
            this.inputText = inputText;
            this.refreshAction = refreshAction;

            ClearCommand = new MvxCommand(() => Reset());
            CancelCommand = new MvxCommand(Hide);
            ApplyFiltersCommand = new MvxCommand(ApplyFilters);

            DateFilters = new DateFilterModel((x) => ShowCalendar = x);
            FileFormatFilters = new FileFormatFilterModel();
            Dates = new CalendarModel();

            Results.CollectionChanged += (o, s) => RaisePropertyChanged(nameof(IsVisible));

            InitSearch();
        }

        public IMvxCommand ApplyFiltersCommand { get; }

        public IMvxCommand CancelCommand { get; }

        public IMvxCommand ClearCommand { get; }

        public Action<bool> CloseAction { get; set; }

        public DateFilterModel DateFilters { get; }

        public CalendarModel Dates { get; }

        public FileFormatFilterModel FileFormatFilters { get; }

        public bool IsVisible => Results.Any();

        public MvxObservableCollection<FilterResultModel> Results { get; } = new MvxObservableCollection<FilterResultModel>();

        public SearchFilterResultModel SearchFilterResult { get; private set; }

        public bool ShowCalendar
        {
            get => showCalendar;
            set => SetProperty(ref showCalendar, value);
        }

        private int? sizeFrom;

        public int? SizeFrom
        {
            get => sizeFrom;
            set => SetProperty(ref sizeFrom, value);
        }

        private int? sizeTo;

        public int? SizeTo
        {
            get => sizeTo;
            set => SetProperty(ref sizeTo, value);
        }

        public void BindFromSearchFilterResult()
        {
            DateFilters.Set(SearchFilterResult.Time);
            FileFormatFilters.Set(SearchFilterResult.FileFormats);

            SizeTo = SearchFilterResult.SizeTo;
            SizeFrom = SearchFilterResult.SizeFrom;

            Dates.Set(SearchFilterResult.TimeFrom, SearchFilterResult.TimeTo);
        }

        public async Task<SearchResultModel> GetData()
        {
            var res = await refreshAction(SearchFilterResult);
            UuId = res.Id;
            return res;
        }

        public void Reset(bool withApply = false)
        {
            Reset(SearchFiltersType.FileFormats);
            Reset(SearchFiltersType.TimePeriods);
            Reset(SearchFiltersType.Size);
            Dates.Reset();

            if (withApply) Apply();
        }

        private void Apply()
        {
            InitSearch();
            SearchFilterResult.FileFormats = FileFormatFilters.IsSelected ? FileFormatFilters.GetAllSelected() : null;
            SearchFilterResult.Time = DateFilters.IsSelected ? DateFilters.GetSelected() : null;

            SearchFilterResult.TimeFrom = Dates.DateFrom;
            SearchFilterResult.TimeTo = Dates.DateTo;

            SearchFilterResult.SizeFrom = SizeFrom;
            SearchFilterResult.SizeTo = SizeTo;

            RefreshTabs();
        }

        private void ApplyFilters()
        {
            Apply();
            CloseAction?.Invoke(true);
        }

        private void Hide()
        {
            Reset();
            CloseAction?.Invoke(false);
        }

        private void InitSearch() => SearchFilterResult = new SearchFilterResultModel { OnRemoveAction = RemoveAction, InputText = inputText, Uuid = UuId };

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
                    SizeFrom = null;
                    SizeTo = null;
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
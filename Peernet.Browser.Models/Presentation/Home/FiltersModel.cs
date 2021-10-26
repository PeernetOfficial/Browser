using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class FiltersModel : MvxNotifyPropertyChanged
    {
        private readonly string inputText;
        private bool showCalendar;
        public string UuId { get; set; }

        public FiltersModel(string inputText)
        {
            this.inputText = inputText;

            ClearCommand = new MvxCommand(() => Reset());
            CancelCommand = new MvxAsyncCommand(Hide);
            ApplyFiltersCommand = new MvxAsyncCommand(ApplyFilters);

            DateFilters = new DateFilterModel((x) => ShowCalendar = x);
            FileFormatFilters = new FileFormatFilterModel();
            Dates = new CalendarModel();

            Results.CollectionChanged += (o, s) => RaisePropertyChanged(nameof(IsVisible));

            InitSearch();
        }

        public IMvxCommand ApplyFiltersCommand { get; }

        public IMvxCommand CancelCommand { get; }

        public IMvxCommand ClearCommand { get; }

        public Func<bool, Task> CloseAction { get; set; }

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

        private async Task ApplyFilters()
        {
            Apply();
            await CloseAction?.Invoke(true);
        }

        private async Task Hide()
        {
            Reset();
            await CloseAction?.Invoke(false);
        }

        private void InitSearch() => SearchFilterResult = new SearchFilterResultModel { InputText = inputText, Uuid = UuId };

        private void RefreshTabs()
        {
            var toAdd = GetTabs();
            Results.Clear();
            toAdd.Foreach(x => Results.Add(x));
        }

        public void RemoveAction(SearchFiltersType type)
        {
            Reset(type);
            Apply();
            RefreshTabs();
        }

        private IEnumerable<FilterResultModel> GetTabs()
        {
            var res = new List<FilterResultModel>();
            if (SearchFilterResult.IsCustomTimeFill)
            {
                res.Add(new FilterResultModel
                {
                    Type = SearchFiltersType.TimePeriods,
                    Content = SearchFilterResult.Time == TimePeriods.Custom ? $"{SearchFilterResult.TimeFrom.Value.ToShortDateString()} - {SearchFilterResult.TimeTo.Value.ToShortDateString()}" : SearchFilterResult.Time.Value.GetDescription()
                });
            }
            if (!SearchFilterResult.FileFormats.IsNullOrEmpty())
            {
                SearchFilterResult.FileFormats.Foreach(x => res.Add(new FilterResultModel { Type = SearchFiltersType.FileFormats, Content = x.GetDescription() }));
            }
            if (SizeFrom.HasValue && SizeTo.HasValue)
            {
                res.Add(new FilterResultModel { Type = SearchFiltersType.Size, Content = $"{SizeFrom}MB - {SizeTo}MB" });
            }
            return res;
        }

        public void Reset(SearchFiltersType type)
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
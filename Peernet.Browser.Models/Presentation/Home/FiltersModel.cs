using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class FiltersModel : INotifyPropertyChanged
    {
        private readonly string inputText;

        public event PropertyChangedEventHandler PropertyChanged;

        public string UuId { get; set; }

        public FiltersModel(string inputText)
        {
            this.inputText = inputText;

            ClearCommand = new AsyncCommand(() =>
            {
                Reset();
                return Task.CompletedTask;
            });

            DateFilters = new DateFilterModel(Apply);
            FileFormatFilters = new FileFormatFilterModel(Apply);

            Results.CollectionChanged += (o, s) => PropertyChanged?.Invoke(this, new(nameof(IsVisible)));

            InitSearch();
        }

        public IAsyncCommand ClearCommand { get; }

        public DateFilterModel DateFilters { get; }

        public FileFormatFilterModel FileFormatFilters { get; }

        public bool IsVisible => Results.Any();

        public ObservableCollection<FilterResultModel> Results { get; } = new ObservableCollection<FilterResultModel>();

        public SearchFilterResultModel SearchFilterResult { get; private set; }

        public void BindFromSearchFilterResult()
        {
            DateFilters.Set(SearchFilterResult.Time);
            FileFormatFilters.Set(SearchFilterResult.FileFormat);
        }

        public void Reset(bool withApply = false)
        {
            Reset(SearchFiltersType.FileFormats);
            Reset(SearchFiltersType.TimePeriods);

            if (withApply)
            {
                Apply();
            }
        }

        public void Apply()
        {
            InitSearch();
            SearchFilterResult.FileFormat = FileFormatFilters.GetSelected();
            SearchFilterResult.Time = DateFilters.GetSelected();

            RefreshTabs();
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
            if (SearchFilterResult.Time.HasValue && SearchFilterResult.Time != TimePeriods.None)
            {
                res.Add(new FilterResultModel
                {
                    Type = SearchFiltersType.TimePeriods,
                    Content = SearchFilterResult.Time.Value.GetDescription()
                });
            }
            if (SearchFilterResult.FileFormat != FileFormat.None)
            {
                res.Add(new FilterResultModel
                { Type = SearchFiltersType.FileFormats, Content = SearchFilterResult.FileFormat.GetDescription() });
            }
            return res;
        }

        public void Reset(SearchFiltersType type)
        {
            switch (type)
            {
                case SearchFiltersType.FileFormats:
                    FileFormatFilters.UnselectAll();
                    break;

                case SearchFiltersType.TimePeriods:
                    DateFilters.UnselectAll();
                    break;
            }
        }
    }
}
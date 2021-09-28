﻿using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Enums;
using Peernet.Browser.Application.ViewModels;
using System;

namespace Peernet.Browser.Application.Models
{
    public class SearchContentElement : MvxNotifyPropertyChanged
    {
        private readonly IMvxNavigationService navigationService;
        public MvxObservableCollection<IconModel> FilterIconModels { get; } = new MvxObservableCollection<IconModel>();

        public MvxObservableCollection<SearchResult> TableResult { get; } = new MvxObservableCollection<SearchResult>();

        public IconModel FiltersIconModel { get; }
        public IconModel ColumnsIconModel { get; }

        public FiltersModel Filters { get; }

        public SearchContentElement()
        {
            navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();

            FilterIconModels.Add(new IconModel(FiltersType.All, count: 2357));
            FilterIconModels.Add(new IconModel(FiltersType.Audio, count: 217));
            FilterIconModels.Add(new IconModel(FiltersType.Video, count: 844));
            FilterIconModels.Add(new IconModel(FiltersType.Ebooks, count: 629));
            FilterIconModels.Add(new IconModel(FiltersType.Documents, count: 632));
            FilterIconModels.Add(new IconModel(FiltersType.Pictures, count: 214));
            FilterIconModels.Add(new IconModel(FiltersType.Text, count: 182));
            FilterIconModels.Add(new IconModel(FiltersType.Binary, count: 1));

            ColumnsIconModel = new IconModel(FiltersType.Columns, true);
            FiltersIconModel = new IconModel(FiltersType.Filters, true, OpenFilters);

            Filters = new FiltersModel();

            Filters.Refresh(new[] { "Last Week", "Word File", "2GB - 10GB" });

            for (var i = 0; i < 100; i++)
            {
                TableResult.Add(new SearchResult(new ApiBlockRecordFile { Date = DateTime.Now.AddMinutes(i), Name = $"Name_{i}", Size = i }, Download));
            }
        }

        private void Download(SearchResult row)
        {
        }

        private void OpenFilters()
        {
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            navigationService.Navigate<FiltersViewModel>();
        }
    }
}
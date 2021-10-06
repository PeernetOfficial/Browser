using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Presentation;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchContentElementViewModel : MvxNotifyPropertyChanged
    {
        private readonly IMvxNavigationService navigationService;

        public MvxObservableCollection<IconModel> FilterIconModels { get; } = new MvxObservableCollection<IconModel>();

        public MvxObservableCollection<SearchResultRowModel> TableResult { get; } = new MvxObservableCollection<SearchResultRowModel>();

        public IconModel FiltersIconModel { get; }
        public IconModel ColumnsIconModel { get; }

        public FiltersModel Filters { get; }

        private readonly List<ApiBlockRecordFile> Files = new List<ApiBlockRecordFile>();

        public SearchContentElementViewModel()
        {
            PrepareFakeFiles();

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
            Filters.Reset(0, 15);
            Filters.Refresh();

            RefreshTable();
        }

        private void PrepareFakeFiles()
        {
            for (var i = 0; i < 100; i++)
            {
                var id = i % 5;
                Files.Add(new ApiBlockRecordFile { Id = id.ToString(), Date = DateTime.Now.AddMinutes(i), Name = $"Name_{i}", Size = i });
            }
        }

        private void RefreshTable()
        {
            TableResult.Clear();
            TableResult.AddRange(Files.Select(x => new SearchResultRowModel(x, Download)));
        }

        private void Download(SearchResultRowModel row)
        {
        }

        private void OpenFilters()
        {
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            navigationService.Navigate<FiltersViewModel, FiltersModel>(Filters);
        }
    }
}
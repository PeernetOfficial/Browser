﻿using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class SearchTabElementViewModel : MvxViewModel
    {
        private readonly Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction;
        private bool showColumnsDate = true;
        private bool showColumnsDownload = true;
        private bool showColumnsSelector;
        private bool showColumnsShared = true;
        private bool showColumnsSize = true;

        public SearchTabElementViewModel(string title, Func<SearchTabElementViewModel, Task> deleteAction, Func<SearchFilterResultModel, Task<SearchResultModel>> refreshAction, Func<SearchResultRowModel, Task> downloadAction) : base()
        {
            this.refreshAction = refreshAction;
            Title = title;

            Filters = new FiltersModel(title);
            Filters.CloseAction += async (x) =>
            {
                if (x)
                {
                    await Refresh();
                }
                FiltersIconModel.IsSelected = false;
            };

            ClearCommand = new MvxAsyncCommand(async () =>
            {
                Filters.Reset(true);
                await Refresh();
            });
            DownloadCommand = new MvxAsyncCommand<SearchResultRowModel>(async (row) => await downloadAction(row));
            DeleteCommand = new MvxAsyncCommand(async () => { await deleteAction(this); });
            RemoveFilterCommand = new MvxAsyncCommand<SearchFiltersType>(async (type) =>
            {
                Filters.RemoveAction(type);
                await Refresh();
            });

            ColumnsIconModel = new IconModel(FiltersType.Columns, true, ShowColumnSelection);
            FiltersIconModel = new IconModel(FiltersType.Filters, true, OpenFilters);

            Map.Fill(new[] { new GeoPoint { Longitude = 19, Latitude = 49 }, new GeoPoint { Longitude = 0, Latitude = 0 } });

            InitIcons();
            Loader.Set("Searching...");
            Task.Run(Refresh);
        }

        public async override Task Initialize()
        {
            await Refresh();
            await base.Initialize();
        }

        public IMvxAsyncCommand ClearCommand { get; }
        public MvxObservableCollection<CustomCheckBoxModel> ColumnsCheckboxes { get; } = new MvxObservableCollection<CustomCheckBoxModel>();

        public LoadingModel Loader { get; } = new LoadingModel();
        public IconModel ColumnsIconModel { get; }
        public IMvxAsyncCommand DeleteCommand { get; }
        public IMvxAsyncCommand<SearchFiltersType> RemoveFilterCommand { get; }
        public IMvxAsyncCommand<SearchResultRowModel> DownloadCommand { get; }
        public MvxObservableCollection<IconModel> FilterIconModels { get; } = new MvxObservableCollection<IconModel>();
        public MapModel Map { get; } = new MapModel { Width = 318, Height = 231 };
        public FiltersModel Filters { get; }
        public IconModel FiltersIconModel { get; }

        public bool ShowColumnsDate
        {
            get => showColumnsDate;
            set => SetProperty(ref showColumnsDate, value);
        }

        public bool ShowColumnsDownload
        {
            get => showColumnsDownload;
            set => SetProperty(ref showColumnsDownload, value);
        }

        public bool ShowColumnsSelector
        {
            get => showColumnsSelector;
            set => SetProperty(ref showColumnsSelector, value);
        }

        public bool ShowColumnsShared
        {
            get => showColumnsShared;
            set => SetProperty(ref showColumnsShared, value);
        }

        public bool ShowColumnsSize
        {
            get => showColumnsSize;
            set => SetProperty(ref showColumnsSize, value);
        }

        public MvxObservableCollection<SearchResultRowModel> TableResult { get; } = new MvxObservableCollection<SearchResultRowModel>();
        public string Title { get; }

        public async Task OnSorting(string columnName, DataGridSortingTypeEnum type)
        {
            Filters.SearchFilterResult.SortName = SearchResultRowModel.Parse(columnName);
            Filters.SearchFilterResult.SortType = type;
            await Refresh();
        }

        public async Task Refresh()
        {
            SearchResultModel data = await refreshAction(Filters.SearchFilterResult);
            await GlobalContext.UiThreadDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                Filters.UuId = data.Id;
                TableResult.Clear();
                data.Rows.Foreach(x => TableResult.Add(x));
                RefreshIconFilters(data.Stats, data.Filters.FilterType);
                Loader.Reset();
            });
        }

        private void InitIcons()
        {
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Date", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Size", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Downloads", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            ColumnsCheckboxes.Add(new CustomCheckBoxModel { Content = "Shared by", IsChecked = true, IsCheckChanged = OnColumnCheckboxClick });
            RefreshIconFilters(SearchResultModel.GetDefaultStats().ToDictionary(x => x, y => 0), FiltersType.All);
        }

        private void OnColumnCheckboxClick(CustomCheckBoxModel selection)
        {
            switch (selection.Content)
            {
                case "Date":
                    ShowColumnsDate = selection.IsChecked;
                    break;

                case "Size":
                    ShowColumnsSize = selection.IsChecked;
                    break;

                case "Downloads":
                    ShowColumnsDownload = selection.IsChecked;
                    break;

                case "Shared by":
                    ShowColumnsShared = selection.IsChecked;
                    break;
            }
        }

        private async Task OnFilterIconClick(IconModel i)
        {
            FilterIconModels.Where(x => x != i).Foreach(x => x.IsSelected = false);
            i.IsSelected = true;
            Filters.SearchFilterResult.FilterType = i.FilterType;
            await Refresh();
        }

        private async Task OpenFilters(IconModel m)
        {
            var navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            Filters.BindFromSearchFilterResult();
            await navigationService.Navigate<FiltersViewModel, FiltersModel>(Filters);
        }

        private void RefreshIconFilters(IDictionary<FiltersType, int> stats, FiltersType selected)
        {
            FilterIconModels.Clear();
            stats.Foreach(x => FilterIconModels.Add(new IconModel(x.Key, onClick: OnFilterIconClick, count: x.Value) { IsSelected = x.Key == selected }));
        }

        private async Task ShowColumnSelection(IconModel i)
        {
            ShowColumnsSelector = !ShowColumnsSelector;
            await Task.CompletedTask;
        }
    }
}
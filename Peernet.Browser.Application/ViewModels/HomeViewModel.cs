using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.Utilities;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using Peernet.SDK.Models.Presentation.Home;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class HomeViewModel : NavigationItemViewModelBase
    {
        private readonly IBlockchainService blockchainService;
        private readonly IDataTransferManager dataTransferManager;
        private readonly IDownloadClient downloadClient;
        private readonly IModalNavigationService modalNavigationService;
        private readonly INavigationService navigationService;
        private readonly IEnumerable<IPlayButtonPlug> playButtonPlugs;
        private readonly ISearchService searchService;
        private readonly ISettingsManager settingsManager;
        private readonly IUserContext userContext;
        private readonly IWarehouseClient warehouseClient;
        private bool filtersActive;
        private string searchInput;
        private int selectedIndex = -1;

        public HomeViewModel(
            IDownloadClient downloadClient,
            ISettingsManager settingsManager,
            ISearchService searchService,
            IWarehouseClient warehouseClient,
            IBlockchainService blockchainService,
            IDataTransferManager dataTransferManager,
            INavigationService navigationService,
            IModalNavigationService modalNavigationService,
            IEnumerable<IPlayButtonPlug> playButtonPlugs,
            IUserContext userContext)
        {
            this.downloadClient = downloadClient;
            this.settingsManager = settingsManager;
            this.searchService = searchService;
            this.warehouseClient = warehouseClient;
            this.blockchainService = blockchainService;
            this.dataTransferManager = dataTransferManager;
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            this.modalNavigationService = modalNavigationService ?? throw new ArgumentNullException(nameof(modalNavigationService));
            this.playButtonPlugs = playButtonPlugs;
            this.userContext = userContext;

            SearchCommand = new AsyncCommand(Search);
            Tabs.CollectionChanged += (o, s) =>
            {
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(IsNotVisible));
                OnPropertyChanged(nameof(Alignment));
                GlobalContext.IsLogoVisible = IsVisible;
            };
        }

        public AdvancedFilterModel AdvancedFilter { get; set; } = new();

        public Alignments Alignment => IsVisible ? Alignments.Stretch : Alignments.Center;

        public SearchTabElementViewModel Content => SelectedIndex < 0 ? null : Tabs[SelectedIndex];

        public bool FiltersActive
        {
            get => filtersActive;
            set
            {
                filtersActive = value;
                OnPropertyChanged(nameof(FiltersActive));
            }
        }

        public bool IsNotVisible => !IsVisible;

        public bool IsVisible => Tabs.Any();

        public IAsyncCommand OpenAdvancedOptionsCommand =>
            new AsyncCommand(async () =>
            {
                await modalNavigationService.Navigate<AdvancedSearchOptionsViewModel, AdvancedFilterModel>(AdvancedFilter);
            });

        public IAsyncCommand SearchCommand { get; }

        public string SearchInput
        {
            get => searchInput;
            set
            {
                searchInput = value;
                OnPropertyChanged(nameof(SearchInput));
            }
        }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
                OnPropertyChanged(nameof(Content));
            }
        }

        public ObservableCollection<SearchTabElementViewModel> Tabs { get; } = new ObservableCollection<SearchTabElementViewModel>();

        public void AddNewTab(SearchTabElementViewModel tab)
        {
            Tabs.Add(tab);
            SelectedIndex = Tabs.Count - 1;
            SearchInput = string.Empty;
        }

        public SearchFilterResultModel CreateNewSearchFilter()
        {
            return new SearchFilterResultModel
            {
                InputText = SearchInput,
                AdvancedFilter = AdvancedFilter
            };
        }

        public bool DoesSupportPlaying(DownloadModel model)
        {
            return playButtonPlugs.Any(plugin => plugin?.IsSupported(model.File) == true);
        }

        public void ExecutePlayButtonPlug(DownloadModel model)
        {
            playButtonPlugs.Foreach(async plug =>
            {
                if (plug?.IsSupported(model.File) == true)
                {
                    await plug?.Execute(model.File);
                }
            });
        }

        public void OpenFile(DownloadModel model)
        {
            var filePath = Path.Combine(settingsManager.DownloadPath, UtilityHelper.StripInvalidWindowsCharactersFromFileName(model.File.Name));
            var download = new SDK.Models.Presentation.Download(downloadClient, model.File, filePath);
            var param = new FilePreviewViewModelParameter(model.File, async () => await dataTransferManager.QueueUp(download), "Download");
            navigationService.Navigate<FilePreviewViewModel, FilePreviewViewModelParameter>(param);
        }

        public async Task RemoveTab(SearchTabElementViewModel e)
        {
            await searchService.Terminate(e.Filters.UuId);
            Tabs.Remove(e);
            SelectedIndex = IsVisible ? 0 : -1;
        }

        public override int GetNavigationIndex() => 0;

        private Task Search()
        {
            var toAdd = new ActiveSearchTabElementViewModel(
                CreateNewSearchFilter(),
                RemoveTab,
                settingsManager,
                downloadClient,
                OpenFile,
                ExecutePlayButtonPlug,
                DoesSupportPlaying,
                searchService,
                warehouseClient,
                dataTransferManager,
                blockchainService,
                userContext);

            AddNewTab(toAdd);
            return Task.CompletedTask;
        }
    }
}
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Peernet.Browser.Application.VirtualFileSystem;

namespace Peernet.Browser.Application.ViewModels
{
    public class DirectoryViewModel : MvxViewModel, ISearchable
    {
        private readonly IBlockchainService blockchainService;
        private List<ApiBlockRecordFile> activeSearchResults;
        private string searchInput;
        private IReadOnlyCollection<ApiBlockRecordFile> sharedFiles;
        private bool showHint = true;
        private bool showSearchBox;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private ObservableCollection<VirtualFileSystemTier> pathElements;

        public DirectoryViewModel(IBlockchainService blockchainService, IVirtualFileSystemFactory virtualFileSystemFactory)
        {
            this.blockchainService = blockchainService;
            this.virtualFileSystemFactory = virtualFileSystemFactory;
        }

        public List<ApiBlockRecordFile> ActiveSearchResults
        {
            get => activeSearchResults;
            set => SetProperty(ref activeSearchResults, value);
        }

        public VirtualFileSystem.VirtualFileSystem VirtualFileSystem
        {
            get
            {
                virtualFileSystem.Sort();
                return virtualFileSystem;
            }
            set => SetProperty(ref virtualFileSystem, value);
        }

        public ObservableCollection<VirtualFileSystemTier> PathElements
        {
            get => pathElements;
            set => SetProperty(ref pathElements, value);
        }

        public IMvxAsyncCommand<ApiBlockRecordFile> DeleteCommand =>
            new MvxAsyncCommand<ApiBlockRecordFile>(
                apiBlockRecordFile =>
                {
                    blockchainService.DeleteSelfFile(apiBlockRecordFile);

                    return Task.CompletedTask;
                });


        public IMvxAsyncCommand<ApiBlockRecordFile> EditCommand =>
            new MvxAsyncCommand<ApiBlockRecordFile>(
                apiBlockRecordFile =>
                {
                    // Logic to be implemented

                    return Task.CompletedTask;
                });

        public IMvxCommand RemoveHint
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (ShowHint)
                    {
                        ShowHint = false;
                        ShowSearchBox = true;
                    }
                });
            }
        }

        public IMvxCommand Search
        {
            get { return new MvxCommand(() => { ActiveSearchResults = ApplySearchResultsFiltering(sharedFiles); }); }
        }

        public IMvxCommand<VirtualFileSystemEntity> UpdateActiveSearchResults =>
            new MvxCommand<VirtualFileSystemEntity>(
                tier => { ActiveSearchResults = ApplySearchResultsFiltering(tier.GetAllFiles()); });


        public string SearchInput
        {
            get => searchInput;
            set => SetProperty(ref searchInput, value);
        }

        public IMvxAsyncCommand<ApiBlockRecordFile> ShareCommand =>
            new MvxAsyncCommand<ApiBlockRecordFile>(
                apiBlockRecordFile =>
                {
                    // Logic to be implemented

                    return Task.CompletedTask;
                });

        public bool ShowHint
        {
            get => showHint;
            set => SetProperty(ref showHint, value);
        }

        public bool ShowSearchBox
        {
            get => showSearchBox;
            set => SetProperty(ref showSearchBox, value);
        }

        public override Task Initialize()
        {
            var header = blockchainService.GetSelfHeader();
            if (header.Height > 0)
            {
                sharedFiles = blockchainService.GetSelfList().Files;
                ActiveSearchResults = sharedFiles.ToList();
            }

            VirtualFileSystem = virtualFileSystemFactory.CreateVirtualFileSystem(sharedFiles);
            AddRecentTier(sharedFiles);
            AddAllFilesTier(sharedFiles);

            return base.Initialize();
        }

        private List<ApiBlockRecordFile> ApplySearchResultsFiltering(IEnumerable<ApiBlockRecordFile> results)
        {
            return !string.IsNullOrEmpty(SearchInput)
                ? results.Where(f => f.Name.Contains(SearchInput, StringComparison.OrdinalIgnoreCase)).ToList()
                : results.ToList();
        }

        private void AddRecentTier(IEnumerable<ApiBlockRecordFile> allFiles)
        {
            var filtered = allFiles.OrderByDescending(f => f.Date).Take(10);
            AddTier("Recent", VirtualFileSystemEntityType.Recent, 0, filtered);
        }

        private void AddAllFilesTier(IEnumerable<ApiBlockRecordFile> allFiles)
        {
            AddTier("All files", VirtualFileSystemEntityType.All, 0, allFiles);
        }

        private void AddTier(string name, VirtualFileSystemEntityType type, int depth, IEnumerable<ApiBlockRecordFile> files)
        {
            var tier = new VirtualFileSystemTier(name, type, depth);
            tier.Files.AddRange(files);

            VirtualFileSystem.VirtualFileSystemTiers.Add(tier);
        }
    }
}
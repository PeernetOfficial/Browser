﻿using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Models.Domain.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class DirectoryViewModel : MvxViewModel, ISearchable
    {
        private readonly IBlockchainService blockchainService;
        private readonly IVirtualFileSystemFactory virtualFileSystemFactory;
        private List<ApiFile> activeSearchResults;
        private ObservableCollection<VirtualFileSystemEntity> pathElements;
        private string searchInput;
        private IReadOnlyCollection<ApiFile> sharedFiles;
        private bool showHint = true;
        private bool showSearchBox;
        private VirtualFileSystem.VirtualFileSystem virtualFileSystem;

        public DirectoryViewModel(IBlockchainService blockchainService, IVirtualFileSystemFactory virtualFileSystemFactory)
        {
            this.blockchainService = blockchainService;
            this.virtualFileSystemFactory = virtualFileSystemFactory;
        }

        public List<ApiFile> ActiveSearchResults
        {
            get => activeSearchResults;
            set => SetProperty(ref activeSearchResults, value);
        }

        public IMvxAsyncCommand<ApiFile> DeleteCommand =>
            new MvxAsyncCommand<ApiFile>(
                async apiBlockRecordFile =>
                {
                    await blockchainService.DeleteSelfFile(apiBlockRecordFile);
                    await Initialize();
                });

        public ObservableCollection<VirtualFileSystemEntity> PathElements
        {
            get => pathElements;
            set => SetProperty(ref pathElements, value);
        }

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
            get
            {
                return new MvxCommand(() =>
                {
                    if (sharedFiles != null)
                    {
                        ActiveSearchResults = ApplySearchResultsFiltering(sharedFiles);
                    }
                });
            }
        }

        public string SearchInput
        {
            get => searchInput;
            set => SetProperty(ref searchInput, value);
        }

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

        public IMvxCommand<VirtualFileSystemEntity> UpdateActiveSearchResults =>
            new MvxCommand<VirtualFileSystemEntity>(
                tier => { ActiveSearchResults = ApplySearchResultsFiltering(tier.GetAllFiles()); });

        public VirtualFileSystem.VirtualFileSystem VirtualFileSystem
        {
            get => virtualFileSystem;
            set => SetProperty(ref virtualFileSystem, value);
        }

        public void ChangeSelectedEntity(VirtualFileSystemEntity entity)
        {
            VirtualFileSystem.ResetSelection();
            entity.IsSelected = true;
        }

        public override async Task Initialize()
        {
            var header = await blockchainService.GetSelfHeader();
            if (header.Height > 0)
            {
                sharedFiles = await blockchainService.GetSelfList() ?? new();
                ActiveSearchResults = sharedFiles?.ToList();
            }

            VirtualFileSystem = virtualFileSystemFactory.CreateVirtualFileSystem(sharedFiles);
            AddRecentTier(sharedFiles);
            AddAllFilesTier(sharedFiles);

            await base.Initialize();
        }

        private void AddAllFilesTier(IEnumerable<ApiFile> allFiles)
        {
            AddTier("All files", VirtualFileSystemEntityType.All, 0, allFiles);
        }

        private void AddRecentTier(IEnumerable<ApiFile> allFiles)
        {
            var filtered = allFiles.OrderByDescending(f => f.Date).Take(10);
            AddTier("Recent", VirtualFileSystemEntityType.Recent, 0, filtered);
        }

        private void AddTier(string name, VirtualFileSystemEntityType type, int depth, IEnumerable<ApiFile> files)
        {
            var tier = new VirtualFileSystemTier(name, type, depth);
            tier.Files.AddRange(files);

            VirtualFileSystem.VirtualFileSystemTiers.Add(tier);
        }

        private List<ApiFile> ApplySearchResultsFiltering(IEnumerable<ApiFile> results)
        {
            return !string.IsNullOrEmpty(SearchInput)
                ? results.Where(f => f.Name.Contains(SearchInput, StringComparison.OrdinalIgnoreCase)).ToList()
                : results.ToList();
        }
    }
}
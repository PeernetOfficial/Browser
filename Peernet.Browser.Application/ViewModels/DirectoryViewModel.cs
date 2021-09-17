﻿using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private VirtualFileSystem virtualFileSystem;

        public DirectoryViewModel(IBlockchainService blockchainService)
        {
            this.blockchainService = blockchainService;
        }

        public List<ApiBlockRecordFile> ActiveSearchResults
        {
            get => activeSearchResults;
            set => SetProperty(ref activeSearchResults, value);
        }

        public VirtualFileSystem VirtualFileSystem
        {
            get => virtualFileSystem;
            set => SetProperty(ref virtualFileSystem, value);
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

            VirtualFileSystem = new VirtualFileSystem(sharedFiles);
            AddRecentCategory(VirtualFileSystem, sharedFiles);
            AddAllFilesCategory(VirtualFileSystem, sharedFiles);

            return base.Initialize();
        }

        private List<ApiBlockRecordFile> ApplySearchResultsFiltering(IEnumerable<ApiBlockRecordFile> results)
        {
            return !string.IsNullOrEmpty(SearchInput)
                ? results.Where(f => f.Name.Contains(SearchInput, StringComparison.OrdinalIgnoreCase)).ToList()
                : results.ToList();
        }

        private void AddRecentCategory(VirtualFileSystem fileSystem, IEnumerable<ApiBlockRecordFile> allFiles)
        {
            var recentFilesTier = new VirtualFileSystemTier("Recent", 0);
            recentFilesTier.Files.AddRange(allFiles.OrderByDescending(f => f.Date).Take(10).ToList());


            fileSystem.VirtualFileSystemTiers.Add(recentFilesTier);
        }

        private void AddAllFilesCategory(VirtualFileSystem fileSystem, IEnumerable<ApiBlockRecordFile> allFiles)
        {
            var allFilesTier = new VirtualFileSystemTier("AllFiles", 0);
            allFilesTier.Files.AddRange(allFiles);

            fileSystem.VirtualFileSystemTiers.Add(allFilesTier);
        }
    }
}
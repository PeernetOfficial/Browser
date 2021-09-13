using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class DirectoryViewModel : MvxViewModel
    {
        private readonly IBlockchainService blockchainService;
        private List<ApiBlockRecordFile> sharedFiles;

        public DirectoryViewModel(IBlockchainService blockchainService)
        {
            this.blockchainService = blockchainService;
        }

        public IMvxAsyncCommand<ApiBlockRecordFile> DeleteCommand =>
            new MvxAsyncCommand<ApiBlockRecordFile>(
                (ApiBlockRecordFile apiBlockRecordFile) =>
            {
                // Logic to be implemented

                return Task.CompletedTask;
            });

        public IMvxAsyncCommand<ApiBlockRecordFile> EditCommand =>
            new MvxAsyncCommand<ApiBlockRecordFile>((ApiBlockRecordFile apiBlockRecordFile) =>
        {
            // Logic to be implemented

            return Task.CompletedTask;
        });

        public IMvxAsyncCommand<ApiBlockRecordFile> ShareCommand =>
            new MvxAsyncCommand<ApiBlockRecordFile>(
                (ApiBlockRecordFile apiBlockRecordFile) =>
        {
            // Logic to be implemented

            return Task.CompletedTask;
        });

        public List<ApiBlockRecordFile> SharedFiles
        {
            get => sharedFiles;
            set
            {
                sharedFiles = value;
                RaisePropertyChanged(nameof(SharedFiles));
            }
        }
        public override Task Initialize()
        {
            var header = blockchainService.GetSelfHeader();
            if (header.Height > 0)
            {
                SharedFiles = blockchainService.GetSelfList().Files;
            }

            return base.Initialize();
        }
    }
}
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

        public List<ApiBlockRecordFile> SharedFiles
        {
            get => sharedFiles;
            set
            {
                sharedFiles = value;
                RaisePropertyChanged(nameof(SharedFiles));
            }
        }

        public DirectoryViewModel(IBlockchainService blockchainService)
        {
            this.blockchainService = blockchainService;
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
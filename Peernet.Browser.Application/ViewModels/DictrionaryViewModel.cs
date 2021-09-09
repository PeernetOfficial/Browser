using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class DirectoryViewModel : MvxViewModel
    {
        private readonly IBlockchainService blockchainService;
        private ApiBlockchainAddFiles sharedFiles;

        public ApiBlockchainAddFiles SharedFiles
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
                SharedFiles = blockchainService.GetSelfList();
            }

            return base.Initialize();
        }
    }
}
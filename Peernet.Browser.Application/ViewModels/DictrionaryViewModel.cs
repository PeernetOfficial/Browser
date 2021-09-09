using MvvmCross.ViewModels;
using Peernet.Browser.Application.Services;

namespace Peernet.Browser.Application.ViewModels
{
    public class DirectoryViewModel : MvxViewModel
    {
        private readonly IBlockchainService blockchainService;

        public DirectoryViewModel(IBlockchainService blockchainService)
        {
            this.blockchainService = blockchainService;

            var header = blockchainService.GetSelfHeader();
        }
    }
}
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Services
{
    public interface IBlockchainService
    {
        ApiBlockchainHeader GetSelfHeader();

        ApiBlockchainAddFiles GetSelfList();

        void DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile);
    }
}
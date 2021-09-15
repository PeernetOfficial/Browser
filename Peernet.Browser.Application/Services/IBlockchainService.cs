using Peernet.Browser.Application.Models;
using System.Collections.Generic;

namespace Peernet.Browser.Application.Services
{
    public interface IBlockchainService
    {
        ApiBlockchainHeader GetSelfHeader();

        ApiBlockchainAddFiles GetSelfList();

        void AddFiles(IEnumerable<SharedNewFileModel> files);

        void DeleteSelfFile(ApiBlockRecordFile apiBlockRecordFile);
    }
}
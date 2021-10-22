﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.Services
{
    public interface IBlockchainService
    {
        Task AddFiles(IEnumerable<SharedNewFileModel> files);

        Task DeleteSelfFile(ApiFile apiFile);

        Task<ApiBlockchainHeader> GetSelfHeader();

        Task<List<ApiFile>> GetSelfList();
    }
}
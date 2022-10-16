using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Domain.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IStatusService
    {
        Task<ApiResponseStatus> GetStatus();

        Task<List<PeerStatus>> GetPeersStatus();
    }
}
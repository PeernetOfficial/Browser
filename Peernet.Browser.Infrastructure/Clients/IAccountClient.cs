using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Blockchain;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface IAccountClient
    {
        Task<ApiBlockchainBlockStatus> Delete(bool confirm);
    }
}
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Blockchain;

namespace Peernet.Browser.Application.Services
{
    public interface IAccountService
    {
        Task<ApiBlockchainBlockStatus> Delete(bool confirm);
    }
}
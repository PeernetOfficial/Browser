using System.Threading.Tasks;
using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Application.Facades
{
    public interface IApiFacade
    {
        Task<ApiResponseStatus> GetStatus();
    }
}

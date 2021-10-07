using System.Threading.Tasks;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Application.Facades
{
    public interface IApiFacade
    {
        Task<ApiResponseStatus> GetStatus();
    }
}

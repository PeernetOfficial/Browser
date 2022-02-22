using System.Threading.Tasks;
using Peernet.SDK.Models.Domain.Common;

namespace Peernet.Browser.Application.Services
{
    public interface IApiService
    {
        Task<ApiResponseStatus> GetStatus();
    }
}
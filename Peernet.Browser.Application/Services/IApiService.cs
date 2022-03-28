using Peernet.SDK.Models.Domain.Common;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IApiService
    {
        Task<ApiResponseStatus> GetStatus();
    }
}
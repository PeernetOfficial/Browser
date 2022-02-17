using Peernet.SDK.Models.Domain.Shutdown;

namespace Peernet.Browser.Application.Services
{
    public interface IShutdownService
    {
        ApiShutdownStatus Shutdown();
    }
}
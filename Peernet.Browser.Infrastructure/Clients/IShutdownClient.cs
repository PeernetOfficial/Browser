using Peernet.Browser.Models.Domain.Shutdown;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface IShutdownClient
    {
        ApiShutdownStatus GetAction(ShutdownAction action);
    }
}
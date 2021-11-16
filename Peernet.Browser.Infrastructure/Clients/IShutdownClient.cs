using Peernet.Browser.Models.Domain.Shutdown;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface IShutdownClient
    {
        ApiShutdownStatus GetAction(ShutdownAction action);
    }
}
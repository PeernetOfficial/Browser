using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Shutdown;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class ShutdownService : IShutdownService
    {
        private readonly IShutdownClient shutdownClient;

        public ShutdownService(IShutdownClient shutdownClient)
        {
            this.shutdownClient = shutdownClient;
        }

        public ApiShutdownStatus Shutdown()
        {
            return shutdownClient.GetAction(ShutdownAction.Shutdown);
        }
    }
}
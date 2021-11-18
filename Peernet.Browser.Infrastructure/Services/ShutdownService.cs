using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Shutdown;

namespace Peernet.Browser.Infrastructure.Services
{
    public class ShutdownService : IShutdownService
    {
        private readonly IShutdownClient shutdownClient;

        public ShutdownService(ISettingsManager settingsManager)
        {
            shutdownClient = new ShutdownClient(settingsManager);
        }

        public ApiShutdownStatus Shutdown()
        {
            return shutdownClient.GetAction(ShutdownAction.Shutdown);
        }
    }
}
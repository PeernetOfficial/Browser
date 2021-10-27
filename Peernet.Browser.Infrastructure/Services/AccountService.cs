using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountClient accountClient;

        public AccountService(ISettingsManager settingsManager)
        {
            accountClient = new AccountClient(settingsManager);
        }

        public async Task Delete(bool confirm)
        {
            await accountClient.Delete(confirm);
        }
    }
}
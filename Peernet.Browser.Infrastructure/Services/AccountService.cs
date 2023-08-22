using Peernet.Browser.Application.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.Account;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class AccountService : IAccountService
    {
        private readonly IAccountClient accountClient;

        public AccountService(IAccountClient accountClient)
        {
            this.accountClient = accountClient;
        }

        public async Task Delete(bool confirm)
        {
            await accountClient.Delete(confirm);
        }

        public async Task<ApiResponsePeerSelf> Info()
        {
            return await accountClient.Info();
        }
    }
}
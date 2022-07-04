using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Services;
using Peernet.SDK.Models.Presentation.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserContext userContext;
        private readonly IBlockchainService blockchainService;

        public UserService(IUserContext userContext, IBlockchainService blockchainService)
        {
            this.userContext = userContext;
            this.blockchainService = blockchainService;
        }

        public async Task<User> GetUser(byte[] nodeId)
        {
            var user = userContext.User;
            user.Files = (await blockchainService.GetList()).ToArray();

            return user;
        }
    }
}

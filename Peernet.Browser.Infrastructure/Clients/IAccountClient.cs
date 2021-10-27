using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface IAccountClient
    {
        Task Delete(bool confirm);
    }
}
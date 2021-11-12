using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IAccountService
    {
        Task Delete(bool confirm);
    }
}
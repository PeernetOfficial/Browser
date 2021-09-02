using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface ISocketClient
    {
        Task Connect();

        Task Send(string data);

        Task<string> Receive();
    }
}

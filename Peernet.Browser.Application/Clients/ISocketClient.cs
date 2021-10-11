using System.Threading.Tasks;

namespace Peernet.Browser.Application.Clients
{
    public interface ISocketClient
    {
        Task Connect();

        Task Send(string data);

        Task<string> Receive();
    }
}
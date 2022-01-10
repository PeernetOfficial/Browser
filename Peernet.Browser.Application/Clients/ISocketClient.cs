using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Clients
{
    public interface ISocketClient
    {
        Task Connect();

        Task Send(string data);

        Task StartReceiving();

        event EventHandler<string> MessageArrived;
    }
}
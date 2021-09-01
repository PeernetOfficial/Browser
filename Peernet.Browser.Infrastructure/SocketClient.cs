using MvvmCross.Plugin.FieldBinding;
using Peernet.Browser.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class SocketClient : ISocketClient
    {
        private ClientWebSocket socket;
        private readonly ISettingsManager settings;

        public SocketClient(ISettingsManager settings)
        {
            this.settings = settings;
        }

        public async Task Connect()
        {
            this.socket = new();
            await socket.ConnectAsync(new Uri(this.settings.SocketUrl), CancellationToken.None);
        }

        public async Task Send(string data)
        {
            await this.socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task<string> Receive()
        {
            // Read the bytes from the web socket and accumulate all into a list.
            var buffer = new ArraySegment<byte>(new byte[1024]);
            WebSocketReceiveResult result = null;
            var allBytes = new List<byte>();

            do
            {
                result = await this.socket.ReceiveAsync(buffer, CancellationToken.None);
                for (int i = 0; i < result.Count; i++)
                {
                    allBytes.Add(buffer.Array[i]);
                }
            }
            while (!result.EndOfMessage);

            // Optional step to convert to a string (UTF-8 encoding).
           return Encoding.UTF8.GetString(allBytes.ToArray(), 0, allBytes.Count);
        }
    }
}

using Peernet.Browser.Application.Clients;
using Peernet.Browser.Application.Managers;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class SocketClient : ISocketClient
    {
        private const int ReceiveBufferSize = 8192;
        private ClientWebSocket socket;
        private readonly ISettingsManager settingsManager;
        //private readonly ILogger<SocketClient> logger;
        public event EventHandler<string> MessageArrived;
        private CancellationTokenSource source;

        public SocketClient(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            //this.logger = logger;
        }

        public async Task Connect()
        {
            socket = new();
            await socket.ConnectAsync(settingsManager.SocketUrl, CancellationToken.None);
        }

        public async Task Send(string data)
        {
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task StartReceiving()
        {
            source = new CancellationTokenSource();
            var socketReceiverToken = source.Token;
            MemoryStream outputStream = null;
            var buffer = WebSocket.CreateClientBuffer(ReceiveBufferSize, ReceiveBufferSize);
            try
            {
                while (!socketReceiverToken.IsCancellationRequested)
                {
                    outputStream = new MemoryStream(ReceiveBufferSize);
                    WebSocketReceiveResult receiveResult;
                    do
                    {
                        receiveResult = await socket.ReceiveAsync(buffer, socketReceiverToken);
                        if (receiveResult.MessageType != WebSocketMessageType.Close)
                        {
                            outputStream.Write(buffer.Array, 0, receiveResult.Count);
                        }
                    }
                    while (!receiveResult.EndOfMessage);
                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    var message = Encoding.UTF8.GetString(outputStream.ToArray(), 0, (int)outputStream.Length);
                    MessageArrived?.Invoke(null, message);
                    outputStream.Position = 0;
                }

            }
            catch (TaskCanceledException e)
            {
                //logger.LogDebug(e, "Socket receiver aborted!");
            }
            finally
            {
                outputStream?.Dispose();
            }
        }

        public void Disconnect()
        {
            source.Cancel();
            source.Token.WaitHandle.WaitOne();
            socket.Dispose();
        }
    }
}
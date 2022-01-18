using System;
using System.Text;
using System.Threading.Tasks;
using Peernet.Browser.Application.ViewModels.Parameters;
using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Clients;

namespace Peernet.Browser.Application.ViewModels
{
    public class TerminalViewModel : GenericViewModelBase<TerminalInstanceParameter>, IDisposable
    {
        private TerminalInstanceParameter parameter;
        public event EventHandler OnOutputChanged;

        private readonly ISocketClient socketClient;

        public TerminalViewModel(ISocketClient socketClient)
        {
            this.socketClient = socketClient;
            Parameter = parameter;

            Initialize().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public IAsyncCommand SendToPeernetConsole => new AsyncCommand(async () =>
        {
            if (Parameter.CommandLineInput.Equals("cls", StringComparison.CurrentCultureIgnoreCase))
            {
                Parameter.CommandLineOutput = string.Empty;
            }
            else
            {
                await socketClient.Send(Parameter.CommandLineInput);
                SetOutput($"\n>> {Parameter.CommandLineInput}\n");
            }

            Parameter.CommandLineInput = string.Empty;
        });

        public async Task Initialize()
        {
            await ConnectToPeernetConsole();
            socketClient.MessageArrived += SocketClientOnMessageArrived;
            await socketClient.StartReceiving();
        }

        private void SocketClientOnMessageArrived(object? sender, string e)
        {
            SetOutput(e);
            OnOutputChanged?.Invoke(null, EventArgs.Empty);
        }

        private async Task ConnectToPeernetConsole()
        {
            await socketClient.Connect();
        }

        private void SetOutput(string output)
        {
            var mbSize = (decimal)Encoding.Unicode.GetByteCount(output) / 1048576;
            if (mbSize > 1)
            {
                Parameter.CommandLineOutput = output;
            }

            else
            {
                Parameter.CommandLineOutput += output;
            }
        }

        public override void Dispose()
        {
            socketClient.Disconnect();

            base.Dispose();
        }
    }
}
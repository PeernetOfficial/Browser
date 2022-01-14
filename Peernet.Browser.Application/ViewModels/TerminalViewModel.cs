using System;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Clients;
using System.Threading.Tasks;
using Peernet.Browser.Application.ViewModels.Parameters;

namespace Peernet.Browser.Application.ViewModels
{
    public class TerminalViewModel : ViewModelBase<TerminalInstanceParameter>
    {
        private TerminalInstanceParameter parameter;
        public event EventHandler OnOutputChanged;

        private readonly ISocketClient socketClient;

        public TerminalViewModel(ISocketClient socketClient)
        {
            this.socketClient = socketClient;
        }

        public TerminalInstanceParameter Parameter
        {
            get => parameter;
            private set
            {
                parameter = value;
                RaisePropertyChanged(nameof(Parameter));
            }
        }

        public IMvxAsyncCommand SendToPeernetConsole => new MvxAsyncCommand(async () =>
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

        public override void Prepare(TerminalInstanceParameter parameter)
        {
            Parameter = parameter;
            base.Prepare();
        }

        public override async Task Initialize()
        {
            await ConnectToPeernetConsole();
            socketClient.MessageArrived += SocketClientOnMessageArrived;
            await socketClient.StartReceiving();
            
            await base.Initialize();
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

        public override void ViewDestroy(bool viewFinishing = true)
        {
            socketClient.Disconnect();
            base.ViewDestroy(viewFinishing);
        }
    }
}
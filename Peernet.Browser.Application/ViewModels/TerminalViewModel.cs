using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Clients;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class TerminalViewModel : MvxViewModel
    {
        private string commandLineInput;
        private string commandLineOutput;

        private readonly ISocketClient socketClient;

        public TerminalViewModel(ISocketClient socketClient)
        {
            this.socketClient = socketClient;
        }

        public string CommandLineOutput
        {
            get => commandLineOutput;
            set
            {
                commandLineOutput = value;
                SetProperty(ref commandLineOutput, value);
                RaisePropertyChanged(nameof(CommandLineOutput));
            }
        }


        public string CommandLineInput
        {
            get => commandLineInput;
            set
            {
                commandLineInput = value;
                RaisePropertyChanged(nameof(CommandLineInput));
                SetProperty(ref commandLineInput, value);
            }
        }

        public IMvxAsyncCommand SendToPeernetConsole => new MvxAsyncCommand(async () =>
        {
            if (CommandLineInput.Equals("cls", StringComparison.CurrentCultureIgnoreCase))
            {
                CommandLineOutput = string.Empty;
            }
            else
            {
                await socketClient.Send(CommandLineInput);
                var response = await socketClient.Receive();
                SetOutput(response);
                CommandLineOutput += response;
            }

            CommandLineInput = string.Empty;
        });

        public override async void Start()
        {
            base.Start();
            
            await ConnectToPeernetConsole();
        }

        private async Task ConnectToPeernetConsole()
        {
            await socketClient.Connect();

            CommandLineOutput = await socketClient.Receive();
        }

        private void SetOutput(string output)
        {
            var mbSize = ((decimal)Encoding.Unicode.GetByteCount(output) / 1048576);
            if (mbSize > 1)
            {
                CommandLineOutput = output;
            }

            else
            {
                CommandLineOutput += output;
            }
        }
    }
}
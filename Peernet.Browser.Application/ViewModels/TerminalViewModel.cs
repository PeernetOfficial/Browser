using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Client.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class TerminalViewModel : GenericViewModelBase<TerminalInstanceParameter>, IDisposable
    {
        public event EventHandler OnOutputChanged;

        private readonly ISocketClient socketClient;

        private List<string> inputTrace;

        public TerminalViewModel(ISocketClient socketClient)
        {
            this.socketClient = socketClient;
            inputTrace = new List<string>();
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

             RegisterInputTrace(Parameter.CommandLineInput);

             Parameter.CommandLineInput = string.Empty;
         });

        private async Task Initialize()
        {
            await ConnectToPeernetConsole();
            socketClient.MessageArrived += SocketClientOnMessageArrived;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(async () => await socketClient.StartReceiving(Parameter.CancellationTokenSource).ConfigureAwait(false));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void SocketClientOnMessageArrived(object sender, string e)
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

        public override async Task Prepare(TerminalInstanceParameter parameter)
        {
            Parameter = parameter;
            await Initialize();
        }

        public IAsyncCommand GetPreviousInputCommand => new AsyncCommand(() =>
         {
             if (inputTrace.Count != 0)
             {
                 if (!string.IsNullOrEmpty(Parameter.CommandLineInput))
                 {
                     var currentPosition = inputTrace.IndexOf(Parameter.CommandLineInput);
                     var previousPosition = currentPosition - 1;
                     if (previousPosition >= 0)
                     {
                         Parameter.CommandLineInput = inputTrace.ElementAt(previousPosition);
                     }
                 }
                 else
                 {
                     Parameter.CommandLineInput = inputTrace.Last();
                 }
             }

             return Task.CompletedTask;
         });

        public IAsyncCommand GetNextInputCommand => new AsyncCommand(() =>
         {
             if (inputTrace.Count != 0)
             {
                 if (!string.IsNullOrEmpty(Parameter.CommandLineInput))
                 {
                     var currentPosition = inputTrace.IndexOf(Parameter.CommandLineInput);
                     var nextPosition = currentPosition + 1;
                     if (nextPosition <= inputTrace.Count - 1)
                     {
                         Parameter.CommandLineInput = inputTrace.ElementAt(nextPosition);
                     }
                 }
                 else
                 {
                     Parameter.CommandLineInput = inputTrace.First();
                 }
             }

             return Task.CompletedTask;
         });

        public IAsyncCommand ClearInputCommand => new AsyncCommand(() =>
         {
             Parameter.CommandLineInput = string.Empty;

             return Task.CompletedTask;
         });

        private void RegisterInputTrace(string input)
        {
            if (inputTrace.Contains(input))
            {
                inputTrace.Remove(input);
            }

            inputTrace.Add(input);
        }
    }
}